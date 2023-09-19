using GroupProject.App.BankManagement;
using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.Account.BankAccounts;
using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Admin;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.App.ConsoleHandling;
using GroupProject.BankDatabase;
using ValidationUtility;

namespace GroupProject.App.LogicHandling
{
  public static class AccountManager
  {
    public static UserCustomer CreateUserCustomerAccount(Database DB)
    {

      string firstName = ConsoleIO.GetUserInformation("Enter your first name: ");
      string lastName = ConsoleIO.GetUserInformation("Enter your last name: ");
      string username;

      while (true)
      {
        username = ConsoleIO.CreateUsername("Enter a username: ");
        if (DB.UserNameExists(username))
        {
          break;
        }
        Console.WriteLine("Username already exist. Choose a different one.");
      }

      string password = ConsoleIO.Password("Enter a password: ");
      string socialSecurityNumber = ConsoleIO.GetUserInformation("Enter your social security number: ");
      DateTime dateOfBirth = DateTimeValidationHelper.GetExactDateTimeAgeRestriction("Enter your date of birth ", "yyyy/MM/dd", 18);

      return new UserCustomer(firstName, lastName, username, password, socialSecurityNumber, dateOfBirth, UserTypes.Customer);
    }
    public static UserAdmin CreateUserAdminAccount(Database DB)
    {

      string firstName = ConsoleIO.GetUserInformation("Enter your first name: ");
      string lastName = ConsoleIO.GetUserInformation("Enter your last name: ");
      string username;

      while (true)
      {
        username = ConsoleIO.CreateUsername("Enter a username: ");
        if (DB.UserNameExists(username))
        {
          break;
        }
        Console.WriteLine("Username already exist. Choose a different one.");
      }

      string password = ConsoleIO.Password("Enter a password: ");
      string socialSecurityNumber = ConsoleIO.GetUserInformation("Enter your social security number: ");
      DateTime dateOfBirth = DateTimeValidationHelper.GetExactDateTimeAgeRestriction("Enter your date of birth ", "yyyy/MM/dd", 18);
      return new UserAdmin(firstName, lastName, username, password, socialSecurityNumber, dateOfBirth, UserTypes.Admin);
    }

    public static CheckingsAccount CreateCheckingsAccount()
    {
      CurrencyTypes curencyType = ConsoleIO.GetCurrencyTypeFromList();
      CheckingsAccount newChecking = new(AccountStatuses.Active, AccountTypes.Checking, 0, curencyType);

      return newChecking;
    }

    /// <summary>
    /// Need to add a check for the rent recieved when making a savings account and a prompt if the user want to add or not etc...
    /// </summary>
    /// <returns></returns>
    public static SavingsAccount CreateSavingsAccount()
    {
      Console.WriteLine("RENT IF YOU ADD X CASH DHORRA OVER Y YEARS");
      Thread.Sleep(599);
      CurrencyTypes currencyType = ConsoleIO.GetCurrencyTypeFromList();
      SavingsAccount newSavings = new(AccountStatuses.Active, AccountTypes.Saving, 0, currencyType);

      return newSavings;
    }
  }
}

