using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    public class PostDatedVoucherService
    {
        TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        public PostDatedVoucherService()
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


                header.TALLYREQUEST = "Export Data";

                tallyRequest.HEADER = header;

                BODY body = new();
                EXPORTDATA exportdata = new EXPORTDATA();
                REQUESTDESC rEQUESTDESC = new REQUESTDESC();

                STATICVARIABLES staticvariables = new STATICVARIABLES();

                staticvariables.SVCURRENTCOMPANY = ApplicationProperties.properties["tally.company"].ToString();
                rEQUESTDESC.REPORTNAME = "post dated vouchers";
                rEQUESTDESC.STATICVARIABLES = staticvariables;
                exportdata.REQUESTDESC = rEQUESTDESC;
                body.EXPORTDATA = exportdata;



                tallyRequest.BODY = body;



                var stringwriter = new System.IO.StringWriter();
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyRequest.GetType());
                x.Serialize(stringwriter, tallyRequest);

                TallyRequestResponse data = await tallyCommunicator.ExecXml(stringwriter.ToString());

                List<PostDatedVoucherDTO> pcToServer = PostDatedVoucherResponseParser(data);

                Dictionary<String, List<PostDatedVoucherAllocationDTO>> voucherAllocationMap = parsePostDatedVoucherAlloc(data);
                if (pcToServer.Count>0 )
                {
                    foreach (var item in pcToServer)
                    {
                        if (voucherAllocationMap.ContainsKey(item.referenceDocumentNumber))
                        {

                            item.postDatedVoucherAllocationList = voucherAllocationMap[item.referenceDocumentNumber];
                        }
                    }


                    upload(pcToServer);
                }




            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
        }

        private void upload(List<PostDatedVoucherDTO> pcToServer)
        {

            try
            {
                LogManager.WriteLog("Uploading Post Dated Voucher To Server Started....");


                string requestUri = ApiConstants.PREFIX + ApiConstants.POST_DATED_VOUCHERS;

                LogManager.WriteLog("uploading POST_DATED_VOUCHERS started...\n" + "Api  : " + requestUri);
                httpClient = RestClientUtil.getClient();
                var myContent = JsonConvert.SerializeObject(pcToServer);
                HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

                var responseTask = httpClient.PostAsync(requestUri, inputContent);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteLog("Uploading POST_DATED_VOUCHERS To Server Completed ....\n Response : ");
                LogManager.WriteResponseLog(Res);

                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("request for uploading POST_DATED_VOUCHERS  Success..");
                    var response = Res.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    LogManager.WriteLog("request for uploading POST_DATED_VOUCHERS  Failed..");
                }
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                LogManager.WriteLog("request for uploading POST_DATED_VOUCHERS  Failed.. unexpected exception occured while uploading data to server");
                throw new ServiceException(ex.Message);
            }


        }

        private Exception ServiceException()
        {
            throw new NotImplementedException();
        }

        private Dictionary<string, List<PostDatedVoucherAllocationDTO>> parsePostDatedVoucherAlloc(TallyRequestResponse data)
        {
            Dictionary<String, List<PostDatedVoucherAllocationDTO>> voucherPdcMap = new Dictionary<string, List<PostDatedVoucherAllocationDTO>>();
            
            PostDatedVoucherAllocationDTO postDatedVoucherAllocationDto = new PostDatedVoucherAllocationDTO();
          
            
            string voucherTypes = ApplicationProperties.properties["pdc.vouchertype"].ToString();

            List<String> pdcVoucherTypes = voucherTypes.ToLower().Split(",").ToList();

            foreach (var item in data.response.BODY.IMPORTDATA.REQUESTDATA.TALLYMESSAGE)
            {
                //if (item.VOUCHER.VOUCHERTYPENAME == ApplicationProperties.properties["pdc.vouchertype"].ToString())
                if (item.VOUCHER!=null)
                {
                    List<PostDatedVoucherAllocationDTO> postDatedVoucherAllocationDTOs = new List<PostDatedVoucherAllocationDTO>();
                    String voucherTypeName = "";
                    voucherTypeName = item.VOUCHER.VOUCHERTYPENAME;
                    if (pdcVoucherTypes.Contains(voucherTypeName.ToLower()))
                    {
                        String voucherNumber = null;
                        voucherNumber = item.VOUCHER.VOUCHERNUMBER;
                        postDatedVoucherAllocationDto = new PostDatedVoucherAllocationDTO();
                        postDatedVoucherAllocationDto.voucherNumber=voucherNumber;
                        

                        //PostDatedVoucherDTO postDatedVoucherDto = new PostDatedVoucherDTO();
                        //postDatedVoucherDto.accountProfileName = item.VOUCHER.PARTYLEDGERNAME;
                        //postDatedVoucherDto.referenceDocumentDate = ConvertToDateTime(item.VOUCHER.DATE);
                        //postDatedVoucherDto.referenceDocumentNumber = item.VOUCHER.VOUCHERNUMBER;
                        foreach (var bankalledgerEntry in item.VOUCHER.ALLLEDGERENTRIES_LIST)
                        {
                            //postDatedVoucherDto.instrumentNumber = bankalledgerEntry.BANKALLOCATIONS_LIST.INSTRUMENTDATE;
                            //postDatedVoucherDto.instrumentDate = ConvertToDateTime(bankalledgerEntry.BANKALLOCATIONS_LIST.INSTRUMENTDATE);
                            //postDatedVoucherDto.pdcReceiptDate = ConvertToDateTime(bankalledgerEntry.BANKALLOCATIONS_LIST.PDCACTUALDATE);
                            //postDatedVoucherDto.referenceDocumentAmount = bankalledgerEntry.BANKALLOCATIONS_LIST.AMOUNT;
                            //postDatedVoucherDto.remark = bankalledgerEntry.BANKALLOCATIONS_LIST.NARRATION;

                            //postDatedVoucherDto.referenceDocumentNumber = bankalledgerEntry.BANKALLOCATIONS_LIST.NARRATION;
                            //postDatedVoucherDto.referenceVoucher = bankalledgerEntry.BANKALLOCATIONS_LIST.NAME;


                            postDatedVoucherAllocationDto.allocReferenceVoucher = bankalledgerEntry.BANKALLOCATIONSLIST.NAME;
                            postDatedVoucherAllocationDto.allocReferenceVoucherAmount = bankalledgerEntry.BANKALLOCATIONSLIST.AMOUNT;
                        }

                        PostDatedVoucherAllocationDTO pdcVoucherAlloc = new PostDatedVoucherAllocationDTO(postDatedVoucherAllocationDto);

                        postDatedVoucherAllocationDTOs.Add(pdcVoucherAlloc);
                        voucherPdcMap.Add(voucherNumber, postDatedVoucherAllocationDTOs);

                    }
                }
            }
            return voucherPdcMap;
        }

        private List<PostDatedVoucherDTO> PostDatedVoucherResponseParser(TallyRequestResponse data)
        {
            try
            {
                List<PostDatedVoucherDTO> postDatedVoucherDTOs = new List<PostDatedVoucherDTO>();
                string voucherTypes = ApplicationProperties.properties["pdc.vouchertype"].ToString();
                List<String> voucherTypeNamesList = voucherTypes.ToLower().Split(",").ToList();
                foreach (var item in data.response.BODY.IMPORTDATA.REQUESTDATA.TALLYMESSAGE)
                {
                    if (item.VOUCHER != null)
                    {
                        //if (item.VOUCHER.VOUCHERTYPENAME == ApplicationProperties.properties["pdc.vouchertype"].ToString())
                        if (voucherTypeNamesList.Contains(item.VOUCHER.VOUCHERTYPENAME.ToLower()))
                        {
                            PostDatedVoucherDTO postDatedVoucherDto = new PostDatedVoucherDTO();
                            postDatedVoucherDto.accountProfileName = item.VOUCHER.PARTYLEDGERNAME;
                            postDatedVoucherDto.referenceDocumentDate = ConvertToDateTime(item.VOUCHER.DATE);
                            postDatedVoucherDto.referenceDocumentNumber = item.VOUCHER.VOUCHERNUMBER;
                            foreach (var bankalledgerEntry in item.VOUCHER.ALLLEDGERENTRIES_LIST)
                            {
                                postDatedVoucherDto.instrumentNumber = bankalledgerEntry.BANKALLOCATIONSLIST.INSTRUMENTDATE;
                                postDatedVoucherDto.instrumentDate = ConvertToDateTime(bankalledgerEntry.BANKALLOCATIONSLIST.INSTRUMENTDATE);
                                postDatedVoucherDto.pdcReceiptDate = ConvertToDateTime(bankalledgerEntry.BANKALLOCATIONSLIST.PDCACTUALDATE);
                                postDatedVoucherDto.referenceDocumentAmount = bankalledgerEntry.BANKALLOCATIONSLIST.AMOUNT;
                                postDatedVoucherDto.remark = bankalledgerEntry.BANKALLOCATIONSLIST.NARRATION;

                          //      postDatedVoucherDto.referenceDocumentNumber = bankalledgerEntry.BANKALLOCATIONS_LIST.NARRATION;
                                postDatedVoucherDto.referenceVoucher = bankalledgerEntry.BANKALLOCATIONSLIST.NAME;

                            }
                            postDatedVoucherDTOs.Add(postDatedVoucherDto);

                        }
                    }
                }
                return postDatedVoucherDTOs;
            }catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw;
            }
        }

        private string ConvertToDateTime(string dATE)
        {
            try
            {
                if (dATE == null)
                    return null;
                return DateTime.ParseExact(dATE, "yyyymmdd", CultureInfo.InvariantCulture).ToString();
            }catch (Exception ex)
            {
                LogManager.HandleException(ex, "Exception  while converting string to date (" + dATE + ") to Date");
                return null;
            }
        }
    }
}
