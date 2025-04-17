using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SNR_ClientApp.TallyResponses
{
    [XmlRoot(ElementName = "StockSummary")]
    public class StockSummaryBatchWiseResponse
    {

        [XmlElement(ElementName = "DSPACCNAME")]
        public DSPACCNAME DSPACCNAME { get; set; }

        [XmlElement(ElementName = "ITEM")]
        public List<ITEM> ITEMs { get; set; }

        //[XmlElement(ElementName = "SSBATCHNAME")]
        //public List<SSBATCHNAME> SSBATCHNAME { get; set; }
    }

    [XmlRoot(ElementName = "StockBatchSub")]
    public class StockBatchSub
    {

        [XmlElement(ElementName = "DSPSTKINFO")]
        public DSPSTKINFO? DSPSTKINFO { get; set; }

        [XmlElement(ElementName = "SSBATCHNAME")]
        public SSBATCHNAME? SSBATCHNAME { get; set; }

        [XmlElement(ElementName = "DSPDISPNAME")]
        public string? DSPDISPNAME { get; set; }
    }

    [XmlRoot(ElementName = "ITEM")]
    public class ITEM
    {

        [XmlElement(ElementName = "DSPSTKINFO")]
        public DSPSTKINFO DSPSTKINFO { get; set; }

        [XmlElement(ElementName = "SSBATCHNAME")]
        public SSBATCHNAME SSBATCHNAME { get; set; }
    }


}
