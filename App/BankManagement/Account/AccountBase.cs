using GroupProject.App.BankManagement.User;
using ValidationUtility;

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
            _accountStatus = accStatus;
            _accountType = accType;
            _accountOwner = accOwner;
            if (balance >= 0)
            {
                _balance = balance;
            }
            _accountNumber = StringValidationHelper.CreateRandomString(20, "1234567890");
        }

        public virtual UserBase AccountOwner(UserType userType)
        {
            if (userType == UserType.Admin)
            {
                return _accountOwner;
            }

            return null;
        }
    }
}
