using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally.generateXml;
using SNR_ClientApp.Tally;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dtos;
using SNR_ClientApp.Parsers;
using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.Exceptions;

namespace SNR_ClientApp.Services
{
    public class SalesUploadService
    {
        readonly TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        private bool fullUpdate = true;
        private string idClentApp;
        private String tallyLedgerParent;
        public string companyName = ApplicationProperties.properties["tally.company"].ToString();
        private SalesVoucherLedgerTallymasterResponseParser salesVoucherLedgerTallymasterResponseParser;
        private SalesVoucherTallyMasterResponceMaster salesVoucherTallyMasterResponceMaster;
        public SalesUploadService()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
            tallyLedgerParent = ApplicationProperties.properties.GetValueOrDefault("tally.ledger.parent").ToString();
            salesVoucherLedgerTallymasterResponseParser = new SalesVoucherLedgerTallymasterResponseParser();
            salesVoucherTallyMasterResponceMaster = new SalesVoucherTallyMasterResponceMaster();
        }
        internal async Task getFromTallyAndUploadAsync(string date)
        {
            try
            {

                DataTable response = new DataTable();
                StringBuilder Query = new StringBuilder();
                String vouchertypeName = ApplicationProperties.properties["salesVoucherType"].ToString();
                Query.Append("select $name,$Parent  from " + Tables.VoucherType + " where $Parent= " + vouchertypeName);

                response = await tallyCommunicator.getdatatable(Query.ToString());

                if (response.Rows.Count > 0)
                {
                 //   List<InventoryVoucherHeaderDTO> inventoryVoucherHeaderDTOs = new List<InventoryVoucherHeaderDTO>();
                    List <VoucherTypeDTO> voucherTypes = new List<VoucherTypeDTO>();

                    foreach (DataRow dr in response.Rows)
                    {
                        VoucherTypeDTO dto = new VoucherTypeDTO();
                        dto.parent = (dr["$Parent"] != DBNull.Value) ? (string)dr["$Parent"] : "";
                        dto.name = (dr["$name"] != DBNull.Value) ? (string)dr["$name"] : "";


                        voucherTypes.Add(dto);

                    }
                    if (voucherTypes.Count > 0)
                    {
                        // selected date generated sales Ledger names.
                        TallyMastersRequestXml tallyMastersRequestXml = new TallyMastersRequestXml();
                        ENVELOPE tallyrequest = tallyMastersRequestXml.getDayWiseReceiptLedgerNames(companyName, date, voucherTypes);


                        var stringwriter = new System.IO.StringWriter();
                        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyrequest.GetType());
                        x.Serialize(stringwriter, tallyrequest);

                        var currntDayLedgerNames = await tallyCommunicator.ExecXmlAndGetXmlAsync(stringwriter.ToString());
                        List<AccountProfileDTO> accountProfileDTOs = salesVoucherLedgerTallymasterResponseParser
                            .ParseSalesVoucherLedgerListXml(currntDayLedgerNames);
                        List<InventoryVoucherHeaderDTO> inventoryVoucherHeaderDTOs = new List<InventoryVoucherHeaderDTO>();

                        foreach (var accountProfileDTO in accountProfileDTOs)
                        {
                            String ledgerName = StringUtilsCustom
                                .replaceSpecialCharactersWithXmlValue(accountProfileDTO.name);
                            if (ledgerName != null && !ledgerName.Equals("")
                            && !ledgerName.Equals("Cash", StringComparison.OrdinalIgnoreCase))
                            {

                              
                                    LogManager.WriteLog(  "th Ledger" + "----------" + ledgerName);
                                    TallyMastersRequestXml tallyMastersRequestXml1 = new TallyMastersRequestXml();
                                    var tallyReq = tallyMastersRequestXml1.getSalesVoucherXml(CompanyService.getCompanyName(),
                                                     date, ledgerName, voucherTypes);


                                    var stringwriter1 = new System.IO.StringWriter();
                                    //System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyReq.GetType());
                                    x.Serialize(stringwriter1, tallyReq);

                                    var res2 = tallyCommunicator.ExecXml(stringwriter1.ToString());
                                //// System.out.println(response.getBody().toString()+"----------------------------");
                                List<InventoryVoucherHeaderDTO> inventoryVoucherHeaderDTO = salesVoucherTallyMasterResponceMaster
                                        .parseSalesVoucherListXml(res2.Result);

                                inventoryVoucherHeaderDTOs.AddRange(inventoryVoucherHeaderDTO);

                            }
                        }
                        if (inventoryVoucherHeaderDTOs.Count > 0)
                        {

                            upload(inventoryVoucherHeaderDTOs);
                        }


                    }




                }
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;

            }
        }

        private void upload(List<InventoryVoucherHeaderDTO> inventoryVoucherHeaderDTOs)
        {
            try { 
            string requestUri = ApiConstants.UPLOAD_INVENTORY_VOUCHERS;


            LogManager.WriteLog("Uploading inventory Voucher data to server size.."+ inventoryVoucherHeaderDTOs.Count);
            httpClient = RestClientUtil.getClient();
            var myContent = JsonConvert.SerializeObject(inventoryVoucherHeaderDTOs);
            HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

            var responseTask = httpClient.PostAsync(requestUri, inputContent);

            responseTask.Wait();

            HttpResponseMessage Res = responseTask.Result;
            LogManager.WriteResponseLog(Res);

            if (Res.IsSuccessStatusCode)
            {
                LogManager.WriteLog("request for uploading UPLOAD_INVENTORY_VOUCHERS  Success..");
                var response = Res.Content.ReadAsStringAsync().Result;
            }
            else
            {
                LogManager.WriteLog("request for uploading UPLOAD_INVENTORY_VOUCHERS  Failed..");
                throw new ServiceException("request for uploading UPLOAD_INVENTORY_VOUCHERS  Failed..");
            }
            }catch(Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
        }
    }
}
