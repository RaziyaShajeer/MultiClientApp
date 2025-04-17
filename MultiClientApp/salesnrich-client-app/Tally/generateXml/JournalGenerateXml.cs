using SNR_ClientApp.DTO;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Services;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Tally.generateXml
{
    internal class JournalGenerateXml
    {
        private String enableReceiptVoucheType=ApplicationProperties.properties["enable.receipt.voucherType"].ToString();
        private String companyString = ApplicationProperties.properties["tally.company"].ToString();
        private String isOptionalReceipt = ApplicationProperties.properties["is.optional.receipt"].ToString();
        private String enableCostCentre=ApplicationProperties.properties["enable.cost.centre"].ToString();
        private String employeeWiseLedger=ApplicationProperties.properties["enable.receipt.employeewise.ledger"].ToString() ;
        private String receiptBankName= ApplicationProperties.properties["receipt.voucher.bank.name"].ToString();
        private String transactionType= ApplicationProperties.properties["transaction.type"].ToString();
        string receiptVoucherTypeCash = ApplicationProperties.properties["receipt.voucher.type.cash"].ToString();
        string receiptVoucherTypeBank = ApplicationProperties.properties["receipt.voucher.type.bank"].ToString();
        DownloadReceiptService downloadReceiptService;
        public JournalGenerateXml()
        {
            downloadReceiptService = new DownloadReceiptService();
        }
        internal ENVELOPE generateJournalXml(ReceiptDTO receiptDTO)
        {
            List<TallyXml> tallyXmls = new List<TallyXml>();
            StringBuilder builder = new StringBuilder();
          
            if (enableReceiptVoucheType.Equals("true", StringComparison.OrdinalIgnoreCase) && receiptDTO.employeeAlias != null)
            {
                var parts = receiptDTO.employeeAlias.Split("~");
                receiptVoucherTypeCash = parts[0];
                receiptVoucherTypeBank = parts[0];
                Console.WriteLine("enableReceiptVoucheType : TRUE\nEmployee Receipt Voucher Type: " + receiptDTO.employeeAlias);
            }
            string trimChar = string.IsNullOrEmpty(receiptDTO.trimChar) ? "" : receiptDTO.trimChar;
            string ledgerName = receiptDTO.ledgerName + trimChar;
            string companyName = companyString;
            string uuid = Guid.NewGuid().ToString();
            string remoteId = KeyGeneratorUtil.GetRandomCustomString(uuid.ToCharArray(), 6) + "-" + ledgerName;
            string date = StringUtilsCustom.convertDateToString(receiptDTO.date);
            string rynamicDate = ApplicationProperties.properties["receiptdate"].ToString();
            if (!string.IsNullOrEmpty(rynamicDate))
            {
                date = StringUtilsCustom.convertDateToString(rynamicDate);
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
            requestDesc.REPORTNAME = "Vouchers";
            STATICVARIABLES staticvariables = new STATICVARIABLES();

            staticvariables.SVCURRENTCOMPANY = ApplicationProperties.properties["tally.company"].ToString();

            requestDesc.STATICVARIABLES = staticvariables;
            requestDesc.REPORTNAME = "All Masters";

            importdata.REQUESTDESC = requestDesc;
            REQUESTDATA requestData = new();
            List<TALLYMESSAGE> talllyMessagesList = new();
            TALLYMESSAGE tallymessage = new();
            tallymessage.UDF = "TallyUDF";

            VOUCHER voucher = new VOUCHER();
            voucher.REMOTEID = remoteId;
            String narration_message = receiptDTO.narrationMessage;

            String provisionalReceiptNo = receiptDTO.provisionalReceiptNo != null
                    ? " ,  Provisional Receipt No. :" + receiptDTO.provisionalReceiptNo
                    : "";
            //narration_message.Replace("&", "&amp;");
            if (receiptDTO.mode != PaymentMode.Cash)
            {
                voucher.VCHKEY = "644ec99e-8f1b-4088-93f0-ac7269dc1b5f-0000a35d:00000048";
                voucher.VCHTYPE = receiptVoucherTypeCash;
                voucher.ACTION = "Create";
                voucher.OBJVIEW = "Accounting Voucher View";
                List<OLDAUDITENTRYIDSLIST> oLDAUDITENTRYIDSLISTs = new List<OLDAUDITENTRYIDSLIST>();
                OLDAUDITENTRYIDSLIST oldAuditEntry=new OLDAUDITENTRYIDSLIST();
                oldAuditEntry.TYPE = "Number";
                oldAuditEntry.OLDAUDITENTRYIDS = "-1";
                oLDAUDITENTRYIDSLISTs.Add(oldAuditEntry);
                voucher.OLDAUDITENTRYIDSLIST = oldAuditEntry;
                voucher.DATE = date;
                voucher.GUID = "b40d21d7-a905-4cef-b58c-d6c58d9e32f8-0000001";
                voucher.NARRATION = receiptDTO.narrationMessage == null ? "" : "Remarks:" + narration_message + provisionalReceiptNo;
                voucher.VOUCHERTYPENAME = receiptVoucherTypeCash;
                voucher.VOUCHERNUMBER = receiptDTO.provisionalReceiptNo != null ? receiptDTO.provisionalReceiptNo : "1";
               // voucher.PARTYLEDGERNAME=

            }
            else if (receiptDTO.mode != PaymentMode.Bank)
            {
                voucher.VCHKEY = "644ec99e-8f1b-4088-93f0-ac7269dc1b5f-0000a35d:00000048";
                voucher.VCHTYPE = receiptVoucherTypeBank;
                voucher.ACTION = "Create";
                voucher.OBJVIEW = "Accounting Voucher View";
                List<OLDAUDITENTRYIDSLIST> oLDAUDITENTRYIDSLISTs = new List<OLDAUDITENTRYIDSLIST>();
                OLDAUDITENTRYIDSLIST oldAuditEntry = new OLDAUDITENTRYIDSLIST();
                oldAuditEntry.TYPE = "Number";
                oldAuditEntry.OLDAUDITENTRYIDS = "-1";
                oLDAUDITENTRYIDSLISTs.Add(oldAuditEntry);
                voucher.OLDAUDITENTRYIDSLIST = oldAuditEntry;
                voucher.DATE = date;
                voucher.GUID = "b40d21d7-a905-4cef-b58c-d6c58d9e32f8-0000001";
                voucher.NARRATION = "Bank Name:" + receiptDTO.bankName + "   Cheque No:"
                    + receiptDTO.chequeNo + "   Cheque Date:" + StringUtilsCustom.FormatDate(receiptDTO.chequeDate)
                    + "   Remarks:" + narration_message + provisionalReceiptNo;
                voucher.VOUCHERTYPENAME = receiptVoucherTypeBank;
                voucher.VOUCHERNUMBER = (receiptDTO.provisionalReceiptNo != null ? receiptDTO.provisionalReceiptNo : "1");
                // voucher.PARTYLEDGERNAME=

            }
            else if (receiptDTO.mode != PaymentMode.RTGS)
            {
                voucher.VCHKEY = "644ec99e-8f1b-4088-93f0-ac7269dc1b5f-0000a35d:00000048";
                voucher.VCHTYPE = receiptVoucherTypeBank;
                voucher.ACTION = "Create";
                voucher.OBJVIEW = "Accounting Voucher View";
                List<OLDAUDITENTRYIDSLIST> oLDAUDITENTRYIDSLISTs = new List<OLDAUDITENTRYIDSLIST>();
                OLDAUDITENTRYIDSLIST oldAuditEntry = new OLDAUDITENTRYIDSLIST();
                oldAuditEntry.TYPE = "Number";
                oldAuditEntry.OLDAUDITENTRYIDS = "-1";
                oLDAUDITENTRYIDSLISTs.Add(oldAuditEntry);
                voucher.OLDAUDITENTRYIDSLIST = oldAuditEntry;
                voucher.DATE = date;
                voucher.GUID = "b40d21d7-a905-4cef-b58c-d6c58d9e32f8-0000001";
                voucher.NARRATION = "Bank Name:" + receiptDTO.bankName + "   RTGS No:"
                    + receiptDTO.chequeNo + "   RTGS Date:" + StringUtilsCustom.FormatDate(receiptDTO.chequeDate)
                    + "   Remarks:" + narration_message + provisionalReceiptNo;
                voucher.VOUCHERTYPENAME = receiptVoucherTypeBank;
                voucher.VOUCHERNUMBER = (receiptDTO.provisionalReceiptNo != null ? receiptDTO.provisionalReceiptNo : "1");
                // voucher.PARTYLEDGERNAME=

            }
            voucher.PARTYLEDGERNAME = ledgerName;
            voucher.CSTFORMISSUETYPE = "";
            voucher.FBTPAYMENTTYPE = "Default";
            voucher.PERSISTEDVIEW = "Accounting Voucher View";
            voucher.VCHGSTCLASS = "";
            voucher.DIFFACTUALQTY = "No";
            voucher.AUDITED = "No";
            voucher.FORJOBCOSTING = "No";
            voucher.ISOPTIONAL = isOptional ? "Yes" : "No";
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
            if ("Yes".Equals(enableCostCentre))
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
            totalAmount = receiptDTO.amount;
            List<ALLLEDGERENTRIESLIST> allledgerEntries = new List<ALLLEDGERENTRIESLIST>();
            ALLLEDGERENTRIESLIST aLLLEDGERENTRIESLIST = new ALLLEDGERENTRIESLIST();
            OLDAUDITENTRYIDSLIST oLDAUDITENTRYIDSLIST = new OLDAUDITENTRYIDSLIST();
            oLDAUDITENTRYIDSLIST.TYPE = "Number";
            oLDAUDITENTRYIDSLIST.OLDAUDITENTRYIDS = "-1";
            aLLLEDGERENTRIESLIST.OLDAUDITENTRYIDSLIST= oLDAUDITENTRYIDSLIST;

            //adding Selvouchers 17/05/2024

            SDKSYNCSELVOUCHERS sDKSYNCSELVOUCHERS = new();
            sDKSYNCSELVOUCHERS.DESC="sdkSyncSelVouchers";
            sDKSYNCSELVOUCHERS.Text= "Yes";
            SDKSYNCSELVOUCHERSLIST sDKSYNCSELVOUCHERSLIST = new();
            sDKSYNCSELVOUCHERSLIST.SDKSYNCSELVOUCHERS=sDKSYNCSELVOUCHERS;
            voucher.SDKSYNCSELVOUCHERSLIST=sDKSYNCSELVOUCHERSLIST;


            String modeBasedLedger = "Emp alias not configured";
            if (employeeWiseLedger.Equals("true",StringComparison.OrdinalIgnoreCase) && receiptDTO.employeeAlias != null
                && receiptDTO.employeeAlias.Split("~").Length > 2)
            {
                modeBasedLedger = receiptDTO.employeeAlias.Split("~")[2];
                aLLLEDGERENTRIESLIST.LEDGERNAME = modeBasedLedger;
            }
            else
            {
                if (receiptDTO.mode == PaymentMode.Cash)
                {
                    aLLLEDGERENTRIESLIST.LEDGERNAME = "Cash" ;
                }
                else if (receiptDTO.mode == PaymentMode.Bank)
                {
                    aLLLEDGERENTRIESLIST.LEDGERNAME = receiptBankName;
                }
                else if (receiptDTO.mode == PaymentMode.RTGS)
                {
                    aLLLEDGERENTRIESLIST.LEDGERNAME = receiptBankName;
                }
            }
            aLLLEDGERENTRIESLIST.ISDEEMEDPOSITIVE = "Yes";
            aLLLEDGERENTRIESLIST.LEDGERFROMITEM = "No";
            aLLLEDGERENTRIESLIST.REMOVEZEROENTRIES = "No";
            aLLLEDGERENTRIESLIST.ISPARTYLEDGER = "Yes";

            aLLLEDGERENTRIESLIST.ISLASTDEEMEDPOSITIVE = "Yes";
            aLLLEDGERENTRIESLIST.AMOUNT =0- totalAmount;

            BANKALLOCATIONSLIST bANKALLOCATIONSLIST = new BANKALLOCATIONSLIST();
            bANKALLOCATIONSLIST.DATE = StringUtilsCustom.ConvertToDateTime(receiptDTO.chequeDate);
            bANKALLOCATIONSLIST.INSTRUMENTDATE = StringUtilsCustom.ConvertToDateTime(receiptDTO.chequeDate);
            bANKALLOCATIONSLIST.BANKNAME=receiptDTO.bankName;
            bANKALLOCATIONSLIST.INSTRUMENTNUMBER = receiptDTO.chequeNo;
            bANKALLOCATIONSLIST.TRANSACTIONTYPE = transactionType;


            aLLLEDGERENTRIESLIST.BANKALLOCATIONSLIST = bANKALLOCATIONSLIST;
            BILLALLOCATIONSLIST bILLALLOCATIONSLIST = new BILLALLOCATIONSLIST();
            aLLLEDGERENTRIESLIST.BILLALLOCATIONSLIST = bILLALLOCATIONSLIST;
            allledgerEntries.Add(aLLLEDGERENTRIESLIST);
           

            ALLLEDGERENTRIESLIST aLLLEDGERENTRIESLIST1= downloadReceiptService.generateIndividualReceiptXml(receiptDTO);
            allledgerEntries.Add(aLLLEDGERENTRIESLIST1);

            voucher.ALLLEDGERENTRIES_LIST = allledgerEntries;
            tallymessage.VOUCHER = voucher;
            talllyMessagesList.Add(tallymessage);
            requestData.TALLYMESSAGE = talllyMessagesList;
            importdata.REQUESTDATA = requestData;
            body.IMPORTDATA= importdata;
            tallyRequest.BODY = body;

            TallyXml tallyXml = new TallyXml();
            tallyXml.pid=(receiptDTO.accountingVoucherHeaderPid);
            tallyXml.xmlObj = tallyRequest;
            tallyXmls.Add(tallyXml);



            return tallyRequest;

        }

      
    }
}
