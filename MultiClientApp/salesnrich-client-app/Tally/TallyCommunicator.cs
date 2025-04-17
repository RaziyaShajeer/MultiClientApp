using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNR_ClientApp.Properties;
using Newtonsoft.Json;
using SNR_ClientApp.Utils;
using System.Collections;
using SNR_ClientApp.TallyResponses;
using System.Xml.Serialization;
using System.Xml;
using SNR_ClientApp.Enums;

namespace SNR_ClientApp.Tally
{
    public class TallyCommunicator
    {
        public OdbcCommand cmd;
        public OdbcConnection con;
        private Lazy<Dictionary<string, object>> lazyProps = new Lazy<Dictionary<string, object>>(() => ApplicationProperties.getAllProperties());

        // Property to access the lazy-loaded dictionary
        public Dictionary<string, object> props => lazyProps.Value;

        public TallyCommunicator()
        
        {
            

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
                string odbcDsn = ApplicationProperties.properties["tally.odbcdsn"].ToString();
                //source = "SERVER="+ props.GetValueOrDefault("tally.hostname") +";DSN=TallyODBC64_"+ props.GetValueOrDefault("tally.port")+";PORT=" + props.GetValueOrDefault("tally.port") + ";DRIVER=Tally ODBC Driver64;" ;
                source = "SERVER="+ props.GetValueOrDefault("tally.hostname") +";DSN="+odbcDsn+";PORT=" + props.GetValueOrDefault("tally.port") + ";DRIVER=Tally ODBC Driver64;";

                LogManager.WriteLog("\nConnection String : " + source);
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
            }catch(Exception e)
            {
                LogManager.WriteLog("Exception occured while getting Connection to Tally");
                LogManager.HandleException(e, "Connection String : " + source);
                throw e;
            }
        }

        public bool TryConnectTally()
        {
            try {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(props.GetValueOrDefault("tally.full.url").ToString());
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

        public async Task<DataTable> getdatatable(string query)
        {
            try
            {
                string hostname = props.GetValueOrDefault("tally.hostname").ToString();
                if (!hostname.Equals("localhost", StringComparison.OrdinalIgnoreCase))
                {
                     var res = getdatatableFromXmlAsync(query);
                    res.Wait();
                    return res.Result;
                }
                else
                {
                    LogManager.WriteLog("Get request to Tally - \n Query : " + query);
                    var con = GetConnection();
                    if (con.State== ConnectionState.Closed)
                        con.Open();
                    OdbcDataAdapter ad = new OdbcDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    LogManager.WriteLog("Tally get request Successfully executed");
                    return dt;
                }

            }
            catch (Exception ex)
            {

                LogManager.WriteLog("UnExpected error occured while getting datas from Tally\n" +ex.Message +"\n"+ex.InnerException);
                LogManager.HandleException(ex);
                throw ex;
            }
        }

        public object execscalar(string query)
        {

            OdbcCommand cmd = new OdbcCommand(query, GetConnection());
            object s;
            s = cmd.ExecuteScalar();
            return s;
        }
        public string GetXmlQueryForSqlCmd(string query)
        {
            string companyName = props.GetValueOrDefault("tally.company").ToString();

            ENVELOPE tallyRequest = new ENVELOPE();
            HEADER header = new HEADER();
         
            header.TALLYREQUEST = "Export Data";
           
            tallyRequest.HEADER = header;
            EXPORTDATA exportData = new();
            REQUESTDESC requestDesc = new REQUESTDESC();
            requestDesc.REPORTNAME="ODBC Report";
            SQLREQUEST sqlRequest = new();
            sqlRequest.TYPE="General";
            sqlRequest.METHOD="SQLExecute";
            sqlRequest.Text=query;
            requestDesc.SQLREQUEST=sqlRequest;
            STATICVARIABLES staticvariable=new();
            staticvariable.SVEXPORTFORMAT="$$SysName:XML";
            if(!String.IsNullOrEmpty(companyName))
            staticvariable.SVCURRENTCOMPANY=companyName;
            requestDesc.STATICVARIABLES=staticvariable;
            exportData.REQUESTDESC=requestDesc;
            
            BODY body = new();
            body.EXPORTDATA=exportData;
            tallyRequest.BODY=body;

           

            var stringwriter2 = new System.IO.StringWriter();
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(tallyRequest.GetType());
            serializer.Serialize(stringwriter2, tallyRequest);

            return stringwriter2.ToString();
        }
        public async Task<DataTable> getdatatableFromXmlAsync(string query)
        {
            string xmlQuery = GetXmlQueryForSqlCmd(query);
                TallyRequestResponse data  = await ExecXml(xmlQuery);
            // Create the DataTable and add columns using LINQ
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(data.response?.BODY?.EXPORTDATARESPONSE?.RESULTDESC?.ROWDESC?.COL?.Select(col => new DataColumn(col.NAME)).ToArray());
            foreach (var row in data.response.BODY.EXPORTDATARESPONSE.RESULTDATA.ROW)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow.ItemArray = row.COL.ToArray();
                dataTable.Rows.Add(dataRow);
            }
            //dataRow.ItemArray = data.response.BODY.EXPORTDATARESPONSE.RESULTDATA.ROW.COL.Select(value => (object)value).ToArray();
            //dataTable.Rows.Add(dataRow);

            return dataTable;

        }
        public int execNonQuery(string query)
        {

            OdbcCommand cmd = new OdbcCommand(query, GetConnection());
            return cmd.ExecuteNonQuery();
        }

        public async Task<TallyRequestResponse> ExecXml(String xmlQuery)
        {
            LogManager.WriteLog("Get request to Tally - \n XML : " + xmlQuery);
            TallyRequestResponse Tallyresponse = new TallyRequestResponse();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(props.GetValueOrDefault("tally.full.url").ToString());
            string companyname = props.GetValueOrDefault("tally.company").ToString();


            HttpContent inputContent = new StringContent(xmlQuery, Encoding.UTF8, "text/xml");
            try
            {
                var responseTask = client.PostAsync("", inputContent);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("tally xml request sccessfully completed...");
                    var response = await Res.Content.ReadAsStringAsync();
                 
                   LogManager.WriteLog(response.ToString());
                    //  var response2 = JsonConvert.DeserializeObject<TallyResponses.ENVELOPE>(response);
                    XmlSerializer serializer =
         new XmlSerializer(typeof(TallyResponses.ENVELOPE));

                    // Declare an object variable of the type to be deserialized.
                    TallyResponses.ENVELOPE i;
                    string responseString = response.ToString().Replace("\u0004", "",StringComparison.OrdinalIgnoreCase);
                    //var reader = new StringReader(responseString);
                    LogManager.WriteLog(responseString);    
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.CheckCharacters = false;

                    using (XmlReader reader = XmlReader.Create(new StringReader(responseString), settings))
                    {
                        // Deserialize the XML data
                        XmlSerializer serializer1 = new XmlSerializer(typeof(ENVELOPE));
                        ENVELOPE obj = (ENVELOPE)serializer1.Deserialize(reader);
                        Tallyresponse.response = obj;
                    }

                    // Call the Deserialize method to restore the object's state.
                    //i = (ENVELOPE)serializer.Deserialize(reader);
                    //Tallyresponse.response = i;
                    //RestClientUtil.setAuthKey(token.id_token);

                }
                else
                {
                    LogManager.WriteLog("tally xml request Failed..\n  statusCode: " + Res.StatusCode + "\n content: " + Res.Content.ToString());
                  
                }
            }
            catch(Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }

            return Tallyresponse;
        }

