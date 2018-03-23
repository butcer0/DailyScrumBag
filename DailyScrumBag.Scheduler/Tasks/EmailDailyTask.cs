using DailyScrumBag.Infrastructure.Constants;
using DailyScrumBag.Infrastructure.Utilities;
using DailyScrumBag.Interfaces.Scheduling;
using DailyScrumBag.Repository.Helpers;
using DailyScrumBag.Repository.Models;
using DailyScrumBag.Repository.Repositories;
using DailyScrumBag.Scheduler.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DailyScrumBag.Scheduler.Tasks
{
    public class EmailDailyTask : IScheduledTask
    {
        public string Schedule => "*/5 * * * *";
        #region Depricated - Use all local time
        //public string Schedule => $"{SchedulerHelpers.ConvertToUTCHour(16)} {SchedulerHelpers.ConvertToUTCMinute(02)} * * *";
        #endregion
        internal IServiceProvider _ServiceProvider;
        internal IHostingEnvironment _Env;
        internal bool firstRun = true;

        public EmailDailyTask(IServiceProvider serviceProvider, IHostingEnvironment env)
        {
            _ServiceProvider = serviceProvider;
            _Env = env;
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
            //Erik - 3/22/2018 Prevent Mailing Everyone on First Run
            if(firstRun)
            {
                firstRun = false;
                return;
            }

            using (IServiceScope scope = _ServiceProvider.CreateScope())
            {
                var dSDBContext = scope.ServiceProvider.GetRequiredService<DSDBContext>();
                //Erik - 3/22/2018 Access wwwroot folder
                var webRoot = _Env.WebRootPath;
                string emailTemplate = string.Empty;
                //var builder = new BodyBuilder();
                if (dSDBContext == null)
                {
                    return;
                }
                try
                {
                    QueuedEmail emailToSend = dSDBContext.GetNextQueuedEmail();
                    if (emailToSend != null)
                    {
                        #region Get Daily Email Template
                        var pathToDailyEmailFile = webRoot + AppSettingConstant.SMTP_DAILY_EMAIL_TEMPLATE;
                       
                        using (StreamReader SourceReader = System.IO.File.OpenText(pathToDailyEmailFile))
                        {
                            emailTemplate = SourceReader.ReadToEnd();
                        }
                        #endregion

                            IEnumerable<string> emailPersons = dSDBContext.GetEmailContactsEmail();
                        EmailHelper.SendDailyMailAsync( emailTemplate
                            , emailPersons
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
                        #region Get Admin Email Template
                        var pathToDailyEmailFile = webRoot + AppSettingConstant.SMTP_ADMIN_EMAIL_TEMPLATE;

                        using (StreamReader SourceReader = System.IO.File.OpenText(pathToDailyEmailFile))
                        {
                            emailTemplate = SourceReader.ReadToEnd();
                        }
                        #endregion

                        AdminEmail adminEmailToSend = dSDBContext.GetAdminEmail(AppSettingConstant.ADMIN_EMAIL_NO_QUEUED_EMAILS);
                        if (adminEmailToSend == null)
                        {
                            adminEmailToSend = AdminEmailHelpers.GenerateDefaultNoQueuedEmailsAdminEmail(ref dSDBContext);
                        }

                        if(adminEmailToSend != null)
                        {

                            IEnumerable<string> adminPersons = dSDBContext.GetAdminContactsEmail();
                            if (adminPersons == null)
                            {
                                adminPersons = new List<string>();
                            }

                            List<string> adminPersonsList = new List<string>(adminPersons);
                            adminPersonsList.Add("butcer0@gmail.com");
                         

                            EmailHelper.SendAdminMailAsync( emailTemplate
                                  , adminPersonsList
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
