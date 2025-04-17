using Microsoft.VisualBasic.Logging;
using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Parsers;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
//using SNR_ClientApp.TallyRequests;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SNR_ClientApp.Services
{
    public class OpeningStockService
    {
        TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        OpeningStockTallyMasterResponseParser openingStockTallyMasterResponseParser;
        private string idClentApp;
        TallyService tallyService ;
        string netstockAvil = ApplicationProperties.properties["netStockAvilable"].ToString();
        public OpeningStockService()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            openingStockTallyMasterResponseParser = new OpeningStockTallyMasterResponseParser();
            idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
            tallyService = new TallyService();
        }
        public async Task<bool> getFromTallyAndUploadAsync()
        {
             bool res=false;
            try
            {
                List<NetStockDetailsDTO> stockSummaryList = new List<NetStockDetailsDTO>();
                String key = ApplicationProperties.properties["stocklocationMethode"].ToString();
                ENVELOPE tallyRequest = new ENVELOPE();
                tallyRequest= getCompanyStockSummaryBatchWiseXml();


                var stringwriter = new System.IO.StringWriter();
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyRequest.GetType());
                x.Serialize(stringwriter, tallyRequest);

                var data = await tallyCommunicator.ExecXmlAndGetXmlAsync(stringwriter.ToString());

                ENVELOPE tallyRequest2 = new ENVELOPE();
                tallyRequest2 = getCompanyNettStockAvilBatchWiseXml();


                var stringwriter2 = new System.IO.StringWriter();
                //System.Xml.Serialization.XmlSerializer x2 = new System.Xml.Serialization.XmlSerializer(tallyRequest2.GetType());
                x.Serialize(stringwriter2, tallyRequest2);

                TallyRequestResponse data2 = await tallyCommunicator.ExecXml(stringwriter2.ToString());
              

                if (netstockAvil.Equals("true",StringComparison.OrdinalIgnoreCase))
                {
                    stockSummaryList = openingStockTallyMasterResponseParser
                            .parseNetStockAvilableXml(data2);
                }

                List<OpeningStockDTO> opstkToServer = new List<OpeningStockDTO>();
                List<OpeningStockDTO> opstkToServerTo = new List<OpeningStockDTO>();
                List<ProductProfileDTO> stockItems=new List<ProductProfileDTO>();
              
                stockItems =await tallyService.getAllStockItems();
                if (key==null || key=="")
                {
                    OpeningStockTallyMasterResponseParser openingStockTallyMasterResponseParser = new OpeningStockTallyMasterResponseParser();
                    opstkToServer = openingStockTallyMasterResponseParser.parseStockSummaryChangeOrderXml(data);

                }
                else
                {

                    opstkToServer = openingStockTallyMasterResponseParser.parseStockSummaryChangeOrderXml(data);

                    foreach (NetStockDetailsDTO netstock in stockSummaryList)
                    {
                        foreach (OpeningStockDTO opt in opstkToServer)
                        {
                          
                            if (netstock.productProfileName.Equals(opt.productProfileName, StringComparison.OrdinalIgnoreCase))
                            {
                                LogManager.WriteLog("presentelse");
                                int netQuanty = netstock.itemQuantity - netstock.stockQuantity;
                               LogManager.WriteLog(netstock.productProfileName + "====" + netstock.itemQuantity
                                                    + "-" + netstock.stockQuantity + "=" + netQuanty);
                                opt.quantity = netQuanty;
                                opstkToServerTo.Add(opt);
                            }
                        }
                    }

                }

                HashSet<string> ppNames = new HashSet<string>(opstkToServer.Select(p => p.Displayname));
                //foreach (var item1 in opstkToServer)
                //{
                //    LogManager.WriteLog(item1.productProfileName + "====" + item1.quantity);
                //}
                //foreach (var item1 in opstkToServerTo)
                //{
                //    LogManager.WriteLog(item1.productProfileName + "====" + item1.quantity);
                //}

                if (ppNames.Count > 0)
                {
                    uploadStockLocation(stockLocations(ppNames));

                }
                if (opstkToServerTo.Count>0 || opstkToServer.Count>0)
                {
                    

                    if (netstockAvil.Equals("true",StringComparison.OrdinalIgnoreCase))
                    {
                        opstkToServerTo.ForEach(p =>
                        {
                            p.productId=stockItems.Where(s => s.name==p.productProfileName).FirstOrDefault()?.productId;
                        });
                        LogManager.WriteLog("netstock true");
                        if(opstkToServerTo.Count>0)
                         res= upload(opstkToServerTo);
                    }
                    else
                    {
                        opstkToServer.ForEach(p =>
                        {
                            p.productId=stockItems.Where(s => s.name==p.productProfileName).FirstOrDefault()?.productId;
                        });
                        LogManager.WriteLog("netstock false");
                        if (opstkToServer.Count>0)
                        {
                            LogManager.WriteLog(opstkToServer.ToString());  
                             res= upload(opstkToServer);
                            return res;
                        }
                    }

                }
                else
                {
                    res=true;
                }


                return res;
            }
            catch(Exception ex)
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

                string requestUri = ApiConstants.PREFIX + ApiConstants.OPENING_STOCK;

                if (idClentApp.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    requestUri = ApiConstants.PREFIX + ApiConstants.OPENING_STOCK_ID;
                }


                LogManager.WriteLog("uploading Opening Stock  started...\n" + "Api  : " + requestUri);
                httpClient = RestClientUtil.getClient();
                var myContent = JsonConvert.SerializeObject(list);
                LogManager.WriteLog(myContent.ToString());  
                HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");
                LogManager.WriteLog("url to opening stock "+requestUri+ "and body"+myContent.ToString());
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
                    throw new ServiceException("Opening Stock upload failed statuscode:" + Res.StatusCode + " Message : " + Res.RequestMessage);

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

        public void uploadStockLocation(List<StockLocationDTO> stockLocationDTOs)
        {

            try
            {
                LogManager.WriteLog(" uploadStockLocation To Server Started....");


                string requestUri = ApiConstants.PREFIX + ApiConstants.STOCK_LOCATION;
             
                LogManager.WriteLog("uploading STOCK_LOCATION started...\n" + "Api  : " + requestUri);
                httpClient = RestClientUtil.getClient();
                var myContent = JsonConvert.SerializeObject(stockLocationDTOs);
                LogManager.WriteLog(myContent.ToString());
                HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");
                LogManager.WriteLog(inputContent.ToString());
                LogManager.WriteLog("url to stockLocation "+requestUri+ "and body"+myContent.ToString());
                var responseTask = httpClient.PostAsync(requestUri, inputContent);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteLog("Uploading STOCK_LOCATION To Server Completed ....\n Response : ");
                LogManager.WriteResponseLog(Res);

                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("request for uploading STOCK_LOCATION  Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                }
                else
                {

                    LogManager.WriteLog("request for uploading STOCK_LOCATION  Failed..");
                    throw new ServiceException("STOCK_LOCATION failed statuscode:" + Res.StatusCode + " Message : " + Res.RequestMessage);

                }
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }

        }

        public ENVELOPE getCompanyNettStockAvilBatchWiseXml()
        {

            ENVELOPE tallyRequest = new ENVELOPE();
            HEADER header = new HEADER();
            header.VERSION = "1";
            header.TALLYREQUEST = "Export";
            header.TYPE = "Data";
            header.ID = "ItemUpload";
            tallyRequest.HEADER = header;

            BODY body = new();
            DESC desc = new();
            STATICVARIABLES staticvariables = new STATICVARIABLES();
           
            staticvariables.SVCURRENTCOMPANY = ApplicationProperties.properties["tally.company"].ToString();
            staticvariables.SVEXPORTFORMAT = "$$SysName:XML";
          
            desc.STATICVARIABLES = staticvariables;
            TDL tdl = new TDL();
            TDLMESSAGE tdlmessage = new TDLMESSAGE();

            REPORT report = new REPORT();
            report.NAME = "ItemUpload";
            report.ISMODIFY = "No";
            report.ISFIXED = "No";
            report.ISINITIALIZE = "No";
            report.ISOPTION = "No";
            report.ISINTERNAL = "No";
            report.FORMS = "ItemUpload";
            List<String> set = new List<string>();
            set.Add("SVFromDate : $$MonthStart:##SVCurrentDate");
            set.Add("SVToDate :$MonthEnd:##SVCurrentDate");
            report.Set = set;

            tdlmessage.REPORT = report;

            FORM form = new FORM();
            form.NAME = "ItemUpload";
            form.ISMODIFY = "No";
            form.ISFIXED = "No";
            form.ISINITIALIZE = "No";
            form.ISOPTION = "No";
            form.ISINTERNAL = "No";
            form.TOPPARTS = "ItemUpload";
            form.HEIGHT = "100 % PAge";
            form.WIDTH = "100 % PAge";

            tdlmessage.FORM = form;

            PART part = new PART();
            part.NAME = "ItemUpload";
            part.ISMODIFY = "No";
            part.ISFIXED = "No";
            part.ISINITIALIZE = "No";
            part.ISOPTION = "No";
            part.ISINTERNAL = "No";
            part.TOPLINES = "ItemUpload";
            part.REPEAT = "ItemUpload : testStockcoll";
            part.SCROLLED = "Vertical";
            part.VERTICAL = "Yes";
            List<PART> parts = new List<PART>();
            parts.Add(part);
            tdlmessage.PART = parts;

            LINE line = new LINE();
            line.NAME = "ItemUpload";
            line.ISMODIFY = "No";
            line.ISFIXED = "No";
            line.ISINITIALIZE = "No";
            line.ISOPTION = "No";
            line.ISINTERNAL = "No";
            line.LEFTFIELDS = "test_StockItem";
            line.RIGHTFIELDS = "test_CLBAL, test_SOQTY";
            line.FIELD="test_Guid";
            line.XMLtag = "Itemlist";

            List<LINE> lines=new List<LINE>();
            lines.Add(line);
            tdlmessage.LINE = lines;
            List<FIELD> fieldList = new List<FIELD>();
            FIELD field = new FIELD();
            field.NAME = "test_StockItem";
            field.ISMODIFY = "No";
            field.ISFIXED = "No";
            field.ISINITIALIZE = "No";
            field.ISOPTION = "No";
            field.ISINTERNAL = "No";
            field.USE = "NameField";
            field.SET = "$Name";
            field.XMLTAG = "itemname";
            fieldList.Add(field);
            FIELD field2 = new FIELD();
            field2.NAME = "test_CLBAL";
            field2.ISMODIFY = "No";
            field2.ISFIXED = "No";
            field2.ISINITIALIZE = "No";
            field2.ISOPTION = "No";
            field2.ISINTERNAL = "No";
            field2.USE = "Number Field";
            field2.SET = "$$Number:$ClosingBalance";
            field2.XMLTAG = "itemqty";
            fieldList.Add(field2);

            FIELD field3 = new FIELD();
            field3.NAME = "test_SOQTY";
            field3.ISMODIFY = "No";
            field3.ISFIXED = "No";
            field3.ISINITIALIZE = "No";
            field3.ISOPTION = "No";
            field3.ISINTERNAL = "No";
            field3.USE = "Number Field";
            field3.SET = "$$Number:$sodue";
            field3.XMLTAG = "itemsoqty";
            fieldList.Add(field3);

            //Todo check this code , added for getting guid 

            FIELD field4 = new FIELD();
            field4.NAME = "test_Guid";
            field4.ISMODIFY = "No";
            field4.ISFIXED = "No";
            field4.ISINITIALIZE = "No";
            field4.ISOPTION = "No";
            field4.ISINTERNAL = "No";
            field4.USE = "NameField";
            field4.SET = "$Guid";
            field4.XMLTAG = "Guid";
            fieldList.Add(field4);

            tdlmessage.FIELD = fieldList;

            List<COLLECTION> collectionsList=new List<COLLECTION>();
            COLLECTION collection = new COLLECTION();
            collection.NAME = "testStockcoll";
            collection.ISMODIFY = "No";
            collection.ISFIXED = "No";
            collection.ISINITIALIZE = "No";
            collection.ISOPTION = "No";
            collection.ISINTERNAL = "No";
           List<String> types= new List<String>();
            types.Add("StockItem");
            collection.TYPE = types;
            collection.BelongsTo = "Yes";
            List<String> NativeMethod = new List<string>();
            NativeMethod.Add("*.*");
            NativeMethod.Add("Name,ClosingBalance,sodue");
            collection.NativeMethod = NativeMethod;
            collectionsList.Add(collection);

            tdlmessage.COLLECTION = collectionsList;
            tdl.TDLMESSAGE = tdlmessage;
            desc.TDL = tdl;
            body.DESC = desc;
            tallyRequest.BODY = body;

            return tallyRequest;
        }

        public ENVELOPE getCompanyStockSummaryBatchWiseXml()
        {
            ENVELOPE tallyRequest = new ENVELOPE();
            HEADER header = new HEADER();
            header.VERSION = "1";
            header.TALLYREQUEST = "Export";
            header.TYPE = "Data";
            header.ID = "Stock Summary";
            tallyRequest.HEADER = header;

            BODY body = new();
            DESC desc = new();
            STATICVARIABLES staticvariables = new STATICVARIABLES();
            staticvariables.EXPLODEFLAG = "Yes";
            staticvariables.SVCURRENTCOMPANY = ApplicationProperties.properties["tally.company"].ToString();
            staticvariables.SVEXPORTFORMAT = "$$SysName:XML";
            staticvariables.IsItemWise = "Yes";
            desc.STATICVARIABLES = staticvariables;
            TDL tdl = new TDL();
            TDLMESSAGE tdlmessage = new TDLMESSAGE();
            COLLECTION collection = new COLLECTION();
            collection.NAME = "Collection of StockItem";

            collection.ISMODIFY = "No";

            List<String> types = new List<String>();
            types.Add("Collection");

            collection.TYPE = types;
            collection.FETCH = "BatchAllocations.*";

            List<COLLECTION> collectionsList = new List<COLLECTION>();
            collectionsList.Add(collection);
            tdlmessage.COLLECTION = collectionsList;
            tdl.TDLMESSAGE = tdlmessage;
            desc.TDL = tdl;
            body.DESC = desc;
            tallyRequest.BODY = body;

            return tallyRequest;
        }

        public List<StockLocationDTO> stockLocations(HashSet<string> ppNames)
        {
            LogManager.WriteLog("creating stock locations.");
            List<StockLocationDTO> stockLocationDTOs = new List<StockLocationDTO>();
            foreach (string slocname in ppNames)
            {
                if (slocname != null)
                {
             

                    
                    stockLocationDTOs.Add(new StockLocationDTO(slocname));
                }
            }
            if (!ApplicationProperties.properties["IsEnableDistributor"].ToString().Equals("true",StringComparison.OrdinalIgnoreCase))
            {
                StockLocationDTO ob = new StockLocationDTO();
                ob.name="Main Location";
                ob.displayName="Main Location";
                ob.stockLocationType = StockLocationType.ACTUAL;
                ob.activated = true;
                stockLocationDTOs.Add(ob);
            }

        
            return stockLocationDTOs;
        }
    }
}
