using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SNR_ClientApp.Parsers
{
	internal class LedgerNameUnderParentParser
	{
		public Task<List<string>> allLedgersUnderParent(string tallyResponseXml)
		{
			List<string> ledgernames = new List<string>();	
			tallyResponseXml = tallyResponseXml.Replace("&#13;", "")
							   .Replace("&#10;", "")
							   .Replace("&#4;", " ")
							   .Replace("\u0004", "");
			var doc = XDocument.Parse(tallyResponseXml);
			var ledgerList = doc.Descendants("LEDGER");
			foreach (var node in ledgerList)
			{
				string pattern = @"(&#13;&#10;|&#13;|&#10;|[\r\n\t])+";



				string name = node.Attribute("NAME")?.Value ?? "";
				name = Regex.Replace(name, pattern, "").Trim();
				name = CleanEncodingIssues(name);
				if(name!="")
				{
					ledgernames.Add(name);
				}
			
			}
			return Task.FromResult(ledgernames);
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
