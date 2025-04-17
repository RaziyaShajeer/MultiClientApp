using SNR_ClientApp.Utils;
using SNR_ClientApp.Windows.CustomControls.AutomationControls;
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
    public partial class UC_Settings_login : UserControl
    {
        public TallyConfigForm ParentForm { get; set; }
        public UC_Settings_login()
        {
            InitializeComponent();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            var username = RestClientUtil.getLoggedUser();
            var password = RestClientUtil.getLoggedUserPassword();
            if (txt_username.Text == username && txt_password.Text == password)
            {
                TalllyConnect talllyConnect = new TalllyConnect();
                talllyConnect.ParentForm = ParentForm;
                ParentForm.AddUserControl(talllyConnect);
            }
            else
            {
                MessageBox.Show("User Verification Failed.. ");
            }
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            MainForm mainform = new MainForm();
            mainform.Show();
            this.Hide();
            ParentForm.Hide();
        }

    
    }
}
