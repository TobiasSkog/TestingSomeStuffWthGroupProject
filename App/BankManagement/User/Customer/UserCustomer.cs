using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.Account.BankAccounts.BankTransactions;
using GroupProject.App.BankManagement.Interfaces;
using GroupProject.App.ConsoleHandling;

using ValidationUtility;

namespace GroupProject.App.BankManagement.User.Customer
{
  public class UserCustomer : UserBase
  {
    public UserCustomer() : base()
    {

    }
    public UserCustomer(string firstName, string lastName, string username, string password, string socialSecurityNumber, DateTime dateOfBirth, UserType userType) : base(firstName, lastName, username, password, socialSecurityNumber, dateOfBirth, userType)
    {

    }
  }
}