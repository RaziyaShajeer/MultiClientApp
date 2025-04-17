using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class AccountingVoucherDetailDTO
    {
        public PaymentMode mode { get; set; }

        public double amount{ get; set; }

        public String instrumentNumber{ get; set; }

        public String instrumentDate{ get; set; }

        public String bankPid{ get; set; }

        public String bankName{ get; set; }

        public String byAccountPid{ get; set; }

        public String byAccountName{ get; set; }

        public String toAccountPid{ get; set; }

        public String toAccountName{ get; set; }

        public String voucherNumber{ get; set; }

        public DateTime voucherDate{ get; set; }

        public String referenceNumber{ get; set; }

        public String remarks{ get; set; }

        public String incomeExpenseHeadPid{ get; set; }

        public String incomeExpenseHeadName{ get; set; }

        public String provisionalReceiptNo{ get; set; }

    }
}
