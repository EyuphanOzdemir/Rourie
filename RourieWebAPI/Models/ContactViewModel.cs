using DBAccessLibrary;
using RourieWebAPI.Models.Shared;

namespace RourieWebAPI.Models
{
    public class ContactViewModel : _NavigateAndSearchModel
    {
        public Contact contact { get; set; }

        public int CompanyCount{get; set;}

        public ContactViewModel()
        {
            contact = new Contact();
            this.CompanyCount = -1;
        }
        public ContactViewModel(int companyCount)
        {
            contact = new Contact();
            this.CompanyCount = companyCount;
        }

        public ContactViewModel(Contact contact, int companyCount)
        {
            this.contact = contact;
            this.CompanyCount = companyCount;
        }


    }
}
