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
            ErrorMessage = CommonLibrary.Common.Regex.PASSWORD_ERROR_MESSAGE)]
        public string Password { get; set; }

        [Display(Name ="User Type")]
        public int UserType { get; set; }

        [Display(Name = "User Type")]
        //here actually this should not be like this
        //but given time limitation it is ok for now
        //normally UserType should be of an enum type
        public string UserTypeString { get { return this.UserType == 0 ? "Normal" : "Admin"; } }
    }
}
