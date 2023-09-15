using GroupProject.App.BankManagement.Account.BankAccounts.BankTransactions;
using GroupProject.App.BankManagement.Interfaces;
using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Customer;
using ValidationUtility;

namespace GroupProject.App.BankManagement.Account
{
    public abstract class AccountBase : ITransaction
    {
        protected virtual AccountStatus _accountStatus { get; set; }
        protected virtual AccountType _accountType { get; set; }
        protected virtual string _accountNumber { get; set; }
        protected virtual CurrencyType _currencyType { get; set; }
        protected virtual UserBase _accountOwner { get; set; }
        protected virtual decimal _accountBalance { get; set; }
        public AccountBase(AccountStatus accStatus, AccountType accType, UserBase accOwner, decimal balance = 0m)
        {
            _accountStatus = accStatus;
            _accountType = accType;
            _accountOwner = accOwner;
            if (balance >= 0)
            {
                _accountBalance = balance;
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
        public decimal GetBalance(UserType userType)
        {
            if (userType == UserType.Admin)
            {
                if (_accountStatus != AccountStatus.Locked)
                {
                    return _accountBalance;
                }
            }

            return -1;
        }


        public string GetAccountNumber(UserType userType)
        {
            if (userType == UserType.Admin)
            {
                if (_accountStatus != AccountStatus.Locked)
                {
                    return _accountNumber;
                }
            }

            return null;
        }
        public AccountType GetAccountType(UserType userType)
        {
            if (userType == UserType.Admin)
            {
                if (_accountStatus != AccountStatus.Locked)
                {
                    return _accountType;
                }
            }

            return default;
        }

        public CurrencyType GetCurrencyType(UserType userType)
        {
            if (userType == UserType.Admin)
            {
                if (_accountStatus != AccountStatus.Locked)
                {
                    return _currencyType;
                }
            }

            return default;
        }

        public virtual void DisplayRentFromDeposit(decimal depositAmount, int timeFrame)
        {

        }
        public TransactionStatus DisplayRentFromDeposit(Transaction transaction)
        {
            return TransactionStatus.Success;
        }


        public virtual void DepositMoney(decimal depositAmount, AccountBase userAccount)
        {
            if (depositAmount < 0)
            {
                throw new ArgumentException("You cannot deposit a negative amount.");
            }

            new Transaction((UserCustomer)_accountOwner, _accountBalance, _currencyType, this, null, _accountStatus);
        }

        public virtual void WithdrawMoney(decimal withdrawAmount, AccountBase userAccount)
        {
            if (_accountBalance < 0 || withdrawAmount > _accountBalance)
            {
                throw new ArgumentException("You cannot withdraw more than you have on your account.");
            }

            new Transaction((UserCustomer)_accountOwner, _accountBalance, _currencyType, this, null, _accountStatus);
        }
        public virtual void TransferMoney(decimal transferAmount, AccountBase userAccount, AccountBase targetAccount)
        {
            if (_accountBalance < 0 || transferAmount > _accountBalance)
            {
                throw new ArgumentException("You cannot transfer more than you have on your account.");
            }
            if (targetAccount._accountStatus != AccountStatus.Active)
            {
                throw new ArgumentException("Could not find the target account.");
            }
            new Transaction((UserCustomer)_accountOwner, _accountBalance, _currencyType, this, null, _accountStatus);
        }


        public virtual TransactionStatus DepositMoney(Transaction transaction)
        {
            if (transaction.Balance < 0)
            {
                return TransactionStatus.DepositIsANegativeValue;
            }

            _accountBalance += transaction.Balance;
            return TransactionStatus.DepositSuccess;
        }
        public virtual TransactionStatus WithdrawMoney(Transaction transaction)
        {
            if (_accountBalance < 0 || transaction.Balance > _accountBalance)
            {
                return TransactionStatus.BalanceTooLowForWithdrawal;
            }
            _accountBalance -= transaction.Balance;
            return TransactionStatus.WithdrawSuccess;
        }
        public virtual TransactionStatus TransferMoney(Transaction transaction)
        {
            if (_accountBalance < 0 || transaction.Balance > _accountBalance)
            {
                return TransactionStatus.BalanceTooLowForTransfer;
            }
            if (transaction.SourceAccount.GetAccountNumber(UserType.Admin) != null)
            {
                return TransactionStatus.DestinationAccountNotFound;
            }
            _accountBalance -= transaction.Balance;
            transaction.TargetAccount._accountBalance += transaction.Balance;
            return TransactionStatus.TransferSuccess;
        }
    }
}
