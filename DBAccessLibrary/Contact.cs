using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DBAccessLibrary
{
    public class Contact
    {

        public Contact()
        {
        }
        public Contact(int companyId, string name, string jobTitle, string email, string mobileNumber)
        {
            Name = name;
            JobTitle = jobTitle;
            Email = email;
            MobileNumber = mobileNumber;
        }

        public int Id { get; set; }

        [Required]
        [DisplayName("Company")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Please select a company")]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        [StringLength(50), MinLength(1)]
        [Required]
        public string Name { get; set; }

        [StringLength(250), MinLength(1)]
        [Required]
        public string JobTitle { get; set; }

        [StringLength(250), MinLength(1)]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Mobile Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        [StringLength(15), MinLength(1)]
        public string MobileNumber { get; set; }
    }

}