using DailyScrumBag.Infrastructure.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace DailyScrumBag.Repository.Models
{
    [Table("tblQueuedEmail")]
    public class QueuedEmail
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
                if(_key == null)
                {
                    _key = Regex.Replace(Subject.ToLower(), AppSettingConstant.REGEXP_ALPHANUMERIC_NUMBER, "-");
                }
                return _key;
            }
            set { _key = value; }
        }

        [Column("Subject")]
        [Display(Name = "Subject")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Subject must be between 1 and 100 characters long")]
        public string Subject { get; set; }

        [Column("Author")]
        public string Author { get; set; }

        [Column("Body")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [Column("CreateDate")]
        public DateTime CreateDate { get; set; }

        private DateTime lastSent = DateTime.MinValue;
        [Column("LastSent")]
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
        [Column("QueueOrder")]
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
