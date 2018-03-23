using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DailyScrumBag.Repository.Models
{
    [Table("tblSpecial")]
    public class Special
    {
        [Key]
        [Column("Id")]
        public long Id { get; set; }

        [Column("Key")]
        public string Key { get; internal set; }

        [Column("Name")]
        public string Name { get; internal set; }

        [Column("Type")]
        public string Type { get; internal set; }

        [Column("Price")]
        public int Price { get; internal set; }
    }
}
