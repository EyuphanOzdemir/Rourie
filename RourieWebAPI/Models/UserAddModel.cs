using Microsoft.AspNetCore.Server.IIS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RourieWebAPI.Models
{
    public class UserAddModel
    {
        [Display(Name ="User Type")]
        public int UserType { get; set;}
        
        
        [StringLength(60)]
        [MinLength(5, ErrorMessage = "The username should be at least 5 characters")]
        public string UserName { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(CommonLibrary.Common.Regex.PASSWORD_REGEX,
            ErrorMessage = CommonLibrary.Common.Regex.PASSWORD_ERROR_MESSAGE)]
        [Display(Name = "Password")]
        public string Password1 { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(CommonLibrary.Common.Regex.PASSWORD_REGEX,
            ErrorMessage = CommonLibrary.Common.Regex.PASSWORD_ERROR_MESSAGE)]
        [Compare("Password1", ErrorMessage ="Passwords do not match")]
        [Display(Name ="Confirm Password")]
        public string Password2 { get; set; }

        public UserAddModel()
        {

        }
    }
}
