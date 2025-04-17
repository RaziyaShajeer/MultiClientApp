    using dtos;
using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
using SNR_ClientApp.Tally.generateXml;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using SNR_ClientApp.Windows.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    internal class DownloadSalesService
    {
        TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        private String enableDateWise = ApplicationProperties.properties["enable.date"].ToString();
        private String enableselectedDateWise = ApplicationProperties.properties["enable.Selecteddate"].ToString();
        SalesVoucherGenerateXml salesVoucherGenerateXml;
        private TransactionProcessor transactionProcessor;
        public DownloadSalesService()
        {
            httpClient = new HttpClient();  
            tallyCommunicator = new TallyCommunicator();
            salesVoucherGenerateXml=new SalesVoucherGenerateXml();
            transactionProcessor = new TransactionProcessor();
        }
        internal async Task getFromServerAndDownloadToTallyWithDateAsync(DateTime salesDate, string employeeVoucher,UC_Logger uC_Logger)
        {
            List<String> successorders = new();
            List<SalesOrderDTO> salesOrderDTOs = new();
        
            int batchCount = 0;
            int totalSuccessCount = 0;
            int totalFailureCount = 0;
            do
            {

                try
                {
                    List<TallyXml> tallyXmls = new List<TallyXml>();
                    LogManager.WriteLog("downloading inventory data from server with date.");

                    salesOrderDTOs = getInventoryDataFromServer(employeeVoucher, salesDate);

                    if (salesOrderDTOs.Count > 0)
                    {
                        batchCount++;
                        uC_Logger.AppendLogMsg("Batch-"+batchCount+"-Downloading");
                        foreach (var salesOrderDto in salesOrderDTOs)
                        {
                            if ("true".Equals(enableDateWise, StringComparison.OrdinalIgnoreCase))
                            {
                                string formattedDate = salesDate.ToString("yyyy-MM-dd");
                                salesOrderDto.date = (formattedDate);
                            }
                            ENVELOPE salesOrderXml = await salesVoucherGenerateXml.generateSalesOrderXml(salesOrderDto);
                            tallyXmls.Add(new TallyXml(salesOrderDto.inventoryVoucherHeaderPid, salesOrderXml));
                        }
                        TallyResponse response = await transactionProcessor.postOrdersToTally(tallyXmls);
                        LogManager.WriteLog(response.ToString());
                        if (response.body != null)
                        {
                            //successorders = (List<String>)response.body;
                            DownloadResponseDto resp = response.body as DownloadResponseDto;
                            successorders = resp.SuccessOrders;
                            totalSuccessCount=successorders.Count+totalSuccessCount;

                            LogManager.WriteLog("Success sales Count:"+successorders.Count);

                            updatesalesOrderStatus(successorders);
                            if (resp.FailedOrders.Count > 0)
                            {
                                totalFailureCount=totalFailureCount+resp.FailedOrders.Count;
                                LogManager.WriteLog("Failed sales Count:"+resp.FailedOrders.Count);
                                string updatesalesOrderFailedStatus = ApiConstants.UPDATE_ORDER_STATUS_PENDING;
                                HttpContent content2 = new StringContent(JsonConvert.SerializeObject(resp.FailedOrders), Encoding.UTF8, "application/json");
                                HttpResponseMessage updateResult = httpClient.PostAsync(updatesalesOrderFailedStatus, content2).Result;
                            }
                            if (resp.isLedgerMissmatch)
                            {
                                string joinedString = string.Join(" \n", resp.failedOrdersLineErrors);
                                UC_Download.showMessageToMasterUpdate(resp.FailedOrders.Count + " Order Creation Failed \n Error : " + joinedString);
                            }
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
                           
                           
                                uC_Logger.AppendLogMsg(+totalSuccessCount + "Sales  Downloaded Successfully");
                                uC_Logger.AppendLogMsg(+totalFailureCount + " Sales   Failed to Download");
                                uC_Logger.AppendLogMsg(" Process Completed");
                            

                        }
                    }

                }
                catch (Exception ex)
                {
                    LogManager.HandleException(ex);
                    throw ex;
                }
            } while (salesOrderDTOs.Count > 0);


        }

        private void updatesalesOrderStatus(List<string> succesOrders)
        {
            string updatesalesOrderStatus =  ApiConstants.UPDATE_ORDER_STATUS;
            LogManager.WriteLog("updating  salesOrders status ....");
            httpClient = RestClientUtil.getClient();
            if (succesOrders.Count > 0)
            {
                HttpContent content = new StringContent(JsonConvert.SerializeObject(succesOrders), Encoding.UTF8, "application/json");

                var responseTask = httpClient.PostAsync(updatesalesOrderStatus, content);
                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteResponseLog(Res);

                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("request for updating  salesOrders  Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;

                }
                else
                {
                    LogManager.WriteLog("request forupdating  salesOrders Failed..");

                }
            }
            else
            {
                LogManager.WriteLog("Succes orders count is 0");
            }

            //var responseTask = httpClient.GetAsync(updatesalesOrderStatus,succesOrders);

            //responseTask.Wait();

            //HttpResponseMessage Res = responseTask.Result;
            //LogManager.WriteResponseLog(Res);

            //if (Res.IsSuccessStatusCode)
            //{
            //    LogManager.WriteLog("request for updating  salesOrders  Success..");
            //    var response = Res.Content.ReadAsStringAsync().Result;

            //}
            //else
            //{
            //    LogManager.WriteLog("request forupdating  salesOrders Failed..");

            //}
        }

        private List<SalesOrderDTO> getInventoryDataFromServer(string employeeVoucher,DateTime? salesDate)
        {
            try
            {
                string serverAddress;
                String formattedDate = salesDate.Value.ToString("yyyy-MM-dd");
                if (ApplicationProperties.properties["Isoptimized"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    if ("true".Equals(enableselectedDateWise, StringComparison.OrdinalIgnoreCase))
                    {
                        serverAddress =  ApiConstants.DOWNLOAD_SALES_OPTIMIZED  + "?employeeVoucher=" + employeeVoucher.Trim()+"?salesDate"+formattedDate;

                    }
                    else
                    {
                        serverAddress = ApiConstants.DOWNLOAD_SALES_OPTIMIZED + "?employeeVoucher=" + employeeVoucher.Trim();
                    }
                }
                else
                {
                    serverAddress = ApiConstants.DOWNLOAD_SALES + "?employeeVoucher=" + employeeVoucher.Trim();
                }
            

                //test code 27/10/2023
                //serverAddress="http://localhost:3000/Salesorder";


                String updatesalesOrderStatus = ApiConstants.UPDATE_ORDER_STATUS;

                LogManager.WriteLog("downloading inventory data from server....");
                httpClient = RestClientUtil.getClient();


                var responseTask = httpClient.GetAsync(serverAddress);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteResponseLog(Res);

                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("request for downloading inventory data    Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                    List<SalesOrderDTO> salesOrderDTOs = JsonConvert.DeserializeObject<List<SalesOrderDTO>>(response);
                    return salesOrderDTOs;
                }
                else
                {
                    LogManager.WriteLog("request for downloading inventory data Failed..");
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog("request for downloading inventory data Failed...\n Exception details : " + ex.Message);
                LogManager.HandleException(ex);
                throw ex;
            }
        }
    }
}
