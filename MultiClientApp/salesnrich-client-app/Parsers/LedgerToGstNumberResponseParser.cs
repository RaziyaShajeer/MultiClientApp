using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Services;
using SNR_ClientApp.Tally;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SNR_ClientApp.Parsers
{
    public class LedgerToGstNumberResponseParser
    {
        TallyCommunicator tallyCommunicator;
        HttpClient httpClient;
        public LedgerToGstNumberResponseParser()
        {
            tallyCommunicator=new TallyCommunicator();
            httpClient=new HttpClient();
        }
        internal async Task<string> getGstNumberAsync(string ledgerName)
        {
            string gstNumber = "";

            // 9/11/2023
            // getting gstnumber using odbc may produce errors so using xml request for all 

            // List<LocationDTO> _list = new List<LocationDTO>();
            // DataTable response = new DataTable();
            // StringBuilder Query = new StringBuilder();
            // string pattern = @"^\d";
            // //   Query.Append("select $name,$PartyGSTIn from " + Tables.Ledger + " where $name = '" + ledgerName+"'");
            // if (!Regex.IsMatch(ledgerName, pattern))
            // {
            //     Query.Append("select $name,$PartyGSTIn from " + Tables.Ledger + " where $name = " + ledgerName);

            // }
            //else if ( !ledgerName.Contains("\""))
            //     Query.Append("select $name,$PartyGSTIn from " + Tables.Ledger + " where $name = \"" + ledgerName+"\" ");
            // else if (!ledgerName.Contains("\'"))
            //     Query.Append("select $name,$PartyGSTIn from " + Tables.Ledger + " where $name = \'" + ledgerName+"\' ");
            // else
            // {
            //     TallyService tallyService = new TallyService();
            //     gstNumber= await tallyService.GetGstNumberOfLedgerXMLAsync(ledgerName);
            //     return gstNumber;
            // }
            // response = tallyCommunicator.getdatatable(Query.ToString());

            // if (response.Rows.Count > 0)
            // {


            //     foreach (DataRow dr in response.Rows)
            //     {

            //         gstNumber = (dr["$PartyGSTIn"] != DBNull.Value) ? (string)dr["$PartyGSTIn"] : "";
            //     }


            // }
            // return gstNumber;


            TallyService tallyService = new TallyService();
            gstNumber= await tallyService.GetGstNumberOfLedgerXMLAsync(ledgerName);
            return gstNumber;

        }
    }
}
