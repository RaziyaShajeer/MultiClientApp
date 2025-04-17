using dtos;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Tally.generateXml;
using SNR_ClientApp.Tally;
using SNR_ClientApp.Utils;
using SNR_ClientApp.Windows.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNR_ClientApp.Exceptions;
using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.Properties;
using System.Net.Http;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.DTO;

namespace SNR_ClientApp.Services
{
    public class DownloadSalesReturnService
    {
        private String enableDateWise = ApplicationProperties.properties["enable.date"].ToString();
        HttpClient _httpClient;
        
        TallyCommunicator tallyCommunicator;
        TransactionProcessor transactionProcessor = new();
        public DownloadSalesReturnService()
        {
            _httpClient=new HttpClient();
            _httpClient = RestClientUtil.getClient();
          
            tallyCommunicator = new TallyCommunicator();    

        }
        public async Task getFromServerAndDownloadToTallyAsync(VoucherType voucherType, DateTime? salesDate = null, UC_Logger? uC_Logger=null)
        {
            try
            {
                List<SalesOrderDTO> salesOrderDTOs = new();
                int batchCount = 0;
                int totalSuccessCount = 0;
                int totalFailureCount = 0;
                List<TallyXml> tallyXmls = new List<TallyXml>();
                SalesReturnGenerateXML salesreturngenerateXML = new();
            
                do
                {
                    uC_Logger.AppendLogMsg("Please Wait.. Processing Transactions.. ");
                    List<String> succesOrders = new();

                    LogManager.WriteLog("downloading inventory data from server with date.");
                   
                    salesOrderDTOs = getSalesReturnFromServer(voucherType, salesDate);
                    if (salesOrderDTOs.Count > 0)
                    {
                        batchCount++;
                        uC_Logger.AppendLogMsg("Batch-"+batchCount+"Downloading");
                        foreach (SalesOrderDTO salesOrderDTO in salesOrderDTOs)
                        {

                            if ("true".Equals(enableDateWise, StringComparison.OrdinalIgnoreCase))
                            {
                                if (salesDate != null)
                                {

                                    String formattedDate = salesDate.Value.ToString("yyyy-MM-dd");
                                    salesOrderDTO.date = (formattedDate);
                                }
                            }

                            ENVELOPE salesOrderXml = await salesreturngenerateXML.generateSalesOrderXml(salesOrderDTO);
                            tallyXmls.Add(new TallyXml(salesOrderDTO.inventoryVoucherHeaderPid, salesOrderXml));
                        }
                        var res = await transactionProcessor.postOrdersToTally(tallyXmls);
                        DownloadResponseDto resp = res.body as DownloadResponseDto;
                        succesOrders = resp.SuccessOrders;
                        //succesOrders = (List<String>)res.body;
                        if (succesOrders.Count > 0)
                        {
                            totalSuccessCount = totalSuccessCount + succesOrders.Count;
                            updatesalesOrderStatus(succesOrders, uC_Logger);
                            LogManager.WriteLog(+succesOrders.Count + " Sales return downloaded");

                        }
                        if (resp.FailedOrders.Count > 0)
                        {
                            totalFailureCount = totalFailureCount + resp.FailedOrders.Count;
                            LogManager.WriteLog(+resp.FailedOrders.Count + " Sales Return failed to downloaded");
                            string updatesalesOrderFailedStatus = ApiConstants.UPDATE_ORDER_STATUS_PENDING;
                            HttpContent content2 = new StringContent(JsonConvert.SerializeObject(resp.FailedOrders), Encoding.UTF8, "application/json");
                            HttpResponseMessage updateResult = _httpClient.PostAsync(updatesalesOrderFailedStatus, content2).Result;

                            if (resp.failedOrdersLineErrors != null && resp.failedOrdersLineErrors.Count > 0 && !resp.isLedgerMissmatch)
                            {
                                string joinedString = string.Join(" \n", resp.failedOrdersLineErrors);
                                LogManager.WriteLog(resp.FailedOrders.Count + " Sales return Creation Failed \n Error : " + joinedString);
                                UC_Download.showMessage(resp.FailedOrders.Count + " Sales Return Creation Failed \n Error : " + joinedString);
                            }


                        }
                        if (resp.isLedgerMissmatch)
                        {
                            string joinedString = string.Join(" \n", resp.failedOrdersLineErrors);
                            UC_Download.showMessageToMasterUpdate(resp.FailedOrders.Count + " Sales return Creation Failed \n Error : " + joinedString);
                        }
                    }
                    else
                    {
                        if (batchCount == 0)
                        {
                            uC_Logger.AppendLogMsg("---------Nothing to Download------------");
                            //uC_Logger.AppendLogMsg("-----------Completed-----");
                        }
                        else
                        {
                       
                                uC_Logger.AppendLogMsg(+totalSuccessCount + "Sales Return Downloaded Successfully");
                                uC_Logger.AppendLogMsg(+totalFailureCount + " Sales Return  Failed to Download");
                                uC_Logger.AppendLogMsg(" Process Completed");
                         

                        }


                    }




                } while (salesOrderDTOs.Count>0);


            }


            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw new ServiceException(ex.Message);
            }

        }
        public List<SalesOrderDTO> getSalesReturnFromServer(VoucherType voucherType, DateTime? salesDate)
        {
            try
            {
                string serverAddress;
                List<SalesOrderDTO> salesOrderDTOs = new();
                String formattedDate = salesDate.Value.ToString("yyyy-MM-dd");
               

                    if (ApplicationProperties.properties["enable.Selecteddate"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        serverAddress = ApiConstants.DOWNLOAD_SALES_RETURN + "?voucherType=" + voucherType + " &salesDate=" + formattedDate;
                    }
                    else
                    {
                        serverAddress = ApiConstants.DOWNLOAD_SALES_RETURN+ "?voucherType=" + voucherType;
                    }
                
                
                LogManager.WriteLog("which api" + serverAddress);
                // test code  testing
                // serverAddress="http://localhost:3000/salesorder";

                LogManager.WriteLog("downloading sales Return from server....");
                _httpClient = RestClientUtil.getClient();


                var responseTask = _httpClient.GetAsync(serverAddress);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteResponseLog(Res);

                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("request for downloading SalesReturn  Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                    salesOrderDTOs = JsonConvert.DeserializeObject<List<SalesOrderDTO>>(response);
                    return salesOrderDTOs;
                }
                else
                {
                    LogManager.WriteLog("request for downloading salesOrders Failed..");
                    LogManager.WriteLog("downloading salesOrders Failed statuscode:" + Res.StatusCode + " Message : " + Res.RequestMessage);

                    throw new ServiceException("downloading salesOrders Failed statuscode: " + Res.StatusCode);

                    return salesOrderDTOs;
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog("request for downloading salesReturn Failed...\n Exception details : " + ex.Message);
                LogManager.HandleException(ex);
                throw ex;
            }
        }
        private void updatesalesOrderStatus(List<string> succesOrders, UC_Logger uC_Logger)
        {
            //uC_Logger.AppendLogMsg(" Updating Staus ");

            string updatesalesOrderStatus = ApiConstants.UPDATE_ORDER_STATUS;
            LogManager.WriteLog("updating  salesOrders status ....");
            _httpClient = RestClientUtil.getClient();
            var myContent = JsonConvert.SerializeObject(succesOrders);
            HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");
            LogManager.WriteRequestContentLog(" MyContent : "+myContent.Length+" - " +myContent.ToString(), updatesalesOrderStatus);
            var responseTask = _httpClient.PostAsync(updatesalesOrderStatus, inputContent);


            // var responseTask = httpClient.PostAsync(updatesalesOrderStatus);

            responseTask.Wait();

            HttpResponseMessage Res = responseTask.Result;
            LogManager.WriteResponseLog(Res);

            if (Res.IsSuccessStatusCode)
            {
                LogManager.WriteLog("request for updating  salesOrders  status  Success..");
                var response = Res.Content.ReadAsStringAsync().Result;

            }
            else
            {
                LogManager.WriteLog("request for updating  salesOrders status Failed..");

            }
        }



    }
}