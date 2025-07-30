using SNR_ClientApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SNR_ClientApp.Parsers
{
	internal class ProductGroupProductGSTxmlParser
	{
		public List<GSTProductGroupWiseDTO> parseGstProductgroupparser(string tallyResponseXml)
		{
			var allGstProductGroup = new List<GSTProductGroupWiseDTO>();
			tallyResponseXml = tallyResponseXml.Replace("&#13;", "")
						   .Replace("&#10;", "")
						   .Replace("&#4;", " ")
						 ;

			var doc = XDocument.Parse(tallyResponseXml);
			var stockgroups = doc.Descendants("STOCKGROUP");
			foreach (var item in stockgroups)
			{

				var gratedTax = item.Element("INTEGRATEDTAX")?.Value ?? "";
	


				GSTProductGroupWiseDTO dto = new GSTProductGroupWiseDTO()
				{

					productGroupName = item.Element("NAME")?.Value ?? "",
					hsnsacCode = item.Element("HSNCODE")?.Value ?? "",
					taxType = item.Element("TAXTYPE")?.Value ?? "",
					integratedTax = item.Element("INTEGRATEDTAX")?.Value ?? "",
					centralTax = item.Element("CENTRALTAX")?.Value ?? "",
					stateTax = item.Element("STATETAX")?.Value ?? ""


				};
				if(!string.IsNullOrEmpty(dto.integratedTax))
				{
					if(dto.integratedTax!="0")
					{
						allGstProductGroup.Add(dto);
					}
		
				}
				
			}
			return allGstProductGroup;

		}
		public string Clean(string input) =>
	input?.Replace("(", "").Replace(")", "").Trim();
	}
}
