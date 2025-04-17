using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Tally;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Windows.Forms;
using System.Xml.Linq;
using SNR_ClientApp.Properties;

namespace SNR_ClientApp.Services
{
    internal class ProductGroupService
    {
        Dictionary<string, string> props = new Dictionary<string, string>();
        TallyCommunicator tallyCommunicator = new TallyCommunicator();
        HttpClient httpClient = new HttpClient();
        private bool fullUpdate =true;
        private string idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();

        public ProductGroupService()
        {
            httpClient = RestClientUtil.getClient();
        }
        internal async  void getFromTallyAndUpload(bool isoptimised)
        {
            try
            {
                List<ProductGroupDTO> _list = new List<ProductGroupDTO>();
                DataTable response = new DataTable();
                StringBuilder Query = new StringBuilder();
                Query.Append("SELECT $name,$alterid,$guid,$RateOfVat FROM " + Tables.StockGroup);
                if (isoptimised)
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
                      
                        var productGroupId = ((string)dr["$guid"]);
                        // productGroupDTO.alterId = ((double)dr["$alterid"]);
                        var name= ((string)dr["$name"]);

                        var s = dr["$RateOfVat"];
                     var taxRate = (s != DBNull.Value && s!="") ? (StringUtilsCustom.ExtractDoubleValue(dr["$RateOfVat"].ToString())) : 0;
                        var alterId = (dr["$Alterid"] != DBNull.Value  && dr["$Alterid"]!="") ? ((StringUtilsCustom.ExtractDoubleValue(dr["$alterid"].ToString()))) : 0;
						ProductGroupDTO productGroupDTO = new ProductGroupDTO(productGroupId,name, taxRate,alterId);
						_list.Add(productGroupDTO);

                    }
					var myContent = JsonConvert.SerializeObject(_list);
                    LogManager.WriteLog(myContent.ToString());
					upload(_list);
                }
			}
			catch (Exception ex)
			{
				LogManager.HandleException(ex);
				throw ex;
			}

		}

       

        private long getAlterId()
        {
           
                LogManager.WriteLog("get alterId in ProductGroupService Service started...");
            httpClient = RestClientUtil.getClient();
            var responseTask = httpClient.GetAsync(httpClient.BaseAddress+ApiConstants.PREFIX+ApiConstants.ALTERID_MASTER + "/" + TallyMasters.PRODUCT_GROUP);

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
            throw new ServiceException("get alterId api call failed \n StatusCode : "+Res.StatusCode +" \n Response : " + Res.Content);
        }

        private void upload(List<ProductGroupDTO> list)
        {
            try
            {
                string requestUri = ApiConstants.PREFIX + ApiConstants.PRODUCT_GROUP;

                if (idClentApp.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    requestUri = ApiConstants.PREFIX + ApiConstants.PRODUCT_GROUP_ID;
                }
                LogManager.WriteLog("uploading PRODUCT GROUP started...");
                httpClient = RestClientUtil.getClient();
                var myContent = JsonConvert.SerializeObject(list);
                LogManager.WriteRequestContentLog(myContent, requestUri);
                HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

                var responseTask = httpClient.PostAsync(requestUri+ "?fullUpdate="+fullUpdate, inputContent);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteResponseLog(Res);

                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("request for uploading PRODUCT GROUP  Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    LogManager.WriteLog("request for uploading PRODUCT GROUP  Failed..");
                    throw new ServiceException("PRODUCT GROUP upload failed statuscode:" + Res.StatusCode + " Message : " + Res.RequestMessage);

                }
                //restTemplate.postForObject(apiUrl + ApiConstants.PRODUCT_GROUP_ID, entity, String.class,fullUpdate);
            }catch(Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
        }
            
    }


}
  
