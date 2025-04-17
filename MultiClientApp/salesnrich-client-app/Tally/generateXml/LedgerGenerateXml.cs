using SNR_ClientApp.DTO;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Services;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace SNR_ClientApp.Tally.generateXml
{
    public class LedgerGenerateXml
    {
        public List<TallyXml> generateLedgerXml(List<AccountProfileDTO> accountProfileDTOs)
        {
            List<TallyXml> tallyXmls = new();
            String companyName = CompanyService.getCompanyName();
            StringBuilder builder = new StringBuilder();
            foreach (AccountProfileDTO accountProfileDTO in accountProfileDTOs)
            {
                String ledgerName = StringUtilsCustom.replaceSpecialCharactersWithXmlValue(accountProfileDTO.name);

                String uuid = Guid.NewGuid().ToString();
                char[] str = uuid.ToCharArray();
                string guid = KeyGeneratorUtil.GetRandomCustomString(uuid.ToCharArray(), 35) + "-" + ledgerName;
                string date = DateTime.Now.ToString("yyyyMMdd");


                ENVELOPE envelop = new ENVELOPE();
                HEADER header = new HEADER();

                header.TALLYREQUEST = "Import Data";

                envelop.HEADER = header;

                BODY body = new();
                IMPORTDATA importdata = new IMPORTDATA();
                REQUESTDESC requestDesc = new();
                requestDesc.REPORTNAME = "All Masters";
                STATICVARIABLES staticvariables = new STATICVARIABLES();

                staticvariables.SVCURRENTCOMPANY = ApplicationProperties.properties["tally.company"].ToString();

                requestDesc.STATICVARIABLES = staticvariables;
                requestDesc.REPORTNAME = "All Masters";

                importdata.REQUESTDESC = requestDesc;
                REQUESTDATA requestData = new();
                List<TALLYMESSAGE> talllyMessagesList = new();
                TALLYMESSAGE tallymessage = new();
                tallymessage.UDF = "TallyUDF";
                LEDGER ledger = new LEDGER();
                ledger.NAME = accountProfileDTO.name;
				// ledger.RESERVEDNAME = "";
				LEDMAILINGDETAILSLIST lEDMAILINGDETAILSLIST=new LEDMAILINGDETAILSLIST(); 
				
				 ADDRESSLIST aDDRESSLIST= new ADDRESSLIST();
                aDDRESSLIST.TYPE = "String";
                //aDDRESSLIST.Text = accountProfileDTO.address;
                List<String> addressList = new();
                addressList.Add(accountProfileDTO.address);
                addressList.Add(accountProfileDTO.city);
                addressList.Add(accountProfileDTO.pin);
                addressList.Add(accountProfileDTO.phone1);
                aDDRESSLIST.ADDRESS = addressList;
                lEDMAILINGDETAILSLIST.ADDRESSLIST = aDDRESSLIST;
                lEDMAILINGDETAILSLIST.PINCODE = accountProfileDTO.pin ;
                lEDMAILINGDETAILSLIST.STATE = accountProfileDTO.stateName;
                lEDMAILINGDETAILSLIST.COUNTRY=accountProfileDTO.countryName;
                lEDMAILINGDETAILSLIST.MAILINGNAME = accountProfileDTO.name;
                if (string.IsNullOrWhiteSpace(ApplicationProperties.properties["LedgerDate"]?.ToString()))

				{
                    var formattedDate = ConvertDate(accountProfileDTO.createdDate.ToString());

				
                    lEDMAILINGDETAILSLIST.APPLICABLEFROM = formattedDate;
                }
                else
                {
                    var ledgerdate = ApplicationProperties.properties["LedgerDate"].ToString();
                    var formattedDate = ConvertDate(ledgerdate);
					 lEDMAILINGDETAILSLIST.APPLICABLEFROM = formattedDate;
				}

				ledger.LEDMAILINGDETAILSLIST = lEDMAILINGDETAILSLIST;   
         
                //MAILINGNAMELIST mailingNameList= new MAILINGNAMELIST();
                //mailingNameList.TYPE = "String";
                //mailingNameList.MAILINGNAME = accountProfileDTO.name;
                //ledger.MAILINGNAMELIST = mailingNameList;
                OLDAUDITENTRYIDSLIST oLDAUDITENTRYIDSLIST=new OLDAUDITENTRYIDSLIST();
                oLDAUDITENTRYIDSLIST.TYPE= "String";
                oLDAUDITENTRYIDSLIST.OLDAUDITENTRYIDS = "-1";
                ledger.OLDAUDITENTRYIDSLIST= oLDAUDITENTRYIDSLIST;
                ledger.STARTINGFROM = date;
                ledger.GUID = guid;
                ledger.COUNTRYNAME = "India";
                ledger.STATENAME = "Kerala";
                ledger.VATDEALERTYPE = "Regular";
                ledger.PARENT = accountProfileDTO.location;
                ledger.TAXTYPE = "Others";
                ledger.COUNTRYOFRESIDENCE = "India";
                ledger.SERVICECATEGORY = "";
                ledger.LEDSTATENAME = "Kerala";
                ledger.ISBILLWISEON = "Yes";
                ledger.ISCOSTCENTRESON = "No";
                ledger.ISINTERESTON = "No";
                ledger.ALLOWINMOBILE = "No";
                ledger.ISCOSTTRACKINGON = "No";
                ledger.ISBENEFICIARYCODEON = "No";
                ledger.ISUPDATINGTARGETID = "No";
                ledger.ASORIGINAL = "Yes";

                ledger.ISCONDENSED = "No";
                ledger.AFFECTSSTOCK = "No";
                ledger.ISRATEINCLUSIVEVAT = "No";
                ledger.FORPAYROLL = "No";
                ledger.ISABCENABLED = "No";
                ledger.ISCREDITDAYSCHKON = "No";
                ledger.INTERESTONBILLWISE = "No";
                ledger.OVERRIDEINTEREST = "No";
                ledger.OVERRIDEADVINTEREST = "No";
                ledger.USEFORVAT = "No";
                ledger.IGNORETDSEXEMPT = "No";
                ledger.ISTCSAPPLICABLE = "No";

                ledger.ISTDSAPPLICABLE = "No";
                ledger.ISFBTAPPLICABLE = "No";
                ledger.ISGSTAPPLICABLE = "No";
                ledger.ISEXCISEAPPLICABLE = "No";
                ledger.ISTDSEXPENSE = "No";
                ledger.ISEDLIAPPLICABLE = "No";
                ledger.ISRELATEDPARTY = "No";
                ledger.USEFORESIELIGIBILITY = "No";
                ledger.ISINTERESTINCLLASTDAY = "No";
                ledger.APPROPRIATETAXVALUE = "No";
                ledger.ISBEHAVEASDUTY = "No";
                ledger.INTERESTINCLDAYOFADDITION = "No";
                ledger.INTERESTINCLDAYOFDEDUCTION = "No";
                ledger.OVERRIDECREDITLIMIT = "No";
                ledger.ISAGAINSTFORMC = "No";
                ledger.ISCHEQUEPRINTINGENABLED = "No";
                ledger.ISPAYUPLOAD = "No";
                ledger.ISPAYBATCHONLYSAL = "No";
                ledger.ISBNFCODESUPPORTED = "No";
                ledger.ISBNFCODESUPPORTED = "No";
                ledger.ALLOWEXPORTWITHERRORS = "No";
                ledger.USEFORNOTIONALITC = "No";
                ledger.SHOWINPAYSLIP = "No";
                ledger.USEFORGRATUITY = "No";
                ledger.ISTDSPROJECTED = "No";
                ledger.FORSERVICETAX = "No";
                ledger.ISINPUTCREDIT = "No";
                ledger.ISEXEMPTED = "No";
                ledger.ISABATEMENTAPPLICABLE = "No";
                ledger.ISSTXPARTY = "No";

                ledger.ISSTXNONREALIZEDTYPE = "No";
                ledger.ISUSEDFORCVD = "No";
                ledger.LEDBELONGSTONONTAXABLE = "No";
                ledger.ISEXCISEMERCHANTEXPORTER = "No";
                ledger.ISPARTYEXEMPTED = "No";
                ledger.ISSEZPARTY = "No";
                ledger.TDSDEDUCTEEISSPECIALRATE = "No";
                ledger.ISECHEQUESUPPORTED = "No";
                ledger.ISEDDSUPPORTED = "No";
                ledger.HASECHEQUEDELIVERYMODE = "No";
                ledger.HASECHEQUEDELIVERYTO = "No";
                ledger.HASECHEQUEPRINTLOCATION = "No";
                ledger.HASECHEQUEPAYABLELOCATION = "No";
                ledger.HASECHEQUEBANKLOCATION = "No";
                ledger.HASEDDDELIVERYMODE = "No";
                ledger.HASEDDDELIVERYTO = "No";
                ledger.HASEDDPRINTLOCATION = "No";
                ledger.HASEDDPAYABLELOCATION = "No";
                ledger.HASEDDBANKLOCATION = "No";
                ledger.ISEBANKINGENABLED = "No";
                ledger.ISEXPORTFILEENCRYPTED = "No";
                ledger.ISBATCHENABLED = "No";
                ledger.ISPRODUCTCODEBASED = "No";
                ledger.HASEDDCITY = "No";
                ledger.HASECHEQUECITY = "No";
                ledger.ISFILENAMEFORMATSUPPORTED = "No";
                ledger.HASCLIENTCODE = "No";
                ledger.PAYINSISBATCHAPPLICABLE = "No";
                ledger.PAYINSISFILENUMAPP = "No";
                ledger.ISSALARYTRANSGROUPEDFORBRS = "No";
                ledger.ISEBANKINGSUPPORTED = "No";
                ledger.ISSCBUAE = "No";
                ledger.ISBANKSTATUSAPP = "No";
                ledger.ISSALARYGROUPED = "No";
                ledger.USEFORPURCHASETAX = "No";
                ledger.AUDITED = "No";
                ledger.SORTPOSITION = "1000";
                ledger.ISSTXPARTY = "No";
                ledger.ISSTXPARTY = "No";
                ledger.ISSTXPARTY = "No";
                ledger.ISSTXPARTY = "No";
                ledger.ISSTXPARTY = "No";
                ledger.ISSTXPARTY = "No";
                ledger.ISSTXPARTY = "No";
                ledger.STATENAME = accountProfileDTO.stateName;
                

                LANGUAGENAMELIST languageNameList = new();
                List<String> nameList = new();
                COL COL = new COL();
                COL.ALIAS = accountProfileDTO.name;
                nameList.Add(accountProfileDTO.name);
				
				NAME_LIST name_list = new();
                nameList.Add(accountProfileDTO.name);
                NAMELIST nAMELIST=new();
                nAMELIST.TYPE = "String";
                nAMELIST.NAME = nameList;
                languageNameList.NAMELIST = nAMELIST;
                languageNameList.LANGUAGEID = 1033;
                ledger.LANGUAGENAMELIST= languageNameList;
                tallymessage.LEDGER=ledger;
                talllyMessagesList.Add(tallymessage);
                requestData.TALLYMESSAGE = talllyMessagesList;
                importdata.REQUESTDATA = requestData;
                body.IMPORTDATA= importdata;
                envelop.BODY=body;


                TallyXml tallyXml = new TallyXml();
                tallyXml.pid=(accountProfileDTO.pid);
                tallyXml.xmlObj = envelop;
                tallyXmls.Add(tallyXml);


            }
            return tallyXmls;
        }

		private string ConvertDate(string date)
		{
			string formattedDate = "";
			string[] splitDates = date.Split('T');
			DateTime dateTime = DateTime.ParseExact(splitDates[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
			formattedDate = dateTime.ToString("yyyyMMdd");
			return formattedDate;
		}
		public ENVELOPE getCompanyLedgersGstNumbergetCompanyLedgersGstNumber( String ledgerName)
        {

            ENVELOPE envelop = new ENVELOPE();
            HEADER header = new HEADER();

            header.TALLYREQUEST = "Export";
            header.VERSION= "1";
            header.TYPE="Data";
            header.ID="List of Ledgers";

            envelop.HEADER = header;

            BODY body = new();
            DESC desc = new();
            STATICVARIABLES staticVaribles= new STATICVARIABLES();
            staticVaribles.EXPLODEFLAG="Yes";
            staticVaribles.SVEXPORTFORMAT="$$SysName:XML";
            //staticVaribles.SVCURRENTCOMPANY=companyName;

            staticVaribles.SVCURRENTCOMPANY = ApplicationProperties.properties["tally.company"].ToString();

            desc.STATICVARIABLES=staticVaribles;

            TDL tdl=new TDL();
            TDLMESSAGE tDLMESSAGE=new TDLMESSAGE();
            REPORT report=new REPORT();
            report.NAME="List of Ledgers";
            report.ISMODIFY="No";
            report.ISFIXED="No";
            report.ISINITIALIZE="No";
            report.ISOPTION="No";
            report.ISINTERNAL="No";
            report.FORMS="List of Ledgers";
            tDLMESSAGE.REPORT=report;

            FORM form=new FORM();
            form.NAME="List of Ledgers";
            form.ISMODIFY="No";
            form.ISFIXED="No";
            form.ISINITIALIZE="No";
            form.ISINTERNAL="No";
            form.TOPPARTS="List of Ledgers";
            form.XMLTAG="List of Ledgers";
            tDLMESSAGE.FORM=form;
            PART pART=new PART();
            pART.NAME="List of Ledgers";
            pART.ISMODIFY="No";
            pART.ISFIXED="No";
            pART.ISINITIALIZE="No";
            pART.ISOPTION="No";
            pART.ISINTERNAL="No";
            pART.TOPLINES="Line Ledgers";
            pART.REPEAT="Line Ledgers : Collection of Ledgers";
            pART.SCROLLED="Vertical";
            tDLMESSAGE.PART=new List<PART>() { pART };

            LINE line=new LINE();
            line.NAME="Line Ledgers";
            line.ISMODIFY="No";
            line.ISFIXED="No";
            line.ISINITIALIZE="No";
            line.ISOPTION="No";
            line.ISINTERNAL="No";
            line.XMLtag="Ledger";
            line.Fields=new List<string>() { "Field Name Ledger", "Field PartyGSTIn Ledger" };
            tDLMESSAGE.LINE=new List<LINE>() { line };

            FIELD field=new FIELD();
            field.NAME="Field Name Ledger";
            field.ISMODIFY="No";
            field.ISFIXED="No";
            field.ISINITIALIZE="No";
            field.ISOPTION="No";
            field.ISINTERNAL="No";
            field.SET="$Name";
            field.XMLTAG="Name";


            FIELD field2 = new FIELD();
            field2.NAME="Field PartyGSTIn Ledger";
            field2.ISMODIFY="No";
            field2.ISFIXED="No";
            field2.ISINITIALIZE="No";
            field2.ISOPTION="No";
            field2.ISINTERNAL="No";
            field2.SET="$PartyGSTIn";
            field2.XMLTAG="PARTYGSTIN";

            tDLMESSAGE.FIELD=new List<FIELD>() { field,field2 };

            COLLECTION cOLLECTION=new COLLECTION();
            cOLLECTION.NAME="Collection of Ledgers";
            cOLLECTION.ISMODIFY="No";
            cOLLECTION.ISFIXED="No";
            cOLLECTION.ISINITIALIZE="No";
            cOLLECTION.ISOPTION="No";
            cOLLECTION.ISINTERNAL="No";
            cOLLECTION.TYPE=new List<string>() { "Ledger" };
            cOLLECTION.FILTERS=new List<string>() { "ofSpecificVchs" };
            tDLMESSAGE.COLLECTION=new List<COLLECTION>() { cOLLECTION };

            SYSTEM system=new SYSTEM();
            system.TYPE="FORMULAE";
            system.NAME="ofSpecificVchs";
            system.Text="($NAME = \""+ledgerName+"\")";

            tDLMESSAGE.SYSTEM=new List<SYSTEM>() { system };
            tdl.TDLMESSAGE=tDLMESSAGE;
            desc.TDL=tdl;
            body.DESC=desc;
            envelop.BODY=body;

            return envelop;


        }


    }
}
