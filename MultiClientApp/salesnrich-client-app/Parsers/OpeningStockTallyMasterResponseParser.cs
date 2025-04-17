using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SNR_ClientApp.Parsers
{
    public class OpeningStockTallyMasterResponseParser
    {
        internal List<OpeningStockDTO> parseStockSummaryChangeOrderXml(string inputString)
        {
            try
            {
                List<OpeningStockDTO> stockSummaryList = new List<OpeningStockDTO>();
                if (String.IsNullOrEmpty(inputString))
                {
                    return stockSummaryList;
                }

                var parts = getSubArrays(inputString, "<DSPACCNAME>", "</ENVELOPE>");
                XmlSerializer serializer1 = new XmlSerializer(typeof(StockBatchSub));
                List<StockBatchSub> stockSummaries = new List<StockBatchSub>();

                foreach (var xml in parts)
                {
                    LogManager.WriteLog(xml.ToString());
                    string newXml = "<StockSummary>" + xml + "</StockSummary>";

                    XDocument doc = XDocument.Parse(newXml);
                    var dspdispname = doc.Root.Element("DSPACCNAME").Element("DSPDISPNAME");

                    //var reader = new StringReader("<StockSummary>" + xml + "</StockSummary>");
                    //var i = (StockSummaryBatchWiseResponse)serializer.Deserialize(reader);
                    //stockSummaryBatchWiseResponsesList.Add(i);

                    //test code 7/20/23
                    //testing success 7/20/23
                    var batchSubParts = getSubArrays(xml, "<SSBATCHNAME>", "</DSPSTKINFO>", "</DSPSTKINFO>");

                    //var godownwiseStocks = getGodownWiseStocks(xml);
                    // var batchSubParts = getSubArrays(xml, "<DSPSTKINFO>", "</SSBATCHNAME>", "</DSPSTKINFO>");

                    //if (batchSubParts.Count==0)
                    //{
                    var stockInfo = doc.Root.Element("DSPSTKINFO");
                    LogManager.WriteLog(stockInfo.ToString());
                    if (stockInfo!=null)
                        batchSubParts.Add(stockInfo.ToString());
                    //}

                    
                    foreach (var batchSub in batchSubParts)
                    {
                        if (batchSubParts.Count>1)
                        {
                            var reader = new StringReader("<StockBatchSub>" + batchSub + "</StockBatchSub>");
                            var item = (StockBatchSub)serializer1.Deserialize(reader);
                            item.DSPDISPNAME = dspdispname.Value;
                            if (item.SSBATCHNAME!=null)
                            {
                                stockSummaries.Add(item);
                            }

                        }
                        else
                        {
                            var reader = new StringReader("<StockBatchSub>" + batchSub + "</StockBatchSub>");
                            var item = (StockBatchSub)serializer1.Deserialize(reader);
                            item.DSPDISPNAME = dspdispname.Value;
                            stockSummaries.Add(item);

                        }
                    }
                }
                
                XmlSerializer serializer = new XmlSerializer(typeof(StockSummaryBatchWiseResponse));
                List<StockSummaryBatchWiseResponse> stockSummaryBatchWiseResponsesList = new List<StockSummaryBatchWiseResponse>();
                foreach (var xml in parts)
                {
                    var reader = new StringReader("<StockSummary>" + xml + "</StockSummary>");
                    var i = (StockSummaryBatchWiseResponse)serializer.Deserialize(reader);
                    stockSummaryBatchWiseResponsesList.Add(i);
                }
                HashSet<string> stockLocations = new HashSet<string>();

                foreach (var item in stockSummaries)
                {
                    OpeningStockDTO stockSummary = new OpeningStockDTO();

                    if (item.SSBATCHNAME != null && item.SSBATCHNAME.SSGODOWN != null)
                    {
                        stockLocations.Add(item.SSBATCHNAME.SSGODOWN);
                    }
                }
                // If there are multiple locations, we will **skip "Main Location"** in the final list
                bool hasMultipleLocations = stockLocations.Count > 1;

                Dictionary<string, OpeningStockDTO> stockSummaryDict = new Dictionary<string, OpeningStockDTO>();
                foreach (var item in stockSummaries)
                {
                    // Dictionary to store unique stock entries per location
                   
                    OpeningStockDTO stockSummary = new OpeningStockDTO();
                    string pproductName = item.DSPDISPNAME;
                    if (item.SSBATCHNAME != null && item.SSBATCHNAME.SSGODOWN != null)
                    {
                        stockSummary.stockLocationName = item.SSBATCHNAME.SSGODOWN;
                    }
                    else
                    {
                        stockSummary.stockLocationName = "Main Location";
                    }

                    string qty = item.DSPSTKINFO.DSPSTKCL.DSPCLQTY;
                    stockSummary.quantity = StringUtilsCustom.getNumberFromString(qty);
                    stockSummary.productProfileName = item.DSPDISPNAME;
                    stockSummary.batchNumber = "123";
                    stockSummary.Displayname = stockSummary.stockLocationName;
                    stockSummary.distributorCode = ApplicationProperties.properties["DistributedCode"].ToString();
                    stockSummary.stockLocationName = DistributedCodeAppend.appendDistributedCode(stockSummary.stockLocationName);
                  
                   stockSummaryList.Add(stockSummary);
                }
                return stockSummaryList;

                LogManager.WriteLog(stockSummaryList.ToString());
                return stockSummaryList;
            }catch(Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }

        }

        private object getGodownWiseStocks(string xmlString)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<Root>" +xmlString+ "</Root>");
         

            var result = new List<JObject>();
            var dspAccNameNodes = xmlDoc.GetElementsByTagName("DSPACCNAME");

            for (int i = 0; i < dspAccNameNodes.Count; i++)
            {
                var dspAccNameNode = dspAccNameNodes[i];
                var dspDispName = dspAccNameNode.SelectSingleNode("DSPDISPNAME").InnerText;
                var ssgodownNode = dspAccNameNode.NextSibling.NextSibling.SelectSingleNode("SSBATCHNAME/SSGODOWN").InnerText;
                var dspStkInfoNodes = dspAccNameNode.NextSibling.SelectNodes("DSPSTKINFO");

                foreach (XmlNode dspStkInfoNode in dspStkInfoNodes)
                {
                    var dspStkClNode = dspStkInfoNode.SelectSingleNode("DSPSTKCL");
                    var dspClQty = dspStkClNode.SelectSingleNode("DSPCLQTY").InnerText;
                    var dspClRate = Convert.ToDouble(dspStkClNode.SelectSingleNode("DSPCLRATE").InnerText);
                    var dspClAmta = Convert.ToDouble(dspStkClNode.SelectSingleNode("DSPCLAMTA").InnerText);

                    var dspStkInfo = new JObject
                    {
                        ["DSPSTKCL"] = new JObject
                        {
                            ["DSPCLQTY"] = dspClQty,
                            ["DSPCLRATE"] = dspClRate,
                            ["DSPCLAMTA"] = dspClAmta
                        }
                    };

                    var jsonObject = new JObject
                    {
                        ["DSPACCNAME"] = new JObject
                        {
                            ["DSPDISPNAME"] = dspDispName
                        },
                        ["SSBATCHNAME"] = new JObject
                        {
                            ["SSGODOWN"] = ssgodownNode
                        },
                        ["DSPSTKINFO"] = new JArray(dspStkInfo)
                    };

                    result.Add(jsonObject);
                }
            }

            var jsonString = JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(jsonString);
            return jsonString;
        }
        
        public List<string> getSubArrays(String inputString, String startingTag,String endingTag,String OptionalEndingtag="")
        {
            List<String> parts = new List<string>();
            if (String.IsNullOrEmpty(inputString))
            {
                return parts;
            }
            int firstIndex = inputString.IndexOf(startingTag);
            int secondIndex = inputString.IndexOf(startingTag, firstIndex + 1);

           
            while (firstIndex != -1 && secondIndex != -1)
            {
                if(secondIndex > 0)
                {
                    parts.Add(inputString.Substring(firstIndex, secondIndex - firstIndex));
                    firstIndex = secondIndex;
                    secondIndex = inputString.IndexOf(startingTag, firstIndex + 1);
                }

                
            }
            if (secondIndex < 0)
            {
                int aditionalLength = 0;
                if (!OptionalEndingtag.Equals(""))
                {
                    int optIndex= inputString.IndexOf(endingTag, firstIndex + 1);
                    if(optIndex > 0)
                    {
                        secondIndex=optIndex;
                        aditionalLength= endingTag.Length;
                    }
                    else
                    {
                        secondIndex = inputString.IndexOf(OptionalEndingtag, firstIndex + 1);
                        aditionalLength = OptionalEndingtag.Length;
                    }
                }
                else
                {
                    secondIndex = inputString.IndexOf(endingTag, firstIndex + 1);
                }
                string tag= startingTag.Substring(1, startingTag.Length-2);
                // secondIndex = inputString.IndexOf(endingTag, firstIndex + 1);
                if (firstIndex>=0 && secondIndex>=0)
                {
                    parts.Add(inputString.Substring(firstIndex, secondIndex - firstIndex+ aditionalLength));
                }
            }


            return parts;
        }

        internal List<NetStockDetailsDTO> parseNetStockAvilableXml(TallyRequestResponse data)
        {
            try
            {
                List<NetStockDetailsDTO> stockSummaryList = new List<NetStockDetailsDTO>();

                if (data.response.ITEMLIST != null)
                {
                    foreach (var item in data.response.ITEMLIST)
                    {
                        NetStockDetailsDTO stockSummary = new NetStockDetailsDTO();
                        stockSummary.productProfileName = item.ITEMNAME;
                        stockSummary.itemQuantity = StringUtilsCustom.getNumberFromString(item.ITEMQTY);
                        stockSummary.stockQuantity = StringUtilsCustom.getNumberFromString((item.ITEMSOQTY));
                        
                        stockSummaryList.Add(stockSummary);
                    }
                }
                return stockSummaryList;
            }catch(Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
        }
    }
}