        internal async Task<TallyDownloadResponse> UploadDataToTally(string xmlQuery)
        {

            LogManager.WriteLog("Get request to Tally - \n XML : " + xmlQuery);
            TallyDownloadResponse Tallyresponse = new TallyDownloadResponse();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(props.GetValueOrDefault("tally.full.url").ToString());

            HttpContent inputContent = new StringContent(xmlQuery, Encoding.UTF8, "text/xml");
            try
            {
                var responseTask = client.PostAsync("", inputContent);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                
                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("tally xml request sccessfully completed...");
                    var response = await Res.Content.ReadAsStringAsync();
             
                    LogManager.WriteLog(response.ToString());
                    //  var response2 = JsonConvert.DeserializeObject<TallyResponses.ENVELOPE>(response);
                    XmlSerializer serializer =
         new XmlSerializer(typeof(RESPONSE));

                    // Declare an object variable of the type to be deserialized.
                    RESPONSE i;

                    var reader = new StringReader(response.ToString());

                    // Call the Deserialize method to restore the object's state.
                    i = (RESPONSE)serializer.Deserialize(reader);
             
                    //Query.Append("select $name,  from " + Tables.VoucherType + " where $= " + );
                    //response = tallyCommunicator.getdatatable(Query.ToString());
                    Tallyresponse.response = i;
                    //RestClientUtil.setAuthKey(token.id_token);

                }
                else
                {
                    LogManager.WriteLog("tally xml request Failed..\n  statusCode: " + Res.StatusCode + "\n content: " + Res.Content.ToString());

                }
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }
            

            return Tallyresponse;
        }
        static string GetVoucherNumberById(int lastVchId)
        {
            string voucherNumber = string.Empty;

            // Define the connection string
            string connectionString = "DSN=TallyODBC_9000;";

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Define the query to fetch the voucher number
                    string query = $"SELECT $VoucherNumber FROM Vouchers WHERE $VoucherID = {lastVchId}";

                    using (OdbcCommand command = new OdbcCommand(query, connection))
                    {
                        using (OdbcDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                voucherNumber = reader["VoucherNumber"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return voucherNumber;
        }

        public async Task<string> ExecXmlAndGetXmlAsync(String xmlQuery)
        {
            LogManager.WriteLog("Get request to Tally - \n XML : " + xmlQuery);
            TallyRequestResponse Tallyresponse = new TallyRequestResponse();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(props.GetValueOrDefault("tally.full.url").ToString());

            HttpContent inputContent = new StringContent(xmlQuery, Encoding.UTF8, "text/xml");
            try
            {
                var responseTask = client.PostAsync("", inputContent);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteResponseLog(Res);
                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("tally xml request sccessfully completed...");
                    var response = await Res.Content.ReadAsStringAsync();
                  LogManager.WriteLog(response.ToString());
                    return response;

                }
                else
                {
                    LogManager.WriteLog("tally xml request Failed..\n  statusCode: " + Res.StatusCode + "\n content: " + Res.Content.ToString());

                }
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex);
                throw ex;
            }

            return null;
        }
    }
}
