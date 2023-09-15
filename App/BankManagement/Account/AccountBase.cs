using GroupProject.App.BankManagement.User;
using GroupProject.App.LogicHandling;
namespace GroupProject.App.BankManagement.Account
{
    public abstract class AccountBase
    {
        protected virtual AccountStatus _accountStatus { get; set; }
        protected virtual AccountType _accountType { get; set; }
        protected virtual string _accountNumber { get; set; }
        protected virtual UserBase _accountOwner { get; set; }
        protected virtual decimal _balance { get; set; }

        public AccountBase(AccountStatus accStatus, AccountType accType, UserBase accOwner, decimal balance = 0m)
        {
            _accountStatus = AccountStatus.Active;
            _accountType = accType;
            _accountOwner = accOwner;
            if (balance >= 0)
            {
                _balance = balance;
            }
        }
    }
}
