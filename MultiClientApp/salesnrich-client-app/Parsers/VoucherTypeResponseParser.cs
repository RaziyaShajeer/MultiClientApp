using SNR_ClientApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace SNR_ClientApp.Parsers
{
	public class VoucherTypeResponseParser
	{
		public async Task<List<VoucherTypeDTO>> GetAllvoucherType(string tallyResponseXml,string vouchertypeName)
{
			 List<VoucherTypeDTO> vouchertypes = new List<VoucherTypeDTO>();
			tallyResponseXml = tallyResponseXml.Replace("&#4;", " ");
			XmlDocument doc = new XmlDocument();

			doc.LoadXml(tallyResponseXml);

			XmlNodeList vouchertypenodes = doc.GetElementsByTagName("VOUCHERTYPE");
			foreach (XmlNode node in vouchertypenodes)
			{
				VoucherTypeDTO voucherTypeDTO = new VoucherTypeDTO();
				string pattern = @"(&#13;&#10;|&#13;|&#10;|[\r\n\t])+";
				voucherTypeDTO.name = node["NAME"]?.InnerText ?? "";
				voucherTypeDTO.parent = node["PARENT"]?.InnerText ?? "";
				voucherTypeDTO.name = Regex.Replace(voucherTypeDTO.name, pattern, "").Trim();

				//name = CleanEncodingIssues(name);
				if (!string.IsNullOrEmpty(voucherTypeDTO.name)&& voucherTypeDTO.parent==vouchertypeName)
				{
					vouchertypes.Add(voucherTypeDTO);
				}

			}
			return vouchertypes;
		}

	}
}
	

