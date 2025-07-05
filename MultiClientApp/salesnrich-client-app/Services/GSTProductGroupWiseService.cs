using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Parsers;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
using SNR_ClientApp.Tally.generateXml;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    internal class GSTProductGroupWiseService
    {
        public static readonly String FILE_NAME = "product-profile";
        TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        private bool fullUpdate = true;
        private string idClentApp;
        private String GSTNames = ApplicationProperties.properties.GetValueOrDefault("tally.gst").ToString();
        ProductGroupProductGSTxmlParser productGroupProductGSTxmlParser;
		public GSTProductGroupWiseService()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
            productGroupProductGSTxmlParser = new ProductGroupProductGSTxmlParser();    
        }
        internal async void getFromTallyAndUpload()
        {
            try {

                var tallyrequest=GroupWiseGSTMaterGenerateXML.groupwiseGstMasterGenerateXML();
				var stringwriter = new System.IO.StringWriter();
				System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyrequest.GetType());
				x.Serialize(stringwriter, tallyrequest);

				var data = await tallyCommunicator.ExecXmlAndGetXmlAsync(stringwriter.ToString());




				List<GSTProductGroupWiseDTO> pgToServer = new List<GSTProductGroupWiseDTO>();
                pgToServer=productGroupProductGSTxmlParser.parseGstProductgroupparser(data);
                LogManager.WriteLog("GroupWiseGST");
				var myContent = JsonConvert.SerializeObject(pgToServer);
				LogManager.WriteLog(myContent.ToString());

				//skip group having 0 taxes,if we need tax by item wise must be added to another product group that does'nt have any tax rate




				if (pgToServer.Count>0)
                upload(pgToServer);
			}
			catch (Exception ex)
			{
				LogManager.HandleException(ex);
				throw ex;
			}
		}

        private void upload(List<GSTProductGroupWiseDTO> pgToServer)
        {
            try
            {
                string requestUri = ApiConstants.PREFIX + ApiConstants.GST_GROUP_WISE;//SNR_CLIENT_APP_PGT_1

				if (idClentApp.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    requestUri = ApiConstants.PREFIX + ApiConstants.GST_GROUP_WISE_ID;//SNR_CLIENT_APP_PGT_2
				}
                LogManager.WriteLog("uploading GST_GROUP_WISE started...");
                httpClient = RestClientUtil.getClient();
                var myContent = JsonConvert.SerializeObject(pgToServer);
                HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

                var responseTask = httpClient.PostAsync(requestUri, inputContent);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteResponseLog(Res);

                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("request for uploading GST_GROUP_WISE  Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    LogManager.WriteLog("request for uploading GST_GROUP_WISE  Failed..");
                    throw new ServiceException("GST_GROUP_WISE upload failed statuscode:" + Res.StatusCode + " Message : " + Res.RequestMessage);

                }
            }catch(Exception ex)
            {
                LogManager.HandleException(ex, "GST_GROUP_WISE upload failed due to unexpected exception while uploading datas to server");
                throw;
            }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        }
    }
}
