using SNR_ClientApp.Properties;
using SNR_ClientApp.TallyResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Tally.generateXml
{
	 public static class TaxMasterGenerateXML
	{
		public static ENVELOPE TaxMasterGenerateXml()
		{
			ENVELOPE tallyRequest = new ENVELOPE();
			HEADER header = new HEADER();
			header.VERSION = "1";
			header.TALLYREQUEST = "Export";
			header.TYPE = "Data";
			header.ID = "List of Ledgers";
			tallyRequest.HEADER = header;
			BODY body = new();
			DESC desc = new();
			STATICVARIABLES staticvariables = new STATICVARIABLES();
			staticvariables.EXPLODEFLAG = "Yes";
			staticvariables.SVCURRENTCOMPANY = ApplicationProperties.properties["tally.company"].ToString();
			staticvariables.SVEXPORTFORMAT = "$$SysName:XML";
			staticvariables.IsItemWise = "Yes";
			desc.STATICVARIABLES = staticvariables;
			TDL tdl = new TDL();
			TDLMESSAGE tdlmessage = new TDLMESSAGE();
			REPORT report = new REPORT();
			report.NAME = "List of Ledgers";
			report.ISMODIFY = "No";
			report.ISFIXED = "No";
			report.ISINITIALIZE = "No";
			report.ISOPTION = "No";
			report.ISINTERNAL = "No";
			report.FORMS = "List of Ledgers";
			tdlmessage.REPORT = report;
			FORM form = new FORM();
			form.NAME = "List of Ledgers";
			form.ISMODIFY = "No";
			form.ISFIXED = "No";
			form.ISINITIALIZE = "No";
			form.ISOPTION = "No";
			form.ISINTERNAL = "No";
			form.TOPPARTS = "List of Ledgers";
			form.XMLTAG = "List of Ledgers";
			tdlmessage.FORM = form;
			PART part = new PART();
			part.NAME = "List of Ledgers";
			part.ISMODIFY = "No";
			part.ISFIXED = "No";
			part.ISINITIALIZE = "No";
			part.ISOPTION = "No";
			part.ISINTERNAL = "No";
			part.TOPLINES = "Line Ledgers";
			part.REPEAT = "Line Ledgers : Collection of Ledgers";
			part.SCROLLED = "Vertical";
			part.VERTICAL = "Yes";
			List<PART> parts = new List<PART>();
			parts.Add(part);
			tdlmessage.PART = parts;
			LINE line = new LINE();
			line.NAME = "Line Ledgers";
			line.ISMODIFY = "No";
			line.ISFIXED = "No";
			line.ISINITIALIZE = "No";
			line.ISOPTION = "No";
			line.ISINTERNAL = "No";
			line.XMLtag = "Ledger";
			// KEY MODIFICATION: Add FIELDS property to connect fields to the line
			line.FIELD = "Field Name Ledger,Field Parent Ledger,Field Address Ledger,Field TAXTYPE Ledger,Field SUBTAXTYPE Ledger,Field TAXCLASSIFICATIONNAME Ledger,Field VATCLASSIFICATIONRATE Ledger,Field PRICELEVEL,alterId";
			List<LINE> lines = new List<LINE>();
			lines.Add(line);
			tdlmessage.LINE = lines;
			List<FIELD> fieldList = new List<FIELD>();
			FIELD field = new FIELD();
			field.NAME = "Field Name Ledger";
			field.ISMODIFY = "No";
			field.ISFIXED = "No";
			field.ISINITIALIZE = "No";
			field.ISOPTION = "No";
			field.ISINTERNAL = "No";
			field.SET = "$Name";
			field.XMLTAG = "NAME";
			fieldList.Add(field);
			FIELD field1 = new FIELD();
			field1.NAME = "Field Parent Ledger";
			field1.ISMODIFY = "No";
			field1.ISFIXED = "No";
			field1.ISINITIALIZE = "No";
			field1.ISOPTION = "No";
			field1.ISINTERNAL = "No";
			field1.SET = "$Parent";
			field1.XMLTAG = "PARENT";
			fieldList.Add(field1);
			FIELD field2 = new FIELD();
			field2.NAME = "Field Address Ledger";
			field2.ISMODIFY = "No";
			field2.ISFIXED = "No";
			field2.ISINITIALIZE = "No";
			field2.ISOPTION = "No";
			field2.ISINTERNAL = "No";
			field2.SET = "$Address";
			field2.XMLTAG = "Address";
			fieldList.Add(field2);
			FIELD field3 = new FIELD();
			field3.NAME = "Field TAXTYPE Ledger";
			field3.ISMODIFY = "No";
			field3.ISFIXED = "No";
			field3.ISINITIALIZE = "No";
			field3.ISOPTION = "No";
			field3.ISINTERNAL = "No";
			field3.SET = "$TAXTYPE";
			field3.XMLTAG = "TAXTYPE";
			fieldList.Add(field3);
			FIELD field4 = new FIELD();
			field4.NAME = "Field SUBTAXTYPE Ledger";
			field4.ISMODIFY = "No";
			field4.ISFIXED = "No";
			field4.ISINITIALIZE = "No";
			field4.ISOPTION = "No";
			field4.ISINTERNAL = "No";
			field4.SET = "$SUBTAXTYPE";
			field4.XMLTAG = "SUBTAXTYPE";
			fieldList.Add(field4);
			FIELD field5 = new FIELD();
			field5.NAME = "Field VATCLASSIFICATIONRATE Ledger";
			field5.ISMODIFY = "No";
			field5.ISFIXED = "No";
			field5.ISINITIALIZE = "No";
			field5.ISOPTION = "No";
			field5.ISINTERNAL = "No";
			field5.SET = "$VATCLASSIFICATIONRATE";
			field5.XMLTAG = "VATCLASSIFICATIONRATE";
			fieldList.Add(field5);
			FIELD field7= new FIELD();
			field7.NAME = "Field TAXCLASSIFICATIONNAME Ledger";
			field7.ISMODIFY = "No";
			field7.ISFIXED = "No";
			field7.ISINITIALIZE = "No";
			field7.ISOPTION = "No";
			field7.ISINTERNAL = "No";
			field7.SET = "$TAXCLASSIFICATIONNAME";
			field7.XMLTAG = "TAXCLASSIFICATIONNAME";
			fieldList.Add(field7);
			FIELD field6 = new FIELD();
			field6.NAME = "Field PRICELEVEL";
			field6.ISMODIFY = "No";
			field6.ISFIXED = "No";
			field6.ISINITIALIZE = "No";
			field6.ISOPTION = "No";
			field6.ISINTERNAL = "No";
			field6.SET = "$PRICELEVEL";
			field6.XMLTAG = "PRICELEVEL";
			fieldList.Add(field6);
			FIELD field8 = new FIELD();
			field8.NAME = "alterId";
			field8.ISMODIFY = "No";
			field8.ISFIXED = "No";
			field8.ISINITIALIZE = "No";
			field8.ISOPTION = "No";
			field8.ISINTERNAL = "No";
			field8.SET = "$alterId";
			field8.XMLTAG = "ALTERID";
			fieldList.Add(field8);
		
			tdlmessage.FIELD = fieldList;
			List<COLLECTION> collectionsList = new List<COLLECTION>();
			COLLECTION collection = new COLLECTION();
			collection.NAME = "Collection of Ledgers";
			collection.ISMODIFY = "No";
			collection.ISFIXED = "No";
			collection.ISINITIALIZE = "No";
			collection.ISOPTION = "No";

			collection.ISINTERNAL = "No";
			List<String> types = new List<String>();
			types.Add("Ledger");
			collection.TYPE = types;
			
			// OPTIONAL MODIFICATION: Replace NativeMethod with FETCH for better compatibility
			List<String> fetch = new List<string>();
			fetch.Add("Name");
			fetch.Add("Parent");
			fetch.Add("Guid");
			fetch.Add("alterid");
			fetch.Add("$RateOfVat");

			collection.FETCH = "Name,PRICELEVEL,Parent,Address,TAXTYPE,SUBTAXTYPE,PRICELEVEL,VATCLASSIFICATIONRATE,alterId";
			// Comment out or remove the NativeMethod if FETCH is used
			// List<String> NativeMethod = new List<string>();
			// NativeMethod.Add("Parent");
			// NativeMethod.Add("Name");
			// NativeMethod.Add("Guid");
			// collection.NativeMethod = NativeMethod;

			
			collectionsList.Add(collection);
			List<String> filters = new List<string>();
			filters.Add("ParentFilter");
			collection.FILTERS = filters;
			tdlmessage.COLLECTION = collectionsList;
			List<SYSTEM> systems = new List<SYSTEM>();

			SYSTEM filter = new SYSTEM();
			filter.NAME = "ParentFilter";
			filter.TYPE = "Formulae";
			filter.Text = "$Parent = \"Duties & Taxes\" OR $Parent = \"GL 13; Duties & Taxes\"";
			systems.Add(filter);
			tdlmessage.SYSTEM = systems;
			tdl.TDLMESSAGE = tdlmessage;
			desc.TDL = tdl;
			body.DESC = desc;
			tallyRequest.BODY = body;
			return tallyRequest;




		}

	}
}
