using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
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
    internal class GstLedgersService
    {
        TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        public GstLedgersService()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
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
            List<GstLedgerDTO> allGstLedgerspTally = new List<GstLedgerDTO>();
            DataTable response = new DataTable();
            StringBuilder Query = new StringBuilder();
			Query.Append("SELECT $name, $parent, $TAXTYPE, $SUBTAXTYPE, $RATEOFTAXCALCULATION, $Guid, $GSTDUTYHEAD FROM "
			  + Tables.Ledger + " WHERE $parent = '" + parent + "'");

			response = await  tallyCommunicator.getdatatable(Query.ToString());


            //StringBuilder Query2 = new StringBuilder();
            //Query2.Append("select $name from " + Tables.Groups + " where $parent = "+parent);
            //DataTable innergroups= tallyCommunicator.getdatatable(Query2.ToString());
            //if(innergroups.Rows.Count > 0)
            //{

            //}

            if (response.Rows.Count > 0)
            {
                foreach (DataRow dr in response.Rows)
                {
                    string taxtype = (dr["$TAXTYPE"] != DBNull.Value) ? (string)dr["$TAXTYPE"] : ""; 
                    if (taxtype.Equals("GST", StringComparison.OrdinalIgnoreCase))
                    {

                        GstLedgerDTO dto = new GstLedgerDTO();

                        dto.name = (dr["$name"] != DBNull.Value) ? (string)dr["$name"] : "";
                        dto.accountType = GstAccountType.DUTIES_AND_TAXES;
                        dto.taxType = (dr["$TAXTYPE"] != DBNull.Value) ? (string)dr["$TAXTYPE"] : "";
                        dto.taxRate = (dr["$RATEOFTAXCALCULATION"] != DBNull.Value) ? StringUtilsCustom.ExtractDoubleValue(dr["$RATEOFTAXCALCULATION"].ToString()) : 0;
                        dto.activated = false;
                        dto.gstDutyHead = (dr["$GSTDUTYHEAD"] != DBNull.Value) ? (string)dr["$GSTDUTYHEAD"] : "";
                        allGstLedgerspTally.Add(dto);

                    }
                }


               
                       }
            return allGstLedgerspTally;
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
