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
    public class ProductGroupProductProfileService
    {
        public static readonly String FILE_NAME = "product-group-product-profile";
        TallyCommunicator tallyCommunicator = new TallyCommunicator();
        HttpClient httpClient = new HttpClient();

        private string idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
        internal async void getFromTallyAndUpload()
        {
            try
            {
                List<TPProductGroupProductDTO> _list = new List<TPProductGroupProductDTO>();

                DataTable response = new DataTable();
                StringBuilder Query = new StringBuilder();
                Query.Append("select $name,$_FirstAlias,$Parent,$Guid,$Category,$RateOfMrp,$ModifyMRPRate,$OpeningRate," +
                    "$RateOfVat,$BaseUnits,$AdditionalUnits,$Conversion,$Denominator,$IsBatchWiseOn,$_HSNCode,$Description," +
                    "$Narration,$AlterID from " + Tables.StockItem);
                long alterid = getAlterIdFromServer();
                if (alterid > 0)
                {
                    Query.Append(" where $Alterid >" + alterid);
                }

                response = await tallyCommunicator.getdatatable(Query.ToString());

                if (response.Rows.Count > 0)
                {

                    foreach (DataRow dr in response.Rows)
                    {
                        TPProductGroupProductDTO tpProductGroupProductDTO = new TPProductGroupProductDTO();
                        //Todo : check this line , make sure productId is Guid of stock item
                        tpProductGroupProductDTO.productId = (dr["$Guid"] != DBNull.Value) ? (string)dr["$Guid"] : "";
						if (ApplicationProperties.properties["IsEnableDistributor"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
							 var name=(dr["$parent"] != DBNull.Value) ? (string)dr["$parent"] : "";
							tpProductGroupProductDTO.groupName =DistributedCodeAppend.appendDistributedCode(name);

                        }
                        else
                        {
							tpProductGroupProductDTO.groupName = (dr["$parent"] != DBNull.Value) ? (string)dr["$parent"] : "";
						}
							
                        tpProductGroupProductDTO.productName = (dr["$name"] != DBNull.Value) ? (string)dr["$name"] : "";
                        tpProductGroupProductDTO.alterId = (dr["$AlterID"] != DBNull.Value) ? long.Parse(dr["$AlterID"].ToString()) : 0;


                        _list.Add(tpProductGroupProductDTO);
                    }
					//List<TPProductGroupProductDTO> updatedList = getAlterIdFromServer(_list);
					var myContent = JsonConvert.SerializeObject(_list);
                    LogManager.WriteLog(myContent.ToString());
					if (_list.Count>0)
						

					
                        upload(_list);
                }
			}
			catch (Exception ex)
			{
				LogManager.HandleException(ex);
				throw ex;
			}
		}

        private void upload(List<TPProductGroupProductDTO> list)
        {
            string requestUri = ApiConstants.PREFIX + ApiConstants.PRODUCTGROUP_PRODUCTPROFILE;

            if (idClentApp.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                requestUri = ApiConstants.PREFIX + ApiConstants.PRODUCTGROUP_PRODUCTPROFILE_ID;
            }
            LogManager.WriteLog("uploading PRODUCTGROUP_PRODUCTPROFILE started...");
            httpClient = RestClientUtil.getClient();
            var myContent = JsonConvert.SerializeObject(list);
            LogManager.WriteLog("requestUri :"+ requestUri);
            LogManager.WriteLog("Data : "+myContent);
            HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

            var responseTask = httpClient.PostAsync(requestUri , inputContent);

            responseTask.Wait();

            HttpResponseMessage Res = responseTask.Result;
            LogManager.WriteResponseLog(Res);

            if (Res.IsSuccessStatusCode)
            {
                LogManager.WriteLog("request for uploading PRODUCTGROUP_PRODUCTPROFILE  Success..");
                var response = Res.Content.ReadAsStringAsync().Result;
            }
            else
            {
                LogManager.WriteLog("request for uploading PRODUCTGROUP_PRODUCTPROFILE  Failed..");
                throw new ServiceException("PRODUCTGROUP_PRODUCTPROFILE upload failed statuscode:" + Res.StatusCode + " Message : " + Res.RequestMessage);

            }
        }
        private long getAlterIdFromServer()
        {
            LogManager.WriteLog("get alterId in ProductGroupProductProfileService started...");
            httpClient = RestClientUtil.getClient();
            var responseTask = httpClient.GetAsync(httpClient.BaseAddress + ApiConstants.PREFIX+ ApiConstants.ALTERID_MASTER + "/" + TallyMasters.PRODUCTGROUP_PRODUCTPROFILE);

            responseTask.Wait();

            HttpResponseMessage Res = responseTask.Result;
            LogManager.WriteResponseLog(Res);
            if (Res.IsSuccessStatusCode)
            {
                LogManager.WriteLog("request for alterId  Success..");
                var response = Res.Content.ReadAsStringAsync().Result;
                long responseAlterID = JsonConvert.DeserializeObject<long>(response);
                //  return tPProductGroupProductDTOs.Where((dto=>dto.alterId>responseAlterID)).ToList(); 
                return responseAlterID;
            }
            throw new ServiceException("get alterId api call failed \n StatusCode : " + Res.StatusCode + " \n Response : " + Res.Content);

        }
    }
}
