using SNR_ClientApp.DTO;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
//using SNR_ClientApp.TallyRequests;

using SNR_ClientApp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;

using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.TallyResponses;
using System.Diagnostics;

namespace SNR_ClientApp.Services
{
    internal class PriceLevelListService
    {
        public static readonly String FILE_NAME = "price-level-list";
        TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        private string idClentApp;
        public PriceLevelListService()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
        }
        internal async Task getFromTallyAndUploadAsync()
        {
            try
            {
                ENVELOPE tallyRequest = new ENVELOPE();
                HEADER header = new HEADER();
                header.VERSION = "1";

                header.TALLYREQUEST = "Export";
                header.TYPE = "Collection";
                header.ID = "All items under Groups";
                tallyRequest.HEADER= header;

                BODY body = new();
                DESC desc=new DESC();
                STATICVARIABLES staticvariables= new STATICVARIABLES();
              
               staticvariables.SVCURRENTCOMPANY = ApplicationProperties.properties["tally.company"].ToString();
                staticvariables.SVEXPORTFORMAT = "$$SysName:XML";
                desc.STATICVARIABLES = staticvariables;
                TDL tdl=new TDL();
                TDLMESSAGE tDLMESSAGE=new TDLMESSAGE();
                COLLECTION cOLLECTION=new COLLECTION();
                List<String> types = new List<String>();
                types.Add("stock item");
                cOLLECTION.TYPE = types;
               
                cOLLECTION.NAME = "All items under Groups";
                cOLLECTION.ISMODIFY = "No";
                cOLLECTION.FETCH = "FullPriceList,Name,Guid";

                List<COLLECTION> collectionsList = new List<COLLECTION>();
                collectionsList.Add(cOLLECTION);
                tDLMESSAGE.COLLECTION = collectionsList;
                tdl.TDLMESSAGE = tDLMESSAGE;
                desc.TDL = tdl;
                body.DESC = desc;


                tallyRequest.BODY=body;
               


                var stringwriter = new System.IO.StringWriter();
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyRequest.GetType());
                x.Serialize(stringwriter, tallyRequest);
                LogManager.WriteLog("Tallyrequest" +tallyRequest);

                TallyRequestResponse data = await tallyCommunicator.ExecXml(stringwriter.ToString());

                List<StockItemPriceLevel> stockItemPriceLevelTally = CovertResponseToStockItemPriceLevels(data);
                PriceLevelPriceLevelList plPLLDto = getPriceLevelAndPriceLevelList(stockItemPriceLevelTally);
                if (plPLLDto.priceLevelDtos.Count>0)
                {
                    uploadPricelevel(plPLLDto.priceLevelDtos);
                }
                if (plPLLDto.priceLevelListDtos.Count>0)
                {
                    uploadPricelevelList(plPLLDto.priceLevelListDtos);
                }

            }
            catch(Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
        }

