using SNR_ClientApp.DTO;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Services;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SNR_ClientApp.Windows.CustomControls
{
    public partial class UC_UploadSalesReceipt : UserControl
    {
        UC_Logger uC_Logger;
        TallyService tallyService;

        CompanyService companyService;
        Dictionary<string, Object> props = new Dictionary<string, Object>();

        DownloadReceiptService downloadReceiptService;
        ReceiptUploadService receiptUploadService;
        SalesUploadService salesUploadService;
        public UC_UploadSalesReceipt()
        {
            InitializeComponent();
            uC_Logger = new UC_Logger();
            companyService = new CompanyService();
            tallyService = new TallyService();
            uC_Logger = new UC_Logger();
            downloadReceiptService = new DownloadReceiptService();
            receiptUploadService = new ReceiptUploadService();
            LoadLoggerArea();
            salesUploadService=new SalesUploadService();
        }

        private void Btn_Receipt_Click(object sender, EventArgs e)
        {
            try
            {
                ParentForm.Cursor = Cursors.WaitCursor;
                this.Cursor = Cursors.WaitCursor;
                GlobalStateService.Currentstateoftask=true;
                UploadReceipt();
                GlobalStateService.Currentstateoftask=false;
            }
            catch (Exception ex)
            {
                appendLogMessage("upload receipt failed.");
            }
            finally
            {
                ParentForm.Cursor = Cursors.Default;
                this.Cursor = Cursors.Default;
            }
            // UploadReceipt();
        }

        private async void UploadReceipt()
        {
            try
            {
                LogManager.WriteLog("upload Receipt started.");
                appendLogMessage("Verifying tally and compnay.");
                checkTallyCompanyIsOpened();

                DateTime date = DatePicker.Value;
                if (date == null)
                {
                    appendLogMessage("please select date..!");
                    return;
                }
                else
                {
                    appendLogMessage("upload receipt started.");
                    await receiptUploadService.getFromTallyAndUploadAsync(date.ToString("d-MMM-yy"));
                    appendLogMessage("upload receipt completed.");
                }
            }
            catch (Exception ex)
            {
                appendLogMessage("upload receipt failed.");
               
            }
        }


        private void LoadLoggerArea()
        {
            uC_Logger = new UC_Logger();
            AddUserControl(uC_Logger);
        }
        private void AddUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            LoggerArea.Controls.Clear();
            LoggerArea.Controls.Add(userControl);

            userControl.BringToFront();
        }


        private async Task<bool> checkTallyCompanyIsOpened()
        {
            appendLogMessage("Verifying Tally and company .");
            List<CompanyDTO> companies =    await companyService.GetCompanies();
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
        private void appendLogMessage(string v)
        {
            uC_Logger.AppendLogMsg(v);
        }
        private void ClearLoggerArea()
        {
            uC_Logger.ClearLogArea();
        }

        private void Btn_Sales_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                ParentForm.Cursor = Cursors.WaitCursor;
                this.Cursor = Cursors.WaitCursor;
                GlobalStateService.Currentstateoftask=true;
                uploadSalesAsync(); 
                GlobalStateService.Currentstateoftask=false;
            }
            catch (Exception ex)
            {
                appendLogMessage("upload receipt failed.");
            }
            finally
            {
                ParentForm.Cursor = Cursors.Default;
                this.Cursor = Cursors.Default;
            }
        }

        private async Task uploadSalesAsync()
        {
            try
            {
                //LogManager.WriteLog("upload Sales started.");
                appendLogMessage("Verifying tally and compnay.");
                checkTallyCompanyIsOpened();

                DateTime date = DatePicker.Value;
                if (date == null)
                {
                    appendLogMessage("please select date..!");
                    return;
                }
                else
                {
                    appendLogMessage("upload Sales started.");
                    await salesUploadService.getFromTallyAndUploadAsync(date.ToString("d-MMM-yy"));
                
                    appendLogMessage("upload Sales completed.");
                }
            }
            catch (Exception ex)
            {
                appendLogMessage("upload Sales failed.");
                throw ex;
            }
        }
    }
}
