using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SNR_ClientApp.Parsers
{
    public class ReceivablePayableReportResponseParser
    {
        public HashSet<string> parseReceivablePayableXmlToLedgerNames(string inputString, ReceivablePayableType receivable)
        {

            HashSet<String> ledgerNames = new HashSet<String>();
            List<String> parts = new List<string>();

            try
            {

                 parts = getSubArrays(inputString, "<BILLFIXED>", "</ENVELOPE>");
                XmlSerializer serializer1 = new XmlSerializer(typeof(BillsReceivableResponse));
                List<BillsReceivableResponse> bilsList = new List<BillsReceivableResponse>();

                foreach (var xml in parts)
                {
                    try
                    {
                        string newXml = "<Root>" + xml + "</Root>";
                        var reader = new StringReader(newXml);
                        var item = (BillsReceivableResponse)serializer1.Deserialize(reader);
                        if (item != null)
                        {
                            if(!ledgerNames.Contains(item.BILLFIXED.BILLPARTY))
                            ledgerNames.Add(item.BILLFIXED.BILLPARTY);
                        }
                        bilsList.Add(item);
                    }catch(Exception ex)
                    {
                        LogManager.HandleException(ex);
                    }

                }

              

            }
            catch(Exception ex)
            {
                LogManager.HandleException(ex, "exception occured while parseReceivablePayableXmlToLedgerNames");
                throw ex;
            }

            return ledgerNames;



        }


        //public List<string> getSubArrays(String inputString, String startingTag)
        //{
        //    int firstIndex = inputString.IndexOf(startingTag);
        //    int secondIndex = inputString.IndexOf(startingTag, firstIndex + 1);
        //    List<String> parts = new List<string>();
        //    while (firstIndex != -1 && secondIndex != -1)
        //    {
        //        parts.Add(inputString.Substring(firstIndex, secondIndex - firstIndex));
        //        firstIndex = secondIndex;
        //        secondIndex = inputString.IndexOf(startingTag, firstIndex + 1);
        //    }

        //    return parts;
        //}

        public List<string> getSubArrays(String inputString, String startingTag, String endingTag, String OptionalEndingtag = "")
        {
            int firstIndex = inputString.IndexOf(startingTag);
            int secondIndex = inputString.IndexOf(startingTag, firstIndex + 1);

            List<String> parts = new List<string>();
            while (firstIndex != -1 && secondIndex != -1)
            {
                if (secondIndex > 0)
                {
                    parts.Add(inputString.Substring(firstIndex, secondIndex - firstIndex));
                    firstIndex = secondIndex;
                    secondIndex = inputString.IndexOf(startingTag, firstIndex + 1);
                }


            }
            if (secondIndex < 0 && firstIndex>=0)
            {
                int aditionalLength = 0;
                if (!OptionalEndingtag.Equals(""))
                {
                    int optIndex = inputString.IndexOf(endingTag, firstIndex + 1);
                    if (optIndex > 0)
                    {
                        secondIndex = optIndex;
                        aditionalLength = endingTag.Length;
                    }
                    else
                    {
                        secondIndex = inputString.IndexOf(OptionalEndingtag, firstIndex + 1);
                        aditionalLength = OptionalEndingtag.Length;
                    }
                }
                else
                {
                    secondIndex = inputString.IndexOf(endingTag, firstIndex + 1);
                }
                string tag = startingTag.Substring(1, startingTag.Length - 2);
                // secondIndex = inputString.IndexOf(endingTag, firstIndex + 1);
                parts.Add(inputString.Substring(firstIndex, secondIndex - firstIndex + aditionalLength));
            }


            return parts;
        }


        internal List<ReceivablePayableDTO> parseReceivablePayableDTOsXml(string billsPayablesResXml, ReceivablePayableType type, String? ledgername=null)
        {
            List<ReceivablePayableDTO> billReceivables = new List<ReceivablePayableDTO>();
           

            List<BillsReceivableResponse> billsReceivableResponses = getConvertedList(billsPayablesResXml);

            foreach(var item in billsReceivableResponses)
            {
                try
                {
                    ReceivablePayableDTO billReceivable = new ReceivablePayableDTO();
                    billReceivable.referenceDocumentDate = StringUtilsCustom.ConvertFormat(item.BILLFIXED.BILLDATE);
                    billReceivable.referenceDocumentNumber = item.BILLFIXED.BILLREF;
                    ledgername = (item.BILLFIXED.BILLPARTY != null) ? item.BILLFIXED.BILLPARTY : ledgername;
                    billReceivable.accountName = ledgername;
                    billReceivable.referenceDocumentBalanceAmount = (item.BILLCL != null) ? Math.Abs(StringUtilsCustom.ExtractDoubleValue(item.BILLCL)) : 0;
                    billReceivable.referenceDocumentAmount = (item.BILLOP != null) ? Math.Abs( StringUtilsCustom.ExtractDoubleValue(item.BILLOP) ): 0;
                    billReceivable.billOverDue = (!String.IsNullOrEmpty(item.BILLOVERDUE)) ? item.BILLOVERDUE.ToString() : "0";
                    billReceivable.receivablePayableType = type;
                    if (!"On Account".Equals(billReceivable.referenceDocumentNumber, StringComparison.OrdinalIgnoreCase))
                    {
                        billReceivables.Add(billReceivable);
                    }
                }catch(Exception ex)
                {
                    LogManager.HandleException(ex);
                    throw ex;
                }
            }
            return billReceivables;
        }

        public List<BillsReceivableResponse> getConvertedList(String inputString)
        {
            List<string> parts=new List<string>();
            List<BillsReceivableResponse> bilsList = new List<BillsReceivableResponse>();
            try
            {

                 parts = getSubArrays(inputString, "<BILLFIXED>", "</ENVELOPE>");
                XmlSerializer serializer1 = new XmlSerializer(typeof(BillsReceivableResponse));
              

                foreach (var xml in parts)
                {
                    string newXml = "<Root>" + xml + "</Root>";
                    var reader = new StringReader(newXml);
                    var item = (BillsReceivableResponse)serializer1.Deserialize(reader);
                   
                    bilsList.Add(item);

                }



            }
            catch (Exception ex)
            {
                LogManager.WriteLog("Exception Occured on getConvertedList on ReceivablePayableReportResponseParser.cs file \n "+ex.Message);
                LogManager.HandleException(ex);
            }
            return bilsList;
        }
    }
}
