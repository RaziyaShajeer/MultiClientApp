using Newtonsoft.Json;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Parsers;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
using SNR_ClientApp.Tally.generateXml;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    internal class GroupsService
    {
        readonly TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        private bool fullUpdate = true;
        private string idClentApp;
        private String tallyLedgerParent;
		CompanyAccountGroupXml companyAccountGroupXml = new CompanyAccountGroupXml();
		public GroupsService()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
            tallyLedgerParent = ApplicationProperties.properties.GetValueOrDefault("tally.ledger.parent").ToString();
        }

        public async Task<List<LocationDTO>> getCompanyAccountGroups()
        {
			ENVELOPE tallyRequest = new ENVELOPE();

			tallyRequest = CompanygroupGenerateXml.getCompanyGroupsXml();



			var stringwriter = new System.IO.StringWriter();
			System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyRequest.GetType());
			x.Serialize(stringwriter, tallyRequest);

			var data = await tallyCommunicator.ExecXmlAndGetXmlAsync(stringwriter.ToString());
			List<LocationDTO> _list = new List<LocationDTO>();
			_list = AccountGroupResponseParser.CompanyGroupresponseParser(data);

		
       
            return _list;

        }

        internal List<LocationDTO> accountGroupsFilter(List<LocationDTO> allGroups)
        {
            List<LocationDTO> sundryChild = new List<LocationDTO>();
            List<LocationDTO> filteredGroups = new List<LocationDTO>();
           
                foreach (LocationDTO stockGroup in allGroups)
                {
                    if (stockGroup.description.Equals(tallyLedgerParent,StringComparison.OrdinalIgnoreCase))
                    {
                        sundryChild.Add(stockGroup);
                    }
                }
            LogManager.WriteLog("Groups unders sundry");
			var myContent = JsonConvert.SerializeObject(sundryChild);
			LogManager.WriteLog(myContent.ToString());

			foreach (LocationDTO sg in sundryChild)
            {
                filteredGroups.Add(sg);



            }
			LogManager.WriteLog("filteredGroup");
		 myContent = JsonConvert.SerializeObject(sundryChild);
			LogManager.WriteLog(myContent.ToString());
			List<LocationDTO> filteredAllGroups = fileList(sundryChild, filteredGroups, allGroups);
            return filteredAllGroups;
        }
        private List<LocationDTO> fileList(List<LocationDTO> sundryChild, List<LocationDTO> filteredGroups,
         List<LocationDTO> allGroups)
        {
            List<LocationDTO> sundry = new List<LocationDTO>();
            for (int i = 0; i < sundryChild.Count(); i++)
            {
                foreach (LocationDTO stockGroup in allGroups)
                {
                    if (sundryChild[i].name.Equals(stockGroup.description, StringComparison.OrdinalIgnoreCase))
                    {
                        sundry.Add(stockGroup);
                    }
                }
            }
            foreach (LocationDTO stockGroup in sundry)
            {
                filteredGroups.Add(stockGroup);
            }
            if (sundry.Count > 0)
            {
                fileList(sundry, filteredGroups, allGroups);
            }
            return filteredGroups;
        }
    }
}
