using dtos;
using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
using SNR_ClientApp.Tally.generateXml;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using SNR_ClientApp.Windows.CustomControls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    internal class DownloadSalesOrderService
    {
        readonly TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        SalesOrderGenerateXml salesOrderGenerateXml;
        TransactionProcessor transactionProcessor;
        private String enableDateWise=ApplicationProperties.properties["enable.date"].ToString();
        public DownloadSalesOrderService()
        {
            httpClient=new HttpClient();
            tallyCommunicator=new TallyCommunicator();
            salesOrderGenerateXml=new SalesOrderGenerateXml();
            transactionProcessor=new TransactionProcessor();    
        }

        public async Task getFromServerAndDownloadToTallyAsync(DateTime salesDate,UC_Logger uC_Logger)
        {
            Console.WriteLine("downloading inventory data from server.");
            try
            {

                List<string> succesOrders = new();
                string serverAddress;
                String formattedDate = salesDate.ToString("yyyy-MM-dd");
                if (ApplicationProperties.properties["Isoptimized"].ToString() .Equals( "True",StringComparison.OrdinalIgnoreCase))
                {

                     if (ApplicationProperties.properties["enable.Selecteddate"].ToString().Equals("true",StringComparison.OrdinalIgnoreCase))
                    {
                         serverAddress =  ApiConstants.DOWNLOAD_ORDER_ISOPTIMIZED +"?salesDate=" + formattedDate;
                    }
                    else
                    {
                        serverAddress = ApiConstants.DOWNLOAD_ORDER_ISOPTIMIZED;
                     

                    }
                }
                else
                {
                      serverAddress =  ApiConstants.DOWNLOAD_ORDER; 
                }
               
                string updatesalesOrderStatus = ApiConstants.UPDATE_ORDER_STATUS;
				string updatesalesOrderFailedStatus = ApiConstants.UPDATE_ORDER_STATUS_PENDING;
                LogManager.WriteLog("downloading inventory data from server.....");
            
                List<SalesOrderDTO> salesOrderDTOs = new();
                int batchCount = 0;
                int totalSuccessCount = 0;
                int totalFailureCount = 0;
                do
                {
                    httpClient = RestClientUtil.getClient();
                    var responseTask = httpClient.GetAsync(serverAddress);
                    List<TallyXml> tallyXmls = new List<TallyXml>();
                    responseTask.Wait();
                    HttpResponseMessage Res = responseTask.Result;
                    LogManager.WriteResponseLog(Res);

                    if (Res.IsSuccessStatusCode)
                    {
                       
                        LogManager.WriteLog("request for downloading salesOrders    Success..");
                        var response = Res.Content.ReadAsStringAsync().Result;
                         salesOrderDTOs = JsonConvert.DeserializeObject<List<SalesOrderDTO>>(response);
                        if (salesOrderDTOs.Count > 0)
                        {
                            batchCount++;
                            if (ApplicationProperties.properties["Isoptimized"].ToString() .Equals("true",StringComparison.OrdinalIgnoreCase))
                            {
                                uC_Logger.AppendLogMsg("Batch -" + batchCount + "- Downloading");
                            }

                            foreach (SalesOrderDTO salesOrderDTO in salesOrderDTOs)
                            {
                                if (string.Equals("true", enableDateWise, StringComparison.OrdinalIgnoreCase))
                                {

                                    salesOrderDTO.date = formattedDate;
                                }

                                ENVELOPE salesOrderXml = await salesOrderGenerateXml.generateSalesOrderXmlAsync(salesOrderDTO);
                                Console.WriteLine(salesOrderXml);
                                tallyXmls.Add(new TallyXml(salesOrderDTO.inventoryVoucherHeaderPid, salesOrderXml));
                            }

                            TallyResponse res = await transactionProcessor.postOrdersToTally(tallyXmls);

                            DownloadResponseDto resp = res.body as DownloadResponseDto;
                            succesOrders=resp.SuccessOrders;
                            if (succesOrders.Count > 0)
                            {
                                totalSuccessCount=totalSuccessCount+succesOrders.Count;

                                LogManager.WriteLog("Succss Sales Orders: "+succesOrders.Count);
                                HttpContent content = new StringContent(JsonConvert.SerializeObject(succesOrders), Encoding.UTF8, "application/json");
                                HttpResponseMessage updateResult = httpClient.PostAsync(updatesalesOrderStatus, content).Result;
                            }
                            if (resp.FailedOrders.Count > 0)
                            {
                                totalFailureCount=totalFailureCount+resp.FailedOrders.Count;
                                LogManager.WriteLog("Failed Sales Orders: "+resp.FailedOrders.Count);
                                HttpContent content2 = new StringContent(JsonConvert.SerializeObject(resp.FailedOrders), Encoding.UTF8, "application/json");
                                HttpResponseMessage updateResult = httpClient.PostAsync(updatesalesOrderFailedStatus, content2).Result;
                            }
                            if (resp.isLedgerMissmatch)
                            {
                                string joinedString = string.Join(" \n", resp.failedOrdersLineErrors);
                                UC_Download.showMessageToMasterUpdate(resp.FailedOrders.Count+" Order Creation Failed \n Error : "+joinedString);
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
                                uC_Logger.AppendLogMsg(+totalSuccessCount + "Sales Orders Downloaded Successfully");
                                uC_Logger.AppendLogMsg(+totalFailureCount + " Sales Orders  Failed to Download");
                          
                            }
                        }
                    }
                    else
                    {
                       
                            LogManager.WriteLog("downloading inventory data from server failed");

                            throw new ServiceException("downloading inventory data from server failed \n Response status code :"+Res.StatusCode);
                        

                    }




                } while(salesOrderDTOs.Count > 0);    

               

            }
            catch (Exception exception)
            {
                LogManager.HandleException(exception);
                throw new ServiceException(exception.Message);
            }
        }
    }
}
