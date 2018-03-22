using DailyScrumBag.Infrastructure.Constants;
using DailyScrumBag.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public DbSet<Person> People { get; set; }

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

        public IEnumerable<Person> GetEmailPersons()
        {
            return this.People.Where(c => c.InEmailList);
        }

        public IEnumerable<string> GetEmailPersonsEmails()
        {
            return this.People.Where(c => c.InEmailList && (!string.IsNullOrEmpty(c.EmailAddress))).Select(c => c.EmailAddress);
        }

        public IEnumerable<Person> GetSMSPersons()
        {
            return this.People.Where(c => c.InSMSList);
        }

        public IEnumerable<Person> GetAdminPersons()
        {
            return this.People.Where(c => c.IsAdmin);
        }

        public IEnumerable<string> GetAdminPersonsEmails()
        {
            return this.People.Where(c => c.IsAdmin && (!string.IsNullOrEmpty(c.EmailAddress))).Select(c => c.EmailAddress);
        }
    }
}
