using SNR_ClientApp.DTO;
using SNR_ClientApp.Properties;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Tally.generateXml
{
    public class ReceiptGenerateXml
    {
        String companyName = ApplicationProperties.properties["tally.company"].ToString();
        private String isOptionalReceipt = ApplicationProperties.properties["is.optional.receipt"].ToString();

        private String receiptVoucherType = ApplicationProperties.properties["receipt.voucher.type"].ToString();
        internal List<TallyXml> BuildDailyAllReceiptRequest(List<ReceiptDTO> receiptList, string username)
        {
            try
            {
                List<TallyXml> tallyXmls = new List<TallyXml>();
                StringBuilder receiptsPids = new StringBuilder();
                StringBuilder sb = new StringBuilder();

                string ledgerName = "";
                string date = "";
               

                string uuid = Guid.NewGuid().ToString();
                char[] str = uuid.ToCharArray();
                string remoteId = KeyGeneratorUtil.GetRandomCustomString(str, 6) + "-" + ledgerName;

                bool isOptional = true;

                if (isOptionalReceipt.Equals("false", StringComparison.OrdinalIgnoreCase))
                {
                    isOptional = false;
                }

                StringBuilder builder = new StringBuilder();
                double totAmount = 0;
                List< ALLLEDGERENTRIESLIST> allLedgerEntriesList = new List<ALLLEDGERENTRIESLIST>();
                foreach(ReceiptDTO rcpt in receiptList)
                {
                    ALLLEDGERENTRIESLIST ledgerEntries = new();
                    totAmount = totAmount + rcpt.amount;
                    String trimChar = rcpt.trimChar == null ? "" : rcpt.trimChar;
                    //ledgerName = StringUtilsCustom.replaceSpecialCharactersWithXmlValue(rcpt.ledgerName) + trimChar;
                    //test code:
                    // StringUtilsCustom.replaceSpecialCharactersWithXmlValue can be avoided and
                    // it will we done automatically while serializing object to xml using xmlSerializer 
                    ledgerName=rcpt.ledgerName+trimChar;

                    // date = convertDate("2017-10-01T12:25:35.065");
                    date = StringUtilsCustom.ConvertToDateTime(rcpt.date);

                    String rynamicDate = ApplicationProperties.properties["receiptdate"].ToString();

                    if (rynamicDate != null && !rynamicDate.Equals(""))
                    {
                        date = StringUtilsCustom.ConvertToDateTime(rynamicDate);
                    }
                    
                        OLDAUDITENTRYIDSLIST oLDAUDITENTRYIDSLIST = new OLDAUDITENTRYIDSLIST();
                    oLDAUDITENTRYIDSLIST.TYPE = "Number";
                    oLDAUDITENTRYIDSLIST.OLDAUDITENTRYIDS = "-1";
                    ledgerEntries.OLDAUDITENTRYIDSLIST= oLDAUDITENTRYIDSLIST;
                    ledgerEntries.LEDGERNAME = ledgerName;
                    ledgerEntries.GSTCLASS = "";
                    ledgerEntries.ISDEEMEDPOSITIVE = "No";
                    ledgerEntries.LEDGERFROMITEM = "No";
                    ledgerEntries.REMOVEZEROENTRIES = "No";
                    ledgerEntries.ISPARTYLEDGER = "Yes";
                    ledgerEntries.ISLASTDEEMEDPOSITIVE = "No";
                    ledgerEntries.AMOUNT=rcpt.amount;
                    allLedgerEntriesList.Add(ledgerEntries);

                    receiptsPids.Append(rcpt.accountingVoucherHeaderPid + ",");
                }


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
             
                voucher.REMOTEID = remoteId+ KeyGeneratorUtil.getRandomAlphaNumericString(5);
                voucher.VCHKEY = "644ec99e-8f1b-4088-93f0-ac7269dc1b5f-0000a35d:00000048";
                voucher.VCHTYPE = receiptVoucherType;
                voucher.ACTION = "Create";
                voucher.OBJVIEW = "Accounting Voucher View";

                OLDAUDITENTRYIDSLIST oLDAUDITENTRYIDSLIST1 = new OLDAUDITENTRYIDSLIST();
                oLDAUDITENTRYIDSLIST1.TYPE = "Number";
                oLDAUDITENTRYIDSLIST1.OLDAUDITENTRYIDS = "-1";
                voucher.OLDAUDITENTRYIDSLIST = oLDAUDITENTRYIDSLIST1;
                voucher.DATE = date.Replace("-", "");
                voucher.GUID = "b40d21d7-a905-4cef-b58c-d6c58d9e32f8-00000015";
                voucher.VOUCHERTYPENAME = receiptVoucherType;
                voucher.VOUCHERNUMBER = "1";
                voucher.PARTYLEDGERNAME = ledgerName;
                voucher.CSTFORMISSUETYPE = "";
                voucher.CSTFORMRECVTYPE = "";
                voucher.NARRATION = "Daily Receipt ( " + username + " )" + "</NARRATION>";
                voucher.FBTPAYMENTTYPE = "Default";
                voucher.PERSISTEDVIEW = "Accounting Voucher View";
                voucher.VCHGSTCLASS = "";
                voucher.DIFFACTUALQTY = "No";
                voucher.AUDITED = "No";
                voucher.FORJOBCOSTING = "No";
                voucher.ISOPTIONAL = isOptional ? "Yes" : "No";
                voucher.EFFECTIVEDATE = date.Replace("-", "");

                voucher.ISFORJOBWORKIN = "No";
                voucher.ALLOWCONSUMPTION = "No";
                voucher.USEFORINTEREST = "No";
                voucher.USEFORGAINLOSS = "No";
                voucher.USEFORGODOWNTRANSFER = "No";
                voucher.USEFORCOMPOUND = "No";
                voucher.ALTERID = 0;
                voucher.USEFORFINALPRODUCTION = "No";
                voucher.ISCANCELLED = "No";


                voucher.HASCASHFLOW = "Yes";
                voucher.ISPOSTDATED = "No";
                voucher.USETRACKINGNUMBER = "No";
                voucher.ISINVOICE = "No";
                voucher.MFGJOURNAL = "No";
                voucher.HASDISCOUNTS = "No";
                voucher.ASPAYSLIP = "No";
                voucher.ISCOSTCENTRE = "No";
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
                ALLLEDGERENTRIESLIST ledgerEntries1 = new();

                OLDAUDITENTRYIDSLIST oLDAUDITENTRYIDSLIST2 = new OLDAUDITENTRYIDSLIST();
                oLDAUDITENTRYIDSLIST2.TYPE = "Number";
                oLDAUDITENTRYIDSLIST2.OLDAUDITENTRYIDS = "-1";
                ledgerEntries1.OLDAUDITENTRYIDSLIST = oLDAUDITENTRYIDSLIST2;
                ledgerEntries1.LEDGERNAME = "Cash";
                ledgerEntries1.GSTCLASS = "";
                ledgerEntries1.ISDEEMEDPOSITIVE = "No";
                ledgerEntries1.LEDGERFROMITEM = "No";
                ledgerEntries1.REMOVEZEROENTRIES = "No";
                ledgerEntries1.ISPARTYLEDGER = "Yes";
                ledgerEntries1.ISLASTDEEMEDPOSITIVE = "Yes";
                ledgerEntries1.AMOUNT = totAmount;
                allLedgerEntriesList.Add(ledgerEntries1);

                voucher.ALLLEDGERENTRIES_LIST = allLedgerEntriesList;
               
                tallymessage.VOUCHER= voucher;
                List<TALLYMESSAGE> messages = new();
                messages.Add(tallymessage);
                requestData.TALLYMESSAGE = messages;
                importdata.REQUESTDATA = requestData;
                body.IMPORTDATA = importdata;
                tallyRequest.BODY = body;


                TallyXml tallyXml = new TallyXml();
                tallyXml.xmlObj=(tallyRequest);
                tallyXml.pid=(receiptsPids.ToString());
                tallyXmls.Add(tallyXml);

                return tallyXmls;





            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
        }
    }
}
