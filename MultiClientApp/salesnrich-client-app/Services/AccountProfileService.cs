using dtos;
using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.Json;
using SNR_ClientApp.Tally.generateXml;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Parsers;
using System.Collections;

namespace SNR_ClientApp.Services
{
    internal class AccountProfileService
    {
        public static readonly String FILE_NAME = "account-profile";

        readonly TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        private bool fullUpdate = true;
        private string idClentApp;
        private String tallyLedgerParent;
        AccountProfileXmlParser AccountProfileXmlParser;
        public AccountProfileService()
        {
			AccountProfileXmlParser=new AccountProfileXmlParser();

			tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
            tallyLedgerParent = ApplicationProperties.properties.GetValueOrDefault("tally.ledger.parent").ToString();
        }
        internal async void getFromTallyAndUpload(bool isOptimise)
        
        {
            try
            {
                ENVELOPE tallyRequest = new ENVELOPE();

                tallyRequest = AccountProfileXml.GenerateAccountProfileXml();


                var stringwriter = new System.IO.StringWriter();
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyRequest.GetType());
                x.Serialize(stringwriter, tallyRequest);

                var data = await tallyCommunicator.ExecXmlAndGetXmlAsync(stringwriter.ToString());


                List<AccountProfileDTO> allAccountProfilespTally = await AccountProfileXmlParser.AccountProfilexmlParser(data);
                LogManager.WriteLog("AccountProfile");
                var myContent = JsonConvert.SerializeObject(allAccountProfilespTally);
				LogManager.WriteLog(myContent.ToString());
				List<AccountProfileDTO> apTally = await getSundryDebtorsChilds(allAccountProfilespTally);
                List<AccountProfileDTO> apToServer = new List<AccountProfileDTO>();


                apToServer = apTally;
				if (apToServer.Count > 0)
				{
					upload(apToServer);
					 myContent = JsonConvert.SerializeObject(apToServer);
					LogManager.WriteLog(myContent.ToString());


				}

			}
			catch (Exception ex)
			{
				LogManager.HandleException(ex);
				throw ex;
			}
			
		}

        private void upload(List<AccountProfileDTO> list)
        {
            LogManager.WriteLog("companylists");

           

            string  requestUri = ApiConstants.PREFIX + ApiConstants.ACCOUNT_PROFILE_ID;//SNR_CLIENT_APP_AP_2
		
            LogManager.WriteLog("uploading ACCOUNT_PROFILE started...");
            httpClient = RestClientUtil.getClient();
            var myContent = JsonConvert.SerializeObject(list);
            LogManager.WriteLog("Request url : "+requestUri);
            LogManager.WriteLog("Request body : " + myContent);
            HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

            var responseTask = httpClient.PostAsync(requestUri , inputContent);

            responseTask.Wait();

            HttpResponseMessage Res = responseTask.Result;
            LogManager.WriteResponseLog(Res);

             
            if (Res.IsSuccessStatusCode)
            {
                LogManager.WriteLog("request for uploading ACCOUNT_PROFILE  Success..");
                var response = Res.Content.ReadAsStringAsync().Result;
            }
            else
            {
                LogManager.WriteLog("request for uploading ACCOUNT_PROFILE  Failed..");
                throw new ServiceException("ACCOUNT_PROFILE upload failed statuscode:" + Res.StatusCode + " Message : " + Res.RequestMessage);

            }

        }

        private async Task< List<AccountProfileDTO>> getSundryDebtorsChilds(List<AccountProfileDTO> allLedgers)
        {
            List<AccountProfileDTO> filteredLedgers = new List<AccountProfileDTO>();
            try
            {


                List<LocationDTO> allGroups = await getCompanyAccountGroups();

                List<LocationDTO> filteredGroups = accountGroupsFilter(allGroups);
                LogManager.WriteLog("fILTERED GROUPS");
				var myContent = JsonConvert.SerializeObject(filteredGroups);
				LogManager.WriteLog(myContent.ToString());
				List<AccountProfileDTO> sundryChild = new List<AccountProfileDTO>();

                String[] tallyLedgers = tallyLedgerParent.Split(",");

                foreach (String tallyLedger in tallyLedgers)
                {
                    foreach (AccountProfileDTO ledger in allLedgers)
                    {
             
                        if (ledger.description.Equals(tallyLedger, StringComparison.OrdinalIgnoreCase))
                        {
                            ledger.tallyLedgerType=tallyLedger;
                            sundryChild.Add(ledger);
                            
                        }
                    }
                }
                int flag = 0;
			
				foreach (String tallyLedger in tallyLedgers)
                {
                    foreach (AccountProfileDTO ledger in allLedgers)
                    {
                        foreach (LocationDTO accountGroup in filteredGroups)
                        {
                            //						System.out.println(ledger.getDescription()+"==="+tallyLedger);
                            if (accountGroup.name.Equals(ledger.description, StringComparison.OrdinalIgnoreCase))
                            {
                        
                                ledger.tallyLedgerType=tallyLedger;
                                sundryChild.Add(ledger);
                                flag = 1;
                                break;
                                //							System.out.println("123"+sundryChild.size());
                            }
                         
							

						}
                        if(flag==0)

                        {
							LogManager.WriteLog("*****");
							LogManager.WriteLog(ledger.description);
                        }

						
					}
                }
			

				filteredLedgers.AddRange(sundryChild);
               
            }
            catch (Exception e)
            {
                LogManager.HandleException(e);
                throw e;
            }
            return filteredLedgers;

        }
		private async Task<List<LocationDTO>> getCompanyAccountGroups()
		{
			ENVELOPE tallyRequest = new ENVELOPE();

			tallyRequest = CompanygroupGenerateXml.getCompanyGroupsXml();


			var stringwriter = new System.IO.StringWriter();
			System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyRequest.GetType());
			x.Serialize(stringwriter, tallyRequest);

