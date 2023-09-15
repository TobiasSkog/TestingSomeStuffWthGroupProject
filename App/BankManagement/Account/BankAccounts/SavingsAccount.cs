using GroupProject.App.BankManagement.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.App.BankManagement.Account.BankAccounts
{
    public class SavingsAccount : AccountBase
    {
        public SavingsAccount(UserBase user) : base(AccountStatus.Active, AccountType.Saving, user, 0)
        {

        }
        public SavingsAccount(AccountStatus accStatus, AccountType accType, UserBase accOwner, decimal balance = 0) : base(accStatus, accType, accOwner, balance)
        {
        }

        public SavingsAccount CreateSavingsAccount(UserBase user)
        {
            return default;
        }
    }
}
