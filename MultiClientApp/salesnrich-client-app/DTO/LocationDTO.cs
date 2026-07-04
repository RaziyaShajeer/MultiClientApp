using SNR_ClientApp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class LocationDTO
    {
        public long alterId { get; set; }
        public String name { get; set; }
        public String description { get; set; }
        public bool activated = true;
		public string tallyName { get; set; }


		public LocationDTO()
        {
			tallyName = ApplicationProperties.properties["tally.company"].ToString();
		}
        public LocationDTO(String name, String description)
        {
           
            this.name = name;
            this.description = description;
            this.activated = true;
			tallyName = ApplicationProperties.properties["tally.company"].ToString();

		}

        public String locationId { get; set; }
    }
}
