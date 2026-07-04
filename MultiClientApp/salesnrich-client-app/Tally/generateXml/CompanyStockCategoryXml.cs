using SNR_ClientApp.Properties;
using SNR_ClientApp.TallyResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Tally.generateXml
{
	public class CompanyStockCategoryXml
	{
		public ENVELOPE getCompanyStockCategoryXml()
		{

			ENVELOPE tallyRequest = new ENVELOPE();
			HEADER header = new HEADER();
			header.VERSION = "1";
			header.TALLYREQUEST = "Export";
			header.TYPE = "Data";
			header.ID = "List of Stock Categories";
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
			report.NAME = "List of Stock Categories";
			report.ISMODIFY = "No";
			report.ISFIXED = "No";
			report.ISINITIALIZE = "No";
			report.ISOPTION = "No";
			report.ISINTERNAL = "No";
			report.FORMS = "List of Stock Categories";
			tdlmessage.REPORT = report;
			FORM form = new FORM();
			form.NAME = "List of Stock Categories";
			form.ISMODIFY = "No";
			form.ISFIXED = "No";
			form.ISINITIALIZE = "No";
			form.ISOPTION = "No";
			form.ISINTERNAL = "No";
			form.TOPPARTS = "Part Stock Categories";
			form.XMLTAG = "LISTOFSTOCKCATEGORIES";
			tdlmessage.FORM = form;
			PART part = new PART();
			part.NAME = "Part Stock Categories";
			part.ISMODIFY = "No";
			part.ISFIXED = "No";
			part.ISINITIALIZE = "No";
			part.ISOPTION = "No";
			part.ISINTERNAL = "No";
			part.TOPLINES = "Line Stock Category";
			part.REPEAT = "Line Stock Category : Collection of Stock Categories";
			part.SCROLLED = "Vertical";
			part.VERTICAL = "Yes";
			List<PART> parts = new List<PART>();
			parts.Add(part);
			tdlmessage.PART = parts;
			LINE line = new LINE();
			line.NAME = "Line Stock Category";
			
			line.ISMODIFY = "No";
			line.ISFIXED = "No";
			line.ISINITIALIZE = "No";
			line.ISOPTION = "No";
			line.ISINTERNAL = "No";
			line.XMLtag = "STOCKCATEGORY";
			// KEY MODIFICATION: Add FIELDS property to connect fields to the line
			line.FIELD = "Field Name Stock Category, Field Parent Stock Category,Field Guid Stock Category,Field alterid Stock Category";
			List<LINE> lines = new List<LINE>();
			lines.Add(line);
			tdlmessage.LINE = lines;
			List<FIELD> fieldList = new List<FIELD>();
			FIELD field = new FIELD();
			field.NAME = "Field Name Stock Category";
			field.ISMODIFY = "No";
			field.ISFIXED = "No";
			field.ISINITIALIZE = "No";
			field.ISOPTION = "No";
			field.ISINTERNAL = "No";
			field.SET = "$Name";
			field.XMLTAG = "NAME";
			fieldList.Add(field);
			FIELD field2 = new FIELD();
			field2.NAME = "Field Guid Stock Category";
			field2.ISMODIFY = "No";
			field2.ISFIXED = "No";
			field2.ISINITIALIZE = "No";
			field2.ISOPTION = "No";
			field2.ISINTERNAL = "No";
			field2.SET = "$Guid";
			field2.XMLTAG = "GUID";
			fieldList.Add(field2);
			FIELD field3 = new FIELD();
			field3.NAME = "Field Parent Stock Category";
			field3.ISMODIFY = "No";
			field3.ISFIXED = "No";
			field3.ISINITIALIZE = "No";
			field3.ISOPTION = "No";
			field3.ISINTERNAL = "No";
			field3.SET = "$Parent";
			field3.XMLTAG = "Parent";
			fieldList.Add(field3);
			FIELD field4= new FIELD();
			field4.NAME = "Field alterid Stock Category";
			field4.ISMODIFY = "No";
			field4.ISFIXED = "No";
			field4.ISINITIALIZE = "No";
			field4.ISOPTION = "No";
			field4.ISINTERNAL = "No";
			field4.SET = "$alterid";
			field4.XMLTAG = "ALTERID";
			fieldList.Add(field4);
			tdlmessage.FIELD = fieldList;
			List<COLLECTION> collectionsList = new List<COLLECTION>();
			COLLECTION collection = new COLLECTION();
			collection.NAME = "Collection of Stock Categories";
			collection.ISMODIFY = "No";
			collection.ISFIXED = "No";
			collection.ISINITIALIZE = "No";
			collection.ISOPTION = "No";
			collection.ISINTERNAL = "No";
			List<String> types = new List<String>();
			types.Add("Stock Category");
			collection.TYPE = types;
			List<String> fetch = new List<string>();
			fetch.Add("Name");
			fetch.Add("Parent");
			fetch.Add("Guid");
			fetch.Add("alterid");
			collection.FETCH = "Name,Parent,Guid,alterid";
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
