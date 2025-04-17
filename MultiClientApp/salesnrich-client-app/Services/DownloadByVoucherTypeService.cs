using dtos;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using OfficeOpenXml.Style;
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
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace SNR_ClientApp.Services
{
    public class DownloadByVoucherTypeService
    {
        TallyCommunicator tallyCommunicator;
        HttpClient httpClient;


        SalesOrderGenerateXmlfromMasterID salesOrderGenerateXmlfromMasterID;

        private String enableDateWise = ApplicationProperties.properties["enable.date"].ToString();
        private String userStockLocation;

        private String companyString=ApplicationProperties.properties["tally.company"].ToString();

        private String apiUrl;
        private String byEmpVoucher;

        private String employeeString;
        //private String isOptionalsalesOrder= ApplicationProperties.properties["is.optional.salesOrder"].ToString();
        //private String companyState= ApplicationProperties.properties["company.state"].ToString();
        //private String orderNumberWithEmployee = ApplicationProperties.properties["order.employee.name"].ToString();
        //private String actualBilledStatus = ApplicationProperties.properties["actual.billed.status"].ToString();
        //private String gstNames= ApplicationProperties.properties["tally.gst"].ToString();
        //private String kfcLedger = ApplicationProperties.properties["kfc.ledger.name"].ToString();
        //private String gstCalculationEnabled= ApplicationProperties.properties["gst.ledger.calculation"].ToString();
        //private String roundOffLedger= ApplicationProperties.properties["round.off.ledger"].ToString();
        public String multiTax = ApplicationProperties.properties["is.multi.tax"].ToString();
        public String gstTax = ApplicationProperties.properties["tally.taxs"].ToString();
        //private String reduceTax= ApplicationProperties.properties["reduce.tax"].ToString();
        //private String productRateIncludingTax= ApplicationProperties.properties["product.rate.including.tax"].ToString();
        //private String salesOrderActivityRemark= ApplicationProperties.properties["sales.order.activity.remarks"].ToString();
        //private String itemRemarksEnabled = ApplicationProperties.properties["item.remarks.enabled"].ToString();
        //private String enableCashOnlyLedgerEntry = ApplicationProperties.properties["enable.cash.only.ledger.entry"].ToString();
        //private String enableCostCentre= ApplicationProperties.properties["enable.cost.centre"].ToString();
        //private String paymentModeOrTerms = ApplicationProperties.properties["payment.mode.terms"].ToString();
        //private String godownFixed = ApplicationProperties.properties["godown.fixed"].ToString();
        //private String batchFixed = ApplicationProperties.properties["batch.fixed"].ToString();
        //private String salesLedger= ApplicationProperties.properties["sales.ledger"].ToString();
        TransactionProcessor transactionProcessor = new TransactionProcessor();
		public static string ledgerErrors;
		public DownloadByVoucherTypeService()
        {
            tallyCommunicator = new TallyCommunicator();
            salesOrderGenerateXmlfromMasterID=new SalesOrderGenerateXmlfromMasterID();
            httpClient = RestClientUtil.getClient();
            enableDateWise = ApplicationProperties.properties["enable.date"].ToString();
            companyString = ApplicationProperties.properties["tally.company"].ToString();
            //apiUrl = ApplicationProperties.properties["service.full.url"].ToString();
            byEmpVoucher = ApplicationProperties.properties["download.by.employee.voucher"].ToString();
            userStockLocation = ApplicationProperties.properties["user.stockLocation"].ToString();
        
        }
        public async Task getFromServerAndDownloadToTally(VoucherType voucherType, DateTime? salesDate = null, UC_Logger uC_Logger = null)
        {
            try
            {
                    List<SalesOrderDTO> salesOrderDTOs = new();
                    int batchCount = 0;
                    int totalSuccessCount = 0;
                    int totalFailureCount = 0;
				

				if (ApplicationProperties.properties["Isoptimized"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                {
					do
					{
						TimeSpan timeToOneIteration = TimeSpan.Zero;
						List<String> succesOrders = new();

						LogManager.WriteLog("downloading inventory data from server with date.");
						SalesVoucherGenerateXml salesVoucherGenerateXml = new();
						SalesOrderGenerateXml salesOrderGenerateXml = new();
				var currentTime = DateTime.Now;
						salesOrderDTOs = getSalesFromServer(voucherType, salesDate);
						var timeToGetFromServer = DateTime.Now- currentTime ;
						timeToOneIteration = timeToOneIteration + timeToGetFromServer;
						LogManager.WriteLog("Time to get from server  optimized:" + timeToGetFromServer);
						if (salesOrderDTOs.Count > 0)
						{
							uC_Logger.AppendLogMsg("Please Wait.. Processing Transactions.. ");
						}



						List<TallyXml> tallyXmls = new List<TallyXml>();

						if (salesOrderDTOs.Count > 0)
						{
							batchCount++;

							if (ApplicationProperties.properties["Isoptimized"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
							{
								uC_Logger.AppendLogMsg("Batch -" + batchCount + "- Downloading");
							}
							if (!userStockLocation.Equals("true", StringComparison.OrdinalIgnoreCase))
							{

								var tasks = salesOrderDTOs.Select(async salesOrderDTO =>
								{
									if ("true".Equals(enableDateWise, StringComparison.OrdinalIgnoreCase) && salesDate != null)
										salesOrderDTO.date = salesDate.Value.ToString("yyyy-MM-dd");

									ENVELOPE salesOrderXml = await salesVoucherGenerateXml.generateSalesOrderXml(salesOrderDTO);
									return new TallyXml(salesOrderDTO.inventoryVoucherHeaderPid, salesOrderXml);
								});

								tallyXmls = (await Task.WhenAll(tasks)).ToList();
								var sendtoTally = DateTime.Now;
								var res = await transactionProcessor.postOrdersToTally(tallyXmls);
								var sendtoTallydiff = DateTime.Now- sendtoTally;
								timeToOneIteration = timeToOneIteration + sendtoTallydiff;
								LogManager.WriteLog("Time to SendToTally" + sendtoTallydiff);
								DownloadResponseDto resp = res.body as DownloadResponseDto;
								succesOrders = resp.SuccessOrders;
								//succesOrders = (List<String>)res.body;
								if (succesOrders.Count > 0)
								{
									totalSuccessCount = totalSuccessCount + succesOrders.Count;
									var updateTime = DateTime.Now;
									updatesalesOrderStatus(succesOrders, uC_Logger);
									var TimetoUpdate = DateTime.Now-updateTime;
									timeToOneIteration = timeToOneIteration + TimetoUpdate;
									LogManager.WriteLog("Time to update Success Orders " + TimetoUpdate);
									LogManager.WriteLog(+succesOrders.Count + " Sales  downloaded");

								}
								if (resp.FailedOrders.Count > 0)
								{
									totalFailureCount = totalFailureCount + resp.FailedOrders.Count;
									LogManager.WriteLog(+resp.FailedOrders.Count + " Sales failed to downloaded");
									string updatesalesOrderFailedStatus = ApiConstants.UPDATE_ORDER_STATUS_PENDING;
									HttpContent content2 = new StringContent(JsonConvert.SerializeObject(resp.FailedOrders), Encoding.UTF8, "application/json");
									var updateTime = DateTime.Now;
									HttpResponseMessage updateResult = httpClient.PostAsync(updatesalesOrderFailedStatus, content2).Result;
									var TimetoUpdate = DateTime.Now-updateTime;
									timeToOneIteration = timeToOneIteration + TimetoUpdate;
									LogManager.WriteLog("Time to update Failed Orders " + TimetoUpdate);
									if (resp.failedOrdersLineErrors != null && resp.failedOrdersLineErrors.Count > 0 && !resp.isLedgerMissmatch)
									{
										string joinedString = string.Join(" \n", resp.failedOrdersLineErrors);
										
										LogManager.WriteLog(resp.FailedOrders.Count + " Order Creation Failed \n Error : " + joinedString);
										//UC_Download.showMessage(resp.FailedOrders.Count + " Order Creation Failed \n Error : " + joinedString);
									}


								}
								if (resp.isLedgerMissmatch)
								{
									string joinedString = string.Join(" \n", resp.failedOrdersLineErrors);
									ledgerErrors = ledgerErrors + joinedString;
									//UC_Download.showMessageToMasterUpdate(resp.FailedOrders.Count + " Order Creation Failed \n Error : " + joinedString);

								}
							}
							else
							{
								if (voucherType == VoucherType.PRIMARY_SALES_ORDER || voucherType == VoucherType.SECONDARY_SALES_ORDER)
								{
									if (salesOrderDTOs.Count > 0)
									{
										var tasks = salesOrderDTOs.Select(async salesOrderDTO =>
										{
											if ("true".Equals(enableDateWise, StringComparison.OrdinalIgnoreCase) && salesDate != null)
												salesOrderDTO.date = salesDate.Value.ToString("yyyy-MM-dd");

											ENVELOPE salesOrderXml = await salesVoucherGenerateXml.generateSalesOrderXml(salesOrderDTO);
											return new TallyXml(salesOrderDTO.inventoryVoucherHeaderPid, salesOrderXml);
										});

										tallyXmls = (await Task.WhenAll(tasks)).ToList();
										var sendtoTally = DateTime.Now;
										var res = await transactionProcessor.postOrdersToTally(tallyXmls);
										var sendtoTallydiff = DateTime.Now- sendtoTally ;
										timeToOneIteration = timeToOneIteration + sendtoTallydiff;
										LogManager.WriteLog("Time to send to tally Orders " + sendtoTallydiff);
										//succesOrders = (List<String>)res.body;
										DownloadResponseDto resp = res.body as DownloadResponseDto;
										succesOrders = resp.SuccessOrders;
										if (succesOrders.Count > 0)
										{

											totalSuccessCount = totalSuccessCount + succesOrders.Count;
											var updateTime = DateTime.Now;
											updatesalesOrderStatus(succesOrders, uC_Logger);
											var TimetoUpdate = DateTime.Now-updateTime;
											timeToOneIteration = timeToOneIteration + TimetoUpdate;
											LogManager.WriteLog("Time to update Success Orders " + TimetoUpdate);
											LogManager.WriteLog(+succesOrders.Count + " Sales Order downloaded");
										}
										if (resp.FailedOrders.Count > 0)
										{
											totalFailureCount = totalFailureCount + resp.FailedOrders.Count;
											string updatesalesOrderFailedStatus = ApiConstants.UPDATE_ORDER_STATUS_PENDING;

											HttpContent content2 = new StringContent(JsonConvert.SerializeObject(resp.FailedOrders), Encoding.UTF8, "application/json");
											
											var updateTime = DateTime.Now;
											HttpResponseMessage updateResult = httpClient.PostAsync(updatesalesOrderFailedStatus, content2).Result;
											var TimetoUpdate = updateTime - DateTime.Now;
											timeToOneIteration = timeToOneIteration + TimetoUpdate;
											LogManager.WriteLog("Time to update Failed Orders " + TimetoUpdate);
											LogManager.WriteLog(+resp.FailedOrders.Count + " Sales Order Failed to Download");
										}
										if (resp.isLedgerMissmatch)
										{
											string joinedString = string.Join(" \n", resp.failedOrdersLineErrors);
											ledgerErrors = ledgerErrors + joinedString;
											LogManager.WriteLog(resp.FailedOrders.Count + " Order Creation Failed \n Error : " + joinedString);
											//UC_Download.showMessageToMasterUpdate(resp.FailedOrders.Count + " Order Creation Failed \n Error : " + joinedString);
										}
									}
								}
								if (voucherType == VoucherType.PRIMARY_SALES)
								{
									if (salesOrderDTOs.Count > 0)
									{
										var tasks = salesOrderDTOs.Select(async salesOrderDTO =>
										{
											ENVELOPE salesOrderXml = await salesVoucherGenerateXml.generateSalesOrderXml(salesOrderDTO);
											return new TallyXml(salesOrderDTO.inventoryVoucherHeaderPid, salesOrderXml);
										});



										tallyXmls = (await Task.WhenAll(tasks)).ToList();
										var sendtoTally = DateTime.Now;
										var res = await transactionProcessor.postOrdersToTally(tallyXmls);
										var sendtoTallydiff = DateTime.Now-sendtoTally;
										LogManager.WriteLog("Time to send to tally " + sendtoTallydiff);
										timeToOneIteration = timeToOneIteration + sendtoTallydiff;
										//succesOrders = (List<String>)res.body;
										DownloadResponseDto resp = res.body as DownloadResponseDto;
										succesOrders = resp.SuccessOrders;
										if (succesOrders.Count > 0)
										{

											totalSuccessCount = totalSuccessCount + succesOrders.Count;

											var updatetime = DateTime.Now;
											updatesalesOrderStatus(succesOrders, uC_Logger);
											var updatetimediff = DateTime.Now-updatetime;
											LogManager.WriteLog("Time to update Success Orders status" + updatetimediff);
											timeToOneIteration = timeToOneIteration + updatetimediff;
											LogManager.WriteLog(+succesOrders.Count + " Sales  downloaded");
										}
										if (resp.FailedOrders.Count > 0)
										{
											//uC_Logger.AppendLogMsg(" Updating Staus ");
											totalFailureCount = totalFailureCount + resp.FailedOrders.Count;

											//uC_Logger.AppendLogMsg(+resp.FailedOrders.Count+ "  Sales Failed to Download");
											LogManager.WriteLog(resp.FailedOrders.Count + " Sales Failed to Download :");

											httpClient = RestClientUtil.getClient();

											var updatetime = DateTime.Now;
											string updatesalesOrderFailedStatus = ApiConstants.UPDATE_ORDER_STATUS_PENDING;
											var updatetimediff = DateTime.Now-updatetime;
											timeToOneIteration= timeToOneIteration+ updatetimediff;
											LogManager.WriteLog("Time to update failed Orders status:" + updatetimediff);
											// LogManager.WriteRequestContentLog(" Failed Content : " + resp.FailedOrders.Count + " - " + resp.FailedOrders.ToString(), updatesalesOrderFailedStatus);
											HttpContent content2 = new StringContent(JsonConvert.SerializeObject(resp.FailedOrders), Encoding.UTF8, "application/json");
											HttpResponseMessage updateResult = httpClient.PostAsync(updatesalesOrderFailedStatus, content2).Result;
										}
										if (resp.isLedgerMissmatch)
										{
											string joinedString = string.Join(" \n", resp.failedOrdersLineErrors);
											ledgerErrors = ledgerErrors + joinedString;
											LogManager.WriteLog(resp.FailedOrders.Count + " Order Creation Failed \n Error : " + joinedString);
											//UC_Download.showMessageToMasterUpdate(resp.FailedOrders.Count + " Order Creation Failed \n Error : " + joinedString);
										}
									}
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
								if (voucherType == VoucherType.PRIMARY_SALES_ORDER)
								{
									uC_Logger.AppendLogMsg(+totalSuccessCount + "Sales Order Downloaded Successfully");
									uC_Logger.AppendLogMsg(+totalFailureCount + " Sales Order  Failed to Download");
									if (totalFailureCount > 0 && ledgerErrors!="")
									{
										UC_Download.showMessageToMasterUpdate(totalFailureCount + " Order Creation Failed \n Error : " + ledgerErrors);
										ledgerErrors = "";
									}
									uC_Logger.AppendLogMsg(" Process Completed");

								}
								else
								{
									uC_Logger.AppendLogMsg(+totalSuccessCount + "Sales  Downloaded Successfully");
									uC_Logger.AppendLogMsg(+totalFailureCount + " Sales   Failed to Download");
									if (totalFailureCount > 0 && ledgerErrors != "")
									{
										UC_Download.showMessageToMasterUpdate(totalFailureCount + " Order Creation Failed \n Error : " + ledgerErrors);
										ledgerErrors = "";
									}
									uC_Logger.AppendLogMsg(" Process Completed");
								}

							}


						}
						LogManager.WriteLog("Total Time for one iteration" + timeToOneIteration);
					} while (salesOrderDTOs.Count > 0);



				}
                else
                {
					List<String> succesOrders = new();

					LogManager.WriteLog("downloading inventory data from server with date.");
					SalesVoucherGenerateXml salesVoucherGenerateXml = new();
					SalesOrderGenerateXml salesOrderGenerateXml = new();
					var date= DateTime.Now;	

					salesOrderDTOs = getSalesFromServer(voucherType, salesDate);
					var datediff = date - DateTime.Now;
					LogManager.WriteLog("Time to get from setver not optimized:" + datediff);
					if (salesOrderDTOs.Count > 0)
					{
						uC_Logger.AppendLogMsg("Please Wait.. Processing Transactions.. ");
					}
					List<TallyXml> tallyXmls = new List<TallyXml>();
					if (salesOrderDTOs.Count > 0)
					{
						if (!userStockLocation.Equals("true", StringComparison.OrdinalIgnoreCase))
						{

							var tasks = salesOrderDTOs.Select(async salesOrderDTO =>
							{
								if ("true".Equals(enableDateWise, StringComparison.OrdinalIgnoreCase) && salesDate != null)
									salesOrderDTO.date = salesDate.Value.ToString("yyyy-MM-dd");

								ENVELOPE salesOrderXml = await salesVoucherGenerateXml.generateSalesOrderXml(salesOrderDTO);
								return new TallyXml(salesOrderDTO.inventoryVoucherHeaderPid, salesOrderXml);
							});

							tallyXmls = (await Task.WhenAll(tasks)).ToList();
							var time = DateTime.Now;

							var res = await transactionProcessor.postOrdersToTally(tallyXmls);
							var timedif = time - DateTime.Now;
							LogManager.WriteLog("Time to post orders to tally" + timedif);
							DownloadResponseDto resp = res.body as DownloadResponseDto;
							succesOrders = resp.SuccessOrders;
							//succesOrders = (List<String>)res.body;
							if (succesOrders.Count > 0)
							{
								totalSuccessCount = totalSuccessCount + succesOrders.Count;
								var updatetime = DateTime.Now;
								updatesalesOrderStatus(succesOrders, uC_Logger);
								var updatetimediff = updatetime - DateTime.Now;
								LogManager.WriteLog("Time to update status:" + datediff);
								LogManager.WriteLog(+succesOrders.Count + " Sales  downloaded");

							}
							if (resp.FailedOrders.Count > 0)
							{
								totalFailureCount = totalFailureCount + resp.FailedOrders.Count;
								LogManager.WriteLog(+resp.FailedOrders.Count + " Sales failed to downloaded");
								var updatetime = DateTime.Now;
								string updatesalesOrderFailedStatus = ApiConstants.UPDATE_ORDER_STATUS_PENDING;
								var updatetimediff = updatetime - DateTime.Now;
								LogManager.WriteLog("Time to update status:" + updatetimediff);
								HttpContent content2 = new StringContent(JsonConvert.SerializeObject(resp.FailedOrders), Encoding.UTF8, "application/json");
								HttpResponseMessage updateResult = httpClient.PostAsync(updatesalesOrderFailedStatus, content2).Result;

								if (resp.failedOrdersLineErrors != null && resp.failedOrdersLineErrors.Count > 0 && !resp.isLedgerMissmatch)
								{
									string joinedString = string.Join(" \n", resp.failedOrdersLineErrors);
									LogManager.WriteLog(resp.FailedOrders.Count + " Order Creation Failed \n Error : " + joinedString);
									UC_Download.showMessage(resp.FailedOrders.Count + " Order Creation Failed \n Error : " + joinedString);
								}


							}
							if (resp.isLedgerMissmatch)
							{
								string joinedString = string.Join(" \n", resp.failedOrdersLineErrors);
								UC_Download.showMessageToMasterUpdate(resp.FailedOrders.Count + " Order Creation Failed \n Error : " + joinedString);
							}
						}
						else
						{
							if (voucherType == VoucherType.PRIMARY_SALES_ORDER || voucherType == VoucherType.SECONDARY_SALES_ORDER)
							{
								if (salesOrderDTOs.Count > 0)
								{
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

										ENVELOPE salesOrderXml = await salesOrderGenerateXml.generateSalesOrderXmlAsync(salesOrderDTO);
										tallyXmls.Add(new TallyXml(salesOrderDTO.inventoryVoucherHeaderPid, salesOrderXml));
									}
									var time = DateTime.Now;
									var res = await transactionProcessor.postOrdersToTally(tallyXmls);
									var timeDiff = time - DateTime.Now;
									LogManager.WriteLog("Time to Post to tally" + timeDiff);
									//succesOrders = (List<String>)res.body;
									DownloadResponseDto resp = res.body as DownloadResponseDto;
									succesOrders = resp.SuccessOrders;
									if (succesOrders.Count > 0)
									{

										totalSuccessCount = totalSuccessCount + succesOrders.Count;
										var timetoupdate = DateTime.Now;
										updatesalesOrderStatus(succesOrders, uC_Logger);
										var difftoUpdate = timetoupdate - DateTime.Now;
										LogManager.WriteLog("Time to update primary salesOrder:{0}" + difftoUpdate);
										LogManager.WriteLog(+succesOrders.Count + " Sales Order downloaded");
									}
									if (resp.FailedOrders.Count > 0)
									{
										totalFailureCount = totalFailureCount + resp.FailedOrders.Count;
										string updatesalesOrderFailedStatus = ApiConstants.UPDATE_ORDER_STATUS_PENDING;

										HttpContent content2 = new StringContent(JsonConvert.SerializeObject(resp.FailedOrders), Encoding.UTF8, "application/json");
										HttpResponseMessage updateResult = httpClient.PostAsync(updatesalesOrderFailedStatus, content2).Result;
										LogManager.WriteLog(+resp.FailedOrders.Count + " Sales Order Failed to Download");
									}
									if (resp.isLedgerMissmatch)
									{
										string joinedString = string.Join(" \n", resp.failedOrdersLineErrors);
										LogManager.WriteLog(resp.FailedOrders.Count + " Order Creation Failed \n Error : " + joinedString);
										UC_Download.showMessageToMasterUpdate(resp.FailedOrders.Count + " Order Creation Failed \n Error : " + joinedString);
									}
								}
							}
							if (voucherType == VoucherType.PRIMARY_SALES)
							{
								if (salesOrderDTOs.Count > 0)
								{
									foreach (SalesOrderDTO salesOrderDTO in salesOrderDTOs)
									{
										ENVELOPE salesOrderXml = await salesVoucherGenerateXml.generateSalesOrderXml(salesOrderDTO);
										tallyXmls.Add(new TallyXml(salesOrderDTO.inventoryVoucherHeaderPid, salesOrderXml));
									}
								}
								var timeinprimarysales = DateTime.Now;
								var res = await transactionProcessor.postOrdersToTally(tallyXmls);
								var timeinprimarysalesdiff=timeinprimarysales - DateTime.Now;
								LogManager.WriteLog("Timesend orders to tally" + timeinprimarysalesdiff);
								//succesOrders = (List<String>)res.body;
								DownloadResponseDto resp = res.body as DownloadResponseDto;
								succesOrders = resp.SuccessOrders;
								if (succesOrders.Count > 0)
								{
								
									totalSuccessCount = totalSuccessCount + succesOrders.Count;
									var timeinprimarysalesupdate = DateTime.Now;
									updatesalesOrderStatus(succesOrders, uC_Logger);
									var timeinprimarysalesupdatediff = timeinprimarysalesupdate - DateTime.Now;

									LogManager.WriteLog("Time to update primary sales:{0}" + timeinprimarysalesupdatediff);
									LogManager.WriteLog(+succesOrders.Count + " Sales  downloaded");
									uC_Logger.AppendLogMsg(+succesOrders.Count + " Sales  downloaded");
								}
								if (resp.FailedOrders.Count > 0)
								{
									//uC_Logger.AppendLogMsg(" Updating Staus ");
									totalFailureCount = totalFailureCount + resp.FailedOrders.Count;

									//uC_Logger.AppendLogMsg(+resp.FailedOrders.Count+ "  Sales Failed to Download");
									LogManager.WriteLog(resp.FailedOrders.Count + " Sales Failed to Download :");
									uC_Logger.AppendLogMsg(+resp.FailedOrders.Count + " Sales Failed to Download :");
									httpClient = RestClientUtil.getClient();
									string updatesalesOrderFailedStatus = ApiConstants.UPDATE_ORDER_STATUS_PENDING;
									// LogManager.WriteRequestContentLog(" Failed Content : " + resp.FailedOrders.Count + " - " + resp.FailedOrders.ToString(), updatesalesOrderFailedStatus);
									HttpContent content2 = new StringContent(JsonConvert.SerializeObject(resp.FailedOrders), Encoding.UTF8, "application/json");
									HttpResponseMessage updateResult = httpClient.PostAsync(updatesalesOrderFailedStatus, content2).Result;
								}
								if (resp.isLedgerMissmatch)
								{
									string joinedString = string.Join(" \n", resp.failedOrdersLineErrors);
									LogManager.WriteLog(resp.FailedOrders.Count + " Order Creation Failed \n Error : " + joinedString);
									uC_Logger.AppendLogMsg(resp.FailedOrders.Count + " Order Creation Failed \n Error : " + joinedString);
									UC_Download.showMessageToMasterUpdate(resp.FailedOrders.Count + " Order Creation Failed \n Error : " + joinedString);
								}
							}
						}
					}
					else
					{
						uC_Logger.AppendLogMsg("---------Nothing to Download------------");
					}

				}






				//return succesOrders;

			}
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw new ServiceException(ex.Message);
            }
        }


        private void updatesalesOrderStatus(List<string> succesOrders,UC_Logger uC_Logger)
        {
           //uC_Logger.AppendLogMsg(" Updating Staus ");

            string updatesalesOrderStatus =  ApiConstants.UPDATE_ORDER_STATUS;
            LogManager.WriteLog("updating  salesOrders status ....");
            httpClient = RestClientUtil.getClient();
            var myContent = JsonConvert.SerializeObject(succesOrders);
            HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");
                     LogManager.WriteRequestContentLog(" MyContent : "+myContent.Length+" - " +myContent.ToString(), updatesalesOrderStatus);
            var responseTask = httpClient.PostAsync(updatesalesOrderStatus, inputContent);


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
      


    
    public List<SalesOrderDTO> getSalesFromServer(VoucherType voucherType,DateTime? salesDate)
        {
            try
            {
                string serverAddress;
                List<SalesOrderDTO> salesOrderDTOs = new();
                 String formattedDate = salesDate.Value.ToString("yyyy-MM-dd");
              if(ApplicationProperties.properties["IsEnableDistributor"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    if (ApplicationProperties.properties["enable.date"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        var distcode = ApplicationProperties.properties["DistributedCode"].ToString();
                        serverAddress = ApiConstants.DOWNLOAD_ORDER_WITHDISTRIBUTED_CODE + "?voucherType=" + voucherType+"&DistributorCode="+distcode;
                    }
                    else if (ApplicationProperties.properties["enable.Selecteddate"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                             var distcode = ApplicationProperties.properties["DistributedCode"].ToString();
                            serverAddress = ApiConstants.DOWNLOAD_ORDER_WITHDISTRIBUTED_CODE + "?voucherType=" + voucherType+"&DistributorCode="+distcode+"&salesDate=" +formattedDate;
                       }
                     else
                    {
                        var distcode = ApplicationProperties.properties["DistributedCode"].ToString();
                        serverAddress = ApiConstants.DOWNLOAD_ORDER_WITHDISTRIBUTED_CODE + "?voucherType=" + voucherType + "&DistributorCode="+distcode;
                     }
                 }
                else
                {
                    if (ApplicationProperties.properties["Isoptimized"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        if (ApplicationProperties.properties["enable.date"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                             serverAddress=ApiConstants.DOWNLOAD_BY_VOUCHER_TYPE+"?voucherType=" + voucherType;
                        }
                        else if(ApplicationProperties.properties["enable.Selecteddate"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            serverAddress = ApiConstants.DOWNLOAD_BY_VOUCHER_TYPE + "?voucherType=" + voucherType + " &salesDate=" + formattedDate;
                        }
                        else
                        {
                            serverAddress=ApiConstants.DOWNLOAD_BY_VOUCHER_TYPE + "?voucherType=" + voucherType;
                        }

                    }
                    else
                    {
                        if (ApplicationProperties.properties["enable.date"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                        {

                            serverAddress=ApiConstants.DOWNLOAD_BY_VOUCHER_TYPE_NOT_OPTIMIZED+"?voucherType=" + voucherType;
                        }
                        else if (ApplicationProperties.properties["enable.Selecteddate"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            serverAddress = ApiConstants.DOWNLOAD_BY_VOUCHER_TYPE_NOT_OPTIMIZED + "?voucherType=" + voucherType + " &salesDate=" + formattedDate;
                        }
                        else
                        {
                            serverAddress=ApiConstants.DOWNLOAD_BY_VOUCHER_TYPE_NOT_OPTIMIZED+"?voucherType=" + voucherType;
                        }


                    }

                        
                }
                   
             
                LogManager.WriteLog("which api" + serverAddress);
                // test code  testing
                // serverAddress="http://localhost:3000/salesorder";

                LogManager.WriteLog("downloading salesOrders from server....");
                httpClient = RestClientUtil.getClient();


                 var responseTask = httpClient.GetAsync(serverAddress);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteResponseLog(Res);

                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("request for downloading salesOrders    Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                    if(!(string.IsNullOrEmpty(response)))
                    {
                        salesOrderDTOs = JsonConvert.DeserializeObject<List<SalesOrderDTO>>(response);
                        LogManager.WriteLog(salesOrderDTOs.ToString());
                    }

                    return salesOrderDTOs;
                }
                else
                {
                    LogManager.WriteLog("request for downloading salesOrders Failed..");
                    LogManager.WriteLog("downloading salesOrders Failed statuscode:" + Res.StatusCode + " Message : " + Res.RequestMessage);

                    throw new ServiceException("downloading salesOrders Failed statuscode: " + Res.StatusCode );

                    return salesOrderDTOs;
                }
            }catch(Exception ex)
            {
                LogManager.WriteLog("request for downloading salesOrders Failed...\n Exception details : " + ex.Message);
                LogManager.HandleException(ex);
                throw ex;
            }    
        }


    }
}

       
  
