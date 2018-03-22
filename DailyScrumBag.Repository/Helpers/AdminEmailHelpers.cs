using DailyScrumBag.Infrastructure.Constants;
using DailyScrumBag.Repository.Models;
using DailyScrumBag.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DailyScrumBag.Repository.Helpers
{
    public static class AdminEmailHelpers
    {
        /// <summary>
        /// Recheck if ADMIN_EMAIL_NO_QUEUED_EMAIL exists and generates if necessary
        /// </summary>
        /// <param name="db"></param>
        /// <returns>ADMIN_EMAIL_NO_QUEUED_EMAIL AdminEmail</returns>
        public static AdminEmail GenerateDefaultNoQueuedEmailsAdminEmail(ref DSDBContext db)
        {
            AdminEmail adminEmailToSend = null;
            try
            {
                if (db.AdminEmails.Where(c => c.Key.Equals(AppSettingConstant.ADMIN_EMAIL_NO_QUEUED_EMAILS, StringComparison.InvariantCultureIgnoreCase)).Any())
                {
                    adminEmailToSend = db.AdminEmails.Where(c => c.Key.Equals(AppSettingConstant.ADMIN_EMAIL_NO_QUEUED_EMAILS, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                }

                if (adminEmailToSend == null)
                {
                    adminEmailToSend = new AdminEmail
                    {
                        GuidId = Guid.NewGuid(),
                        Author = AppSettingConstant.ADMIN_EMAIL_GENERATED_AUTHOR,
                        CreateDate = DateTime.Now,
                        Subject = "No Queued Emails Exist",
                        Key = AppSettingConstant.ADMIN_EMAIL_NO_QUEUED_EMAILS,
                        Body = "<p>No Daily ScrumBag email sent today. Please check email queue and update as necessary.</p><p>Thanks!</p>"
                    };

                    db.AdminEmails.Add(adminEmailToSend);
                    db.SaveChangesAsync();
                }
            }
            catch { }

            return adminEmailToSend;
        }
    }
}
