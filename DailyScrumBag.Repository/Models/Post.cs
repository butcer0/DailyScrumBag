using DailyScrumBag.Infrastructure.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace DailyScrumBag.Repository.Models
{
    [Table("tblPost")]
    public class Post
    {
        [Key]
        [Column("Id")]
        public long Id { get; set; }

        [Column("GuidId")]
        public Guid GuidId { get; set; }

        private string _key;
        [Column("Key")]
        public string Key
        {
            get
            {
                if (_key == null)
                {
                    _key = Regex.Replace(Title.ToLower(), AppSettingConstant.REGEXP_ALPHANUMERIC_NUMBER, "-");
                }
                return _key;
            }
            set { _key = value; }
        }

        [Column("Title")]
        [Display(Name = "Post Title")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 5,
            ErrorMessage = "Title must be between 5 and 100 characters long")]
        public string Title { get; set; }

        [Column("Author")]
        public string Author { get; set; }

        [Column("Body")]
        [Required]
        [DataType(DataType.MultilineText)]
        [MinLength(100, ErrorMessage = "Blog posts must be at least 100 characters long")]
        public string Body { get; set; }

        [Column("Posted")]
        public DateTime Posted { get; set; }

        [Column("LastFeaturedDate")]
        public DateTime LastFeaturedDate { get; set; }

        [Column("Featured")]
        public bool Featured { get; set; }
        

    }
}
