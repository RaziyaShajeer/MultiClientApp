using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Parsers;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    public class ProductWiseDefaultLedgerService
    {
        readonly TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        private bool fullUpdate = true;
        private string idClentApp;
        private String tallyLedgerParent;
        private  VatLedgerService vatLedgerService;
        public ProductWiseDefaultLedgerService()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
            tallyLedgerParent = ApplicationProperties.properties.GetValueOrDefault("tally.ledger.parent").ToString();
            vatLedgerService = new VatLedgerService();
        }
        internal async Task getFromTallyAndUploadAsync()
        {
            try
            {
                //for getting default ledger
                ENVELOPE tallyRequest = new ENVELOPE();
                tallyRequest= getCompanyStockItemsWithVatXml();


                var stringwriter = new System.IO.StringWriter();
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyRequest.GetType());
                x.Serialize(stringwriter, tallyRequest);

                var data = await tallyCommunicator.ExecXml(stringwriter.ToString());
                StockItemVatResponseParser stockItemVatResponseParser = new StockItemVatResponseParser();
                List<ProductProfileDTO> ppToServer = stockItemVatResponseParser.parseProductWiseDefaultLedgerListXml(data);
                if (ppToServer.Count > 0)
                {
                    upload(ppToServer);
                }

                //prev code using odbc
                //DataTable response = new DataTable();
                //StringBuilder Query = new StringBuilder();
                //Query.Append("select $Parent,$Name,$TAXCLASSIFICATIONNAME,$alterid from " + Tables.StockItem);

                //response = tallyCommunicator.getdatatable(Query.ToString());

                //List<VatLedgerDTO> vatLedgerDTOs = vatLedgerService.getFromTallyAndUpload();
                //if (response.Rows.Count > 0)
                //{
                //    List<ProductProfileDTO> _list = new List<ProductProfileDTO>();

                //    foreach (DataRow dr in response.Rows)
                //    {
                //        ProductProfileDTO productProfileDTo = new ProductProfileDTO();

                //        productProfileDTo.name = (dr["$name"] != DBNull.Value) ? (string)dr["$name"] : "";
                //        productProfileDTo.description = (dr["$parent"] != DBNull.Value) ? (string)dr["$parent"] : "";
                //        string TCName = (dr["$TAXCLASSIFICATIONNAME"] != DBNull.Value) ? (string)dr["$TAXCLASSIFICATIONNAME"] : "";
                //        foreach (VatLedgerDTO vat in vatLedgerDTOs)
                //        {
                //            if (vat.vatClass.Equals(TCName, StringComparison.OrdinalIgnoreCase))
                //            {
                //                productProfileDTo.taxRate = vat.percentageOfCalculation;
                //            }
                //        }
                //        productProfileDTo.alterId = (dr["$alterid"] != DBNull.Value) ? (StringUtilsCustom.ExtractDoubleValue(dr["$alterid"].ToString())) : 0;

                //        _list.Add(productProfileDTo);

                //    }




                //    if (_list.Count > 0)
                //    {
                //        upload(_list);
                //    }
                //    //fileManagerService.writeObjectToFile(apTally, FILE_NAME);
                //}
            }
            catch(Exception ex)
            {
				LogManager.HandleException(ex);
				throw ex;
			}
        }

        private ENVELOPE getCompanyStockItemsWithVatXml()
        {
            ENVELOPE tallyRequest = new ENVELOPE();
            HEADER header = new HEADER();
            header.VERSION = "1";
            header.TALLYREQUEST = "Export";
            header.TYPE = "Collection";
            header.ID = "All Stock Items";
            tallyRequest.HEADER = header;

            BODY body = new();
            DESC desc = new();
            STATICVARIABLES staticvariables = new STATICVARIABLES();
            //staticvariables.EXPLODEFLAG = "Yes";
            staticvariables.SVCURRENTCOMPANY = ApplicationProperties.properties["tally.company"].ToString();
            staticvariables.SVEXPORTFORMAT = "$$SysName:XML";
            //staticvariables.IsItemWise = "Yes";
            desc.STATICVARIABLES = staticvariables;
            TDL tdl = new TDL();
            TDLMESSAGE tdlmessage = new TDLMESSAGE();
            COLLECTION collection = new COLLECTION();
            collection.NAME = "All Stock Items";

            collection.ISMODIFY = "No";

            List<String> types = new List<String>();
            types.Add("stock item");

            collection.TYPE = types;
            collection.FETCH = "name,GUID,parent,saleslist.list";

            List<COLLECTION> collectionsList = new List<COLLECTION>();
            collectionsList.Add(collection);
            tdlmessage.COLLECTION = collectionsList;
            tdl.TDLMESSAGE = tdlmessage;
            desc.TDL = tdl;
            body.DESC = desc;
            tallyRequest.BODY = body;

            return tallyRequest;
        }

        private void upload(List<ProductProfileDTO> list)
        {
            string requestUri = ApiConstants.PREFIX + ApiConstants.PRODUCT_WISE_DEFAULT_LEDGER;//SNR_CLIENT_APP_LPP_1

			if (idClentApp.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                requestUri = ApiConstants.PREFIX + ApiConstants.PRODUCT_WISE_DEFAULT_LEDGER_ID;//SNR_CLIENT_APP_LPP_2
			}
            LogManager.WriteLog("uploading PRODUCT_WISE_DEFAULT_LEDGER_ID started...");
            httpClient = RestClientUtil.getClient();
            var myContent = JsonConvert.SerializeObject(list);
            HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

            var responseTask = httpClient.PostAsync(requestUri , inputContent);

            responseTask.Wait();

            HttpResponseMessage Res = responseTask.Result;
            LogManager.WriteResponseLog(Res);

            if (Res.IsSuccessStatusCode)
            {
                LogManager.WriteLog("request for uploading PRODUCT_WISE_DEFAULT_LEDGER_ID  Success..");
                var response = Res.Content.ReadAsStringAsync().Result;
            }
            else
            {
                LogManager.WriteLog("request for uploading PRODUCT_WISE_DEFAULT_LEDGER_ID  Failed..");
                throw new ServiceException("PRODUCT_WISE_DEFAULT_LEDGER_ID upload failed statuscode:" + Res.StatusCode + " Message : " + Res.RequestMessage);

            }
        }
    }
}
