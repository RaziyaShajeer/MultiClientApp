using SNR_ClientApp.DTO;
using SNR_ClientApp.Utils;
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
	internal class GoDownNameParser
	{
		public Task<List<string>> getAllGodownNames(string tallyResponseXml)
		{

			List<string> goDownNames = new List<string>();


			//tallyResponseXml = tallyResponseXml.Replace("&apos;", "'");

			//tallyResponseXml = tallyResponseXml.Replace("&amp;", "&");

			tallyResponseXml = tallyResponseXml.Replace("&#13;", "")
							   .Replace("&#10;", "")
							   .Replace("&#4;", " ")
							 ;
			var doc = XDocument.Parse(tallyResponseXml);




			var godownlist = doc.Descendants("GODOWN");





			foreach (var node in godownlist)
			{

				string pattern = @"(&#13;&#10;|&#13;|&#10;|[\r\n\t])+";



				string name = node.Attribute("NAME")?.Value ?? "";
				name = Regex.Replace(name, pattern, "").Trim();
				name = CleanEncodingIssues(name);
				if (name != "")
				{
					goDownNames.Add(name);
				}


			}
			return Task.FromResult(goDownNames);

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