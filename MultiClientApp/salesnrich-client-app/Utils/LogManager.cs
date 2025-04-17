using SNR_ClientApp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Utils
{
    public class LogManager
    {
        static string basefolderPath = "C:\\SNR\\Logs\\";
        static string folderPath = "C:\\SNR\\Logs\\DailyLogFolder";
        static string filePath = "C:\\SNR\\Logs\\ExceptionFile.txt";
        static string company  ="";

        public static void HandleException(Exception ex)
        {
            HandleException(ex, null);
        }
        public static void HandleException(Exception ex, string context)
        {
            WriteLog(ex.Message);
            WriteLog(context);
            string content = "[" + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss.ff tt") + "] "
                + (string.IsNullOrEmpty(context) ? null : "Context: " + context + Environment.NewLine)
                + "Error: " + ex.Message + Environment.NewLine
                + "Stack: " + ex.StackTrace + Environment.NewLine + Environment.NewLine
                + "Source: " + ex.StackTrace + Environment.NewLine + Environment.NewLine
                + "InnerException: " + ex.InnerException + Environment.NewLine + Environment.NewLine;
            WriteException(content);
        }
        private static void WriteException(string content)
        {
            // company= ApplicationProperties.properties.GetValueOrDefault("tally.company").ToString();
            LoadCompanyName();
            filePath=basefolderPath+company+"\\ExceptionFile.txt";
            if (!string.IsNullOrEmpty(filePath))
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    try
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                    }
                }
                WriteToFile(content, filePath);
            }
        }

        private static void WriteToFile(string content, string filePath)
        {
            try
            {
                File.AppendAllText(filePath, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }


        public static void WriteLog(string content)
        {
            LoadCompanyName();
            folderPath=basefolderPath+company+"\\DailyLogFolder";
            if (!string.IsNullOrEmpty(folderPath))
            {
                DateTime now = DateTime.Now;
                string filePath = folderPath + "\\" + now.ToString(@"yyyy\MM-MMM\dd-ddd") + ".txt";
                content = "[" + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss.ff tt") + "] " + content + Environment.NewLine;
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    try
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                    }
                }
                WriteToFile(content, filePath);
            }
        }

        public static void WriteResponseLog(HttpResponseMessage Res)
        {

            LogManager.WriteLog("Request : \n" + Res.RequestMessage.Method + "Request For " + Res.RequestMessage.RequestUri+"\n");
            if (Res.RequestMessage.Content != null)
            {
                LogManager.WriteLog("\n Request Body :" + Encoding.UTF8.GetString(Res.RequestMessage.Content.ReadAsByteArrayAsync().Result));
            }
            LogManager.WriteLog("Response : \n StatusCode: " + Res.StatusCode.ToString()+"\n Response Body : "+Res.Content.ReadAsStringAsync().Result);
        }

        internal static void WriteRequestContentLog(string myContent, string requestUri)
        {
            LogManager.WriteLog("Requested Url : "+requestUri);

            LogManager.WriteLog("Request body : " + myContent);
        }

        public static void LoadCompanyName() {
            try
            {
                company= ApplicationProperties.properties.GetValueOrDefault("tally.company").ToString();
            }catch(Exception ex) {

                company="Initial";   
            }
        }
    }
}
