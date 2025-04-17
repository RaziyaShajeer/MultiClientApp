using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.DTO.Salesorder;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Tally.generateXml;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using SNR_ClientApp.Windows.CustomControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SNR_ClientApp.Tally
{
    public class TransactionProcessor
    {
        TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        SalesOrderGenerateXmlfromMasterID salesOrderGenerateXmlfromMaster;
        public TransactionProcessor()
        {
            salesOrderGenerateXmlfromMaster=new SalesOrderGenerateXmlfromMasterID();    
            tallyCommunicator = new();
            httpClient = new();
        }
        public async Task<TallyResponse> postOrdersToTally(List<TallyXml> tallyXmls)
        {
            LogManager.WriteLog("postOrdersToTally ................" + tallyXmls.Count);
            List<String> successOrders = new List<String>();
            List<String> failedOrders = new List<String>();
            Dictionary<int, string> masterIds = new Dictionary<int, string>();
            bool isLedgerMissmatch=false;
			List<String> failedOrdersLineErrors = new List<String>();
         List<SalesStatusDTO> salesStatusDTOS = new List<SalesStatusDTO>();


            foreach (TallyXml tallyRequest in tallyXmls)
            {
                try
                {
					var ns = new XmlSerializerNamespaces();
					ns.Add("UDF", "http://schemas.sdk"); // prefix "UDF" maps to your namespace URI

					var serializer = new XmlSerializer(tallyRequest.xmlObj.GetType());
					var stringwriter = new StringWriter();
					serializer.Serialize(stringwriter, tallyRequest.xmlObj, ns);
					
                    LogManager.WriteLog(stringwriter.ToString());
                    TallyDownloadResponse data = await tallyCommunicator.UploadDataToTally(stringwriter.ToString());
                    LogManager.WriteLog(data.response.ToString());
                 
                    if (data != null)
                    {
                        if (data.response.ERRORS > 0||data.response.EXCEPTIONS>0)
                        {
                 
                            LogManager.WriteLog("Orders post failed for "+ tallyRequest.pid+"\n LineError :  "+data.response.LINEERROR);
                            /*failedOrders.Add(tallyRequest.pid)*/;
                            if (!String.IsNullOrEmpty(data.response.LINEERROR))
                            {
								string mainString = data.response.LINEERROR;
                                //string pattern = "(?=.*Ledger)(?=.*does not exist)";
                                string pattern = "(?=.*does not exist)";
                                if (Regex.IsMatch(mainString, pattern))
                                {
									isLedgerMissmatch=true;
                                    //failedOrdersLineErrors.Add(mainString);
                                    //UC_Download.showMessage("Order Creation Failed \n"+data.response.LINEERROR);
                                }
                                failedOrdersLineErrors.Add(mainString);
                                //if (data.response.LINEERROR.Contains("sdfdsf")
                            }
                            //UC_Download.showMessage("Order Creation Failed \n"+data.response.LINEERROR);

							throw new ServiceException( "\n Error :  " + data.response.LINEERROR);
                        }
                        else
                        {

                            LogManager.WriteLog("Orders post success .............." + tallyRequest.pid);
                            successOrders.Add(tallyRequest.pid);

                            masterIds.Add(data.response.LASTVCHID, tallyRequest.pid);//Api Need


                          
                        }
                    }
                    else
                    {
                        LogManager.WriteLog("Orders post failed  .......");
						
						throw new ServiceException("Orders post failed for " + tallyRequest.pid);
                    }
                }
                catch (ServiceException ex)
                {
                    LogManager.WriteLog("Orders post failed  .......");
                    LogManager.HandleException(ex);
					failedOrders.Add(tallyRequest.pid);
					// throw ex;vuccessOrders);
				}
                catch (Exception ex)
                {
                    LogManager.WriteLog("Orders post failed  .......");
                    failedOrders.Add(tallyRequest.pid);
                    LogManager.HandleException(ex);
                    //throw new ServiceException("Orders post failed ", ex.InnerException);
                    //return new TallyResponse("OK", "Orders post to tally failed", successOrders);
                }
            }
            foreach (var masterId in masterIds)
            {
                ENVELOPE voucherIds = await salesOrderGenerateXmlfromMaster.GenerateSalesOrderVoucherNumberfromMastrerId(masterId.Key.ToString());
                var stringwriter = new System.IO.StringWriter();
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(voucherIds.GetType());
                x.Serialize(stringwriter, voucherIds);

                var voucherIdListquery = await tallyCommunicator.ExecXml(stringwriter.ToString());
                SalesStatusDTO salesOrderDetails = new SalesStatusDTO();
                foreach (var voucher in voucherIdListquery.response.BODY.DATA.COLLECTION.VOUCHER)
                {
                    salesOrderDetails.invPid=masterId.Value;
                    salesOrderDetails.voucherNo=voucher.VOUCHERNUMBER;

                    salesStatusDTOS.Add(salesOrderDetails);
                }
            }

            //object wrappedObject = salesStatusDTOS;
            LogManager.WriteLog(salesStatusDTOS.ToString());
            string UpdateSalesOrderStatus = ApiConstants.UPDATE_STATUS;

            LogManager.WriteLog("updating  salesOrders status Api:...."+UpdateSalesOrderStatus);
            httpClient = RestClientUtil.getClient();
           
            HttpContent content2 = new StringContent(JsonConvert.SerializeObject(salesStatusDTOS), Encoding.UTF8, "application/json");
            var responseTask = httpClient.PostAsync(UpdateSalesOrderStatus, content2);
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


            
			DownloadResponseDto resp=new DownloadResponseDto(successOrders, failedOrders);
            resp.failedOrdersLineErrors= failedOrdersLineErrors;
            LogManager.WriteLog(failedOrders.Count.ToString() + "Failed" +successOrders.Count+"Success");
            resp.isLedgerMissmatch= isLedgerMissmatch;
			return new TallyResponse("OK", "Orders post to tally successfully", resp);
        }

        public async Task<TallyResponse> postReceiptsToTallyAsync(List<TallyXml> receipts)
        {
            LogManager.WriteLog("postReceiptsToTally ................" + receipts.Count);
            List<String> successReceipts = new List<String>();
			//List<String> successOrders = new List<String>();
			List<String> failedReceipts = new List<String>();
			bool isLedgerMissmatch = false;
			List<String> failedLineErrors = new List<String>();
			foreach (TallyXml tallyRequest in receipts)
            {
                try
                {
                    var stringwriter = new System.IO.StringWriter();
                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyRequest.xmlObj.GetType());
                    x.Serialize(stringwriter, tallyRequest.xmlObj);

                    TallyDownloadResponse data = await tallyCommunicator.UploadDataToTally(stringwriter.ToString());
                    if (data != null)
                    {
                        if (data.response.ERRORS > 0)
                        {
                            LogManager.WriteLog("Receipt post failed for "+ tallyRequest.pid+"\n LineError :  "+data.response.LINEERROR);
                            /*failedOrders.Add(tallyRequest.pid)*/
                            ;
                            if (!String.IsNullOrEmpty(data.response.LINEERROR))
                            {
                                string mainString = data.response.LINEERROR;
                                //string pattern = "(?=.*Ledger)(?=.*does not exist)";
                                string pattern = "(?=.*does not exist)";

                                if (Regex.IsMatch(mainString, pattern))
                                {
                                    isLedgerMissmatch=true;
                                   
                                    //UC_Download.showMessage("Order Creation Failed \n"+data.response.LINEERROR);
                                }
								failedLineErrors.Add(mainString);
								//if (data.response.LINEERROR.Contains("sdfdsf")
							}
                            //UC_Download.showMessage("Order Creation Failed \n"+data.response.LINEERROR);

                            throw new ServiceException( data.response.LINEERROR);
                        }
                        else
                        {
                            LogManager.WriteLog("Receipt post success .............." + tallyRequest.pid);
                            successReceipts.Add(tallyRequest.pid);
                        }
                    }
                    else
                    {
                        LogManager.WriteLog("Receipt post failed  .......");
						throw new ServiceException("Orders post failed for " + tallyRequest.pid);
					}
                }
				catch (ServiceException ex)
				{
					LogManager.WriteLog("Orders post failed  .......");
					LogManager.HandleException(ex);
					failedReceipts.Add(tallyRequest.pid);
					// throw ex;
					//return new TallyResponse("OK", "Orders post to tally failed", successOrders);
				}
				catch (Exception ex)
                {
                    LogManager.WriteLog("Receipt post failed  .......");
                    LogManager.HandleException(ex);
                    //return new TallyResponse("OK", "receipt post to tally failed", successReceipts);
                }
            }
			DownloadResponseDto resp = new DownloadResponseDto(successReceipts, failedReceipts);
			resp.failedOrdersLineErrors= failedLineErrors;
			resp.isLedgerMissmatch= isLedgerMissmatch;
			return new TallyResponse("OK", "receipt post to tally successfully", resp);
        }
    }
}
