using Microsoft.VisualBasic.Logging;
using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
using SNR_ClientApp.Tally.generateXml;
using SNR_ClientApp.TallyResponses;
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
    public class DownloadJournalService
    {
        private String enableReceiptVoucheType =ApplicationProperties.properties["enable.receipt.voucherType"].ToString();
        private String disableChequeueEntry = ApplicationProperties.properties["download.disable.chequeue.entry"].ToString();
        JournalGenerateXml journalGenerateXml;
        TransactionProcessor transactionProcessor;
        public DownloadJournalService()
        {
            journalGenerateXml=new JournalGenerateXml();
            transactionProcessor = new TransactionProcessor();
        }
        internal async Task getFromServerAndDownloadToTallyAsync(DateTime? saleDate,UC_Logger? uC_Logger)
        {
            List<ReceiptDTO> receiptDTOs = new List<ReceiptDTO>();
            int batchCount = 0;
            int totalSuccessCount = 0;
            int totalFailureCount = 0;
            do
            {
                try
                {
                    uC_Logger.AppendLogMsg("Please Wait.. Processing Transactions.. ");
                    DownloadReceiptService downloadReceiptService = new DownloadReceiptService();
                 receiptDTOs = downloadReceiptService.getReceiptsFromServer(saleDate);
                    List<String> succesReceiptpids = new List<string>();
                    List<TallyXml> tallyXmls = new List<TallyXml>();
                    if (receiptDTOs.Count > 0)
                    {
                        batchCount++;
                        if (ApplicationProperties.properties["Isoptimized"].ToString() == "True")
                        {
                            uC_Logger.AppendLogMsg("Batch -" + batchCount + "- Downloading");
                        }
                     
                        var receiptHeaderGroup = receiptDTOs.GroupBy(r => r.accountingVoucherHeaderPid).ToDictionary(g => g.Key, g => g.ToList());

                        foreach (var entry in receiptHeaderGroup)
                        {
                            string key = entry.Key;
                            List<ReceiptDTO> receiptListValue = entry.Value;

                            List<ReceiptDTO> tempAllocationList = receiptListValue.Where(receipt => receipt.reference != null && !string.IsNullOrEmpty(receipt.reference)).ToList();

                            foreach (ReceiptDTO receipt in receiptListValue)
                            {
                                List<ReceiptAllocationDTO> receiptAllocDtos = new List<ReceiptAllocationDTO>();
                                foreach (ReceiptDTO alloc in tempAllocationList)
                                {
                                    if (alloc.detailId == receipt.detailId)
                                    {
                                        ReceiptAllocationDTO receiptAllocDTO = new ReceiptAllocationDTO();
                                        receiptAllocDTO.reference = alloc.reference;
                                        receiptAllocDTO.amount = alloc.amount;
                                        receiptAllocDTO.provisionalReceiptNo = alloc.provisionalReceiptNo;
                                        string narration_message = alloc.narrationMessage;
                                        receiptAllocDTO.remarks = alloc.reference + " ,  " + narration_message;
                                        receiptAllocDtos.Add(receiptAllocDTO);
                                    }
                                }
                                receipt.receiptAllocationList = receiptAllocDtos;
                            }

                            foreach (ReceiptDTO receipt in receiptListValue)
                            {
                                List<ReceiptAllocationDTO> receiptAllocationDtos = receipt.receiptAllocationList;
                                if (receiptAllocationDtos != null && receiptAllocationDtos.Count > 0)
                                {
                                    receipt.amount = receipt.detailAmount;

                                    string narration_message = string.Join(" , ", receiptAllocationDtos.Select(ral => ral.remarks));
                                    //narration_message = narration_message.Replace("&", "&amp;");
                                    receipt.narrationMessage = narration_message;

                                    var provisionalReceiptNo = receiptAllocationDtos.Select(ral => ral.provisionalReceiptNo).Distinct();
                                    receipt.provisionalReceiptNo = provisionalReceiptNo.First();
                                }
                            }
                        }

                        var detailIds = new HashSet<long>(receiptDTOs.Select(receipt => receipt.detailId));
                        List<ReceiptDTO> receiptTempList = new List<ReceiptDTO>();
                        foreach (var id in detailIds)
                        {
                            var tempDtos = receiptDTOs.Where(receipt => receipt.detailId == id).ToList();
                            if (tempDtos.Any())
                            {
                                receiptTempList.Add(tempDtos[0]);
                            }
                        }
                        if (receiptTempList.Any())
                        {
                            receiptDTOs = new List<ReceiptDTO>(receiptTempList);
                        }
                        if (enableReceiptVoucheType.Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            receiptDTOs.Sort((r1, r2) => r1.employeeName.CompareTo(r2.employeeName));
                        }

                        foreach (ReceiptDTO receiptDTO in receiptDTOs)
                        {
                            if ((disableChequeueEntry == "True"))
                            {
                                if ((receiptDTO.mode != PaymentMode.Bank))
                                {
                                    ENVELOPE receiptXml = journalGenerateXml.generateJournalXml(receiptDTO);
                                    //System.out.println(receiptXml);
                                    tallyXmls.Add(new TallyXml(receiptDTO.accountingVoucherHeaderPid, receiptXml));
                                }
                            }
                            else
                            {

                                ENVELOPE receiptXml = journalGenerateXml.generateJournalXml(receiptDTO);
                                //System.out.println(receiptXml);
                                tallyXmls.Add(new TallyXml(receiptDTO.accountingVoucherHeaderPid, receiptXml));
                            }
                        }

                        LogManager.WriteLog("Receipts  Generated");
                        TallyResponse succesReceipts = await transactionProcessor.postReceiptsToTallyAsync(tallyXmls);
                        LogManager.WriteLog("Receipts posted to server");
                        List<String> allSuccesReceiptpid = new();
                        //List<String> allSuccesReceiptpid = (List<String>)succesReceipts.body;
                        DownloadResponseDto resp = succesReceipts.body as DownloadResponseDto;
                        allSuccesReceiptpid = resp.SuccessOrders;
                        succesReceiptpids.AddRange(allSuccesReceiptpid);
                        LogManager.WriteLog("Status updating for " + succesReceiptpids.Count + " Receipts");

                        if (succesReceiptpids.Count > 0)
                        {
                            totalSuccessCount = succesReceiptpids.Count + totalSuccessCount;
                            LogManager.WriteLog(+succesReceiptpids.Count + " Sales  downloaded");
                            updateReceiptSuccessStatus(succesReceiptpids);
                        }
                        if (resp.FailedOrders.Count > 0)
                        {
                            totalFailureCount = totalFailureCount + resp.FailedOrders.Count;
                            LogManager.WriteLog(+resp.FailedOrders.Count + " Sales failed to downloaded");
                            HttpClient httpClient = new HttpClient();
                            httpClient = RestClientUtil.getClient();
                            string updatesalesOrderFailedStatus = ApiConstants.UPDATE_RECEIPT_STATUS_PENDING;
                            HttpContent content2 = new StringContent(JsonConvert.SerializeObject(resp.FailedOrders), Encoding.UTF8, "application/json");
                            HttpResponseMessage updateResult = httpClient.PostAsync(updatesalesOrderFailedStatus, content2).Result;
                        }
                        if (resp.isLedgerMissmatch)
                        {
                            string joinedString = string.Join(" \n", resp.failedOrdersLineErrors);
                            UC_Download.showMessageToMasterUpdate(resp.FailedOrders.Count + " Download Failed \n Error : " + joinedString);
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
                                uC_Logger.AppendLogMsg(+totalSuccessCount + "Sales Downloaded Successfully");
                                uC_Logger.AppendLogMsg(+totalFailureCount + " Sales Failed to Download");
                                uC_Logger.AppendLogMsg(" Process Completed");
                            }


                        
                    }

                  
                }
                catch (Exception ex)
                {

                    LogManager.HandleException(ex);
                    throw ex;
                }
            } while (receiptDTOs.Count > 0);
            
          
        }

        private void updateReceiptSuccessStatus(List<string> succesReceipts)
        {
            string updateReceiptStatus = ApiConstants.UPDATE_RECEIPT_STATUS;
            List<String> succesReceiptpids = new List<String>();
            List<String> allSuccesReceiptpid = (List<String>)succesReceipts;
            succesReceiptpids.AddRange(allSuccesReceiptpid);
            LogManager.WriteLog("Status updated for " + succesReceiptpids.Count + " Receipts");

            if (succesReceiptpids.Count > 0)
            {

                try
                {
                    LogManager.WriteLog("Uploading succesReceiptpids To Server ....");


                    HttpClient httpClient = new();
                    LogManager.WriteLog("uploading succesReceiptpids started...\n" + "Api  : " + updateReceiptStatus);
                    httpClient = RestClientUtil.getClient();
                    var myContent = JsonConvert.SerializeObject(succesReceiptpids);
                    HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

                    var responseTask = httpClient.PostAsync(updateReceiptStatus, inputContent);

                    responseTask.Wait();

                    HttpResponseMessage Res = responseTask.Result;
                    LogManager.WriteLog("Uploading succesReceiptpids To Server Completed ....\n Response : ");
                    LogManager.WriteResponseLog(Res);

                    if (Res.IsSuccessStatusCode)
                    {
                        LogManager.WriteLog("request for uploading succesReceiptpids  Success..");
                        var response = Res.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        LogManager.WriteLog("request for uploading succesReceiptpids  Failed..");
                    }
                }
                catch (Exception ex)
                {
                    LogManager.HandleException(ex);
                    throw ex;
                }
            }










            //LogManager.WriteLog("updating  ReceiptSuccess status ....");
            //HttpClient httpClient = new HttpClient();
            //httpClient = RestClientUtil.getClient();


            //var responseTask = httpClient.GetAsync(updatesalesOrderStatus);

            //responseTask.Wait();

            //HttpResponseMessage Res = responseTask.Result;
            //LogManager.WriteResponseLog(Res);

            //if (Res.IsSuccessStatusCode)
            //{
            //    LogManager.WriteLog("request for updating  ReceiptSuccess  status  Success..");
            //    var response = Res.Content.ReadAsStringAsync().Result;

            //}
            //else
            //{
            //    LogManager.WriteLog("request for updating  ReceiptSuccess status Failed..");

            //}
        }
    }
}
