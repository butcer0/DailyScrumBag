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
        public static async void SendMailAsync(IEnumerable<string> recipients, string subject, string body, string userId, string password, string senderEmail, string senderName, string hostClient, int port = 25, bool enableSsl = false, bool isBodyHtml = false, string attachmentFilename = "")
        #region Depricated - Pass list of recipients
        //public static async void SendMailAsync(string recipient, string subject, string body, string userId, string password, string senderEmail, string senderName, string hostClient, int port = 25, bool enableSsl = false, bool isBodyHtml = false, string attachmentFilename = "")
        #endregion
        {

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(senderEmail);
                foreach (var recipient in recipients)
                {
                    mail.To.Add(recipient);
                }
                #region Depricated - AddRange
                //mail.To.Add(recipients);
                #endregion
                string htmlBody = "<html><body><h1>Picture</h1><br><img src=\"cid:filename\"></body></html>";

                mail.Subject = subject;
                mail.Body = String.Format(
            "<h3>Client:Has Sent You A Screenshot</h3>" +
            @"<img src=""cid:{0}"" />", "https://github.com/butcer0/DailyScrumBag/blob/3349a4a332f29812f2d4627c197b56b44f56ac36/DailyScrumBagHeaderLogo.png");
                //mail.Body = @"<img src=""https://github.com/butcer0/DailyScrumBag/blob/3349a4a332f29812f2d4627c197b56b44f56ac36/DailyScrumBagHeaderLogo.png"" />" + $"< h1>{body}</h1>";
                mail.IsBodyHtml = true;
                if (!string.IsNullOrEmpty(attachmentFilename))
                {
                    mail.Attachments.Add(new Attachment(attachmentFilename));
                }

                #region Depricated - Shows how to attach file from IO
                //mail.Attachments.Add(new Attachment("C:\\file.zip"));
                #endregion

                using (SmtpClient smtp = new SmtpClient(hostClient, port))
                {
                    smtp.Credentials = new NetworkCredential(senderEmail, password);
                    smtp.EnableSsl = enableSsl;
                    smtp.Send(mail);
                }
            }

            #region Depricated - Version 1, update to Dispose
            //SmtpClient smtpClient = new SmtpClient();
            //#region Depricated - Include Domain
            ////NetworkCredential basicCredential = new NetworkCredential(userId, password);
            //#endregion
            //NetworkCredential basicCredential = new NetworkCredential(userId, password, host);
            //MailMessage message = new MailMessage();
            //MailAddress fromAddress = new MailAddress(senderEmail, senderName);

            //smtpClient.Host = host;
            //smtpClient.UseDefaultCredentials = false;
            //smtpClient.Credentials = basicCredential;
            //smtpClient.Timeout = (60 * 5 * 1000);
            //smtpClient.Port = port;
            //smtpClient.EnableSsl = enableSsl;

            //message.From = fromAddress;
            //message.Subject = subject;
            //message.IsBodyHtml = isBodyHtml;
            //message.Body = body;
            //message.To.Add(recipient);

            //if (!string.IsNullOrEmpty(attachmentFilename))
            //    message.Attachments.Add(new Attachment(attachmentFilename));

            //smtpClient.Send(message);
            #endregion
        }
    }
 
}

