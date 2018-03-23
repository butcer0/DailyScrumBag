using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DailyScrumBag.Repository.Models
{
    [Table("tblContact")]
    public class Contact
    {
        [Key]
        [Column("Id")]
        public long Id { get; set; }

        [Column("GuidId")]
        public Guid GuidId { get; set; }

        [Column("FirstName")]
        [Display(Name = "First Name")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "First name must be between 1 and 100 characters long")]
        public string FirstName { get; set; }

        [Column("MiddleName")]
        [Display(Name = "Middle Name")]
        [DataType(DataType.Text)]
        public string MiddleName { get; set; }

        [Column("LastName")]
        [Display(Name = "Last Name")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Last name must be between 1 and 100 characters long")]
        public string LastName { get; set; }

        [Column("TelephoneNumber")]
        [Display(Name = "Telephone Number")]
        [DataType(DataType.PhoneNumber)]
        public string TelephoneNumber { get; set; }

        [Column("TelephoneSpecialInstructions")]
        [Display(Name = "Telephone Special Instructions")]
        [DataType(DataType.Text)]
        public string TelephoneSpecialInstructions { get; set; }

        [Column("Street")]
        [Display(Name = "Street")]
        [DataType(DataType.Text)]
        public string Street { get; set; }

        [Column("City")]
        [Display(Name = "City")]
        [DataType(DataType.Text)]
        public string City { get; set; }

        [Column("StateProvince")]
        [Display(Name = @"State/Province")]
        [DataType(DataType.Text)]
        public string StateProvince { get; set; }

        [Column("CountryRegion")]
        [Display(Name = @"Country/Region")]
        [DataType(DataType.Text)]
        public string CountryRegion { get; set; }

        [Column("HomeAddressSpecialInstructions")]
        [Display(Name = "Home Address Special Instructions")]
        [DataType(DataType.Text)]
        public string HomeAddressSpecialInstructions { get; set; }

        [Column("EmailAddress")]
        [Display(Name = "Email Address")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Column("EmailSpecialInstructions")]
        [Display(Name = "Email Special Instructions")]
        [DataType(DataType.Text)]
        public string EmailSpecialInstructions { get; set; }

        [Column("CreateDate")]
        public DateTime CreateDate { get; set; }

        [Column("ModifiedDate")]
        public DateTime ModifiedDate { get; set; }

        private bool inEmailList = true;
        [Column("InEmailList")]
        public bool InEmailList
        {
            get
            {
                return inEmailList;
            }
            set
            {
                inEmailList = value;
            }
        }

        private bool inSMSList = false;
        [Column("InSMSList")]
        public bool InSMSList
        {
            get
            {
                return inSMSList;
            }
            set
            {
                inSMSList = value;
            }
        }

        private bool isAdmin = false;
        [Column("IsAdmin")]
        public bool IsAdmin
        {
            get
            {
                return isAdmin;
            }
            set
            {
                isAdmin = value;
            }
        }


    }
}
