using dtos;
using Microsoft.VisualBasic.Logging;
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
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    public class DownloadReceiptService
    {

        TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        private TransactionProcessor transactionProcessor;

        private String receiptVoucherType = ApplicationProperties.properties["receipt.voucher.type"].ToString();


    private String receiptVoucherTypeBank = ApplicationProperties.properties["receipt.voucher.type.bank"].ToString();


    private String receiptVoucherTypeCash = ApplicationProperties.properties["receipt.voucher.type.cash"].ToString();


    private String receiptBankName = ApplicationProperties.properties["receipt.voucher.bank.name"].ToString();


    private String transactionType = ApplicationProperties.properties["transaction.type"].ToString();


    private String enableCostCentre = ApplicationProperties.properties["enable.cost.centre"].ToString();


        private String costCentreCashReceipts = ApplicationProperties.properties["enable.cost.centre.cash.receipts"].ToString();

    private String employeeWiseLedger = ApplicationProperties.properties["enable.receipt.employeewise.ledger"].ToString();

    private String showProvisionalNo = ApplicationProperties.properties["show.provisional.no"].ToString();

    private String isOptionalReceipt = ApplicationProperties.properties["is.optional.receipt"].ToString();

    private String godownInReceipt = ApplicationProperties.properties["godown.in.sales.receipt"].ToString();
        private string PaymentModeRemarks = ApplicationProperties.properties["PaymentModeRemarks"].ToString();
        String companyName= ApplicationProperties.properties["tally.company"].ToString();
        String enableReceiptVoucheType = ApplicationProperties.properties["enable.receipt.voucherType"].ToString();
        private String disableChequeueEntry = ApplicationProperties.properties["download.disable.chequeue.entry"].ToString();
        ReceiptGenerateXml receiptGenerateXml = new ReceiptGenerateXml();
        public DownloadReceiptService()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            transactionProcessor = new();
        }

        public async Task  getFromServerAndDownloadToTallyAsync(UC_Logger? uC_Logger,DateTime? salesDate)
        {
            try
            {
                List<ReceiptDTO> receiptDtos = new List<ReceiptDTO>();
                DownloadResponseDto resp = new();
                int batchcount = 0;
                int totalSuccessCount = 0;
                int totalFailureCount = 0;
                do
                {

                    uC_Logger.AppendLogMsg("Please Wait.. Processing Transactions.. ");
                    receiptDtos = getReceiptsFromServer(salesDate);
                    if (receiptDtos.Count > 0)
                    {
                        batchcount++;
                        uC_Logger.AppendLogMsg("Batch-" + batchcount + "is downloading");

                        List<String> succesReceiptpids = new List<String>();
                        var receiptHeaderGroup = receiptDtos.GroupBy(r => r.accountingVoucherHeaderPid).ToDictionary(g => g.Key, g => g.ToList());
                        foreach (var entry in receiptHeaderGroup)
                        {
                            string key = entry.Key;
                            List<ReceiptDTO> receiptListValue = entry.Value;

                            var tempAllocationList = receiptListValue.Where(r => r.reference != null && !string.IsNullOrEmpty(r.reference)).ToList();

                            foreach (ReceiptDTO receipt in receiptListValue)
                            {
                                var receiptAllocDtos = new List<ReceiptAllocationDTO>();
                                foreach (ReceiptDTO alloc in tempAllocationList)
                                {
                                    if (alloc.detailId == receipt.detailId)
                                    {
                                        var receiptAllocDTO = new ReceiptAllocationDTO();
                                        receiptAllocDTO.reference = alloc.reference;
                                        receiptAllocDTO.amount = alloc.amount;
                                        receiptAllocDTO.provisionalReceiptNo = alloc.provisionalReceiptNo;
                                        var narration_message = alloc.narrationMessage;
                                        //narration_message = narration_message.Replace("&", "&amp;");
                                        receiptAllocDTO.remarks = alloc.reference + " , " + narration_message;
                                        receiptAllocDtos.Add(receiptAllocDTO);
                                    }
                                }
                                receipt.receiptAllocationList = receiptAllocDtos;
                            }

                            foreach (ReceiptDTO receipt in receiptListValue)
                            {
                                var receiptAllocationDtos = receipt.receiptAllocationList;
                                if (receiptAllocationDtos != null && receiptAllocationDtos.Count > 0)
                                {
                                    receipt.amount = receipt.detailAmount;

                                    var narration_message = string.Join(" , ", receiptAllocationDtos.Select(ral => ral.remarks));
                                    //narration_message = narration_message.Replace("&", "&amp;");
                                    receipt.narrationMessage = narration_message;

                                    var provisionalReceiptNo = receiptAllocationDtos.Select(ral => ral.provisionalReceiptNo).ToHashSet();
                                    receipt.provisionalReceiptNo = provisionalReceiptNo.FirstOrDefault();


                                }
                            }
                        }

                        var detailIds = receiptDtos.Select(r => r.detailId).Distinct().ToHashSet();

                        var receiptTempList = new List<ReceiptDTO>();
                        foreach (long id in detailIds)
                        {
                            var tempDtos = receiptDtos.Where(r => r.detailId == id).ToList();
                            if (tempDtos.Count > 0)
                            {
                                receiptTempList.Add(tempDtos[0]);
                            }
                        }

                        if (receiptTempList.Count > 0)
                        {
                            receiptDtos = receiptTempList;
                        }

                        if (enableReceiptVoucheType.Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            receiptDtos = receiptDtos.OrderBy(r => r.employeeName).ToList();
                        }
                        List<TallyXml> tallyXmls = new List<TallyXml>();

                        foreach (ReceiptDTO receiptDTO in receiptDtos)
                        {
                            if ((disableChequeueEntry == "True"))
                            {
                                if ((receiptDTO.mode != PaymentMode.Bank))
                                {
                                    ENVELOPE TallyXmlRequestObj = generateXmlRequestObj(receiptDTO);
                                    TallyXml tallyXml = new TallyXml();
                                    tallyXml.pid = receiptDTO.accountingVoucherHeaderPid;
                                    tallyXml.xmlObj = TallyXmlRequestObj;
                                    tallyXmls.Add(tallyXml);
                                }

                            }
                            else
                            {
                                ENVELOPE TallyXmlRequestObj = generateXmlRequestObj(receiptDTO);
                                TallyXml tallyXml = new TallyXml();
                                tallyXml.pid = receiptDTO.accountingVoucherHeaderPid;
                                tallyXml.xmlObj = TallyXmlRequestObj;
                                tallyXmls.Add(tallyXml);
                            }
                        }
                        var res = await postReceiptsToTallyAsync(tallyXmls);
                        if (res.status == "OK")
                        {
                            List<String> allSuccesReceiptpid = new();
                            //List<String> allSuccesReceiptpid = (List<String>)succesReceipts.body;
                            resp = res.body as DownloadResponseDto;
                            allSuccesReceiptpid = resp.SuccessOrders;
                            uploadSuccesReceptsToServer(allSuccesReceiptpid);
                            if (resp.SuccessOrders.Count > 0)
                            {
                                totalSuccessCount = totalSuccessCount + resp.SuccessOrders.Count;
                                //LogManager.WriteLog(+resp.SuccessOrders.Count + " Reciept is downloaded");
                                //uC_Logger.AppendLogMsg(+resp.SuccessOrders.Count + " Reciept is downloaded");
                            }
                            if (resp.FailedOrders.Count > 0)
                            {
                                totalFailureCount = resp.FailedOrders.Count;
                                HttpClient httpClient = new HttpClient();
                                httpClient = RestClientUtil.getClient();
                                string updatesalesOrderFailedStatus = ApiConstants.UPDATE_RECEIPT_STATUS_PENDING;
                                HttpContent content2 = new StringContent(JsonConvert.SerializeObject(resp.FailedOrders), Encoding.UTF8, "application/json");
                                HttpResponseMessage updateResult = httpClient.PostAsync(updatesalesOrderFailedStatus, content2).Result;
                            }
                            if (resp.isLedgerMissmatch)
                            {
                                string joinedString = string.Join(" \n", resp.failedOrdersLineErrors);
                                UC_Download.showMessageToMasterUpdate(resp.FailedOrders.Count + " Receipt Creation Failed \n Error : " + joinedString);
                            }
                        }


                    }
                    else
                    {
                        if (batchcount == 0)
                        {
                            uC_Logger.AppendLogMsg(" No Orders to download");
                        }
                        else
                        {
                            uC_Logger.AppendLogMsg(+totalSuccessCount + "Reciepts Downloaded Successfully");
                            uC_Logger.AppendLogMsg(+totalFailureCount + " Reciepts Failed to Download");
                            uC_Logger.AppendLogMsg(" Process Completed");
                        }

                    }

                } while (receiptDtos.Count > 0);
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
           


        }

                    
                
            
            

        public void uploadSuccesReceptsToServer(object succesReceipts)
        {
            string updateReceiptStatus =  ApiConstants.UPDATE_RECEIPT_STATUS;
            List<String> succesReceiptpids = new List<String>();
            List<String> allSuccesReceiptpid = (List<String>)succesReceipts;
            succesReceiptpids.AddRange(allSuccesReceiptpid);
            LogManager.WriteLog("Status updated for " + succesReceiptpids.Count + " Receipts");

            if (succesReceiptpids.Count > 0)
            {

                try
                {
                    LogManager.WriteLog("Uploading succesReceiptpids To Server ....");



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
           
        }

        public async Task<TallyResponse> postReceiptsToTallyAsync(List<TallyXml> receipts)
        {
            LogManager.WriteLog("postReceiptsToTally ................" + receipts.Count);
            List<String> successReceipts = new List<String>();
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
                            LogManager.WriteLog("Receipt post Failed For ............." + tallyRequest.pid + "\n LineError : " + data.response.LINEERROR);

							if (!String.IsNullOrEmpty(data.response.LINEERROR))
							{
								string mainString = data.response.LINEERROR;
								string pattern = "(?=.*Ledger)(?=.*does not exist)";

								if (Regex.IsMatch(mainString, pattern))
								{
									isLedgerMissmatch=true;
									
									//UC_Download.showMessage("Order Creation Failed \n"+data.response.LINEERROR);
								}
								failedLineErrors.Add(mainString);
								//if (data.response.LINEERROR.Contains("sdfdsf")
							}

							throw new ServiceException(data.response.LINEERROR);
                        }
                        else
                        {
                            LogManager.WriteLog("Receipt post success .............." + tallyRequest.pid);
                            successReceipts.Add(tallyRequest.pid);
                        }
                    }
                    else
                    {
                        LogManager.WriteLog("Receipt post failed  ......." );
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
                    throw ex;
                    // return new TallyResponse("OK", "receipt post to tally failed", successReceipts);
                }
            }
			DownloadResponseDto resp = new DownloadResponseDto(successReceipts, failedReceipts);
			resp.failedOrdersLineErrors= failedLineErrors;
			resp.isLedgerMissmatch= isLedgerMissmatch;
            resp.TotalCount=receipts.Count;

            return new TallyResponse("OK", "receipt post to tally successfully", resp);
			//return new TallyResponse("OK", "receipt post to tally successfully", successReceipts);
        }

        public List<ReceiptDTO> getReceiptsFromServer(DateTime? salesDate)
        {

            try
            {
                List<SalesOrderDTO> salesOrderDTOs = new();
                String formattedDate = salesDate.Value.ToString("yyyy-MM-dd");

                string serverAddress;
                if (ApplicationProperties.properties["Isoptimized"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    if (ApplicationProperties.properties["enable.Selecteddate"].ToString().Equals("true",StringComparison.OrdinalIgnoreCase))
                    {
                        serverAddress = ApiConstants.DOWNLOAD_RECEIPT + "?salesDate=" + formattedDate;
                        string updateReceiptStatus = ApiConstants.UPDATE_RECEIPT_STATUS + "?salesDate=" + formattedDate;
                    }
                    else
                    {
                        serverAddress = ApiConstants.DOWNLOAD_RECEIPT;
                        string updateReceiptStatus = ApiConstants.UPDATE_RECEIPT_STATUS;
                    }
                }
                else
                {
                    serverAddress = ApiConstants.DOWNLOAD_RECEIPT_NOT_OPTIMIZED;
                }
                 
              

                LogManager.WriteLog("downloading receipts from server....");
                httpClient = RestClientUtil.getClient();


                var responseTask = httpClient.GetAsync(serverAddress);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteResponseLog(Res);

                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("request for downloading receipts    Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                    List<ReceiptDTO> receiptsList = JsonConvert.DeserializeObject<List<ReceiptDTO>>(response);
                    return receiptsList;
                }
                else
                {
                    LogManager.WriteLog("request for downloading receipts Failed..");
                    throw new ServiceException("request for downloading receipts Failed \n status code : " + Res.StatusCode);
                    return null;
                }
            }catch(Exception ex)
            {
                LogManager.HandleException(ex);
                LogManager.WriteLog(ex.Message);
                throw ex;
            }
            return null;
        }

        private ENVELOPE generateXmlRequestObj(ReceiptDTO receiptDTO)
        {
            if (enableReceiptVoucheType.Equals("true", StringComparison.OrdinalIgnoreCase) && receiptDTO.employeeAlias!= null)
            {
                receiptVoucherTypeCash = receiptDTO.employeeAlias.Replace("&", "&").Split("~")[0];
                receiptVoucherTypeBank = receiptDTO.employeeAlias.Replace("&", "&").Split("~")[0];
                LogManager.WriteLog("enableReceiptVoucheType : TRUE\nEmployee Receipt Voucher Type: " + receiptDTO.employeeAlias.Replace("&", "&"));
            }
                string newUuid = Guid.NewGuid().ToString();

                string ledgerName = receiptDTO.ledgerName;
            String companyName = CompanyService.getCompanyName();
            char[] str = (newUuid).ToCharArray();
                string RemoteId = KeyGeneratorUtil.GetRandomCustomString(str, 6) + "-" + ledgerName;
                String dates = receiptDTO.date;
                //convert date to yyyymmdd format 
                string date = StringUtilsCustom.convertDateToString(dates);
            string receiptDate = ApplicationProperties.properties["receiptdate"].ToString();
            if (!receiptDate.Equals(""))
                {
                date = StringUtilsCustom.convertDateToString(receiptDate);
            }

            bool isOptional = true;

                if (isOptionalReceipt.Equals("false", StringComparison.OrdinalIgnoreCase))
                {
                    isOptional = false;
                }

                double totalAmount = 0;

                ENVELOPE tallyRequest = new ENVELOPE();
                HEADER header = new HEADER();
             
                header.TALLYREQUEST = "Import Data";
                
                tallyRequest.HEADER = header;

                BODY body = new();
                IMPORTDATA importdata = new IMPORTDATA();
                REQUESTDESC requestDesc = new();
                STATICVARIABLES staticvariables = new STATICVARIABLES();
               
                staticvariables.SVCURRENTCOMPANY = ApplicationProperties.properties["tally.company"].ToString();

                requestDesc.STATICVARIABLES = staticvariables;
                requestDesc.REPORTNAME = "All Masters";
                importdata.REQUESTDESC = requestDesc;

                REQUESTDATA requestData = new();
                TALLYMESSAGE tallymessage = new();
                tallymessage.UDF = "TallyUDF";

                VOUCHER voucher = new VOUCHER();
                OLDAUDITENTRYIDSLIST oLDAUDITENTRYIDSLIST = new();
                voucher.REMOTEID = RemoteId;

                string narration_message = receiptDTO.narrationMessage;


                string provisionalReceiptNo = receiptDTO.provisionalReceiptNo!=""
                    ? " ,  Provisional Receipt No. :" + receiptDTO.provisionalReceiptNo
                    : "";

                if (showProvisionalNo != null)
                {
                    if (showProvisionalNo.Equals("false", StringComparison.OrdinalIgnoreCase))
                    {
                        provisionalReceiptNo = "";
                    }
                    else
                    {
                    if(receiptDTO.provisionalReceiptNo==" ")
                    {
                        provisionalReceiptNo = "";
                    }
                    }
                }

                //narration_message.Replace("&", "&amp;");

                if (receiptDTO.mode == PaymentMode.Cash)
                {
                    voucher.VCHKEY = "644ec99e-8f1b-4088-93f0-ac7269dc1b5f-0000a35d:00000048";
                    voucher.VCHTYPE = receiptVoucherTypeCash;
                    voucher.ACTION = "Create";
                    voucher.OBJVIEW = "Accounting Voucher View";
                   
                    oLDAUDITENTRYIDSLIST.TYPE = "Number";
                    oLDAUDITENTRYIDSLIST.OLDAUDITENTRYIDS = "-1";
                    voucher.OLDAUDITENTRYIDSLIST = oLDAUDITENTRYIDSLIST;
                    voucher.DATE = date;
                    voucher.GUID = "b40d21d7-a905-4cef-b58c-d6c58d9e32f8-00000015";
                if(PaymentModeRemarks.Equals("true",StringComparison.OrdinalIgnoreCase))
                    {
                    voucher.NARRATION = receiptDTO.narrationMessage == null ? "For Cash \n"
                   : "For Cash \n"+ narration_message + provisionalReceiptNo;
                }
                else
                {
                    voucher.NARRATION = receiptDTO.narrationMessage == null ? ""
                  : narration_message + provisionalReceiptNo;
                }
                  

                    voucher.VOUCHERTYPENAME = receiptVoucherTypeCash;
                    voucher.VOUCHERNUMBER = receiptDTO.provisionalReceiptNo != null ? receiptDTO.provisionalReceiptNo : "1";
                    //  

                }
                else if (receiptDTO.mode == PaymentMode.Bank)
                {
                    voucher.VCHKEY = "644ec99e-8f1b-4088-93f0-ac7269dc1b5f-0000a35d:00000048";
                    voucher.VCHTYPE = receiptVoucherTypeBank;
                    voucher.ACTION = "Create";
                    voucher.OBJVIEW = "Accounting Voucher View";
                  //  OLDAUDITENTRYIDSLIST oLDAUDITENTRYIDSLIST = new();
                    oLDAUDITENTRYIDSLIST.TYPE = "Number";
                    oLDAUDITENTRYIDSLIST.OLDAUDITENTRYIDS = "-1";
                    voucher.OLDAUDITENTRYIDSLIST = oLDAUDITENTRYIDSLIST;
                    voucher.DATE = date;
                    voucher.GUID = "b40d21d7-a905-4cef-b58c-d6c58d9e32f8-00000015";
                if (PaymentModeRemarks.Equals("true", StringComparison.OrdinalIgnoreCase))
                {

                    voucher.NARRATION ="For Cheque \n" +"Bank Name:" + receiptDTO.bankName + "   Cheque No:" + receiptDTO.chequeNo + "   Cheque Date:" + StringUtilsCustom.ConvertStringDateToDD_MM_YYYY(receiptDTO.chequeDate) +
                         narration_message + provisionalReceiptNo;
                }
                else
                {
                    voucher.NARRATION = "Bank Name:" + receiptDTO.bankName + "   Cheque No:" + receiptDTO.chequeNo + "   Cheque Date:" + StringUtilsCustom.ConvertStringDateToDD_MM_YYYY(receiptDTO.chequeDate) +
                      narration_message + provisionalReceiptNo;
                }
                   

                    voucher.VOUCHERTYPENAME = receiptVoucherTypeBank;
                    voucher.VOUCHERNUMBER = (receiptDTO.provisionalReceiptNo != null ? receiptDTO.provisionalReceiptNo : "1");

                }
                else if (receiptDTO.mode == PaymentMode.RTGS)
                {
                    voucher.VCHKEY = "644ec99e-8f1b-4088-93f0-ac7269dc1b5f-0000a35d:00000048";
                    voucher.VCHTYPE = receiptVoucherTypeBank;
                    voucher.ACTION = "Create";
                    voucher.OBJVIEW = "Accounting Voucher View";
                  //  OLDAUDITENTRYIDSLIST oLDAUDITENTRYIDSLIST = new();
                    oLDAUDITENTRYIDSLIST.TYPE = "Number";
                    oLDAUDITENTRYIDSLIST.OLDAUDITENTRYIDS = "-1";
                    voucher.OLDAUDITENTRYIDSLIST = oLDAUDITENTRYIDSLIST;
                    voucher.DATE = date;
                    voucher.GUID = "b40d21d7-a905-4cef-b58c-d6c58d9e32f8-00000015";
                if (PaymentModeRemarks.Equals("true", StringComparison.OrdinalIgnoreCase))
                {

                    voucher.NARRATION ="For G-Pay \n"+ "Bank Name:" + receiptDTO.bankName + "   RTGS No:" + receiptDTO.chequeNo + "   RTGS Date:" + StringUtilsCustom.convertDateToString(receiptDTO.chequeDate) +
                          narration_message + provisionalReceiptNo;
                }
                else
                {
                    voucher.NARRATION = "Bank Name:" + receiptDTO.bankName + "   RTGS No:" + receiptDTO.chequeNo + "   RTGS Date:" + StringUtilsCustom.convertDateToString(receiptDTO.chequeDate) +
                       narration_message + provisionalReceiptNo;
                }

                    voucher.VOUCHERTYPENAME = receiptVoucherTypeBank;
                    voucher.VOUCHERNUMBER = (receiptDTO.provisionalReceiptNo != null ? receiptDTO.provisionalReceiptNo : "1");

                }

                voucher.PARTYLEDGERNAME = ledgerName;
                voucher.FBTPAYMENTTYPE = "Default";
                voucher.PERSISTEDVIEW = "Accounting Voucher View";
               // voucher.VCHGSTCLASS = "";
                voucher.DIFFACTUALQTY = "No";
                voucher.AUDITED = "No";
                voucher.FORJOBCOSTING = "No";
                voucher.ISOPTIONAL = (isOptional) ? "Yes" : "No";
                voucher.EFFECTIVEDATE = date;
                voucher.ISFORJOBWORKIN = "No";
                voucher.ALLOWCONSUMPTION = "No";
                voucher.USEFORINTEREST = "No";
                voucher.USEFORGAINLOSS = "No";
                voucher.USEFORGODOWNTRANSFER = "No";
                voucher.USEFORCOMPOUND = "No";
                voucher.ALTERID = 0;
                voucher.EXCISEOPENING = "No";
                voucher.USEFORFINALPRODUCTION = "No";
                voucher.ISCANCELLED = "No";
                voucher.HASCASHFLOW = "Yes";
                voucher.ISPOSTDATED = "No";
                voucher.USETRACKINGNUMBER = "No";
                voucher.ISINVOICE = "No";
                voucher.MFGJOURNAL = "No";
                voucher.HASDISCOUNTS = "No";
                voucher.ASPAYSLIP = "No";
                if (enableCostCentre.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    voucher.COSTCENTRENAME = receiptDTO.employeeName;
                    voucher.ISCOSTCENTRE = enableCostCentre;
                }
                voucher.ISSTXNONREALIZEDVCH = "No";
                voucher.ISEXCISEMANUFACTURERON = "No";
                voucher.ISBLANKCHEQUE = "No";
                voucher.ISVOID = "No";
                voucher.ISONHOLD = "No";
                voucher.ISDELETED = "No";
                voucher.ASORIGINAL = "No";
                voucher.VCHISFROMSYNC = "No";
                voucher.MASTERID = 0;
                voucher.VOUCHERKEY = "179224690294837";
                //skipping empty tags 

                totalAmount = receiptDTO.amount;


                //generate individual receipt 

                StringBuilder builder = new StringBuilder();
                string trimChar = receiptDTO.trimChar == null ? "" : receiptDTO.trimChar;
                string ledgerName1 = StringUtilsCustom.replaceSpecialCharactersWithXmlValue(receiptDTO.particularsName) + trimChar;
                ALLLEDGERENTRIESLIST aLLLEDGERENTRIESLIST = new ALLLEDGERENTRIESLIST();
               // OLDAUDITENTRYIDSLIST oLDAUDITENTRYIDSLIST1 = new();
                oLDAUDITENTRYIDSLIST.TYPE = "Number";
                oLDAUDITENTRYIDSLIST.OLDAUDITENTRYIDS = "-1";

                aLLLEDGERENTRIESLIST.OLDAUDITENTRYIDSLIST = oLDAUDITENTRYIDSLIST;
                aLLLEDGERENTRIESLIST.LEDGERNAME = receiptDTO.particularsName;
                aLLLEDGERENTRIESLIST.GSTCLASS = "";
                aLLLEDGERENTRIESLIST.ISDEEMEDPOSITIVE = "No";
                aLLLEDGERENTRIESLIST.LEDGERFROMITEM = "No";
                aLLLEDGERENTRIESLIST.REMOVEZEROENTRIES = "No";
                aLLLEDGERENTRIESLIST.ISPARTYLEDGER = "Yse";
                aLLLEDGERENTRIESLIST.ISLASTDEEMEDPOSITIVE = "No";
                aLLLEDGERENTRIESLIST.AMOUNT = receiptDTO.amount;
                if (receiptDTO.receiptAllocationList != null && receiptDTO.receiptAllocationList.Count != 0)
                {
                    foreach (ReceiptAllocationDTO allocDto in receiptDTO.receiptAllocationList)
                    {
                        aLLLEDGERENTRIESLIST.BANKALLOCATIONSLIST = new BANKALLOCATIONSLIST();
                        BILLALLOCATIONSLIST billAllocationList = new BILLALLOCATIONSLIST();
                        billAllocationList.NAME = allocDto.reference;
                        billAllocationList.BILLTYPE = "Agst Ref";
                        billAllocationList.TDSDEDUCTEEISSPECIALRATE = "No";
                        billAllocationList.AMOUNT = allocDto.amount.ToString()  ;
                        if (godownInReceipt != null && godownInReceipt != "" && !godownInReceipt.Equals("")
                    && godownInReceipt.Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            if (receiptDTO.godownName != null && !receiptDTO.godownName.Equals(""))
                            {
                                PPSIVCHSALESMANLIST pPSIVCHSALESMANLIST = new PPSIVCHSALESMANLIST();
                                pPSIVCHSALESMANLIST.DESC = "PPSIVCHSalesMan";
                                pPSIVCHSALESMANLIST.ISLIST = "Yes";
                                pPSIVCHSALESMANLIST.TYPE = "String";
                                pPSIVCHSALESMANLIST.INDEX = "103";

                                PPSIVCHSALESMAN pPSIVCHSALESMAN = new();
                                pPSIVCHSALESMAN.DESC = "PPSIVCHSalesMan";
                                pPSIVCHSALESMAN.Text = receiptDTO.godownName;
                                pPSIVCHSALESMANLIST.PPSIVCHSALESMAN = pPSIVCHSALESMAN;
                                billAllocationList.PPSIVCHSALESMANLIST = pPSIVCHSALESMANLIST;
                            }
                        }
                        // INTERESTCOLLECTIONLIST 

                        aLLLEDGERENTRIESLIST.BILLALLOCATIONSLIST = billAllocationList;

                    }


                }

                //<INTERESTCOLLECTION.LIST></INTERESTCOLLECTION.LIST><OLDAUDITENTRIES.LIST>       </OLDAUDITENTRIES.LIST><ACCOUNTAUDITENTRIES.LIST></ACCOUNTAUDITENTRIES.LIST><AUDITENTRIES.LIST></AUDITENTRIES.LIST><TAXBILLALLOCATIONS.LIST></TAXBILLALLOCATIONS.LIST><TAXOBJECTALLOCATIONS.LIST></TAXOBJECTALLOCATIONS.LIST><TDSEXPENSEALLOCATIONS.LIST></TDSEXPENSEALLOCATIONS.LIST><VATSTATUTORYDETAILS.LIST></VATSTATUTORYDETAILS.LIST><COSTTRACKALLOCATIONS.LIST></COSTTRACKALLOCATIONS.LIST></ALLLEDGERENTRIES.LIST>

                OLDAUDITENTRYIDSLIST oLDAUDITENTRYIDSLIST2 = new OLDAUDITENTRYIDSLIST();
                oLDAUDITENTRYIDSLIST2.TYPE = "Number";
                oLDAUDITENTRYIDSLIST2.OLDAUDITENTRYIDS = "-1";
                ALLLEDGERENTRIESLIST aLLLEDGERENTRIESLIST2= new();
                aLLLEDGERENTRIESLIST2.OLDAUDITENTRIESLIST = oLDAUDITENTRYIDSLIST2;

                if (receiptDTO.mode == PaymentMode.Cash)
                {

                    if (employeeWiseLedger.Equals("true", StringComparison.OrdinalIgnoreCase)
                            && receiptDTO.employeeAlias != null)
                    {
                    var splitarray = receiptDTO.employeeAlias.Split("~");
                    if(splitarray.Length >2)
                        aLLLEDGERENTRIESLIST2.LEDGERNAME = splitarray[2];
                    else
                    aLLLEDGERENTRIESLIST2.LEDGERNAME = receiptDTO.employeeAlias.Split("~")[1];
                    }
                    else
                    {
                        aLLLEDGERENTRIESLIST2.LEDGERNAME = "Cash";
                    }
                }
                else if (receiptDTO.mode == PaymentMode.Bank)
                {
                    aLLLEDGERENTRIESLIST2.LEDGERNAME =  receiptBankName ;
                }
                else if (receiptDTO.mode == PaymentMode.RTGS)
                {
                    aLLLEDGERENTRIESLIST2.LEDGERNAME = receiptBankName;
                }

            if (costCentreCashReceipts.Equals("true",StringComparison.OrdinalIgnoreCase))
            {
                if (aLLLEDGERENTRIESLIST2.LEDGERNAME.Equals("cash", StringComparison.OrdinalIgnoreCase))
                {
                    COSTCENTREALLOCATIONSLIST cOSTCENTREALLOCATIONSLIST = new();
                    cOSTCENTREALLOCATIONSLIST.NAME = receiptDTO.employeeName;
                    cOSTCENTREALLOCATIONSLIST.AMOUNT = (0 - receiptDTO.amount).ToString();
                    CATEGORYALLOCATIONSLIST cATEGORYALLOCATIONSLIST = new();
                    cATEGORYALLOCATIONSLIST.COSTCENTREALLOCATIONSLIST = cOSTCENTREALLOCATIONSLIST;
                    aLLLEDGERENTRIESLIST2.CATEGORYALLOCATIONSLIST = cATEGORYALLOCATIONSLIST;
                }
            }
            


                aLLLEDGERENTRIESLIST2.ISDEEMEDPOSITIVE = "Yes";

                aLLLEDGERENTRIESLIST2.LEDGERFROMITEM = "No";
                aLLLEDGERENTRIESLIST2.REMOVEZEROENTRIES = "No";

                aLLLEDGERENTRIESLIST2.ISPARTYLEDGER = "Yes";
                aLLLEDGERENTRIESLIST2.ISLASTDEEMEDPOSITIVE = "Yes";
                aLLLEDGERENTRIESLIST2.AMOUNT =0- totalAmount;

                BANKALLOCATIONSLIST bANKALLOCATIONSLIST = new();
                bANKALLOCATIONSLIST.DATE=StringUtilsCustom.convertDateToString(receiptDTO.chequeDate);
                bANKALLOCATIONSLIST.INSTRUMENTDATE = StringUtilsCustom.convertDateToString(receiptDTO.chequeDate);
                bANKALLOCATIONSLIST.BANKNAME = receiptDTO.bankName;
                bANKALLOCATIONSLIST.INSTRUMENTNUMBER = receiptDTO.chequeNo;
                bANKALLOCATIONSLIST.TRANSACTIONTYPE = transactionType;
                aLLLEDGERENTRIESLIST2.BANKALLOCATIONSLIST = bANKALLOCATIONSLIST;
                List<ALLLEDGERENTRIESLIST> aLLLEDGERENTRIESLISTsList = new List<ALLLEDGERENTRIESLIST>();
                aLLLEDGERENTRIESLISTsList.Add(aLLLEDGERENTRIESLIST);
                aLLLEDGERENTRIESLISTsList.Add(aLLLEDGERENTRIESLIST2);
                voucher.ALLLEDGERENTRIES_LIST = aLLLEDGERENTRIESLISTsList;
                tallymessage.VOUCHER = voucher;
                requestData.TALLYMESSAGE = new List<TALLYMESSAGE>
                {
                    tallymessage
                };
                importdata.REQUESTDATA = requestData;
                body.IMPORTDATA = importdata;
                
                tallyRequest.BODY = body;

                return tallyRequest;


        }

        internal async Task<DownloadResponseDto> getFromServerAndDownloadToTallyDailyReceipt(DateTime? salesDate)
        {
            DownloadResponseDto resp=new();
            try
            {
               
                List<ReceiptDTO> receiptDtos = new List<ReceiptDTO>();
                receiptDtos = getReceiptsFromServer(salesDate);
                if (receiptDtos.Count > 0)
                {
                    List<String> succesReceiptpids = new List<string>();
                    var receiptDtosMap = receiptDtos.GroupBy(pl => pl.userName).ToDictionary(g => g.Key, g => g.ToList());
                    foreach (var entry in receiptDtosMap)
                    {
                        List<TallyXml> tallyXmls = receiptGenerateXml.BuildDailyAllReceiptRequest(entry.Value, entry.Key);
                        TallyResponse succesReceipts =await postReceiptsToTallyAsync(tallyXmls);
						List<String> allSuccesReceiptpid = new();
						//List<String> allSuccesReceiptpid = (List<String>)succesReceipts.body;
						 resp = succesReceipts.body as DownloadResponseDto;
						allSuccesReceiptpid=resp.SuccessOrders;
						succesReceiptpids.AddRange(allSuccesReceiptpid);
						//succesReceiptpids.AddRange((List<String>)succesReceipts.body);
						if (resp.FailedOrders.Count > 0)
						{
							HttpClient httpClient = new HttpClient();
							httpClient = RestClientUtil.getClient();
							string updateFailedStatus = ApiConstants.UPDATE_RECEIPT_STATUS_PENDING;
							HttpContent content2 = new StringContent(JsonConvert.SerializeObject(resp.FailedOrders), Encoding.UTF8, "application/json");
							HttpResponseMessage updateResult = httpClient.PostAsync(updateFailedStatus, content2).Result;
                            string joinedString = string.Join(" \n", resp.failedOrdersLineErrors);

                        }
						if (resp.isLedgerMissmatch)
						{
							string joinedString = string.Join(" \n", resp.failedOrdersLineErrors);
							UC_Download.showMessageToMasterUpdate(resp.FailedOrders.Count+" Download Failed \n Error : "+joinedString);
						}
					}
                    if (succesReceiptpids.Count > 0)
                    {
                        uploadSuccesReceptsToServer(succesReceiptpids);
                    }
					

				}
                return resp;
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
        }

        public ALLLEDGERENTRIESLIST generateIndividualReceiptXml(ReceiptDTO receiptDTO)
        {

            StringBuilder builder = new StringBuilder();
            string trimChar = receiptDTO.trimChar == null ? "" : receiptDTO.trimChar;
            //string ledgerName1 = StringUtilsCustom.replaceSpecialCharactersWithXmlValue(receiptDTO.particularsName) + trimChar;
            string ledgerName1= receiptDTO.particularsName;
            ALLLEDGERENTRIESLIST aLLLEDGERENTRIESLIST = new ALLLEDGERENTRIESLIST();
            OLDAUDITENTRYIDSLIST oLDAUDITENTRYIDSLIST = new();
            oLDAUDITENTRYIDSLIST.TYPE = "Number";
            oLDAUDITENTRYIDSLIST.OLDAUDITENTRYIDS = "-1";

            aLLLEDGERENTRIESLIST.OLDAUDITENTRYIDSLIST = oLDAUDITENTRYIDSLIST;
            aLLLEDGERENTRIESLIST.LEDGERNAME = ledgerName1;
            aLLLEDGERENTRIESLIST.GSTCLASS = "";
            aLLLEDGERENTRIESLIST.ISDEEMEDPOSITIVE = "No";
            aLLLEDGERENTRIESLIST.LEDGERFROMITEM = "No";
            aLLLEDGERENTRIESLIST.REMOVEZEROENTRIES = "No";
            aLLLEDGERENTRIESLIST.ISPARTYLEDGER = "Yse";
            aLLLEDGERENTRIESLIST.ISLASTDEEMEDPOSITIVE = "No";
            aLLLEDGERENTRIESLIST.AMOUNT = receiptDTO.amount;
            if (receiptDTO.receiptAllocationList != null && receiptDTO.receiptAllocationList.Count != 0)
            {
                foreach (ReceiptAllocationDTO allocDto in receiptDTO.receiptAllocationList)
                {
                    aLLLEDGERENTRIESLIST.BANKALLOCATIONSLIST = new BANKALLOCATIONSLIST();
                    BILLALLOCATIONSLIST billAllocationList = new BILLALLOCATIONSLIST();
                    billAllocationList.NAME = allocDto.reference;
                    billAllocationList.BILLTYPE = "Agst Ref";
                    billAllocationList.TDSDEDUCTEEISSPECIALRATE = "No";
                    billAllocationList.AMOUNT = allocDto.amount.ToString();
                    if (godownInReceipt != null && godownInReceipt != "" && !godownInReceipt.Equals("")
                && godownInReceipt.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        if (receiptDTO.godownName != null && !receiptDTO.godownName.Equals(""))
                        {
                            PPSIVCHSALESMANLIST pPSIVCHSALESMANLIST = new PPSIVCHSALESMANLIST();
                            pPSIVCHSALESMANLIST.DESC = "PPSIVCHSalesMan";
                            pPSIVCHSALESMANLIST.ISLIST = "Yes";
                            pPSIVCHSALESMANLIST.TYPE = "String";
                            pPSIVCHSALESMANLIST.INDEX = "103";

                            PPSIVCHSALESMAN pPSIVCHSALESMAN = new();
                            pPSIVCHSALESMAN.DESC = "PPSIVCHSalesMan";
                            pPSIVCHSALESMAN.Text = receiptDTO.godownName;
                            pPSIVCHSALESMANLIST.PPSIVCHSALESMAN = pPSIVCHSALESMAN;
                            billAllocationList.PPSIVCHSALESMANLIST = pPSIVCHSALESMANLIST;
                        }
                    }
                    // INTERESTCOLLECTIONLIST 

                    aLLLEDGERENTRIESLIST.BILLALLOCATIONSLIST = billAllocationList;

                }

              
            }
            return aLLLEDGERENTRIESLIST;
        }
    }
}
