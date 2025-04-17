using SNR_ClientApp.Tally.generateXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Tally
{
    public class TallySalesOrderProcessor
    {
        SalesOrderGenerateXmlfromMasterID salesOrderGenerateXmlfromMasterID;
        HttpClient httpClient;
        public TallySalesOrderProcessor(SalesOrderGenerateXmlfromMasterID _salesOrderGenerateXmlfromMasterID)
        {
            salesOrderGenerateXmlfromMasterID=_salesOrderGenerateXmlfromMasterID;
            httpClient = new();
        }

    }
}
