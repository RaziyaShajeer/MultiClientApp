using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    internal class GSTProductGroupWiseDTO
    {
        public String productGroupName { get; set; }
        public String applyDate { get; set; }
        public String hsnsacCode { get; set; }
        public String taxType { get; set; }
        public String integratedTax { get; set; }
        public String centralTax { get; set; }
        public String stateTax { get; set; }
        public String aditionalCess { get; set; }
    }
}
