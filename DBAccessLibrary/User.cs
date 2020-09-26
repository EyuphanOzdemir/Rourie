using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DBAccessLibrary
{
    public class User
    {
        public int Id { get; set; }

        public User()
        {
        }

        public User(string userName, string password)
        {
            this.UserName = userName;
            this.Password = password;
        }

        [StringLength(60)]
        [MinLength(5, ErrorMessage ="The username should be at least 5 characters")]
        public string UserName { get; set; }


        [StringLength(250)]
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(CommonLibrary.Common.Regex.PASSWORD_REGEX, 
            ErrorMessage = "The password should be 8 characters, and contain at least 1 Alphabet, 1 Number or 1 Special Character")]
        public string Password { get; set; }

        public int UserType { get; set; }
    }
}
