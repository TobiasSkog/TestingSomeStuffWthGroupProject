﻿using GroupProject.App.BankManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.App.BankManagement.User.Customer
{
    public class UserCustomer : UserBase, ITransaction
    {
        public UserCustomer(string firstName, string lastName, string socialSecurityNumber, DateTime dateOfBirth) : base(firstName, lastName, socialSecurityNumber, dateOfBirth, UserType.Customer)
        {

        }

        public void CheckBalance(string accNumber)
        {
            throw new NotImplementedException();
        }

        public void DepositMoney(decimal balance, string accNumber)
        {
            throw new NotImplementedException();
        }

        public void TransferMoney(decimal balance, string userAccNumber, string destAccNumber)
        {
            throw new NotImplementedException();
        }

        public void WithdrawMoney(decimal balance, string accNumber)
        {
            throw new NotImplementedException();
        }
    }
}