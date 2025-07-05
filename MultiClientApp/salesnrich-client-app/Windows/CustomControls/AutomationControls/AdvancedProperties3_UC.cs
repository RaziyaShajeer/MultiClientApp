using SNR_ClientApp.DTO;
using SNR_ClientApp.Properties;
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

namespace SNR_ClientApp.Windows.CustomControls.AutomationControls
{
    public partial class AdvancedProperties3_UC : UserControl
    {
        TallyService tallyService;
        List<DropdownDTO> Cgsts;
        List<DropdownDTO> Sgsts;
        List<DropdownDTO> Igsts;
        public TallyConfigForm parentform { get; set; }
        public AdvancedProperties3_UC()
        {
            tallyService = new TallyService();
            InitializeComponent();
            BindAllDatas();
            bindDatasFromProperties();
        }
        private async  void BindAllDatas()
        {
            List<String> cessLists = new List<String>();
			string[] row = await tallyService.getAllLedgersNamesByParentcess("Duties & Taxes ");
			//string[] row = await tallyService.getAllLedgersNamesByParent("GL 13; Duties & Taxes");
			cessLists.AddRange(row);
			//TallyService tallyService = new TallyService();
			string[] groups = await tallyService.getAllGroupsByParent("Duties & Taxes");
			//string[] groups = await tallyService.getAllGroupsByParent("GL 13; Duties & Taxes");
			foreach (string group in groups)
            {
                cessLists.AddRange(await tallyService.getAllLedgersNamesByParent(group));
            }
            CessLedgerSelect.Items.Clear();
            CessLedgerSelect.Items.AddRange(cessLists.ToArray());

        }


