using SNR_ClientApp.DTO;
using SNR_ClientApp.Enums;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Tally;
using SNR_ClientApp.Tally.generateXml;
using SNR_ClientApp.TallyResponses;
using SNR_ClientApp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Newtonsoft.Json;
using SNR_ClientApp.Parsers;


namespace SNR_ClientApp.Services
{
    public class TallyService
    {
		public OdbcConnection con;
		public static Dictionary<string, string> props = new Dictionary<string, string>();
        TallyCommunicator tallyCommunicator = new TallyCommunicator();

        public Boolean Connect(String host, String port,String odbcDsn = "")
        {
            try
            {
                props.Clear();
                props.Add("tally.hostname", host);
                props.Add("tally.port", port);
                String fullurl = "http://" + host + ":" + port;
                props.Add("tally.full.url", fullurl);
                props.Add("tally.odbcdsn", odbcDsn);
                // string odbcDsn = ApplicationProperties.properties["tally.odbcdsn"].ToString();
                // if (String.IsNullOrEmpty(odbcDsn))
                //odbcDsn="TallyODBC64_"+port?.Trim();
                //props.Add("tally.odbcdsn", odbcDsn);
                ApplicationProperties.setProperties(props);
                if(!host.Equals("localhost",StringComparison.OrdinalIgnoreCase))
                {
                  return TryConnectTally();
                }
                OdbcConnection con = GetConnection();
                if (con != null && con.State == ConnectionState.Open)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (OdbcException ex)
            {
                Console.WriteLine("Tally connection failed" + ex.Message);
            }catch(Exception ex)
            {
                LogManager.HandleException(ex);
            }
            return false;


        }
		public OdbcConnection GetConnection()
		{
			string source = "";
			try
			{
				LogManager.WriteLog("Get Connection to Tally ");
				//props = ApplicationProperties.getAllProperties();
				//source = "DSN=TallyODBC64_9000;PORT=9000;DRIVER=;DRIVER=Tally ODBC Driver64;SERVER=192.168.1.12";
				//source = "DSN=TallyODBC64_9000;PORT=" + props.GetValueOrDefault("tally.port") + ";DRIVER=Tally ODBC Driver;SERVER={(" + props.GetValueOrDefault("tally.hostname") + ")}";
				//source = "Driver={Tally ODBC Driver};Server="+props.GetValueOrDefault("tally.hostname")+";Port="+ props.GetValueOrDefault("tally.port")+";DSN=TallyODBC64_9000;";
				string odbcDsn = ApplicationProperties.userinitialproperty["tally.odbcdsn"].ToString();
				//source = "SERVER="+ props.GetValueOrDefault("tally.hostname") +";DSN=TallyODBC64_"+ props.GetValueOrDefault("tally.port")+";PORT=" + props.GetValueOrDefault("tally.port") + ";DRIVER=Tally ODBC Driver64;" ;
				source = "SERVER=" + ApplicationProperties.userinitialproperty["tally.hostname"] + ";DSN=" + odbcDsn + ";PORT=" + props.GetValueOrDefault("tally.port") + ";DRIVER=Tally ODBC Driver64;";

				//LogManager.WriteLog("\nConnection String : " + source);
				//con.Dispose();
				//string source = "DSN=TallyODBC64_9000;PORT=" + props.GetValueOrDefault("port") + ";DRIVER=Tally ODBC Driver;SERVER={(" + props.GetValueOrDefault("host") + ")}";
				con = new OdbcConnection(source);
				//OdbcConnection con = new OdbcConnection("Data Source=DESKTOP-INJ5THJ\\MSSQLEXPRESS; Initial Catalog = StudentManagementSystem; Integrated Security = True;");
				//SqlConnection con = new SqlConnection("Data Source=AITRICH-WIN8\\sqlexpress;Initial Catalog=StudentManagementSystem;User ID=students;Password=password");
				if (con.State == ConnectionState.Open)
				{

					con.Close();

				}
				con.Open();
				return con;
			}
			catch (Exception e)
			{
				LogManager.WriteLog("Exception occured while getting Connection to Tally");
				LogManager.HandleException(e, "Connection String : " + source);
				throw e;
			}
		}
		public bool TryConnectTally()
		{
			try
			{
				HttpClient client = new HttpClient();
				client.BaseAddress = new Uri(ApplicationProperties.userinitialproperty.GetValueOrDefault("tally.full.url").ToString());
				var responseTask = client.GetAsync("");

				responseTask.Wait();

				HttpResponseMessage Res = responseTask.Result;
				if (Res.IsSuccessStatusCode)
				{
					return true;
				}
				else
				{
					return false;
				}

			}
			catch (Exception ex)
			{
				LogManager.HandleException(ex);
				return false;
			}
		}

		internal async Task<object[]> getCompanies()
        {
            List<String> Groups = new List<string>();
           // LogManager.WriteLog("listing company started...");
            DataTable response = await tallyCommunicator.getdatatableofTAlly("SELECT $Name FROM " + Tables.Company);
            if (response.Rows.Count > 0)
            {

                foreach (DataRow dr in response.Rows)
                {

                    Groups.Add(((string)dr["$name"]));
                }

            }

            LogManager.WriteLog("listing company ended...");
            return Groups.ToArray();


        }

        internal async  Task<List<LocationDTO>> getAllGroups()
        {
			ENVELOPE tallyRequest = new ENVELOPE();
			LogManager.WriteLog("listing Groups started...");
			tallyRequest = CompanygroupGenerateXml.getCompanyGroupsXml();
			var stringwriter = new System.IO.StringWriter();
			System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(tallyRequest.GetType());
			x.Serialize(stringwriter, tallyRequest);

			var data = await tallyCommunicator.ExecXmlAndGetXmlAsync(stringwriter.ToString());
			List<LocationDTO> _list = new List<LocationDTO>();
			_list = AccountGroupResponseParser.CompanyGroupresponseParser(data);

			
			return _list;
        }
		public async Task<String[]> getAllLedgersNamesByParentcess(String Parent)
		{
			List<String> Groups = new List<string>();

			DataTable response = new DataTable();
			StringBuilder Query = new StringBuilder();

			string query = $"SELECT $Name FROM {Tables.Ledger} WHERE $Parent = '{Parent}' OR $Parent = 'GL 13; Duties & Taxes'";
			Query.Append(query);
			response = await tallyCommunicator.getdatatable(Query.ToString());

			if (response.Rows.Count > 0)
			{

				foreach (DataRow dr in response.Rows)
				{
					Groups.Add(((string)dr["$name"]));
				}

			}
			return Groups.ToArray();
		}

		public async Task<String[]> getAllLedgersNamesByParent(String Parent)
        {
            List<String> Groups = new List<string>();
            ApplicationProperties.getAllProperties(StringUtilsCustom.TALLY_COMPANY);
				
            DataTable response = new DataTable();
            StringBuilder Query = new StringBuilder();

			string query = $"SELECT $Name FROM {Tables.Ledger} WHERE $Parent = '{Parent}' OR $Parent = 'GL 13; Duties & Taxes'";
			Query.Append(query);
			response = await tallyCommunicator.getdatatable(Query.ToString());

            if (response.Rows.Count > 0)
            {

                foreach (DataRow dr in response.Rows)
                {
                    Groups.Add(((string)dr["$name"]));
                }

            }
            return Groups.ToArray();
        }

        internal async Task<string[]> getAllGroupsCurrentAssets()
		{
			ApplicationProperties.getAllProperties(StringUtilsCustom.TALLY_COMPANY);

			List<String> Groups = new List<string>();
			LogManager.WriteLog("listing Groups started...");
			string query = $"SELECT $Name FROM {Tables.Groups} WHERE $Parent = 'Current Assets' OR $Parent = 'GL 07; Current Assets'";

			DataTable response = await tallyCommunicator.getdatatable(query);
			if (response.Rows.Count > 0)
			{
				foreach (DataRow dr in response.Rows)
				{
					Groups.Add(((string)dr["$name"]));
				}
			}
			LogManager.WriteLog("listing Groups ended...");
			return Groups.ToArray();
		
        }

        public async Task<String[]> getAllGroupsByParent(String parent)
        {
            List<String> Groups = new List<string>();
            LogManager.WriteLog("listing Groups started...");
			string query = $"SELECT $Name FROM {Tables.Groups} WHERE $Parent = {parent}";

			DataTable response = await tallyCommunicator.getdatatable(query);
			if (response.Rows.Count > 0)
            {
                foreach (DataRow dr in response.Rows)
                {
                    Groups.Add(((string)dr["$name"]));
                }
            }
            LogManager.WriteLog("listing Groups ended...");
            return Groups.ToArray();
        }

       
        internal async Task<string[]> getAllCashReceiptVoucherTypes()
        {

            List<String> voucherTypes = new List<string>();
            LogManager.WriteLog("listing Cash Receipt Voucher Types started...");
            // Tables.CAVoucherType  replaced with Tables.VoucherType
            DataTable response = await tallyCommunicator.getdatatable("SELECT $Name FROM " + Tables.VoucherType +" where $parent = Receipt");
            if (response.Rows.Count > 0)
            {
                foreach (DataRow dr in response.Rows)
                {
                    voucherTypes.Add(((string)dr["$name"]));
                }
            }
            LogManager.WriteLog("listing Cash Receipt Voucher Types ended...");
            return voucherTypes.ToArray();

        }
        internal async Task<string[]> getAllSalesReturnVoucherTypes()
        {

            List<String> voucherTypes = new List<string>();
            LogManager.WriteLog("listing Cash Sales Return Voucher Types started...");
            // Tables.CAVoucherType  replaced with Tables.VoucherType
            DataTable response = await tallyCommunicator.getdatatable("SELECT $Name FROM " + Tables.VoucherType +" where $parent = Credit Note");
            if (response.Rows.Count > 0)
            {
                foreach (DataRow dr in response.Rows)
                {
                    voucherTypes.Add(((string)dr["$name"]));
                }
            }
            LogManager.WriteLog("listing Cash Receipt Voucher Types ended...");
            return voucherTypes.ToArray();

        }
        internal async Task<string[]> getAllSalesReturnLedgerNames()
        {

            List<String> ledgerNames = new List<string>();
            LogManager.WriteLog("listing Cash Sales Return Ledger Names started...");
            // Tables.CAVoucherType  replaced with Tables.VoucherType
            DataTable response = await tallyCommunicator.getdatatable("SELECT $Name FROM " + Tables.Ledger +" where $parent = Sales Accounts");
            if (response.Rows.Count > 0)
            {
                foreach (DataRow dr in response.Rows)
                {
                    ledgerNames.Add(((string)dr["$name"]));
                }
            }
            LogManager.WriteLog("listing Cash Receipt Voucher Types ended...");
            return ledgerNames.ToArray();

        }
        public async Task<string[]> getAllLedgersByParent(string parent="")
        {
            //if (string.IsNullOrEmpty(parent))
            //    parent="Bank Accounts";
            List<String> BankNames = new List<string>();
            LogManager.WriteLog($"listing  Ledger under {parent} Group from tally started...");
            DataTable response = await tallyCommunicator.getdatatable("SELECT $Name FROM " + Tables.Ledger + " where $parent = "+parent);

            
            if (response.Rows.Count > 0)
            {
                foreach (DataRow dr in response.Rows)
                {
                    BankNames.Add(((string)dr["$name"]));
                }
            }
            LogManager.WriteLog("listing Cash Receipt Voucher Types ended...");
            return BankNames.ToArray();
        }

        public async Task<string[]> getAllIndirectIncomes()
        {
            List<String> datas = new List<string>();
            LogManager.WriteLog("listing IndirectIncomes Ledger from tally started...");
            DataTable response = await tallyCommunicator.getdatatable("SELECT $Name FROM " + Tables.Ledger + " where $parent = Indirect Incomes ");

			if (response.Rows.Count > 0)
            {
                foreach (DataRow dr in response.Rows)
                {
                    datas.Add(((string)dr["$name"]));
                }
            }
            LogManager.WriteLog("listing IndirectIncomes ended...");
            return datas.ToArray();
        }

        public async Task<string[]> getAllIndirectExpences()
        
                                    {
            List<String> datas = new List<string>();
            LogManager.WriteLog("listing Indirect Expences Ledger from tally started...");
            var query = $"SELECT $Name FROM {Tables.Ledger} WHERE $Parent = 'Indirect Expenses' OR $Parent = 'GL 34; Business Promition Expenses'";

			DataTable response = await tallyCommunicator.getdatatable(query.ToString());

			if (response.Rows.Count > 0)
            {
                foreach (DataRow dr in response.Rows)
                {
                    datas.Add(((string)dr["$name"]));
                }
            }
            LogManager.WriteLog("listing Indirect Expences ended...");
            return datas.ToArray();
        }

        internal async Task<string[]> getAllGodowns()
        {
            List<String> datas = new List<string>();
            LogManager.WriteLog("listing Godown Names from tally started...");
            DataTable response = await tallyCommunicator.getdatatable("SELECT $Name FROM " + Tables.Godown );
            if (response.Rows.Count > 0)
            {
                foreach (DataRow dr in response.Rows)
                {
                    datas.Add(((string)dr["$name"]));
                }
            }
            LogManager.WriteLog("listing Godown Names from tally ended...");
            return datas.ToArray();
        }

        public async Task< List<AccountProfileDTO>> getAllLedgers()
        {
            DataTable response = new DataTable();
            StringBuilder Query = new StringBuilder();
            Query.Append("select $Name,$Guid from " + Tables.Ledger);
            List<AccountProfileDTO> allAccountProfilespTally = new List<AccountProfileDTO>();
            response = await tallyCommunicator.getdatatable(Query.ToString());

            if (response.Rows.Count > 0)
            {


                foreach (DataRow dr in response.Rows)
                {
                    AccountProfileDTO accountProfileDTO = new AccountProfileDTO();
                    accountProfileDTO.customerId = ((string)dr["$guid"]);
                    accountProfileDTO.name = (dr["$name"] != DBNull.Value) ? (string)dr["$name"] : "";


                    allAccountProfilespTally.Add(accountProfileDTO);

                }
            }
            return allAccountProfilespTally;
        }

        public async Task< List<ProductProfileDTO>> getAllStockItems()
        {
            DataTable response = new DataTable();
            StringBuilder Query = new StringBuilder();
            Query.Append("select $Name,$Guid from " + Tables.StockItem);
            List<ProductProfileDTO> allStockItems = new List<ProductProfileDTO>();
            response = await tallyCommunicator.getdatatable(Query.ToString());

            if (response.Rows.Count > 0)
            {


                foreach (DataRow dr in response.Rows)
                {
                    ProductProfileDTO stockItem = new ProductProfileDTO();
                    stockItem.productId = ((string)dr["$guid"]);
                    stockItem.name = (dr["$name"] != DBNull.Value) ? (string)dr["$name"] : "";


                    allStockItems.Add(stockItem);

                }
            }
            return allStockItems;
        }

        //public List<GstLedgerDTO> getCompanyLedgers(String parentValue)
        //{

        //}

        public async Task<string> GetGstNumberOfLedgerXMLAsync(string ledgerName)
        {
            String gstNumber = "";
            try
            {
             
                TallyCommunicator tallyCommunicator = new TallyCommunicator();
                //String gstNumber = getGstNumber(ledgerName);
                if (String.IsNullOrEmpty(gstNumber))
                {
                    LedgerGenerateXml ledgerGenerateXml = new LedgerGenerateXml();
                    ENVELOPE envelope = ledgerGenerateXml.getCompanyLedgersGstNumbergetCompanyLedgersGstNumber(ledgerName);

                    var stringwriter1 = new System.IO.StringWriter();
                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(envelope.GetType());
                    x.Serialize(stringwriter1, envelope);
                    var responseString = await tallyCommunicator.ExecXmlAndGetXmlAsync(stringwriter1.ToString());

                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.CheckCharacters = false;
                    ListOfLedgers obj = new ListOfLedgers();
                    using (XmlReader reader = XmlReader.Create(new StringReader(responseString), settings))
                    {
                        // Deserialize the XML data
                        XmlSerializer serializer1 = new XmlSerializer(typeof(ListOfLedgers));
                        obj = (ListOfLedgers)serializer1.Deserialize(reader);

                    }
                    gstNumber=obj.LEDGER!=null ? obj.LEDGER?.PARTYGSTIN : "";
                }
            }catch(Exception ex)
            {
                LogManager.HandleException(ex,"Excepetion occured while getting gst number from tally using xml .");

            }
            return gstNumber;
        }

    }
    
}
