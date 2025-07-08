using Microsoft.VisualBasic;
using SNR_ClientApp.Properties;
using SNR_ClientApp.TallyResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Tally.generateXml
{
	internal class CompanyStockItemXml
	{
		public ENVELOPE getCompanyStockItemXml()
		{








			ENVELOPE tallyRequest = new ENVELOPE();
			HEADER header = new HEADER();
			header.VERSION = "1";
			header.TALLYREQUEST = "Export";
			header.TYPE = "Collection";
			header.ID = "Collection of Stock Items";
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
			report.NAME = "List of Stock Items";
			report.ISMODIFY = "No";
			report.ISFIXED = "No";
			report.ISINITIALIZE = "No";
			report.ISOPTION = "No";
			report.ISINTERNAL = "No";
			report.FORMS = "List of Stock Items";
			tdlmessage.REPORT = report;
			FORM form = new FORM();
			form.NAME = "List of Stock Items";
			form.ISMODIFY = "No";
			form.ISFIXED = "No";
			form.ISINITIALIZE = "No";
			form.ISOPTION = "No";
			form.ISINTERNAL = "No";
			form.TOPPARTS = "List of Stock Items";
			form.XMLTAG = "List of Stock Items";
			tdlmessage.FORM = form;
			PART part = new PART();
			part.NAME = "List of Stock Items";
			part.ISMODIFY = "No";
			part.ISFIXED = "No";
			part.ISINITIALIZE = "No";
			part.ISOPTION = "No";
			part.ISINTERNAL = "No";
			part.TOPLINES = "List of Stock Items";
			part.REPEAT = "List of Stock Items : Collection of Stock Items";
			part.SCROLLED = "Vertical";
			part.VERTICAL = "Yes";
			List<PART> parts = new List<PART>();
			parts.Add(part);
			tdlmessage.PART = parts;
			LINE line = new LINE();
			line.NAME = "List of Stock Items";

			line.ISMODIFY = "No";
			line.ISFIXED = "No";
			line.ISINITIALIZE = "No";
			line.ISOPTION = "No";
			line.ISINTERNAL = "No";
			line.XMLtag = "StockItem";
			// KEY MODIFICATION: Add FIELDS property to connect fields to the line
			line.FIELD = "Name,Guid";
			List<LINE> lines = new List<LINE>();
			lines.Add(line);
			tdlmessage.LINE = lines;
			List<FIELD> fieldList = new List<FIELD>();
			FIELD field = new FIELD();
			field.NAME = "Name";
			field.ISMODIFY = "No";
			field.ISFIXED = "No";
			field.ISINITIALIZE = "No";
			field.ISOPTION = "No";
			field.ISINTERNAL = "No";
			field.SET = "$Name";
			field.XMLTAG = "NAME";
			fieldList.Add(field);
			FIELD field2 = new FIELD();
			field2.NAME = "Guid";
			field2.ISMODIFY = "No";
			field2.ISFIXED = "No";
			field2.ISINITIALIZE = "No";
			field2.ISOPTION = "No";
			field2.ISINTERNAL = "No";
			field2.SET = "$Guid";
			field2.XMLTAG = "GUID";
			fieldList.Add(field2);
			tdlmessage.FIELD = fieldList;
			List<COLLECTION> collectionsList = new List<COLLECTION>();
			COLLECTION collection = new COLLECTION();
			collection.NAME = "Collection of Stock Items";
			collection.ISMODIFY = "No";
			collection.ISFIXED = "No";
			collection.ISINITIALIZE = "No";
			collection.ISOPTION = "No";
			collection.ISINTERNAL = "No";
			List<String> types = new List<String>();
			types.Add("Stock Item");
			collection.TYPE = types;
			List<String> fetch = new List<string>();
			fetch.Add("Name");
			fetch.Add("Guid");

			collection.FETCH = "Name,Guid";
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
