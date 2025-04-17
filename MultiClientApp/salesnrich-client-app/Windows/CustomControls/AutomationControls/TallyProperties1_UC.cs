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
    public partial class TallyProperties1_UC : UserControl
    {
        public TallyConfigForm parentform { get; set; }
        TallyService tallyService;
        String[] accountGroupParents;
        List<DropdownDTO> CgstAndsgst;
        List<DropdownDTO> Cgsts;
        List<DropdownDTO> Sgsts;
        List<DropdownDTO> Igsts;

        List<DropdownDTO> SelectedGstList;
        List<DropdownDTO> SelectedIgstList;
        public TallyProperties1_UC()
        {
            CgstAndsgst = new List<DropdownDTO>();
            Cgsts = new List<DropdownDTO>();
            Sgsts = new List<DropdownDTO>();
            Igsts = new List<DropdownDTO>();

            SelectedGstList = new List<DropdownDTO>();
            SelectedIgstList = new List<DropdownDTO>();
            InitializeComponent();

            tallyService = new TallyService();
            LoadDefaultValues();
        }

        private void LoadDefaultValues()
        {
            bindDatasFromProperties();
            LoadAccountGroupsParents();
            LoadAccountGroupsCustomers();

        }


        private async void LoadAccountGroupsCustomers()
        {
            String[] row = await tallyService.getAllGroupsCurrentAssets();
            tallyLedgerParentSelect.DataSource = row;
            //if (row.Contains("Sundry Debtors"))
            //{
            //    if (ApplicationProperties.properties["tally.ledger.parent"].ToString()=="")
            //    {
            //        tallyLedgerParentSelect.SelectedItem = "Sundry Debtors";
            //    }

            //}
            //Setting value to tallyLedger select
            var talyLedgerParent = ApplicationProperties.properties["tally.ledger.parent"].ToString();

            tallyLedgerParentSelect.SelectedItem=talyLedgerParent;
            if (row.Contains(talyLedgerParent))
            {
                var item = row.Select(i => i).Where(i => i.ToString().Equals(talyLedgerParent)).SingleOrDefault();
                tallyLedgerParentSelect.SelectedItem=item;
            }
        }

        private async void LoadAccountGroupsParents()
        {
            try
            {
                String[] row = await tallyService.getAllGroups();
                accountGroupParents = row;
                AccountGroupsSelect.Items.Clear();
                AccountGroupsSelect.Items.AddRange(row);
                SalesLedgerParentSelect.Items.Clear();
                SalesLedgerParentSelect.Items.AddRange(row);
                if (ApplicationProperties.properties["gstParentGroup"] != "")
                {
                    String gstParent = ApplicationProperties.properties["gstParentGroup"].ToString();
                    if (row.Contains(gstParent))
                    {
                        var item = row.Select(i => i).Where(i => i.ToString().Equals(gstParent)).SingleOrDefault();
                        AccountGroupsSelect.SelectedItem = item;
                        loadGstLedgers(gstParent);
                    }
                    //AccountGroupsSelect.SelectedItem = gstParent;
                    //loadGstLedgers(gstParent);
                }
                else if (row.Contains("Duties & Taxes"))
                {
                    AccountGroupsSelect.SelectedItem = "Duties & Taxes";
                    loadGstLedgers("Duties & Taxes");
                }
                else if (row.Count()>0)
                {
                    AccountGroupsSelect.SelectedItem = row[0];
                    loadGstLedgers(row[0]);
                }
                if (ApplicationProperties.properties["salesLedgerParentGroup"] != "")
                {
                    String salesParent = ApplicationProperties.properties["salesLedgerParentGroup"].ToString();
                    SalesLedgerParentSelect.SelectedItem = salesParent;
                    loadSalesLedgers(salesParent);
                }
                else if (row.Contains("Sales Accounts"))
                {
                    SalesLedgerParentSelect.SelectedItem = "Sales Accounts";
                    loadSalesLedgers("Sales Accounts");
                }
                else if (row.Count() > 0)
                {
                    SalesLedgerParentSelect.SelectedItem = row[0];
                    loadSalesLedgers(row[0]);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to fetch Account Groups names");
            }
        }

        private async void loadSalesLedgers(string Parent)
        {
            String[] row = await tallyService.getAllLedgersNamesByParent(Parent);
            String[] salesLedgers = { "" };
            //salesLedgers[0] = "";

            SalesLedgerSelect.DataSource = salesLedgers.Concat(row).ToArray();

            BindSelectedSalesLedger();

        }

        private void BindSelectedSalesLedger()
        {
            string salesLedger = ApplicationProperties.properties["sales.ledger"].ToString();
            if (salesLedger != "")
            {
                SalesLedgerSelect.SelectedItem= salesLedger;
            }

        }

        private async void loadGstLedgers(String Parent)
        {
            try
            {
                Cgsts.Clear();
                Sgsts.Clear();
                Igsts.Clear();
                CgstAndsgst.Clear();
                GstCheckedListBox.Items.Clear();
                IgstCheckedListbox.Items.Clear();
                //List<DropdownDTO> CgstAndsgst = new List<DropdownDTO>();
                List<Object> objectitems = new List<Object>();

                GstLedgersService gstLedgersService = new GstLedgersService();
                List<GstLedgerDTO> allGstLedgerspTally = await gstLedgersService.getAllGstLedgers(Parent);
                TallyService tallyService = new TallyService();
                string[] groups =await  tallyService.getAllGroupsByParent(Parent);
                string cgstDutyHead = ApplicationProperties.properties["tally.productCGST"].ToString();
                string sgstDutyHead = ApplicationProperties.properties["tally.productSGST"].ToString();
                string igstDutyHead = ApplicationProperties.properties["tally.productIGST"].ToString();

                foreach (string group in groups)
                {
                    allGstLedgerspTally.AddRange(await gstLedgersService.getAllGstLedgers(group));
                }
                foreach (GstLedgerDTO itm in allGstLedgerspTally)
                {
                    DropdownDTO dto = new DropdownDTO();
                    if (itm.gstDutyHead.Contains(cgstDutyHead, StringComparison.OrdinalIgnoreCase)||(itm.gstDutyHead.Contains("CGST", StringComparison.OrdinalIgnoreCase)))
                    {
                        dto.name = itm.name;
                        dto.value = itm.taxRate;
                        Cgsts.Add(dto);
                    }
                    else if (itm.gstDutyHead.Contains(igstDutyHead, StringComparison.OrdinalIgnoreCase)||(itm.gstDutyHead.Contains("IGST", StringComparison.OrdinalIgnoreCase)))
                    {
                        dto.name = itm.name;
                        dto.value = itm.taxRate;
                        Igsts.Add(dto);
                    }
                    else if (itm.gstDutyHead.Contains(sgstDutyHead, StringComparison.OrdinalIgnoreCase)||(itm.gstDutyHead.Contains("SGST/UTGST", StringComparison.OrdinalIgnoreCase)))
                    {
                        dto.name = itm.name;
                        dto.value = itm.taxRate;
                        Sgsts.Add(dto);

                    }
                }


                for (
                    int i = 0; i < Cgsts.Count; i++)
                {
                    for (int j = 0; j < Sgsts.Count; j++)
                    {
                        if (Cgsts.ElementAtOrDefault(i).value.Equals(Sgsts.ElementAtOrDefault(j).value))
                        {
                            DropdownDTO dto = new DropdownDTO();
                            StringBuilder it = new StringBuilder(Cgsts.ElementAtOrDefault(i).name + "," + Sgsts.ElementAtOrDefault(j).name);
                            dto.name = it.ToString();
                            dto.value = (Cgsts.ElementAtOrDefault(i).value + Sgsts.ElementAtOrDefault(j).value);
                            CgstAndsgst.Add(dto);
                            objectitems.Add(dto.name);
                        }
                    }

                }
                //gstLedgerSelect.DataSource = CgstAndsgst;
                //IGSTSelect.DataSource = Igsts;


                GstCheckedListBox.Items.AddRange(CgstAndsgst.ToArray());
                IgstCheckedListbox.Items.AddRange(Igsts.ToArray());

                BindSelectdGst();
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                LogManager.WriteLog("Exception occured while binding GstParent and Gst ledgers : "+ex.Message);
                MessageBox.Show(ex.Message);
                throw ex;
            }
        }

        private void BindSelectdGst()
        {
            String[] gstArr;
            String[] igstArr;
            var gsts = ApplicationProperties.properties["tally.gst.index"];
            if (gsts != "")
            {
                gstArr = gsts.ToString().Split(",");
                foreach (String i in gstArr)
                {
                    try
                    {
                        GstCheckedListBox.SetItemChecked(int.Parse(i), true);
                    }
                    catch (Exception ex) { LogManager.HandleException(ex); }
                }
            }
            var isIgstEnabled = ApplicationProperties.properties["isIgstEnabled"];
            if (isIgstEnabled.Equals("True"))
            {
                Chk_EnalbeIgst.Checked = true;
                IgstCheckedListbox.Enabled = true;
                lbl_EnabeIgst.Enabled = true;
                var igsts = ApplicationProperties.properties["tally.igst.index"];
                if (igsts != "")
                {
                    igstArr = igsts.ToString().Split(",");
                    foreach (String i in igstArr)
                    {
                        IgstCheckedListbox.SetItemChecked(int.Parse(i), true);
                    }
                }
            }

            //dta.Checked= true;
            //   GstCheckedListBox.Items[0] = dta;

        }
        private void bindDatasFromProperties()
        {

           
            var autodownload = ApplicationProperties.properties["AutoDownload"].ToString();
            chk_download.Checked=false;
            txt_downloadtime.Enabled=false;

            var AutoDownloadTimePeriod = ApplicationProperties.properties["AutoDownloadTimePeriod"].ToString();
            if (autodownload.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                chk_download.Checked=true;
                txt_downloadtime.Enabled=true;
                txt_downloadtime.Text=AutoDownloadTimePeriod;
            }
            var autoUpload = ApplicationProperties.properties["AutoUpload"].ToString();
            chk_upload.Checked=false;
            txt_uploadtime.Enabled=false;
            var AutoUploadTimePeriod = ApplicationProperties.properties["AutoUploadTimePeriod"].ToString();
            if (autoUpload.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                chk_upload.Checked=true;
                txt_uploadtime.Enabled=true;
                txt_uploadtime.Text=AutoUploadTimePeriod;

            }
            var distributedCode = ApplicationProperties.properties["DistributedCode"].ToString();
            if(distributedCode!=null)
            {
                Txt_DistributedCode.Text=distributedCode;
                Txt_DistributedCode.Enabled=false;

            }
            var IsEnableDistributor = ApplicationProperties.properties["IsEnableDistributor"].ToString();
            if (IsEnableDistributor.Equals("false",StringComparison.OrdinalIgnoreCase))

            {
                Txt_DistributedCode.Visible=false ;
                label1.Visible=false ;  
            }

        }





        

        private void Chk_EnaleIgst_CheckedChanged(object sender, EventArgs e)
        {
            if (Chk_EnalbeIgst.Checked)
            {
                //IGSTSelect.Enabled = true;
                lbl_EnabeIgst.Enabled = true;
                IgstCheckedListbox.Enabled = true;
            }
            else
            {
                //IGSTSelect.Enabled = false;
                lbl_EnabeIgst.Enabled = false;
                IgstCheckedListbox.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parentform.Cursor = Cursors.WaitCursor;
            try
            {
                // button2_Validating(sender, e);
                SelectedGstList.Clear();
                SelectedIgstList.Clear();
                List<int> selectedGstIndexes = new List<int>();
                List<int> selectedIgstIndexes = new List<int>();
                //string? TallyGstValues= gstLedgerSelect.SelectedItem.ToString();
                string? GstParent = AccountGroupsSelect.SelectedItem.ToString();

                if (GstCheckedListBox.CheckedItems.Count > 0)
                {
                    selectedGstIndexes.Clear();
                    for (int x = 0; x < GstCheckedListBox.CheckedItems.Count; x++)
                    {
                        DropdownDTO dt = (DropdownDTO)GstCheckedListBox.CheckedItems[x];
                        SelectedGstList.Add(dt);
                        int intex = GstCheckedListBox.Items.IndexOf(GstCheckedListBox.CheckedItems[x]);
                        selectedGstIndexes.Add(intex);
                    }
                    StringBuilder selectedGstIndexesBuilder = new StringBuilder();
                    string prefix = "";
                    foreach (int i in selectedGstIndexes)
                    {
                        selectedGstIndexesBuilder.Append(prefix + i);
                        prefix = ",";
                    }
                    ApplicationProperties.properties["tally.gst.index"] = selectedGstIndexesBuilder.ToString();
                }
                else
                {
                    ApplicationProperties.properties["tally.gst.index"] = "";
                }
                //if (Chk_EnaleIgst.Checked)
                //{
                //    if (IgstCheckedListbox.CheckedItems.Count != 0)
                //    {
                //        selectedIgstIndexes.Clear();
                //        for (int x = 0; x < IgstCheckedListbox.CheckedItems.Count; x++)
                //        {
                //            DropdownDTO dt = (DropdownDTO)IgstCheckedListbox.CheckedItems[x];
                //            SelectedGstList.Add(dt);
                //            int intex = IgstCheckedListbox.Items.IndexOf(IgstCheckedListbox.CheckedItems[x]);
                //            selectedIgstIndexes.Add(intex);
                //        }
                //    }
                //}



                if (SelectedGstList.Count > 1)
                {
                    ApplicationProperties.properties["is.multi.tax"] = true;
                    //properties.setProperty("is.multi.tax", "true");
                }
                else
                {
                    ApplicationProperties.properties["is.multi.tax"] = false;
                    //properties.setProperty("is.multi.tax", "false");
                }
                StringBuilder gsts = new StringBuilder();
                StringBuilder taxrate = new StringBuilder();
                String prefix1 = "";
                for (int i = 0; i < SelectedGstList.Count; i++)
                {
                    gsts.Append(prefix1 + SelectedGstList.ElementAtOrDefault(i).name);
                    taxrate.Append(prefix1 + SelectedGstList.ElementAtOrDefault(i).value);
                    prefix1 = ",";

                }
                ApplicationProperties.properties["isIgstEnabled"] = false;
                if (Chk_EnalbeIgst.Checked)
                {
                    ApplicationProperties.properties["isIgstEnabled"] = true;
                    if (IgstCheckedListbox.CheckedItems.Count != 0)
                    {
                        for (int x = 0; x < IgstCheckedListbox.CheckedItems.Count; x++)
                        {
                            DropdownDTO dt = (DropdownDTO)IgstCheckedListbox.CheckedItems[x];
                            SelectedIgstList.Add(dt);
                        }

                    }
                    StringBuilder igsts = new StringBuilder();

                    foreach (DropdownDTO it in SelectedIgstList)
                    {
                        gsts.Append(prefix1 + it.name);
                        taxrate.Append(prefix1 + it.value);
                    }


                    if (IgstCheckedListbox.CheckedItems.Count != 0)
                    {
                        selectedIgstIndexes.Clear();
                        for (int x = 0; x < IgstCheckedListbox.CheckedItems.Count; x++)
                        {
                            DropdownDTO dt = (DropdownDTO)IgstCheckedListbox.CheckedItems[x];
                            SelectedGstList.Add(dt);
                            int index = IgstCheckedListbox.Items.IndexOf(IgstCheckedListbox.CheckedItems[x]);
                            selectedIgstIndexes.Add(index);
                        }
                    }
                    StringBuilder selectedIgstIndexesBuilder = new StringBuilder();
                    string prefix = "";
                    foreach (int i in selectedIgstIndexes)
                    {
                        selectedIgstIndexesBuilder.Append(prefix + i);
                        prefix = ",";
                    }
                    prefix = "";
                    ApplicationProperties.properties["tally.igst.index"] = selectedIgstIndexesBuilder.ToString();
                }




                string? TallyLedgerParent;
				//  Object[] GstParents = checkedListBox1.SelectedItems;
				if (tallyLedgerParentSelect.SelectedItem.ToString()!="")
                {
					 TallyLedgerParent = tallyLedgerParentSelect.SelectedItem.ToString();
				}
				else
				{
                    TallyLedgerParent = " ";
				}

				string? TallySalesLedger = (SalesLedgerSelect.SelectedItem !=null) ? SalesLedgerSelect.SelectedItem.ToString() : "";
                string? SalesLedgerParent = SalesLedgerParentSelect.SelectedItem.ToString();

                ApplicationProperties.properties["tally.ledger.parent"] = TallyLedgerParent;
                ApplicationProperties.properties["sales.ledger"] = TallySalesLedger;
                ApplicationProperties.properties["gstParentGroup"] = GstParent;
                ApplicationProperties.properties["salesLedgerParentGroup"] = SalesLedgerParent;
                ApplicationProperties.properties["tally.gst"] = gsts.ToString();
                ApplicationProperties.properties["tally.taxs"] = taxrate.ToString();
                ApplicationProperties.properties["AutoDownload"]=chk_download.Checked;
                ApplicationProperties.properties["AutoDownloadTimePeriod"]=txt_downloadtime.Text;
                ApplicationProperties.properties["AutoUpload"]=chk_upload.Checked.ToString();
                ApplicationProperties.properties["AutoUploadTimePeriod"]=txt_uploadtime.Text;
				ApplicationProperties.properties["DistributedCode"] = Txt_DistributedCode.Text;


				//var myContent = JsonConvert.SerializeObject(SelectedGstList);
				//var myContent2 = JsonConvert.SerializeObject(SelectedIgstList);
				//ApplicationProperties.properties["tallyGstListObjects"] = myContent;
				//ApplicationProperties.properties["tallyIstListObjects"] = myContent2;
				ApplicationProperties.updatePropertiesFile();

                //TallyProperties2 tallyProperties2 = new TallyProperties2();
                //tallyProperties2.Show();
                TallyProperties2_UC tallyProperties2_UC = new();

                tallyProperties2_UC.parentform = parentform;
                parentform.AddUserControl(tallyProperties2_UC);


                this.Hide();
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
            finally { parentform.Cursor = Cursors.Default; }

        }
        
        private void AccountGroupsSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            string GstParent = "";
            try
            {
                GstParent = AccountGroupsSelect.SelectedItem.ToString();
                loadGstLedgers(GstParent);
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                MessageBox.Show("Unable to get GST ledgers under parent " + GstParent);
            }
        }

        private void SalesLedgerParentSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            string salesLedgerParent = "";
            try
            {
                salesLedgerParent = SalesLedgerParentSelect.SelectedItem.ToString();
                loadSalesLedgers(salesLedgerParent);
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                MessageBox.Show("Unable to load sales ledgers under parent " + salesLedgerParent);
            }
        }

        private void GstCheckedListBox_Validating(object sender, CancelEventArgs e)
        {


            if (GstCheckedListBox.SelectedItems.Count <= 0)
            {
                e.Cancel = true;
                //password.Focus();
                errorProvider1.SetError(GstCheckedListBox, "Please select atleast one .");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(GstCheckedListBox, "");
            }
        }

        private void button2_Validating(object sender, CancelEventArgs e)
        {

            if (GstCheckedListBox.SelectedItems.Count <= 0)
            {
                e.Cancel = true;
                //password.Focus();
                errorProvider1.SetError(GstCheckedListBox, "Please select atleast one .");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(GstCheckedListBox, "");
            }
        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
            parentform.Cursor = Cursors.WaitCursor;
            try
            {
                // printDatasToResourceFile();
                TalllyConnect tallyProperties1_UC = new TalllyConnect();
                tallyProperties1_UC.ParentForm = parentform;
                parentform.AddUserControl(tallyProperties1_UC);
                this.Hide();
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

        private void chk_download_CheckedChanged(object sender, EventArgs e)
        {
            txt_downloadtime.Enabled=chk_download.Checked;
            txt_downloadtime.Text=null;

        }

        private void chk_upload_CheckedChanged(object sender, EventArgs e)
        {
            txt_uploadtime.Enabled=chk_upload.Checked;
            txt_uploadtime.Text=null;
        }

        private void txt_downloadtime_TextChanged(object sender, EventArgs e)
        {
            var downloadenable = chk_download.Checked.ToString();
            var timetodownload = txt_downloadtime.Text;
            if (downloadenable.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                ApplicationProperties.properties["AutoDownload"]=chk_download.Checked;

                if (!string.IsNullOrWhiteSpace(timetodownload))
                {
                    if (int.TryParse(timetodownload, out int timePeriod))
                    {
                        if (timePeriod< 5)
                        {
                            MessageBox.Show("Please give the Time grater than 5");
                        }

                    }
                }
                else
                {
                    ApplicationProperties.properties["AutoDownloadTimePeriod"]=txt_downloadtime.Text;
                }

            }
            else
            {
                MessageBox.Show("Please give the Time ");
            }


        }

        private void txt_uploadtime_TextChanged(object sender, EventArgs e)
        {
            var timetoupload = txt_uploadtime.Text;
            var uploadenable = chk_upload.Checked.ToString();
            if (uploadenable.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                ApplicationProperties.properties["AutoUpload"]=chk_upload.Checked.ToString();
                if (!string.IsNullOrWhiteSpace(timetoupload))
                {
                    if (int.TryParse(timetoupload, out int timePeriod))
                    {
                        if (timePeriod < 5)
                        {
                            MessageBox.Show("Please give the Time grater than 5");

                        }

                    }
                    else
                    {
                        ApplicationProperties.properties["AutoUploadTimePeriod"]=txt_uploadtime.Text;

                    }
                }
                else
                {
                    
                    MessageBox.Show("Please give the Time");
                }
            }
           
        }


        

        //private void GstCheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        List<DropdownDTO> SelectedGstListCpy = new();
        //        SelectedGstListCpy.AddRange(SelectedGstList);
        //        for (int x = 0; x < GstCheckedListBox.CheckedItems.Count; x++)
        //        {
        //            SelectedGstListCpy.ForEach(x =>
        //            {
        //                if (!GstCheckedListBox.CheckedItems.Contains(x))
        //                {
        //                    SelectedGstList.Remove(x);
        //                }
        //            });
        //            DropdownDTO dt = (DropdownDTO)GstCheckedListBox.CheckedItems[x];
        //            if (!SelectedGstList.Contains(dt))
        //            {
        //                SelectedGstList.Add(dt);
        //            }

        //        }
        //    }catch(Exception ex)
        //    {

        //    }
        //}
    }
}
