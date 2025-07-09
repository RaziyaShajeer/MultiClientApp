using dtos;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using ProductProfileDTO = SNR_ClientApp.DTO.ProductProfileDTO;


namespace SNR_ClientApp.Parsers
{
    public class ProductProfileParser
    {
        private String GSTNames = ApplicationProperties.properties.GetValueOrDefault("tally.gst").ToString();
        private String ProductIgstName = ApplicationProperties.properties.GetValueOrDefault("tally.productIGST").ToString();
        private String ProductCessName = ApplicationProperties.properties.GetValueOrDefault("tally.productCESS").ToString();

        public async Task<List<ProductProfileDTO>> getProductprofiles(string tallyResponseXml)
        {
            try
            {
                List<ProductProfileTaxMasterDTO> uploadPPTaxToServer = new List<ProductProfileTaxMasterDTO>();
                List<ProductProfileDTO> duplicateProductslist = new List<ProductProfileDTO>();
                List<ProductProfileDTO> _list = new List<ProductProfileDTO>();
                string pattern = @"[\r\n\t]+$";
                var gstItems = GSTNames.Split(",");
                if (string.IsNullOrEmpty(GSTNames))
                {
                    gstItems = null;
                }
                List<ProductProfileDTO> allproductprofiles = new List<ProductProfileDTO>();
                tallyResponseXml = tallyResponseXml.Replace("&#13;", "")
                                   .Replace("&#10;", "")
                                   .Replace("&#4;", "").Replace("&apos;", "'")
                                   .Replace("&", "").Replace("\u0004", "");
                var doc = XDocument.Parse(tallyResponseXml);
                var productprofilelist = doc.Descendants("STOCKITEM");
                int ledgerCount = productprofilelist.Count();
                foreach (var productprofile in productprofilelist)
                {
                    List<TaxMasterDTO> masterDTOs = new List<TaxMasterDTO>();
                    ProductProfileDTO productProfileDTO = new ProductProfileDTO();

                    productProfileDTO.productId = productprofile.Element("GUID")?.Value ?? "";
                    productProfileDTO.activated = true;
                    productProfileDTO.description = productprofile.Element("PARENT")?.Value ?? "";
                    productProfileDTO.name = productprofile.Attribute("NAME")?.Value ?? "";
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
                    productProfileDTO.name = result;
                    productProfileDTO.trimChar = trimchar;
                    productProfileDTO.alias = productprofile.Element("_FIRSTALIAS")?.Value ?? "";
                    string caseof = productprofile.Element("BASEUNITS")?.Value ?? "";
                    if (!String.IsNullOrEmpty(caseof) && caseof.Contains("case of"))
                    {

                        caseof = Regex.Replace(caseof, @"[^\d]", " ");
                        caseof = caseof.Trim();
                        caseof = Regex.Replace(caseof, @"\s+", " ");
                        productProfileDTO.alias = caseof;
                        productProfileDTO.sku = "case";
                    }
                    else
                    {
                        productProfileDTO.sku = caseof;
                    }
                    string alterId = productprofile.Element("ALTERID")?.Value ?? "0";
                    productProfileDTO.alterId = Double.Parse(alterId);
                    string mrp = productprofile.Element("RATEOFMRP")?.Value ?? "0";
                    productProfileDTO.mrp = double.Parse(mrp);
                    string lastsellingPrice = productprofile.Element("_LASTSALEPRICE")?.Value ?? "0";
                    if(lastsellingPrice=="")
                    {
                        lastsellingPrice = "0";
                    }
                    productProfileDTO.price = double.Parse(lastsellingPrice);
                    LogManager.WriteLog(productProfileDTO.price.ToString());
                    string price =( productprofile.Element("STANDARDPRICE")?.Value) ?? lastsellingPrice;
                    if (string.IsNullOrWhiteSpace(price))
                    {
                        price = "0";
                    }
                    else
                    {
                        // Extract numeric part (e.g., 139.56 from "139.56/Nos.")
                        var match = Regex.Match(price, @"[\d.]+");
                        if (match.Success)
                        {
                            price = match.Value;
                        }
                        else
                        {
                            price = "0";
                        }
                    }

                    LogManager.WriteLog(price);

                    productProfileDTO.price = double.Parse(price);
                    LogManager.WriteLog(price);
                    productProfileDTO.price = double.Parse(price);

                    string integratedTax = productprofile.Element("_INTEGRATEDTAX")?.Value ?? "0";

                    productProfileDTO.taxRate = Double.Parse(integratedTax);
                    if (productProfileDTO.taxRate == 0)
                    {
                        string taxrate = productprofile.Element("RATEOFVAT")?.Value ?? "0";

                        productProfileDTO.taxRate = double.Parse(taxrate);

                    }
                    productProfileDTO.productCategoryName = productprofile.Element("CATEGORY")?.Value ?? "";
                    productProfileDTO.hsnCode = productprofile.Element("_HSNCODE")?.Value ?? "0";
                    productProfileDTO.barcode = productprofile.Element("PARTNUMBER")?.Value ?? "0";
                    productProfileDTO.barcode = productprofile.Element("PARTNUMBER")?.Value ?? "0";
                    productProfileDTO.remarks = productprofile.Element("NARRATION")?.Value ?? "";
                    string unitQty = productprofile.Element("CONVERSION")?.Value ?? "1";
                    productProfileDTO.unitQty = double.Parse(unitQty);
                    productProfileDTO.unitQty = productProfileDTO.unitQty == 0 ? 1 : productProfileDTO.unitQty;
                    string Cess = productprofile.Element("_CESS")?.Value ?? "0";
                    productProfileDTO.cessTaxRate = double.Parse(Cess);
                    ProductProfileTaxMasterDTO productProfileTaxMasterDTO = new ProductProfileTaxMasterDTO(productProfileDTO);
                    if (gstItems != null && gstItems.Length > 0)
                    {
                        TaxMasterDTO cgstMasterDTO = new TaxMasterDTO();
                        cgstMasterDTO.vatClass = gstItems[0];
                        string vatPercentage = productprofile.Element("_CENTRALTAX")?.Value ?? "0";
                        masterDTOs.Add(cgstMasterDTO);
                        //TaxMasterDTO sgstMasterDTO = new TaxMasterDTO();
                        //sgstMasterDTO.vatClass = gstItems[0];
                        //sgstMasterDTO.vatPercentage = (dr["$_CentralTax"] != DBNull.Value) ? ((double)dr["$_CentralTax"]) : 0;
                        //masterDTOs.Add(sgstMasterDTO);

                        TaxMasterDTO sgstMasterDTO = new TaxMasterDTO();
                        sgstMasterDTO.vatClass = gstItems[1];
                        string vatpercentage = productprofile.Element("STATETAX")?.Value ?? "0";
                        sgstMasterDTO.vatPercentage = double.Parse(vatPercentage);
                        masterDTOs.Add(sgstMasterDTO);

                    }
                    if (_list.Where(x => x.name == productProfileDTO.name).Count() == 0)
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
                    tallyRequest = getCompanyStockItemGSTWithCessRateXml();
                    if (_list.Count > 0)
                    {
                        ENVELOPE tallyRequest1 = new ENVELOPE();
                        tallyRequest = getCompanyStockItemGSTWithCessRateXml();
                        var stringwriter = new System.IO.StringWriter();
                        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyRequest.GetType());
                        x.Serialize(stringwriter, tallyRequest);
                        TallyCommunicator tallyCommunicator = new TallyCommunicator();
                        TallyRequestResponse CompanyStockItemGSTWithCessRateResponseXml = await tallyCommunicator.ExecXml(stringwriter.ToString());
                        if (CompanyStockItemGSTWithCessRateResponseXml.response?.BODY?.DATA?.COLLECTION.STOCKITEM.Count > 0)
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
                                var statewiseGstLists = dicItem.Value?.RATEDETAILSLIST.GroupBy(obj => obj.GSTRATEDUTYHEAD).ToDictionary(
                                    x => x.Key,
                                    x => x.Select(item => item.GSTRATE)
                                    );
                                int index = _list.FindIndex(x => x.name == dicItem.Key);
                                string taxrate = "0";
                                IEnumerable<string> taxrates = new List<string>();
                                if (statewiseGstLists.ContainsKey(ProductIgstName))
                                {
                                    statewiseGstLists.TryGetValue(ProductIgstName, out taxrates);

                                    taxrate = taxrates?.First();
                                }
                                else
                                {
                                    if (statewiseGstLists.ContainsKey("IGST"))
                                    {
                                        statewiseGstLists.TryGetValue("IGST", out taxrates);
                                        taxrate = taxrates?.First();
                                    }
                                }
                                //var taxlist= taxrates.First();




                                IEnumerable<string> cessrates = new List<string>();
                                string cessRate = "0";
                                if (statewiseGstLists.ContainsKey(ProductCessName))
                                {
                                    statewiseGstLists.TryGetValue(ProductCessName, out cessrates);
                                    cessRate = cessrates?.First();
                                }

                                if (index >= 0 && index < _list.Count)
                                {
                                    _list[index].taxRate = StringUtilsCustom.ExtractDoubleValue(taxrate);
                                    _list[index].cessTaxRate = StringUtilsCustom.ExtractDoubleValue(cessRate);
                                    _list[index].mrp = StringUtilsCustom.ExtractDoubleValue(dicItem.Value.mrpRate);
                                }
                            }
                        }


                        if (duplicateProductslist.Count > 0)
                        {
                            LogManager.WriteLog("Duplicate Products are : \n");
                            duplicateProductslist.ForEach(x =>
                            {
                                LogManager.WriteLog("==>" + x.name + "\n");
                            });
                        }



                    }

                }
                return _list;

            }
            catch(Exception ex)
            {
                LogManager.WriteLog(ex.Message);
                throw ex;
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
            TDL tdl = new TDL();
            TDLMESSAGE tdlMessage = new TDLMESSAGE();
            COLLECTION cOLLECTION = new COLLECTION();
            cOLLECTION.NAME = "All Stock Items";
            cOLLECTION.ISMODIFY = "No";

            cOLLECTION.TYPE = new List<String>{
                "stock item"};
            cOLLECTION.FETCH = "name,parent,GSTDETAILS.STATEWISEDETAILS,mrpdetails[Last].mrpratedetails[Last].mrprate";
            tdlMessage.COLLECTION = new List<COLLECTION> { cOLLECTION };
            tdl.TDLMESSAGE = tdlMessage;
            desc.TDL = tdl;
            body.DESC = desc;
            tallyRequest.BODY = body;

            return tallyRequest;
        }
    }
}


           


        
