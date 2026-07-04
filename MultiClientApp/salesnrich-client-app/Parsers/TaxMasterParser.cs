using SNR_ClientApp.DTO;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SNR_ClientApp.Parsers
{
	public class TaxMasterParser
	{
		public static List<TaxMasterDTO> ParseTaxMasterListXml(string tallyResponseXml)
		{
			var TaxMasterList = new List<TaxMasterDTO>();

			tallyResponseXml = tallyResponseXml.Replace("&#13;", "")
						   .Replace("&#10;", "")
						   .Replace("&#4;", " ")
						 ;

			var doc = XDocument.Parse(tallyResponseXml);
			List<TaxMasterDTO> taxMasterDTOs = new List<TaxMasterDTO>();
			var TaxMAsters = doc.Descendants("LEDGER");
			foreach (var item in TaxMAsters)
			{
				string taxtype = item.Element("TAXTYPE")?.Value ?? "";
				if (taxtype.Equals("GST"))
				{
					TaxMasterDTO taxMasterDTO = new TaxMasterDTO();

					taxMasterDTO.vatName = item.Element("NAME")?.Value ?? "";
					taxMasterDTO.pid = item.Element("GUID")?.Value ?? "";
					taxMasterDTO.vatClass = item.Element("TAXCLASSIFICATIONNAME")?.Value ?? "";

					taxMasterDTO.alterId = long.TryParse(item.Element("ALTERID")?.Value, out long id) ? id : 0;
					double percentageOfCalculation = item.Element("VATCLASSIFICATIONRATE").Value != " " ? ((StringUtilsCustom.ExtractDoubleValue(item.Element("VATCLASSIFICATIONRATE").ToString()))) : 0;
					if (percentageOfCalculation == 0)
					{
						//  String b = taxMasterDTO.vatName.Replace("[^\\d.]", "");

						String b = String.Concat(taxMasterDTO.vatName.Where(char.IsDigit));
						try
						{
							if (b != "")
							{
								double c = Convert.ToDouble(b);
								percentageOfCalculation = c;
							}
						}
						catch (Exception e)
						{
							LogManager.WriteLog("number format exceptoion name that converting to String : "
									+ taxMasterDTO.vatName);
						}
						if (percentageOfCalculation != 0)
						{
							String vatClass = taxMasterDTO.vatName.Replace("\\P{L}", "");
							taxMasterDTO.vatClass = vatClass;
							taxMasterDTO.vatPercentage = percentageOfCalculation;
							taxMasterDTOs.Add(taxMasterDTO);
						}
					}

				}
			}
			return taxMasterDTOs;
		}
	}
}

