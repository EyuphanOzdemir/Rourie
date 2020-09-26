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
        [Required]
        public string UserName { get; set; }


        [StringLength(250)]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int UserType { get; set; }
    }
}
