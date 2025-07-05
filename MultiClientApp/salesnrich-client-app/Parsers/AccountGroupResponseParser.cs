using dtos;
using Newtonsoft.Json;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace SNR_ClientApp.Parsers
{
	public static class AccountGroupResponseParser
	{
		public static List<LocationDTO> CompanyGroupresponseParser(string tallyResponseXml) 
			
		
		{
			List<LocationDTO> locationDtos = new List<LocationDTO>();
			//tallyResponseXml = tallyResponseXml.Replace("\u0004","");
			tallyResponseXml = tallyResponseXml.Replace("&#4;", " ");
		


			tallyResponseXml = tallyResponseXml.Replace("&apos;", "'");
			tallyResponseXml = tallyResponseXml.Replace("&","");


			XmlDocument doc = new XmlDocument();

			doc.LoadXml(tallyResponseXml);

			XmlNodeList GroupNodes = doc.GetElementsByTagName("GROUPS");
			foreach(XmlNode node in GroupNodes)
			{
				string pattern = @"(&#13;&#10;|&#13;|&#10;|[\r\n\t])+";
				


				string name = node["NAME"]?.InnerText ?? "";
				name = Regex.Replace(name, pattern, "").Trim();
				name = CleanEncodingIssues(name);
				string locationId = node["GUID"]?.InnerText?.Trim() ?? "";
				string discription = node["PARENT"]?.InnerText?? "";
				//discription = CleanEncodingIssues(discription);
				string alterIdStr = node["ALTERID"]?.InnerText?.Trim() ?? "0";
				double alterId = double.TryParse(alterIdStr, out double tempAlterId) ? tempAlterId : 0.0;

				LocationDTO dto = new LocationDTO
				{
					locationId = locationId,
					name = name,
					description = discription, // Assuming no tax rate in XML, default to 0
					alterId =Convert.ToInt64(alterId)
				};

				locationDtos.Add(dto);
			}
			LogManager.WriteLog("gROUPS");
			var myContent = JsonConvert.SerializeObject(locationDtos);
			LogManager.WriteLog(myContent.ToString());
			return locationDtos;	


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
		public static string RemoveInvalidCharacters(string text)
		{
			if (string.IsNullOrEmpty(text))
				return text;

			return new string(text
				.Where(c => c != '\uFFFD' && !char.IsControl(c)) // Remove replacement character and control characters
				.ToArray());
		}
	}
}
