using SNR_ClientApp.Config;
using SNR_ClientApp.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Utils
{
    public class RestClientUtil
    {
        private static string AUTH_KEY;
       
        static string baseUrl = ConfigurationManager.AppSettings["BaseURL"];
        public static string fullUrl = ConfigurationManager.AppSettings["FullURL"];
        private static string LoggedUser;
        private static string LoggedUserPassword;
        public static void setAuthKey(string authKey)
        {
            RestClientUtil.AUTH_KEY = authKey;
        }
        public static string getLoggedUser()
        {
            return LoggedUser;
        }
        public static string getLoggedUserPassword()
        {
            return LoggedUserPassword;
        }
        public static void setLoggedUser(LoginDto loginDto)
        {
            RestClientUtil.LoggedUser = loginDto.username;
            RestClientUtil.LoggedUserPassword = loginDto.password;

        }

        public static HttpClient getClient()
        {
            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(baseUrl);
            ConfigurationManager.RefreshSection("appSettings");
            fullUrl = ConfigurationManager.AppSettings["FullURL"];
            client.BaseAddress = new Uri(fullUrl);
           
            client.DefaultRequestHeaders.Clear();
            if(AUTH_KEY!=null)
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AUTH_KEY);
            return client;

        }


    }
}