			var data = await tallyCommunicator.ExecXmlAndGetXmlAsync(stringwriter.ToString());
			List<LocationDTO> _list = new List<LocationDTO>();
			_list = AccountGroupResponseParser.CompanyGroupresponseParser(data);

			var myContent = JsonConvert.SerializeObject(_list);
			LogManager.WriteLog(myContent.ToString());
            return _list;
		}
		

        private List<LocationDTO> accountGroupsFilter(List<LocationDTO> allGroups)
        {
            LogManager.WriteLog("****aLLGROUPS");
			var myContent = JsonConvert.SerializeObject(allGroups);
			LogManager.WriteLog(myContent.ToString());
			List<LocationDTO> sundryChild = new List<LocationDTO>();
            List<LocationDTO> filteredGroups = new List<LocationDTO>();
            String[] tallyLedgers = tallyLedgerParent.Split(",");

            foreach (String tallyLedger in tallyLedgers)
            {
                foreach (LocationDTO stockGroup in allGroups)
                {
                    if (stockGroup.description.Equals(tallyLedger, StringComparison.OrdinalIgnoreCase))
                    {
                        sundryChild.Add(stockGroup);
                    }
                }
            }
            foreach (LocationDTO sg in sundryChild)
            {
                filteredGroups.Add(sg);
            }
            List<LocationDTO> filteredAllGroups = fileList(sundryChild, filteredGroups, allGroups);
			LogManager.WriteLog("Filtered Accounts");
			myContent = JsonConvert.SerializeObject(filteredAllGroups);

			LogManager.WriteLog(myContent.ToString());
			return filteredAllGroups;
        }

        private List<LocationDTO> fileList(List<LocationDTO> sundryChild, List<LocationDTO> filteredGroups,
          List<LocationDTO> allGroups)
        {
            List<LocationDTO> sundry = new List<LocationDTO>();
            for (int i = 0; i < sundryChild.Count(); i++)
            {
                foreach (LocationDTO stockGroup in allGroups)
                {
                    if (sundryChild[i].name.Equals(stockGroup.description, StringComparison.OrdinalIgnoreCase))
                    {
                        sundry.Add(stockGroup);
                    }
                }
            }
            foreach (LocationDTO stockGroup in sundry)
            {
                filteredGroups.Add(stockGroup);
            }
            if (sundry.Count > 0)
            {
                fileList(sundry, filteredGroups, allGroups);
            }
            return filteredGroups;
        }

        private long getAlterId()
        {

            LogManager.WriteLog("get alterId in AccountProfileService started...");
            httpClient = RestClientUtil.getClient();
            var responseTask = httpClient.GetAsync(httpClient.BaseAddress + ApiConstants.PREFIX+ ApiConstants.ALTERID_MASTER + "/" + TallyMasters.ACCOUNT_PROFILE);

            responseTask.Wait();

            HttpResponseMessage Res = responseTask.Result;
            LogManager.WriteResponseLog(Res);
            if (Res.IsSuccessStatusCode)
            {
                LogManager.WriteLog("request for alterId  Success..");
                var response = Res.Content.ReadAsStringAsync().Result;
                long responseAlterID = JsonConvert.DeserializeObject<long>(response);
                return responseAlterID;
            }
            throw new ServiceException("get alterId api call failed \n StatusCode : " + Res.StatusCode + " \n Response : " + Res.Content);

        }
    }
}
