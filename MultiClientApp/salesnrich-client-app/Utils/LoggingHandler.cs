using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Utils
{
    public class LoggingHandler: DelegatingHandler
    {
        public LoggingHandler()
        {

        }
        public LoggingHandler(HttpMessageHandler innerHandler)
       : base(innerHandler)
        {
        }

      
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LogManager.WriteLog("Request : \n");
            Console.WriteLine("Request:");
            LogManager.WriteLog(request.ToString());
            Console.WriteLine(request.ToString());
            if (request.Content != null)
            {
                LogManager.WriteLog(await request.Content.ReadAsStringAsync());
            }
            LogManager.WriteLog("\n");
            var cts = new CancellationTokenSource(System.TimeSpan.FromMinutes(1));
            
           HttpResponseMessage response = await base.SendAsync(request, cts.Token);

            LogManager.WriteLog("Response: \n");
             LogManager.WriteLog(response.ToString());
            if (response.Content != null)
            {
                LogManager.WriteLog(await response.Content.ReadAsStringAsync());
            }
            LogManager.WriteLog("\n");

            return response;
        }
    }
}
