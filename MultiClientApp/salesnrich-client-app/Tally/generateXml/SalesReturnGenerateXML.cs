using dtos;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Parsers;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Services;
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
    public class SalesReturnGenerateXML
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
        private String invoiceNumberAsReference = ApplicationProperties.properties["invoice.number.as.reference"].ToString();
        private String salesOrderNumberAsReference = ApplicationProperties.properties["salesorder.number.as.reference"].ToString();
        private String enableReceiptVoucherType = ApplicationProperties.properties["enable.receipt.voucherType"].ToString();
        private String godownInSales = ApplicationProperties.properties["godown.in.sales.receipt"].ToString();
        private string godownName = ApplicationProperties.properties["godownName"].ToString();

        public String cessLedger = ApplicationProperties.properties["Cess.ledger.name"].ToString();
        private String donloadVehicleDetails = ApplicationProperties.properties["downloadVehicleDetails"].ToString();

        //for discount ledger
        public String IsDiscountLedgerEnabled = ApplicationProperties.properties["enable.discount.ledger"].ToString();
        public String DiscountLedger = ApplicationProperties.properties["discount.ledger"].ToString();
        public String salesreturnvouchertype=ApplicationProperties.properties["salesreturnvouchertype"].ToString();
        public string salesReturnLedgerName = ApplicationProperties.properties["salesReturnLedgerName"].ToString();
        TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        SalesCalculationXml salesCalculation = new SalesCalculationXml();
        public SalesReturnGenerateXML()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            enableDateWise = ApplicationProperties.properties["enable.date"].ToString();
            companyString = ApplicationProperties.properties["tally.company"].ToString();
            apiUrl = ApplicationProperties.properties["service.full.url"].ToString();
            byEmpVoucher = ApplicationProperties.properties["download.by.employee.voucher"].ToString();
            userStockLocation = ApplicationProperties.properties["user.stockLocation"].ToString();
        }
        public async Task<ENVELOPE> generateSalesOrderXml(SalesOrderDTO config)
        {

            List<LEDGERENTRIESLIST> alllegerEntriesList = new List<LEDGERENTRIESLIST>();
            //if (byEmpVoucher.Equals("true", StringComparison.OrdinalIgnoreCase))
            //{
            string trimChar = config.trimChar == null ? "" : config.trimChar;
            string ledgerName = config.ledgerName + trimChar;
            string companyName = ApplicationProperties.properties["tally.company"].ToString();

            string uuid = Guid.NewGuid().ToString();
            char[] str = uuid.ToCharArray();
            string orderReference = KeyGeneratorUtil.GetRandomCustomString(str, 8);

            bool isOptional = false;

            if (isOptionalsalesOrder.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                isOptional = true;
            }

            bool ifIgst = false;
            string ledgerState = config.ledgerState.Trim();
            Console.WriteLine("Ledger state " + ledgerState + " | company state" + companyState + "|");
            if (ledgerState != null && ledgerState.Trim().Length > 0 && !ledgerState.Trim().Equals("Not Applicable", StringComparison.OrdinalIgnoreCase))
            {
                if (companyState != null && companyState.Length > 0 && !ledgerState.Equals(companyState, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("igst applicable");
                    ifIgst = true;
                }
            }
            String gstNumber = "";
            //String gstNumber = getGstNumber(ledgerName);
            LedgerToGstNumberResponseParser ledgerToGstNumberResponseParser = new LedgerToGstNumberResponseParser();
            gstNumber =await ledgerToGstNumberResponseParser.getGstNumberAsync(gstNumber);
            if (String.IsNullOrEmpty(gstNumber))
            {

                TallyService tallyService = new TallyService();
                gstNumber= await tallyService.GetGstNumberOfLedgerXMLAsync(ledgerName);
            }

            String date = ConvertDate(config.date.ToString());
            string dynamicDate = ApplicationProperties.properties["salessorderDate"].ToString();
            if (dynamicDate != null && !dynamicDate.Equals(""))
            {
                date = ConvertDate(dynamicDate);
            }
            // append employee name with order number
            if (orderNumberWithEmployee.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                var employeeName = config.inventoryVoucherHeaderDTO.employeeName.Replace(" ", "").ToUpper().Substring(0, 3);
                orderReference = employeeName + "-" + orderReference;
                Console.WriteLine(orderReference);
            }
            // config.IsVat
            bool isVat = true;
            // diffQuantity -> do we need 2 different quantity (billed and actual)
            bool diffQuantity = true;
            diffQuantity = actualBilledStatus.Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false;

            // sales order total amount including vat
            double finalTotal = 0;
            // string builder for vat XML
            var stringBuilder = new StringBuilder();

            // checks vat option is enabled or not
            List<string> ledgerDTOs = new List<string>();
            //test code 4/1/2024
            if (!string.IsNullOrEmpty(gstNames))
            {
                ledgerDTOs = new List<string>(gstNames.Split(','));
            }


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

            BASICBUYERADDRESSLIST bASICBUYERADDRESSLIST = new();
            List<BASICBUYERADDRESSLIST> basicBuyyerAddress = new();
            if (config.ledgerAddress != null)
            {
                List<String> addressListss = new();

                String[] addressList = config.ledgerAddress.Split("~");
                foreach (string address in addressList)
                {
                    if (!address.Contains("No Address", StringComparison.OrdinalIgnoreCase))
                    {

                        Console.WriteLine("Address:- " + address);

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
                bASICBUYERADDRESSLIST.TYPE = "String";
                bASICBUYERADDRESSLIST.BASICBUYERADDRESS =addressListss;
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
                    output = salesCalculation.GenerateGSTCalculationXml(config.salesOrderItemDTOs, vatLedgerList, roundOffLedger, config.docDiscountAmount, config.docDiscountPercentage);
                }
                else
                {
                    if (string.Equals(multiTax, "true", StringComparison.OrdinalIgnoreCase))
                    {
                        LogManager.WriteLog("multitax enter--------");
                        List<string> vatTaxs = new List<string>(gstTax.Split(','));

                        output = salesCalculation.NormalGenerateProductGSTWithVatCalculationXml(config.salesOrderItemDTOs, ledgerDTOs, reduceTax, roundOffLedger, config.ledgerGstType, kfcLedger,
                            ifIgst, productRateIncludingTax, vatTaxs, cessLedger, config.docDiscountAmount, config.docDiscountPercentage);
                    }
                    else
                    {
                        output = salesCalculation.NormalGenerateGSTWithVatCalculationXml(
                        config.salesOrderItemDTOs, ledgerDTOs, reduceTax, roundOffLedger,
                        config.ledgerGstType, kfcLedger, ifIgst, productRateIncludingTax, cessLedger, config.docDiscountAmount, config.docDiscountPercentage);
                    }

                }
                finalTotal += (double)output["finalTotal"];

                alllegerEntriesList.AddRange((IEnumerable<LEDGERENTRIESLIST>)output["vatXml"]);


            }
            else
            {
                finalTotal = calculateTotal(config.salesOrderItemDTOs);
            }

            //generate xmlObject

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
            //requestDesc.REPORTNAME = "All Masters";
            REQUESTDATA requestData = new();
            List<TALLYMESSAGE> talllyMessagesList = new();
            TALLYMESSAGE tallymessage = new();
            tallymessage.UDF = "TallyUDF";

            VOUCHER voucher = new VOUCHER();
            if ("true".Equals(byEmpVoucher, StringComparison.OrdinalIgnoreCase))
            {
                String employeeAlias = config.employeeAlias;
                //if ("true".Equals(enableReceiptVoucherType, StringComparison.OrdinalIgnoreCase))
                //{

                String[] voucherTypes = employeeAlias.Split("~");

                if (voucherTypes.ElementAtOrDefault(1) != null)
                {
                    employeeAlias = voucherTypes[1];
                }

                //}
                //employeeAlias = employeeAlias.Replace("&", "&amp;");
                importdata.REQUESTDESC = requestDesc;

                voucher.REMOTEID = voucherRemoteId;
                voucher.VCHKEY= voucherKey;
                voucher.VCHTYPE = salesreturnvouchertype;
                voucher.OBJVIEW = "Invoice Voucher View";
                voucher.ACTION = "Create";
                voucher.DATE = date;

            }
            else
            {
                importdata.REQUESTDESC = requestDesc;
                voucher.REMOTEID = voucherRemoteId;
                voucher.VCHKEY = voucherKey;
                voucher.VCHTYPE =salesreturnvouchertype;
                voucher.OBJVIEW = "Invoice Voucher View";
                voucher.ACTION = "Create";
                voucher.DATE = date;
            }
            //REQUESTDATA requestData = new();
            //List<TALLYMESSAGE> talllyMessagesList = new();
            //TALLYMESSAGE tallymessage = new();
            tallymessage.UDF = "TallyUDF";


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
            if (!String.IsNullOrEmpty(config.narration))
            {
                voucher.NARRATION = config.narration;
            }
            String activityRemarks = "";
            //if (salesOrderActivityRemark.Equals("true", StringComparison.OrdinalIgnoreCase))
            //{
            //    activityRemarks = config.activityRemarks;

            //}

            //if (itemRemarksEnabled.Equals("true", StringComparison.OrdinalIgnoreCase))
            //{
            //    //voucher.NARRATION

            //    string narration = "";

            //    foreach (var soi in config.salesOrderItemDTOs)
            //    {
            //        Console.WriteLine(soi.remarks);
            //        if (!string.IsNullOrEmpty(soi.remarks))
            //        {


            //            narration += (soi.itemName + "(" + soi.remarks + "),");
            //        }
            //    }
            //    narration += (activityRemarks);
            //    voucher.NARRATION = narration;
            //}
            //else
            //{
            //    if ("cash".Equals(enableCashOnlyLedgerEntry))
            //{

            //    voucher.NARRATION = (ledgerName + "," + Address + "," + activityRemarks);

            //}
            //else if (config.narration != null)
            //{

            //    voucher.NARRATION = (config.narration + "," + activityRemarks);
            //}
            //else
            //{
            //    voucher.NARRATION = (activityRemarks);
            //}

            //}
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

            //if ("cash".Equals(enableCashOnlyLedgerEntry, StringComparison.OrdinalIgnoreCase))
            //{
            //    ledgerName = enableCashOnlyLedgerEntry;
            //}
            voucher.PARTYNAME = ledgerName;
            if ("true".Equals(byEmpVoucher, StringComparison.OrdinalIgnoreCase))
            {

                String employeeAlias = config.employeeAlias;

                //if ("true".Equals(enableReceiptVoucherType,StringComparison.OrdinalIgnoreCase))
                //{

                String[] voucherTypes = employeeAlias.Split("~");

                if (voucherTypes !=null && voucherTypes.ElementAtOrDefault(1) != null)
                {
                    employeeAlias = voucherTypes[1];
                }

                //}
                //employeeAlias = employeeAlias.Replace("&", "&amp;");
                voucher.VOUCHERTYPENAME = salesreturnvouchertype;
            }
            else
            {
                voucher.VOUCHERTYPENAME =salesreturnvouchertype;
                //builder.append("<VOUCHERTYPENAME>Sales</VOUCHERTYPENAME>");
            }

            //voucher.VOUCHERTYPENAME = "Sales Order";
            voucher.VOUCHERTYPECLASS = "Vat";
            voucher.VOUCHERNUMBER = config.inventoryVoucherHeaderDTO.documentNumberServer;
            voucher.REFERENCE = orderReference;
            if (invoiceNumberAsReference.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                voucher.REFERENCE = config.inventoryVoucherHeaderDTO.documentNumberServer;
            }
            if (salesOrderNumberAsReference.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                if (config.salesOrderRefNumber != null)
                {

                    voucher.REFERENCE = config.salesOrderRefNumber;
                }
                else
                {
                    voucher.REFERENCE = "";
                }
            }

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
            voucher.ISINVOICE = "Yes";
            voucher.VATISASSESABLECALCVCH = "Yes";
            voucher.ISCOSTCENTRE = "Yes";
            voucher.HASCASHFLOW = "Yes";
            //if ("Yes".Equals(enableCostCentre, StringComparison.OrdinalIgnoreCase))
            //{
            //    voucher.COSTCENTRENAME = config.inventoryVoucherHeaderDTO.employeeName;
            //    voucher.ISCOSTCENTRE = enableCostCentre;
            //}
            //// Payment mode set for Document Type with CD (cash discount) in alias
            //if (!"".Equals(paymentModeOrTerms) && paymentModeOrTerms != null && config.documentAlias != null
            //        && config.documentAlias.Equals("CD"))
            //{
            //    voucher.BASICDUEDATEOFPYMT = paymentModeOrTerms;
            //}

            List<LEDGERENTRIESLIST> legersEntriesList = new List<LEDGERENTRIESLIST>();
            LEDGERENTRIESLIST ledgerEntriy = new LEDGERENTRIESLIST();
            ledgerEntriy.LEDGERNAME = ledgerName;
            ledgerEntriy.ISDEEMEDPOSITIVE = "Yes";
            ledgerEntriy.LEDGERFROMITEM = "No";
            ledgerEntriy.REMOVEZEROENTRIES = "No";
            ledgerEntriy.ISPARTYLEDGER = "Yes";
            ledgerEntriy.ISLASTDEEMEDPOSITIVE = "Yes";
            ledgerEntriy.AMOUNT = 0 - finalTotal;

            if (godownInSales.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                BILLALLOCATIONSLIST billAllocationsList = new BILLALLOCATIONSLIST();
                billAllocationsList.NAME = config.inventoryVoucherHeaderDTO.documentNumberServer;
                billAllocationsList.BILLTYPE = "New Ref";
                billAllocationsList.TDSDEDUCTEEISSPECIALRATE = "No";
                billAllocationsList.AMOUNT = finalTotal.ToString();


                if (config.godownName != null && !config.godownName.Equals(""))
                {
                    PPSALESMANLIST ppsalesmanlist = new PPSALESMANLIST();
                    ppsalesmanlist.DESC = "PPSalesMan";
                    ppsalesmanlist.ISLIST = "YES";
                    ppsalesmanlist.TYPE = "String";
                    ppsalesmanlist.INDEX = "102";

                    PPSALESMAN pPSALESMAN = new PPSALESMAN();
                    pPSALESMAN.DESC = "PPSalesMan";
                    pPSALESMAN.Text= config.godownName;
                    ppsalesmanlist.PPSALESMAN = pPSALESMAN;

                    billAllocationsList.PPSALESMANLIST= ppsalesmanlist;

                    PPVCHSALESMANNAMELIST pPVCHSALESMANNAMELIST = new PPVCHSALESMANNAMELIST();
                    pPVCHSALESMANNAMELIST.DESC = "PPSalesMan";
                    pPVCHSALESMANNAMELIST.ISLIST = "YES";
                    pPVCHSALESMANNAMELIST.TYPE="String";
                    pPVCHSALESMANNAMELIST.INDEX = "101";

                    PPVCHSALESMANNAME pPVCHSALESMANNAME = new PPVCHSALESMANNAME();
                    pPVCHSALESMANNAME.DESC = "PPSalesMan";
                    pPVCHSALESMANNAME.Text= config.godownName;

                    pPVCHSALESMANNAMELIST.PPVCHSALESMANNAME = pPVCHSALESMANNAME;

                    voucher.PPVCHSALESMANNAMELIST= pPVCHSALESMANNAMELIST;
                }
                ledgerEntriy.BILLALLOCATIONSLIST= billAllocationsList;
                legersEntriesList.Add(ledgerEntriy);

                //builder.append("<INTERESTCOLLECTION.LIST></INTERESTCOLLECTION.LIST></BILLALLOCATIONS.LIST>");
            }


            legersEntriesList.Add(ledgerEntriy);
            legersEntriesList.AddRange(alllegerEntriesList);
            // alllegerEntriesList.Add(ledgerEntriy);
            voucher.LEDGERENTRIESLIST = legersEntriesList;


            // looping sales order entities
            List<ALLINVENTORYENTRIESLIST> allINVENTORYENTRIESLIST = new();
            foreach (SalesOrderItemDTO salesOrderItem in config.salesOrderItemDTOs)
            {
                ALLINVENTORYENTRIESLIST itemXml = new ALLINVENTORYENTRIESLIST();
                itemXml = generateSingleItemXml(salesOrderItem, config, false);
                allINVENTORYENTRIESLIST.Add(itemXml);


                if (!diffQuantity)
                {
                    if (salesOrderItem.itemFreeQuantity > 0)
                    {
                        ALLINVENTORYENTRIESLIST freeItemXml = generateSingleItemXml(salesOrderItem, config, true);
                        allINVENTORYENTRIESLIST.Add(freeItemXml);
                    }
                }
            }
            voucher.ALLINVENTORYENTRIESLIST = allINVENTORYENTRIESLIST;
            voucher.LEDGERENTRIESLIST = legersEntriesList;

            //add vehicle details
            //  15/3/2024
            if (donloadVehicleDetails.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                EWAYBILLDETAILSLIST eWAYBILLDETAILSLIST = new EWAYBILLDETAILSLIST();
                TRANSPORTDETAILSLIST tRANSPORTDETAILSLIST = new TRANSPORTDETAILSLIST();
                tRANSPORTDETAILSLIST.VEHICLETYPE=config.vehicleType;
                tRANSPORTDETAILSLIST.VEHICLENUMBER=config.vehicleNo;
                //tRANSPORTDETAILSLIST.
                eWAYBILLDETAILSLIST.TRANSPORTDETAILSLIST=tRANSPORTDETAILSLIST;

                voucher.EWAYBILLDETAILSLIST=eWAYBILLDETAILSLIST;
                //addding Vehicle type 
                SDKOWNVEHICLEF1LIST sDKOWNVEHICLEF1LIST = new();
                sDKOWNVEHICLEF1LIST.DESC="SdkownvehicleF1";
                sDKOWNVEHICLEF1LIST.ISLIST="Yes";
                sDKOWNVEHICLEF1LIST.TYPE="String";
                SDKOWNVEHICLEF1 sDKOWNVEHICLEf1 = new();
                sDKOWNVEHICLEf1.DESC="SdkownvehicleF1";
                sDKOWNVEHICLEf1.Text=config.vehicleType;
                sDKOWNVEHICLEF1LIST.SDKOWNVEHICLEF1=sDKOWNVEHICLEf1;
                voucher.SDKOWNVEHICLEF1LIST=sDKOWNVEHICLEF1LIST;

                if (config.vehicleType!=null)
                {
                    //Adding vehicle No if it is rent
                    if (config.vehicleType.Equals("Rent", StringComparison.OrdinalIgnoreCase))
                    {

                        SDKRENTVEHICLEFLIST SdkRentVehicleList = new();
                        SdkRentVehicleList.DESC="SdkrentvehicleF";
                        SdkRentVehicleList.ISLIST="Yes";
                        SdkRentVehicleList.TYPE="String";
                        SDKRENTVEHICLEF sDKOWNVEHICLE = new();
                        sDKOWNVEHICLE.DESC="SdkrentvehicleF";
                        sDKOWNVEHICLE.Text=config.vehicleNo;
                        SdkRentVehicleList.SDKRENTVEHICLEF=sDKOWNVEHICLE;
                        voucher.SDKRENTVEHICLEFLIST=SdkRentVehicleList;

                    }
                    else if (config.vehicleType.Equals("Own", StringComparison.OrdinalIgnoreCase))
                    {

                        SDKOWNVEHICLEFLIST sDKOWNVEHICLEFLIST = new();
                        sDKOWNVEHICLEFLIST.DESC="SdkownvehicleF";
                        sDKOWNVEHICLEFLIST.ISLIST="Yes";
                        sDKOWNVEHICLEFLIST.TYPE="String";
                        SDKOWNVEHICLEF sDKOWNVEHICLE = new();
                        sDKOWNVEHICLE.DESC="SdkownvehicleF";
                        sDKOWNVEHICLE.Text=config.vehicleNo;
                        sDKOWNVEHICLEFLIST.SDKOWNVEHICLEF=sDKOWNVEHICLE;
                        voucher.SDKOWNVEHICLEFLIST=sDKOWNVEHICLEFLIST;
                    }
                }
                //Adding DriverName

                SDKDRIVERLISTF SdkDriverListF = new();
                SdkDriverListF.DESC="SdkdriverlistF";
                SdkDriverListF.Text=config.vehicleDriver;
                SDKDRIVERLISTFLIST SdkDriverListf = new();
                SdkDriverListf.SDKDRIVERLISTF=SdkDriverListF;
                voucher.SDKDRIVERLISTFLIST=SdkDriverListf;

                //adiing Salesman

                SDKSALESMAN2F SdkSalesman2f = new();
                SdkSalesman2f.DESC="SdkdriverlistF";
                SdkSalesman2f.Text= config.inventoryVoucherHeaderDTO.employeeName;
                SDKSALESMAN2FLIST SdkSalesman2fList = new();
                SdkSalesman2fList.SDKSALESMAN2F=SdkSalesman2f;
                voucher.SDKSALESMAN2FLIST=SdkSalesman2fList;


                //adding Selvouchers 17/05/2024

                SDKSYNCSELVOUCHERS sDKSYNCSELVOUCHERS = new();
                sDKSYNCSELVOUCHERS.DESC="sdkSyncSelVouchers";
                sDKSYNCSELVOUCHERS.Text= "Yes";
                SDKSYNCSELVOUCHERSLIST sDKSYNCSELVOUCHERSLIST = new();
                sDKSYNCSELVOUCHERSLIST.SDKSYNCSELVOUCHERS=sDKSYNCSELVOUCHERS;
                voucher.SDKSYNCSELVOUCHERSLIST=sDKSYNCSELVOUCHERSLIST;

            }

            tallymessage.VOUCHER = voucher;
            talllyMessagesList.Add(tallymessage);
            requestData.TALLYMESSAGE = talllyMessagesList;
            importdata.REQUESTDATA = requestData;
            body.IMPORTDATA = importdata;
            tallyRequest.BODY = body;
            return tallyRequest;
        }
        private ALLINVENTORYENTRIESLIST generateSingleItemXml(SalesOrderItemDTO salesOrder, SalesOrderDTO config, bool isFreeItem)
        {
            bool discount = true;
            bool diffQuantity = false;
            String trimChar = salesOrder.trimChar == null ? "" : salesOrder.trimChar;
            String itemName = StringUtilsCustom.replaceSpecialCharactersWithXmlValue(salesOrder.itemName) + trimChar;

            //diffQuantity = actualBilledStatus.Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false;

            //bool isOptional = true;

            //if (isOptionalsalesOrder.Equals("false", StringComparison.OrdinalIgnoreCase))
            //{
            //    isOptional = false;
            //}


            ALLINVENTORYENTRIESLIST aLLINVENTORYENTRIESLIST = new ALLINVENTORYENTRIESLIST();
            //aLLINVENTORYENTRIESLIST.STOCKITEMNAME = itemName;
            //if (salesOrder.remarks != null)
            //{
            //    BASICUSERDESCRIPTIONLIST bASICUSERDESCRIPTIONLIST = new BASICUSERDESCRIPTIONLIST();
            //    bASICUSERDESCRIPTIONLIST.TYPE = "String";
            //    bASICUSERDESCRIPTIONLIST.BASICUSERDESCRIPTION = salesOrder.remarks;
            //    aLLINVENTORYENTRIESLIST.BASICUSERDESCRIPTIONLIST = bASICUSERDESCRIPTIONLIST;

            //}
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

                //double sellingRate = salesOrder.sellingRate;

                //if (reduceTax.Equals("true"))
                //{
                //    sellingRate = salesOrder.sellingRate * 100 / (100 + salesOrder.taxPercentage);
                //}

                //if (productRateIncludingTax != null && productRateIncludingTax.Equals("true", StringComparison.OrdinalIgnoreCase))
                //{
                //    double totalTax = 0.0;

                //    if (salesOrder.taxPercentage > 5 && config.ledgerGstType != null
                //            && !"Regular".Equals(config.ledgerGstType) && kfcLedger != null)
                //    {
                //        totalTax = salesOrder.taxPercentage + 1;
                //    }
                //    else
                //    {
                //        totalTax = salesOrder.taxPercentage;
                //    }
                //    sellingRate = (salesOrder.sellingRate * (100.0 / (100.0 + totalTax)));
                //}

                //sellingRate = round(sellingRate);

                aLLINVENTORYENTRIESLIST.RATE = round(salesOrder.sellingRate).ToString() + "/" + salesOrder.unit;


            }
            discount = salesOrder.itemDiscount == 0 ? false : true;

            if (discount)
            {
                aLLINVENTORYENTRIESLIST.DISCOUNT = salesOrder.itemDiscount;
            }
            double discountAmount = 0;
            if (!isFreeItem)
            {
                double itemTotalExeVat = round(salesOrder.sellingRate) * salesOrder.quantity;
                itemTotalExeVat = round(itemTotalExeVat);
                //if (reduceTax.Equals("true"))
                //{
                //    itemTotalExeVat = salesOrder.sellingRate * 100 / (100 + salesOrder.taxPercentage) * salesOrder.quantity;
                //}
                if (salesOrder.itemDiscount != 0)
                {
                    discountAmount = itemTotalExeVat * salesOrder.itemDiscount / 100;
                    discountAmount = round(discountAmount);
                    itemTotalExeVat = itemTotalExeVat - discountAmount;
                }
                double totalAmount = round(itemTotalExeVat);
                aLLINVENTORYENTRIESLIST.AMOUNT= totalAmount;

                //if (productRateIncludingTax != null && productRateIncludingTax.Equals("true", StringComparison.OrdinalIgnoreCase))
                //{
                //    double totalTax = 0.0;

                //    if (salesOrder.taxPercentage > 5 && config.ledgerGstType != null
                //            && !"Regular".Equals(config.ledgerGstType, StringComparison.OrdinalIgnoreCase) && kfcLedger != null)
                //    {
                //        totalTax = salesOrder.taxPercentage + 1;
                //    }
                //    else
                //    {
                //        totalTax = salesOrder.taxPercentage;
                //    }

                //    double sellRate = (salesOrder.sellingRate * (100.0 / (100.0 + totalTax)));

                //    sellRate = round(sellRate);
                //    itemTotalExeVat = sellRate * salesOrder.quantity;
                //}

                //if (salesOrder.itemDiscount != 0)
                //{
                //    double discountAmount = itemTotalExeVat * salesOrder.itemDiscount / 100;
                //    itemTotalExeVat = itemTotalExeVat - discountAmount;
                //}
                //if (salesOrder.itemDiscount != 0)
                //{
                //    double discountAmount = itemTotalExeVat * salesOrder.itemDiscount / 100;
                //    itemTotalExeVat = itemTotalExeVat - discountAmount;
                //}

                //aLLINVENTORYENTRIESLIST.AMOUNT = itemTotalExeVat;

            }
            // checks different quantity is enabled or not
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
            aLLINVENTORYENTRIESLIST.ACTUALQTY = actualQuantity + salesOrder.unit;
            aLLINVENTORYENTRIESLIST.BILLEDQTY = billedQuantity + salesOrder.unit;
            BATCHALLOCATIONSLIST bATCHALLOCATIONSLIST = new BATCHALLOCATIONSLIST();

            String mainLocation = "";
            //LogManager.WriteLog("SOURCE STOCK LOCATION :" + salesOrder.sourceStockLocationName);
            //LogManager.WriteLog("STOCKLOCATION : " + salesOrder.stockLocationName);
            if ("true".Equals(userStockLocation, StringComparison.OrdinalIgnoreCase))
            {
                LogManager.WriteLog("SOURCE STOCK LOCATION :" + salesOrder.sourceStockLocationName);
                LogManager.WriteLog("STOCKLOCATION : " + salesOrder.stockLocationName);
                if (String.IsNullOrEmpty(salesOrder.sourceStockLocationName))
                {
                   // mainLocation = godownName;
                   mainLocation=salesOrder.stockLocationName;
                }
                else
                {
                    mainLocation = salesOrder.sourceStockLocationName;
                }

                bATCHALLOCATIONSLIST.GODOWNNAME= mainLocation;
                bATCHALLOCATIONSLIST.BATCHNAME = "Primary Batch";
                bATCHALLOCATIONSLIST.DESTINATIONGODOWNNAME = mainLocation;
                bATCHALLOCATIONSLIST.INDENTNO = "";
                bATCHALLOCATIONSLIST.ORDERNO = "-1";
                bATCHALLOCATIONSLIST.DYNAMICCSTISCLEARED = "No";

                //salesOrder.stockLocationName = ApplicationProperties.properties["godownName"].ToString();
            }
            //if (salesOrder.sourceStockLocationName == null)
            //{

            //    if (salesOrder.stockLocationName == null)
            //    {
            //        mainLocation = ApplicationProperties.properties["godownName"].ToString();
            //    }
            //    else
            //    {
            //        mainLocation = salesOrder.stockLocationName;
            //    }

            //}
            else
            {
                if ("true".Equals(godownFixed, StringComparison.OrdinalIgnoreCase))
                {
                    salesOrder.stockLocationName=godownName;
                }

                if (String.IsNullOrEmpty(salesOrder.sourceStockLocationName))
                {
                    if (String.IsNullOrEmpty(salesOrder.stockLocationName))
                    {
                        mainLocation = godownName;
                    }
                    else
                    {
                        mainLocation = salesOrder.stockLocationName;
                    }
                }
                else
                {

                    if (String.IsNullOrEmpty(salesOrder.stockLocationName))
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


            }




            if (!isFreeItem)
            {
                double itemTotalExeVat2 = actualQuantity * round(salesOrder.sellingRate) - discountAmount;
                double totalAmount = round(itemTotalExeVat2);
                bATCHALLOCATIONSLIST.AMOUNT = totalAmount;

            }
            bATCHALLOCATIONSLIST.ACTUALQTY = actualQuantity + salesOrder.unit;
            bATCHALLOCATIONSLIST.BILLEDQTY = billedQuantity + salesOrder.unit;

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
                // aCCOUNTINGALLOCATIONSLIST.LEDGERNAME = salesOrder.productProfileDTO.defaultLedger;
                aCCOUNTINGALLOCATIONSLIST.LEDGERNAME=salesReturnLedgerName;

            }
            else
            {
                aCCOUNTINGALLOCATIONSLIST.LEDGERNAME = salesReturnLedgerName;
            }
            aCCOUNTINGALLOCATIONSLIST.GSTCLASS = "";
            aCCOUNTINGALLOCATIONSLIST.ISDEEMEDPOSITIVE = "No";
            aCCOUNTINGALLOCATIONSLIST.LEDGERFROMITEM = "No";
            aCCOUNTINGALLOCATIONSLIST.REMOVEZEROENTRIES = "No";
            aCCOUNTINGALLOCATIONSLIST.ISPARTYLEDGER = "No";
            aCCOUNTINGALLOCATIONSLIST.ISLASTDEEMEDPOSITIVE = "No";
            if (!isFreeItem)
            {
                double itemTotalExeVat2 = actualQuantity * round(salesOrder.sellingRate) - discountAmount;
                double totalAmount = round(itemTotalExeVat2);

                aCCOUNTINGALLOCATIONSLIST.AMOUNT = totalAmount;
                CATEGORYALLOCATIONSLIST cATEGORYALLOCATIONSLIST = new CATEGORYALLOCATIONSLIST();
                cATEGORYALLOCATIONSLIST.CATEGORY = "Primary Cost Category";
                cATEGORYALLOCATIONSLIST.ISDEEMEDPOSITIVE = "No";
                aCCOUNTINGALLOCATIONSLIST.CATEGORYALLOCATIONSLIST=cATEGORYALLOCATIONSLIST;



            }
            aLLINVENTORYENTRIESLIST.ACCOUNTINGALLOCATIONSLIST = aCCOUNTINGALLOCATIONSLIST;


            return aLLINVENTORYENTRIESLIST;

        }

        private double calculateTotal(List<SalesOrderItemDTO> salesOrderList)
        {
            double finalTotal = 0;
            foreach (SalesOrderItemDTO salesOrder in salesOrderList)
            {
                finalTotal += salesOrder.rowTotal;
            }
            return finalTotal;
        }
        private Dictionary<string, object>? NormalGenerateGSTWithVatCalculationXml(List<SalesOrderItemDTO> salesOrderList,
            List<String> vatLedgerList, String reduceTax, String roundOffLedger, String ledgerGstType, String kfcLedger,
            bool ifIgst, String productRateIncludingTax)
        {
            double finalTotal = 0;
            double totalVat = 0;
            // double scgst = 0;
            double vat = 0;
            double kfc = 0;
            double igst = 0;


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
                            && !"Regular".Equals(ledgerGstType, StringComparison.OrdinalIgnoreCase) && kfcLedger.Count() > 0)
                    {

                        totalTax = salesOrder.taxPercentage + 1;

                    }
                    else
                    {

                        totalTax = salesOrder.taxPercentage;

                    }

                    sellingRate = (salesOrder.sellingRate * (100.0 / (100.0 + totalTax)));

                }

                sellingRate = Math.Round(sellingRate, 2);
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
            }

            totalVat = vat;
            totalVat = round(round(totalVat / 2) * 2);
            totalVat = round(totalVat);
            finalTotal = round(finalTotal) + round(kfc);// adding KFC
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
            Kfcledgerentry = getLegderEntriesList(kfcLedger, totalvatAmount);
            ledgerEntriesList.Add(Kfcledgerentry);

            if (roundOffLedger != null && roundOffLedger.Count() > 0 && !roundOffLedger.Equals(""))
            {
                double roundTo = Math.Round(finalTotal) - finalTotal;

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

        private Dictionary<string, object>? NormalGenerateProductGSTWithVatCalculationXml(List<SalesOrderItemDTO> salesOrderList, List<string> vatLedgerList, string reduceTax, string roundOffLedger, string ledgerGstType, string kfcLedger, bool ifIgst, string productRateIncludingTax, List<string> taxRatesProduct)
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
                totalAmount =round(totalAmount);

                if (taxRate1 != 0 && salesOrder.rateOfVat == taxRate1)
                {
                    vat1 += round(totalAmount *round(salesOrder.rateOfVat / 100.0));
                    // finalTotal1 += totalAmount;
                }
                if (taxRate1 != 0 && salesOrder.rateOfVat == taxRate2)
                {
                    vat2 += round(totalAmount * round(salesOrder.rateOfVat / 100.0));
                    // finalTotal2 += totalAmount;
                }

                if (taxRate1 != 0 && salesOrder.rateOfVat == taxRate3)
                {
                    vat3 += round(totalAmount * round(salesOrder.rateOfVat / 100.0));
                    // finalTotal3 += totalAmount;
                }
                if (taxRate1 != 0 && salesOrder.rateOfVat == taxRate4)
                {
                    vat4 += round(totalAmount * round(salesOrder.rateOfVat / 100.0));
                    // finalTotal3 += totalAmount;
                }
                vat += round(totalAmount * round(salesOrder.rateOfVat / 100.0));

                finalTotal += totalAmount;

                // finalTotalNew += totalAmount;

                if (salesOrder.rateOfVat >= 12 && ledgerGstType != null && !"Regular".Equals(ledgerGstType, StringComparison.OrdinalIgnoreCase) && kfcLedger.Count() > 0)
                {
                    double kfcAmt = round(totalAmount * 0.01);
                    kfc = round(kfc + round(kfcAmt));
                }
            }

            if (vat1 != 0)
            {
                totalVat1 = vat1;
                totalVat1 = Math.Round(Math.Round(totalVat1 / 2) * 2);
                totalVat1 = Math.Round(totalVat1);
                // finalTotal1 = round(finalTotal1) + round(kfc);// adding KFC
                // finalTotal1 = finalTotal1 + totalVat1;
            }
            if (vat2 != 0)
            {
                totalVat2 = vat2;
                totalVat2 = Math.Round(Math.Round(totalVat2 / 2) * 2);
                totalVat2 = Math.Round(totalVat2);
                // finalTotal2 = round(finalTotal2) + round(kfc);// adding KFC
                // finalTotal2 = finalTotal2 + totalVat2;
            }
            if (vat3 != 0)
            {
                totalVat3 = vat3;
                totalVat3 = Math.Round(Math.Round(totalVat3 / 2) * 2);
                totalVat3 = Math.Round(totalVat3);
                // finalTotal3 = round(finalTotal3) + round(kfc);// adding KFC
                // finalTotal3 = finalTotal3 + totalVat3;
            }

            if (vat4 != 0)
            {
                totalVat4 = vat4;
                totalVat4 = Math.Round(Math.Round(totalVat4 / 2) * 2);
                totalVat4 = Math.Round(totalVat4);
                // finalTotal3 = round(finalTotal3) + round(kfc);// adding KFC
                // finalTotal3 = finalTotal3 + totalVat3;
            }


            double totalvatAmount1 = Math.Round(totalVat1 / 2);
            double totalvatAmount2 = Math.Round(totalVat2 / 2);
            double totalvatAmount3 = Math.Round(totalVat3 / 2);
            double totalvatAmount4 = Math.Round(totalVat4 / 2);



            totalVat = totalVat1 + totalVat2 + totalVat3 + totalVat4;
            // totalVat = round(vat1)  + round(vat2) + round(vat3);
            totalVat = Math.Round(Math.Round(totalVat / 2) * 2);
            totalVat = Math.Round(totalVat);
            finalTotal = Math.Round(finalTotal) + Math.Round(kfc);// adding KFC
            finalTotal = Math.Round(finalTotal) + Math.Round(cess);// adding CESS
            finalTotal = finalTotal + totalVat;

            double totalvatAmount = Math.Round(totalVat / 2);

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
                                //OLDAUDITENTRYIDSLIST oLDAUDITENTRYIDSLIST = new OLDAUDITENTRYIDSLIST();
                                //oLDAUDITENTRYIDSLIST.OLDAUDITENTRYIDS = "-1";
                                //oLDAUDITENTRYIDSLIST.TYPE = "Number";
                                //ledgerentrieslist.OLDAUDITENTRYIDSLIST = oLDAUDITENTRYIDSLIST;

                                //BASICRATEOFINVOICETAXLIST bASICRATEOFINVOICETAXLIST = new BASICRATEOFINVOICETAXLIST();
                                //bASICRATEOFINVOICETAXLIST.TYPE = "Number";
                                ////bASICRATEOFINVOICETAXLIST.BASICRATEOFINVOICETAX = scgst;
                                //ledgerentrieslist.BASICRATEOFINVOICETAXLIST = bASICRATEOFINVOICETAXLIST;
                                //ledgerentrieslist.ROUNDTYPE = "Normal Rounding";
                                //ledgerentrieslist.LEDGERNAME = vatLedgerList[i];
                                //ledgerentrieslist.GSTCLASS = "";
                                //ledgerentrieslist.ISDEEMEDPOSITIVE = "No";
                                //ledgerentrieslist.LEDGERFROMITEM = "No";
                                //ledgerentrieslist.REMOVEZEROENTRIES = "No";
                                //ledgerentrieslist.ISPARTYLEDGER = "No";
                                //ledgerentrieslist.ISLASTDEEMEDPOSITIVE = "No";
                                //ledgerentrieslist.AMOUNT = totalvatAmount;
                                //ledgerentrieslist.VATEXPAMOUNT = totalvatAmount;

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

            if (roundOffLedger != null && !roundOffLedger.Equals(""))
            {
                double roundTo = Math.Round(finalTotal) - finalTotal;
                roundTo = Math.Round(roundTo);
                LEDGERENTRIESLIST ledgerentrieslist2 = new LEDGERENTRIESLIST();
                ledgerentrieslist2 = getLegderEntriesList(roundOffLedger, roundTo, false);
                alllegerEntriesList.Add(ledgerentrieslist2);
                finalTotal = Math.Round(finalTotal);

            }

            Dictionary<String, Object> hashMap = new Dictionary<String, Object>
                    {
                        { "finalTotal", Math.Round(finalTotal) },
                        { "vatXml", alllegerEntriesList }
                    };
            return hashMap;

        }
        private LEDGERENTRIESLIST getLegderEntriesList(string ledgerName, double amount, bool IsvatAdded = true)
        {
            LEDGERENTRIESLIST ledgerentrieslist = new LEDGERENTRIESLIST();
            OLDAUDITENTRYIDSLIST oLDAUDITENTRYIDSLIST = new OLDAUDITENTRYIDSLIST();
            oLDAUDITENTRYIDSLIST.OLDAUDITENTRYIDS = "-1";
            oLDAUDITENTRYIDSLIST.TYPE = "Number";
            ledgerentrieslist.OLDAUDITENTRYIDSLIST = oLDAUDITENTRYIDSLIST;

            BASICRATEOFINVOICETAXLIST bASICRATEOFINVOICETAXLIST = new BASICRATEOFINVOICETAXLIST();
            bASICRATEOFINVOICETAXLIST.TYPE = "Number";
            //bASICRATEOFINVOICETAXLIST.BASICRATEOFINVOICETAX = scgst;
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
        private string ConvertDate(string date)
        {
            string formattedDate = "";
            string[] splitDates = date.Split('T');
            DateTime dateTime = DateTime.ParseExact(splitDates[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            formattedDate = dateTime.ToString("dd-MM-yyyy");
            return formattedDate;
        }

        private async Task<string> getGstNumber(string ledgerName)
        {
            string gstNumber = "";
            try
            {


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
            catch (Exception ex)
            {
                LogManager.WriteLog("Exception Occured :\n while geting gstnumber from tally using odbc \n "+ex.Message);
                LogManager.HandleException(ex);
                return gstNumber;
            }


        }


        private Dictionary<string, object>? GenerateGSTCalculationXml(List<SalesOrderItemDTO> salesOrderList, List<GstLedgerDTO> vatLedgerList, string roundOffLedger)
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
                    double itemTotalWithoutTax = Math.Round(dto.sellingRate * dto.quantity);
                    double discountedAmt = Math.Round(itemTotalWithoutTax * (dto.itemDiscount / 100));
                    double totalWithoutTax = itemTotalWithoutTax - discountedAmt;
                    gstAmt += Math.Round(totalWithoutTax * (ledger.taxRate / 100));
                }
                ledger.totalTaxAmt = Math.Round(gstAmt);
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
                double amount = Math.Round(item.quantity * item.sellingRate);
                double amountWithDiscount = amount - Math.Round(amount * (item.discountPercentage / 100));
                withoutTax += amountWithDiscount;
            }
            finalTotal = withoutTax + totalTax;

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

            Dictionary<String, Object> hashMap = new Dictionary<String, Object>();
            hashMap.Add("finalTotal", finalTotal);
            hashMap.Add("vatXml", alllegerEntriesList);
            return hashMap;


        }


        private static double round(double number)
        {

            //return Math.Round(number, 2);
            return Math.Round(number, 2, MidpointRounding.AwayFromZero);
        }
    }
}
