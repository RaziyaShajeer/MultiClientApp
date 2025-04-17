using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    public class TaxMasterDTO
    {

        public long? alterId { get; set; }
        public String pid { get; set; }
        public String vatName { get; set; }
        public String description { get; set; }
        public double? vatPercentage { get; set; }
        public String companyName { get; set; }
        public String companyPid { get; set; }
        public String vatClass { get; set; }

    }
}
