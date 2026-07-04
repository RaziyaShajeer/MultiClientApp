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
						 ;
			var doc = XDocument.Parse(tallyResponseXml);
			var ledgerList = doc.Descendants("LEDGER");
			foreach (var node in ledgerList)
			{
				string pattern = @"(&#13;&#10;|&#13;|&#10;|[\r\n\t])+";



				string name = node.Attribute("NAME")?.Value ?? "";
				name = Regex.Replace(name, pattern, "").Trim();
				name = CleanEncodingIssues(name);

				if(!string.IsNullOrEmpty(name))
				{
					ledgernames.Add(name);
				}
			
			}
			return Task.FromResult(ledgernames);


		}
		public Task<List<string>> allLedgersUnderParentIndirectIncomes(string tallyResponseXml)
		{
			List<string> ledgernames = new List<string>();
			tallyResponseXml = tallyResponseXml.Replace("&#13;", "")
						   .Replace("&#10;", "")
						   .Replace("&#4;", " ")
						 ;
			var doc = XDocument.Parse(tallyResponseXml);
			var ledgerList = doc.Descendants("LEDGER");
			foreach (var node in ledgerList)
			{
				string pattern = @"(&#13;&#10;|&#13;|&#10;|[\r\n\t])+";



				string name = node.Attribute("NAME")?.Value ?? "";
				name = Regex.Replace(name, pattern, "").Trim();
				name = CleanEncodingIssues(name);
				string parentnode = node.Element("PARENT")?.Value ?? "";

				if (!string.IsNullOrEmpty(name))
				{
					ledgernames.Add(name);
				}

			}
			return Task.FromResult(ledgernames);

		}
		public Task<List<string>> allLedgersUnderParentIndirectExpenses(string tallyResponseXml)
		{
			List<string> ledgernames = new List<string>();
			tallyResponseXml = tallyResponseXml.Replace("&#13;", "")
						   .Replace("&#10;", "")
						   .Replace("&#4;", " ")
						 ;
			var doc = XDocument.Parse(tallyResponseXml);
			var ledgerList = doc.Descendants("LEDGER");
			foreach (var node in ledgerList)
			{
				string pattern = @"(&#13;&#10;|&#13;|&#10;|[\r\n\t])+";




				string name = node.Attribute("NAME")?.Value ?? "";
				name = Regex.Replace(name, pattern, "").Trim();
				name = CleanEncodingIssues(name);
				string parentnode = node.Element("PARENT")?.Value ?? "";
				if (!string.IsNullOrEmpty(name))
				{
					ledgernames.Add(name);
				}

			}
			return Task.FromResult(ledgernames);

		}
		public Task<List<string>> allLedgersUnderParentgroup(string tallyResponseXml,string parent)
		{
			List<string> ledgernames = new List<string>();
			tallyResponseXml = tallyResponseXml.Replace("&#13;", "")
						   .Replace("&#10;", "")
						   .Replace("&#4;", " ")
						 ;
			var doc = XDocument.Parse(tallyResponseXml);
			var ledgerList = doc.Descendants("LEDGER");
			foreach (var node in ledgerList)
			{
				string pattern = @"(&#13;&#10;|&#13;|&#10;|[\r\n\t])+";



				string name = node.Attribute("NAME")?.Value ?? "";
				string parentnode = node.Element("PARENT")?.Value ?? "";
				name = Regex.Replace(name, pattern, "").Trim();
				name = CleanEncodingIssues(name);
				if (!string.IsNullOrEmpty(name))
				{
					ledgernames.Add(name);
				}

			}
			return Task.FromResult(ledgernames);

		}

		public Task<List<string>> allLedgersUnderParentBankAccount(string tallyResponseXml)
		{
			List<string> ledgernames = new List<string>();
			tallyResponseXml = tallyResponseXml.Replace("&#13;", "")
						   .Replace("&#10;", "")
						   .Replace("&#4;", " ")
						 ;
			var doc = XDocument.Parse(tallyResponseXml);
			var ledgerList = doc.Descendants("LEDGER");
			foreach (var node in ledgerList)
			{
				string pattern = @"(&#13;&#10;|&#13;|&#10;|[\r\n\t])+";



				string name = node.Attribute("NAME")?.Value ?? "";
				string parent= node.Element("PARENT")?.Value ?? "";
				name = Regex.Replace(name, pattern, "").Trim();
				name = CleanEncodingIssues(name);
				if (!string.IsNullOrEmpty(name))
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
