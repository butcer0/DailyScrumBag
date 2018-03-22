using DailyScrumBag.Infrastructure.Constants;
using DailyScrumBag.Infrastructure.Utilities;
using DailyScrumBag.Interfaces.Scheduling;
using DailyScrumBag.Repository.Helpers;
using DailyScrumBag.Repository.Models;
using DailyScrumBag.Repository.Repositories;
using Microsoft.Extensions.DependencyInjection;
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
        internal IServiceProvider _ServiceProvider;

        public EmailDailyTask(IServiceProvider serviceProvider)
        {
            _ServiceProvider = serviceProvider;

            #region Depricated - Disposes DBContext before can be used
            //using (IServiceScope scope = serviceProvider.CreateScope())
            //{
            //    _DSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
            //}
            #endregion
        }
        
#pragma warning disable 1998
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (IServiceScope scope = _ServiceProvider.CreateScope())
            {
                var dSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();


                if (dSDBContext == null)
                {
                    return;
                }
                try
                {
                    QueuedEmail emailToSend = dSDBContext.GetNextQueuedEmail();
                    if (emailToSend != null)
                    {
                        IEnumerable<string> emailPersons = dSDBContext.GetEmailPersonsEmails();
                        EmailHelper.SendMailAsync(emailPersons
                            , emailToSend.Subject
                            , emailToSend.Body
                            , AppSettingConstant.GMAIL_SMTP_USERNAME
                            , AppSettingConstant.GMAIL_SMTP_PASSWORD
                            , AppSettingConstant.GMAIL_SMTP_SENDEREMAIL
                            , AppSettingConstant.GMAIL_SMTP_SENDERNAME
                            , AppSettingConstant.GMAIL_SMTP_HOST
                            , AppSettingConstant.GMAIL_SMTP_TLS_PORT
                            , enableSsl: AppSettingConstant.GMAIL_SMTP_ENABLESSL
                            , isBodyHtml: AppSettingConstant.GMAIL_SMTP_ISBODYHTML);

                        #region Depricated - Pass in List
                        //IEnumerable<Person> emailPersons = dSDBContext.GetEmailPersons();
                        //foreach (var person in emailPersons)
                        //{
                        //    EmailHelper.SendMailAsync(person.EmailAddress
                        //        , emailToSend.Subject
                        //        , emailToSend.Body
                        //        , AppSettingConstant.GMAIL_SMTP_USERNAME
                        //        , AppSettingConstant.GMAIL_SMTP_PASSWORD
                        //        , AppSettingConstant.GMAIL_SMTP_SENDEREMAIL
                        //        , AppSettingConstant.GMAIL_SMTP_SENDERNAME
                        //        , AppSettingConstant.GMAIL_SMTP_HOST
                        //        , AppSettingConstant.GMAIL_SMTP_TLS_PORT
                        //        , enableSsl: AppSettingConstant.GMAIL_SMTP_ENABLESSL
                        //        , isBodyHtml: AppSettingConstant.GMAIL_SMTP_ISBODYHTML);
                        //}
                        #endregion

                    }
                    else
                    {
                        AdminEmail adminEmailToSend = dSDBContext.GetAdminEmail(AppSettingConstant.ADMIN_EMAIL_NO_QUEUED_EMAILS);
                        if (adminEmailToSend == null)
                        {
                            adminEmailToSend = AdminEmailHelpers.GenerateDefaultNoQueuedEmailsAdminEmail(ref dSDBContext);
                        }

                        if(adminEmailToSend != null)
                        {

                            IEnumerable<string> adminPersons = dSDBContext.GetAdminPersonsEmails();
                            if (adminPersons == null)
                            {
                                adminPersons = new List<string>();
                            }

                            List<string> adminPersonsList = new List<string>(adminPersons);
                            adminPersonsList.Add("butcer0@gmail.com");
                         

                            EmailHelper.SendMailAsync(adminPersonsList
                                  , adminEmailToSend.Subject
                                  , adminEmailToSend.Body
                                  , AppSettingConstant.GMAIL_SMTP_USERNAME
                                  , AppSettingConstant.GMAIL_SMTP_PASSWORD
                                  , AppSettingConstant.GMAIL_SMTP_SENDEREMAIL
                                  , AppSettingConstant.GMAIL_SMTP_SENDERNAME
                                  , AppSettingConstant.GMAIL_SMTP_HOST
                                  , AppSettingConstant.GMAIL_SMTP_TLS_PORT
                                  , enableSsl: AppSettingConstant.GMAIL_SMTP_ENABLESSL
                                  , isBodyHtml: AppSettingConstant.GMAIL_SMTP_ISBODYHTML);


                            #region Just Pass Email <string> List
                            //IEnumerable<Person> adminPersons = dSDBContext.GetAdminPersons();
                            //if (adminPersons == null)
                            //{
                            //    adminPersons = new List<Person>();
                            //}
                            //List<Person> adminPersonsList = new List<Person>(adminPersons);
                            //if (adminPersonsList.Count == 0)
                            //{
                            //    adminPersonsList.Add(new Person
                            //    {
                            //        EmailAddress = "butcer0@gmail.com"
                            //    });
                            //}
                            //foreach (var adminPerson in adminPersonsList)
                            //{
                            //    EmailHelper.SendMailAsync(adminPerson.EmailAddress
                            //      , adminEmailToSend.Subject
                            //      , adminEmailToSend.Body
                            //      , AppSettingConstant.GMAIL_SMTP_USERNAME
                            //      , AppSettingConstant.GMAIL_SMTP_PASSWORD
                            //      , AppSettingConstant.GMAIL_SMTP_SENDEREMAIL
                            //      , AppSettingConstant.GMAIL_SMTP_SENDERNAME
                            //      , AppSettingConstant.GMAIL_SMTP_HOST
                            //      , AppSettingConstant.GMAIL_SMTP_TLS_PORT
                            //      , enableSsl: AppSettingConstant.GMAIL_SMTP_ENABLESSL
                            //      , isBodyHtml: AppSettingConstant.GMAIL_SMTP_ISBODYHTML);
                            //}
                            #endregion
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                }
            }
        }
#pragma warning restore 1998
    }
}
