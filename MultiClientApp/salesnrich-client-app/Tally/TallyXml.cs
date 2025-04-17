using SNR_ClientApp.TallyResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Tally
{
    public class TallyXml
    {
        public String pid { get; set; }
        public ENVELOPE xmlObj { get; set; }
        public TallyXml()
        {

        }
        public TallyXml(string pid, ENVELOPE xmlObj)
        {
            this.pid = pid;
            this.xmlObj = xmlObj;
        }
    }
}
