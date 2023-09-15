using GroupProject.App.BankManagement.Account.BankAccounts.BankTransactions;
using GroupProject.App.BankManagement.Interfaces;
using GroupProject.App.BankManagement.User;
using GroupProject.App.ConsoleHandling;
using ValidationUtility;

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
