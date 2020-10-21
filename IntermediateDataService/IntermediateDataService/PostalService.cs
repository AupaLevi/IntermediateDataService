using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace IntermediateDataService
{
    class PostalService
    {
        public int SendMail(string toAddr, string subject, string content)
        {
            SmtpClient aupaSmtp = new SmtpClient("mail.aupa.com.tw");
            aupaSmtp.Credentials = new NetworkCredential("levi.huang", "Withyui87");

            MailMessage msgMail = new MailMessage();
            msgMail.From = new MailAddress("levi.huang@aupa.com.tw");
            msgMail.To.Add(toAddr);

            msgMail.Subject = subject;

            AlternateView alt = AlternateView.CreateAlternateViewFromString(content, null, "text/html");
            msgMail.AlternateViews.Add(alt);

            aupaSmtp.Send(msgMail);

            return 0;
        }
    }
}
