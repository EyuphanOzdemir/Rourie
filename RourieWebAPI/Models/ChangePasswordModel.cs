using Microsoft.AspNetCore.Server.IIS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RourieWebAPI.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(CommonLibrary.Common.Regex.PASSWORD_REGEX,
            ErrorMessage = "The password should be 8 characters, and contain at least 1 Alphabet, 1 Number or 1 Special Character")]
        [Display(Name = "Password")]
        public string Password1 { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(CommonLibrary.Common.Regex.PASSWORD_REGEX,
            ErrorMessage = "The password should be 8 characters, and contain at least 1 Alphabet, 1 Number or 1 Special Character")]
        [Compare("Password1", ErrorMessage ="Passwords do not match")]
        [Display(Name ="Confirm Password")]
        public string Password2 { get; set; }

        public ChangePasswordModel(string password1="", string password2="")
        {
            this.Password1 = password1;
            this.Password2 = password2;
        }

        public ChangePasswordModel()
        {

        }
    }
}
