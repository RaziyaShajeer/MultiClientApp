using SNR_ClientApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class GstLedgerDTO
    {
        public String name { get; set; }
        public String taxType { get; set; }
        public GstAccountType accountType { get; set; }
        public double taxRate { get; set; }
        public double totalTaxAmt { get; set; }
        public bool activated { get; set; }
        public String gstDutyHead { get; set; }
    }
}
