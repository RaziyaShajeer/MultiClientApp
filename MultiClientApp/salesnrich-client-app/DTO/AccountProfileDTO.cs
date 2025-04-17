using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class AccountProfileDTO
    {
        public static readonly long serialVersionUID = 1L;

        public long? alterId { get; set; }

        public String pid{ get; set; }

        public String name{ get; set; }

        public String alias{ get; set; }

        public String description{ get; set; }

        public String accountTypeName{ get; set; }

        public String address{ get; set; }

        public String city{ get; set; }

        public String location{ get; set; }

        public String pin{ get; set; }

        public String phone1{ get; set; }

        public String email1{ get; set; }

        public String defaultPriceLevelName{ get; set; }

        public String accountStatus{ get; set; }

        public bool activated = true;

        public double? closingBalance{ get; set; }

        public String trimChar{ get; set; }

        public String stateName{ get; set; }

        public String countryName{ get; set; }

        public String gstRegistrationType{ get; set; }

        public String customerId{ get; set; }

        public String tinNo{ get; set; }

        public String tallyLedgerType{ get; set; }

        public String mailingName{ get; set; }
        public string createdDate { get; set; }
	}
}
