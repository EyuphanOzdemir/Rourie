using DBAccessLibrary;
using RourieWebAPI.Models.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RourieWebAPI.Models
{
    public class ContactListViewModel : _NavigateModel
    {
        public int SearchCompanyId { get; set; }
        public List<Contact> Contacts;
    }
}
