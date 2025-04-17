using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Parsers;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    public class TemporaryOpeningStockService
    {
        TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        OpeningStockTallyMasterResponseParser openingStockTallyMasterResponseParser;
        private string idClentApp;
        OpeningStockService openingStockService;
        public TemporaryOpeningStockService()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            openingStockTallyMasterResponseParser = new OpeningStockTallyMasterResponseParser();
            idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
            openingStockService = new OpeningStockService();
        }
        public async Task<bool> getFromTallyAndUploadAsync()
        {
            bool res = false;
            try
            {
                List<NetStockDetailsDTO> stockSummaryList = new List<NetStockDetailsDTO>();
                String key = ApplicationProperties.properties["stocklocationMethode"].ToString();
                ENVELOPE tallyRequest = new ENVELOPE();
                tallyRequest = openingStockService.getCompanyStockSummaryBatchWiseXml();


                var stringwriter = new System.IO.StringWriter();
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyRequest.GetType());
                x.Serialize(stringwriter, tallyRequest);

                var data = await tallyCommunicator.ExecXmlAndGetXmlAsync(stringwriter.ToString());

              

                List<OpeningStockDTO> opstkToServer = new List<OpeningStockDTO>();
               

                if (key == null || key == "")
                {
                    OpeningStockTallyMasterResponseParser openingStockTallyMasterResponseParser = new OpeningStockTallyMasterResponseParser();
                    //opstkToServer = openingStockTallyMasterResponseParser.parseStockSummaryChangeOrderXml(data);

                }
                else
                {

                    opstkToServer = openingStockTallyMasterResponseParser.parseStockSummaryChangeOrderXml(data);

                    
                }

                HashSet<string> ppNames = new HashSet<string>(opstkToServer.Select(p => p.stockLocationName));
               

                if (ppNames.Count > 0)
                {
                    openingStockService.uploadStockLocation(openingStockService.stockLocations(ppNames));

                }
                if (opstkToServer.Count > 0)
                {
                    res = upload(opstkToServer);
                    return res;
                }
                   

                return res;
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw new ServiceException(ex.Message);
                throw ex;
            }

        }

        private bool upload(List<OpeningStockDTO> list)
        {
            try
            {
                LogManager.WriteLog("uploading Opening Stock  To Server ....");

                string requestUri = ApiConstants.PREFIX + ApiConstants.TEMPORARY_OPENING_STOCK;

              

                LogManager.WriteLog("uploading Opening Stock  started...\n" + "Api  : " + requestUri);
                httpClient = RestClientUtil.getClient();
                var myContent = JsonConvert.SerializeObject(list);
                HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

                var responseTask = httpClient.PostAsync(requestUri, inputContent);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteLog("Uploading Opening Stock  To Server Completed ....\n Response : ");
                LogManager.WriteResponseLog(Res);

                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("request for uploading Opening Stock   Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                    return true;
                }
                else
                {
                    LogManager.WriteLog("request for uploading Opening Stock   Failed..");
                    throw new ServiceException("uploading Opening Stock   Failed \n \n Response Status code : "+Res.StatusCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
            return false;
        }
    }
}
