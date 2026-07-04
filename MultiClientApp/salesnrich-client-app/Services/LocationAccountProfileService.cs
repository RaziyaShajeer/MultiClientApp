using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Parsers;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
using SNR_ClientApp.Tally.generateXml;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    public class LocationAccountProfileService
    {

        public static readonly String FILE_NAME = "location-account-profile";
        readonly TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        private bool fullUpdate = true;
        private string idClentApp;
		AccountProfileXmlParser AccountProfileXmlParser;

		private String tallyLedgerParent;
        public LocationAccountProfileService()

        {

			AccountProfileXmlParser = new AccountProfileXmlParser();
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


                List<LocationAccountProfileDTO> allAccountProfilespTally = await AccountProfileXmlParser.LocationWiseAccounctProfileParser(data);
                var myContent = JsonConvert.SerializeObject(allAccountProfilespTally);
                LogManager.WriteLog("LocationWise AccountProfile");
                LogManager.WriteLog(myContent.ToString());
                List<LocationAccountProfileDTO> apTally = await getSundryDebtorsChilds(allAccountProfilespTally);
                List<LocationDTO> _list = new List<LocationDTO>();


                List<LocationAccountProfileDTO> apToServer = new List<LocationAccountProfileDTO>();
                //if (fileManagerService.fileExists(FILE_NAME))
                //{
                //    //					List<AccountProfileDTO> apFile = fileManagerService.readObjectFromFile(FILE_NAME,
                //							AccountProfileDTO.class);
                //			
                //					apToServer = findNewAndDeletedAccountProfiles(apTally, apFile);
                apToServer = apTally;
				myContent = JsonConvert.SerializeObject(apToServer);
				LogManager.WriteLog(myContent.ToString());
				//}
				//else
				//{
				//    fileManagerService.createApplicationDirectories();
				//    apToServer = apTally;
				//}


				if (apToServer.Count > 0)
                {
                    upload(apToServer);
					LogManager.WriteLog("GroupWise Account Upload" + apToServer.Count);
				}

            }

            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
        }
		

        private void upload(List<LocationAccountProfileDTO> apToServer)
        {
    
             string   requestUri = ApiConstants.PREFIX + ApiConstants.LOCATION_ACCOUNT_PROFILE;

			LogManager.WriteLog("uploading LOCATION_ACCOUNT_PROFILE started...");
            httpClient = RestClientUtil.getClient();
            var myContent = JsonConvert.SerializeObject(apToServer);
            HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

            var responseTask = httpClient.PostAsync(requestUri + "?fullUpdate=" + fullUpdate, inputContent);

            responseTask.Wait();

            HttpResponseMessage Res = responseTask.Result;
            LogManager.WriteResponseLog(Res);

            if (Res.IsSuccessStatusCode)
            {
                LogManager.WriteLog("request for uploading LOCATION_ACCOUNT_PROFILE  Success..");
                var response = Res.Content.ReadAsStringAsync().Result;
            }
            else
            {
                LogManager.WriteLog("request for uploading LOCATION_ACCOUNT_PROFILE  Failed..");
            }

        }

        private async Task< List<LocationAccountProfileDTO>> getSundryDebtorsChilds(List<LocationAccountProfileDTO> allAccountProfilespTally)
        {
            List<LocationAccountProfileDTO> filteredLedgers = new List<LocationAccountProfileDTO>();

            try
            {
                GroupsService gs = new GroupsService();


               
				List<LocationDTO> allGroups = await gs.getCompanyAccountGroups();
                LogManager.WriteLog("AllGroups");
				var myContent = JsonConvert.SerializeObject(allGroups);
				LogManager.WriteLog(myContent.ToString());
				LogManager.WriteLog("Filtered Groups");
				List<LocationDTO> filteredGroups = gs.accountGroupsFilter(allGroups);
				 myContent = JsonConvert.SerializeObject(filteredGroups);
				LogManager.WriteLog(myContent.ToString());

				List<LocationAccountProfileDTO> sundryChild = new List<LocationAccountProfileDTO>();
                foreach (LocationAccountProfileDTO ledger in allAccountProfilespTally)
                {

                    if (ledger.locationName.ToUpper().Equals(tallyLedgerParent.ToUpper()))
                    {
                        sundryChild.Add(ledger);
                    }
                }

                foreach (LocationAccountProfileDTO ledger in allAccountProfilespTally)
                {
                   
                    foreach (LocationDTO accountGroup in filteredGroups)

                    {
					
						if (accountGroup.name.Equals(ledger.locationName, StringComparison.OrdinalIgnoreCase))
                        {
                            sundryChild.Add(ledger);
                        }
                        //else
                        //{
                        //    LogManager.WriteLog(accountGroup.name + ":" + ledger.locationName);
                        //}
                    }
                }
                filteredLedgers.AddRange(sundryChild);



            }
            catch (Exception e)
            {
                LogManager.HandleException(e);
            }
            return filteredLedgers;

        }

        private long getAlterId()
        {

            LogManager.WriteLog("get alterId in LocationAccountProfileService started...");
            httpClient = RestClientUtil.getClient();
            var responseTask = httpClient.GetAsync(httpClient.BaseAddress +ApiConstants.PREFIX+ ApiConstants.ALTERID_MASTER + "/" + TallyMasters.LOCATION_ACCOUNT_PROFILE);

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
