using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Exceptions;
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

namespace SNR_ClientApp.Services
{
    internal class ProductCategoryService
    {
        TallyCommunicator tallyCommunicator = new TallyCommunicator();
        HttpClient httpClient = new HttpClient();
        CompanyStockCategoryXml companyStockCategory = new CompanyStockCategoryXml();

		private string idClentApp = ApplicationProperties.userinitialproperty.GetValueOrDefault("idclientapp").ToString();
        internal async void getFromTallyAndUpload(bool isoptimised)
        {
            try
            {

                ENVELOPE tallyRequest = new ENVELOPE();

                tallyRequest = companyStockCategory.getCompanyStockCategoryXml();



                var stringwriter = new System.IO.StringWriter();
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyRequest.GetType());
                x.Serialize(stringwriter, tallyRequest);

                var data = await tallyCommunicator.ExecXmlAndGetXmlAsync(stringwriter.ToString());


                List<ProductCategoryDTO> _list = new List<ProductCategoryDTO>();
                _list = ProductCategoryRsponseParser.ParseStockCategoryListXml(data);

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
