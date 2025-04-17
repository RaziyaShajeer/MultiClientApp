using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.DTO.Salesorder;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
using SNR_ClientApp.Tally.generateXml;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SNR_ClientApp.Services
{
    public class UploadService
    {
        TallyCommunicator tallyCommunicator;
        private string idClentApp;

        HttpClient httpClient;
        public string companyName = ApplicationProperties.properties["tally.company"].ToString();
        TallyMastersRequestXml tallyMastersRequestXml;
        public UploadService()
        {
            tallyMastersRequestXml = new TallyMastersRequestXml();  
            httpClient = new HttpClient();
            tallyCommunicator = new TallyCommunicator();
            idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();

        }
        internal async Task getFromTallyAndUploadAsync()
        {
            List<string> salesreferences = new List<string>();
            DataTable response = new DataTable();
            StringBuilder Query = new StringBuilder();
            try
            {

          
            Query.Append("select $name,$Parent  from " + Tables.VoucherType + " where $Parent= Sales");


            if (response.Rows.Count > 0)
            {
                List<VoucherTypeDTO> voucherTypes = new List<VoucherTypeDTO>();

                foreach (DataRow dr in response.Rows)
                {
                    VoucherTypeDTO dto = new VoucherTypeDTO();
                    dto.parent = (dr["$Parent"] != DBNull.Value) ? (string)dr["$Parent"] : "";
                    dto.name = (dr["$name"] != DBNull.Value) ? (string)dr["$name"] : "";


                    voucherTypes.Add(dto);

                }
                    if (voucherTypes.Count > 0)
                    {
                        TallyMastersRequestXml tallyMastersRequestXml = new TallyMastersRequestXml();
                        string getsalesordervouchernumber = ApiConstants.GET_SALESVOUCHERNUMBER_FROM_SERVER;
                        LogManager.WriteLog("updating  salesOrders status Api:...."+getsalesordervouchernumber);
                        LogManager.WriteLog(getsalesordervouchernumber.ToString());
                        httpClient = RestClientUtil.getClient();

                        var responseTask1 = httpClient.GetAsync(getsalesordervouchernumber);
                        responseTask1.Wait();
                        HttpResponseMessage Res1 = responseTask1.Result;

                        LogManager.WriteResponseLog(Res1);


                        if (Res1.IsSuccessStatusCode)
                        {
                            LogManager.WriteLog("request for updating  salesOrders  status  Success..");
                            string jsonString = await Res1.Content.ReadAsStringAsync();

                            // Deserialize the JSON string into a List<string>
                            List<string> list = JsonConvert.DeserializeObject<List<string>>(jsonString);
                            if (list.Count>0)
                            {
                                ENVELOPE tallyrequest = tallyMastersRequestXml.GetSalesFromReference(list);
                                var stringwriter = new System.IO.StringWriter();
                                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyrequest.GetType());
                                x.Serialize(stringwriter, tallyrequest);
                                var Salesxmlquery = await tallyCommunicator.ExecXml(stringwriter.ToString());

                                foreach (var sales in Salesxmlquery.response.BODY.DATA.COLLECTION.VOUCHER)
                                {
                                    if (sales.VOUCHERNUMBER!=sales.REFERENCE)
                                    {
                                        salesreferences.Add(sales.REFERENCE);
                                    }
                                }
                                string UpdateSalesOrderStatus = ApiConstants.UPDATE_STATUS_OF_SALES_ORDER;
                                LogManager.WriteLog("updating  salesOrders status Api:...."+UpdateSalesOrderStatus);
                                LogManager.WriteLog(salesreferences.ToString());
                                httpClient = RestClientUtil.getClient();

                                HttpContent content2 = new StringContent(JsonConvert.SerializeObject(salesreferences), Encoding.UTF8, "application/json");
                                var responseTask2 = httpClient.PostAsync(UpdateSalesOrderStatus, content2);
                                responseTask2.Wait();
                                HttpResponseMessage Res2 = responseTask2.Result;
                                LogManager.WriteResponseLog(Res1);


                                if (Res2.IsSuccessStatusCode)
                                {
                                    LogManager.WriteLog("request for updating  salesOrders  status  Success..");
                                    var response2 = Res2.Content.ReadAsStringAsync().Result;

                                }
                                else
                                {
                                    LogManager.WriteLog("request for updating  salesOrders status Failed..");

                                }

                            }
                        }
                        else
                        {
                            LogManager.WriteLog("request for updating  salesOrders status Failed..");

                        }
                    }
                   

                  
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
