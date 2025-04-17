using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SNR_ClientApp.TallyResponses
{
    [XmlRoot(ElementName = "LISTOFLEDGERS")]
    public class ListOfLedgers
    {
        [XmlElement(ElementName = "LEDGER")]
        public LEDGER LEDGER { get; set; }
    }
}
