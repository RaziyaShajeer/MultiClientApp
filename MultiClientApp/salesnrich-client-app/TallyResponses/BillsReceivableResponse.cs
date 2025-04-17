using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SNR_ClientApp.TallyResponses
{
    [XmlRoot(ElementName = "Root")]
    public class BillsReceivableResponse
    {
        [XmlElement(ElementName = "BILLFIXED")]
        public BILLFIXED? BILLFIXED { get; set; }

        [XmlElement(ElementName = "BILLCL")]
        public string? BILLCL { get; set; }

        [XmlElement(ElementName = "BILLOP")]
        public string? BILLOP { get; set; }

        [XmlElement(ElementName = "BILLDUE")]
        public String ? BILLDUE { get; set; }

        [XmlElement(ElementName = "BILLOVERDUE")]
        public string? BILLOVERDUE { get; set; }
    }


    [XmlRoot(ElementName = "BILLFIXED")]
    public class BILLFIXED
    {

        [XmlElement(ElementName = "BILLDATE")]
        public String BILLDATE { get; set; }

        [XmlElement(ElementName = "BILLREF")]
        public string BILLREF { get; set; }

        [XmlElement(ElementName = "BILLPARTY")]
        public string BILLPARTY { get; set; }
    }
}
