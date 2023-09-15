using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.App.BankManagement.User.Contact
{
    public class Adress
    {
        protected virtual string _country { get; set; }
        protected virtual string _city { get; set; }
        protected virtual string _street { get; set; }
        protected virtual string _postalNumber { get; set; }
    }
}
