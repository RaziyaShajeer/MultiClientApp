using Newtonsoft.Json;
using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Security
{
    public class AuthenticationService
    {
        HttpClient httpClient ;
        public AuthenticationService()
        {
            //var baseUrl = ConfigurationManager.AppSettings["BaseURL"];
            var baseUrl = ConfigurationManager.AppSettings["FullURL"];
            httpClient = RestClientUtil.getClient();
           // httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.DefaultRequestHeaders.Clear();
        }
        public HttpResponseMessage authenticateAsync(LoginDto loginDto)
        { 
     

            CheckSecureConnection();


            //HttpClient httpClient = RestClientUtil.getClient();
            //HttpClient httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri(baseUrl);
            //httpClient.DefaultRequestHeaders.Clear();
            var myContent = JsonConvert.SerializeObject(loginDto);
            HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");
            try
            {
                var responseTask = httpClient.PostAsync(ApiConstants.AUTHENTICATION, inputContent);
                responseTask.Wait();
           
            HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteLog("Request : \n" + Res.RequestMessage.ToString());
                LogManager.WriteLog("Response : \n StatusCode: " + Res.StatusCode.ToString());
                if (Res.IsSuccessStatusCode)
            {
                LogManager.WriteLog("Login Success..");
                    var rawResponse = Res.Content.ReadAsStringAsync().Result;
                    LogManager.WriteLog("Raw Response Content: \n" + rawResponse); 
                 
                    var response = Res.Content.ReadAsStringAsync().Result;
                Token token = JsonConvert.DeserializeObject<Token>(response);
                RestClientUtil.setAuthKey(token.id_token);
                    RestClientUtil.setLoggedUser(loginDto);
                
            }
            else
            {
                LogManager.WriteLog("Login Failed..\n  statusCode: "+ Res.StatusCode+"\n content: "+ Res.Content.ToString());
                //throw new AuthenticationException();
            }
                return Res;
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex, "Exception Occured while Calling server..");
            }
           
            return null;
           

        }

        private void CheckSecureConnection()
        {
            try
            {
                LogManager.WriteLog("CheckSecureConnection for Switching to HTTPs secure url");


                var baseUrl = ConfigurationManager.AppSettings["SecureURL"];
               
                httpClient.BaseAddress = new Uri(baseUrl);
                var responseTask = httpClient.GetAsync("/");
                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteLog("Request : \n" + Res.RequestMessage.ToString());
                LogManager.WriteLog("Response : \n StatusCode: " + Res.StatusCode.ToString());
                if (Res.IsSuccessStatusCode)
                {
                    LogManager.WriteLog("CheckSecureConnection Success..\n Switching to HTTPs secure url");
                    RestClientUtil.fullUrl=baseUrl;
                }
                else
                {
                    LogManager.WriteLog("CheckSecureConnection Failed..\n  statusCode: "+ Res.StatusCode+"\n content: "+ Res.Content.ToString()+"\n Switching to HTTP Non secure url");
                    //throw new AuthenticationException();
                }
             
            }
            catch (Exception ex)
            {
                httpClient=RestClientUtil.getClient();
                LogManager.WriteLog("CheckSecureConnection Failed..\n  Switching to HTTP Non secure url");

                LogManager.HandleException(ex, "Exception Occured while Calling server..");
            }
        }

        internal void setDeviceKey()
        {
            String hardDiskNo = GetHardDiskSerialNumber();
           
            var myContent = JsonConvert.SerializeObject(hardDiskNo);
            HttpContent inputContent = new StringContent(myContent, Encoding.UTF8, "application/json");
            try
            {
                httpClient= RestClientUtil.getClient();
                LogManager.WriteLog(" DeviceKey saving api called ..");
                var responseTask = httpClient.PostAsync(ApiConstants.SAVE_HARD_DISK_NO+ hardDiskNo, inputContent);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                if (Res.IsSuccessStatusCode)
                    LogManager.WriteLog("Saving DeviceKey Success..");
                else
                    LogManager.WriteLog("Saving DeviceKey Failed..\n  statusCode: " + Res.StatusCode + "\n content: " + Res.Content.ToString());
                 
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex, "Exception Occured while Saving DeviceKey to server..");
            }

        }

        internal bool validateApplication()
        {
            string hardDiskNo = GetHardDiskSerialNumber();
            LogManager.WriteLog("Hard Disk Number "+ hardDiskNo);
      
            httpClient=RestClientUtil.getClient();
            //var myContent = JsonConvert.SerializeObject(hardDiskNo);
            HttpContent inputContent = new StringContent(hardDiskNo, Encoding.UTF8, "application/json");
            try
            {
                LogManager.WriteLog(" Validating Application Started...");
              
              
                var responseTask = httpClient.PostAsync(ApiConstants.PREFIX+ApiConstants.VALIDATEAPP, inputContent);

                responseTask.Wait();

                HttpResponseMessage Res = responseTask.Result;
                LogManager.WriteResponseLog(Res);
                if (Res.IsSuccessStatusCode)
                {
                    var response = Res.Content.ReadAsStringAsync().Result;
                    if (response.Equals("true",StringComparison.OrdinalIgnoreCase))
                    {
                        LogManager.WriteLog(" Validating Application Successed...");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {

                    LogManager.WriteLog(" Validating Application Failed..\n  statusCode: " + Res.StatusCode + "\n content: " + Res.Content.ToString());
                    return false;
                    // throw new AuthenticationException("Validating Application Failed");
                }
            }
            catch (Exception ex)
            {
                LogManager.HandleException(ex, "Exception Occured while Validating Application..");
                return false;
            }

        }

    //    private string getHardDiskSerialNumber()
    //    {
    //        try
    //        {
    //            LogManager.WriteLog("Getting DeviceKey..");
    //            //        ManagementObjectSearcher searcher = new
    //            //ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
    //            ManagementObjectSearcher searcher = new
    //            ManagementObjectSearcher("SELECT * FROM Win32_Processor ");

    //            string deviceKey = "";
    //            foreach (ManagementObject wmi_HD in searcher.Get())
    //            {

    //                deviceKey= wmi_HD["SerialNumber"].ToString();
    //                deviceKey = wmi_HD["ProcessorId"].ToString();
				//	//deviceKey = wmi_HD["VolumeSerialNumber"].ToString();
				//	//deviceKey=GetHardDiskSerialNumber2();


				//}
    //            return deviceKey;
    //            LogManager.WriteLog("Getting DeviceKey Completed..");
    //        }catch(Exception ex)
    //        {
    //            LogManager.HandleException(ex);
    //            throw ex;
    //        }
    //    }


		private string GetHardDiskSerialNumber()
		{
			string result = "";
			try
			{
				string vbs = @"Set objFSO = CreateObject(""Scripting.FileSystemObject"")" + Environment.NewLine +
							 @"Set colDrives = objFSO.Drives" + Environment.NewLine +
							 @"Set objDrive = colDrives.Item(""C"")" + Environment.NewLine +
							 @"Wscript.Echo objDrive.SerialNumber";

				string tempFilePath = Path.ChangeExtension(Path.GetTempFileName(), ".vbs");
				using (StreamWriter writer = new StreamWriter(tempFilePath))
				{
					writer.Write(vbs);
				}

				ProcessStartInfo psi = new ProcessStartInfo();
				psi.FileName = "cscript";
				psi.Arguments = $"//NoLogo \"{tempFilePath}\"";
				psi.RedirectStandardOutput = true;
				psi.UseShellExecute = false;
				psi.CreateNoWindow = true;

				Process process = new Process();
				process.StartInfo = psi;
				process.Start();

				string output = process.StandardOutput.ReadToEnd();
				process.WaitForExit();

				result = output.Trim();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			LogManager.WriteLog("Device Key : "+ result);
			return result;
          
		}
	}
}
