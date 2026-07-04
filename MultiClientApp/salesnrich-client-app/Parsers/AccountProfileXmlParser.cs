using dtos;
using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Services;
using SNR_ClientApp.Tally;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SNR_ClientApp.Parsers
{
	public class AccountProfileXmlParser
	{
		LocationService locationService = new LocationService();
		public async Task<List<AccountProfileDTO>> AccountProfilexmlParser(string tallyResponseXml)
		{
			List<AccountProfileDTO> allAccountProfilespTally = new List<AccountProfileDTO>();
			try
			{
				List<AccountProfileDTO> duplicatelist = new List<AccountProfileDTO>();


				tallyResponseXml = tallyResponseXml.Replace("&#13;", "")
							 .Replace("&#10;", "")
							 .Replace("&#4;", "");

				var doc = XDocument.Parse(tallyResponseXml);
				var ledgerList = doc.Descendants("LEDGER");
				int ledgerCount = ledgerList.Count();
				var count = 0;

				// Log the count
				LogManager.WriteLog("Number of LEDGER nodes: " + ledgerCount);

				
				foreach (var node in ledgerList)
				{
					AccountProfileDTO accountProfileDTO = new AccountProfileDTO();
					

					accountProfileDTO.customerId = node.Element("GUID")?.Value ?? "";
				if(!string.IsNullOrEmpty(accountProfileDTO.customerId))
					{
						accountProfileDTO.customerId = accountProfileDTO.customerId.Replace("\\s", "");
					}
				
					accountProfileDTO.name = node.Attribute("NAME")?.Value ?? "";


					//string pattern = @"(&#13;&#10;|&#13;|&#10;|[\r\n\t])+";
					if (accountProfileDTO.name.EndsWith("\r\n"))
					{
						accountProfileDTO.name = accountProfileDTO.name.Replace("\r\n", "");
						accountProfileDTO.trimChar = "#13;#10;";
					}

					// 13/10/2023
					// todo : need to verify with download order
					string pattern = @"[\r\n\t]+$";
					string result = Regex.Replace(accountProfileDTO.name, pattern, "");

					accountProfileDTO.name = result;


				
					LogManager.WriteLog(accountProfileDTO.name.ToString());
					
					var mailingNameList = node.Element("MAILINGNAME.LIST")?.Elements("MAILINGNAME").ToList();
					if (mailingNameList != null && mailingNameList.Any())
					{
						accountProfileDTO.mailingName = string.Join("~", mailingNameList.Select(a => a.Value.Trim()));
					}
					else
					{
						accountProfileDTO.mailingName = "";
					}
						
					if (accountProfileDTO.mailingName.EndsWith("\r\n"))
					{
						accountProfileDTO.mailingName = accountProfileDTO.mailingName.Replace("\r\n", "");
						//accountProfileDTO.trimChar = "#13;#10;";
					}
				
					accountProfileDTO.description = node.Element("PARENT")?.Value ?? "";
					accountProfileDTO.description=CleanEncodingIssues(accountProfileDTO.description);
					string alterIdStr = node.Element("ALTERID")?.Value ?? "";
					double alterId = double.TryParse(alterIdStr, out double tempAlterId) ? tempAlterId : 0.0;
					accountProfileDTO.alterId = Convert.ToInt64(alterId);

					var addressList = node.Element("ADDRESS.LIST")?.Elements("ADDRESS").ToList();
					if (addressList != null && addressList.Any())
					{
						// Join all address lines with "~" separator
						accountProfileDTO.address = string.Join("~", addressList.Select(a => a.Value.Trim()));
					}
					else
					{
						accountProfileDTO.address = "No Address";
					}
					//accountProfileDTO.address = node.Element("_ADDRESS1")?.Value ?? "No Adddress";

					//accountProfileDTO.address += string.IsNullOrWhiteSpace(node.Element("_ADDRESS2")?.Value) ? "" : "~" + node.Element("_ADDRESS2")?.Value;

					accountProfileDTO.defaultPriceLevelName = node.Element("PARENT")?.Value ?? "";
					accountProfileDTO.accountTypeName = node.Element("TAXTYPE")?.Value ?? "";
					accountProfileDTO.phone1 = node.Element("LEDGERMOBILE")?.Value ?? "";
					var phone = accountProfileDTO.phone1.Split(",");
					accountProfileDTO.phone1 = phone[0];
					accountProfileDTO.defaultPriceLevelName= node.Element("PRICELEVEL")?.Value ?? "";
					accountProfileDTO.stateName = node.Element("LEDSTATENAME")?.Value ?? "";
					accountProfileDTO.countryName = node.Element("COUNTRYOFRESIDENCE")?.Value??"";
					accountProfileDTO.gstRegistrationType = node.Element("GSTREGISTRATIONTYPE")?.Value ?? "Regular";
					accountProfileDTO.tinNo = node.Element("PARTYGSTIN")?.Value ?? "";
					
					accountProfileDTO.pin = node.Element("PINCODE")?.Value?.Trim() ?? "";
					if(!string.IsNullOrEmpty(accountProfileDTO.pid))
					{
						accountProfileDTO.pin = accountProfileDTO.pin.Replace("\\s", "");
					}
					var cityValue = node.Element("HASEDDCITY")?.Value?.Trim();
					accountProfileDTO.city = string.IsNullOrWhiteSpace(cityValue) ? "No City" : cityValue;

					if ( !allAccountProfilespTally.Any(x => x.name == accountProfileDTO.name)&& accountProfileDTO.name!="")
					{

						allAccountProfilespTally.Add(accountProfileDTO);
					}
					else
					{
						duplicatelist.Add(accountProfileDTO);
					}

				}
				LogManager.WriteLog(count.ToString());

				//fileManagerService.writeObjectToFile(apTally, FILE_NAME);


			}
			catch (Exception ex)
			{
				LogManager.HandleException(ex);
				throw ex;
			}
			return allAccountProfilespTally;
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
		public async Task<List<LocationAccountProfileDTO>> LocationWiseAccounctProfileParser(string tallyResponseXml)
		{
			// Clean up the XML - Remove control characters but preserve HTML entities
			
			tallyResponseXml = tallyResponseXml.Replace("&#4;", " ");
			tallyResponseXml = tallyResponseXml.Replace("&apos;", "'");

			// DON'T remove & here - it's needed for HTML entities like &apos;

			var doc = XDocument.Parse(tallyResponseXml);
			var ledgerList = doc.Descendants("LEDGER");

			List<LocationAccountProfileDTO> allAccountProfilespTally = new List<LocationAccountProfileDTO>();
			
							  
		
			foreach (var node in ledgerList)
			{
				LocationAccountProfileDTO ladto = new LocationAccountProfileDTO();
				ladto.locationName = node.Element("PARENT")?.Value ?? "";
				ladto.locationName=CleanEncodingIssues(ladto.locationName);
				ladto.accountProfileName = node.Attribute("NAME")?.Value ?? "";

			
				string pattern = @"(&#13;&#10;|&#13;|&#10;|[\r\n\t])+";
				ladto.accountProfileName = Regex.Replace(ladto.accountProfileName, pattern, "").Trim();




				if (ladto.accountProfileName.EndsWith("\r\n"))
				{
					ladto.accountProfileName = ladto.accountProfileName.Replace("\r\n", "");

				}

				string result = Regex.Replace(ladto.accountProfileName, pattern, "");


				ladto.accountProfileName = result;
				string alterIdStr = node.Element("ALTERID")?.Value ?? "";
				double alterId = double.TryParse(alterIdStr, out double tempAlterId) ? tempAlterId : 0.0;
				ladto.alterId = Convert.ToInt64(alterId);
				ladto.customer_id = node.Element("GUID")?.Value ?? "";
				allAccountProfilespTally.Add(ladto);
			}

			return allAccountProfilespTally;


		}



		}
}

