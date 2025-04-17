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
        public AccountProfileService()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
            tallyLedgerParent = ApplicationProperties.properties.GetValueOrDefault("tally.ledger.parent").ToString();
        }
        internal async void getFromTallyAndUpload(bool isOptimise)
        {
            try {
                List<AccountProfileDTO> duplicatelist = new List<AccountProfileDTO>();
                List<LocationDTO> _list = new List<LocationDTO>();
            DataTable response = new DataTable();
            StringBuilder Query = new StringBuilder();
            Query.Append("select $_Address1,$_Address2,$PriceLevel,$Parent,$TaxType,$LedgerMobile,$LedStateName,$CountryofResidence,$Name," +
                "$MailingName,$GSTRegistrationType,$PartyGSTIN,$PinCode,$Guid,$Alterid,$HaseDDCity from " + Tables.Ledger);
            if (isOptimise)
            {
                long alterID = getAlterId();

                Query.Append(" where $Alterid >" + alterID);
                fullUpdate = false;
            }
           
      
            response = await tallyCommunicator.getdatatable(Query.ToString());

            if (response.Rows.Count > 0)
            {
                List<AccountProfileDTO> allAccountProfilespTally = new List<AccountProfileDTO>();

                foreach (DataRow dr in response.Rows)
                {
                    AccountProfileDTO accountProfileDTO = new AccountProfileDTO();
                    accountProfileDTO.customerId = ((string)dr["$guid"]);
                    accountProfileDTO.name = (dr["$name"] != DBNull.Value) ? (string)dr["$name"] : "";

                        //testing code 28/09/2023
                        //  accountProfileDTO.name=accountProfileDTO.name.Trim();
                       

                        // Use Regex.Replace to remove the escape sequences at the end of the string
                       
                        if (accountProfileDTO.name.EndsWith("\r\n"))
                        {
                            accountProfileDTO.name = accountProfileDTO.name.Replace("\r\n", "");
                            accountProfileDTO.trimChar = "#13;#10;";
                        }

                        // 13/10/2023
                        // todo : need to verify with download order
                        string pattern = @"[\r\n\t]+$";
                        string result = Regex.Replace(accountProfileDTO.name, pattern, "");

                        accountProfileDTO.name=result;


                        accountProfileDTO.mailingName = (dr["$MailingName"] != DBNull.Value) ? (string)dr["$MailingName"] : "";
                        if (accountProfileDTO.mailingName.EndsWith("\r\n"))
                        {
                            accountProfileDTO.mailingName = accountProfileDTO.mailingName.Replace("\r\n", "");
                            //accountProfileDTO.trimChar = "#13;#10;";
                        }
                        //string[] endingChars = { "#13;", "#10;", "\t" }; // Add any other characters as needed
                        //string name = accountProfileDTO.name;
                        //string trimChar = "";
                        //foreach (string endingChar in endingChars)
                        //{
                        //    if (name.EndsWith(endingChar))
                        //    {
                        //        trimChar = endingChar;
                        //        name = name.Substring(0, name.Length - endingChar.Length);
                        //        break; // Exit the loop once we find a matching ending character
                        //    }
                        //}

                        //accountProfileDTO.name= accountProfileDTO.name.Replace("\r\n", "");


                        accountProfileDTO.description = (dr["$parent"] != DBNull.Value) ? (string)dr["$parent"] : "";

                    accountProfileDTO.alterId = (dr["$alterid"] != DBNull.Value) ? (long.Parse(dr["$alterid"].ToString())) : 0;
                    accountProfileDTO.address = (dr["$_Address1"] != DBNull.Value && (dr["$_Address1"]!="")) ? (string)dr["$_Address1"] : "No Address";
                     accountProfileDTO.address += (dr["$_Address2"] != DBNull.Value  && (dr["$_Address2"]!="")) ?"~"+ (string)dr["$_Address2"] : "";
                    accountProfileDTO.defaultPriceLevelName = (dr["$PriceLevel"] != DBNull.Value) ? (string)dr["$PriceLevel"] : "";
                    accountProfileDTO.accountTypeName = (dr["$TaxType"] != DBNull.Value) ? (string)dr["$TaxType"] : "";

                    accountProfileDTO.phone1 = (dr["$LedgerMobile"] != DBNull.Value) ? (string)dr["$LedgerMobile"] : "";
                      var phone=  accountProfileDTO.phone1.Split(",");
                        accountProfileDTO.phone1=phone[0];
                    accountProfileDTO.stateName = (dr["$LedStateName"] != DBNull.Value) ? (string)dr["$LedStateName"] : "";
                    accountProfileDTO.countryName = (dr["$CountryofResidence"] != DBNull.Value) ? (string)dr["$CountryofResidence"] : "";
               
                    accountProfileDTO.gstRegistrationType = (dr["$GSTRegistrationType"] != DBNull.Value) ? (string)dr["$GSTRegistrationType"] : "";
                    accountProfileDTO.tinNo = (dr["$PartyGSTIN"] != DBNull.Value) ? (string)dr["$PartyGSTIN"] : "";
                    accountProfileDTO.pin = (dr["$PinCode"] != DBNull.Value) ? (string)dr["$PinCode"] : "";
                    accountProfileDTO.city = (dr["$HaseDDCity"] != DBNull.Value && dr["$HaseDDCity"]!="" ) ? (string)dr["$HaseDDCity"] : "No City";

                        //todo : check this is working
                        if (allAccountProfilespTally.Where(x => x.name==accountProfileDTO.name).Count() == 0)
                        {
                            allAccountProfilespTally.Add(accountProfileDTO);
                        }
                        else
                        {
                            duplicatelist.Add(accountProfileDTO);
                        }
                        //allAccountProfilespTally.Add(accountProfileDTO);

                }

                List<AccountProfileDTO> apTally = await getSundryDebtorsChilds(allAccountProfilespTally);
                List<AccountProfileDTO> apToServer = new List<AccountProfileDTO>();
                //if (fileManagerService.fileExists(FILE_NAME))
                //{
                //    //					List<AccountProfileDTO> apFile = fileManagerService.readObjectFromFile(FILE_NAME,
                    //							AccountProfileDTO.class);
                    //					apToServer = findNewAndDeletedAccountProfiles(apTally, apFile);
                    apToServer = apTally;
                //}
                //else
                //{
                //    fileManagerService.createApplicationDirectories();
                //    apToServer = apTally;
                //}

               
                if (apToServer.Count>0)
                {
                    upload(apToServer);
						var myContent = JsonConvert.SerializeObject(apToServer);
						LogManager.WriteLog(myContent.ToString());

					}
                    if (duplicatelist.Count>0)
                    {
                        LogManager.WriteLog("Duplicate AccountProfiles are : \n");
                        duplicatelist.ForEach(x =>
                        {
                            LogManager.WriteLog("==>"+x.name+"\n");
                        });
                    }
                    //fileManagerService.writeObjectToFile(apTally, FILE_NAME);
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

           

            string requestUri = ApiConstants.PREFIX + ApiConstants.ACCOUNT_PROFILE; //SNR_CLIENT_APP_AP_1


			if (idClentApp.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                requestUri = ApiConstants.PREFIX + ApiConstants.ACCOUNT_PROFILE_ID;//SNR_CLIENT_APP_AP_2
			}
            LogManager.WriteLog("uploading ACCOUNT_PROFILE started...");
            httpClient = RestClientUtil.getClient();
            var myContent = JsonConvert.SerializeObject(list);
            LogManager.WriteLog("Request url : "+requestUri);
            LogManager.WriteLog("Request body : " + myContent);
            HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

            var responseTask = httpClient.PostAsync(requestUri  +"/"+ fullUpdate, inputContent);

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
                                //							System.out.println("123"+sundryChild.size());
                            }
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
            try
            {
                List<LocationDTO> _list = new List<LocationDTO>();
                DataTable response = new DataTable();
                StringBuilder Query = new StringBuilder();
                Query.Append("select $guid,$name,$alterid,$Parent from " + Tables.Groups);

                response = await tallyCommunicator.getdatatable(Query.ToString());

                if (response.Rows.Count > 0)
                {


                    foreach (DataRow dr in response.Rows)
                    {
                        LocationDTO locationDTO = new LocationDTO();
                        locationDTO.locationId = ((string)dr["$guid"]);
                        locationDTO.name = (dr["$name"] != DBNull.Value) ? (string)dr["$name"] : "";
                        locationDTO.description = (dr["$parent"] != DBNull.Value) ? (string)dr["$parent"] : "";

                        locationDTO.alterId = (dr["$alterid"] != DBNull.Value) ? (long.Parse(dr["$alterid"].ToString())) : 0;

                        _list.Add(locationDTO);

                    }

                    // List<LocationDTO> filteredGroups = accountGroupsFilter(_list);
                }
                return _list;
            }catch(Exception ex)
            {
                LogManager.HandleException(ex, "Exception occured while getCompanyAccountGroups in AccountProfileServices  ");
                throw ex;
            }

        }

        private List<LocationDTO> accountGroupsFilter(List<LocationDTO> allGroups)
        {
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
