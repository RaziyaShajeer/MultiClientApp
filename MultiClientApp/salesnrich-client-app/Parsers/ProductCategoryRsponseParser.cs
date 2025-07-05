using SNR_ClientApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SNR_ClientApp.Parsers
{
	public static class ProductCategoryRsponseParser
	{
		public static List<ProductCategoryDTO> ParseStockCategoryListXml(string tallyResponseXml)
		{
			var categoryList = new List<ProductCategoryDTO>();
			tallyResponseXml = tallyResponseXml.Replace("&#4;", " ");
			tallyResponseXml = tallyResponseXml.Replace("&apos;", "'");
			tallyResponseXml = tallyResponseXml.Replace("&", "&");
			var doc = XDocument.Parse(tallyResponseXml);
			var stockCategories = doc.Descendants("STOCKCATEGORY");

			foreach (var item in stockCategories)
			{
				var dto = new ProductCategoryDTO
				{
					name = item.Element("NAME")?.Value ?? "",
					pid = item.Element("GUID")?.Value ?? "",
					productCategoryId = item.Element("GUID")?.Value ?? "",
					alterId = double.TryParse(item.Element("ALTERID")?.Value, out double id) ? id : 0,
					activated = true, // or set based on any logic
					alias = null,     // set if available in XML
					description = null // set if needed
				};

				categoryList.Add(dto);
			}

			return categoryList;

		}
	}
}
