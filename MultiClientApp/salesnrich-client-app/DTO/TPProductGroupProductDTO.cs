using SNR_ClientApp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    internal class TPProductGroupProductDTO
    {
        private static readonly long serialVersionUID = 1L;
        public String productId { get;set; }
        public long alterId { get; set; }
        public String productName { get; set; }
        public String groupName { get; set; }

		public string distributorCode = ApplicationProperties.properties["DistributedCode"].ToString();

	}
}
