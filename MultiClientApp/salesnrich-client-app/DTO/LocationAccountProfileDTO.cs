using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.DTO
{
    internal class LocationAccountProfileDTO
    {
        private static readonly long serialVersionUID = 1L;

        public long alterId;
        public String accountProfileName;
        public String locationName;
        public DateTime lastModifiedDate;
        //Customer Id Added 
        public String customer_id { get; set; }
    }
}
