using SNR_ClientApp.Properties;
using SNR_ClientApp.TallyResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Tally.generateXml
{
	internal static class CashRecieptVocherTypeGenerateXML
	{
		public static ENVELOPE getAllReciptVoucherTypeGenerateXml(string parent)
		{
			
			List<string> Parents = new List<string>(); ;
			Parents.Add("Receipt");
			
			StringBuilder voucherTypeStringBuilder = new StringBuilder();



			String vouchertypeNAme = " ($Parent= \"" + parent + "\")";
			voucherTypeStringBuilder.Append(vouchertypeNAme);


			ENVELOPE tallyRequest = new ENVELOPE();
			HEADER header = new HEADER();
			header.VERSION = "1";
			header.TALLYREQUEST = "Export";
			header.TYPE = "Data";
			header.ID = "List of VoucherType";
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
			report.NAME = "List of VoucherType";
			report.ISMODIFY = "No";
			report.ISFIXED = "No";
			report.ISINITIALIZE = "No";
			report.ISOPTION = "No";
			report.ISINTERNAL = "No";
			report.FORMS = "List of VoucherType";
			tdlmessage.REPORT = report;
			FORM form = new FORM();
			form.NAME = "List of VoucherType";
			form.ISMODIFY = "No";
			form.ISFIXED = "No";
			form.ISINITIALIZE = "No";
			form.ISOPTION = "No";
			form.ISINTERNAL = "No";
			form.TOPPARTS = "List of VoucherType";
			form.XMLTAG = "List of VoucherType";
			tdlmessage.FORM = form;
			PART part = new PART();
			part.NAME = "List of VoucherType";
			part.ISMODIFY = "No";
			part.ISFIXED = "No";
			part.ISINITIALIZE = "No";
			part.ISOPTION = "No";
			part.ISINTERNAL = "No";
			part.TOPLINES = "Line VoucherType";
			part.REPEAT = "Line VoucherType : Collection of VoucherType";
			part.SCROLLED = "Vertical";
			part.VERTICAL = "Yes";
			List<PART> parts = new List<PART>();
			parts.Add(part);
			tdlmessage.PART = parts;
			LINE line = new LINE();
			line.NAME = "Line VoucherType";
			line.ISMODIFY = "No";
			line.ISFIXED = "No";
			line.ISINITIALIZE = "No";
			line.ISOPTION = "No";
			line.ISINTERNAL = "No";
			line.XMLtag = "VoucherType";
			// KEY MODIFICATION: Add FIELDS property to connect fields to the line
			line.FIELD = "Field Name VoucherType";
			List<LINE> lines = new List<LINE>();
			lines.Add(line);
			tdlmessage.LINE = lines;
			List<FIELD> fieldList = new List<FIELD>();
			FIELD field = new FIELD();
			field.NAME = "Field Name VoucherType";
			field.ISMODIFY = "No";
			field.ISFIXED = "No";
			field.ISINITIALIZE = "No";
			field.ISOPTION = "No";
			field.ISINTERNAL = "No";
			field.SET = "$Name";
			field.XMLTAG = "NAME";
			fieldList.Add(field);



			tdlmessage.FIELD = fieldList;
			List<COLLECTION> collectionsList = new List<COLLECTION>();
			COLLECTION collection = new COLLECTION();
			collection.NAME = "Collection of VoucherType";
			collection.ISMODIFY = "No";
			collection.ISFIXED = "No";
			collection.ISINITIALIZE = "No";
			collection.ISOPTION = "No";
			collection.ISINTERNAL = "No";
			List<String> types = new List<String>();
			types.Add("VoucherType");
			collection.TYPE = types;
			// OPTIONAL MODIFICATION: Replace NativeMethod with FETCH for better compatibility
			List<String> fetch = new List<string>();
			fetch.Add("Name");


			collection.FETCH = "Name";
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
			filter.Text = voucherTypeStringBuilder.ToString();
			systems.Add(filter);
			tdlmessage.SYSTEM = systems;
			tdlmessage.COLLECTION = collectionsList;
			tdl.TDLMESSAGE = tdlmessage;
			desc.TDL = tdl;
			body.DESC = desc;
			tallyRequest.BODY = body;
			return tallyRequest;
		}
	}
}
