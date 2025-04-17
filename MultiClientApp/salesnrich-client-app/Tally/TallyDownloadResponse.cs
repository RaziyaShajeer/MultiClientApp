using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SNR_ClientApp.Tally
{
    public class TallyDownloadResponse
    {
  public RESPONSE response { get; set; }
     

    }

    [XmlRoot(ElementName = "RESPONSE")]
    public class RESPONSE
    {

        [XmlElement(ElementName = "LINEERROR")]
        public string LINEERROR { get; set; }

        [XmlElement(ElementName = "CREATED")]
        public int CREATED { get; set; }

        [XmlElement(ElementName = "ALTERED")]
        public int ALTERED { get; set; }

        [XmlElement(ElementName = "DELETED")]
        public int DELETED { get; set; }

        [XmlElement(ElementName = "LASTVCHID")]
        public int LASTVCHID { get; set; }

        [XmlElement(ElementName = "LASTMID")]
        public int LASTMID { get; set; }

        [XmlElement(ElementName = "COMBINED")]
        public int COMBINED { get; set; }

        [XmlElement(ElementName = "IGNORED")]
        public int IGNORED { get; set; }

        [XmlElement(ElementName = "ERRORS")]
        public int ERRORS { get; set; }

        [XmlElement(ElementName = "CANCELLED")]
        public int CANCELLED { get; set; }
        [XmlElement(ElementName = "EXCEPTIONS")]
        public int EXCEPTIONS { get; set; }

        [XmlElement(ElementName = "LASTVCHNO")]
        public string LASTVCHNO { get; set; } // Added Voucher Number

    }


}
