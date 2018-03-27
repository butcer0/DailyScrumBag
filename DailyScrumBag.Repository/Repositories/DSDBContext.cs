﻿using DailyScrumBag.Infrastructure.Constants;
using DailyScrumBag.Repository.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DailyScrumBag.Repository.Repositories
{
    public class DSDBContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Special> Specials { get; set; }
        public DbSet<QueuedEmail> QueuedEmails { get; set; }
        public DbSet<AdminEmail> AdminEmails { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        public DSDBContext(DbContextOptions<DSDBContext> options)
            :base(options)
        {
            //Erik - 3/15/2018 Verify DB Existss, otherwise generates it
            Database.EnsureCreated();
        }

        public IEnumerable<Special> GetMonthlySpecials()
        {
            return Specials.ToArray();
        }

        public IEnumerable<Post> GetFeaturedSlides()
        {
            IEnumerable<Post> featuredSlides = Posts.Where(c => c.Featured).OrderByDescending(d => d.LastFeaturedDate);
            if (featuredSlides != null
                && featuredSlides.Count() > 5)
            {
                featuredSlides = featuredSlides.Take(5);
            }

            return new List<Post>();
        }

        public QueuedEmail GetNextQueuedEmail()
        {
            QueuedEmail emailToSend = null;
            try
            {
                if (QueuedEmails != null
                       && QueuedEmails.Any())
                {


                    IEnumerable<QueuedEmail> emailQueue = QueuedEmails.Where(c => c.QueueOrder > -1);
                    if (emailQueue.Where(c => c.QueueOrder == 0).Any())
                    {
                        //Erik - 3/19/2018 At least 1 order 0 found -> valid to send
                        if (emailQueue.Where(c => c.QueueOrder == 0).Count() > 1)
                        {
                            //Erik - 3/19/2018 More than 1 order 0 found -> send longest since sent, dont decrement order list
                            emailToSend = emailQueue.Where(c => c.QueueOrder == 0).OrderByDescending(c => c.LastSent).FirstOrDefault();
                            //Erik - 3/19/2018 Remove from queue
                            emailToSend.QueueOrder = -1;
                            emailToSend.LastSent = DateTime.Now;
                            this.SaveChangesAsync();
                        }
                        else
                        {
                            //Erik - 3/19/2018 Only 1 order 0 found, update, decrement all, and send
                            emailToSend = emailQueue.Where(c => c.QueueOrder == 0).FirstOrDefault();

                            foreach (var queuedEmail in emailQueue)
                            {
                                queuedEmail.QueueOrder -= 1;
                            }

                            emailToSend.LastSent = DateTime.Now;
                            this.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        //Erik - 3/27/2018 No emails queue order set -> possibly send admin email
                        emailToSend = this.QueuedEmails.Where(c => c.QueueOrder == -1).OrderByDescending(c => c.LastSent).FirstOrDefault();
                        emailToSend.LastSent = DateTime.UtcNow;
                        this.SaveChangesAsync();
                    }
                }
                else
                {
                    //Erik - 3/19/2018 Return null to trigger AdminEmail to send
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
            return emailToSend;
        }

        public AdminEmail GetAdminEmail(string key)
        {
            AdminEmail emailToSend = null;

            try
            {
                //Erik - 3/19/2018 No emails to send -> send email to admin email will not be sent today
                if (this.AdminEmails.Where(c => c.Key == key).Any())
                {
                    emailToSend = this.AdminEmails.Where(c => c.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                }
            }
            catch { }

            return emailToSend;
        }

        public IEnumerable<Contact> GetEmailContacts()
        {
            return this.Contacts.Where(c => c.InEmailList);
        }

        public IEnumerable<string> GetEmailContactsEmail()
        {
            return this.Contacts.Where(c => c.InEmailList && (!string.IsNullOrEmpty(c.EmailAddress))).Select(c => c.EmailAddress);
        }

        public IEnumerable<Contact> GetSMSContacts()
        {
            return this.Contacts.Where(c => c.InSMSList);
        }

        public IEnumerable<Contact> GetAdminContacts()
        {
            return this.Contacts.Where(c => c.IsAdmin);
        }

        public IEnumerable<string> GetAdminContactsEmail()
        {
            return this.Contacts.Where(c => c.IsAdmin && (!string.IsNullOrEmpty(c.EmailAddress))).Select(c => c.EmailAddress);
        }

        public void VerifyPreparedQueuedMessagesIntroduced(ref IHostingEnvironment env)
        {
            if(this.QueuedEmails.Any())
            {
                //Erik - 3/27/2018 Queued emails found
                return;
            }

            try
            {
                var webRoot = env.WebRootPath;


                var pathToDailyEmailFile = webRoot + AppSettingConstant.SMTP_PREPARED_EMAILS_PATH;

                string[] allfiles = Directory.GetFiles(pathToDailyEmailFile, "*.html", SearchOption.AllDirectories);

                for (int ii = 0; ii < allfiles.Length; ii++)
                {
                    using (StreamReader SourceReader = System.IO.File.OpenText(allfiles[ii]))
                    {
                        var preparedEmail = SourceReader.ReadToEnd();

                        this.QueuedEmails.Add(new QueuedEmail
                        {
                            GuidId = Guid.NewGuid(),
                            Subject = "Daily ScrumBag",
                            Author = "Scrum Manifesto",
                            Body = preparedEmail,
                            CreateDate = DateTime.UtcNow
                        });

                    }
                }

                this.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                Console.WriteLine("Error occured in VerifyPreparedQueuedMessagesIntroduced: " + ex.Message);
            }
            
        }
    }
}
