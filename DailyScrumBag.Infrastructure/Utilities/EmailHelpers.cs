#pragma warning disable 1998
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
        /// Sends the Email - Body with No Template
        /// </summary>
        /// <param name="recipients">Recipient(s)</param>
        /// <param name="subject">Email Subject</param>
        /// <param name="body">Email Body</param>
        /// <param name="isBodyHtml">Is Body Html</param>
        /// <param name="attachmentFilename">Attachment File path</param>
        public static async void SendMailAsync(IEnumerable<string> recipients, string subject, string body, string userId, string password, string senderEmail, string senderName, string hostClient, int port = 25, bool enableSsl = false, bool isBodyHtml = false, string attachmentFilename = "")
        {

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(senderEmail);
                foreach (var recipient in recipients)
                {
                    mail.To.Add(recipient);
                }
                #region Depricated - Not Used
                //string htmlBody = "<html><body><h1>Picture</h1><br><img src=\"cid:filename\"></body></html>";
                #endregion
                mail.Subject = subject;
                mail.Body = body;
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
        }

        /// <summary>
        /// Sends the Email
        /// </summary>
        /// <param name="recipients">Recipient(s)</param>
        /// <param name="subject">Email Subject</param>
        /// <param name="body">Email Body</param>
        /// <param name="isBodyHtml">Is Body Html</param>
        /// <param name="attachmentFilename">Attachment File path</param>
        public static async void SendAdminMailAsync(string bodyTemplate, IEnumerable<string> recipients, string subject, string body, string userId, string password, string senderEmail, string senderName, string hostClient, int port = 25, bool enableSsl = false, bool isBodyHtml = false, string attachmentFilename = "")
        {
            
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(senderEmail);
                foreach (var recipient in recipients)
                {
                    mail.To.Clear();
                    mail.To.Add(recipient);


                    mail.Subject = subject;
                    mail.Body = string.Format(bodyTemplate
                                            , recipient //email recipient
                                            , body);
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
            }
        }

        /// <summary>
        /// Sends the Email
        /// </summary>
        /// <param name="recipients">Recipient(s)</param>
        /// <param name="subject">Email Subject</param>
        /// <param name="body">Email Body</param>
        /// <param name="isBodyHtml">Is Body Html</param>
        /// <param name="attachmentFilename">Attachment File path</param>
        public static async void SendDailyMailAsync(string bodyTemplate, IEnumerable<string> recipients, string subject, string body, string userId, string password, string senderEmail, string senderName, string hostClient, int port = 25, bool enableSsl = false, bool isBodyHtml = false, string attachmentFilename = "")
        {

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(senderEmail);
                foreach (var recipient in recipients)
                {
                    mail.To.Clear();
                    mail.To.Add(recipient);
                    mail.Subject = subject;
                    mail.Body = string.Format(bodyTemplate
                                            , recipient //email recipient
                                            , body);
                    mail.IsBodyHtml = true;
                    if (!string.IsNullOrEmpty(attachmentFilename))
                    {
                        mail.Attachments.Add(new Attachment(attachmentFilename));
                    }

                    using (SmtpClient smtp = new SmtpClient(hostClient, port))
                    {
                        smtp.Credentials = new NetworkCredential(senderEmail, password);
                        smtp.EnableSsl = enableSsl;
                        smtp.Send(mail);
                    }
                }
            }
        }


    }

}

