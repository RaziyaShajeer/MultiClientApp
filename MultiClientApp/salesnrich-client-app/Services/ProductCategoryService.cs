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
    internal class ProductCategoryService
    {
        TallyCommunicator tallyCommunicator = new TallyCommunicator();
        HttpClient httpClient = new HttpClient();
      
        private string idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
        internal async void getFromTallyAndUpload(bool isoptimised)
        {
            try { 
            List<ProductCategoryDTO> _list = new List<ProductCategoryDTO>();
           
            DataTable response = new DataTable();
            StringBuilder Query = new StringBuilder();
            Query.Append("select $name,$parent,$AlterID,$Guid from " + Tables.StockCategory);
            if (isoptimised)
            {
                long alterID = getAlterId();
               
                Query.Append(" where $Alterid >" + alterID);
               
            }
            response =await tallyCommunicator.getdatatable(Query.ToString());
          
            if (response.Rows.Count > 0)
            {
               
                foreach (DataRow dr in response.Rows)
                {
                    ProductCategoryDTO productCategoryDTO = new ProductCategoryDTO();
                    productCategoryDTO.productCategoryId = ((string)dr["$guid"]);
                    productCategoryDTO.activated=true;
                    // productGroupDTO.alterId = ((double)dr["$alterid"]);
                    productCategoryDTO.name = ((string)dr["$name"]);

                    productCategoryDTO.alterId = (dr["$alterid"] != DBNull.Value && (dr["$alterid"] != "") )? (StringUtilsCustom.ExtractDoubleValue(dr["$alterid"].ToString())) : 0;
                    
                    _list.Add(productCategoryDTO);
                }
               
            }
            ProductCategoryDTO productCategory = new ProductCategoryDTO();
            productCategory.name = "Not Applicable";
            productCategory.productCategoryId = "Not Applicable";
            productCategory.alterId = 0;
            productCategory.activated = true;
            _list.Add(productCategory);
            upload(_list);
			}
			catch (Exception ex)
			{
				LogManager.HandleException(ex);
				throw ex;
			}
		}

        private void upload(List<ProductCategoryDTO> list)
        {
            try
            {
                string requestUri = ApiConstants.PREFIX + ApiConstants.PRODUCT_CATEGORY;

                if (idClentApp.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    requestUri = ApiConstants.PREFIX + ApiConstants.PRODUCT_CATEGORY_ID;
                }
                LogManager.WriteLog("uploading PRODUCT_CATEGORY started...\n");

                httpClient = RestClientUtil.getClient();
                LogManager.WriteLog(httpClient.BaseAddress + requestUri);
                var myContent = JsonConvert.SerializeObject(list);
                LogManager.WriteLog("\n" + myContent);
                HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

                var responseTask = httpClient.PostAsync(requestUri, inputContent);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteResponseLog(Res);
                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("request for uploading PRODUCT_CATEGORY  Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    LogManager.WriteLog("request for uploading PRODUCT_CATEGORY  Failed..");
                    throw new ServiceException("PRODUCT_CATEGORY upload failed statuscode:" + Res.StatusCode + " Message : " + Res.RequestMessage);

                    
                }
            }
            catch(Exception ex)
            {
                LogManager.WriteLog( "Upload ProductCategory to server failed ... " + ex.Message);
                LogManager.HandleException(ex);
                throw ex;
            }
        }

        private long getAlterId()
        {

            LogManager.WriteLog("get alterId in ProductCategoryService Service started...");
            httpClient = RestClientUtil.getClient();
            var responseTask = httpClient.GetAsync(httpClient.BaseAddress + ApiConstants.PREFIX+ ApiConstants.ALTERID_MASTER + "/" + TallyMasters.PRODUCT_CATEGORY);

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
