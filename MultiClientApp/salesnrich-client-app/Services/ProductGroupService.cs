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
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Parsers;
using SNR_ClientApp.Tally.generateXml;

namespace SNR_ClientApp.Services
{
    internal class ProductGroupService
    {
        Dictionary<string, string> props = new Dictionary<string, string>();
        TallyCommunicator tallyCommunicator = new TallyCommunicator();
        HttpClient httpClient = new HttpClient();
        CompanyAccountGroupXml companyAccountGroupXml=new CompanyAccountGroupXml();
		private bool fullUpdate =true;

        private string idClentApp = ApplicationProperties.userinitialproperty.GetValueOrDefault("idclientapp").ToString();

        public ProductGroupService()
        {
            httpClient = RestClientUtil.getClient();

        }
		

		internal async  void getFromTallyAndUpload(bool isoptimised)
        {
            try
            {
				ENVELOPE tallyRequest = new ENVELOPE();

				tallyRequest = companyAccountGroupXml.getCompanyAccountGroupsXml();


				var stringwriter = new System.IO.StringWriter();
				System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyRequest.GetType());
				x.Serialize(stringwriter, tallyRequest);

				var data = await tallyCommunicator.ExecXmlAndGetXmlAsync(stringwriter.ToString());


				List<ProductGroupDTO> _list = new List<ProductGroupDTO>();
			_list= ProductGroupMasterResponseParser.ParseStockGroupListXml(data);


                

                if (_list.Count > 0)
                {
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

             
                LogManager.WriteLog("uploading PRODUCT GROUP started...");
                httpClient = RestClientUtil.getClient();
                var myContent = JsonConvert.SerializeObject(list);
                LogManager.WriteRequestContentLog(myContent, requestUri);
                HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

                var responseTask = httpClient.PostAsync(requestUri, inputContent);

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
  
