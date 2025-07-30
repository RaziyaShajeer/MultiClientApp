using Microsoft.VisualBasic.Logging;
using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Parsers;
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
    internal class TaxMasterService
    {
        TallyCommunicator tallyCommunicator ;
        HttpClient httpClient ;
        TaxMasterParser taxMasterParser;
		public TaxMasterService()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            taxMasterParser = new TaxMasterParser();
        }
        internal async void getFromTallyAndUpload()
        {
            try
            {

                ENVELOPE tallyRequest = new ENVELOPE();

                tallyRequest = TaxMasterGenerateXML.TaxMasterGenerateXml();
                var stringwriter = new System.IO.StringWriter();
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyRequest.GetType());
                x.Serialize(stringwriter, tallyRequest);

                var data = await tallyCommunicator.ExecXmlAndGetXmlAsync(stringwriter.ToString());
                List<TaxMasterDTO> _list = new List<TaxMasterDTO>();
                _list = TaxMasterParser.ParseTaxMasterListXml(data);
                var myContent = JsonConvert.SerializeObject(_list);
                LogManager.WriteLog(myContent.ToString());

				LogManager.WriteLog("TaxMaster---" + _list.Count);
				upload(_list);
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
        }
		
        

        private void upload(List<TaxMasterDTO> list)
        {
            string requestUri = ApiConstants.PREFIX + ApiConstants.TAX_MASTER;//SNR_CLIENT_APP_TM_1


			LogManager.WriteLog("uploading tax master data to server started...");
            httpClient = RestClientUtil.getClient();
            var myContent = JsonConvert.SerializeObject(list);
            HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

            var responseTask = httpClient.PostAsync(requestUri , inputContent);

            responseTask.Wait();

            HttpResponseMessage Res = responseTask.Result;
            LogManager.WriteResponseLog(Res);

            if (Res.IsSuccessStatusCode)
            {
                LogManager.WriteLog("request for uploading tax master data   Success..");
                var response = Res.Content.ReadAsStringAsync().Result;
            }
            else
            {

                LogManager.WriteLog("request for uploading tax master data   Failed..");
                throw new ServiceException("tax master upload failed statuscode:" + Res.StatusCode + " Message : " + Res.RequestMessage);

            }
        }
    }
}
