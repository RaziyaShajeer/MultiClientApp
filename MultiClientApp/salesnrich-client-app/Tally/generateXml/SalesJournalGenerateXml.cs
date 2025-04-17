using dtos;
using SNR_ClientApp.Parsers;
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
    public class SalesJournalGenerateXml
    {
		private String employeeString;
		private String isOptionalsalesOrder = ApplicationProperties.properties["is.optional.salesOrder"].ToString();
		private String companyState = ApplicationProperties.properties["company.state"].ToString();
		private String companyString = ApplicationProperties.properties["tally.company"].ToString();

		private String orderNumberWithEmployee = ApplicationProperties.properties["order.employee.name"].ToString();
		private String byEmpVoucher = ApplicationProperties.properties["download.by.employee.voucher"].ToString();
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
		private String taxRequired = ApplicationProperties.properties["tax.required"].ToString();
		SalesCalculationXml salesCalculationXml = new SalesCalculationXml();
		public async Task<ENVELOPE> generateSalesOrderXmlAsync(SalesOrderDTO config)
        {
			List<LEDGERENTRIESLIST> alllegerEntriesList = new List<LEDGERENTRIESLIST>();
			// checking if ledgerState is currentCompany
			bool ifIgst = (config.ledgerState != null && !config.ledgerState.Equals(companyState))
					? true
					: false;

			String trimChar = config.trimChar == null ? "" : config.trimChar;

			String ledgerName = StringUtilsCustom.replaceSpecialCharactersWithXmlValue(config.ledgerName) + trimChar;
			String companyName = companyString;
			LedgerToGstNumberResponseParser ledgerToGstNumberResponseParser = new LedgerToGstNumberResponseParser();
			String gstNumber = await ledgerToGstNumberResponseParser.getGstNumberAsync(ledgerName);
					

			String date = StringUtilsCustom.ConvertToDateTime(config.date);
			string dynamicDate = ApplicationProperties.properties["salessorderDate"].ToString();
			if (dynamicDate != null && !dynamicDate.Equals("", StringComparison.OrdinalIgnoreCase))
			{
				date = StringUtilsCustom.FormatDate(dynamicDate);
			}

			string uuid = Guid.NewGuid().ToString();
			char[] str = uuid.ToCharArray();
			String oderReference = KeyGeneratorUtil.GetRandomCustomString(str, 8);

			// config.getIsVat()
			bool isVat = true;
			if ("false".Equals(taxRequired,StringComparison.OrdinalIgnoreCase))
			{
				isVat = false;
			}
			bool diffQuantity = false;

			// sales order total amount including vat
			double finalTotal = 0;
			// string builder for vat XML
			StringBuilder stringBuilder = new StringBuilder();

			// checks vat option is enabled or not
			// List<VatLedgerDTO> ledgerDTOs =
			// vatLedgerService.getFromTallyAndUpload();

			List<String> ledgerDTOs = gstNames.Split(",").ToList();

			String SALTCHARS = "1234567890";
			StringBuilder salt = new StringBuilder();
			Random rnd = new Random();
			while (salt.Length < 8)
			{

				int index = (int)(rnd.NextDouble() * SALTCHARS.Length);
				salt.Append(SALTCHARS[index]);
			}
			string saltStr = salt.ToString();

			

			String voucherRemoteId = "4ea2d475-dac6-4155-a2d7-32220d3a168a-" + oderReference;
			String voucherKey = "4ea2d475-dac6-4155-a2d7-32220d3a168a-0000a637:" + saltStr;

			

			ADDRESSLIST Address = new();
			// string basicBuyyerAddress = "";
			List<BASICBUYERADDRESSLIST> basicBuyyerAddress = new();
			if (config.ledgerAddress != null)
			{
				List<String> addressListss = new();
				String[] addressList = config.ledgerAddress.Split(",");
				foreach (string address in addressList)
				{
					if (!address.Contains("No Address"))
					{

						Console.WriteLine("Address:- " + address);
						BASICBUYERADDRESSLIST bASICBUYERADDRESSLIST = new();
						bASICBUYERADDRESSLIST.TYPE = "String";
						bASICBUYERADDRESSLIST.Text = address;
						basicBuyyerAddress.Add(bASICBUYERADDRESSLIST);
						addressListss.Add(address);


						//Address += "<ADDRESS>" + address + "</ADDRESS>";
						//basicBuyyerAddress += "<BASICBUYERADDRESS>" + address + "</BASICBUYERADDRESS>";
					}
				}
				Address.TYPE = "String";
				Address.ADDRESS = addressListss;
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
				

				Dictionary<string, object> output = null;
				output = salesCalculationXml.NormalGenerateGSTWithVatCalculationXml(config.salesOrderItemDTOs, ledgerDTOs, "false", roundOffLedger, config.ledgerGstType, kfcLedger, ifIgst, productRateIncludingTax);
				alllegerEntriesList.AddRange((IEnumerable<LEDGERENTRIESLIST>)output["vatXml"]);
				finalTotal += (double)output["finalTotal"];
			}
			else
			{
				finalTotal = salesCalculationXml.calculateTotal(config.salesOrderItemDTOs);
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
			requestDesc.REPORTNAME = "Vouchers";
			STATICVARIABLES staticvariables = new STATICVARIABLES();

			staticvariables.SVCURRENTCOMPANY = ApplicationProperties.properties["tally.company"].ToString();

			requestDesc.STATICVARIABLES = staticvariables;


			importdata.REQUESTDESC = requestDesc;
			REQUESTDATA requestData = new();
			List<TALLYMESSAGE> talllyMessagesList = new();
			TALLYMESSAGE tallymessage = new();
			tallymessage.UDF = "TallyUDF";

			VOUCHER voucher = new VOUCHER();
			if ("true".Equals(byEmpVoucher))
			{
				
				voucher.REMOTEID = voucherRemoteId;
				voucher.VCHKEY = "Sales Order";
				voucher.VCHTYPE = config.employeeAlias;
				voucher.ACTION = "Create";
				voucher.OBJVIEW = "Invoice Voucher View";

            }
            else
            {
				
				voucher.REMOTEID = voucherRemoteId;
				voucher.VCHKEY = "Sales Order";
				voucher.VCHTYPE = "Journal";
				voucher.ACTION = "Create";
				voucher.OBJVIEW = "Invoice Voucher View";
			}

			voucher.DATE = date;
			if (config.inventoryVoucherHeaderDTO.priceLevelName != null)
			{
				voucher.PRICELEVEL = config.inventoryVoucherHeaderDTO.priceLevelName;
				
			}

            if (gstNumber != null)
            {
				voucher.PARTYGSTIN=gstNumber;
				voucher.CONSIGNEEGSTIN=gstNumber;

			}
            if (config.narration != null)
            {
				voucher.NARRATION = config.narration;	
			}
            if (ledgerName.Equals("Cash", StringComparison.OrdinalIgnoreCase))
            {
				voucher.ADDRESSLIST = Address;
				voucher.BASICBUYERADDRESSLIST = basicBuyyerAddress;
			}
			else
			{

				ADDRESSLIST aDDRESSLIST = new ADDRESSLIST();

				aDDRESSLIST = Address;
				voucher.ADDRESSLIST = aDDRESSLIST;

				voucher.BASICBUYERADDRESSLIST = basicBuyyerAddress;
			}
			voucher.PARTYNAME = ledgerName;
			if ("true".Equals(byEmpVoucher,StringComparison.OrdinalIgnoreCase))
			{
				voucher.VOUCHERTYPENAME = config.employeeAlias;
            }
            else
            {
				voucher.VOUCHERTYPENAME = "Journal";
			}
			voucher.VOUCHERTYPECLASS = "Vat";
			voucher.VOUCHERNUMBER = config.inventoryVoucherHeaderDTO.documentNumberServer;
			voucher.REFERENCE = oderReference;
			voucher.PARTYLEDGERNAME = ledgerName;
			voucher.PARTYPINCODE = config.ledgerPinCode;
			voucher.BASICBASEPARTYNAME = ledgerName;
			voucher.FBTPAYMENTTYPE = "Default";
			if (diffQuantity)
			{
				voucher.DIFFACTUALQTY = "Yes";
				
			}
			if (companyState == null || companyState.Length <= 0 || companyState.Equals(""))
			{
				companyState = "Kerala";
			}
			voucher.PERSISTEDVIEW = "Invoice Voucher View";
			voucher.BASICBUYERNAME = config.mailingName;
			voucher.STATENAME = companyState;
			voucher.COUNTRYOFRESIDENCE = "India";
			voucher.PLACEOFSUPPLY = companyState;
			voucher.ISOPTIONAL = "No";
			List<LEDGERENTRIESLIST> legersEntriesList = new List<LEDGERENTRIESLIST>();
			LEDGERENTRIESLIST ledgerEntriy = new LEDGERENTRIESLIST();
			ledgerEntriy.LEDGERNAME = ledgerName;
			ledgerEntriy.ISDEEMEDPOSITIVE = "Yes";
			ledgerEntriy.LEDGERFROMITEM = "No";
			ledgerEntriy.REMOVEZEROENTRIES = "No";
			ledgerEntriy.ISPARTYLEDGER = "Yes";
			ledgerEntriy.ISLASTDEEMEDPOSITIVE = "Yes";
			ledgerEntriy.AMOUNT = 0 - finalTotal;

			List<CATEGORYALLOCATIONSLIST> cATEGORYALLOCATIONSLISTs = new List<CATEGORYALLOCATIONSLIST>();

			CATEGORYALLOCATIONSLIST categoryAllocation = new CATEGORYALLOCATIONSLIST();
			categoryAllocation.CATEGORY = "Primary Cost Category";
			categoryAllocation.ISDEEMEDPOSITIVE = "No";
			cATEGORYALLOCATIONSLISTs.Add(categoryAllocation);
			ledgerEntriy.CATEGORYALLOCATIONSLIST = cATEGORYALLOCATIONSLISTs;

			legersEntriesList.Add(ledgerEntriy);
			voucher.LEDGERENTRIESLIST = legersEntriesList;
			List<ALLINVENTORYENTRIESLIST> allINVENTORYENTRIESLIST = new();
			foreach (SalesOrderItemDTO salesOrderItem in config.salesOrderItemDTOs)
			{
				ALLINVENTORYENTRIESLIST itemXml = new ALLINVENTORYENTRIESLIST();
				itemXml = salesCalculationXml.generateSingleItemXml(salesOrderItem, config, false);
				allINVENTORYENTRIESLIST.Add(itemXml);

			}
			voucher.ALLINVENTORYENTRIESLIST=allINVENTORYENTRIESLIST;



			voucher.LEDGERENTRIESLIST = alllegerEntriesList;
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
