using DailyScrumBag.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DailyScrumBag.Repository.Repositories
{
    public class DSDBContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }

        public DSDBContext(DbContextOptions<DSDBContext> options)
            :base(options)
        {
            //Erik - 3/15/2018 Verify DB Existss, otherwise generates it
            Database.EnsureCreated();
        }
    }
}
