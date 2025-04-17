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
        private String tallyLedgerParent;
        public LocationAccountProfileService()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
            tallyLedgerParent = ApplicationProperties.properties.GetValueOrDefault("tally.ledger.parent").ToString();
        }
        internal async  void getFromTallyAndUpload(bool isOptimise)
        {
            try { 

            List<LocationDTO> _list = new List<LocationDTO>();
            DataTable response = new DataTable();
            StringBuilder Query = new StringBuilder();
            Query.Append("select $guid,$name,$alterid,$Parent,$MailingName, $_Address1,$_Address2,$PriceLevel,$TaxType,$LedgerMobile," +
                "$LedStateName,$CountryofResidence,$GSTRegistrationType,$PartyGSTIN,$PinCode,$SubTaxType from " + Tables.Ledger);
            if (isOptimise)
            {
                long alterID = getAlterId();

                Query.Append(" where $Alterid >" + alterID);
                fullUpdate = false;
            }
            response = await tallyCommunicator.getdatatable(Query.ToString());

            if (response.Rows.Count > 0)
            {
                List<LocationAccountProfileDTO> allAccountProfilespTally = new List<LocationAccountProfileDTO>();

                foreach (DataRow dr in response.Rows)
                {
                    LocationAccountProfileDTO ladto = new LocationAccountProfileDTO();
                    ladto.locationName = (dr["$Parent"] != DBNull.Value) ? (string)dr["$Parent"] : "";
                    ladto.accountProfileName = (dr["$name"] != DBNull.Value) ? (string)dr["$name"] : "";
                    ladto.alterId = (dr["$alterid"] != DBNull.Value) ? (long.Parse(dr["$alterid"].ToString())) : 0;
                    //Customer Id Added 
                    ladto.customer_id = (dr["$guid"].ToString()); 
                    allAccountProfilespTally.Add(ladto);

                }

                List<LocationAccountProfileDTO> apTally = await getSundryDebtorsChilds(allAccountProfilespTally);
                List<LocationAccountProfileDTO> apToServer = new List<LocationAccountProfileDTO>();
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


                if (apToServer.Count > 0)
                {
                    upload(apToServer);
                }

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
            string requestUri = ApiConstants.PREFIX + ApiConstants.LOCATION_ACCOUNT_PROFILE;

            if (idClentApp.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                requestUri = ApiConstants.PREFIX + ApiConstants.LOCATION_ACCOUNT_PROFILE_ID;
            }
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

                List<LocationDTO> filteredGroups = gs.accountGroupsFilter(allGroups);

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
