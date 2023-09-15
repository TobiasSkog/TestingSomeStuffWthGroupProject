using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.App.BankManagement.User.Contact
{
    internal class ContactInformation
    {
        protected virtual string _email { get; set; }
        protected virtual string _phoneNumber { get; set; }
        protected virtual string _mobileNumer { get; set; }
        protected virtual string _street { get; set; }
        protected virtual string _city { get; set; }
        protected virtual string _postalNumber { get; set; }
    }
}