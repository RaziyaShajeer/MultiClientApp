using SNR_ClientApp.DTO;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Services
{
    public class MailService
    {
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            try
            {
                var FromMail = ConfigurationManager.AppSettings["FromMail"];
                var ToMail = ConfigurationManager.AppSettings["ToMail"];
                var fromAddress = new MailAddress(FromMail, "SalesNrich");
                var toAddress = new MailAddress(ToMail, "Admin");
                string fromPassword = ConfigurationManager.AppSettings["Password"];
                 string subject = mailRequest.Subject;
                 string body = mailRequest.Body;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    var attachment = new Attachment(mailRequest.Attachment);
                    message.Attachments.Add(attachment);
                    smtp.Send(message);
                }
            }
            catch(Exception ex)
            {
                LogManager.WriteLog("Exception occured while sending mail");
                LogManager.HandleException(ex);
              //  throw ex
           
            }
            
        }
    }
}
