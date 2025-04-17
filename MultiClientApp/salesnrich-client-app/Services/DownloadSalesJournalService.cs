using dtos;
using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Tally;
using SNR_ClientApp.Tally.generateXml;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    public class DownloadSalesJournalService
    {
        HttpClient _httpClient;
        public DownloadSalesJournalService()
        {
            _httpClient=new HttpClient();
            _httpClient = RestClientUtil.getClient();
        }

        DownloadByVoucherTypeService downloadByVoucherTypeService=new DownloadByVoucherTypeService();
        SalesJournalGenerateXml salesJournalGenerateXml = new SalesJournalGenerateXml();
        TransactionProcessor transactionProcessor=new TransactionProcessor();
        internal async Task<List<SalesOrderDTO>> getFromServerAndDownloadToTally(VoucherType pRIMARY_SALES,DateTime? saleDate)
        {
            try
            {
                LogManager.WriteLog("downloading inventory data from server with date.");
                SalesVoucherGenerateXml salesVoucherGenerateXml = new();
                SalesOrderGenerateXml salesOrderGenerateXml = new();
                List<TallyXml> tallyXmls = new List<TallyXml>();
                List<SalesOrderDTO> salesOrderDTOs = downloadByVoucherTypeService.getSalesFromServer(pRIMARY_SALES,saleDate);
                if (salesOrderDTOs.Count > 0)
                {

                    foreach (SalesOrderDTO salesOrderDTO in salesOrderDTOs)
                    {

                        ENVELOPE salesOrderXml = await salesJournalGenerateXml.generateSalesOrderXmlAsync(salesOrderDTO);
                        tallyXmls.Add(new TallyXml(salesOrderDTO.inventoryVoucherHeaderPid, salesOrderXml));
                    }

                    var res = await transactionProcessor.postOrdersToTally(tallyXmls);
                    List<String> succesOrders = (List<String>)res.body;
                    if (succesOrders.Count > 0)
                    {

                        uploadSuccesOrdersToServer(succesOrders);
                    }
                }
                return salesOrderDTOs;
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
          
        }

        private void uploadSuccesOrdersToServer(object succesReceipts)
        {
            string updateReceiptStatus = ApiConstants.UPDATE_ORDER_STATUS;
            List<String> succesReceiptpids = new List<String>();
            List<String> allSuccesReceiptpid = (List<String>)succesReceipts;
            succesReceiptpids.AddRange(allSuccesReceiptpid);
            LogManager.WriteLog("Status updated for " + succesReceiptpids.Count + " Orders");

            if (succesReceiptpids.Count > 0)
            {

                try
                {
                    LogManager.WriteLog("Uploading SuccerssOrders To Server ....");



                    LogManager.WriteLog("uploading SuccerssOrders started...\n" + "Api  : " + updateReceiptStatus);
                    _httpClient = RestClientUtil.getClient();
                    var myContent = JsonConvert.SerializeObject(succesReceiptpids);
                    HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");

                    var responseTask = _httpClient.PostAsync(updateReceiptStatus, inputContent);

                    responseTask.Wait();

                    HttpResponseMessage Res = responseTask.Result;
                    LogManager.WriteLog("Uploading succesReceiptpids To Server Completed ....\n Response : ");
                    LogManager.WriteResponseLog(Res);

                    if (Res.IsSuccessStatusCode)
                    {
                        LogManager.WriteLog("request for uploading succesReceiptpids  Success..");
                        var response = Res.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        LogManager.WriteLog("request for uploading succesReceiptpids  Failed..");
                    }
                }
                catch (Exception ex)
                {
                    LogManager.HandleException(ex);
                    throw ex;
                }
            }

        }


    }
}
