using GroupProject.App.BankManagement.Account.BankAccounts.BankTransactions;
using GroupProject.App.BankManagement.Interfaces;
using GroupProject.App.BankManagement.User;
using GroupProject.App.ConsoleHandling;
using ValidationUtility;

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

