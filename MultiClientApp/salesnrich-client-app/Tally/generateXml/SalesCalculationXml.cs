using dtos;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Properties;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Tally.generateXml
{
    public class SalesCalculationXml
    {
        private String enableDateWise = ApplicationProperties.properties["enable.date"].ToString();
        private String userStockLocation;

        private String companyString = ApplicationProperties.properties["tally.company"].ToString();

        private String apiUrl;
        private String byEmpVoucher;

        private String employeeString;
        private String isOptionalsalesOrder = ApplicationProperties.properties["is.optional.salesOrder"].ToString();
        private String companyState = ApplicationProperties.properties["company.state"].ToString();
        private String orderNumberWithEmployee = ApplicationProperties.properties["order.employee.name"].ToString();
        private String actualBilledStatus = ApplicationProperties.properties["actual.billed.status"].ToString();
        private String gstNames = ApplicationProperties.properties["tally.gst"].ToString();
        private String kfcLedger = ApplicationProperties.properties["kfc.ledger.name"].ToString();
        private String gstCalculationEnabled = ApplicationProperties.properties["gst.ledger.calculation"].ToString();
        private String roundOffLedger = ApplicationProperties.properties["round.off.ledger"].ToString();
        public String multiTax = ApplicationProperties.properties["is.multi.tax"].ToString();
        public String gstTax = ApplicationProperties.properties["tally.taxs"].ToString();
        private String reduceTax = ApplicationProperties.properties["reduce.tax"].ToString();
        private String productRateIncludingTax = ApplicationProperties.properties["product.rate.including.tax"].ToString();
        private String salesOrderActivityRemark = ApplicationProperties.properties["sales.order.activity.remarks"].ToString();
        private String itemRemarksEnabled = ApplicationProperties.properties["item.remarks.enabled"].ToString();
        private String enableCashOnlyLedgerEntry = ApplicationProperties.properties["enable.cash.only.ledger.entry"].ToString();
        private String enableCostCentre = ApplicationProperties.properties["enable.cost.centre"].ToString();
        private String paymentModeOrTerms = ApplicationProperties.properties["payment.mode.terms"].ToString();
        private String godownFixed = ApplicationProperties.properties["godown.fixed"].ToString();
        private String batchFixed = ApplicationProperties.properties["batch.fixed"].ToString();
        private String salesLedger = ApplicationProperties.properties["sales.ledger"].ToString();
        private String caseValue = ApplicationProperties.properties["case.value"].ToString();

        //for discount ledger
        public String IsDiscountLedgerEnabled = ApplicationProperties.properties["enable.discount.ledger"].ToString();
        public String DiscountLedger = ApplicationProperties.properties["discount.ledger"].ToString();


        TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        public SalesCalculationXml()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            enableDateWise = ApplicationProperties.properties["enable.date"].ToString();
            companyString = ApplicationProperties.properties["tally.company"].ToString();
            apiUrl = ApplicationProperties.properties["service.full.url"].ToString();
            byEmpVoucher = ApplicationProperties.properties["download.by.employee.voucher"].ToString();
            userStockLocation = ApplicationProperties.properties["user.stockLocation"].ToString();
        }
        public ALLINVENTORYENTRIESLIST generateSingleItemXml(SalesOrderItemDTO salesOrder, SalesOrderDTO config, bool isFreeItem)
        {
            bool discount = !isFreeItem;

            // diffQuantity -> do we need 2 different quantity( billed and actual)
            bool diffQuantity = true;
            diffQuantity = actualBilledStatus.Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false;

            bool isOptional = true;

            if (isOptionalsalesOrder.Equals("false", StringComparison.OrdinalIgnoreCase))
            {
                isOptional = false;
            }

            String trimChar = salesOrder.trimChar == null ? "" : salesOrder.trimChar;
            String itemName = StringUtilsCustom.replaceSpecialCharactersWithXmlValue(salesOrder.itemName) + trimChar;
            ALLINVENTORYENTRIESLIST aLLINVENTORYENTRIESLIST = new ALLINVENTORYENTRIESLIST();
            if (salesOrder.remarks != null)
            {
                BASICUSERDESCRIPTIONLIST bASICUSERDESCRIPTIONLIST = new BASICUSERDESCRIPTIONLIST();
                bASICUSERDESCRIPTIONLIST.TYPE = "String";
                bASICUSERDESCRIPTIONLIST.BASICUSERDESCRIPTION = salesOrder.remarks;
                aLLINVENTORYENTRIESLIST.BASICUSERDESCRIPTIONLIST = bASICUSERDESCRIPTIONLIST;

            }
            //aLLINVENTORYENTRIESLIST.STOCKITEMNAME = itemName;
            aLLINVENTORYENTRIESLIST.STOCKITEMNAME = salesOrder.itemName+trimChar;
            aLLINVENTORYENTRIESLIST.SUBCATEGORY = "+ VAT";
            aLLINVENTORYENTRIESLIST.ISDEEMEDPOSITIVE = "No";
            aLLINVENTORYENTRIESLIST.ISLASTDEEMEDPOSITIVE = "No";
            aLLINVENTORYENTRIESLIST.ISAUTONEGATE = "No";
            aLLINVENTORYENTRIESLIST.ISCUSTOMSCLEARANCE = "No";
            aLLINVENTORYENTRIESLIST.ISTRACKCOMPONENT = "No";
            aLLINVENTORYENTRIESLIST.ISTRACKPRODUCTION = "No";
            aLLINVENTORYENTRIESLIST.ISPRIMARYITEM = "No";
            aLLINVENTORYENTRIESLIST.ISSCRAP = "No";

            if (!isFreeItem)
            {

                double sellingRate = salesOrder.sellingRate;

                if (reduceTax.Equals("true",StringComparison.OrdinalIgnoreCase))
                {
                    sellingRate = salesOrder.sellingRate * 100 / (100 + salesOrder.taxPercentage);
                }

                if (productRateIncludingTax != null && productRateIncludingTax.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    double totalTax = 0.0;

                    if (salesOrder.taxPercentage > 5 && config.ledgerGstType != null
                            && !"Regular".Equals(config.ledgerGstType, StringComparison.OrdinalIgnoreCase) && kfcLedger != null)
                    {
                        totalTax = salesOrder.taxPercentage + 1;
                    }
                    else
                    {
                        totalTax = salesOrder.taxPercentage;
                    }
                    sellingRate = (salesOrder.sellingRate * (100.0 / (100.0 + totalTax)));
                }

                sellingRate = round(sellingRate);

                string rate;
                if (caseValue.Equals("true", StringComparison.OrdinalIgnoreCase ) && salesOrder.unit.Equals("case",StringComparison.OrdinalIgnoreCase)){
                    rate = sellingRate + "/nos";
                }
                else
                {
                    rate = sellingRate + "/" + salesOrder.unit;
                }

                aLLINVENTORYENTRIESLIST.RATE = rate;


            }
            if (discount)
            {
                aLLINVENTORYENTRIESLIST.DISCOUNT = salesOrder.itemDiscount;
            }
            if (!isFreeItem)
            {
                double itemTotalExeVat = salesOrder.sellingRate * salesOrder.quantity;
                if (reduceTax.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    itemTotalExeVat = salesOrder.sellingRate * 100 / (100 + salesOrder.taxPercentage) * salesOrder.quantity;
                }

                if (productRateIncludingTax != null && productRateIncludingTax.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    double totalTax = 0.0;

                    if (salesOrder.taxPercentage > 5 && config.ledgerGstType != null
                            && !"Regular".Equals(config.ledgerGstType, StringComparison.OrdinalIgnoreCase) && kfcLedger != null)
                    {
                        totalTax = salesOrder.taxPercentage + 1;
                    }
                    else
                    {
                        totalTax = salesOrder.taxPercentage;
                    }

                    double sellRate = (salesOrder.sellingRate * (100.0 / (100.0 + totalTax)));

                    sellRate = round(sellRate);
                    itemTotalExeVat = sellRate * salesOrder.quantity;
                }

                if (salesOrder.itemDiscount != 0)
                {
                    double discountAmount = itemTotalExeVat * salesOrder.itemDiscount / 100;
                    itemTotalExeVat = itemTotalExeVat - discountAmount;
                }
                //if (salesOrder.itemDiscount != 0)
                //{
                //    double discountAmount = itemTotalExeVat * salesOrder.itemDiscount / 100;
                //    itemTotalExeVat = itemTotalExeVat - discountAmount;
                //}

                aLLINVENTORYENTRIESLIST.AMOUNT = itemTotalExeVat;

            }
            // checks different quantity is enabled or not
      
            String actualQuantityCase = "";
            String billedQuantityCase = "";
            double actualQuantity = 0;
            double billedQuantity = 0;
            if (diffQuantity)
            {
                actualQuantity = salesOrder.quantity + salesOrder.itemFreeQuantity;
                billedQuantity = salesOrder.quantity;
            }
            else
            {
                if (isFreeItem)
                {
                    actualQuantity = salesOrder.itemFreeQuantity;
                    billedQuantity = salesOrder.itemFreeQuantity;
                }
                else
                {
                    actualQuantity = salesOrder.quantity;
                    billedQuantity = salesOrder.quantity;
                }
            }
            string actualQtyToTally = "";
            string billedQtyToTally = "";
            String caseOfValuestr = salesOrder.alias == null ? "" : salesOrder.alias;
            if (caseValue.Equals("true", StringComparison.OrdinalIgnoreCase) && caseOfValuestr != "") {
                double caseOfValue=0;
                    Double.TryParse(caseOfValuestr,out caseOfValue);
                int resultActualQuantity = (int)(actualQuantity / caseOfValue);
                int reminderactualQuantity = (int)(actualQuantity % caseOfValue);

                actualQuantityCase = resultActualQuantity + "-" +reminderactualQuantity;
                int resultBilledQuantity = (int)(billedQuantity / caseOfValue);
                int reminderbilledQuantity = (int)(billedQuantity % caseOfValue);

                billedQuantityCase = resultBilledQuantity + "-" + reminderbilledQuantity;

                aLLINVENTORYENTRIESLIST.ACTUALQTY = actualQuantityCase + salesOrder.unit;
                aLLINVENTORYENTRIESLIST.BILLEDQTY = billedQuantityCase + salesOrder.unit;
                actualQtyToTally = actualQuantityCase + salesOrder.unit;
                billedQtyToTally= billedQuantityCase + salesOrder.unit;

            }
            else
            {
                actualQtyToTally = actualQuantity + salesOrder.unit;
                billedQtyToTally = billedQuantity + salesOrder.unit;
            }

            //aLLINVENTORYENTRIESLIST.ACTUALQTY = actualQuantity + salesOrder.unit;
            //aLLINVENTORYENTRIESLIST.BILLEDQTY = billedQuantity + salesOrder.unit;

            aLLINVENTORYENTRIESLIST.ACTUALQTY = actualQtyToTally;
            aLLINVENTORYENTRIESLIST.BILLEDQTY = billedQtyToTally;

            BATCHALLOCATIONSLIST bATCHALLOCATIONSLIST = new BATCHALLOCATIONSLIST();

            String mainLocation = "";
            LogManager.WriteLog("SOURCE STOCK LOCATION :" + salesOrder.sourceStockLocationName);
            LogManager.WriteLog("STOCKLOCATION : " + salesOrder.stockLocationName);
            if ("true".Equals(godownFixed))
            {
                salesOrder.stockLocationName = ApplicationProperties.properties["godownName"].ToString();
            }
            if (salesOrder.sourceStockLocationName == null)
            {

                if (salesOrder.stockLocationName == null)
                {
                    mainLocation = ApplicationProperties.properties["godownName"].ToString();
                }
                else
                {
                    mainLocation = salesOrder.stockLocationName;
                }

            }
            else
            {

                if (salesOrder.stockLocationName == null)
                {
                    mainLocation = salesOrder.sourceStockLocationName;
                }
                else
                {
                    mainLocation = salesOrder.stockLocationName;
                }

            }


            bATCHALLOCATIONSLIST.GODOWNNAME = mainLocation;
            String batchName = "Primary Batch";
            if ("true".Equals(batchFixed))
            {
                batchName = ApplicationProperties.properties["batchName"].ToString();
            }
            bATCHALLOCATIONSLIST.BATCHNAME = batchName;
            bATCHALLOCATIONSLIST.DESTINATIONGODOWNNAME = mainLocation;
            bATCHALLOCATIONSLIST.INDENTNO = "";
            bATCHALLOCATIONSLIST.ORDERNO = "-1";
            bATCHALLOCATIONSLIST.DYNAMICCSTISCLEARED = "No";

            if (!isFreeItem)
            {
                bATCHALLOCATIONSLIST.AMOUNT = salesOrder.rowTotal;

            }
            //bATCHALLOCATIONSLIST.ACTUALQTY = actualQuantity + salesOrder.unit;
            //bATCHALLOCATIONSLIST.BILLEDQTY = billedQuantity + salesOrder.unit;
            bATCHALLOCATIONSLIST.ACTUALQTY = actualQtyToTally;
            bATCHALLOCATIONSLIST.BILLEDQTY = billedQtyToTally ;

            String date = StringUtilsCustom.FormatDate(config.date.ToString());
            String dynamicDate = ApplicationProperties.properties["salessorderDate"].ToString();
            if (dynamicDate != null && !dynamicDate.Equals(""))
            {
                date = StringUtilsCustom.FormatDate(dynamicDate);
            }
            bATCHALLOCATIONSLIST.ORDERDUEDATE = date;

            ATCSTKITEMBATCHNETRATELIST aTCSTKITEMBATCHNETRATELIST = new ATCSTKITEMBATCHNETRATELIST();
            aTCSTKITEMBATCHNETRATELIST.DESC = "ATCStkItemBatchNetRate";
            aTCSTKITEMBATCHNETRATELIST.ISLIST = "YES";
            aTCSTKITEMBATCHNETRATELIST.TYPE = "Rate";
            aTCSTKITEMBATCHNETRATELIST.INDEX = "53001";
            ATCSTKITEMBATCHNETRATE aTCSTKITEMBATCHNETRATE = new();
            aTCSTKITEMBATCHNETRATE.DESC = "ATCStkItemBatchNetRate";
            aTCSTKITEMBATCHNETRATE.Text = round(salesOrder.sellingRate) + "/nos";

            aTCSTKITEMBATCHNETRATELIST.ATCSTKITEMBATCHNETRATE = aTCSTKITEMBATCHNETRATE;

            bATCHALLOCATIONSLIST.ATCSTKITEMBATCHNETRATELIST = aTCSTKITEMBATCHNETRATELIST;
            aLLINVENTORYENTRIESLIST.BATCHALLOCATIONSLIST = bATCHALLOCATIONSLIST;


            ACCOUNTINGALLOCATIONSLIST aCCOUNTINGALLOCATIONSLIST = new();

            if (salesOrder.productProfileDTO.defaultLedger != null)
            {
                aCCOUNTINGALLOCATIONSLIST.LEDGERNAME = salesOrder.productProfileDTO.defaultLedger;

            }
            else
            {
                aCCOUNTINGALLOCATIONSLIST.LEDGERNAME = salesLedger;
            }
            aCCOUNTINGALLOCATIONSLIST.GSTCLASS = "";
            aCCOUNTINGALLOCATIONSLIST.ISDEEMEDPOSITIVE = "No";
            aCCOUNTINGALLOCATIONSLIST.LEDGERFROMITEM = "No";
            aCCOUNTINGALLOCATIONSLIST.REMOVEZEROENTRIES = "No";
            aCCOUNTINGALLOCATIONSLIST.ISPARTYLEDGER = "No";
            aCCOUNTINGALLOCATIONSLIST.ISLASTDEEMEDPOSITIVE = "No";
            if (!isFreeItem)
            {

                aCCOUNTINGALLOCATIONSLIST.AMOUNT = salesOrder.rowTotal;

            }
            aLLINVENTORYENTRIESLIST.ACCOUNTINGALLOCATIONSLIST = aCCOUNTINGALLOCATIONSLIST;


            return aLLINVENTORYENTRIESLIST;

        }

      

        public double calculateTotal(List<SalesOrderItemDTO> salesOrderList)
        {
            double finalTotal = 0;
            foreach (SalesOrderItemDTO salesOrder in salesOrderList)
            {
                finalTotal += salesOrder.rowTotal;
            }
            return finalTotal;
        }

        public Dictionary<string, object>? NormalGenerateGSTWithVatCalculationXml(List<SalesOrderItemDTO> salesOrderList,
            List<String> vatLedgerList, String reduceTax, String roundOffLedger, String ledgerGstType, String kfcLedger,
            bool ifIgst, String productRateIncludingTax, String cessLedger="", double? docDiscountAmount = 0, double? docDiscountPercentage = 0)
        {
            double finalTotal = 0;
            double totalVat = 0;
            // double scgst = 0;
            double vat = 0;
            double kfc = 0;
            double igst = 0;
            double cess = 0;

            foreach (SalesOrderItemDTO salesOrder in salesOrderList)
            {
                double sellingRate = 0.0;

                if (reduceTax.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    sellingRate = salesOrder.sellingRate;
                    sellingRate = sellingRate * 100 / (100 + salesOrder.taxPercentage);
                }
                else
                {
                    sellingRate = salesOrder.sellingRate;
                }

                if (productRateIncludingTax != null && productRateIncludingTax.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    double totalTax = 0.0;

                    if (salesOrder.taxPercentage > 5 && ledgerGstType != null
                            && !"Regular".Equals(ledgerGstType) && kfcLedger.Count() > 0)
                    {

                        totalTax = salesOrder.taxPercentage + 1;

                    }
                    else
                    {

                        totalTax = salesOrder.taxPercentage;

                    }

                    sellingRate = (salesOrder.sellingRate * (100.0 / (100.0 + totalTax)));

                }

                sellingRate = round(sellingRate);
                // scgst = round(salesOrder.getRateOfVat() / 2.0);

                double itemTotalExeVat = round(sellingRate * salesOrder.quantity);
                double itemDiscountPercent = salesOrder.itemDiscount * 0.01;
                double discountAmount = round(itemTotalExeVat * itemDiscountPercent);
                LogManager.WriteLog(
                         "Item : " + salesOrder.itemName + "\tDiscount % : " + salesOrder.itemDiscount + " %");
                double totalAmount = itemTotalExeVat - discountAmount;
                totalAmount = round(totalAmount);
                vat += round(totalAmount * round(salesOrder.rateOfVat / 100.0));

                finalTotal += totalAmount;

                if (!"".Equals(kfcLedger) || kfcLedger.Count() > 0)
                    // KFC calculation
                    if (salesOrder.rateOfVat >= 12 && ledgerGstType != null
                            && !"Regular".Equals(ledgerGstType, StringComparison.OrdinalIgnoreCase) && kfcLedger.Count() > 0)
                    {
                        double kfcAmt = round(totalAmount * 0.01);
                        kfc = round(kfc + round(kfcAmt));
                    }

                // KFC calculation

                // Cess calculation

                if (!"".Equals(cessLedger) && cessLedger!=null)
                {
                    if (salesOrder.cessRateOfVat > 0)
                    {
                        cess += round(totalAmount * round(salesOrder.cessRateOfVat / 100.0));
                    }
                }
            }
            //test code for taxless sales download
            //4/1/2024
            if (vatLedgerList.Count()>0)
            {
                totalVat = vat;
            }
			totalVat = round(round(totalVat / 2) * 2);
            totalVat = round(totalVat);
            finalTotal = round(finalTotal) + round(kfc);// adding KFC
            finalTotal = round(finalTotal) + round(cess);// adding CESS
            finalTotal = finalTotal + totalVat;

            StringBuilder builder = new StringBuilder();
            double totalvatAmount = round(totalVat / 2);
            List<LEDGERENTRIESLIST> ledgerEntriesList = new List<LEDGERENTRIESLIST>();
            for (int i = 0; i < vatLedgerList.Count(); i++)
            {
                if (ifIgst)
                { // igst applicable
                    if (i == 2)
                    {
                        LEDGERENTRIESLIST ledgerentry = new LEDGERENTRIESLIST();
                        ledgerentry = getLegderEntriesList(vatLedgerList[i], totalVat);
                        ledgerEntriesList.Add(ledgerentry);
                    }
                }
                else
                {
                    if (i != 2)
                    {
                        LEDGERENTRIESLIST ledgerentry = new LEDGERENTRIESLIST();
                        ledgerentry = getLegderEntriesList(vatLedgerList[i], totalvatAmount);
                        ledgerEntriesList.Add(ledgerentry);
                    }
                }
            }
            // KFC calculation
            LEDGERENTRIESLIST Kfcledgerentry = new LEDGERENTRIESLIST();
            Kfcledgerentry = getLegderEntriesList(kfcLedger, kfc);
            ledgerEntriesList.Add(Kfcledgerentry);

            //cess calculation
            if (!cessLedger.Equals(""))
            {
                LEDGERENTRIESLIST ledgerentrieslist1 = new LEDGERENTRIESLIST();
                ledgerentrieslist1 = getLegderEntriesList(cessLedger, cess);
                ledgerEntriesList.Add(ledgerentrieslist1);
            }
            //Adding Discount Ledger
            if (IsDiscountLedgerEnabled.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                if (DiscountLedger != null && !DiscountLedger.Equals(""))
                {

                    LEDGERENTRIESLIST DiscountLedgerentry = new LEDGERENTRIESLIST();
                    DiscountLedgerentry = getLegderEntriesList(DiscountLedger, 0-docDiscountAmount.Value, true, 0-docDiscountPercentage.Value);
                    ledgerEntriesList.Add(DiscountLedgerentry);
                    finalTotal=finalTotal-docDiscountAmount.Value;
                    //finalTotal = Math.Round(finalTotal);
                }
            }


            if (roundOffLedger != null && roundOffLedger.Count() > 0 && !roundOffLedger.Equals(""))
            {
                double roundTo = Math.Round(finalTotal) - finalTotal;
                roundTo = round(roundTo);
                LEDGERENTRIESLIST ledgerentry = new LEDGERENTRIESLIST();
                ledgerentry = getLegderEntriesList(roundOffLedger, roundTo, false);
                ledgerEntriesList.Add(ledgerentry);

                finalTotal = Math.Round(finalTotal);

            }

  
            Dictionary<String, Object> hashMap = new Dictionary<String, Object>();
            hashMap.Add("finalTotal", round(finalTotal));
            hashMap.Add("vatXml", ledgerEntriesList);
            return hashMap;

        }

        public Dictionary<string, object>? NormalGenerateProductGSTWithVatCalculationXml(List<SalesOrderItemDTO> salesOrderList, List<string> vatLedgerList,
            string reduceTax, string roundOffLedger, string ledgerGstType, string kfcLedger, bool ifIgst, string productRateIncludingTax, List<string> taxRatesProduct, String cessLedger = "",double? docDiscountAmount=0,double? docDiscountPercentage=0)
        {
            double finalTotal = 0;
            double totalVat = 0;
            double totalVat1 = 0;
            double totalVat2 = 0;
            double totalVat3 = 0;
            double totalVat4 = 0;
            // double scgst = 0;
            double taxRate1 = 0;
            double taxRate2 = 0;
            double taxRate3 = 0;
            double taxRate4 = 0;

            double vat = 0;
            double vat1 = 0;
            double vat2 = 0;
            double vat3 = 0;
            double vat4 = 0;
            double kfc = 0;
            double cess = 0;
            double igst = 0;
            String vatLedgerCGST5 = vatLedgerList.ElementAtOrDefault(0);
            String vatLedgerSGST5 = vatLedgerList.ElementAtOrDefault(1);
            String vatLedgerCGST12 = vatLedgerList.ElementAtOrDefault(2);
            String vatLedgerSGST12 = vatLedgerList.ElementAtOrDefault(3);
            String vatLedgerCGST18 = vatLedgerList.ElementAtOrDefault(4);
            String vatLedgerSGST18 = vatLedgerList.ElementAtOrDefault(5);
            String vatLedgerCGST28 = vatLedgerList.ElementAtOrDefault(6);
            String vatLedgerSGST28 = vatLedgerList.ElementAtOrDefault(7);
            if (taxRatesProduct.Count > 0 && !taxRatesProduct[0].Equals("0") && !taxRatesProduct[0].Equals("0.0"))
            {
                Double.TryParse(taxRatesProduct.ElementAtOrDefault(0), out taxRate1);
                Double.TryParse(taxRatesProduct.ElementAtOrDefault(1), out taxRate2);
                Double.TryParse(taxRatesProduct.ElementAtOrDefault(2), out taxRate3);
                Double.TryParse(taxRatesProduct.ElementAtOrDefault(3), out taxRate4);
                //taxRate1 = StringUtilsCustom.ExtractDoubleValue(taxRatesProduct[0]);
                //taxRate2 = StringUtilsCustom.ExtractDoubleValue(taxRatesProduct[1]);
                //taxRate3 = StringUtilsCustom.ExtractDoubleValue(taxRatesProduct[2]);
                //taxRate4 = StringUtilsCustom.ExtractDoubleValue(taxRatesProduct[3]);
            }
            foreach (SalesOrderItemDTO salesOrder in salesOrderList)
            {
                double sellingRate = 0.0;
                if (reduceTax.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    sellingRate = salesOrder.sellingRate;
                    sellingRate = sellingRate * 100 / (100 + salesOrder.taxPercentage);
                }
                else
                {
                    sellingRate = salesOrder.sellingRate;
                }

                if (productRateIncludingTax != null && productRateIncludingTax.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    double totalTax = 0.0;

                    if (salesOrder.taxPercentage > 5 && ledgerGstType != null
                    && !ledgerGstType.Equals("Regular", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(kfcLedger))
                    {

                        totalTax = salesOrder.taxPercentage + 1;

                    }
                    else
                    {

                        totalTax = salesOrder.taxPercentage;

                    }

                    sellingRate = (salesOrder.sellingRate * (100.0 / (100.0 + totalTax)));
                }
                sellingRate = round(sellingRate);
                // scgst = round(salesOrder.getRateOfVat() / 2.0);

                double itemTotalExeVat = round(sellingRate * salesOrder.quantity);
                double itemDiscountPercent = salesOrder.itemDiscount * 0.01;
                double discountAmount = round(itemTotalExeVat * itemDiscountPercent);
                LogManager.WriteLog(
                 "Item : " + salesOrder.itemName + "\tDiscount % : " + salesOrder.itemDiscount + " %");

                double totalAmount = itemTotalExeVat - discountAmount;
                totalAmount = round(totalAmount);

                if (taxRate1 != 0 && salesOrder.rateOfVat == taxRate1)
                {
                    //vat1 += round(totalAmount * round(salesOrder.rateOfVat / 100.0));
                    //test code 16/4/2024
                    decimal testVat = (decimal)(totalAmount * round(salesOrder.rateOfVat / 100.0));
                    var totalvat1 = ((decimal)vat1+ testVat);
                    vat1=(double)totalvat1;
                    //vat1=(double)(Math.Truncate(totalvat1 * 100) / 100.0M);

                    //vat1 += (totalAmount * round(salesOrder.rateOfVat / 100.0));
                    //vat1=(Math.Truncate(vat1 * 100) / 100.0);

                    // finalTotal1 += totalAmount;
                }
                if (taxRate2 != 0 && salesOrder.rateOfVat == taxRate2)
                {

                    // vat2 += round(totalAmount * round(salesOrder.rateOfVat / 100.0));

                    //need to change the round
                    //test code 16/4/2024
                    decimal testVat =(decimal)(totalAmount * round(salesOrder.rateOfVat / 100.0));
                    var totalvat2 =((decimal)vat2+ testVat);
                    vat2=(double)totalvat2;

                    //vat2=(double)(Math.Truncate(totalvat2 * 100) / 100.0M);
                    // finalTotal2 += totalAmount;
                }

                if (taxRate3 != 0 && salesOrder.rateOfVat == taxRate3)
                {
                    decimal testVat = (decimal)(totalAmount * round(salesOrder.rateOfVat / 100.0));
                    var totalvat3 = ((decimal)vat3+ testVat);
                    vat3=(double)totalvat3;

                    //vat3=(double)(Math.Truncate(totalvat3 * 100) / 100.0M);

                    //vat3 +=testvat3;
                    //vat3=(Math.Truncate(vat3 * 100) / 100.0);

                    // finalTotal3 += totalAmount;
                }
                if (taxRate4 != 0 && salesOrder.rateOfVat == taxRate4)
                {
                    //vat4 += round(totalAmount * round(salesOrder.rateOfVat / 100.0));
                    //test code 16/4/2024
                    decimal testVat = (decimal)(totalAmount * round(salesOrder.rateOfVat / 100.0));
                    var totalvat4 = ((decimal)vat4+ testVat);
                    vat4=(double)totalvat4;

                    //vat4=(double)(Math.Truncate(totalvat4 * 100) / 100.0M);

                    //vat4 += testvat4;
                    //vat4=(Math.Truncate(vat4 * 100) / 100.0);
                    // finalTotal3 += totalAmount;
                }
                //vat += round(totalAmount * round(salesOrder.rateOfVat / 100.0));

                //test code 16/4/2024
                vat += (totalAmount * round(salesOrder.rateOfVat / 100.0));
                decimal finaltotaldecemal = (decimal)finalTotal + (decimal)totalAmount;
              
                finalTotal = (double)finaltotaldecemal;

                if (salesOrder.rateOfVat >= 12 && ledgerGstType != null && !"Regular".Equals(ledgerGstType, StringComparison.OrdinalIgnoreCase) && kfcLedger.Count() > 0)
                {
                    double kfcAmt = round(totalAmount * 0.01);
                    kfc = round(kfc + round(kfcAmt));
                }

                // Cess calculation
                if (!"".Equals(cessLedger) && cessLedger!=null)
                {
                    if (salesOrder.cessRateOfVat > 0)
                    {
                        cess += round(totalAmount * round(salesOrder.cessRateOfVat / 100.0));
                    }
                }

            }

            if (vat1 != 0)
            {
                totalVat1 = vat1;
                //totalVat1 = round(round(totalVat1 / 2) * 2);
                //totalVat1 = round(totalVat1);

                //test code
                //var testtotalvat = Math.Round(totalVat1, 2);
                //testtotalvat=(Math.Truncate(totalVat1 * 100) / 100.0);
                //totalVat1=testtotalvat;
               
            }
            if (vat2 != 0)
            {
                totalVat2 = vat2;
                //test code 16/4/2024
                //var testtotslvat = round((totalVat2 / 2) * 2);
                //var testtotalvat = Math.Round(totalVat2, 2);
                //testtotalvat=(Math.Truncate(totalVat2 * 100) / 100.0);
                ////totalVat2 = round(round(totalVat2 / 2) * 2);
                ////totalVat2 = round(totalVat2);
                ////test code 17/4/2024

                //totalVat2=testtotalvat;
               
            }
            if (vat3 != 0)
            {
                totalVat3 = vat3;

                //totalVat3 = round(round(totalVat3 / 2) * 2);
                //totalVat3 = round(totalVat3);

                ////test code
                //var testtotalvat = Math.Round(totalVat3, 2);
                //testtotalvat=(Math.Truncate(totalVat3 * 100) / 100.0);
                //totalVat3=testtotalvat;

              
            }

            if (vat4 != 0)
            {
                totalVat4 = vat4;
                //totalVat4 = round(round(totalVat4 / 2) * 2);
                //totalVat4 = round(totalVat4);

                ////test code
                //var testtotalvat = Math.Round(totalVat4, 2);
                //testtotalvat=(Math.Truncate(totalVat4 * 100) / 100.0);
                //totalVat4=testtotalvat;

            }


            double totalvatAmount1 = round(totalVat1 / 2);
            double totalvatAmount2 = round(totalVat2 / 2);

            double totalvatAmount3 = round(totalVat3 / 2);

            double totalvatAmount4 = round(totalVat4 / 2);

            //test code 17/4/2024
            //var totalvatAmount1 = (double)(Math.Truncate((totalVat1 / 2) * 100) / 100);
            //var totalvatAmount2 = (double)(Math.Truncate((totalVat2 / 2) * 100) / 100);
            //var totalvatAmount3 = (double)(Math.Truncate((totalVat3 / 2) * 100) / 100);
            //var totalvatAmount4 = (double)(Math.Truncate((totalVat4 / 2) * 100) / 100);

            //var totalvatAmount1 = (double)(Math.Truncate((totalVat1 / 2) * 100) / 100);
            //var totalvatAmount2 = (double)(Math.Truncate((totalVat2 / 2) * 100) / 100);
            //var totalvatAmount3 = (double)(Math.Truncate((totalVat3 / 2) * 100) / 100);
            //var totalvatAmount4 = (double)(Math.Truncate((totalVat4 / 2) * 100) / 100);

            totalVat = totalVat1 + totalVat2 + totalVat3 + totalVat4;
            //test code 18/4/2024
           var totalHalfVat=totalvatAmount1+totalvatAmount2+totalvatAmount3+totalvatAmount4;

            finalTotal = round(finalTotal) + round(kfc);// adding KFC
            finalTotal = round(finalTotal) + round(cess);// adding CESS
            //finalTotal = finalTotal + (totalVat*2);
           
            decimal finalTotaldecimal = (decimal)finalTotal +(decimal)totalVat;
            finalTotal=(double)finalTotaldecimal+ (totalHalfVat*2)-totalVat;

            //finalTotal=(Math.Truncate(finalTotal * 100) / 100.0);
            //double totalvatAmount = round(totalVat / 2);
            //test code 18/4/2024
            //double totalvatAmount = totalVat*2;
            double totalvatAmount = totalVat;

            List<LEDGERENTRIESLIST> alllegerEntriesList = new List<LEDGERENTRIESLIST>();
            for (int i = 0; i < vatLedgerList.Count(); i++)
            {
                if (ifIgst)
                { // igst applicable
                    if (i == 8)
                    {

                        LEDGERENTRIESLIST ledgerentrieslist = new LEDGERENTRIESLIST();
                        ledgerentrieslist = getLegderEntriesList(vatLedgerList[i], totalvatAmount);


                        alllegerEntriesList.Add(ledgerentrieslist);

                    }
                }
                else
                {
                    if (i != 8)
                    {
                        if (vat1 != 0)
                        {
                            if (vatLedgerList[i].Equals(vatLedgerCGST5) || vatLedgerList[i].Equals(vatLedgerSGST5))
                            {
                                LEDGERENTRIESLIST ledgerentrieslist = new LEDGERENTRIESLIST();
                                

                                ledgerentrieslist = getLegderEntriesList(vatLedgerList[i], totalvatAmount1);


                                alllegerEntriesList.Add(ledgerentrieslist);

                            }
                        }
                        if (vat2 != 0)
                        {
                            if (vatLedgerList[i].Equals(vatLedgerCGST12) || vatLedgerList[i].Equals(vatLedgerSGST12))
                            {
                                LEDGERENTRIESLIST ledgerentrieslist = new LEDGERENTRIESLIST();


                                ledgerentrieslist = getLegderEntriesList(vatLedgerList[i], totalvatAmount2);


                                alllegerEntriesList.Add(ledgerentrieslist);

                            }
                        }
                        if (vat3 != 0)
                        {
                            if (vatLedgerList[i].Equals(vatLedgerCGST18) || vatLedgerList[i].Equals(vatLedgerSGST18))
                            {
                                LEDGERENTRIESLIST ledgerentrieslist = new LEDGERENTRIESLIST();


                                ledgerentrieslist = getLegderEntriesList(vatLedgerList[i], totalvatAmount3);


                                alllegerEntriesList.Add(ledgerentrieslist);

                            }
                        }
                        if (vat4 != 0)
                        {
                            if (vatLedgerList[i].Equals(vatLedgerCGST28) || vatLedgerList[i].Equals(vatLedgerSGST28))
                            {
                                LEDGERENTRIESLIST ledgerentrieslist = new LEDGERENTRIESLIST();


                                ledgerentrieslist = getLegderEntriesList(vatLedgerList[i], totalvatAmount4);


                                alllegerEntriesList.Add(ledgerentrieslist);

                            }
                        }
                    }
                }
            }


            // KFC calculation
            LEDGERENTRIESLIST ledgerentrieslist1 = new LEDGERENTRIESLIST();
            ledgerentrieslist1 = getLegderEntriesList(kfcLedger, kfc);
            alllegerEntriesList.Add(ledgerentrieslist1);

            // Cess calculation
            LEDGERENTRIESLIST ledgerentriesCessList = new LEDGERENTRIESLIST();
            ledgerentriesCessList = getLegderEntriesList(cessLedger, cess);
            alllegerEntriesList.Add(ledgerentriesCessList);


            //adding discount ledger entry 
            if (IsDiscountLedgerEnabled.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                if (DiscountLedger != null && !DiscountLedger.Equals(""))
                {

                    LEDGERENTRIESLIST DiscountLedgerentry = new LEDGERENTRIESLIST();
                    DiscountLedgerentry = getLegderEntriesList(DiscountLedger, 0-docDiscountAmount.Value, true, 0-docDiscountPercentage.Value);
                    alllegerEntriesList.Add(DiscountLedgerentry);
                    finalTotal=finalTotal-docDiscountAmount.Value;
                    //finalTotal = Math.Round(finalTotal);
                }
            }


            //adding Roundoff ledger
            if (roundOffLedger != null && roundOffLedger!="")
            {
                var roundedvalue =(decimal) Math.Round(finalTotal, 0, MidpointRounding.AwayFromZero);
                var roundTo = roundedvalue - (decimal)finalTotal;
                roundTo=(Math.Truncate(roundTo * 100) / 100.0M);
                //double roundTo = round(finalTotal) - finalTotal;
                //roundTo = round(roundTo);
                LEDGERENTRIESLIST ledgerentrieslist2 = new LEDGERENTRIESLIST();
                ledgerentrieslist2 = getLegderEntriesList(roundOffLedger, (double)roundTo, false);
                alllegerEntriesList.Add(ledgerentrieslist2);
                finalTotal = Math.Round(finalTotal, 0, MidpointRounding.AwayFromZero);

            }
        

            Dictionary<String, Object> hashMap = new Dictionary<String, Object>
                    {
                        { "finalTotal", round(finalTotal) },
                        { "vatXml", alllegerEntriesList }
                    };
            return hashMap;

        }

        private double Truncate(double value)
        {
            var trucatedval=(decimal) (Math.Truncate(value * 100) / 100.0);
            return (double) trucatedval;
        }

        public LEDGERENTRIESLIST getLegderEntriesList(string ledgerName, double amount, bool IsvatAdded = true,double discountPercentage=0)
        {
            LEDGERENTRIESLIST ledgerentrieslist = new LEDGERENTRIESLIST();
            OLDAUDITENTRYIDSLIST oLDAUDITENTRYIDSLIST = new OLDAUDITENTRYIDSLIST();
            oLDAUDITENTRYIDSLIST.OLDAUDITENTRYIDS = "-1";
            oLDAUDITENTRYIDSLIST.TYPE = "Number";
            ledgerentrieslist.OLDAUDITENTRYIDSLIST = oLDAUDITENTRYIDSLIST;

            BASICRATEOFINVOICETAXLIST bASICRATEOFINVOICETAXLIST = new BASICRATEOFINVOICETAXLIST();
            bASICRATEOFINVOICETAXLIST.TYPE = "Number";
           
            if (discountPercentage!=0)
            {
                bASICRATEOFINVOICETAXLIST.BASICRATEOFINVOICETAX = discountPercentage;
            }
            ledgerentrieslist.BASICRATEOFINVOICETAXLIST = bASICRATEOFINVOICETAXLIST;
            ledgerentrieslist.ROUNDTYPE = "Normal Rounding";
            ledgerentrieslist.LEDGERNAME = ledgerName;
            ledgerentrieslist.GSTCLASS = "";
            ledgerentrieslist.ISDEEMEDPOSITIVE = "No";
            ledgerentrieslist.LEDGERFROMITEM = "No";
            ledgerentrieslist.REMOVEZEROENTRIES = "No";
            ledgerentrieslist.ISPARTYLEDGER = "No";
            ledgerentrieslist.ISLASTDEEMEDPOSITIVE = "No";
            ledgerentrieslist.AMOUNT = amount;
            if (IsvatAdded)
                ledgerentrieslist.VATEXPAMOUNT = amount;

            return ledgerentrieslist;
        }

        public string ConvertDate(string date)
        {
            string formattedDate = "";
            string[] splitDates = date.Split('T');
            DateTime dateTime = DateTime.ParseExact(splitDates[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            formattedDate = dateTime.ToString("dd-MM-yyyy");
            return formattedDate;
        }

        public async  Task<string> getGstNumber(string ledgerName)
        {
            string gstNumber = "";

            List<LocationDTO> _list = new List<LocationDTO>();
            DataTable response = new DataTable();
            StringBuilder Query = new StringBuilder();
            Query.Append("select $name,$PartyGSTIn from " + Tables.Ledger + " where $name = " + ledgerName);

            response = await tallyCommunicator.getdatatable(Query.ToString());

            if (response.Rows.Count > 0)
            {


                foreach (DataRow dr in response.Rows)
                {

                    gstNumber = (dr["$PartyGSTIn"] != DBNull.Value) ? (string)dr["$PartyGSTIn"] : "";
                }


            }
            return gstNumber;


        }


        public Dictionary<string, object>? GenerateGSTCalculationXml(List<SalesOrderItemDTO> salesOrderList, List<GstLedgerDTO> vatLedgerList, string roundOffLedger, double? docDiscountAmount = 0, double? docDiscountPercentage = 0)
        {
            double finalTotal = 0;
            double totalVat = 0;
            double scgst = 0;
            StringBuilder stringBuilder = new StringBuilder();

            Dictionary<double, List<SalesOrderItemDTO>> productTaxWise = salesOrderList.GroupBy(x => x.taxPercentage)
            .ToDictionary(x => x.Key, x => x.ToList());
            vatLedgerList.Sort((x, y) => x.taxRate.CompareTo(y.taxRate));
            foreach (GstLedgerDTO ledger in vatLedgerList)
            {
                if (!productTaxWise.TryGetValue(ledger.taxRate * 2, out List<SalesOrderItemDTO> taxWiseProducts))
                {
                    continue;
                }
                else if (taxWiseProducts.Count == 0)
                {
                    continue;
                }
                double gstAmt = 0.0;
                foreach (SalesOrderItemDTO dto in taxWiseProducts)
                {
                    double itemTotalWithoutTax = round(dto.sellingRate * dto.quantity);
                    double discountedAmt = round(itemTotalWithoutTax * (dto.itemDiscount / 100));
                    double totalWithoutTax = itemTotalWithoutTax - discountedAmt;
                    gstAmt += round(totalWithoutTax * (ledger.taxRate / 100));
                }
                ledger.totalTaxAmt = round(gstAmt);
            }
            List<LEDGERENTRIESLIST> alllegerEntriesList = new List<LEDGERENTRIESLIST>();
            foreach (GstLedgerDTO gstVat in vatLedgerList)
            {
                if (gstVat.totalTaxAmt == 0.0)
                {
                    continue;
                }
                LEDGERENTRIESLIST ledgerentrieslist = new LEDGERENTRIESLIST();
                OLDAUDITENTRYIDSLIST oLDAUDITENTRYIDSLIST = new OLDAUDITENTRYIDSLIST();
                oLDAUDITENTRYIDSLIST.OLDAUDITENTRYIDS = "-1";
                oLDAUDITENTRYIDSLIST.TYPE = "Number";
                ledgerentrieslist.OLDAUDITENTRYIDSLIST = oLDAUDITENTRYIDSLIST;

                BASICRATEOFINVOICETAXLIST bASICRATEOFINVOICETAXLIST = new BASICRATEOFINVOICETAXLIST();
                bASICRATEOFINVOICETAXLIST.TYPE = "Number";
                bASICRATEOFINVOICETAXLIST.BASICRATEOFINVOICETAX = scgst;
                ledgerentrieslist.BASICRATEOFINVOICETAXLIST = bASICRATEOFINVOICETAXLIST;
                ledgerentrieslist.ROUNDTYPE = "Normal Rounding";
                ledgerentrieslist.LEDGERNAME = gstVat.name;
                ledgerentrieslist.GSTCLASS = "";
                ledgerentrieslist.ISDEEMEDPOSITIVE = "No";
                ledgerentrieslist.LEDGERFROMITEM = "No";
                ledgerentrieslist.REMOVEZEROENTRIES = "No";
                ledgerentrieslist.ISPARTYLEDGER = "No";
                ledgerentrieslist.ISLASTDEEMEDPOSITIVE = "No";
                ledgerentrieslist.AMOUNT = gstVat.totalTaxAmt;
                ledgerentrieslist.VATEXPAMOUNT = gstVat.totalTaxAmt;


                alllegerEntriesList.Add(ledgerentrieslist);


            }

            double withoutTax = 0.0;
            double totalTax = vatLedgerList.Sum(gst => gst.totalTaxAmt);
            foreach (var item in salesOrderList)
            {
                double amount = round(item.quantity * item.sellingRate);
                double amountWithDiscount = amount - round(amount * (item.discountPercentage / 100));
                withoutTax += amountWithDiscount;
            }
            finalTotal = withoutTax + totalTax;


            //adding discountledger
            if (IsDiscountLedgerEnabled.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                if (DiscountLedger != null && !DiscountLedger.Equals(""))
                {

                    LEDGERENTRIESLIST DiscountLedgerentry = new LEDGERENTRIESLIST();
                    DiscountLedgerentry = getLegderEntriesList(DiscountLedger, 0-docDiscountAmount.Value, true, 0-docDiscountPercentage.Value);
                    alllegerEntriesList.Add(DiscountLedgerentry);
                    finalTotal=finalTotal-docDiscountAmount.Value;
                    //finalTotal = Math.Round(finalTotal);
                }
            }

            //Adding Rouud off ledger
            if (roundOffLedger != null && !roundOffLedger.Equals(""))
            {
                double roundTo = Math.Round(finalTotal) - finalTotal;

                LEDGERENTRIESLIST ledgerentrieslist = new LEDGERENTRIESLIST();
                OLDAUDITENTRYIDSLIST oLDAUDITENTRYIDSLIST = new OLDAUDITENTRYIDSLIST();
                oLDAUDITENTRYIDSLIST.OLDAUDITENTRYIDS = "-1";
                oLDAUDITENTRYIDSLIST.TYPE = "Number";
                ledgerentrieslist.OLDAUDITENTRYIDSLIST = oLDAUDITENTRYIDSLIST;

                BASICRATEOFINVOICETAXLIST bASICRATEOFINVOICETAXLIST = new BASICRATEOFINVOICETAXLIST();

                ledgerentrieslist.ROUNDTYPE = "Normal Rounding";
                ledgerentrieslist.LEDGERNAME = roundOffLedger;
                ledgerentrieslist.GSTCLASS = "";
                ledgerentrieslist.ISDEEMEDPOSITIVE = "No";
                ledgerentrieslist.LEDGERFROMITEM = "No";
                ledgerentrieslist.REMOVEZEROENTRIES = "No";
                ledgerentrieslist.ISPARTYLEDGER = "No";
                ledgerentrieslist.ISLASTDEEMEDPOSITIVE = "No";
                ledgerentrieslist.AMOUNT = roundTo;


                alllegerEntriesList.Add(ledgerentrieslist);

            }

            //for discount ledger
            //if (IsDiscountLedgerEnabled.Equals("true", StringComparison.OrdinalIgnoreCase))
            //{
            //    if (DiscountLedger != null && !DiscountLedger.Equals(""))
            //    {
            //        double roundTo = Math.Round(finalTotal) - finalTotal;

            //        LEDGERENTRIESLIST ledgerentrieslist = new LEDGERENTRIESLIST();
            //        OLDAUDITENTRYIDSLIST oLDAUDITENTRYIDSLIST = new OLDAUDITENTRYIDSLIST();
            //        oLDAUDITENTRYIDSLIST.OLDAUDITENTRYIDS = "-1";
            //        oLDAUDITENTRYIDSLIST.TYPE = "Number";
            //        ledgerentrieslist.OLDAUDITENTRYIDSLIST = oLDAUDITENTRYIDSLIST;

            //        BASICRATEOFINVOICETAXLIST bASICRATEOFINVOICETAXLIST = new BASICRATEOFINVOICETAXLIST();
            //        bASICRATEOFINVOICETAXLIST.TYPE = "Number";
            //        bASICRATEOFINVOICETAXLIST.BASICRATEOFINVOICETAX= 

            //    ledgerentrieslist.BASICRATEOFINVOICETAXLIST = bASICRATEOFINVOICETAXLIST;
            //        ledgerentrieslist.ROUNDTYPE = "Normal Rounding";
            //        ledgerentrieslist.LEDGERNAME = gstVat.name;
            //        ledgerentrieslist.GSTCLASS = "";
            //        ledgerentrieslist.ISDEEMEDPOSITIVE = "No";
            //        ledgerentrieslist.LEDGERFROMITEM = "No";
            //        ledgerentrieslist.REMOVEZEROENTRIES = "No";
            //        ledgerentrieslist.ISPARTYLEDGER = "No";
            //        ledgerentrieslist.ISLASTDEEMEDPOSITIVE = "No";
            //        ledgerentrieslist.AMOUNT = gstVat.totalTaxAmt;
            //        ledgerentrieslist.VATEXPAMOUNT = gstVat.totalTaxAmt;



            //        ledgerentrieslist.ROUNDTYPE = "Normal Rounding";
            //        ledgerentrieslist.LEDGERNAME = roundOffLedger;
            //        ledgerentrieslist.GSTCLASS = "";
            //        ledgerentrieslist.ISDEEMEDPOSITIVE = "No";
            //        ledgerentrieslist.LEDGERFROMITEM = "No";
            //        ledgerentrieslist.REMOVEZEROENTRIES = "No";
            //        ledgerentrieslist.ISPARTYLEDGER = "No";
            //        ledgerentrieslist.ISLASTDEEMEDPOSITIVE = "No";
            //        ledgerentrieslist.AMOUNT = roundTo;


            //        alllegerEntriesList.Add(ledgerentrieslist);

            //    }
            //}

            Dictionary<String, Object> hashMap = new Dictionary<String, Object>();
            hashMap.Add("finalTotal", finalTotal);
            hashMap.Add("vatXml", alllegerEntriesList);
            return hashMap;


        }
        public static double round(double number)
        {
            return Math.Round(number, 2, MidpointRounding.AwayFromZero);
        }
    }

    }