        private void uploadPricelevelList(List<PriceLevelListDTO> list)
        {
            try
            {
                idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
                LogManager.WriteLog("Uploading PriceLevel To Server Started....");
                string requestUri = ApiConstants.PREFIX + ApiConstants.PRICE_LIST_LEVEL_LIST;//SNR_CLIENT_APP_PLL_1
				if (idClentApp.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                     requestUri = ApiConstants.PREFIX + ApiConstants.PRICE_LIST_LEVEL_LIST_ID; //SNR_CLIENT_APP_PLL_2
				} 



                LogManager.WriteLog("uploading PRICE_LIST_LEVEL_LIST started...\n" + "Api  : " + requestUri);
                httpClient = RestClientUtil.getClient();
                var myContent = JsonConvert.SerializeObject(list);
                HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

                var responseTask = httpClient.PostAsync(requestUri, inputContent);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteLog("Uploading PriceLevelList To Server Completed ....\n Response : ");
                LogManager.WriteResponseLog(Res);

                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("request for uploading PRICE_LIST_LEVEL_LIST  Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    LogManager.WriteLog("request for uploading PRICE_LIST_LEVEL_LIST  Failed..");
                }
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
        }

        private void uploadPricelevel(List<PriceLevelDTO> list)
        {
            try
            {
                LogManager.WriteLog("Uploading PriceLevel To Server Started....");


                string requestUri = ApiConstants.PREFIX + ApiConstants.PRICE_LEVEL;//SNR_CLIENT_APP_PL_1

				LogManager.WriteLog("uploading PRICE_LEVEL started...\n" + "Api  : " + requestUri);
                httpClient = RestClientUtil.getClient();
                var myContent = JsonConvert.SerializeObject(list);
                HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

                var responseTask = httpClient.PostAsync(requestUri, inputContent);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteLog("Uploading PriceLevel To Server Completed ....\n Response : ");
                LogManager.WriteResponseLog(Res);

                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("request for uploading PRICE_LEVEL  Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    LogManager.WriteLog("request for uploading PRICE_LEVEL  Failed..");
                }
            }catch(Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
        }

        private PriceLevelPriceLevelList getPriceLevelAndPriceLevelList(List<StockItemPriceLevel> stockItemPriceLevelTally)
        {
            string serializedString = JsonConvert.SerializeObject(stockItemPriceLevelTally);
            LogManager.WriteLog("Items:" +serializedString);


            // avoid duplicate with date
            List<StockItemPriceLevel> stockItemPriceLevels = updateToOnlyLatestPriceLevels(stockItemPriceLevelTally);

            List<PriceLevelListDTO> priceLevelListDtos = new List<PriceLevelListDTO>();
            HashSet<PriceLevelDTO> priceLevelDtos = new HashSet<PriceLevelDTO>();
            foreach (StockItemPriceLevel siPriceLevel in stockItemPriceLevels)
            {
                foreach ( PriceLevel pl in siPriceLevel.priceLevels) {
                    PriceLevelListDTO priceLevelListDTO = new PriceLevelListDTO(siPriceLevel.productName,
                            pl.priceLevelName, pl.price, pl.date, 0.0, 0.0);

                    //todo need to check with server
                    priceLevelListDTO.productId=pl.productId;
                    priceLevelListDTO.discount=pl.discount; 
                    priceLevelListDtos.Add(priceLevelListDTO);
                    PriceLevelDTO priceLevelDto = new PriceLevelDTO(pl.priceLevelName, true);
                    //Todo : need to verify that product id is required in pricelevelDto
                    //  priceLevelDto.productId=pl.productId; 
                    if (!priceLevelDtos.Any(e => e.name==priceLevelDto.name))
                        priceLevelDtos.Add(priceLevelDto);
                }
            }
		return new PriceLevelPriceLevelList(priceLevelDtos: new List<PriceLevelDTO>(priceLevelDtos), priceLevelListDtos);

        }

        private List<StockItemPriceLevel> updateToOnlyLatestPriceLevels(List<StockItemPriceLevel> stockItemPriceLevelTally)
        {
            foreach (StockItemPriceLevel stockItemPriceLevel in stockItemPriceLevelTally)
            {
                

                // group by name
                Dictionary<String, List<PriceLevel>> pricelevelMap = stockItemPriceLevel.priceLevels.GroupBy(item => item.priceLevelName).ToDictionary(chunk => chunk.Key, chunk => chunk.ToList());

                List<PriceLevel> filteredPriceLevel = new List<PriceLevel>();
                // find max date
                foreach (var item in pricelevelMap)
                {
                    


                    DateTime maxDate = item.Value.Max(x => x.date);
                    LogManager.WriteLog("MAXDATE : " + maxDate + " -- " + stockItemPriceLevel.productName + "--" +item.Key);
                    filteredPriceLevel.AddRange(item.Value.Where(x => x.date == maxDate).ToList());

                }

                stockItemPriceLevel.priceLevels = filteredPriceLevel;
            }
            return stockItemPriceLevelTally;
        
    }


        private List<StockItemPriceLevel> CovertResponseToStockItemPriceLevels(TallyRequestResponse data)
        {
            List<StockItemPriceLevel> stockItemPriceLevels = new List<StockItemPriceLevel>();
         
            foreach (var item in data.response.BODY.DATA.COLLECTION.STOCKITEM)
            {
                StockItemPriceLevel stockItemPriceLevel = new StockItemPriceLevel();
                stockItemPriceLevel.productName = item.NAME;
                int size = item.FULLPRICELIST_LIST.Count;
    //            var priceLevelDictionary = item.FULLPRICELIST_LIST
    //.Where(item => !string.IsNullOrEmpty(item.PRICELEVEL))
    //.ToDictionary(item => item.PRICELEVEL, item => item);

                foreach (FULLPRICELIST_LIST pricelevelItem in item.FULLPRICELIST_LIST)
                {
                //    if (size>0)
                //{
                    //FULLPRICELIST_LIST pricelevelItem = item.FULLPRICELIST_LIST[size-1];

                    if (pricelevelItem.DATE != null && pricelevelItem.PRICELEVEL != null)
                    {
                        PriceLevel priceLevel = new PriceLevel();
                        priceLevel.priceLevelName = pricelevelItem.PRICELEVEL;
                        priceLevel.date = DateTime.ParseExact(pricelevelItem.DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
                        //DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyyMMdd");
                        //priceLevel.date = DateTime.Parse(pricelevelItem.DATE,);

                        //String price = String.Join("", pricelevelItem.PRICELEVELLIST_LIST.RATE.Where(char.IsDigit));
                        String price = pricelevelItem.PRICELEVELLIST_LIST.RATE;

                        priceLevel.price = StringUtilsCustom.ExtractDoubleValue(price);
                        //Todo 
                        // Check this line is correct 
                        priceLevel.productId=item.GUID.text;
                        priceLevel.discount=StringUtilsCustom.ExtractDoubleValue(pricelevelItem.PRICELEVELLIST_LIST.DISCOUNT);

                        if (priceLevel.priceLevelName != null && priceLevel.date != null)
                        {
                            if (priceLevel.date <= DateTime.Now)
                            {

                                if (!stockItemPriceLevel.priceLevels.Any(e => e.priceLevelName==priceLevel.priceLevelName))
                            {
                                stockItemPriceLevel.priceLevels.Add(priceLevel);
                            }
                            else
                            {
                                var pricelevelexist = stockItemPriceLevel.priceLevels.Where(e => e.priceLevelName==priceLevel.priceLevelName).FirstOrDefault();
                                LogManager.WriteLog("PRODUCT_NAME" +stockItemPriceLevel.productName+"-Date-"+pricelevelexist.date+"-NewDate-"+priceLevel.date);

                                if (pricelevelexist.date<priceLevel.date)
                                {
                                    LogManager.WriteLog("Entered : "  + stockItemPriceLevel.productName);
                                    stockItemPriceLevel.priceLevels.Remove(stockItemPriceLevel.priceLevels.Where(e => e.priceLevelName==priceLevel.priceLevelName).FirstOrDefault());

                                    stockItemPriceLevel.priceLevels.Add(priceLevel);
                                    LogManager.WriteLog("Date"+priceLevel.date);
                                }


                                //if (pricelevelexist.date<priceLevel.date)
                                //{
                                //    LogManager.WriteLog("Entered : "  + stockItemPriceLevel.productName);
                                //    stockItemPriceLevel.priceLevels.Remove(stockItemPriceLevel.priceLevels.Where(e => e.priceLevelName==priceLevel.priceLevelName).FirstOrDefault());

                                //    stockItemPriceLevel.priceLevels.Add(priceLevel);
                                //    LogManager.WriteLog("Date"+priceLevel.date);
                                //}
                            }
                        }
                                
                        }
                    }

                }
               //}

                stockItemPriceLevels.Add(stockItemPriceLevel);
            }

            return stockItemPriceLevels;
        }
    }
}
