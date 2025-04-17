using SNR_ClientApp.Properties;
using SNR_ClientApp.Services;
using SNR_ClientApp.TallyResponses;
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

namespace SNR_ClientApp.Windows.CustomControls.AutomationControls
{
    public partial class AdvancedProperties1_UC : UserControl
    {
        public TallyConfigForm parentform { get; set; }
        TallyService tallyService;
        public AdvancedProperties1_UC()
        {
            InitializeComponent();
            tallyService = new TallyService();
            BindAllDatas();
            bindDatasFromProperties();
        }
        private async void BindAllDatas()
        {
            string[] row = await tallyService.getAllGodowns();
            GodownSelect.Items.Clear();
            GodownSelect.Items.AddRange(row);
            GodownSelect.SelectedIndex = 0;


        }
        private void bindDatasFromProperties()
        {
            try
            {
                var caseValue = ApplicationProperties.properties["case.value"].ToString();

                var actualBiledStatus = ApplicationProperties.properties["actual.billed.status"];
                var isGodown = ApplicationProperties.properties["godown.fixed"];
                var isOptimized = ApplicationProperties.properties["Isoptimized"].ToString();
                var isBatch = ApplicationProperties.properties["batch.fixed"];
                var downloadByEmployee = ApplicationProperties.properties["download.by.employees"];
                var optionalSalesOrder = ApplicationProperties.properties["is.optional.salesOrder"];
                var downloadByEmpVoucher = ApplicationProperties.properties["download.by.employee.voucher"];
                var state = ApplicationProperties.properties["company.state"].ToString();
                var provisionalNo = ApplicationProperties.properties["show.provisional.no"];

                var godownName = ApplicationProperties.properties["godownName"];
                var netStockAvilable = ApplicationProperties.properties["netStockAvilable"].ToString();
                var batchName = ApplicationProperties.properties["batchName"];

                var enableselectedDateWise = ApplicationProperties.properties["enable.Selecteddate"].ToString();
                //  var costCenter = ApplicationProperties.properties["enable.cost.centre"];
                var postdate = ApplicationProperties.properties["enable.date"];
                var companyPrefix = ApplicationProperties.properties["tally.company.prefix"];

                var donloadVehicleDetails = ApplicationProperties.properties["downloadVehicleDetails"].ToString();
                chk_optimized.Checked = isOptimized.ToString().ToLower().Equals("true");
                chk_VehicleDetails.Checked = donloadVehicleDetails.ToString().ToLower().Equals("true");
                chk_CaseValue.Checked = caseValue.ToString().ToLower().Equals("true");


                chk_CompanyPrefix.Checked = companyPrefix.ToString().ToLower().Equals("true");


                chk_NetStockAvailable.Checked = netStockAvilable.ToString().ToLower().Equals("true");
                chk_batch.Checked = isBatch.ToString().ToLower().Equals("true");

                chk_ActualBillledStatus.Checked = actualBiledStatus.ToString().ToLower().Equals("true");

                chk_Godown.Checked = isGodown.ToString().ToLower().Equals("true");

                //  chk_CostCenter.Checked = costCenter.ToString().ToLower().Equals("true");
                chk_PostDatedVoucher.Checked = postdate.ToString().ToLower().Equals("true");
                chk_SelectedDatedVoucher.Checked = enableselectedDateWise.ToString().ToLower().Equals("true");
                chk_OptionalSalesOrder.Checked = optionalSalesOrder.ToString().ToLower().Equals("true");
                company_State.Text = state.ToString();
                if (enableselectedDateWise.Equals("True", StringComparison.OrdinalIgnoreCase))
                {
                    chk_SelectedDatedVoucher.Checked=true;
                }
                if (isOptimized.Equals("True", StringComparison.OrdinalIgnoreCase))
                {
                    chk_optimized.Checked = true;
                }
                if (chk_batch.Checked)
                {
                    txt_Batch.Enabled = true;
                    lbl_BatchName.Enabled = true;
                    txt_Batch.Text = batchName.ToString();
                }
                if (godownName != "")
                {
                    GodownSelect.Enabled = true;
                    GodownSelect.SelectedItem = godownName;
                }
                else
                {
                    GodownSelect.Enabled = false;
                }
                //disable Cheque Entry
                var isDisabledCheckEntry = ApplicationProperties.properties["download.disable.chequeue.entry"].ToString();
                Chk_receipt.Checked = false;
                if (isDisabledCheckEntry.Equals("True", StringComparison.OrdinalIgnoreCase))
                {
                    Chk_receipt.Checked = true;
                }

            }
            catch (Exception ex)
            {
                LogManager.WriteLog("exception occured while Binding datas from properties file in Tally Advanced Properties .");
                LogManager.HandleException(ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parentform.Cursor = Cursors.WaitCursor;
            try
            {
                printDatasToProperties();

                //AdvancedProperties2 advancedProperties2 = new AdvancedProperties2();
                //advancedProperties2.Show();
                //this.Hide();
                AdvancedProperties2_UC nextScreen = new();

                nextScreen.parentform = parentform;
                parentform.AddUserControl(nextScreen);
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

        private void printDatasToProperties()
        {
            ApplicationProperties.properties["batch.fixed"] = chk_batch.Checked.ToString();



            ApplicationProperties.properties["tally.company.prefix"] = chk_CompanyPrefix.Checked;

            ApplicationProperties.properties["case.value"] = chk_CaseValue.Checked.ToString();
            ApplicationProperties.properties["batch.fixed"]=chk_batch.Checked.ToString();
            ApplicationProperties.properties["Isoptimized"] = chk_optimized.Checked.ToString();
            ApplicationProperties.properties["actual.billed.status"] = chk_ActualBillledStatus.Checked;
            //ApplicationProperties.properties["enable.cost.centre"] = chk_CostCenter.Checked;
            ApplicationProperties.properties["enable.date"] = chk_PostDatedVoucher.Checked;
            ApplicationProperties.properties["enable.Selecteddate"] = chk_SelectedDatedVoucher.Checked;

            ApplicationProperties.properties["is.optional.salesOrder"] = chk_OptionalSalesOrder.Checked.ToString();

            ApplicationProperties.properties["netStockAvilable"] = chk_NetStockAvailable.Checked.ToString();
            if (chk_batch.Checked)
            {
                ApplicationProperties.properties["batchName"] = txt_Batch.Text;
            }
            else
            {
                ApplicationProperties.properties["batchName"] = "";
            }
            if (chk_Godown.Checked)
            {
                ApplicationProperties.properties["godownName"] = GodownSelect.SelectedItem.ToString();
            }
            else
            {
                ApplicationProperties.properties["godownName"] = "";

            }
            ApplicationProperties.properties["downloadVehicleDetails"] = chk_VehicleDetails.Checked;
            ApplicationProperties.properties["download.disable.chequeue.entry"]=Chk_receipt.Checked;
          
                ApplicationProperties.properties["company.state"] =company_State.Text;
            ApplicationProperties.updatePropertiesFile();

        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
            //TallyProperties2 tallyProperties1 = new TallyProperties2();
            //tallyProperties1.Show();
            //this.Hide();
            parentform.Cursor = Cursors.WaitCursor;
            try
            {

                TallyProperties2_UC tallyProperties2_UC = new();

                tallyProperties2_UC.parentform = parentform;
                parentform.AddUserControl(tallyProperties2_UC);
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
            }
            finally
            {
                parentform.Cursor = Cursors.Default;
            }
        }



        private void chk_Godown_CheckedChanged(object sender, EventArgs e)
        {
            lbl_GodownName.Enabled = chk_Godown.Checked;
            GodownSelect.Enabled = chk_Godown.Checked;
        }

        private void chk_batch_CheckedChanged(object sender, EventArgs e)
        {
            lbl_BatchName.Enabled = chk_batch.Checked;
            txt_Batch.Enabled = chk_batch.Checked;
        }
    }
}
