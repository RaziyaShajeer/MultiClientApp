using SNR_ClientApp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class ProductCategoryDTO
    {
        public String? pid { get; set; } 

        public double alterId { get; set; }

        public String name { get; set; }

        public String? alias { get; set; }

        public String? description { get; set; }

        public bool activated { get; set; }

        public String productCategoryId { get; set; }
		public string tallyCompanyName { get; set; }
		public ProductCategoryDTO()
        {
			tallyCompanyName = ApplicationProperties.properties["tally.company"].ToString();
		}
	

	}
}
