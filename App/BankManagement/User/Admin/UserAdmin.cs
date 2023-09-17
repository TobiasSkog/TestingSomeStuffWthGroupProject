﻿using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.App.ConsoleHandling;
using System.Runtime.Serialization;
using ValidationUtility;

namespace GroupProject.App.BankManagement.User.Admin
{
  public class UserAdmin : UserBase
  {
    public UserAdmin() : base()
    {

    }
    public UserAdmin(string firstName, string lastName, string username, string password, string socialSecurityNumber, DateTime dateOfBirth, UserType userType) : base(firstName, lastName, username, password, socialSecurityNumber, dateOfBirth, userType)
    {

    }

    public UserChoice UpdateCurrencyExchange()
    {
      return ConsoleIO.AdminCurrencyExchangeMenu();
    }

  }
}