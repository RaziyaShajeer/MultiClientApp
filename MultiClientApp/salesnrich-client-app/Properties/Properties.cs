using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Properties
{
    internal class Properties
    {
        public string isFirstTimeLogin { get; set; }
        [DisplayName("tally.company")]
        public string tallyCompany { get; set; }
        [DisplayName("tally.full.url")]
        public string tallyFullUrl { get; set; }
        [DisplayName("tally.hostname")]
        public string tallyHostname { get; set; }
        [DisplayName("tally.port")]
        public string tallyPort { get; set; }
        //public string tallyCompany { get; set; }


    }
}
