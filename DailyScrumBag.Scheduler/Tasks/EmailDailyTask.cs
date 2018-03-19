using DailyScrumBag.Infrastructure.Constants;
using DailyScrumBag.Infrastructure.Utilities;
using DailyScrumBag.Interfaces.Scheduling;
using DailyScrumBag.Repository.Helpers;
using DailyScrumBag.Repository.Models;
using DailyScrumBag.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DailyScrumBag.Scheduler.Tasks
{
    public class EmailDailyTask : IScheduledTask
    {
        public string Schedule => "* * * * *";

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {

            DSDBContext db = DSDBContextHelpers.GetDSDBContext();
            if(db == null)
            {
                return;
            }
            try
            {
                QueuedEmail emailToSend = db.GetNextQueuedEmail();
                if (emailToSend != null)
                {
                    IEnumerable<Person> emailPersons = db.GetEmailPersons();
                    foreach (var person in emailPersons)
                    {
                        EmailHelper.SendMailAsync(person.EmailAddress
                            , emailToSend.Subject
                            , emailToSend.Body
                            , AppSettingConstant.GMAIL_SMTP_USERNAME
                            , AppSettingConstant.GMAIL_SMTP_PASSWORD
                            , AppSettingConstant.GMAIL_SMTP_SENDEREMAIL
                            , AppSettingConstant.GMAIL_SMTP_SENDERNAME
                            , AppSettingConstant.GMAIL_SMTP_HOST
                            , AppSettingConstant.GMAIL_SMTP_TLS_PORT
                            , isBodyHtml: true);
                    }

                }
                else
                {
                    AdminEmail adminEmailToSend = db.GetAdminEmail(AppSettingConstant.ADMIN_EMAIL_NO_QUEUED_EMAILS);
                    if (adminEmailToSend == null)
                    {
                        adminEmailToSend = AdminEmailHelpers.GenerateDefaultNoQueuedEmailsAdminEmail(ref db);

                        IEnumerable<Person> adminPersons = db.GetAdminPersons();
                        foreach (var adminPerson in adminPersons)
                        {
                            EmailHelper.SendMailAsync(adminPerson.EmailAddress
                              , emailToSend.Subject
                              , emailToSend.Body
                              , AppSettingConstant.GMAIL_SMTP_USERNAME
                              , AppSettingConstant.GMAIL_SMTP_PASSWORD
                              , AppSettingConstant.GMAIL_SMTP_SENDEREMAIL
                              , AppSettingConstant.GMAIL_SMTP_SENDERNAME
                              , AppSettingConstant.GMAIL_SMTP_HOST
                              , AppSettingConstant.GMAIL_SMTP_TLS_PORT
                              , isBodyHtml: true);
                        }


                    }

                }
            }
            catch { }

        }    
    }
}
