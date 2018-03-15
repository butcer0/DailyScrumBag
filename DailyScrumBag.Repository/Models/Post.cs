using DailyScrumBag.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace DailyScrumBag.Repository.Models
{
    public class Post
    {
        public long Id { get; set; }
        private string _key;
        public string Key
        {
            get
            {
                if (_key == null)
                {
                    _key = Regex.Replace(Title.ToLower(), AppSettingConstant.REGEXP_ALPHANUMERIC_NUMBER, "-");
                    #region Depricated - Use Constants
                    //_key = Regex.Replace(Title.ToLower(), "[^a-z0-9]", "-");
                    #endregion
                }
                return _key;
            }
            set { _key = value; }
        }

        [Display(Name = "Post Title")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 5,
            ErrorMessage = "Title must be between 5 and 100 characters long")]
        public string Title { get; set; }
        public string Author { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [MinLength(100, ErrorMessage = "Blog posts must be at least 100 characters long")]
        public string Body { get; set; }
        public DateTime Posted { get; set; }

    }
}
