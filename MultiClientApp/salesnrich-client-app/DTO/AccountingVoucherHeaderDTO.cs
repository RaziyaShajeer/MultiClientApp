using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class AccountingVoucherHeaderDTO
    {
        public String pid { get; set; }

        public String documentPid{ get; set; }

        public String documentName{ get; set; }

        public String accountProfilePid{ get; set; }

        public String accountProfileName{ get; set; }

        public String createdDate{ get; set; }

        public String documentDate{ get; set; }

        public String phone{ get; set; }

        public String employeePid{ get; set; }

        public String employeeName{ get; set; }

        public String userName{ get; set; }

        public double totalAmount{ get; set; }

        public double outstandingAmount{ get; set; }

        public String remarks{ get; set; }

        public List<AccountingVoucherDetailDTO> accountingVoucherDetails{ get; set; }

        public String documentNumberLocal{ get; set; }

        public String documentNumberServer{ get; set; }

        public List<AccountingVoucherHeaderDTO> history{ get; set; }

        /* for show in aac voucher report */
        public double byAmount{ get; set; }

        public double toAmount{ get; set; }

        // SaveOrUpdate Dashboard update
        public bool isNew = false;
    }
}
