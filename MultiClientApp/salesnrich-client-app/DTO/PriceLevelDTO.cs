using SNR_ClientApp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class PriceLevelDTO
    {
        public String name;
        public bool activated = true;
		//Todo : need to very that productId is required in this dto
		// public String productId { get; set; }
		public string tallyCompanyName { get; set; }
		public PriceLevelDTO(String name, bool activated)
        {
			tallyCompanyName = ApplicationProperties.properties["tally.company"].ToString();
			this.name = name;
            this.activated = activated;
        }
    }
}
