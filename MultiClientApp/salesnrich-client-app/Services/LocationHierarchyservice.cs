using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    public class LocationHierarchyservice
    {
        readonly TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        private bool fullUpdate = true;
        private string idClentApp;
        private String tallyLedgerParent;
        public LocationHierarchyservice()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
            tallyLedgerParent = ApplicationProperties.properties.GetValueOrDefault("tally.ledger.parent").ToString();
        }
        internal async Task  getFromTallyAndUpload()
        {
            try
            {
                List<LocationDTO> _list = new List<LocationDTO>();
                DataTable response = new DataTable();
                StringBuilder Query = new StringBuilder();
                Query.Append("select $Name,$Parent,$Guid,$AlterID from  " + Tables.Groups);

                response = await tallyCommunicator.getdatatable(Query.ToString());

                if (response.Rows.Count > 0)
                {


                    foreach (DataRow dr in response.Rows)
                    {
                        LocationDTO item = new LocationDTO();
                        item.locationId = ((string)dr["$guid"]);
                        item.name = (dr["$name"] != DBNull.Value) ? (string)dr["$name"] : "";
                        item.description = (dr["$parent"] != DBNull.Value) ? (string)dr["$parent"] : "";
                        item.activated = true;

                        item.alterId = (dr["$alterid"] != DBNull.Value) ? (long.Parse(dr["$alterid"].ToString())) : 0;


                        _list.Add(item);

                    }

                    List<LocationDTO> locationToServer = SundryDebterUnderLocaions(_list);

                    locationToServer.Add(new LocationDTO("Territory", null));
                    locationToServer.Add(new LocationDTO("Primary", "Territory"));

                    // creating location-hierarchy
                    List<LocationHierarchyDTO> locationHierarchyDTOs = new List<LocationHierarchyDTO>();
                    foreach (LocationDTO location in locationToServer)
                    {
                        LocationHierarchyDTO locationHierarchyDTO = new LocationHierarchyDTO(location);
                        locationHierarchyDTOs.Add(locationHierarchyDTO);
                    }
                    if (locationHierarchyDTOs.Count() > 0)
                    {
                        upload(locationHierarchyDTOs);
                    }



                    //fileManagerService.writeObjectToFile(apTally, FILE_NAME);
                }
            }catch(Exception ex)
            {
                LogManager.HandleException(ex, "Exception Occured while Uploading Location Hierarchy");
                throw ex;
            }
        }

        private List<LocationDTO> SundryDebterUnderLocaions(List<LocationDTO> locationDTOs)
        {
            List<LocationDTO> sundryChilds = new List<LocationDTO>();
            List<LocationDTO> filteredGroups = new List<LocationDTO>();

            foreach (LocationDTO locationDTO in locationDTOs)
            {
                if (locationDTO.description.Equals(tallyLedgerParent, StringComparison.OrdinalIgnoreCase))
                {
                    sundryChilds.Add(locationDTO);
                }
                if (locationDTO.name.Equals(tallyLedgerParent, StringComparison.OrdinalIgnoreCase))
                {
                    locationDTO.description = "Primary";
                    sundryChilds.Add(locationDTO);
                }
            }
            filteredGroups.AddRange(sundryChilds);
            List<LocationDTO> filteredAllLocations = FileList(sundryChilds, filteredGroups, locationDTOs);

            foreach (LocationDTO locationDTO in locationDTOs)
            {
                if (locationDTO.description.Equals(tallyLedgerParent, StringComparison.OrdinalIgnoreCase))
                {
                    filteredAllLocations.Add(locationDTO);
                }
            }

            // avoid duplicate entry in locations
            HashSet<LocationDTO> duplicateAvoidLocations = new HashSet<LocationDTO>(filteredAllLocations);
            return new List<LocationDTO>(duplicateAvoidLocations);
        }

        private void upload(List<LocationHierarchyDTO> list)
        {
            try
            {

                string requestUri = ApiConstants.PREFIX + ApiConstants.LOCATION_HIERARCHY;


                LogManager.WriteLog("uploading LOCATION_HIERARCHY started...");
                httpClient = RestClientUtil.getClient();
                var myContent = JsonConvert.SerializeObject(list);
                HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

                var responseTask = httpClient.PostAsync(requestUri, inputContent);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteResponseLog(Res);

                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("request for uploading LOCATION_HIERARCHY  Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    LogManager.WriteLog("request for uploading LOCATION_HIERARCHY  Failed..");
                    throw new ServiceException("LOCATION_HIERARCHY upload failed statuscode:" + Res.StatusCode + " Message : " + Res.RequestMessage);

                }
            }
            catch(Exception ex)
            {
                LogManager.WriteLog("request for uploading LOCATION_HIERARCHY  Failed due to unexpected Exception while Uploading datas t server..");
                LogManager.HandleException(ex, "request for uploading LOCATION_HIERARCHY  Failed due to unexpected Exception while Uploading datas t server..");
                throw ex;
            }

        }

        private List<LocationDTO> FileList(List<LocationDTO> sundryChilds, List<LocationDTO> filteredGroups, List<LocationDTO> locationDTOs)
        {
            List<LocationDTO> sundry = new List<LocationDTO>();
            for (int i = 0; i < sundryChilds.Count; i++)
            {
                foreach (LocationDTO stockGroup in locationDTOs)
                {
                    if (sundryChilds[i].name == stockGroup.description)
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
                FileList(sundry, filteredGroups, locationDTOs);
            }
            return filteredGroups;
        }
    }
}
