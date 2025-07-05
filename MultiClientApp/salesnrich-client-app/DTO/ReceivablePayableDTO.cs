using SNR_ClientApp.Enums;
using SNR_ClientApp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class ReceivablePayableDTO
    {
		public String customerId { get; set; }
		public String accountName { get; set; }

        public String referenceDocumentNumber { get; set; }

        public DateTime referenceDocumentDate { get; set; }

        public String referenceDocumentType { get; set; }

        public double referenceDocumentAmount { get; set; }

        public double referenceDocumentBalanceAmount { get; set; }

        public String remarks { get; set; }

        public String billOverDue { get; set; }
		public string tallyCompanyName { get; set; }

		public ReceivablePayableType receivablePayableType { get; set; }
        public ReceivablePayableDTO()
        {
			tallyCompanyName = ApplicationProperties.properties["tally.company"].ToString();
		}
    
    }
}
