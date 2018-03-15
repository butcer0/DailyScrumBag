using DailyScrumBag.Interfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DailyScrumBag.Repository.Models
{
    public class UserContext : IUserContext
    {
        public int UId { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public Dictionary<string, string> DSSettings { get; set; }

    }
}
