using GroupProject.Bank.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Bank.Account
{
    public abstract class AccountBase : UserBase
    {
        protected AccountStatus _accountStatus { get; set; }
        protected AccountType _accountType { get; set; }
        protected int _accountNumber { get; set; }
        protected UserBase _accountOwner { get; set; }
        protected decimal _balance { get; set; }

        public AccountBase(string firstName, string lastName, string socialSecurityNumber, DateTime dateOfBirth, UserType userType, AccountType accType, UserBase accOwner, decimal balance = 0m) : base(firstName, lastName, socialSecurityNumber, dateOfBirth, userType)
        {
            _accountStatus = AccountStatus.Active;
            _accountType = accType;
            _accountOwner = accOwner;
            if (balance >= 0)
            {
                _balance = balance;
            }
        }
        public virtual void Deposit(decimal amount)
        {
            _balance += amount;
        }

    }
}
