using Microsoft.VisualBasic.Logging;
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
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    internal class TaxMasterService
    {
        TallyCommunicator tallyCommunicator ;
        HttpClient httpClient ;

        public TaxMasterService()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
        }
        internal async void getFromTallyAndUpload()
        {
            try { 
            List<TaxMasterDTO> taxMasterDTOs = new List<TaxMasterDTO>();

            DataTable response = new DataTable();
            StringBuilder Query = new StringBuilder();
            Query.Append("select $name,$parent,$Address,$TAXTYPE,$SUBTAXTYPE,$TAXCLASSIFICATIONNAME,$VATCLASSIFICATIONRATE,$PRICELEVEL,$AlterID,$Guid from " + Tables.Ledger + " where  $Parent = 'Duties & Taxes' OR $Parent = 'GL 13; Duties & Taxes'");
           
            response = await tallyCommunicator.getdatatable(Query.ToString());

            if (response.Rows.Count > 0)
            {
                //TaxMasterDTO taxMasterDTO = new TaxMasterDTO();
                foreach (DataRow dr in response.Rows)
                {
                    string taxtype= (dr["$TAXTYPE"] != DBNull.Value) ? (string)dr["$TAXTYPE"] : "";
                    if (taxtype.Equals("GST"))
                    {
                        TaxMasterDTO taxMasterDTO = new TaxMasterDTO();

                        taxMasterDTO.vatName = (dr["$name"] != DBNull.Value) ? (string)dr["$name"] : "";
                        taxMasterDTO.vatClass = (dr["$TAXCLASSIFICATIONNAME"] != DBNull.Value) ? (string)dr["$TAXCLASSIFICATIONNAME"] : "";
                        taxMasterDTO.alterId = (dr["$alterid"] != DBNull.Value) ? (long.Parse(dr["$alterid"].ToString())) : 0;
                        double percentageOfCalculation= (dr["$VATCLASSIFICATIONRATE"] != DBNull.Value) ? ((StringUtilsCustom.ExtractDoubleValue(dr["$VATCLASSIFICATIONRATE"].ToString()))) : 0;
                        if (percentageOfCalculation == 0)
                        {
                          //  String b = taxMasterDTO.vatName.Replace("[^\\d.]", "");

                          String  b = String.Concat(taxMasterDTO.vatName.Where(char.IsDigit));
                            try
                            {
                                if (b != "")
                                {
                                    double c = Convert.ToDouble(b);
                                    percentageOfCalculation = c;
                                }
                            }
                            catch (Exception e)
                            {
                                LogManager.WriteLog("number format exceptoion name that converting to String : "
                                        + taxMasterDTO.vatName);
                            }
                        }
                        if (percentageOfCalculation != 0)
                        {
                            String vatClass = taxMasterDTO.vatName.Replace("\\P{L}", "");
                            taxMasterDTO.vatClass=vatClass;
                            taxMasterDTO.vatPercentage=percentageOfCalculation;
                            taxMasterDTOs.Add(taxMasterDTO);
                        }
                       
                       
                    }
                }
            
              
                upload(taxMasterDTOs);
            }
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
