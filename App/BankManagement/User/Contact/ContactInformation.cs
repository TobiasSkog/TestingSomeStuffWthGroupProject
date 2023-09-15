using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.App.BankManagement.User.Contact
{
    public class ContactInformation
    {
        protected virtual Email _email { get; set; }
        protected virtual Phone _phone { get; set; }
        protected virtual Adress _adress { get; set; }

    }
}