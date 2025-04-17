using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
using SNR_ClientApp.Tally.generateXml;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    public class CompanyService
    {
        Dictionary<string, string> props = new Dictionary<string, string>();
        TallyCommunicator tallyCommunicator = new TallyCommunicator();
        internal async Task<List<CompanyDTO>> GetCompanies()
        {
            LogManager.WriteLog("listing company started...");
            DataTable table = await tallyCommunicator.getdatatable("SELECT $Name,$Guid,$STATENAME FROM " + Tables.Company);

            //object[] row = { table.Rows[0]["$Name"].ToString() };
            LogManager.WriteLog("listing company ended...");

            List<CompanyDTO> _list = new List<CompanyDTO>();
            foreach (DataRow dr in table.Rows)
            {
                _list.Add(new CompanyDTO
                {
                    guid = ((string)dr["$Guid"]),
                    companyName = dr["$Name"].ToString(),
                    stateName = dr["$STATENAME"].ToString()
                });
            }
            return _list;

        }

        public static String getCompanyName()
        {
            return StringUtilsCustom.TALLY_COMPANY==null? ApplicationProperties.properties["tally.company"].ToString(): StringUtilsCustom.TALLY_COMPANY;
        }

        internal async Task<string> getCurrentActiveCompanyAsync()
        {
            string companyName = "";
            try
            {
                CompanyXml companyXml = new CompanyXml();
                var xmlrequest = companyXml.getActiveCompanyRequestXml();


                TallyCommunicator tallyCommunicator = new TallyCommunicator();
                var res = await tallyCommunicator.ExecXml(xmlrequest);
                if (res!=null)
                {
                     companyName = res.response.BODY?.DATA?.COLLECTION?.CURRENTCOMPANY?.CurrentCompany?.Text;
                }
            }catch (Exception ex)
            {
                LogManager.WriteLog("exception occured while getting current active company using xml \n "+ex.Message);
                LogManager.HandleException(ex);
                companyName= "";
            }
            return companyName;
        }
    }
}
