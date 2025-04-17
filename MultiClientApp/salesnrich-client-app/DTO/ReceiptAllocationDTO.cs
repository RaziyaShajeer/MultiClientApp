using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class ReceiptAllocationDTO
    {
        public double amount{get;set;}
        public String reference{get;set;}
        public String remarks{get;set;}
        public String provisionalReceiptNo{get;set;}
        public PaymentMode paymentMode{get;set;}
        public double detailAmount{get;set;}
        public long detailId{get;set;}
    }
}
