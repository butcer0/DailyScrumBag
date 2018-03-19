using System;
using System.ComponentModel.DataAnnotations;

namespace DailyScrumBag.Repository.Models
{
    public class Person
    {
        public long Id { get; set; }
        public Guid GuidId { get; set; }
        [Display(Name = "First Name")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 1, ErrorMessage ="First name must be between 1 and 100 characters long")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        [DataType(DataType.Text)]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Last name must be between 1 and 100 characters long")]
        public string LastName { get; set; }
        [Display(Name = "Telephone Number")]
        [DataType(DataType.PhoneNumber)]
        public string TelephoneNumber { get; set; }
        [Display(Name = "Telephone Special Instructions")]
        [DataType(DataType.Text)]
        public string TelephoneSpecialInstructions { get; set; }
        [Display(Name = "Street")]
        [DataType(DataType.Text)]
        public string Street { get; set; }
        [Display(Name = "City")]
        [DataType(DataType.Text)]
        public string City { get; set; }
        [Display(Name = @"State/Province")]
        [DataType(DataType.Text)]
        public string StateProvince { get; set; }
        [Display(Name = @"Country/Region")]
        [DataType(DataType.Text)]
        public string CountryRegion { get; set; }
        [Display(Name = "Home Address Special Instructions")]
        [DataType(DataType.Text)]
        public string HomeAddressSpecialInstructions { get; set; }
        [Display(Name = "Email Address")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [Display(Name = "Email Special Instructions")]
        [DataType(DataType.Text)]
        public string EmailSpecialInstructions { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        private bool inEmailList = true;
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
