
using SNR_ClientApp.DTO;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Parsers
{
    public class SalesVoucherLedgerTallymasterResponseParser
    {
        public List<AccountProfileDTO> ParseSalesVoucherLedgerListXml(string tallyResponseXml)
        {
            tallyResponseXml = tallyResponseXml.Replace("&#4;", " ").Replace("[&]", "");

            StringBuilder savedSalesLedgerXml = new StringBuilder();
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(tallyResponseXml)))
            {
                savedSalesLedgerXml.Append(GetStringFromInputStream(stream));
            }

            HashSet<string> accountProfileDTOString = new HashSet<string>();
            string[] savedSalesLedgers = savedSalesLedgerXml.ToString().Split('~');
            foreach (string legdgerName in savedSalesLedgers)
            {
                //string name = legdgerName.Replace(" ", string.Empty);
                if (!String.IsNullOrEmpty(legdgerName))
                {
                    accountProfileDTOString.Add(legdgerName);
                }
            }

            List<AccountProfileDTO> accountProfileDTOs = new List<AccountProfileDTO>();
            foreach (string name in accountProfileDTOString)
            {
                AccountProfileDTO accountProfileDTO = new AccountProfileDTO
                {
                    name = name
                };
                accountProfileDTOs.Add(accountProfileDTO);
            }

            return accountProfileDTOs;
        }

        private static string GetStringFromInputStream(Stream inputStream)
        {
            System.IO.StreamReader reader = null;
            StringBuilder sb = new StringBuilder();
            string line;
            try
            {
                reader = new System.IO.StreamReader(inputStream);
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("PARTYLEDGERNAME"))
                    {
                        string value = line.Substring(1, line.Length - 19);
                        value = value.Substring(35);
                        sb.Append(StringUtilsCustom.replaceSpecialCharacters(value) + "~");
                    }
                }
            }
            catch (System.IO.IOException e)
            {
                System.Diagnostics.Trace.WriteLine(e.StackTrace);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return sb.ToString();
        }

       
    }
}
