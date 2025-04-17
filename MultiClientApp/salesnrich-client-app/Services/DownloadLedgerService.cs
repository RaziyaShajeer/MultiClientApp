using dtos;
using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
using SNR_ClientApp.Tally.generateXml;
using SNR_ClientApp.Utils;
using SNR_ClientApp.Windows.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    public class DownloadLedgerService
    {
        readonly TallyCommunicator tallyCommunicator;
        HttpClient httpClient= new HttpClient();
        //private bool fullUpdate = true;
        //private string idClentApp;
        //private String tallyLedgerParent;
        public DownloadLedgerService()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = RestClientUtil.getClient();
            //idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
            //tallyLedgerParent = ApplicationProperties.properties.GetValueOrDefault("tally.ledger.parent").ToString();
        }
        internal async Task<List<AccountProfileDTO>> SendAllLedgerToTally()
        {
			List<String> succesOrders = new();
			try
            {
                String updateAccountStatus =  ApiConstants.UPDATE_LEDGER_STATUS;
                List<AccountProfileDTO> accountProfileDTOs = new List<AccountProfileDTO>();
                accountProfileDTOs = getLedgersFromServer();
                if (accountProfileDTOs.Count > 0)
                {
                    List<String> succesAccountpids = new();
                    List<TallyXml> tallyXmls = new();
                    LedgerGenerateXml ledgerGenerateXml = new LedgerGenerateXml();
                    tallyXmls = ledgerGenerateXml.generateLedgerXml(accountProfileDTOs);
                    TransactionProcessor transactionProcessor = new TransactionProcessor();
                    TallyResponse res = await transactionProcessor.postOrdersToTally(tallyXmls);
					DownloadResponseDto resp = res.body as DownloadResponseDto;
					succesOrders=resp.SuccessOrders;
					//List<string> succesOrders = res.body as List<string>;
					if (succesOrders.Count > 0)
                    {
                        HttpContent content = new StringContent(JsonConvert.SerializeObject(succesOrders), Encoding.UTF8, "application/json");
                        HttpResponseMessage updateResult = httpClient.PostAsync(updateAccountStatus, content).Result;
                    }
					if (resp.FailedOrders.Count > 0)
					{
						string updatesalesOrderFailedStatus = ApiConstants.UPDATE_ORDER_STATUS_PENDING;
						HttpContent content2 = new StringContent(JsonConvert.SerializeObject(resp.FailedOrders), Encoding.UTF8, "application/json");
						HttpResponseMessage updateResult = httpClient.PostAsync(updatesalesOrderFailedStatus, content2).Result;
					}
					if (resp.isLedgerMissmatch)
					{
						string joinedString = string.Join(" \n", resp.failedOrdersLineErrors);
						UC_Download.showMessageToMasterUpdate(resp.FailedOrders.Count+" Order Creation Failed \n Error : "+joinedString);
					}
				}
                return accountProfileDTOs;
            }
            catch(Exception ex)
            {
                LogManager.HandleException(ex);
                throw new ServiceException(ex.Message);
            }
            
        }

        public List<AccountProfileDTO> getLedgersFromServer()
        {
            try
            {
                List<AccountProfileDTO> responseDtos = new();
                string serverAddress = ApiConstants.DOWNLOAD_LEDGER;


                LogManager.WriteLog("downloading ledgers from server....");
                httpClient = RestClientUtil.getClient();


                var responseTask = httpClient.GetAsync(serverAddress);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteResponseLog(Res);

                if (Res.IsSuccessStatusCode)
                {
                                                                                                                                 LogManager.WriteLog("request for downloading ledger    Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                    responseDtos = JsonConvert.DeserializeObject<List<AccountProfileDTO>>(response);
                    return responseDtos;
                }
                else
                {
                    LogManager.WriteLog("request for downloading ledger Failed..");
                    throw new ServiceException("downloading ledger Failed statuscode:" + Res.StatusCode + " Message : " + Res.RequestMessage);

                    return responseDtos;
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog("request for downloading ledger Failed...\n Exception details : " + ex.Message);
                LogManager.HandleException(ex);
                throw ex;
            }
        }
    }
}
