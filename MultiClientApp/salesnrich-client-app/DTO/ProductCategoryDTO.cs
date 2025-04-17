using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    internal class ProductCategoryDTO
    {
        public String? pid { get; set; } 

        public double alterId { get; set; }

        public String name { get; set; }

        public String? alias { get; set; }

        public String? description { get; set; }

        public bool activated { get; set; }

        public String productCategoryId { get; set; }

    }
}
