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

        public User(string userName, string email, string password)
        {
            this.UserName = userName;
            this.Email = email;
            this.Password = password;
        }

        [StringLength(60), MinLength(1)]
        [Required]
        [RegularExpression(CommonLibrary.Common.Regex.UserName)]
        public string UserName { get; set; }

        [StringLength(250), MinLength(1)]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(250), MinLength(1)]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
