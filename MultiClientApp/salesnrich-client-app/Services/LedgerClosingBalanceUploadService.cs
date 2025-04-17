using Microsoft.VisualBasic.Logging;
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
    internal class LedgerClosingBalanceUploadService
    {
        readonly TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        private bool fullUpdate = true;
        private string idClentApp;
        private String tallyLedgerParent;

        public LedgerClosingBalanceUploadService()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
            tallyLedgerParent = ApplicationProperties.properties.GetValueOrDefault("tally.ledger.parent").ToString();
        }
        internal async void getFromTallyAndUpload()
        {
            try
            {
                List<LocationDTO> _list = new List<LocationDTO>();
                DataTable response = new DataTable();
                StringBuilder Query = new StringBuilder();
                Query.Append("select $guid,$Name,$ClosingBalance from " + Tables.Ledger);

                response = await tallyCommunicator.getdatatable(Query.ToString());

                if (response.Rows.Count > 0)
                {
                    List<AccountProfileDTO> pgToServer = new List<AccountProfileDTO>();

                    foreach (DataRow dr in response.Rows)
                    {
                        AccountProfileDTO accountProfileDTO = new AccountProfileDTO();
                        accountProfileDTO.customerId = ((string)dr["$guid"]);
                        accountProfileDTO.name = (dr["$name"] != DBNull.Value) ? (string)dr["$name"] : "";
                        if (dr["$ClosingBalance"] != DBNull.Value)
                        {
                            accountProfileDTO.closingBalance = StringUtilsCustom.ExtractDoubleValue(dr["$ClosingBalance"].ToString()) * -1;
                            pgToServer.Add(accountProfileDTO);
                        }

                    }

                    if (pgToServer.Count() > 0)
                    {
                        upload(pgToServer);
                    }
                }
            }catch(Exception ex)
            {
				LogManager.HandleException(ex);
				throw ex;
			}
        }

        private void upload(List<AccountProfileDTO> pgToServer)
        {
            try
            {
                string requestUri = ApiConstants.PREFIX + ApiConstants.ACCOUNT_PROFILE_CLOSING_BALANCE;

                if (idClentApp.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    requestUri = ApiConstants.PREFIX + ApiConstants.ACCOUNT_PROFILE_CLOSING_BALANCE_ID;
                }
                LogManager.WriteLog("Uploading AccountProfileDTO ClosingBalance data to server....");
                LogManager.WriteLog("Calling server with url : " + requestUri);
                httpClient = RestClientUtil.getClient();
                var myContent = JsonConvert.SerializeObject(pgToServer);
                HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

                var responseTask = httpClient.PostAsync(requestUri + "?fullUpdate=" + fullUpdate, inputContent);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteResponseLog(Res);

                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("request for uploading ACCOUNT_PROFILE_CLOSING_BALANCE  Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    LogManager.WriteLog("request for uploading ACCOUNT_PROFILE_CLOSING_BALANCE  Failed..");
                }
            }catch(Exception exception)
            {
                LogManager.WriteLog("ERROR : " + exception.Message);
               
                    throw new ServiceException(exception.Message);
               
            }
        }
    }
}
