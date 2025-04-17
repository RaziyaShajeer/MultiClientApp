using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class ReceiptDTO
    {

        public String accountingVoucherHeaderPid{get;set;}
        public String particularsName{get;set;}
        public double amount{get;set;}
        public double detailAmount{get;set;}
        public long detailId{get;set;}
        public double headerAmount{get;set;}
        public String reference{get;set;}
        public String date{get;set;}
        public String chequeNo{get;set;}
        public String bankName{get;set;}
        public String remoteId{get;set;}
        public String ledgerName{get;set;}
        public String narrationMessage{get;set;}
        public String userName{get;set;}
        public String trimChar{get;set;}
        public String chequeDate{get;set;}
        public PaymentMode mode{get;set;}
        public String employeeName{get;set;}
        public String provisionalReceiptNo{get;set;}
        public String employeeAlias{get;set;}
        public List<ReceiptAllocationDTO> receiptAllocationList{get;set;}
        public String godownName{get;set;}
    }
}
