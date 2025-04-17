using SNR_ClientApp.Properties;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    internal class ProductGroupDTO
    {
        public String pid { get; set; }
        public double alterId = 0;
        public String name  { get; set; }
		public string displayName { get; set; }
		public string distributorCode { get; set; }
		public string distributorName { get; set; }
		public String alias { get; set; }

        public String description { get; set; }

        public double taxRate { get; set; }

        public String productGroupId { get; set; }
        public ProductGroupDTO(string ProductGroupId,string Name,double Taxrate,double AlterId)
        {
            productGroupId = ProductGroupId;

            displayName = Name;
            alterId = AlterId;
            taxRate = Taxrate;
			this.name = DistributedCodeAppend.appendDistributedCode(Name);
			this.distributorCode = ApplicationProperties.properties["DistributedCode"].ToString();
			this.distributorName = ApplicationProperties.properties["DistributedCodeCompany"].ToString();
        }
	}
}
