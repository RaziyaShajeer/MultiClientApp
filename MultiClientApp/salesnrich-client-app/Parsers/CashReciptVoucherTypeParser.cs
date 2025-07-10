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
				name = Regex.Replace(name, pattern, "").Trim();
				name = CleanEncodingIssues(name);

				vouchertypes.Add(name);
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