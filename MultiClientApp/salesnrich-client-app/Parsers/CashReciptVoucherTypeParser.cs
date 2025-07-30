using SNR_ClientApp.TallyResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace SNR_ClientApp.Parsers
{
	internal class CashReciptVoucherTypeParser
	{
		public async Task<List<string>> getAllReciptVochertypeParser(string tallyResponseXml)
		{
			List<string> vouchertypes = new List<string>();
			tallyResponseXml = tallyResponseXml.Replace("&#4;", " ");
			XmlDocument doc = new XmlDocument();

			doc.LoadXml(tallyResponseXml);

			XmlNodeList vouchertypenodes = doc.GetElementsByTagName("VOUCHERTYPE");
			foreach (XmlNode node in vouchertypenodes)
			{
				string pattern = @"(&#13;&#10;|&#13;|&#10;|[\r\n\t])+";
				string name = node["NAME"]?.InnerText ?? "";
				string parent = node["PARENT"]?.InnerText ?? "";
				name = Regex.Replace(name, pattern, "").Trim();
				//name = CleanEncodingIssues(name);
				if(!string.IsNullOrEmpty(name) && parent== "Receipt")
				{
					vouchertypes.Add(name);
				}
							
			}
			return vouchertypes;
		}
		public async Task<List<string>> getCreditNotVochertypeParser(string tallyResponseXml)
		{
			List<string> vouchertypes = new List<string>();
			tallyResponseXml = tallyResponseXml.Replace("&#4;", " ");
			XmlDocument doc = new XmlDocument();

			doc.LoadXml(tallyResponseXml);

			XmlNodeList vouchertypenodes = doc.GetElementsByTagName("VOUCHERTYPE");
			foreach (XmlNode node in vouchertypenodes)
			{
				string pattern = @"(&#13;&#10;|&#13;|&#10;|[\r\n\t])+";
				string name = node["NAME"]?.InnerText ?? "";
				name = Regex.Replace(name, pattern, "").Trim();
				//name = CleanEncodingIssues(name);
				string parent = node["PARENT"]?.InnerText ?? "";
				if (!string.IsNullOrEmpty(name)&&parent== "Credit Note")
				{
					vouchertypes.Add(name);
				}

			}
			return vouchertypes;
		}
		public async Task<List<string>> getCreditNotVAtVochertypeParser(string tallyResponseXml)
		{
			List<string> vouchertypes = new List<string>();
			tallyResponseXml = tallyResponseXml.Replace("&#4;", " ");
			XmlDocument doc = new XmlDocument();

			doc.LoadXml(tallyResponseXml);

			XmlNodeList vouchertypenodes = doc.GetElementsByTagName("VOUCHERTYPE");
			foreach (XmlNode node in vouchertypenodes)
			{
				string pattern = @"(&#13;&#10;|&#13;|&#10;|[\r\n\t])+";
				string name = node["NAME"]?.InnerText ?? "";
				name = Regex.Replace(name, pattern, "").Trim();
				//name = CleanEncodingIssues(name);
				string parent = node["PARENT"]?.InnerText ?? "";
				if (!string.IsNullOrEmpty(name) && parent == "Credit Note (VAT)")
				{
					vouchertypes.Add(name);
				}

			}
			return vouchertypes;
		}
		private static string CleanEncodingIssues(string input)
		{
			if (string.IsNullOrEmpty(input))
				return input;

			return input
				.Replace("�", "\t")
				.Replace("?", "")

				.Trim();
		}
	}
}