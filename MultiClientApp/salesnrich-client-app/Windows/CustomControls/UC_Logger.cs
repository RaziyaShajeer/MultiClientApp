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
    public partial class UC_Logger : UserControl
    {
        public UC_Logger()
        {
            InitializeComponent();
        }

        public void AppendLogMsg(String msg)
        {
            loggerArea.Text += Environment.NewLine+ msg;
        }

        public void ClearLogArea()
        {
            loggerArea.Text ="";
        }
    }
}
