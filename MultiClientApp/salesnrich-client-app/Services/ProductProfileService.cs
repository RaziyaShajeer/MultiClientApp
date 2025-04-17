
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Exceptions;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    internal class ProductProfileService
    {
        public static readonly   String FILE_NAME = "product-profile";
        TallyCommunicator tallyCommunicator = new TallyCommunicator();
        HttpClient httpClient = new HttpClient();
        private bool fullUpdate = true;
        private string idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
        private String GSTNames= ApplicationProperties.properties.GetValueOrDefault("tally.gst").ToString();
        private String ProductIgstName = ApplicationProperties.properties.GetValueOrDefault("tally.productIGST").ToString();
        private String ProductCessName = ApplicationProperties.properties.GetValueOrDefault("tally.productCESS").ToString();

        internal async Task getFromTallyAndUploadAsync(bool isoptimised)
        {
            List<ProductProfileDTO> _list = new List<ProductProfileDTO>();
            List<ProductProfileDTO> duplicateProductslist = new List<ProductProfileDTO>();
            List<ProductProfileTaxMasterDTO> uploadPPTaxToServer = new List<ProductProfileTaxMasterDTO>();
            string pattern = @"[\r\n\t]+$";
            var gstItems = GSTNames.Split(",");
            if (string.IsNullOrEmpty(GSTNames)) {
                gstItems=null;
            }
            
            DataTable response = new DataTable();
            StringBuilder Query = new StringBuilder();
            Query.Append("select $name,$_FirstAlias,$parent,$AlterID,$Guid,$Category,$RateOfMrp,$OpeningRate,$RateOfVat," +
                "$BaseUnits,$AdditionalUnits,$Conversion,$Denominator,$IsBatchWiseOn,$PartNo," +
                "$Description,$Narration,$_LastSalePrice,$_HSNCode,$_IntegratedTax,$_StateTax,$_CentralTax,$_Cess,$StandardPrice" +
                " from " + Tables.StockItem);
            if (isoptimised)
            {
                long alterID = getAlterId();
                Query.Append(" where $Alterid >" + alterID);
                fullUpdate = false;
            }
            try
            {
                response = await tallyCommunicator.getdatatable(Query.ToString());

                if (response.Rows.Count > 0)
                {

                    ProductProfileDTO productCategory = new ProductProfileDTO();
                    foreach (DataRow dr in response.Rows)
                    {
                        List<TaxMasterDTO> masterDTOs = new List<TaxMasterDTO>();
                        ProductProfileDTO productProfileDTO = new ProductProfileDTO();
                        productProfileDTO.productId = ((string)dr["$guid"]);

                        productProfileDTO.activated = true;
                        productProfileDTO.description = (dr["$parent"] != DBNull.Value) ? (string)dr["$parent"] : "";
                        productProfileDTO.name = (dr["$name"] != DBNull.Value) ? (string)dr["$name"] : "";

                        //Remove escape sequences
                        
                      //  string result = Regex.Replace(productProfileDTO.name, pattern, "");

                        string trimchar = string.Empty;

                        // Use a MatchEvaluator to capture the removed sequences
                        string result = Regex.Replace(productProfileDTO.name, pattern, match =>
                        {
                            // match.Groups[0].Value contains the captured escape sequence
                            // You can add it to the 'trimchar' string
                            trimchar += match.Groups[0].Value;

                            // You can remove the escape sequence from the original string by returning an empty string
                            return string.Empty;
                        });





                        productProfileDTO.name=result;
                        productProfileDTO.trimChar=trimchar;
                        productProfileDTO.alias = (dr["$_FirstAlias"] != DBNull.Value) ? (string)dr["$_FirstAlias"] : "";
                        //productProfileDTO.alias= (dr["$name"] != DBNull.Value) ? (string)dr["$name"] : "";
                       string caseof= (dr["$BaseUnits"] != DBNull.Value) ? (string)dr["$BaseUnits"] :"";
                        if(!String.IsNullOrEmpty(caseof) && caseof.Contains("case of"))
                        {

                            caseof = Regex.Replace(caseof, @"[^\d]", " ");
                            caseof = caseof.Trim();
                            caseof = Regex.Replace(caseof, @"\s+", " ");
                            productProfileDTO.alias=caseof;
                            productProfileDTO.sku="case";
                        }
                        else
                        {
                            productProfileDTO.sku=caseof;
                        }
                       
                        productProfileDTO.alterId = (dr["$alterid"] != DBNull.Value) ? (StringUtilsCustom.ExtractDoubleValue((dr["$alterid"].ToString()))) : 0;
                        productProfileDTO.mrp = (dr["$RateOfMrp"] != DBNull.Value) ? ((StringUtilsCustom.ExtractDoubleValue(dr["$RateOfMrp"].ToString()))) : 0;
                        productProfileDTO.price = (dr["$_LastSalePrice"] != DBNull.Value) ? StringUtilsCustom.ExtractDoubleValue(dr["$_LastSalePrice"].ToString()) : 0;
                        //changing price from $_LastSalePrice to $StandardPrice
                        productProfileDTO.price = (dr["$StandardPrice"] != DBNull.Value) ? StringUtilsCustom.ExtractDoubleValue(dr["$StandardPrice"].ToString()) : productProfileDTO.price;


                        productProfileDTO.taxRate = (dr["$_IntegratedTax"] != DBNull.Value) ? (StringUtilsCustom.ExtractDoubleValue(dr["$_IntegratedTax"].ToString())) : 0;
                        
                        if (productProfileDTO.taxRate==0)
                        {
                            productProfileDTO.taxRate = (dr["$RateOfVAT"] != DBNull.Value) ? (StringUtilsCustom.ExtractDoubleValue(dr["$RateOfVAT"].ToString())) : 0;
                        }
                        productProfileDTO.productCategoryName = (dr["$Category"] != DBNull.Value) ? (string)dr["$Category"] : "";
                        productProfileDTO.hsnCode = (dr["$_HSNCode"] != DBNull.Value)? (string)dr["$_HSNCode"] :"";
                        productProfileDTO.barcode = (dr["$PartNo"] != DBNull.Value) ? (string)dr["$PartNo"] : ""  ;
                        productProfileDTO.remarks = (dr["$Narration"] != DBNull.Value) ? (string)dr["$Narration"] : "";
                        productProfileDTO.unitQty = (dr["$Conversion"] != DBNull.Value) ? ((StringUtilsCustom.ExtractDoubleValue(dr["$Conversion"].ToString()))) : 1;
                        productProfileDTO.unitQty= productProfileDTO.unitQty==0 ? 1 : productProfileDTO.unitQty;
                        productProfileDTO.cessTaxRate = (dr["$_Cess"] != DBNull.Value) ? (StringUtilsCustom.ExtractDoubleValue(dr["$_Cess"].ToString())) : 0;
                        //if(dr["$_IntegratedTax"] != DBNull.Value)
                        //{
                        //    productProfileDTO.taxRate = ((double)dr["$_IntegratedTax"]);
                        //}

                        ProductProfileTaxMasterDTO productProfileTaxMasterDTO = new ProductProfileTaxMasterDTO(productProfileDTO);

                        if (gstItems!=null && gstItems.Length > 0)
                        {
                            TaxMasterDTO cgstMasterDTO = new TaxMasterDTO();
                            cgstMasterDTO.vatClass = gstItems[0];
                            cgstMasterDTO.vatPercentage = (dr["$_CentralTax"] != DBNull.Value) ? (StringUtilsCustom.ExtractDoubleValue(dr["$_CentralTax"].ToString())) : 0;
                            masterDTOs.Add(cgstMasterDTO);
                            //TaxMasterDTO sgstMasterDTO = new TaxMasterDTO();
                            //sgstMasterDTO.vatClass = gstItems[0];
                            //sgstMasterDTO.vatPercentage = (dr["$_CentralTax"] != DBNull.Value) ? ((double)dr["$_CentralTax"]) : 0;
                            //masterDTOs.Add(sgstMasterDTO);

                            TaxMasterDTO sgstMasterDTO = new TaxMasterDTO();
                            sgstMasterDTO.vatClass = gstItems[1];
                            sgstMasterDTO.vatPercentage = (dr["$_StateTax"] != DBNull.Value) ? (StringUtilsCustom.ExtractDoubleValue(dr["$_StateTax"].ToString())) : 0;
                            masterDTOs.Add(sgstMasterDTO);

                        }
                        //test code 
                      
                        if(_list.Where(x=>x.name==productProfileDTO.name).Count() == 0)
                        {
                            _list.Add(productProfileDTO);
                        }
                        else
                        {
                            duplicateProductslist.Add(productProfileDTO);
                        }
                       
                        productProfileTaxMasterDTO.productProfileTaxMasterDTOs = masterDTOs;
                        uploadPPTaxToServer.Add(productProfileTaxMasterDTO);
                    }
                    if (_list.Count > 0)
                    {
                        ENVELOPE tallyRequest = new ENVELOPE();
                        tallyRequest=getCompanyStockItemGSTWithCessRateXml();

                        var stringwriter = new System.IO.StringWriter();
                        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyRequest.GetType());
                        x.Serialize(stringwriter, tallyRequest);
                        TallyCommunicator tallyCommunicator=new TallyCommunicator();
                        TallyRequestResponse CompanyStockItemGSTWithCessRateResponseXml = await tallyCommunicator.ExecXml(stringwriter.ToString());
                        if (CompanyStockItemGSTWithCessRateResponseXml.response?.BODY?.DATA?.COLLECTION.STOCKITEM.Count>0)
                        {
                            //var dict = CompanyStockItemGSTWithCessRateResponseXml.response?.BODY?.DATA?.COLLECTION.STOCKITEM.ToDictionary(

                            //    obj => Regex.Replace(obj.NAME, pattern, ""),
                            //    obj => new StockItemHelper(obj.GSTDETAILSLIST?.Last()?.STATEWISEDETAILSLIST.RATEDETAILSLIST,obj.MRPDETAILSLIST?.Last()?.MRPRATEDETAILSLIST?.MRPRATE.Text));

                            var res1 = CompanyStockItemGSTWithCessRateResponseXml.response?.BODY?.DATA?.COLLECTION.STOCKITEM.GroupBy(obj => Regex.Replace(obj.NAME, pattern, "")).ToList();
                            var dict = CompanyStockItemGSTWithCessRateResponseXml.response?.BODY?.DATA?.COLLECTION.STOCKITEM.GroupBy(obj => Regex.Replace(obj.NAME, pattern, "")).ToDictionary(

                              obj => Regex.Replace(obj.Key, pattern, ""),
                              obj => new StockItemHelper(obj.SelectMany(item => item.GSTDETAILSLIST?.Last()?.STATEWISEDETAILSLIST.RATEDETAILSLIST).Distinct().ToList(), obj.Select(item => item.MRPDETAILSLIST?.Last()?.MRPRATEDETAILSLIST?.MRPRATE.Text).FirstOrDefault(""))
                              );


                            foreach (var dicItem in dict)
                            {
                                var statewiseGstLists = dicItem.Value?.RATEDETAILSLIST.GroupBy(obj=>obj.GSTRATEDUTYHEAD).ToDictionary(
                                    x => x.Key,
                                    x => x.Select(item=>item.GSTRATE)
                                    );
                                int index = _list.FindIndex(x => x.name==dicItem.Key);
                                string taxrate = "0";
                                IEnumerable<string> taxrates=new List<string>();
                                if (statewiseGstLists.ContainsKey(ProductIgstName))
                                {
                                    statewiseGstLists.TryGetValue(ProductIgstName, out taxrates);
                               
                                    taxrate=taxrates?.First();
                                }
                                else
                                {
                                    if (statewiseGstLists.ContainsKey("IGST"))
                                    {
                                        statewiseGstLists.TryGetValue("IGST", out taxrates);
                                        taxrate=taxrates?.First();
                                    }
                                }
                                //var taxlist= taxrates.First();




                                IEnumerable<string> cessrates = new List<string>();
                                string cessRate = "0";
                                if (statewiseGstLists.ContainsKey(ProductCessName))
                                {
                                    statewiseGstLists.TryGetValue(ProductCessName, out cessrates);
                                    cessRate=cessrates?.First();
                                }
                                
                                if (index>=0 && index<_list.Count)
                                {
                                    _list[index].taxRate=StringUtilsCustom.ExtractDoubleValue(taxrate);
                                    _list[index].cessTaxRate=StringUtilsCustom.ExtractDoubleValue(cessRate);
                                    _list[index].mrp=StringUtilsCustom.ExtractDoubleValue(dicItem.Value.mrpRate);
                                }
                            }
                        }

                            upload(_list);
                        
                    }
                    //if (uploadPPTaxToServer.Count > 0)
                    //{
                    //    uploadTaxMaster(uploadPPTaxToServer);
                    //}

                    if(duplicateProductslist.Count>0)
                    {
                        LogManager.WriteLog("Duplicate Products are : \n");
                        duplicateProductslist.ForEach(x =>
                        {
                            LogManager.WriteLog("==>"+x.name+"\n");
                        });
                    }


                }

            }catch(Exception ex)
            {
                LogManager.HandleException(ex);
                LogManager.WriteLog(ex.StackTrace);
                throw ex;
                throw new ServiceException(ex.Message, ex);
            }
        }

        private ENVELOPE getCompanyStockItemGSTWithCessRateXml()
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

            staticvariables.SVCURRENTCOMPANY = ApplicationProperties.properties["tally.company"].ToString();
            staticvariables.SVEXPORTFORMAT = "$$SysName:XML";

            desc.STATICVARIABLES = staticvariables;
            TDL tdl= new TDL();
            TDLMESSAGE tdlMessage=new TDLMESSAGE();
            COLLECTION cOLLECTION = new COLLECTION();
            cOLLECTION.NAME="All Stock Items";
            cOLLECTION.ISMODIFY="No";

            cOLLECTION.TYPE =new List<String>{
                "stock item"};
            cOLLECTION.FETCH="name,parent,GSTDETAILS.STATEWISEDETAILS,mrpdetails[Last].mrpratedetails[Last].mrprate";
            tdlMessage.COLLECTION=new List<COLLECTION> { cOLLECTION };
            tdl.TDLMESSAGE=tdlMessage;
            desc.TDL=tdl;
            body.DESC = desc;
            tallyRequest.BODY = body;

            return tallyRequest;
        }

        private void uploadTaxMaster(List<ProductProfileTaxMasterDTO> uploadPPTaxToServer)
        {
            string requestUri = ApiConstants.PREFIX + ApiConstants.PRODUCT_PROFILE_TAX_MASTER;

          
            LogManager.WriteLog("uploading PRODUCT_PROFILE_TAX_MASTER started...");
            httpClient = RestClientUtil.getClient();
            var myContent = JsonConvert.SerializeObject(uploadPPTaxToServer);
            HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

            var responseTask = httpClient.PostAsync(requestUri , inputContent);

            responseTask.Wait();

            HttpResponseMessage Res = responseTask.Result;
            LogManager.WriteResponseLog(Res);

            if (Res.IsSuccessStatusCode)
            {
                LogManager.WriteLog("request for uploading PRODUCT_PROFILE_TAX_MASTER Success..");
                var response = Res.Content.ReadAsStringAsync().Result;
            }
            else
            {
                LogManager.WriteLog("request for uploading PRODUCT_PROFILE_TAX_MASTER  Failed..");
                throw new ServiceException("PRODUCT PROFILE upload failed statuscode:" + Res.StatusCode + " Message : " + Res.RequestMessage);

            }
        }

        private void upload(List<ProductProfileDTO> list)
        {
            try
            {
                string requestUri = ApiConstants.PREFIX + ApiConstants.STOCK_ITEM;

                if (idClentApp.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    requestUri = ApiConstants.PREFIX + ApiConstants.STOCK_ITEM_ID;
                }
                LogManager.WriteLog("uploading PRODUCT PROFILE started...");
                httpClient = RestClientUtil.getClient();
                var myContent = JsonConvert.SerializeObject(list);
                LogManager.WriteLog(myContent);
                HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

                var responseTask = httpClient.PostAsync(requestUri + "/" + fullUpdate, inputContent);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteResponseLog(Res);

                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("request for uploading PRODUCT PROFILE  Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    LogManager.WriteLog("request for uploading PRODUCT PROFILE  Failed..");
                    throw new ServiceException("PRODUCT PROFILE upload failed statuscode:" + Res.StatusCode + " Message : " + Res.RequestMessage);
                }
            }catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw new ServiceException("Upload Product Profile to Server Failed...");
            }
        }

        private long getAlterId()
        {

            LogManager.WriteLog("get alterId in ProductProfileService Service started...");
            httpClient = RestClientUtil.getClient();
            var responseTask = httpClient.GetAsync(httpClient.BaseAddress +ApiConstants.PREFIX+ ApiConstants.ALTERID_MASTER + "/" + TallyMasters.PRODUCT_PROFILE);

            responseTask.Wait();

            HttpResponseMessage Res = responseTask.Result;
            LogManager.WriteResponseLog(Res);
            if (Res.IsSuccessStatusCode)
            {
                LogManager.WriteLog("request for alterId  Success..");
                var response = Res.Content.ReadAsStringAsync().Result;
                long responseAlterID = JsonConvert.DeserializeObject<long>(response);
                return responseAlterID;
            }
            throw new ServiceException("get alterId api call failed \n StatusCode : " + Res.StatusCode + " \n Response : " + Res.Content);
        }
    }
}
