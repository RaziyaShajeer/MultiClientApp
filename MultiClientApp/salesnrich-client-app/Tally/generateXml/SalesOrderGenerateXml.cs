using dtos;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Parsers;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Services;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Tally.generateXml
{
    public class SalesOrderGenerateXml
    {
        private String isOptionalsalesOrder= ApplicationProperties.properties["is.optional.salesOrder"].ToString();
        private String companyState = ApplicationProperties.properties["company.state"].ToString();
        private String orderNumberWithEmployee = ApplicationProperties.properties["order.employee.name"].ToString();
        private String enableDateWise = ApplicationProperties.properties["enable.date"].ToString();
        private String userStockLocation;

        private String companyString = ApplicationProperties.properties["tally.company"].ToString();

        private String apiUrl;
        private String byEmpVoucher;

        private String employeeString;
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
        public String cessLedger= ApplicationProperties.properties["Cess.ledger.name"].ToString();
        private String invoiceNumberAsReference = ApplicationProperties.properties["invoice.number.as.reference"].ToString();
        private String salesOrderNumberAsReference = ApplicationProperties.properties["salesorder.number.as.reference"].ToString();
        //private String godownFixed = ApplicationProperties.properties["godown.fixed"].ToString();
        //private String batchFixed = ApplicationProperties.properties["batch.fixed"].ToString();
        //private String salesLedger = ApplicationProperties.properties["sales.ledger"].ToString();
        SalesCalculationXml SalesCalculationXml ;
        public SalesOrderGenerateXml()
        {
            SalesCalculationXml = new SalesCalculationXml();
        }
        public async Task<ENVELOPE> generateSalesOrderXmlAsync(SalesOrderDTO config)
        {
            List<LEDGERENTRIESLIST> alllegerEntriesList = new List<LEDGERENTRIESLIST>();
            string trimChar = config.trimChar == null ? "" : config.trimChar;
            string ledgerName = config.ledgerName+ trimChar;
            Console.WriteLine(ledgerName);
            Console.WriteLine(trimChar);
            string companyName =ApplicationProperties.properties["tally.company"].ToString();
            string uuid = Guid.NewGuid().ToString();
            char[] str = uuid.ToCharArray();
            string orderReference = KeyGeneratorUtil.GetRandomCustomString(str, 8);
            bool isOptional = false;

            if (isOptionalsalesOrder.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                isOptional = true;
            }

            bool ifIgst = false;
            string ledgerState = config.ledgerState;
            Console.WriteLine("Ledger state " + ledgerState + " | company state" + companyState + "|");
            if (ledgerState != null && ledgerState.Trim().Length > 0 && !ledgerState.Trim().Equals("Not Applicable", StringComparison.OrdinalIgnoreCase))
            {
                if (companyState != null && companyState.Length > 0 && !ledgerState.Equals(companyState, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("igst applicable");
                    ifIgst = true;
                }
            }
            LedgerToGstNumberResponseParser ledgerToGstNumberResponseParser = new LedgerToGstNumberResponseParser();
            String gstNumber = await ledgerToGstNumberResponseParser.getGstNumberAsync(ledgerName);

            string date = StringUtilsCustom.FormatDate(config.date.ToString());
            string dynamicDate = ApplicationProperties.properties["salessorderDate"].ToString();
            if (dynamicDate != null && !dynamicDate.Equals("", StringComparison.OrdinalIgnoreCase))
            {
                date = StringUtilsCustom.FormatDate(dynamicDate);
            }

          //  string oderReference = KeyGeneratorUtil.GetRandomCustomString(str, 8);
            StringBuilder orderIds = new StringBuilder();
            if (orderNumberWithEmployee.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                string employeeName = config.inventoryVoucherHeaderDTO.employeeName.Replace(" ", "").ToUpper().Substring(0, 3);
                orderReference = employeeName + "-" + orderReference;
                Console.WriteLine(orderReference);
            }

            bool isVat = true;
            bool diffQuantity = true;
            diffQuantity = actualBilledStatus.Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false;

            double finalTotal = 0;
            StringBuilder stringBuilder = new StringBuilder();

            List<string> ledgerDTOs = new List<string>(gstNames.Split(','));

            string SALTCHARS = "1234567890";
            var salt = new StringBuilder();
            var rnd = new Random();
            while (salt.Length < 8)
            {
                int index = (int)(rnd.NextDouble() * SALTCHARS.Length);
                salt.Append(SALTCHARS[index]);
            }
            string saltStr = salt.ToString();

            string voucherRemoteId = "4ea2d475-dac6-4155-a2d7-32220d3a168a-" + orderReference;
            string voucherKey = "4ea2d475-dac6-4155-a2d7-32220d3a168a-0000a637:" + saltStr;


            ADDRESSLIST Address = new();
            // string basicBuyyerAddress = "";
            List<BASICBUYERADDRESSLIST> basicBuyyerAddress = new();
            if (config.ledgerAddress != null)
            {
                List<String> addressListss = new();
				BASICBUYERADDRESSLIST bASICBUYERADDRESSLIST = new();
				bASICBUYERADDRESSLIST.TYPE = "String";

				String[] addressList = config.ledgerAddress.Split("~");
                foreach (string address in addressList)
                {
                    if (!address.Contains("No Address"))
                    {

                        Console.WriteLine("Address:- " + address);
                        //BASICBUYERADDRESSLIST bASICBUYERADDRESSLIST = new();
                        //bASICBUYERADDRESSLIST.TYPE = "String";
                        //bASICBUYERADDRESSLIST.Text = address;
                        //basicBuyyerAddress.Add(bASICBUYERADDRESSLIST);
                        addressListss.Add(address);


                        //Address += "<ADDRESS>" + address + "</ADDRESS>";
                        //basicBuyyerAddress += "<BASICBUYERADDRESS>" + address + "</BASICBUYERADDRESS>";
                    }
                }
                Address.TYPE = "String";
                Address.ADDRESS = addressListss;
                bASICBUYERADDRESSLIST.BASICBUYERADDRESS=addressListss;
				basicBuyyerAddress.Add(bASICBUYERADDRESSLIST);
			}

            if (isVat)
            {


                if (string.IsNullOrEmpty(kfcLedger))
                {
                    kfcLedger = "";
                    config.ledgerGstType = "Regular";
                }
                else
                {
                    if (string.IsNullOrEmpty(config.ledgerGstType))
                    {
                        config.ledgerGstType = "Regular";
                    }
                }

                Console.WriteLine("Ledger : " + ledgerName);
                Dictionary<string, object> output = null;
                foreach (SalesOrderItemDTO dto in config.salesOrderItemDTOs)
                {
                    double discountedAmt = dto.discountAmount;
                    double sellingRate = dto.sellingRate;
                    dto.sellingRate = sellingRate - discountedAmt;
                }
                if (string.Equals(gstCalculationEnabled, "true", StringComparison.OrdinalIgnoreCase))
                {
                    List<GstLedgerDTO> vatLedgerList = new List<GstLedgerDTO>();
                    // fetch the list of gst ledgers
                    vatLedgerList = config.gstLedgerDtos;
                    output = SalesCalculationXml.GenerateGSTCalculationXml(config.salesOrderItemDTOs, vatLedgerList, roundOffLedger);
                }
                else
                {
                    if (string.Equals(multiTax, "true", StringComparison.OrdinalIgnoreCase))
                    {
                        LogManager.WriteLog("multitax enter--------");
                        List<string> vatTaxs = new List<string>(gstTax.Split(','));
                        output = SalesCalculationXml.NormalGenerateProductGSTWithVatCalculationXml(config.salesOrderItemDTOs, ledgerDTOs, reduceTax, roundOffLedger, config.ledgerGstType, kfcLedger, ifIgst, productRateIncludingTax, vatTaxs, cessLedger);
                    }
                    else
                    {
                        output = SalesCalculationXml.NormalGenerateGSTWithVatCalculationXml(
                        config.salesOrderItemDTOs, ledgerDTOs, reduceTax, roundOffLedger,
                        config.ledgerGstType, kfcLedger, ifIgst, productRateIncludingTax,cessLedger);
                    }

                }
                finalTotal += (double)output["finalTotal"];

                alllegerEntriesList.AddRange((IEnumerable<LEDGERENTRIESLIST>)output["vatXml"]);


            }
            else
            {
                finalTotal = SalesCalculationXml.calculateTotal(config.salesOrderItemDTOs);
            }
            finalTotal = SalesCalculationXml.round(finalTotal);




            //generate xml object


            ENVELOPE tallyRequest = new ENVELOPE();
            HEADER header = new HEADER();

            header.TALLYREQUEST = "Import Data";

            tallyRequest.HEADER = header;

            BODY body = new();
            IMPORTDATA importdata = new IMPORTDATA();
            REQUESTDESC requestDesc = new();
            requestDesc.REPORTNAME = "All Masters";
            STATICVARIABLES staticvariables = new STATICVARIABLES();

            staticvariables.SVCURRENTCOMPANY = ApplicationProperties.properties["tally.company"].ToString();

            requestDesc.STATICVARIABLES = staticvariables;


            importdata.REQUESTDESC = requestDesc;

            REQUESTDATA requestData = new();
            List<TALLYMESSAGE> talllyMessagesList = new();
            TALLYMESSAGE tallymessage = new();
            tallymessage.UDF = "TallyUDF";

            VOUCHER voucher = new VOUCHER();
            //voucher.REMOTEID = voucherRemoteId;
            voucher.VCHTYPE = "Sales Order";
            voucher.ACTION = "Create";
            voucher.DATE = date;
            voucher.ISOPTIONAL = (isOptional) ? "Yes" : "No";

            if (config.inventoryVoucherHeaderDTO.priceLevelName != null
                && !config.inventoryVoucherHeaderDTO.priceLevelName.Equals(""))
            {
                voucher.PRICELEVEL = config.inventoryVoucherHeaderDTO.priceLevelName;

            }
            if (gstNumber != null)
            {
                voucher.PARTYGSTIN = gstNumber;
                voucher.CONSIGNEEGSTIN = gstNumber;
            }
            String activityRemarks = "";
            if (salesOrderActivityRemark.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                activityRemarks = config.activityRemarks;

            }

            if (itemRemarksEnabled.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                //voucher.NARRATION

                string narration = "";

                foreach (var soi in config.salesOrderItemDTOs)
                {
                    Console.WriteLine(soi.remarks);
                    if (!string.IsNullOrEmpty(soi.remarks))
                    {


                        narration += (soi.itemName + "(" + soi.remarks + "),");
                    }
                }
                narration += (activityRemarks);
                voucher.NARRATION = narration;
            }
            else
            {
                if ("cash".Equals(enableCashOnlyLedgerEntry,StringComparison.OrdinalIgnoreCase))
                {

                    //voucher.NARRATION = (ledgerName + "," + Address.ADDRESS.ToString() + "," + activityRemarks);
                    voucher.NARRATION = (ledgerName + " , " + activityRemarks);

                }
                else if (config.narration != null)
                {

                    voucher.NARRATION = (config.narration + "," + activityRemarks);
                }
                else
                {
                    voucher.NARRATION = (activityRemarks);
                }

            }
            if (ledgerName.Equals("Cash", StringComparison.OrdinalIgnoreCase))
            {
                ledgerName = "Cash";
                ADDRESSLIST aDDRESSLIST = new ADDRESSLIST();

                aDDRESSLIST = Address;
                voucher.ADDRESSLIST = aDDRESSLIST;

                voucher.BASICBUYERADDRESSLIST = basicBuyyerAddress;



            }
            else
            {

                ADDRESSLIST aDDRESSLIST = new ADDRESSLIST();

                aDDRESSLIST = Address;
                voucher.ADDRESSLIST = aDDRESSLIST;

                voucher.BASICBUYERADDRESSLIST = basicBuyyerAddress;
            }

            if ("cash".Equals(enableCashOnlyLedgerEntry, StringComparison.OrdinalIgnoreCase))
            {
                ledgerName = enableCashOnlyLedgerEntry;
            }
            voucher.PARTYNAME = ledgerName;
            voucher.VOUCHERTYPENAME = "Sales Order";
            voucher.VOUCHERTYPECLASS = "Vat";
            if (ApplicationProperties.properties["DocumentNoAsVoucher"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                voucher.VOUCHERNUMBER = config.inventoryVoucherHeaderDTO.documentNumberServer;
                voucher.REFERENCE=config.inventoryVoucherHeaderDTO.documentNumberServer;
            }
            else
            {
                voucher.VOUCHERNUMBER = orderReference;
                voucher.REFERENCE = orderReference;
            }

               

            
            //if (invoiceNumberAsReference.Equals("true", StringComparison.OrdinalIgnoreCase))
            //{
            //    voucher.REFERENCE = config.inventoryVoucherHeaderDTO.documentNumberServer;
            //}
            

            voucher.PARTYLEDGERNAME = ledgerName;
            voucher.PARTYPINCODE = config.ledgerPinCode;
            voucher.BASICBASEPARTYNAME = ledgerName;
            voucher.FBTPAYMENTTYPE = "Default";
            // checks different quantity is enabled or not
            if (diffQuantity)
            {
                voucher.DIFFACTUALQTY = "Yes";

            }
            if (companyState == null || companyState.Length <= 0 || companyState.Equals(""))
            {
                companyState = "Kerala";
            }
            voucher.PERSISTEDVIEW = "Invoice Voucher View";
            voucher.ISOPTIONAL = (isOptional) ? "Yes" : "No";
            voucher.BASICBUYERNAME = config.mailingName;
            voucher.STATENAME = (config.ledgerState == null || config.ledgerState == "") ? companyState
                : config.ledgerState;
            voucher.COUNTRYOFRESIDENCE = (config.ledgerCountry == null || config.ledgerCountry == "") ? "India"
                : config.ledgerCountry;
            voucher.PLACEOFSUPPLY = (config.ledgerState == null || config.ledgerState == "") ? companyState
                : config.ledgerState;
            voucher.CONSIGNEEMAILINGNAME = config.mailingName;
            voucher.CONSIGNEECOUNTRYNAME = "India";
            voucher.CONSIGNEESTATENAME = companyState;
            voucher.CONSIGNEEPINCODE=config.ledgerPinCode;
            if ("Yes".Equals(enableCostCentre, StringComparison.OrdinalIgnoreCase))
            {
                voucher.COSTCENTRENAME = config.inventoryVoucherHeaderDTO.employeeName;
                voucher.ISCOSTCENTRE = enableCostCentre;
            }
            // Payment mode set for Document Type with CD (cash discount) in alias
            if (!"".Equals(paymentModeOrTerms) && paymentModeOrTerms != null && config.documentAlias != null
                    && config.documentAlias.Equals("CD"))
            {
                voucher.BASICDUEDATEOFPYMT = paymentModeOrTerms;
            }

            List<LEDGERENTRIESLIST> legersEntriesList = new List<LEDGERENTRIESLIST>();
            LEDGERENTRIESLIST ledgerEntriy = new LEDGERENTRIESLIST();
            ledgerEntriy.LEDGERNAME = ledgerName;
            ledgerEntriy.ISDEEMEDPOSITIVE = "Yes";
            ledgerEntriy.LEDGERFROMITEM = "No";
            ledgerEntriy.REMOVEZEROENTRIES = "No";
            ledgerEntriy.ISPARTYLEDGER = "Yes";
            ledgerEntriy.ISLASTDEEMEDPOSITIVE = "Yes";
            ledgerEntriy.AMOUNT = 0 - finalTotal;
            legersEntriesList.Add(ledgerEntriy);
            voucher.LEDGERENTRIESLIST = legersEntriesList;

            //need to check this code 
            legersEntriesList.AddRange(alllegerEntriesList);
           // alllegerEntriesList.AddRange(legersEntriesList);

            // looping sales order entities
            List<ALLINVENTORYENTRIESLIST> allINVENTORYENTRIESLIST = new();
            foreach (SalesOrderItemDTO salesOrderItem in config.salesOrderItemDTOs)
            {
                ALLINVENTORYENTRIESLIST itemXml = new ALLINVENTORYENTRIESLIST();
                itemXml = SalesCalculationXml.generateSingleItemXml(salesOrderItem, config, false);
                allINVENTORYENTRIESLIST.Add(itemXml);


                if (!diffQuantity)
                {
                    if (salesOrderItem.itemFreeQuantity > 0)
                    {
                        ALLINVENTORYENTRIESLIST freeItemXml = SalesCalculationXml.generateSingleItemXml(salesOrderItem, config, true);
                        allINVENTORYENTRIESLIST.Add(freeItemXml);
                    }
                }
            }
            voucher.ALLINVENTORYENTRIESLIST = allINVENTORYENTRIESLIST;
            //voucher.LEDGERENTRIESLIST = alllegerEntriesList;
            //need to verify this code
            voucher.LEDGERENTRIESLIST = legersEntriesList;

            tallymessage.VOUCHER = voucher;
            talllyMessagesList.Add(tallymessage);
            requestData.TALLYMESSAGE = talllyMessagesList;
            importdata.REQUESTDATA = requestData;
            body.IMPORTDATA = importdata;
            tallyRequest.BODY = body;
            return tallyRequest;

        }
    }
}
