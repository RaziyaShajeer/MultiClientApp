using dtos;
using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Parsers;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
using SNR_ClientApp.Tally.generateXml;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    public class ReceiptUploadService
    {
        readonly TallyCommunicator tallyCommunicator;
        private ReceiptVoucherTallyMasterResponceMaster receiptVoucherTallyMasterResponceMaster;
        HttpClient httpClient;
        private bool fullUpdate = true;
        private string idClentApp;
        private String tallyLedgerParent;
        public string companyName = ApplicationProperties.properties["tally.company"].ToString();
        public ReceiptUploadService()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
            tallyLedgerParent = ApplicationProperties.properties.GetValueOrDefault("tally.ledger.parent").ToString();
            receiptVoucherTallyMasterResponceMaster = new ReceiptVoucherTallyMasterResponceMaster();
        }
        internal async Task getFromTallyAndUploadAsync(string date)
        {
            try
            {
               
                DataTable response = new DataTable();
                StringBuilder Query = new StringBuilder();
                String vouchertypeName = ApplicationProperties.properties["receiptVoucherType"].ToString();
                Query.Append("select $name,$Parent  from " + Tables.VoucherType + " where $Parent= " + vouchertypeName);

                response = await tallyCommunicator.getdatatable(Query.ToString());

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
                        // selected date generated sales Ledger names.
                        TallyMastersRequestXml tallyMastersRequestXml = new TallyMastersRequestXml();
                        ENVELOPE tallyrequest = tallyMastersRequestXml.getDayWiseReceiptLedgerNames(companyName, date, voucherTypes);


                        var stringwriter = new System.IO.StringWriter();
                        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyrequest.GetType());
                        x.Serialize(stringwriter, tallyrequest);

                        var currntDayLedgerNames = await tallyCommunicator.ExecXmlAndGetXmlAsync(stringwriter.ToString());
                        SalesVoucherLedgerTallymasterResponseParser salesVoucherLedgerTallymasterResponseParser = new SalesVoucherLedgerTallymasterResponseParser();

                        List<AccountProfileDTO> accountProfileDTOs = salesVoucherLedgerTallymasterResponseParser.ParseSalesVoucherLedgerListXml(currntDayLedgerNames);
                        List<AccountingVoucherHeaderDTO> accountingVoucherHeaderDTOs = new List<AccountingVoucherHeaderDTO>();
                        TallyResponse res = new TallyResponse();
                        int i = 0;
                        foreach(var accountProfileDTO in accountProfileDTOs)
                        {
                            String ledgerName = StringUtilsCustom
                                .replaceSpecialCharactersWithXmlValue(accountProfileDTO.name);
                            if (ledgerName != null && !ledgerName.Equals("")
                            && !ledgerName.Equals("Cash",StringComparison.OrdinalIgnoreCase))
                            {

                                if (!ledgerName.Contains("BANK",StringComparison.OrdinalIgnoreCase) && !ledgerName.Contains("Suspense", StringComparison.OrdinalIgnoreCase))
                                {
                                    i++;
                                   LogManager.WriteLog(i + "th Ledger" + "----------" + ledgerName);
                                    TallyMastersRequestXml tallyMastersRequestXml1 = new TallyMastersRequestXml();
                                   var tallyReq = tallyMastersRequestXml1.getReceptVoucherXml(CompanyService.getCompanyName(),
                                                    date, ledgerName, voucherTypes);


                                    var stringwriter1 = new System.IO.StringWriter();
                                    //System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyReq.GetType());
                                    x.Serialize(stringwriter1, tallyReq);

                                    var res2= tallyCommunicator.ExecXml(stringwriter1.ToString());
                                    //// System.out.println(response.getBody().toString()+"----------------------------");
                                    List<AccountingVoucherHeaderDTO> accountingVoucherHeaderDTO= receiptVoucherTallyMasterResponceMaster
                                            .parseSalesVoucherListXml(res2.Result);
                                    if(accountingVoucherHeaderDTO!=null)
                                    accountingVoucherHeaderDTOs.AddRange(accountingVoucherHeaderDTO);
                                }
                            }
                        }
                        if (accountingVoucherHeaderDTOs.Count>0)
                        {
                          
                            upload(accountingVoucherHeaderDTOs);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
           // return null;
        }

        private void upload(List<AccountingVoucherHeaderDTO> list)
        {
            string requestUri =  ApiConstants.UPLOAD_ACCOUNTING_VOUCHERS;

            try
            {
                LogManager.WriteLog("uploading UPLOAD_ACCOUNTING_VOUCHERS started...");
                httpClient = RestClientUtil.getClient();
                var myContent = JsonConvert.SerializeObject(list);
                HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

                var responseTask = httpClient.PostAsync(requestUri, inputContent);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteResponseLog(Res);

                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("request for uploading UPLOAD_ACCOUNTING_VOUCHERS  Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    LogManager.WriteLog("request for uploading UPLOAD_ACCOUNTING_VOUCHERS  Failed..");
                    throw new ServiceException("uploading UPLOAD_ACCOUNTING_VOUCHERS  Failed");
                }
            }catch(Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
        }
    }
}
