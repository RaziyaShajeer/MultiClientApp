using SNR_ClientApp.Enums;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
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
        public GroupsService()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
            tallyLedgerParent = ApplicationProperties.properties.GetValueOrDefault("tally.ledger.parent").ToString();
        }

        public async Task<List<LocationDTO>> getCompanyAccountGroups()
        {
            List<LocationDTO> _list = new List<LocationDTO>();
            DataTable response = new DataTable();
            StringBuilder Query = new StringBuilder();
            Query.Append("select $guid,$name,$alterid,$Parent from " + Tables.Groups);

            response = await tallyCommunicator.getdatatable(Query.ToString());

            if (response.Rows.Count > 0)
            {


                foreach (DataRow dr in response.Rows)
                {
                    LocationDTO locationDTO = new LocationDTO();
                    locationDTO.locationId = ((string)dr["$guid"]);
                    locationDTO.name = (dr["$name"] != DBNull.Value) ? (string)dr["$name"] : "";
                    locationDTO.description = (dr["$parent"] != DBNull.Value) ? (string)dr["$parent"] : "";

                    locationDTO.alterId = (dr["$alterid"] != DBNull.Value) ? (long.Parse(dr["$alterid"].ToString())) : 0;

                    _list.Add(locationDTO);

                }

                // List<LocationDTO> filteredGroups = accountGroupsFilter(_list);
            }
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
           
            foreach (LocationDTO sg in sundryChild)
            {
                filteredGroups.Add(sg);
            }
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
