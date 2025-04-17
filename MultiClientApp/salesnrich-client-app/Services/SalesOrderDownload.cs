using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Services;
using SNR_ClientApp.Tally;
using SNR_ClientApp.Utils;
using SNR_ClientApp.Windows.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    public class SalesOrderDownload
    {
        Dictionary<string, Object> props = new Dictionary<string, Object>();
        private String enableDateWise;
        private String enableselectedDateWise;
        private String userStockLocation;
        DownloadByVoucherTypeService downloadByVoucherTypeService;
        DownloadSalesOrderService downloadSalesOrderService;
        UC_Logger uC_Logger;
        CompanyService companyService;
        private String companyString;
        readonly TallyCommunicator tallyCommunicator;
        public SalesOrderDownload()
        {
            tallyCommunicator=new TallyCommunicator();  
            companyString = ApplicationProperties.properties["tally.company"].ToString();
            companyService = new CompanyService();
            uC_Logger = new UC_Logger();
            enableDateWise = ApplicationProperties.properties["enable.date"].ToString();
            enableselectedDateWise = ApplicationProperties.properties["enable.Selecteddate"].ToString();
            userStockLocation = ApplicationProperties.properties["user.stockLocation"].ToString();
            downloadByVoucherTypeService = new DownloadByVoucherTypeService();
            downloadSalesOrderService = new DownloadSalesOrderService();
        }

        public async Task salesOrderAutomaticDownload()
        {
            try
            {
                //DateTime? salesDate = DateTime.Now;
                DateTime? salesDate = DateTime.Now;
                //appendLogMessage("Verifying tally and compnay...");
                if (!await checkTallyCompanyIsOpened())
                {
                    BackgroundTaskManagerService.Instance.StopBackgroundTask();
                    MessageBox.Show("Please open Tally and Restart application... ");
                    
                    Application.Exit();
                }
                else
                {
                    //appendLogMessage("Tally and company verified.");
                    appendLogMessage("Download Sales Order started.");
                    DownloadResponseDto res = new();
                    if (userStockLocation.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        if (ApplicationProperties.properties["IsEnableDistributor"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            await downloadByVoucherTypeService
                               .getFromServerAndDownloadToTally(VoucherType.SECONDARY_SALES_ORDER, salesDate.Value, uC_Logger);
                        }
                        else
                        {
                            await downloadByVoucherTypeService
                              .getFromServerAndDownloadToTally(VoucherType.PRIMARY_SALES_ORDER, salesDate.Value, uC_Logger);
                        }
                        

                    }
                    else
                    {

                        await downloadSalesOrderService
                                    .getFromServerAndDownloadToTallyAsync(salesDate.Value, uC_Logger);
                    }
                }
            }

            //if (res.SuccessCount > 0)
            //{
            //    appendLogMessage(res.SuccessCount+" sales order downloaded .");
            //}
            //if (res.FailedCount > 0)
            //{
            //    appendLogMessage(res.FailedCount+" sales order failed to downloaded .");
            //}
            //if (res.TotalCount <= 0)
            //{
            //    appendLogMessage("No sales found for downloading.");
            //}
            //else
            //{
            //	appendLogMessage("No sales order found for downloading.");
            //}
            //List<ReceiptDTO> receiptDTOs = await downloadReceiptService.getFromServerAndDownloadToTallyAsync();
            //if (receiptDTOs.Count > 0)
            //{
            //    appendLogMessage("Download receipt completed.");
            //}
            //else
            //{
            //    appendLogMessage("No receipts found for downloading");
            //}


            catch (ServiceException ex)
            {
                LogManager.HandleException(ex);
                appendLogMessage("downloading sales order failed .\n " + ex.Message);
                //appendLogMessage(ex.Message);
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                appendLogMessage("downloading sales order failed .");
                //appendLogMessage(ex.Message);
            }
        }



        private async Task<bool> checkTallyCompanyIsOpened()
        {
            try
            {
                appendLogMessage("Verifying Tally and company .");
                List<CompanyDTO> companies = await companyService.GetCompanies();
                if (companies != null)
                {
                    if (!checkCompanyExist(companies))
                    {
                        MessageBox.Show("Please ensure company is open in tally ");
                        throw new ServiceException("Please ensure company is open in tally");
                        return false;
                    }
                    else
                    {
                        appendLogMessage("Tally and company verified.");
                        return true;

                    }
                }
                appendLogMessage("Tally and company verification failed.");
                return false;
            }
            catch (Exception ex)
            {
                appendLogMessage("Tally and company verification failed.");
                LogManager.HandleException(ex);
                return false;
            }
        }
        private void appendLogMessage(string v)
        {
            uC_Logger.AppendLogMsg(v);
        }
        private void ClearLoggerArea()
        {
            uC_Logger.ClearLogArea();
        }
        private bool checkCompanyExist(List<CompanyDTO> companies)
        {
            foreach (CompanyDTO dto in companies)
            {
                if (dto.companyName.Equals(StringUtilsCustom.TALLY_COMPANY))
                {
                    return true;
                }
            }
            return false;
        }
    }
}