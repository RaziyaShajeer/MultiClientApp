namespace SNR_ClientApp.Windows.CustomControls
{
    partial class UC_UserHome
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            flowLayoutPanel1=new FlowLayoutPanel();
            panel2=new Panel();
            companySelect=new ComboBox();
            panel1=new Panel();
            chk_selectAll=new CheckBox();
            chk_fullUpdate=new CheckBox();
            chk_productGroup=new CheckBox();
            chk_productCategory=new CheckBox();
            chk_productProfile=new CheckBox();
            chk_openingStock=new CheckBox();
            chk_defultLedgerWiseItem=new CheckBox();
            chk_temporary_openingStock_id=new CheckBox();
            chk_priceLevelList_id=new CheckBox();
            chk_groupWiseItem=new CheckBox();
            chk_groupWiseGST=new CheckBox();
            chk_taxMaster=new CheckBox();
            chk_location=new CheckBox();
            chk_accountProfile=new CheckBox();
            chk_receiveblePayeble_id=new CheckBox();
            chk_groupWiseAccount=new CheckBox();
            chk_Account_ClosingBalance=new CheckBox();
            chk_post_dated_voucher=new CheckBox();
            chk_gst_ledgers=new CheckBox();
            btn_upload=new Button();
            uploadLocationHeirarchyButton=new Button();
            loggerPanel=new FlowLayoutPanel();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll=true;
            flowLayoutPanel1.AutoScrollMargin=new Size(10, 0);
            flowLayoutPanel1.BackColor=Color.FromArgb(143, 184, 202);
            flowLayoutPanel1.Controls.Add(panel2);
            flowLayoutPanel1.Controls.Add(panel1);
            flowLayoutPanel1.Controls.Add(chk_productGroup);
            flowLayoutPanel1.Controls.Add(chk_productCategory);
            flowLayoutPanel1.Controls.Add(chk_productProfile);
            flowLayoutPanel1.Controls.Add(chk_openingStock);
            flowLayoutPanel1.Controls.Add(chk_defultLedgerWiseItem);
            flowLayoutPanel1.Controls.Add(chk_temporary_openingStock_id);
            flowLayoutPanel1.Controls.Add(chk_priceLevelList_id);
            flowLayoutPanel1.Controls.Add(chk_groupWiseItem);
            flowLayoutPanel1.Controls.Add(chk_groupWiseGST);
            flowLayoutPanel1.Controls.Add(chk_taxMaster);
            flowLayoutPanel1.Controls.Add(chk_location);
            flowLayoutPanel1.Controls.Add(chk_accountProfile);
            flowLayoutPanel1.Controls.Add(chk_receiveblePayeble_id);
            flowLayoutPanel1.Controls.Add(chk_groupWiseAccount);
            flowLayoutPanel1.Controls.Add(chk_Account_ClosingBalance);
            flowLayoutPanel1.Controls.Add(chk_post_dated_voucher);
            flowLayoutPanel1.Controls.Add(chk_gst_ledgers);
            flowLayoutPanel1.Controls.Add(btn_upload);
            flowLayoutPanel1.Controls.Add(uploadLocationHeirarchyButton);
            flowLayoutPanel1.Dock=DockStyle.Left;
            flowLayoutPanel1.FlowDirection=FlowDirection.TopDown;
            flowLayoutPanel1.ForeColor=Color.White;
            flowLayoutPanel1.Location=new Point(0, 0);
            flowLayoutPanel1.Margin=new Padding(5);
            flowLayoutPanel1.Name="flowLayoutPanel1";
            flowLayoutPanel1.Padding=new Padding(10);
            flowLayoutPanel1.Size=new Size(299, 632);
            flowLayoutPanel1.TabIndex=2;
            flowLayoutPanel1.WrapContents=false;
            // 
            // panel2
            // 
            panel2.Controls.Add(companySelect);
            panel2.Location=new Point(13, 13);
            panel2.Name="panel2";
            panel2.Padding=new Padding(5, 10, 5, 5);
            panel2.Size=new Size(264, 30);
            panel2.TabIndex=52;
            // 
            // companySelect
            // 
            companySelect.BackColor=Color.FromArgb(224, 224, 224);
            companySelect.DropDownStyle=ComboBoxStyle.DropDownList;
            companySelect.FormattingEnabled=true;
            companySelect.ImeMode=ImeMode.Hiragana;
            companySelect.Location=new Point(8, 5);
            companySelect.Name="companySelect";
            companySelect.Size=new Size(248, 23);
            companySelect.TabIndex=51;
            companySelect.SelectedIndexChanged+=companySelect_SelectedIndexChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(chk_selectAll);
            panel1.Controls.Add(chk_fullUpdate);
            panel1.Location=new Point(13, 49);
            panel1.Name="panel1";
            panel1.Size=new Size(256, 34);
            panel1.TabIndex=55;
            // 
            // chk_selectAll
            // 
            chk_selectAll.AutoSize=true;
            chk_selectAll.FlatAppearance.BorderSize=3;
            chk_selectAll.FlatAppearance.CheckedBackColor=Color.DodgerBlue;
            chk_selectAll.FlatAppearance.MouseOverBackColor=Color.FromArgb(128, 255, 255);
            chk_selectAll.Font=new Font("Ebrima", 12F, FontStyle.Regular, GraphicsUnit.Point);
            chk_selectAll.ForeColor=Color.Black;
            chk_selectAll.Location=new Point(8, 3);
            chk_selectAll.Name="chk_selectAll";
            chk_selectAll.Size=new Size(92, 25);
            chk_selectAll.TabIndex=29;
            chk_selectAll.Text="Select All";
            chk_selectAll.UseVisualStyleBackColor=true;
            chk_selectAll.CheckedChanged+=chk_selectAll_CheckedChanged;
            // 
            // chk_fullUpdate
            // 
            chk_fullUpdate.AutoSize=true;
            chk_fullUpdate.FlatAppearance.BorderColor=Color.White;
            chk_fullUpdate.FlatAppearance.BorderSize=3;
            chk_fullUpdate.FlatAppearance.CheckedBackColor=Color.RoyalBlue;
            chk_fullUpdate.FlatAppearance.MouseDownBackColor=Color.FromArgb(0, 0, 64);
            chk_fullUpdate.FlatAppearance.MouseOverBackColor=Color.White;
            chk_fullUpdate.Font=new Font("Ebrima", 12F, FontStyle.Regular, GraphicsUnit.Point);
            chk_fullUpdate.ForeColor=Color.Black;
            chk_fullUpdate.Location=new Point(139, 3);
            chk_fullUpdate.Name="chk_fullUpdate";
            chk_fullUpdate.Size=new Size(108, 25);
            chk_fullUpdate.TabIndex=47;
            chk_fullUpdate.Text="Full Update";
            chk_fullUpdate.UseVisualStyleBackColor=true;
            // 
            // chk_productGroup
            // 
            chk_productGroup.AutoSize=true;
            chk_productGroup.FlatAppearance.BorderColor=Color.White;
            chk_productGroup.FlatAppearance.BorderSize=15;
            chk_productGroup.FlatAppearance.CheckedBackColor=Color.FromArgb(19, 143, 43);
            chk_productGroup.FlatAppearance.MouseDownBackColor=Color.FromArgb(0, 64, 0);
            chk_productGroup.FlatAppearance.MouseOverBackColor=Color.White;
            chk_productGroup.Font=new Font("Ebrima", 12F, FontStyle.Regular, GraphicsUnit.Point);
            chk_productGroup.ForeColor=Color.FromArgb(20, 20, 20);
            chk_productGroup.Location=new Point(13, 89);
            chk_productGroup.Name="chk_productGroup";
            chk_productGroup.Padding=new Padding(10, 0, 0, 0);
            chk_productGroup.Size=new Size(141, 25);
            chk_productGroup.TabIndex=36;
            chk_productGroup.Text="Product Group";
            chk_productGroup.UseVisualStyleBackColor=true;
            // 
            // chk_productCategory
            // 
            chk_productCategory.AutoSize=true;
            chk_productCategory.FlatAppearance.BorderColor=Color.White;
            chk_productCategory.FlatAppearance.BorderSize=3;
            chk_productCategory.FlatAppearance.CheckedBackColor=Color.Green;
            chk_productCategory.FlatAppearance.MouseDownBackColor=Color.FromArgb(0, 64, 0);
            chk_productCategory.FlatAppearance.MouseOverBackColor=Color.White;
            chk_productCategory.Font=new Font("Ebrima", 12F, FontStyle.Regular, GraphicsUnit.Point);
            chk_productCategory.ForeColor=Color.FromArgb(20, 20, 20);
            chk_productCategory.Location=new Point(13, 120);
            chk_productCategory.Name="chk_productCategory";
            chk_productCategory.Padding=new Padding(10, 0, 0, 0);
            chk_productCategory.Size=new Size(160, 25);
            chk_productCategory.TabIndex=32;
            chk_productCategory.Text="Product Category";
            chk_productCategory.UseVisualStyleBackColor=true;
            // 
            // chk_productProfile
            // 
            chk_productProfile.AutoSize=true;
            chk_productProfile.FlatAppearance.CheckedBackColor=Color.Green;
            chk_productProfile.FlatAppearance.MouseDownBackColor=Color.FromArgb(0, 64, 0);
            chk_productProfile.FlatAppearance.MouseOverBackColor=Color.White;
            chk_productProfile.Font=new Font("Ebrima", 12F, FontStyle.Regular, GraphicsUnit.Point);
            chk_productProfile.ForeColor=Color.Black;
            chk_productProfile.Location=new Point(13, 151);
            chk_productProfile.Name="chk_productProfile";
            chk_productProfile.Padding=new Padding(10, 0, 0, 0);
            chk_productProfile.Size=new Size(142, 25);
            chk_productProfile.TabIndex=33;
            chk_productProfile.Text="Product Profile";
            chk_productProfile.UseVisualStyleBackColor=true;
            // 
            // chk_openingStock
            // 
            chk_openingStock.AutoSize=true;
            chk_openingStock.FlatAppearance.CheckedBackColor=Color.Green;
            chk_openingStock.FlatAppearance.MouseDownBackColor=Color.FromArgb(0, 64, 0);
            chk_openingStock.FlatAppearance.MouseOverBackColor=Color.White;
            chk_openingStock.Font=new Font("Ebrima", 12F, FontStyle.Regular, GraphicsUnit.Point);
            chk_openingStock.ForeColor=Color.Black;
            chk_openingStock.Location=new Point(13, 182);
            chk_openingStock.Name="chk_openingStock";
            chk_openingStock.Padding=new Padding(10, 0, 0, 0);
            chk_openingStock.Size=new Size(141, 25);
            chk_openingStock.TabIndex=34;
            chk_openingStock.Text="Opening Stock";
            chk_openingStock.UseVisualStyleBackColor=true;
            // 
            // chk_defultLedgerWiseItem
            // 
            chk_defultLedgerWiseItem.AutoSize=true;
            chk_defultLedgerWiseItem.FlatAppearance.CheckedBackColor=Color.Green;
            chk_defultLedgerWiseItem.FlatAppearance.MouseDownBackColor=Color.FromArgb(0, 64, 0);
            chk_defultLedgerWiseItem.FlatAppearance.MouseOverBackColor=Color.White;
            chk_defultLedgerWiseItem.Font=new Font("Ebrima", 12F, FontStyle.Regular, GraphicsUnit.Point);
            chk_defultLedgerWiseItem.ForeColor=Color.Black;
            chk_defultLedgerWiseItem.Location=new Point(13, 213);
            chk_defultLedgerWiseItem.Name="chk_defultLedgerWiseItem";
            chk_defultLedgerWiseItem.Padding=new Padding(10, 0, 0, 0);
            chk_defultLedgerWiseItem.Size=new Size(279, 25);
            chk_defultLedgerWiseItem.TabIndex=30;
            chk_defultLedgerWiseItem.Text="Default Ledgerwise Product Profile";
            chk_defultLedgerWiseItem.UseVisualStyleBackColor=true;
            // 
            // chk_temporary_openingStock_id
            // 
            chk_temporary_openingStock_id.AutoSize=true;
            chk_temporary_openingStock_id.FlatAppearance.CheckedBackColor=Color.Green;
            chk_temporary_openingStock_id.FlatAppearance.MouseDownBackColor=Color.FromArgb(0, 64, 0);
            chk_temporary_openingStock_id.FlatAppearance.MouseOverBackColor=Color.White;
            chk_temporary_openingStock_id.Font=new Font("Ebrima", 12F, FontStyle.Regular, GraphicsUnit.Point);
            chk_temporary_openingStock_id.ForeColor=Color.Black;
            chk_temporary_openingStock_id.Location=new Point(13, 244);
            chk_temporary_openingStock_id.Name="chk_temporary_openingStock_id";
            chk_temporary_openingStock_id.Padding=new Padding(10, 0, 0, 0);
            chk_temporary_openingStock_id.Size=new Size(221, 25);
            chk_temporary_openingStock_id.TabIndex=35;
            chk_temporary_openingStock_id.Text="Temporary Opening Stock";
            chk_temporary_openingStock_id.UseVisualStyleBackColor=true;
            // 
            // chk_priceLevelList_id
            // 
            chk_priceLevelList_id.AutoSize=true;
            chk_priceLevelList_id.FlatAppearance.CheckedBackColor=Color.Green;
            chk_priceLevelList_id.FlatAppearance.MouseDownBackColor=Color.FromArgb(0, 64, 0);
            chk_priceLevelList_id.FlatAppearance.MouseOverBackColor=Color.White;
            chk_priceLevelList_id.Font=new Font("Ebrima", 12F, FontStyle.Regular, GraphicsUnit.Point);
            chk_priceLevelList_id.ForeColor=Color.Black;
            chk_priceLevelList_id.Location=new Point(13, 275);
            chk_priceLevelList_id.Name="chk_priceLevelList_id";
            chk_priceLevelList_id.Padding=new Padding(10, 0, 0, 0);
            chk_priceLevelList_id.Size=new Size(141, 25);
            chk_priceLevelList_id.TabIndex=36;
            chk_priceLevelList_id.Text="Price Level List";
            chk_priceLevelList_id.UseVisualStyleBackColor=true;
            // 
            // chk_groupWiseItem
            // 
            chk_groupWiseItem.AutoSize=true;
            chk_groupWiseItem.FlatAppearance.CheckedBackColor=Color.Green;
            chk_groupWiseItem.FlatAppearance.MouseDownBackColor=Color.FromArgb(0, 64, 0);
            chk_groupWiseItem.FlatAppearance.MouseOverBackColor=Color.White;
            chk_groupWiseItem.Font=new Font("Ebrima", 12F, FontStyle.Regular, GraphicsUnit.Point);
            chk_groupWiseItem.ForeColor=Color.Black;
            chk_groupWiseItem.Location=new Point(13, 306);
            chk_groupWiseItem.Name="chk_groupWiseItem";
            chk_groupWiseItem.Padding=new Padding(10, 0, 0, 0);
            chk_groupWiseItem.Size=new Size(221, 25);
            chk_groupWiseItem.TabIndex=37;
            chk_groupWiseItem.Text="Groupwise Product Profile";
            chk_groupWiseItem.UseVisualStyleBackColor=true;
            // 
            // chk_groupWiseGST
            // 
            chk_groupWiseGST.AutoSize=true;
            chk_groupWiseGST.FlatAppearance.CheckedBackColor=Color.Green;
            chk_groupWiseGST.FlatAppearance.MouseDownBackColor=Color.FromArgb(0, 64, 0);
            chk_groupWiseGST.FlatAppearance.MouseOverBackColor=Color.White;
            chk_groupWiseGST.Font=new Font("Ebrima", 12F, FontStyle.Regular, GraphicsUnit.Point);
            chk_groupWiseGST.ForeColor=Color.Black;
            chk_groupWiseGST.Location=new Point(13, 337);
            chk_groupWiseGST.Name="chk_groupWiseGST";
            chk_groupWiseGST.Padding=new Padding(10, 0, 0, 0);
            chk_groupWiseGST.Size=new Size(146, 25);
            chk_groupWiseGST.TabIndex=38;
            chk_groupWiseGST.Text="Groupwise GST";
            chk_groupWiseGST.UseVisualStyleBackColor=true;
            // 
            // chk_taxMaster
            // 
            chk_taxMaster.AutoSize=true;
            chk_taxMaster.FlatAppearance.CheckedBackColor=Color.Green;
            chk_taxMaster.FlatAppearance.MouseDownBackColor=Color.FromArgb(0, 64, 0);
            chk_taxMaster.FlatAppearance.MouseOverBackColor=Color.White;
            chk_taxMaster.Font=new Font("Ebrima", 12F, FontStyle.Regular, GraphicsUnit.Point);
            chk_taxMaster.ForeColor=Color.Black;
            chk_taxMaster.Location=new Point(13, 368);
            chk_taxMaster.Name="chk_taxMaster";
            chk_taxMaster.Padding=new Padding(10, 0, 0, 0);
            chk_taxMaster.Size=new Size(119, 25);
            chk_taxMaster.TabIndex=39;
            chk_taxMaster.Text="GST Master";
            chk_taxMaster.UseVisualStyleBackColor=true;
            // 
            // chk_location
            // 
            chk_location.AutoSize=true;
            chk_location.FlatAppearance.CheckedBackColor=Color.Green;
            chk_location.FlatAppearance.MouseDownBackColor=Color.FromArgb(0, 64, 0);
            chk_location.FlatAppearance.MouseOverBackColor=Color.White;
            chk_location.Font=new Font("Ebrima", 12F, FontStyle.Regular, GraphicsUnit.Point);
            chk_location.ForeColor=Color.Black;
            chk_location.Location=new Point(13, 399);
            chk_location.Name="chk_location";
            chk_location.Padding=new Padding(10, 0, 0, 0);
            chk_location.Size=new Size(216, 25);
            chk_location.TabIndex=40;
            chk_location.Text="Location / Account Group";
            chk_location.UseVisualStyleBackColor=true;
            // 
            // chk_accountProfile
            // 
            chk_accountProfile.AutoSize=true;
            chk_accountProfile.FlatAppearance.CheckedBackColor=Color.Green;
            chk_accountProfile.FlatAppearance.MouseDownBackColor=Color.FromArgb(0, 64, 0);
            chk_accountProfile.FlatAppearance.MouseOverBackColor=Color.White;
            chk_accountProfile.Font=new Font("Ebrima", 12F, FontStyle.Regular, GraphicsUnit.Point);
            chk_accountProfile.ForeColor=Color.Black;
            chk_accountProfile.Location=new Point(13, 430);
            chk_accountProfile.Name="chk_accountProfile";
            chk_accountProfile.Padding=new Padding(10, 0, 0, 0);
            chk_accountProfile.Size=new Size(144, 25);
            chk_accountProfile.TabIndex=41;
            chk_accountProfile.Text="Account Profile";
            chk_accountProfile.UseVisualStyleBackColor=true;
            // 
            // chk_receiveblePayeble_id
            // 
            chk_receiveblePayeble_id.AutoSize=true;
            chk_receiveblePayeble_id.FlatAppearance.CheckedBackColor=Color.Green;
            chk_receiveblePayeble_id.FlatAppearance.MouseDownBackColor=Color.FromArgb(0, 64, 0);
            chk_receiveblePayeble_id.FlatAppearance.MouseOverBackColor=Color.White;
            chk_receiveblePayeble_id.Font=new Font("Ebrima", 12F, FontStyle.Regular, GraphicsUnit.Point);
            chk_receiveblePayeble_id.ForeColor=Color.Black;
            chk_receiveblePayeble_id.Location=new Point(13, 461);
            chk_receiveblePayeble_id.Name="chk_receiveblePayeble_id";
            chk_receiveblePayeble_id.Padding=new Padding(10, 0, 0, 0);
            chk_receiveblePayeble_id.Size=new Size(171, 25);
            chk_receiveblePayeble_id.TabIndex=42;
            chk_receiveblePayeble_id.Text="Receivable Payable";
            chk_receiveblePayeble_id.UseVisualStyleBackColor=true;
            // 
            // chk_groupWiseAccount
            // 
            chk_groupWiseAccount.AutoSize=true;
            chk_groupWiseAccount.FlatAppearance.CheckedBackColor=Color.Green;
            chk_groupWiseAccount.FlatAppearance.MouseDownBackColor=Color.FromArgb(0, 64, 0);
            chk_groupWiseAccount.FlatAppearance.MouseOverBackColor=Color.White;
            chk_groupWiseAccount.Font=new Font("Ebrima", 12F, FontStyle.Regular, GraphicsUnit.Point);
            chk_groupWiseAccount.ForeColor=Color.Black;
            chk_groupWiseAccount.Location=new Point(13, 492);
            chk_groupWiseAccount.Name="chk_groupWiseAccount";
            chk_groupWiseAccount.Padding=new Padding(10, 0, 0, 0);
            chk_groupWiseAccount.Size=new Size(238, 25);
            chk_groupWiseAccount.TabIndex=43;
            chk_groupWiseAccount.Text="Locationwise Account Profile";
            chk_groupWiseAccount.UseVisualStyleBackColor=true;
            // 
            // chk_Account_ClosingBalance
            // 
            chk_Account_ClosingBalance.AutoSize=true;
            chk_Account_ClosingBalance.FlatAppearance.CheckedBackColor=Color.Green;
            chk_Account_ClosingBalance.FlatAppearance.MouseDownBackColor=Color.FromArgb(0, 64, 0);
            chk_Account_ClosingBalance.FlatAppearance.MouseOverBackColor=Color.White;
            chk_Account_ClosingBalance.Font=new Font("Ebrima", 12F, FontStyle.Regular, GraphicsUnit.Point);
            chk_Account_ClosingBalance.ForeColor=Color.Black;
            chk_Account_ClosingBalance.Location=new Point(13, 523);
            chk_Account_ClosingBalance.Name="chk_Account_ClosingBalance";
            chk_Account_ClosingBalance.Padding=new Padding(10, 0, 0, 0);
            chk_Account_ClosingBalance.Size=new Size(257, 25);
            chk_Account_ClosingBalance.TabIndex=44;
            chk_Account_ClosingBalance.Text="Closing Balance Account Profile";
            chk_Account_ClosingBalance.UseVisualStyleBackColor=true;
            // 
            // chk_post_dated_voucher
            // 
            chk_post_dated_voucher.AutoSize=true;
            chk_post_dated_voucher.FlatAppearance.CheckedBackColor=Color.Green;
            chk_post_dated_voucher.FlatAppearance.MouseDownBackColor=Color.FromArgb(0, 64, 0);
            chk_post_dated_voucher.FlatAppearance.MouseOverBackColor=Color.White;
            chk_post_dated_voucher.Font=new Font("Ebrima", 12F, FontStyle.Regular, GraphicsUnit.Point);
            chk_post_dated_voucher.ForeColor=Color.Black;
            chk_post_dated_voucher.Location=new Point(13, 554);
            chk_post_dated_voucher.Name="chk_post_dated_voucher";
            chk_post_dated_voucher.Padding=new Padding(10, 0, 0, 0);
            chk_post_dated_voucher.Size=new Size(176, 25);
            chk_post_dated_voucher.TabIndex=45;
            chk_post_dated_voucher.Text="Post Dated Voucher";
            chk_post_dated_voucher.UseVisualStyleBackColor=true;
            // 
            // chk_gst_ledgers
            // 
            chk_gst_ledgers.AutoSize=true;
            chk_gst_ledgers.FlatAppearance.CheckedBackColor=Color.Green;
            chk_gst_ledgers.FlatAppearance.MouseDownBackColor=Color.FromArgb(0, 64, 0);
            chk_gst_ledgers.FlatAppearance.MouseOverBackColor=Color.White;
            chk_gst_ledgers.Font=new Font("Ebrima", 12F, FontStyle.Regular, GraphicsUnit.Point);
            chk_gst_ledgers.ForeColor=Color.Black;
            chk_gst_ledgers.Location=new Point(13, 585);
            chk_gst_ledgers.Name="chk_gst_ledgers";
            chk_gst_ledgers.Padding=new Padding(10, 0, 0, 0);
            chk_gst_ledgers.Size=new Size(119, 25);
            chk_gst_ledgers.TabIndex=46;
            chk_gst_ledgers.Text="GST Ledger";
            chk_gst_ledgers.UseVisualStyleBackColor=true;
            // 
            // btn_upload
            // 
            btn_upload.BackColor=Color.FromArgb(6, 96, 137);
            btn_upload.Dock=DockStyle.Bottom;
            btn_upload.FlatAppearance.BorderColor=Color.FromArgb(6, 96, 137);
            btn_upload.FlatAppearance.BorderSize=0;
            btn_upload.FlatAppearance.MouseDownBackColor=Color.FromArgb(7, 58, 82);
            btn_upload.FlatAppearance.MouseOverBackColor=Color.FromArgb(14, 123, 173);
            btn_upload.FlatStyle=FlatStyle.Flat;
            btn_upload.Font=new Font("Sylfaen", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btn_upload.ForeColor=Color.White;
            btn_upload.Location=new Point(13, 616);
            btn_upload.Name="btn_upload";
            btn_upload.Size=new Size(279, 32);
            btn_upload.TabIndex=49;
            btn_upload.Text="Upload";
            btn_upload.UseVisualStyleBackColor=false;
            btn_upload.Click+=btn_upload_Click;
            // 
            // uploadLocationHeirarchyButton
            // 
            uploadLocationHeirarchyButton.BackColor=Color.FromArgb(6, 96, 137);
            uploadLocationHeirarchyButton.Dock=DockStyle.Bottom;
            uploadLocationHeirarchyButton.FlatAppearance.BorderColor=Color.FromArgb(6, 96, 137);
            uploadLocationHeirarchyButton.FlatAppearance.BorderSize=0;
            uploadLocationHeirarchyButton.FlatAppearance.MouseDownBackColor=Color.FromArgb(7, 58, 82);
            uploadLocationHeirarchyButton.FlatAppearance.MouseOverBackColor=Color.FromArgb(14, 123, 173);
            uploadLocationHeirarchyButton.FlatStyle=FlatStyle.Flat;
            uploadLocationHeirarchyButton.Font=new Font("Sylfaen", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            uploadLocationHeirarchyButton.ForeColor=Color.White;
            uploadLocationHeirarchyButton.Location=new Point(13, 654);
            uploadLocationHeirarchyButton.Name="uploadLocationHeirarchyButton";
            uploadLocationHeirarchyButton.Size=new Size(279, 32);
            uploadLocationHeirarchyButton.TabIndex=54;
            uploadLocationHeirarchyButton.Text="Location-Heirarchy";
            uploadLocationHeirarchyButton.UseVisualStyleBackColor=false;
            uploadLocationHeirarchyButton.Visible=false;
            uploadLocationHeirarchyButton.Click+=uploadLocationHeirarchyButton_Click;
            // 
            // loggerPanel
            // 
            loggerPanel.BackColor=Color.White;
            loggerPanel.Dock=DockStyle.Fill;
            loggerPanel.Location=new Point(299, 0);
            loggerPanel.Name="loggerPanel";
            loggerPanel.Size=new Size(501, 632);
            loggerPanel.TabIndex=3;
            // 
            // UC_UserHome
            // 
            AutoScaleDimensions=new SizeF(7F, 15F);
            AutoScaleMode=AutoScaleMode.Font;
            Controls.Add(loggerPanel);
            Controls.Add(flowLayoutPanel1);
            Name="UC_UserHome";
            Size=new Size(800, 632);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private FlowLayoutPanel flowLayoutPanel1;
        private ComboBox companySelect;
        private Button btn_upload;
        private Panel panel2;
        private FlowLayoutPanel loggerPanel;
        private CheckBox chk_groupWiseAccount;
        private CheckBox chk_Account_ClosingBalance;
        private CheckBox chk_post_dated_voucher;
        private CheckBox chk_selectAll;
        private CheckBox chk_productGroup;
        private CheckBox chk_productCategory;
        private CheckBox chk_productProfile;
        private CheckBox chk_openingStock;
        private CheckBox chk_defultLedgerWiseItem;
        private CheckBox chk_temporary_openingStock_id;
        private CheckBox chk_priceLevelList_id;
        private CheckBox chk_groupWiseItem;
        private CheckBox chk_groupWiseGST;
        private CheckBox chk_taxMaster;
        private CheckBox chk_location;
        private CheckBox chk_accountProfile;
        private CheckBox chk_receiveblePayeble_id;
        private CheckBox chk_gst_ledgers;
        private CheckBox chk_fullUpdate;
        private Button uploadLocationHeirarchyButton;
        private Panel panel1;
    }
}
