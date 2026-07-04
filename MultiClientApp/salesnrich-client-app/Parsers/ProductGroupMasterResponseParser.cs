using SNR_ClientApp.DTO;
using SNR_ClientApp.Tally;
using SNR_ClientApp.TallyResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SNR_ClientApp.Parsers
{
	public static class ProductGroupMasterResponseParser
	{
		public static List<ProductGroupDTO> ParseStockGroupListXml(string tallyResponseXml)

		{
			List<ProductGroupDTO> productGroups = new List<ProductGroupDTO>();
			tallyResponseXml = tallyResponseXml.Replace("&#13;", "")
								   .Replace("&#10;", "")
								   .Replace("&#4;", " ")
								 ;
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(tallyResponseXml);

			XmlNodeList stockGroupNodes = doc.GetElementsByTagName("STOCKGROUP");

			foreach (XmlNode node in stockGroupNodes)
			{
				string name = node["NAME"]?.InnerText ?? "";
				string alterIdStr = node["ALTERID"]?.InnerText?.Trim() ?? "0";
				double alterId = double.TryParse(alterIdStr, out double tempAlterId) ? tempAlterId : 0.0;

				ProductGroupDTO dto = new ProductGroupDTO(
					ProductGroupId: node["GUID"]?.InnerText?.Trim() ?? Guid.NewGuid().ToString(),
					Name: name,
					Taxrate: 0.0, // Assuming no tax rate in XML, default to 0
					AlterId: alterId
				);

				productGroups.Add(dto);
			}

			return productGroups;
		}
		
		
	}
}
