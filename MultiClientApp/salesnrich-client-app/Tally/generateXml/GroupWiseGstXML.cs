using SNR_ClientApp.Properties;
using SNR_ClientApp.TallyResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Xml.Linq;

namespace SNR_ClientApp.Tally.generateXml
{
	public static class GroupWiseGSTXML
	{
		public static ENVELOPE GroupWiseGstXml()
		{

			ENVELOPE tallyRequest = new ENVELOPE();
			HEADER header = new HEADER();
			header.VERSION = "1";
			header.TALLYREQUEST = "Export";
			header.TYPE = "Data";
			header.ID = "GST RATE SETUP";
			tallyRequest.HEADER = header;
			BODY body = new();
			DESC desc = new();

			STATICVARIABLES staticvariables = new STATICVARIABLES();
			staticvariables.EXPLODEFLAG = "Yes";
			staticvariables.SVCURRENTCOMPANY = ApplicationProperties.properties["tally.company"].ToString();
			staticvariables.SVEXPORTFORMAT = "$$SysName:XML";
			staticvariables.IsItemWise = "Yes";
			desc.STATICVARIABLES = staticvariables;

			body.DESC = desc;
			tallyRequest.BODY = body;
			return tallyRequest;

			
		}

	}
}
