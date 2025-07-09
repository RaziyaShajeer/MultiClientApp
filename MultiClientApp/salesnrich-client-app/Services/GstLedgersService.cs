using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
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
    internal class GstLedgersService
    {
        TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        GST_LedgerParse gST_LedgerParse;
		public GstLedgersService()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            gST_LedgerParse=new GST_LedgerParse();
        }
        internal async  void getFromTallyAndUpload()
        {
            try
            {
                List<GstLedgerDTO> allGstLedgerspTally = new List<GstLedgerDTO>();
                allGstLedgerspTally = await getAllGstLedgers("Duties & Taxes ");
                if (allGstLedgerspTally.Count > 0)
                {
                    upload(allGstLedgerspTally);
                }
            }catch(Exception e)
            {
				LogManager.HandleException(e);
				throw e;
			}

        }

        public async Task< List<GstLedgerDTO>>  getAllGstLedgers(String parent)
        {

			ENVELOPE tallyRequest = new ENVELOPE();
			tallyRequest= GSTLedgerGenerateXML.GstLedgerGenerateXml(parent);
			var stringwriter = new System.IO.StringWriter();
			System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyRequest.GetType());
			x.Serialize(stringwriter, tallyRequest);

			var data = await tallyCommunicator.ExecXmlAndGetXmlAsync(stringwriter.ToString());
			List<GstLedgerDTO> _list = new List<GstLedgerDTO>();
			_list =gST_LedgerParse.getAllGstLEdgers(data);
			
            return _list;
        }

        private void upload(List<GstLedgerDTO> allGstLedgerspTally)
        {
            string requestUri = ApiConstants.PREFIX + ApiConstants.GST_LEDGERS;


            LogManager.WriteLog("uploading GST_LEDGERS data to server started...");
            httpClient = RestClientUtil.getClient();
            var myContent = JsonConvert.SerializeObject(allGstLedgerspTally);
            HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

            var responseTask = httpClient.PostAsync(requestUri, inputContent);

            responseTask.Wait();

            HttpResponseMessage Res = responseTask.Result;
            LogManager.WriteResponseLog(Res);

            if (Res.IsSuccessStatusCode)
            {
                LogManager.WriteLog("request for uploading GST_LEDGERS data   Success..");
                var response = Res.Content.ReadAsStringAsync().Result;
            }
            else
            {
                LogManager.WriteLog("request for uploading GST_LEDGERS data   Failed..");
            }
        }
    }
}
