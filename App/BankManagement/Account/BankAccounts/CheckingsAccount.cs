using GroupProject.App.BankManagement.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.App.BankManagement.Account.BankAccounts
{
    public class CheckingsAccount : AccountBase
    {

        public CheckingsAccount(UserBase user) : base(AccountStatus.Active, AccountType.Checking, user, 0)
        {

        }
        public CheckingsAccount(AccountStatus accStatus, AccountType accType, UserBase accOwner, decimal balance = 0) : base(accStatus, accType, accOwner, balance)
        {

        }

        public CheckingsAccount CreateSavingsAccount(UserBase user)
        {
            return default;
        }
    }
}
