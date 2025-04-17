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
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    internal class LocationService
    {
        readonly TallyCommunicator tallyCommunicator;
         HttpClient httpClient ;
        private bool fullUpdate = true;
        private string idClentApp;
        private String tallyLedgerParent;
        public LocationService()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
            tallyLedgerParent= ApplicationProperties.properties.GetValueOrDefault("tally.ledger.parent").ToString();
        }
        internal async  void getFromTallyAndUpload(bool isOptimise)
        {
            try { 
            List<LocationDTO> _list = new List<LocationDTO>();
            DataTable response = new DataTable();
            StringBuilder Query = new StringBuilder();
            Query.Append("SELECT $name,$alterid,$guid,$parent FROM " + Tables.Groups);
            if (isOptimise)
            {
                long alterID = getAlterId();

                Query.Append(" where $Alterid >" + alterID);
                fullUpdate = false;
                //Query = "SELECT $name,$alterid,$guid FROM " + Tables.StockGroup + " where $Alterid >" + alterID;
                //response = tallyCommunicator.getdatatable("SELECT $name,$alterid,$guid FROM " + Tables.StockGroup + " where $Alterid >"+alterID);
            }
            response = await tallyCommunicator.getdatatable(Query.ToString());

            if (response.Rows.Count > 0)
            {

                foreach (DataRow dr in response.Rows)
                {
                    LocationDTO locationDto = new LocationDTO();
                    locationDto.locationId = ((string)dr["$guid"]);
                    locationDto.name = (dr["$name"] != DBNull.Value) ? (string)dr["$name"] : "";
                    locationDto.description = (dr["$parent"] != DBNull.Value) ? (string)dr["$parent"] : "";
                  
                    locationDto.alterId = (dr["$alterid"] != DBNull.Value) ? (long.Parse(dr["$alterid"].ToString())) : 0;
                    
                   

                    _list.Add(locationDto);

                }

                List<LocationDTO> locationToServer = SundryDebterUnderLocaions(_list);
                if (locationToServer.Count != 0)
                {
                    LocationDTO locationDTO = new LocationDTO();
                    locationDTO.alterId = 0;
                    locationDTO.name = "Territory";
                    locationToServer.Add(locationDTO);
                    upload(locationToServer);
                }
  
               
            }
			}
			catch (Exception ex)
			{
				LogManager.HandleException(ex);
				throw ex;
			}
		}

        private void upload(List<LocationDTO> locationToServer)
        {
            string requestUri = ApiConstants.PREFIX + ApiConstants.LOCATION;//SNR_CLIENT_APP_L_1

			if (idClentApp.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                requestUri = ApiConstants.PREFIX + ApiConstants.LOCATION_ID;//SNR_CLIENT_APP_L_2
			}
            LogManager.WriteLog("uploading LOCATION started...");
            httpClient = RestClientUtil.getClient();
            var myContent = JsonConvert.SerializeObject(locationToServer);
            HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

            var responseTask = httpClient.PostAsync(requestUri + "/" + fullUpdate, inputContent);

            responseTask.Wait();

            HttpResponseMessage Res = responseTask.Result;
            LogManager.WriteResponseLog(Res);

            if (Res.IsSuccessStatusCode)
            {
                LogManager.WriteLog("request for uploading LOCATION Success..");
                var response = Res.Content.ReadAsStringAsync().Result;
            }
            else
            {
                LogManager.WriteLog("request for uploading LOCATION Failed..");
                throw new ServiceException("LOCATION upload failed statuscode:" + Res.StatusCode + " Message : " + Res.RequestMessage);


            }
        }

        private List<LocationDTO> SundryDebterUnderLocaions(List<LocationDTO> locationDTOs)
        {
            List<LocationDTO> sundryChilds = new List<LocationDTO>();
            List<LocationDTO> filterdGroups = new List<LocationDTO>();

            locationDTOs.ForEach((locationDTO) =>
            {
                if (locationDTO.description.Equals(tallyLedgerParent, StringComparison.OrdinalIgnoreCase))
                {
                    sundryChilds.Add(locationDTO);
                }
                if (locationDTO.name.Equals(tallyLedgerParent, StringComparison.OrdinalIgnoreCase))
                {
                    locationDTO.description = "Territory";
                    sundryChilds.Add(locationDTO);
                }
            });
            filterdGroups.AddRange(sundryChilds);

            List<LocationDTO> filteredAllLocations = fileList(sundryChilds, filterdGroups, locationDTOs);
            foreach (LocationDTO locationDTO in locationDTOs)
            {
                if (locationDTO.description.Equals(tallyLedgerParent, StringComparison.OrdinalIgnoreCase))
                {
                    filteredAllLocations.Add(locationDTO);
                }
            }

            // avoid duplicate entry in locations
            HashSet<LocationDTO> duplicateAvoidLocations = new HashSet<LocationDTO>();
            foreach (LocationDTO location in filteredAllLocations)
            {
                duplicateAvoidLocations.Add(location);
            }
            return new List<LocationDTO>(duplicateAvoidLocations);
        }

        private List<LocationDTO> fileList(List<LocationDTO> sundryChilds, List<LocationDTO> filterdGroups, List<LocationDTO> locationDTOs)
        {
            List<LocationDTO> sundry = new List<LocationDTO>();
            for (int i = 0; i < sundryChilds.Count; i++)
            {
                foreach (LocationDTO stockGroup in locationDTOs)
                {
                    if (sundryChilds[i].name.Equals(stockGroup.description, StringComparison.OrdinalIgnoreCase))
                    {
                        sundry.Add(stockGroup);
                    }
                }
            }
            foreach (LocationDTO stockGroup in sundry)
            {
                filterdGroups.Add(stockGroup);
            }
            if (sundry.Count > 0)
            {
                fileList(sundry, filterdGroups, locationDTOs);
            }
            return filterdGroups;
        }

        private long getAlterId()
        {

            LogManager.WriteLog("get alterId in LocationService started...");
            httpClient = RestClientUtil.getClient();
            var responseTask = httpClient.GetAsync(httpClient.BaseAddress+ ApiConstants.PREFIX+ ApiConstants.ALTERID_MASTER + "/" + TallyMasters.LOCATION);

            responseTask.Wait();

            HttpResponseMessage Res = responseTask.Result;
            LogManager.WriteResponseLog(Res);
            if (Res.IsSuccessStatusCode)
            {
                LogManager.WriteLog("request for alterId  Success..");
                var response = Res.Content.ReadAsStringAsync().Result;
                long responseAlterID = JsonConvert.DeserializeObject<long>(response);
                return responseAlterID;
            }
            throw new ServiceException("get alterId api call failed \n StatusCode : " + Res.StatusCode + " \n Response : " + Res.Content);

        }
    }
}
