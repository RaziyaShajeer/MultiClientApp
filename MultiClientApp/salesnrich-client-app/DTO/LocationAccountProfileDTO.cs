using SNR_ClientApp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class LocationAccountProfileDTO
    {
        private static readonly long serialVersionUID = 1L;

        public long alterId;
        public String accountProfileName;
        public String locationName;
        public DateTime lastModifiedDate;
        //Customer Id Added 
        public String customer_id { get; set; }
		public string tallycompanyName { get; set; }
        public LocationAccountProfileDTO()
        {
			tallycompanyName = ApplicationProperties.properties["tally.company"].ToString();
		}
	}
}
