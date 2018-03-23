using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DailyScrumBag.Repository.Models
{
    [Table("tblComment")]
    public class Comment
    {
        [Key]
        [Column("Id")]
        public long Id { get; set; }

        [Column("PostId")]
        public long PostId { get; set; }

        [Column("Post")]
        public virtual Post Post { get; set; }

        [Column("Posted")]
        public DateTime Posted { get; set; }

        [Column("Author")]
        public string Author { get; set; }

        [Column("Body")]
        [Required]
        public string Body { get; set; }

    }
}
