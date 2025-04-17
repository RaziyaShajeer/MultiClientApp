using SNR_ClientApp.Properties;
using SNR_ClientApp.Services;
using SNR_ClientApp.Utils;

namespace SNR_ClientApp.Windows.CustomControls.AutomationControls
{
    public partial class AdvancedProperties2_UC : UserControl
    {
        TallyService tallyService;
        public TallyConfigForm parentform { get; set; }
        public AdvancedProperties2_UC()
        {
            tallyService = new TallyService();
            InitializeComponent();

            bindDatasFromProperties();
        }



        private void button2_Click(object sender, EventArgs e)
        {
            parentform.Cursor = Cursors.WaitCursor;
            try
            {
                printDatasToProperties();
                //TallyProperties2 tallyProperties2 = new TallyProperties2();
                //tallyProperties2.Show();

                AdvancedProperties3_UC nextScreen = new();

                nextScreen.parentform = parentform;
                parentform.AddUserControl(nextScreen);
                this.Hide();
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
            finally
            {
                parentform.Cursor = Cursors.Default;
            }
        }

        private void bindDatasFromProperties()
        {
            try
            {


                var optionalReceipt = ApplicationProperties.properties["is.optional.receipt"];
                var optionalSalesOrder = ApplicationProperties.properties["is.optional.salesOrder"];
                var cashOnlyLedger = ApplicationProperties.properties["enable.cash.only.ledger.entry"];
                var paymentModeTerms = ApplicationProperties.properties["payment.mode.terms"];
                var receiptEmplpoyee = ApplicationProperties.properties["enable.receipt.employeewise.ledger"];
                var state = ApplicationProperties.properties["company.state"].ToString();
                var orderEmployee = ApplicationProperties.properties["order.employee.name"];
                var batchName = ApplicationProperties.properties["batchName"];
                var isCashonlyEnabled = ApplicationProperties.properties["isCashonlyLedgerEnabled"];
                var costCenter = ApplicationProperties.properties["enable.cost.centre"];
                var costCenterCashReceipts = ApplicationProperties.properties["enable.cost.centre.cash.receipts"];
                var receiptVoucherType = ApplicationProperties.properties["enable.receipt.voucherType"];
                var provisionalNo = ApplicationProperties.properties["show.provisional.no"];
                var PaymentModeRemarks = ApplicationProperties.properties["PaymentModeRemarks"].ToString();
                chk_CostCenterCashReceipts.Checked = costCenterCashReceipts.ToString().ToLower().Equals("true");
                chk_ReceiptVoucherType.Checked = receiptVoucherType.ToString().ToLower().Equals("true");

                chk_OptionalReceipt.Checked = optionalReceipt.ToString().ToLower().Equals("true");

                chk_CostCenter.Checked = costCenter.ToString().ToLower().Equals("true");


                chk_EmployeewiseReceipt.Checked = receiptEmplpoyee.ToString().ToLower().Equals("true");

                chk_OrderEmployeeName.Checked = orderEmployee.ToString().ToLower().Equals("true");
                chk_ProvisionalReceipt.Checked = provisionalNo.ToString().ToLower().Equals("true");
                txt_PaymentModeTerms.Text = paymentModeTerms.ToString();

                chk_CashOnly.Checked = isCashonlyEnabled.ToString().ToLower().Equals("true");
                if (PaymentModeRemarks.Equals("True", StringComparison.OrdinalIgnoreCase))
                {
                    chk_paymentMode.Checked = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog("Exception occured while Binding datas from properties file in Tally Advanced Properties .");
                LogManager.HandleException(ex);
            }
        }

        private void printDatasToProperties()
        {
            ApplicationProperties.properties["payment.mode.terms"] = txt_PaymentModeTerms.Text;
            ApplicationProperties.properties["enable.cash.only.ledger.entry"] = chk_CashOnly.Checked ? txt_CashOnlyLedgerName.Text : "";

            ApplicationProperties.properties["is.optional.receipt"] = chk_OptionalReceipt.Checked.ToString();

            ApplicationProperties.properties["enable.cost.centre"] = chk_CostCenter.Checked;
            ApplicationProperties.properties["enable.cost.centre.cash.receipts"] = chk_CostCenterCashReceipts.Checked;

            ApplicationProperties.properties["isCashonlyLedgerEnabled"] = chk_CashOnly.Checked;

            ApplicationProperties.properties["enable.receipt.employeewise.ledger"] = chk_EmployeewiseReceipt.Checked;

            ApplicationProperties.properties["order.employee.name"] = chk_OrderEmployeeName.Checked;

            ApplicationProperties.properties["show.provisional.no"] = chk_ProvisionalReceipt.Checked.ToString();
            ApplicationProperties.properties["enable.receipt.voucherType"] = chk_ReceiptVoucherType.Checked;

            ApplicationProperties.properties["isDownloadByEmployeesEnabled"] = chk_DownloadByEmployeeVoucher.Checked.ToString();
            ApplicationProperties.properties["download.by.employees"] =(chk_DownloadByEmployeeVoucher.Checked) ? txtEmployees.Text : "";

            ApplicationProperties.properties["download.by.employee.voucher"] = chk_DownloadByEmployeeVoucher.Checked.ToString();
            ApplicationProperties.properties["PaymentModeRemarks"]=chk_paymentMode.Checked.ToString();
            ApplicationProperties.updatePropertiesFile();
        }





        private void btn_Back_Click(object sender, EventArgs e)
        {
            //AdvancedProperties advancedProperties = new AdvancedProperties();
            //advancedProperties.Show();
            //this.Hide();
            AdvancedProperties1_UC nextScreen = new();

            nextScreen.parentform = parentform;
            parentform.AddUserControl(nextScreen);
        }


        private void company_State_TextChanged(object sender, EventArgs e)
        {

        }

        private void chk_DownloadByEmployeeVoucher_CheckedChanged(object sender, EventArgs e)
        {

            lblEmployeeNames.Enabled = chk_DownloadByEmployeeVoucher.Checked;
            txtEmployees.Enabled = chk_DownloadByEmployeeVoucher.Checked;

        }

        private void chk_CashOnly_CheckedChanged(object sender, EventArgs e)
        {
            lbl_CashOnlyLedgerName.Enabled=chk_CashOnly.Checked;
            txt_CashOnlyLedgerName.Enabled = chk_CashOnly.Checked;
        }
    }
}
