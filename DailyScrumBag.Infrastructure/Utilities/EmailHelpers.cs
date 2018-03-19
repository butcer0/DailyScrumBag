using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace DailyScrumBag.Infrastructure.Utilities
{
    public static class EmailHelper
    {
        /// <summary>
        /// Sends the Email
        /// </summary>
        /// <param name="recipient">Recipient(s)</param>
        /// <param name="subject">Email Subject</param>
        /// <param name="body">Email Body</param>
        /// <param name="isBodyHtml">Is Body Html</param>
        /// <param name="attachmentFilename">Attachment File path</param>
        public static async void SendMailAsync(string recipient, string subject, string body, string userId, string password, string senderEmail, string senderName, string host, int port = 25, bool enableSsl = false, bool isBodyHtml = false, string attachmentFilename = "")
        {
            SmtpClient smtpClient = new SmtpClient();
            NetworkCredential basicCredential = new NetworkCredential(userId, password);
            MailMessage message = new MailMessage();
            MailAddress fromAddress = new MailAddress(senderEmail, senderName);

            smtpClient.Host = host;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredential;
            smtpClient.Timeout = (60 * 5 * 1000);
            smtpClient.Port = port;
            smtpClient.EnableSsl = enableSsl;

            message.From = fromAddress;
            message.Subject = subject;
            message.IsBodyHtml = isBodyHtml;
            message.Body = body;
            message.To.Add(recipient);

            if (!string.IsNullOrEmpty(attachmentFilename))
                message.Attachments.Add(new Attachment(attachmentFilename));

            smtpClient.Send(message);
        }
    }
 
}

