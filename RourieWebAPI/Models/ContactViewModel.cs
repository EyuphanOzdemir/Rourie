using DBAccessLibrary;
using RourieWebAPI.Models.Shared;

namespace RourieWebAPI.Models
{
    public class ContactViewModel:_NavigateModel
    {
        public Contact contact { get; set; }

        public ContactViewModel()
        {
            contact = new Contact();
        }

        public ContactViewModel(Contact contact)
        {
            this.contact = contact;
        }
    }
}
