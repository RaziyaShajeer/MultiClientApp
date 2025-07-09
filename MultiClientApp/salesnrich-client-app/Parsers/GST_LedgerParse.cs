using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SNR_ClientApp.Parsers
{
	public class GST_LedgerParse
	{
		public List<GstLedgerDTO> getAllGstLEdgers(string tallyResponseXml)
		{
			
			List<GstLedgerDTO > lst = new List<GstLedgerDTO>();
			try
			{



				

				tallyResponseXml = tallyResponseXml.Replace("&#13;", "")
							   .Replace("&#10;", "")
							   .Replace("&#4;", " ");
				var doc = XDocument.Parse(tallyResponseXml);
				var ledgerList = doc.Descendants("LEDGER");
				int ledgerCount = ledgerList.Count();

				//tallyResponseXml = tallyResponseXml.Replace("&apos;", "&");
				//tallyResponseXml = tallyResponseXml.Replace("&", "&");
				//tallyResponseXml = tallyResponseXml.Replace("&amp;", "&");

				//var doc = XDocument.Parse(tallyResponseXml);
				//var ledgerList = doc.Descendants("LEDGER");
				foreach (var item in ledgerList)
				{
					string taxtype = item.Element("TAXTYPE")?.Value ?? "";
					if (taxtype.Equals("GST"))
					{
						GstLedgerDTO dto = new GstLedgerDTO();

						dto.name = item.Attribute("NAME")?.Value ?? "";
						dto.accountType = GstAccountType.DUTIES_AND_TAXES;

						dto.taxType = item.Element("TAXTYPE")?.Value ?? "";
						dto.taxRate = double.TryParse(item.Element("RATEOFTAXCALCULATION")?.Value, out double id) ? id : 0;

						dto.activated = false;
						dto.gstDutyHead = item.Element("GSTDUTYHEAD")?.Value ?? "";
						lst.Add(dto);


					}
				}
			}
			catch(Exception ex)
			{
				LogManager.WriteLog(ex.Message);

			}
			return lst;
		}
	}
	}

