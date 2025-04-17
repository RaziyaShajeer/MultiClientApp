using Microsoft.VisualBasic;
using SNR_ClientApp.DTO;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Tally.generateXml
{

    internal class TallyMastersRequestXml
    {
        public ENVELOPE getDayWiseReceiptLedgerNames(String companyName, String date,
        List<VoucherTypeDTO> receiptUnderVoucherTypes)
        {
            StringBuilder voucherTypeStringBuilder = new StringBuilder();
            string prefix = "";
            foreach (VoucherTypeDTO voucherType in receiptUnderVoucherTypes)
            {
                String vouchertypeNAme = prefix+" ($VOUCHERTYPENAME = \"" + voucherType.name + "\")";
                voucherTypeStringBuilder.Append(vouchertypeNAme);
                prefix = "or";
            }
            ENVELOPE envelope = new ENVELOPE();
            HEADER header = new HEADER();
            header.VERSION = "1";
            header.TALLYREQUEST = "Export";
            header.TYPE = "Collection";
            header.ID = "Vch Collection";
            envelope.HEADER = header;

            BODY body = new BODY();
            DESC desc = new DESC();
            STATICVARIABLES staticVariables = new STATICVARIABLES();
            staticVariables.SVCURRENTCOMPANY = companyName;
            staticVariables.SVEXPORTFORMAT = "$$SysName:XML";
            desc.STATICVARIABLES = staticVariables;
            TDL tdl = new TDL();
            TDLMESSAGE tdlmessage = new TDLMESSAGE();
            COLLECTION collection = new();
            collection.NAME = "Vch Collection";
            collection.ISMODIFY = "No";
            collection.ISFIXED = "No";
            collection.ISINITIALIZE = "No";
            collection.ISOPTION = "No";
            collection.ISINTERNAL = "No";

            List<String> types1 = new();
            types1.Add("Vouchers");

            collection.TYPE = types1;

            // collection.TYPE = "Voucher";
            collection.FETCH = "PARTYLEDGERNAME,REFERENCE";
            //collection.FETCH = "REFERENCE";
            //collection.FETCH="VOUCHERNUMBER";
            List<String> filters = new List<string>();
            filters.Add("dateSearch");
            filters.Add("ofSpecificVchs");
            collection.FILTERS= filters;
            List<COLLECTION> cOLLECTIONSlist = new List<COLLECTION>();
            cOLLECTIONSlist.Add(collection);
            tdlmessage.COLLECTION = cOLLECTIONSlist;
            List<SYSTEM> systemsList = new List<SYSTEM>();
            SYSTEM system = new SYSTEM();
            system.NAME = "dateSearch";
            system.TYPE = "Formulae";
            //system.Text = StringUtilsCustom.ConvertFormat(date);
            system.Text = "$$String:$Date = $$String:\""+date+"\" ";
            systemsList.Add(system);
            SYSTEM system2 = new SYSTEM();
            system2.NAME = "ofSpecificVchs";
            system2.TYPE = "Formulae";
            system2.Text= voucherTypeStringBuilder.ToString();
            systemsList.Add(system2);
            tdlmessage.SYSTEM = systemsList;
            tdl.TDLMESSAGE = tdlmessage;
            desc.TDL = tdl;
            body.DESC = desc;
            envelope.BODY = body;
            return envelope;



        }
      

        public  ENVELOPE GetSalesFromReference(List<string> voucherReference)
        {
            string prefix = "";
            StringBuilder voucherTypeStringBuilder = new StringBuilder();
           
            foreach (var reference in voucherReference)
            {
                String references = prefix+" ($REFERENCE = \"" +reference  + "\")";
                voucherTypeStringBuilder.Append(references);
                prefix = "or";
            }
            ENVELOPE envelope = new ENVELOPE();
            HEADER header = new HEADER();
            header.VERSION = "1";
            header.TALLYREQUEST = "Export";
            header.TYPE = "Collection";
            header.ID = "Vch Collection";
            envelope.HEADER = header;

            BODY body = new BODY();
            DESC desc = new DESC();
            STATICVARIABLES staticVariables = new STATICVARIABLES();
          
            staticVariables.SVEXPORTFORMAT = "$$SysName:XML";
            desc.STATICVARIABLES = staticVariables;
            TDL tdl = new TDL();
            TDLMESSAGE tdlmessage = new TDLMESSAGE();
            COLLECTION collection=new ();
            collection.NAME = "Vch Collection";
            collection.ISMODIFY = "No";
            collection.ISFIXED = "No";
            collection.ISINITIALIZE = "No";
            collection.ISOPTION = "No";
            collection.ISINTERNAL = "No";

            List<String> types1 = new();
            types1.Add("Vouchers");
          
            collection.TYPE = types1;

           // collection.TYPE = "Voucher";
            
            collection.FETCH = "REFERENCE";
            //collection.FETCH="VOUCHERNUMBER";
            List<String> filters = new List<string>();
            filters.Add("referencefilter");
           
            collection.FILTERS= filters;
            List<COLLECTION> cOLLECTIONSlist = new List<COLLECTION>();
            cOLLECTIONSlist.Add(collection);
           
            tdlmessage.COLLECTION = cOLLECTIONSlist;
            List<SYSTEM> systemsList = new List<SYSTEM>();
            SYSTEM system=new SYSTEM();
            system.NAME = "referencefilter";

            system.TYPE = "Formulae";
            //system.Text = StringUtilsCustom.ConvertFormat(date);
            system.Text = voucherTypeStringBuilder.ToString();
            systemsList.Add(system);
            //SYSTEM system2 = new SYSTEM();
            //system2.NAME = "ofSpecificVchs";
            //system2.TYPE = "Formulae";
            //system2.Text= "$$String:$VOUCHERTYPENAME=SALES" ;
            //systemsList.Add(system2);
            tdlmessage.SYSTEM = systemsList;
            tdl.TDLMESSAGE = tdlmessage;
            desc.TDL = tdl;
            body.DESC = desc;
            envelope.BODY = body;
            return envelope;



        }
        

        public ENVELOPE getReceptVoucherXml(String companyName, String date, String ledgerName,
      List<VoucherTypeDTO> receiptUnderVoucherTypes)
        {
            StringBuilder voucherTypeStringBuilder = new StringBuilder();
            string prefix = "";
            foreach (VoucherTypeDTO voucherType in receiptUnderVoucherTypes)
            {
                String vouchertypeNAme = prefix + " ($VOUCHERTYPENAME = \"" + voucherType.name + "\")";
                voucherTypeStringBuilder.Append(vouchertypeNAme);
                prefix = "or";
            }
            ENVELOPE envelope = new ENVELOPE();
            HEADER header = new HEADER();
            header.VERSION = "1";
            header.TALLYREQUEST = "Export";
            header.TYPE = "Data";
            header.ID = "List of Day Book";
            envelope.HEADER = header;

            BODY body = new BODY();
            DESC desc = new DESC();
            //STATICVARIABLES staticVariables = new STATICVARIABLES();
            //staticVariables.SVCURRENTCOMPANY = companyName;
            //staticVariables.SVEXPORTFORMAT = "$$SysName:XML";
            //desc.STATICVARIABLES = staticVariables;
            TDL tdl = new TDL();
            TDLMESSAGE tdlmessage = new TDLMESSAGE();
           REPORT report=new REPORT();
            report.NAME = "List of Day Book";
            report.FORMS = "List of Day Book";
            report.VARIABLE = "EXPLODEFLAG,SVEXPORTFORMAT,SVCURRENTCOMPANY,SVFROMDATE,SVTODATE";
            List<String> sets = new List<string>();
            sets.Add("EXPLODEFLAG:Yes");
            sets.Add("SVEXPORTFORMAT:$$SysName:XML");
            sets.Add("SVCURRENTCOMPANY:"+companyName);
            report.Set = sets;
            tdlmessage.REPORT = report;

            FORM form = new FORM();
            form.NAME = "List of Day Book";
            form.PARTS = "List of Day Book";
            tdlmessage.FORM = form;
            List<PART> parts = new List<PART>();
            PART part = new();
            part.NAME = "List of Day Book";
            part.LINES = "List of Day Book";
            part.REPEAT = "List of Day Book : Collection of Day Book";
            part.SCROLLED = "Vertical";
            parts.Add(part);

            PART part2 = new();
            part2.NAME = "list of address";
            part2.LINES = "list of address";
            part2.REPEAT = "list of address : Collection of Address";
            part2.SCROLLED = "Vertical";
            parts.Add(part2);

            PART part3 = new();
            part3.NAME = "list of ledger entries";
            part3.LINES = "list of ledger entries";
            part3.REPEAT = "list of ledger entries : Collection of Ledger Entry";
            part3.SCROLLED = "Vertical";
            parts.Add(part3);

            PART part4 = new();
            part4.NAME = "list of inventory";
            part4.LINES = "list of inventory";
            part4.REPEAT = "list of inventory : Collection of Allinventoryentries";
            part4.SCROLLED = "Vertical";
            parts.Add(part4);

            tdlmessage.PART = parts;


            List<LINE>lines=new List<LINE>();

            LINE line = new();
            line.NAME = "List of Day Book";
            line.XMLtag = "VOUCHERS";
            List<String> fields = new();
            fields.Add("VMasterID");
            fields.Add("Voucherdate");
            fields.Add("VTypeName");
            fields.Add("VoucherNumber");
            fields.Add("reference");
           
            line.Fields = fields;
            List<string> explodes = new();
            explodes.Add("list of inventory");
            explodes.Add("list of ledger entries");
            line.explode=explodes;

            lines.Add(line);

            LINE line2 = new();
            line2.NAME = "List of ledger entries";
            line2.XMLtag = "VAT";
            List<String> fields2 = new();
            fields2.Add("VATLEDGERNAME");
            fields2.Add("VATAMOUNT");


            line2.Fields = fields2;
            lines.Add(line2);

            LINE line3 = new();
            line3.NAME = "List of inventory";
            line3.XMLtag = "VAT";
            List<String> fields3 = new();
            fields3.Add("INVNAME");
            fields3.Add("INVAMOUNT");
            fields3.Add("ACTUALQTY");
            fields3.Add("BilledQTY");
            fields3.Add("INVRATE");
            fields3.Add("SALESLEDGER");
            line3.Fields = fields3;
            lines.Add(line3);

            LINE line4 = new();
            line4.NAME = "List of address";
            line4.XMLtag = "Address";
            List<String> fields4 = new();
            fields4.Add("Address");
            line4.Fields = fields4;
            lines.Add(line4);

            //LINE line5 = new();
            //line5.NAME = "List of address";
            //line5.XMLtag = "Address";
          
            //lines.Add(line5);


            List<FIELD> fieldsList = new List<FIELD>();
          
            FIELD fieldObj = new();
            fieldObj.NAME = "INVNAME";
            fieldObj.USE = "Name Field";
            fieldObj.SET = "$STOCKITEMNAME";
            fieldsList.Add(fieldObj);

            FIELD fieldObj2 = new();
            fieldObj2.NAME = "INVAMOUNT";
            fieldObj2.USE = "Name Field";
            fieldObj2.SET = "$$String:$AMOUNT";
            fieldsList.Add(fieldObj2);

            FIELD fieldObj3 = new();
            fieldObj3.NAME = "Address";
            fieldObj3.USE = "Name Field";
            fieldObj3.SET = "$address";
            fieldsList.Add(fieldObj3);

            FIELD fieldObj4 = new();
            fieldObj4.NAME = "ACTUALQTY";
            fieldObj4.USE = "Name Field";
            fieldObj4.SET = "$$String:$ACTUALQTY";
            fieldsList.Add(fieldObj4);

            FIELD fieldObj5 = new();
            fieldObj5.NAME = "BilledQTY";
            fieldObj5.USE = "Name Field";
            fieldObj5.SET = "$$String:$BilledQTY";
            fieldsList.Add(fieldObj5);

            FIELD fieldObj6 = new();
            fieldObj6.NAME = "INVRATE";
            fieldObj6.USE = "Name Field";
            fieldObj6.SET = "$$String:$RATE";
            fieldsList.Add(fieldObj6);

            FIELD fieldObj7 = new();
            fieldObj7.NAME = "SALESLEDGER";
            fieldObj7.USE = "Name Field";
            fieldObj7.SET = "$LedgerName";
            fieldsList.Add(fieldObj7);

            FIELD fieldObj9 = new();
            fieldObj9.NAME = "VATLEDGERNAME";
            fieldObj9.USE = "Name Field";
            fieldObj9.SET = "$LedgerName";
            fieldsList.Add(fieldObj9);

            FIELD fieldObj10 = new();
            fieldObj10.NAME = "VATAMOUNT";
            fieldObj10.USE = "Name Field";
            fieldObj10.SET = "$$String:$Amount";
            fieldsList.Add(fieldObj10);


            FIELD fieldObj11 = new();
            fieldObj11.NAME = "VMasterID";
            fieldObj11.USE = "Name Field";
            fieldObj11.SET = "$MasterId";
            fieldsList.Add(fieldObj11);

            FIELD fieldObj12 = new();
            fieldObj12.NAME = "Voucherdate";
            fieldObj12.USE = "Name Field";
            fieldObj12.SET = "$date";
            fieldsList.Add(fieldObj12);

            FIELD fieldObj13 = new();
            fieldObj13.NAME = "VTypeName";
            fieldObj13.USE = "Type";
            fieldObj13.SET = "$VoucherTypeName";
            fieldsList.Add(fieldObj13);

            FIELD fieldObj14 = new();
            fieldObj14.NAME = "Reference";
            fieldObj14.USE = "Name Field";
            fieldObj14.SET = "$reference";
            fieldsList.Add(fieldObj14);
            FIELD fieldObj15 = new();
            fieldObj15.NAME = "VTypeName";
            fieldObj15.USE = "Type";
            fieldObj15.SET = "$VoucherTypeName";
            fieldsList.Add(fieldObj15);
            tdlmessage.FIELD = fieldsList;
            tdlmessage.LINE = lines;
            List<COLLECTION>collections=new();
            COLLECTION collection = new();
            collection.NAME = "Collection of Address";
            collection.SOURCECOLLECTION = "Collection of Day Book";
            collection.WALK = "Address";

            collection.FETCH = "Address";
            collections.Add(collection);


            COLLECTION collection2 = new();
            collection2.NAME = "Collection of Allinventoryentries";
            collection2.SOURCECOLLECTION = "Collection of Day Book";
            collection2.WALK = "ALLINVENTORYENTRIES";

            collection2.FETCH = "MasterId,STOCKITEMNAME,Amount,ActualQTY,BilledQTY,RATE,LedgerName";
            collections.Add(collection2);


            COLLECTION collection3 = new();
            collection3.NAME = "Collection of Ledger Entry";
            collection3.SOURCECOLLECTION = "Collection of Day Book";
        

            collection3.FETCH = "MasterId,LedgerName,Amount";
            collections.Add(collection3);

            COLLECTION collection4 = new();
            collection4.NAME = "Collection of Day Book";
            //collection4.SOURCECOLLECTION = "Collection of Day Book";
            collection4.ISFIXED = "No";
            collection4.ISINITIALIZE = "No";
            collection4.ISINTERNAL = "No";
            collection4.ISMODIFY = "No";
            collection4.ISOPTION = "No";
            List<String> types=new();
            types.Add("Vouchers");
            types.Add("Vouchers : Ledger");

            collection4.TYPE = types;
            List<string> filters=new();
            filters.Add("dateSearch");
            filters.Add("ofSpecificVchs");
            collection4.FILTERS = filters;
            collection4.childof = "\"" + ledgerName + "\"";


            collections.Add(collection4);

            tdlmessage.COLLECTION = collections;

           
            List<SYSTEM> systemsList = new List<SYSTEM>();
            SYSTEM system = new SYSTEM();
            system.NAME = "dateSearch";
            system.TYPE = "Formulae";
            //system.Text = StringUtilsCustom.ConvertFormat(date);
            system.Text = "$$String:$Date = $$String:\"" + date + "\" ";
            systemsList.Add(system);
            SYSTEM system2 = new SYSTEM();
            system2.NAME = "ofSpecificVchs";
            system2.TYPE = "Formulae";
            system2.Text = voucherTypeStringBuilder.ToString();
            systemsList.Add(system2);
            tdlmessage.SYSTEM = systemsList;
            tdl.TDLMESSAGE = tdlmessage;
            desc.TDL = tdl;
            body.DESC = desc;
            envelope.BODY = body;
            return envelope;



        }

        internal object getSalesVoucherXml(string companyName, string date, string ledgerName, List<VoucherTypeDTO> voucherTypes)
        {
            StringBuilder voucherTypeStringBuilder = new StringBuilder();
            string prefix = "";
            foreach (VoucherTypeDTO voucherType in voucherTypes)
            {
                String vouchertypeNAme = prefix + " ($VOUCHERTYPENAME = \"" + voucherType.name + "\")";
                voucherTypeStringBuilder.Append(vouchertypeNAme);
                prefix = "or";
            }
            ENVELOPE envelope = new ENVELOPE();
            HEADER header = new HEADER();
            header.VERSION = "1";
            header.TALLYREQUEST = "Export";
            header.TYPE = "Data";
            header.ID = "List of Day Book";
            envelope.HEADER = header;

            BODY body = new BODY();
            DESC desc = new DESC();
            //STATICVARIABLES staticVariables = new STATICVARIABLES();
            //staticVariables.SVCURRENTCOMPANY = companyName;
            //staticVariables.SVEXPORTFORMAT = "$$SysName:XML";
            //desc.STATICVARIABLES = staticVariables;
            TDL tdl = new TDL();
            TDLMESSAGE tdlmessage = new TDLMESSAGE();
            REPORT report = new REPORT();
            report.NAME = "List of Day Book";
            report.FORMS = "List of Day Book";
            report.VARIABLE = "EXPLODEFLAG,SVEXPORTFORMAT,SVCURRENTCOMPANY,SVFROMDATE,SVTODATE";
            List<String> sets = new List<string>();
            sets.Add("EXPLODEFLAG:Yes");
            sets.Add("SVEXPORTFORMAT:$$SysName:XML");
            sets.Add("SVCURRENTCOMPANY:" + companyName);
            report.Set = sets;
            tdlmessage.REPORT = report;

            FORM form = new FORM();
            form.NAME = "List of Day Book";
            form.PARTS = "List of Day Book";
            tdlmessage.FORM = form;
            List<PART> parts = new List<PART>();
            PART part = new();
            part.NAME = "List of Day Book";
            part.LINES = "List of Day Book";
            part.REPEAT = "List of Day Book : Collection of Day Book";
            part.SCROLLED = "Vertical";
            parts.Add(part);

            PART part2 = new();
            part2.NAME = "list of address";
            part2.LINES = "list of address";
            part2.REPEAT = "list of address : Collection of Address";
            part2.SCROLLED = "Vertical";
            parts.Add(part2);

            PART part3 = new();
            part3.NAME = "list of ledger entries";
            part3.LINES = "list of ledger entries";
            part3.REPEAT = "list of ledger entries : Collection of Ledger Entry";
            part3.SCROLLED = "Vertical";
            parts.Add(part3);

            PART part4 = new();
            part4.NAME = "list of inventory";
            part4.LINES = "list of inventory";
            part4.REPEAT = "list of inventory : Collection of Allinventoryentries";
            part4.SCROLLED = "Vertical";
            parts.Add(part4);

            tdlmessage.PART = parts;


            List<LINE> lines = new List<LINE>();

            LINE line = new();
            line.NAME = "List of Day Book";
            line.XMLtag = "VOUCHERS";
            List<String> fields = new();
            fields.Add("VMasterID");
            fields.Add("Voucherdate");
            fields.Add("VTypeName");
            fields.Add("VoucherNumber");
            //fields.Add("Reference");
            line.Fields = fields;
            List<string> explodes = new();
            explodes.Add("list of inventory");
            explodes.Add("list of ledger entries");
            line.explode = explodes;

            lines.Add(line);

            LINE line2 = new();
            line2.NAME = "List of ledger entries";
            line2.XMLtag = "VAT";
            List<String> fields2 = new();
            fields2.Add("VATLEDGERNAME");
            fields2.Add("VATAMOUNT");


            line2.Fields = fields2;
            lines.Add(line2);

            LINE line3 = new();
            line3.NAME = "List of inventory";
            line3.XMLtag = "VAT";
            List<String> fields3 = new();
            fields3.Add("INVNAME");
            fields3.Add("INVAMOUNT");
            fields3.Add("ACTUALQTY");
            fields3.Add("BilledQTY");
            fields3.Add("INVRATE");
            fields3.Add("SALESLEDGER");
            line3.Fields = fields3;
            lines.Add(line3);

            LINE line4 = new();
            line4.NAME = "List of address";
            line4.XMLtag = "Address";
            List<String> fields4 = new();
            fields4.Add("Address");
            line4.Fields = fields4;
            lines.Add(line4);

            //LINE line5 = new();
            //line5.NAME = "List of address";
            //line5.XMLtag = "Address";

            //lines.Add(line5);


            List<FIELD> fieldsList = new List<FIELD>();

            FIELD fieldObj = new();
            fieldObj.NAME = "INVNAME";
            fieldObj.USE = "Name Field";
            fieldObj.SET = "$STOCKITEMNAME";
            fieldsList.Add(fieldObj);
            FIELD fieldObj1 = new FIELD();
            fieldObj1.NAME = "Reference";
            fieldObj1.USE = "Name Field";
            fieldObj1.SET = "$Reference";  // Fetches the Reference Number
            fieldsList.Add(fieldObj1);
            FIELD fieldObj2 = new();
            fieldObj2.NAME = "INVAMOUNT";
            fieldObj2.USE = "Name Field";
            fieldObj2.SET = "$$String:$AMOUNT";
            fieldsList.Add(fieldObj2);

            FIELD fieldObj3 = new();
            fieldObj3.NAME = "Address";
            fieldObj3.USE = "Name Field";
            fieldObj3.SET = "$address";
            fieldsList.Add(fieldObj3);

            FIELD fieldObj4 = new();
            fieldObj4.NAME = "ACTUALQTY";
            fieldObj4.USE = "Name Field";
            fieldObj4.SET = "$$String:$ACTUALQTY";
            fieldsList.Add(fieldObj4);

            FIELD fieldObj5 = new();
            fieldObj5.NAME = "BilledQTY";
            fieldObj5.USE = "Name Field";
            fieldObj5.SET = "$$String:$BilledQTY";
            fieldsList.Add(fieldObj5);

            FIELD fieldObj6 = new();
            fieldObj6.NAME = "INVRATE";
            fieldObj6.USE = "Name Field";
            fieldObj6.SET = "$$String:$RATE";
            fieldsList.Add(fieldObj6);

            FIELD fieldObj7 = new();
            fieldObj7.NAME = "SALESLEDGER";
            fieldObj7.USE = "Name Field";
            fieldObj7.SET = "$LedgerName";
            fieldsList.Add(fieldObj7);

            FIELD fieldObj9 = new();
            fieldObj9.NAME = "VATLEDGERNAME";
            fieldObj9.USE = "Name Field";
            fieldObj9.SET = "$LedgerName";
            fieldsList.Add(fieldObj9);

            FIELD fieldObj10 = new();
            fieldObj10.NAME = "VATAMOUNT";
            fieldObj10.USE = "Name Field";
            fieldObj10.SET = "$$String:$Amount";
            fieldsList.Add(fieldObj10);


            FIELD fieldObj11 = new();
            fieldObj11.NAME = "VMasterID";
            fieldObj11.USE = "Name Field";
            fieldObj11.SET = "$MasterId";
            fieldsList.Add(fieldObj11);

            FIELD fieldObj12 = new();
            fieldObj12.NAME = "Voucherdate";
            fieldObj12.USE = "Name Field";
            fieldObj12.SET = "$date";
            fieldsList.Add(fieldObj12);

            FIELD fieldObj13 = new();
            fieldObj13.NAME = "VTypeName";
            fieldObj13.USE = "Type";
            fieldObj13.SET = "$VoucherTypeName";
            fieldsList.Add(fieldObj13);

            FIELD fieldObj14 = new();
            fieldObj14.NAME = "VoucherNumber";
            fieldObj14.USE = "Name Field";
            fieldObj14.SET = "$VoucherNumber";
            fieldsList.Add(fieldObj14);

            tdlmessage.FIELD = fieldsList;
            tdlmessage.LINE = lines;
            List<COLLECTION> collections = new();
            COLLECTION collection = new();
            collection.NAME = "Collection of Address";
            collection.SOURCECOLLECTION = "Collection of Day Book";
            collection.WALK = "Address";


            collection.FETCH = "Address";
            collections.Add(collection);


            COLLECTION collection2 = new();
            collection2.NAME = "Collection of Allinventoryentries";
            collection2.SOURCECOLLECTION = "Collection of Day Book";
            collection2.WALK = "ALLINVENTORYENTRIES";

            collection2.FETCH = "MasterId,STOCKITEMNAME,Amount,ActualQTY,BilledQTY,RATE,LedgerName";
            collections.Add(collection2);


            COLLECTION collection3 = new();
            collection3.NAME = "Collection of Ledger Entry";
            collection3.SOURCECOLLECTION = "Collection of Day Book";


            collection3.FETCH = "MasterId,LedgerName,Amount";
            collections.Add(collection3);

            COLLECTION collection4 = new();
            collection4.NAME = "Collection of Day Book";
            //collection4.SOURCECOLLECTION = "Collection of Day Book";
            collection4.ISFIXED = "No";
            collection4.ISINITIALIZE = "No";
            collection4.ISINTERNAL = "No";
            collection4.ISMODIFY = "No";
            collection4.ISOPTION = "No";
            List<String> types = new();
            types.Add("Vouchers");
            types.Add("Vouchers : Ledger");

            collection4.TYPE = types;
            List<string> filters = new();
            filters.Add("dateSearch");
            filters.Add("ofSpecificVchs");
            collection4.FILTERS = filters;
            collection4.childof = "\"" + ledgerName + "\"";


            collections.Add(collection4);
            //COLLECTION collection5 =  
            //new COLLECTION
            //{
            //    NAME = "Collection of Day Book",
            //    ISFIXED = "No",
            //    ISINITIALIZE = "No",
            //    ISINTERNAL = "No",
            //    ISMODIFY = "No",
            //    ISOPTION = "No",
            //    TYPE = new List<string> { "Vouchers", "Vouchers : Ledger" },
            //    FILTERS = new List<string> { "dateSearch", "ofSpecificVchs" },
            //    childof = "\"" + ledgerName + "\"",
            //    FETCH = "MasterId,Date,VoucherTypeName,VoucherNumber,Reference"
            //};
            //collections.Add (collection5);

            tdlmessage.COLLECTION = collections;


            List<SYSTEM> systemsList = new List<SYSTEM>();
            SYSTEM system = new SYSTEM();
            system.NAME = "dateSearch";
            system.TYPE = "Formulae";
            //system.Text = StringUtilsCustom.ConvertFormat(date);
            system.Text = "$$String:$Date = $$String:\"" + date + "\" ";
            systemsList.Add(system);
            SYSTEM system2 = new SYSTEM();
            system2.NAME = "ofSpecificVchs";
            system2.TYPE = "Formulae";
            system2.Text = voucherTypeStringBuilder.ToString();
            systemsList.Add(system2);
            tdlmessage.SYSTEM = systemsList;
            tdl.TDLMESSAGE = tdlmessage;
            desc.TDL = tdl;
            body.DESC = desc;
            envelope.BODY = body;
            return envelope;
        }
        
    }
   
}

