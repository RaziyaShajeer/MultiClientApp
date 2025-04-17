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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    public class ReceivablePayableservice
    {

        TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        OpeningStockTallyMasterResponseParser openingStockTallyMasterResponseParser;
        private string idClentApp;
        OpeningStockService openingStockService;
        private  ReceivablePayableReportResponseParser receivablePayableReportResponseParser;
        public ReceivablePayableservice()
        {
            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            openingStockTallyMasterResponseParser = new OpeningStockTallyMasterResponseParser();
            idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
            openingStockService = new OpeningStockService();
            receivablePayableReportResponseParser = new ReceivablePayableReportResponseParser();
        }
        public async Task<bool> getFromTallyAndUploadAsync()
        {
            try
            {
                bool res = false;
                ENVELOPE tallyRequest = new ENVELOPE();
                tallyRequest = getCompanyBillReceivableXml("Bills Receivable");


                var stringwriter = new System.IO.StringWriter();
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyRequest.GetType());
                x.Serialize(stringwriter, tallyRequest);

                var BillsReceivablesResXml = await tallyCommunicator.ExecXmlAndGetXmlAsync(stringwriter.ToString());

                ENVELOPE tallyRequestForBilsPayable = new ENVELOPE();
                tallyRequestForBilsPayable = getCompanyBillReceivableXml("Bills Payable");


                var stringwriterForPayable = new System.IO.StringWriter();
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(tallyRequestForBilsPayable.GetType());
                xmlSerializer.Serialize(stringwriterForPayable, tallyRequestForBilsPayable);

                var BillsPayablesResXml = await tallyCommunicator.ExecXmlAndGetXmlAsync(stringwriterForPayable.ToString());

                ReceivablePayableReportResponseParser receivablePayableReportResponseParser = new ReceivablePayableReportResponseParser();

                HashSet<string> ledgerNames = receivablePayableReportResponseParser.parseReceivablePayableXmlToLedgerNames(BillsReceivablesResXml, ReceivablePayableType.Receivable);
                List<ReceivablePayableDTO> pToServer = receivablePayableReportResponseParser.parseReceivablePayableDTOsXml(BillsPayablesResXml, ReceivablePayableType.Payable);
                List<ReceivablePayableDTO> rpToServer = await findLedgerWiseOutStandingAsync(ledgerNames);

                foreach (var pDto in pToServer)
                {
                    var opReceivalbes = rpToServer.Where(r => r.referenceDocumentNumber.Equals( pDto.referenceDocumentNumber) && pDto.referenceDocumentAmount == 0).FirstOrDefault();
                   // var duplicates = rpToServer.Where(r => r.referenceDocumentNumber.Equals(pDto.referenceDocumentNumber,StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (opReceivalbes != null)
                    {
                       var index= pToServer.IndexOf(pDto);
                        rpToServer.RemoveAll(r => r.referenceDocumentNumber.Equals( pDto.referenceDocumentNumber) && pDto.referenceDocumentAmount == 0);
                        pDto.referenceDocumentAmount = pDto.referenceDocumentBalanceAmount;
                       // pToServer[index].referenceDocumentAmount = pDto.referenceDocumentBalanceAmount;
                    }
                }

                rpToServer.AddRange(pToServer);
                List<ReceivablePayableDTO> receivablePayableDTOToServer = new();

				if (rpToServer.Count > 0)
                {
					TallyService tallyService= new TallyService();
					List<AccountProfileDTO> allAccountProfilespTally= await tallyService.getAllLedgers();


                    //				var accountProfileMap = allAccountProfilespTally
                    //.ToDictionary(data => data.name, data => data.customerId);

                    var accountProfileMap = allAccountProfilespTally
    .GroupBy(data => data.name)
    .ToDictionary(group => group.Key, group => group.First().customerId);

                    receivablePayableDTOToServer = rpToServer
						.Where(pDto => accountProfileMap.ContainsKey(pDto.accountName))
						.Select(pDto =>
						{
							pDto.customerId = accountProfileMap[pDto.accountName];
							return pDto;
						})
						.ToList();

					res = upload(receivablePayableDTOToServer);
                }
                else
                {
                    LogManager.WriteLog("No recevable payable found to upload..");
                }



                return res;
            }catch(Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
            return false;
        }

        private bool upload(List<ReceivablePayableDTO> list)
        {
            try
            {
                LogManager.WriteLog("uploading receivable-payable  To Server ....");

                string requestUri = ApiConstants.PREFIX + ApiConstants.RECEIVALE_PAYABLES;//SNR_CLIENT_APP_RP_1

				if (idClentApp.Equals("true"))
                {
                    requestUri = ApiConstants.PREFIX + ApiConstants.RECEIVALE_PAYABLES_ID;
                }


                LogManager.WriteLog("uploading  receivable-payable   started...\n" + "Api  : " + requestUri);
                httpClient = RestClientUtil.getClient();
                var myContent = JsonConvert.SerializeObject(list);
                LogManager.WriteLog(myContent);
                HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

                var responseTask = httpClient.PostAsync(requestUri, inputContent);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteLog("Uploading receivable-payable   To Server Completed ....\n Response : ");
                LogManager.WriteResponseLog(Res);

                
                
                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("request for uploading  receivable-payable     Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                    return true;
                }
                else
                {
                    LogManager.WriteLog("request for uploading receivable-payable     Failed..");
                    throw new ServiceException("receivable-payable  upload failed statuscode:" + Res.StatusCode + " Message : " + Res.RequestMessage);

                    return false;
                }
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex, "request for uploading receivable-payable Failed due to exception : \n"+ex.Message);
                throw ex;
            }
            return false;
        }

        private async Task<List<ReceivablePayableDTO>> findLedgerWiseOutStandingAsync(HashSet<string> ledgerNames)
        {
            Dictionary<String, String> outStandingResponseXMLs = new Dictionary<String, String>();
            int i = 1;
            foreach (String name in ledgerNames)
            {
                if (!name.StartsWith("#") && !name.StartsWith("@") && !name.StartsWith("$"))
                {
                    ENVELOPE tallyRequest = new ENVELOPE();
                    tallyRequest = getLedgerWiseOutstandingXml(name);


                    var stringwriter = new System.IO.StringWriter();
                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyRequest.GetType());
                    x.Serialize(stringwriter, tallyRequest);

                    var BillsReceivablesResXml = await tallyCommunicator.ExecXmlAndGetXmlAsync(stringwriter.ToString());
                    if(BillsReceivablesResXml != null)
                    {
                        outStandingResponseXMLs.TryAdd(name, BillsReceivablesResXml);
                    }

                }

            }
            return parseOutstanding(outStandingResponseXMLs);
        }

        private List<ReceivablePayableDTO> parseOutstanding(Dictionary<string, string> outStandingResponseXMLs)
        {
            List<ReceivablePayableDTO> rToServer = new List<ReceivablePayableDTO>();
           
            foreach (var responseXML in outStandingResponseXMLs)
            {
                if (responseXML.Value.Contains("????????"))
                {
                    continue;
                }

                List<ReceivablePayableDTO> parsedData = receivablePayableReportResponseParser.parseReceivablePayableDTOsXml(responseXML.Value, ReceivablePayableType.Receivable, responseXML.Key);
            rToServer.AddRange(parsedData);


            }
            return rToServer;
        }

        private ENVELOPE getLedgerWiseOutstandingXml(string name)
        {

            ENVELOPE tallyRequest = new ENVELOPE();
            HEADER header = new HEADER();
           
            header.TALLYREQUEST = "Export Data";
           
            tallyRequest.HEADER = header;

            BODY body = new();
            EXPORTDATA exportdata = new();
            REQUESTDESC requestDesc = new();

            STATICVARIABLES staticvariables = new STATICVARIABLES();

            staticvariables.SVCURRENTCOMPANY = ApplicationProperties.properties["tally.company"].ToString();
            staticvariables.SVEXPORTFORMAT = "$$SysName:XML";
            staticvariables.LEDGERNAME = name;
            requestDesc.STATICVARIABLES = staticvariables;
            requestDesc.REPORTNAME = "Ledger Outstandings";

            exportdata.REQUESTDESC = requestDesc;
            body.EXPORTDATA = exportdata;
            tallyRequest.BODY = body;

            return tallyRequest;
        }

        private ENVELOPE getCompanyBillReceivableXml(String requestId)
        { 
            ENVELOPE tallyRequest = new ENVELOPE();
            HEADER header = new HEADER();
            header.VERSION = "1";
            header.TALLYREQUEST = "Export";
            header.TYPE = "Data";
            header.ID = requestId;
            tallyRequest.HEADER = header;

            BODY body = new();
            DESC desc = new();
            STATICVARIABLES staticvariables = new STATICVARIABLES();
      
            staticvariables.SVCURRENTCOMPANY = ApplicationProperties.properties["tally.company"].ToString();
            staticvariables.SVEXPORTFORMAT = "$$SysName:XML";
          
            desc.STATICVARIABLES = staticvariables;
            
            body.DESC = desc;
            tallyRequest.BODY = body;

            return tallyRequest;
        }
    }
}
