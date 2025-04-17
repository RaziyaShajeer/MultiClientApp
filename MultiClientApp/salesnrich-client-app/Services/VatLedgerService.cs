using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    internal class VatLedgerService
    {
        readonly TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        private bool fullUpdate = true;
        private string idClentApp;
        private String tallyLedgerParent;
        public VatLedgerService()
        {

            tallyCommunicator = new TallyCommunicator();
            httpClient = new HttpClient();
            idClentApp = ApplicationProperties.properties.GetValueOrDefault("idclientapp").ToString();
            tallyLedgerParent = ApplicationProperties.properties.GetValueOrDefault("tally.ledger.parent").ToString();
          
        }
        internal async Task<List<VatLedgerDTO>> getFromTallyAndUpload()
        {
            List<VatLedgerDTO> _list = new List<VatLedgerDTO>();
            DataTable response = new DataTable();
            StringBuilder Query = new StringBuilder();
            Query.Append("select $name, $Parent, $Address,$TAXTYPE,$SUBTAXTYPE,$TAXCLASSIFICATIONNAME,$VATCLASSIFICATIONRATE,$PRICELEVEL,$AlterID,$RATEOFTAXCALCULATION from " + Tables.Ledger + "  where $parent = duties & taxes ");

            response = await tallyCommunicator.getdatatable(Query.ToString());
            if (response.Rows.Count > 0)
            {
                //TaxMasterDTO taxMasterDTO = new TaxMasterDTO();
                foreach (DataRow dr in response.Rows)
                {
                    string taxtype = (dr["$TAXTYPE"] != DBNull.Value) ? (string)dr["$TAXTYPE"] : "";
                    if (taxtype.Equals("VAT"))
                    {
                        VatLedgerDTO taxMasterDTO = new VatLedgerDTO();

                        taxMasterDTO.name = (dr["$name"] != DBNull.Value) ? (string)dr["$name"] : "";
                        taxMasterDTO.vatClass = (dr["$TAXCLASSIFICATIONNAME"] != DBNull.Value) ? (string)dr["$TAXCLASSIFICATIONNAME"] : "";
                        taxMasterDTO.percentageOfCalculation = (dr["$VATCLASSIFICATIONRATE"] != DBNull.Value) ? (StringUtilsCustom.ExtractDoubleValue(dr["$VATCLASSIFICATIONRATE"].ToString())) : 0;
                        var percentageOfCalculation = (dr["$RATEOFTAXCALCULATION"] != DBNull.Value) ? (StringUtilsCustom.ExtractDoubleValue(dr["$RATEOFTAXCALCULATION"].ToString())) : 0;
                        if (taxMasterDTO.percentageOfCalculation==0)
                        {
                            taxMasterDTO.percentageOfCalculation=percentageOfCalculation;
                        }
                        _list.Add(taxMasterDTO);
                    }
                }
               

               
            }
            return _list;
        }
    }
}
