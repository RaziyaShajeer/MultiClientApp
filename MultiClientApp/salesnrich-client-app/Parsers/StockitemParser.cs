using SNR_ClientApp.DTO;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SNR_ClientApp.Parsers
{
	public class StockitemParser
	{
		public  List<ProductProfileDTO> ParseStockItemListXml(string tallyResponseXml)
		{
			var allStockItems = new List<ProductProfileDTO>();
			tallyResponseXml = tallyResponseXml.Replace("\u0004", "");
			tallyResponseXml = tallyResponseXml.Replace("&#4;", " ");
			tallyResponseXml = tallyResponseXml.Replace("&apos;", "'");
			var doc = XDocument.Parse(tallyResponseXml);
			var stockItems = doc.Descendants("STOCKITEM");
			foreach (var item in stockItems)
			{
				var dto = new ProductProfileDTO
				{
					name = item.Element("NAME")?.Value ?? "",
					productId = item.Element("GUID")?.Value ?? "",

				};

				allStockItems.Add(dto);
			}
			return allStockItems;

		}
		public List<TPProductGroupProductDTO> getAllProductGroupsproduct(string tallyResponseXml)
		{
			var allStockItems = new List<TPProductGroupProductDTO>();
			tallyResponseXml = tallyResponseXml.Replace("&#4;", " ");
			tallyResponseXml = tallyResponseXml.Replace("&apos;", "'");
			tallyResponseXml = tallyResponseXml.Replace("&", "&");

			var doc = XDocument.Parse(tallyResponseXml);
			var stockItems = doc.Descendants("STOCKITEM");
			foreach (var item in stockItems)
			{

				TPProductGroupProductDTO dto = new TPProductGroupProductDTO()
				{

					productName = item.Attribute("NAME")?.Value ?? "",
					groupName = item.Element("PARENT")?.Value ?? "",
					alterId = Convert.ToInt32(item.Element("ALTERID")?.Value ?? "0")


				};
				if (!string.IsNullOrEmpty(dto.productName))
				{
					allStockItems.Add(dto);
				}


			}
			return allStockItems;
			

		}
	}
}
