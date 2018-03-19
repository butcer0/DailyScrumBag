using DailyScrumBag.Infrastructure.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DailyScrumBag.Repository.Models
{
    public class QueuedEmail
    {
        public long Id { get; set; }
        public Guid GuidId { get; set; }
        private string _key;
        public string Key
        {
            get
            {
                if(_key == null)
                {
                    _key = Regex.Replace(Subject.ToLower(), AppSettingConstant.REGEXP_ALPHANUMERIC_NUMBER, "-");
                }
                return _key;
            }
            set { _key = value; }
        }
        [Display(Name = "Subject")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Subject must be between 1 and 100 characters long")]
        public string Subject { get; set; }
        public string Author { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
        public DateTime CreateDate { get; set; }
        private DateTime lastSent = DateTime.MinValue;
        public DateTime LastSent
        {
            get
            {
                return lastSent;
            }
            set
            {
                lastSent = value;
            }

        }
        private int queueOrder = -1;
        public int QueueOrder
        {
            get
            {
                return queueOrder;
            }
            set
            {
                queueOrder = value;
            }
        }


    }
}