        private async void bindDatasFromProperties()
        {
            try
            {
				var taxRequired = ApplicationProperties.properties["tax.required"].ToString();
				var salesOrderRemarks = ApplicationProperties.properties["sales.order.activity.remarks"].ToString();


				var cessLedgerName = ApplicationProperties.properties["Cess.ledger.name"].ToString();
				var IsCessEnabled = ApplicationProperties.properties["IsCessEnabled"].ToString();
				var invoiceNumberAsReference = ApplicationProperties.properties["invoice.number.as.reference"].ToString();
				var salesorderNumberAsReference = ApplicationProperties.properties["salesorder.number.as.reference"].ToString();
				var itemRemarks = ApplicationProperties.properties["item.remarks.enabled"].ToString();
				var productRate = ApplicationProperties.properties["product.rate.including.tax"];

				var cgstDutyHead = ApplicationProperties.properties["tally.productCGST"].ToString();
				var sgstDutyHead = ApplicationProperties.properties["tally.productSGST"].ToString();
				var igstDutyHead = ApplicationProperties.properties["tally.productIGST"].ToString();
				var gstLedger = ApplicationProperties.properties["gst.ledger.calculation"];
				string documentNoAsVoucher = ApplicationProperties.properties["DocumentNoAsVoucher"].ToString();
				var SalesOrderActivityRemarks = ApplicationProperties.properties["sales.order.activity.remarks"].ToString();
				//New Code is being written from here.............
				var reduceTax = ApplicationProperties.properties["reduce.tax"];

				String gstParent = ApplicationProperties.properties["gstParentGroup"].ToString();
				GstLedgersService gstLedgersService = new GstLedgersService();
				List<GstLedgerDTO> allGstLedgerspTally = await gstLedgersService.getAllGstLedgers(gstParent);
				string[] groups = await tallyService.getAllGroupsByParent(gstParent);
				foreach (string group in groups)
				{
					allGstLedgerspTally.AddRange(await gstLedgersService.getAllGstLedgers(group));
				}


				List<DropdownDTO> dropdownList = new List<DropdownDTO>();
				HashSet<string> uniqueValues = new HashSet<string>();

				foreach (GstLedgerDTO itm in allGstLedgerspTally)
				{
					// Check for duplicate values before adding to the list
					if (uniqueValues.Add(itm.gstDutyHead))
					{
						DropdownDTO dto = new DropdownDTO();
						dto.name = itm.gstDutyHead;
						dropdownList.Add(dto);
					}
				}


				// Set the DataSource for each dropdown individually
				taxTypeCGST.DataSource = new List<DropdownDTO>(dropdownList);
				taxTypeSGST.DataSource = new List<DropdownDTO>(dropdownList);
				taxTypeIGST.DataSource = new List<DropdownDTO>(dropdownList);

				// Fetch selected values from dropdowns
				string selectedCGSTValue = ApplicationProperties.properties["tally.productCGST"].ToString();
				string selectedSGSTValue = ApplicationProperties.properties["tally.productSGST"].ToString();
				string selectedIGSTValue = ApplicationProperties.properties["tally.productIGST"].ToString();

				// Set the selected item for each dropdown
				taxTypeCGST.SelectedIndex = taxTypeCGST.FindStringExact(selectedCGSTValue);
				taxTypeSGST.SelectedIndex = taxTypeSGST.FindStringExact(selectedSGSTValue);
				taxTypeIGST.SelectedIndex = taxTypeIGST.FindStringExact(selectedIGSTValue);
				foreach (DropdownDTO itm in taxTypeCGST.Items)
				{
					if (itm.ToString().IndexOf("CGST", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						taxTypeCGST.SelectedItem = itm;
						break;
					}
					else
					{
						if (itm.ToString().IndexOf("Central Tax", StringComparison.OrdinalIgnoreCase) >= 0)
						{
							taxTypeCGST.SelectedItem = itm;
							break;
						}
					}

				}
				foreach (DropdownDTO itm in taxTypeIGST.Items)
				{
					if (itm.ToString().IndexOf("IGST", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						taxTypeIGST.SelectedItem = itm;
						break;
					}
					else
					{
						if (itm.ToString().IndexOf("Integrated Tax", StringComparison.OrdinalIgnoreCase) >= 0)
						{
							taxTypeIGST.SelectedItem = itm;
							break;
						}
					}

				}
				foreach (DropdownDTO itm in taxTypeSGST.Items)
				{
					if (itm.ToString().IndexOf("SGST/UTGST", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						taxTypeSGST.SelectedItem = itm;
						break;
					}
					else
					{
						if (itm.ToString().IndexOf("State Tax", StringComparison.OrdinalIgnoreCase) >= 0)
						{
							taxTypeSGST.SelectedItem = itm;
							break;
						}
					}

				}



				if (reduceTax.ToString().Equals("True", StringComparison.OrdinalIgnoreCase))
				{
					chk_DeductTax.Checked = true;

				}

				if (taxRequired.ToString().Equals("True", StringComparison.OrdinalIgnoreCase))
				{
					chk_TaxRequired.Checked = true;

				}
				else
				{
					chk_TaxRequired.Checked = false;
				}
				if (salesOrderRemarks.ToString().Equals("True", StringComparison.OrdinalIgnoreCase))
				{
					chk_SalesOrderActivityRemarks.Checked = true;
				}

				if (productRate.ToString().Equals("True", StringComparison.OrdinalIgnoreCase))
				{
					chk_ProductRate.Checked = true;
				}

				if (IsCessEnabled.ToString().Equals("True", StringComparison.OrdinalIgnoreCase))
				{
					chk_Cess.Checked = true;
					if (cessLedgerName.ToString() != null)
					{
						CessLedgerSelect.SelectedItem = cessLedgerName.ToString();
					}
					else
					{
						chk_Cess.Checked = false;
					}



				}
				if (invoiceNumberAsReference.ToString().Equals("True", StringComparison.OrdinalIgnoreCase))
				{
					chk_InvoiceNumberAsReference.Checked = true;
				}

				if (salesorderNumberAsReference.ToString().Equals("True", StringComparison.OrdinalIgnoreCase))
				{
					chk_SoNo.Checked = true;
				}
				if (itemRemarks.Equals("True", StringComparison.OrdinalIgnoreCase))
				{
					chk_ItemRemarks.Checked = true;
				}
				else
				{
					chk_ItemRemarks.Checked = false;
				}
				if (documentNoAsVoucher.Equals("True", StringComparison.OrdinalIgnoreCase))
				{
					doc_number.Checked = true;
				}
				if (gstLedger.ToString().Equals("True", StringComparison.OrdinalIgnoreCase))
				{
					chk_GstCalculation.Checked = true;
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
			ApplicationProperties.properties["tax.required"] = chk_TaxRequired.Checked.ToString();
			ApplicationProperties.properties["reduce.tax"] = chk_DeductTax.Checked.ToString();
			ApplicationProperties.properties["product.rate.including.tax"] = chk_ProductRate.Checked.ToString();
			ApplicationProperties.properties["gst.ledger.calculation"] = chk_GstCalculation.Checked.ToString();
			ApplicationProperties.properties["IsCessEnabled"] = chk_Cess.Checked.ToString();
			ApplicationProperties.properties["sales.order.activity.remarks"] = chk_SalesOrderActivityRemarks.Checked.ToString();
			ApplicationProperties.properties["item.remarks.enabled"] = chk_ItemRemarks.Checked.ToString();



			if (chk_Cess.Checked)
			{
				if (CessLedgerSelect.SelectedItem.ToString() != null)
				{
					ApplicationProperties.properties["Cess.ledger.name"] = (chk_Cess.Checked) ? CessLedgerSelect.SelectedItem.ToString() : "";
				}
				else
				{
					chk_Cess.Checked = false;
				}
			}



			ApplicationProperties.properties["invoice.number.as.reference"] = chk_InvoiceNumberAsReference.Checked.ToString();
			ApplicationProperties.properties["salesorder.number.as.reference"] = chk_SoNo.Checked.ToString();

			ApplicationProperties.properties["DocumentNoAsVoucher"] = doc_number.Checked.ToString();
			// Fetch selected values from dropdowns
			string selectedCGSTValue = ((DropdownDTO)taxTypeCGST.SelectedItem)?.name ?? "";
			string selectedSGSTValue = ((DropdownDTO)taxTypeSGST.SelectedItem)?.name ?? "";
			string selectedIGSTValue = ((DropdownDTO)taxTypeIGST.SelectedItem)?.name ?? "";

			ApplicationProperties.properties["tally.productCGST"] = selectedCGSTValue.ToString();
			ApplicationProperties.properties["tally.productSGST"] = selectedSGSTValue.ToString();
			ApplicationProperties.properties["tally.productIGST"] = selectedIGSTValue.ToString();

			ApplicationProperties.updatePropertiesFile(StringUtilsCustom.TALLY_COMPANY);
        }


        private void NextBtn_Click(object sender, EventArgs e)
        {
            parentform.Cursor = Cursors.WaitCursor;
            try
            {
                printDatasToProperties();
                //TallyProperties2 tallyProperties2 = new TallyProperties2();
                //tallyProperties2.Show();

                TallyProperties2_UC nextScreen = new();

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

        private void btn_Back_Click(object sender, EventArgs e)
        {
            parentform.Cursor = Cursors.WaitCursor;
            try
            {
                printDatasToProperties();
                //TallyProperties2 tallyProperties2 = new TallyProperties2();
                //tallyProperties2.Show();
                AdvancedProperties2_UC nextScreen = new();

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

        private void chk_Cess_CheckedChanged(object sender, EventArgs e)
        {
            CessLedgerSelect.Enabled = chk_Cess.Checked;
            lbl_cess.Enabled = chk_Cess.Checked;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

      
    }
}
