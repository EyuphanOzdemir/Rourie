using DBAccessLibrary;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RourieWebAPI.Models.Shared;

namespace RourieWebAPI.Models
{
    public class UserViewModel:_NavigateModel
    {
        public List<User> Users;
    }
}
