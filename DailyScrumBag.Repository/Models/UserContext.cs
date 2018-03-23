using DailyScrumBag.Interfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DailyScrumBag.Repository.Models
{
    [Table("tblUserContext")]
    public class UserContext : IUserContext
    {
        [Key]
        [Column("uId")]
        public int UId { get; set; }

        [Column("UserId")]
        public int UserId { get; set; }

        [Column("Username")]
        public string Username { get; set; }

        [Column("DSSettings")]
        public Dictionary<string, string> DSSettings { get; set; }

    }
}
