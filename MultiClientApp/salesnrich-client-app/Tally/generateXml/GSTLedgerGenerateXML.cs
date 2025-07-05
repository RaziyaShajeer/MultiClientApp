using SNR_ClientApp.Properties;
using SNR_ClientApp.TallyResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace SNR_ClientApp.Tally.generateXml
{
	public static class GSTLedgerGenerateXML
	{
		public static ENVELOPE GstLedgerGenerateXml(string parent)
		{
			ENVELOPE tallyRequest = new ENVELOPE();
			HEADER header = new HEADER();
			header.VERSION = "1";
			header.TALLYREQUEST = "Export";
			header.TYPE = "Collection";
			header.ID = "All Ledgers";
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
			report.NAME = "All Ledgers";
			report.ISMODIFY = "No";
			report.ISFIXED = "No";
			report.ISINITIALIZE = "No";
			report.ISOPTION = "No";
			report.ISINTERNAL = "No";
			report.FORMS = "All Ledgers";
			tdlmessage.REPORT = report;
			FORM form = new FORM();
			form.NAME = "All Ledgers";
			form.ISMODIFY = "No";
			form.ISFIXED = "No";
			form.ISINITIALIZE = "No";
			form.ISOPTION = "No";
			form.ISINTERNAL = "No";
			form.TOPPARTS = "All Ledgers";
			form.XMLTAG = "AllLedgers";
			tdlmessage.FORM = form;
			PART part = new PART();
			part.NAME = "All Ledgers";
			part.ISMODIFY = "No";
			part.ISFIXED = "No";
			part.ISINITIALIZE = "No";
			part.ISOPTION = "No";
			part.ISINTERNAL = "No";
			part.TOPLINES = "Ledger Line";
			part.REPEAT = "Ledger Line : All Ledgers";
			part.SCROLLED = "Vertical";
			part.VERTICAL = "Yes";
			List<PART> parts = new List<PART>();
			parts.Add(part);
			tdlmessage.PART = parts;
			LINE line = new LINE();
			line.NAME = "Ledger Line";

			line.ISMODIFY = "No";
			line.ISFIXED = "No";
			line.ISINITIALIZE = "No";
			line.ISOPTION = "No";
			line.ISINTERNAL = "No";
			line.XMLtag = "Ledger";
			// KEY MODIFICATION: Add FIELDS property to connect fields to the line
			line.FIELD = "Field name, Field Parent, Field TaxType, Field SUBTAXTYPE, Field Guid,Field GSTDUTYHEAD FROM";
			List<LINE> lines = new List<LINE>();
			lines.Add(line);
			tdlmessage.LINE = lines;
			List<FIELD> fieldList = new List<FIELD>();
			FIELD field = new FIELD();
			field.NAME = "Field name";
			field.ISMODIFY = "No";
			field.ISFIXED = "No";
			field.ISINITIALIZE = "No";
			field.ISOPTION = "No";
			field.ISINTERNAL = "No";
			field.SET = "$Name";
			field.XMLTAG = "NAME";
			fieldList.Add(field);
						
			FIELD field4 = new FIELD();
			field4.NAME = "Field Parent";
			field4.ISMODIFY = "No";
			field4.ISFIXED = "No";
			field4.ISINITIALIZE = "No";
			field4.ISOPTION = "No";
			field4.ISINTERNAL = "No";
			field4.SET = "$Parent";
			field4.XMLTAG = "Parent";
			fieldList.Add(field4);
			FIELD field5 = new FIELD();
			field5.NAME = "Field TaxType";
			field5.ISMODIFY = "No";
			field5.ISFIXED = "No";
			field5.ISINITIALIZE = "No";
			field5.ISOPTION = "No";
			field5.ISINTERNAL = "No";
			field5.SET = "$TaxType";
			field5.XMLTAG = "TaxType";
			fieldList.Add(field5);
		
			FIELD field6 = new FIELD();
			field6.NAME = "Field SUBTAXTYPE";
			field6.ISMODIFY = "No";
			field6.ISFIXED = "No";
			field6.ISINITIALIZE = "No";
			field6.ISOPTION = "No";
			field6.ISINTERNAL = "No";
			field6.SET = "$SUBTAXTYPE";
			field6.XMLTAG = "SUBTAXTYPE";
			fieldList.Add(field6);
			FIELD field7 = new FIELD();
			field7.NAME = "Field Guid";
			field7.ISMODIFY = "No";
			field7.ISFIXED = "No";
			field7.ISINITIALIZE = "No";
			field7.ISOPTION = "No";
			field7.ISINTERNAL = "No";
			field7.SET = "$Guid";
			field7.XMLTAG = "GUID";
			fieldList.Add(field7);
			FIELD field8 = new FIELD();
			field8.NAME = "Field GSTDUTYHEAD FROM";
			field8.ISMODIFY = "No";
			field8.ISFIXED = "No";
			field8.ISINITIALIZE = "No";
			field8.ISOPTION = "No";
			field8.ISINTERNAL = "No";
			field8.SET = "$GSTDUTYHEAD FROM";
			field8.XMLTAG = "GSTDUTYHEAD FROM";
			fieldList.Add(field8);
			
			tdlmessage.FIELD = fieldList;


			List<COLLECTION> collectionsList = new List<COLLECTION>();
			COLLECTION collection = new COLLECTION();
			collection.NAME = "All Ledgers";
			collection.ISMODIFY = "No";
			collection.ISFIXED = "No";
			collection.ISINITIALIZE = "No";
			collection.ISOPTION = "No";
			collection.ISINTERNAL = "No";
			List<String> types = new List<String>();
			types.Add("Ledgers");
			collection.TYPE = types;
			List<String> fetch = new List<string>();
			collection.FETCH = "Name,Parent,TaxType,Guid,GSTDUTYHEAD,SUBTAXTYPE FROM";
			collectionsList.Add(collection);
			tdlmessage.COLLECTION = collectionsList;
			tdl.TDLMESSAGE = tdlmessage;
			desc.TDL = tdl;
			body.DESC = desc;
			tallyRequest.BODY = body;
			return tallyRequest;
		}
	}
}
