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
    public static UserCustomer CreateUserCustomerAccount(UserTypes userType, Database DB)
    {
      if (userType == UserTypes.Customer)
      {
        string firstName = ConsoleIO.GetUserInformation("Enter your first name: ");
        string lastName = ConsoleIO.GetUserInformation("Enter your last name: ");
        string username = ConsoleIO.CreateUsername("Enter a username: ", DB);
        string password = ConsoleIO.Password("Enter a password: ");
        string socialSecurityNumber = ConsoleIO.GetUserInformation("Enter your social security number: ");
        DateTime dateOfBirth = DateTimeValidationHelper.GetExactDateTimeAgeRestriction("Enter your date of birth ", "yyyy/MM/dd", 18);
        return new UserCustomer(firstName, lastName, username, password, socialSecurityNumber, dateOfBirth, userType);
      }
      return default;
    }
    public static UserAdmin CreateUserAdminAccount(UserTypes userType, Database DB)
    {
      if (userType == UserTypes.Admin)
      {
        string firstName = ConsoleIO.GetUserInformation("Enter your first name");
        string lastName = ConsoleIO.GetUserInformation("Enter your last name");
        string username = ConsoleIO.CreateUsername("Enter a username", DB);
        string password = ConsoleIO.Password("Enter a password");
        string socialSecurityNumber = ConsoleIO.GetUserInformation("Enter your social security number");
        DateTime dateOfBirth = DateTimeValidationHelper.GetExactDateTimeAgeRestriction("Enter your date of birth", "yyyy/MM/dd", 18);
        return new UserAdmin(firstName, lastName, username, password, socialSecurityNumber, dateOfBirth, userType);
      }
      return default;
    }

    public static CheckingsAccount CreateCheckingsAccount(UserTypes userType)
    {
      if (userType == UserTypes.Customer)
      {
        CurrencyTypes curencyType = ConsoleIO.GetCurrencyTypeFromList();
        CheckingsAccount newChecking = new(AccountStatuses.Active, AccountTypes.Checking, 0, curencyType);

        return newChecking;
      }

      return default;
    }

    public static SavingsAccount CreateSavingsAccount(UserTypes userType)
    {
      if (userType == UserTypes.Customer)
      {
        Console.WriteLine("RENT IF YOU ADD X CASH DHORRA OVER Y YEARS");
        Thread.Sleep(599);
        CurrencyTypes currencyType = ConsoleIO.GetCurrencyTypeFromList();
        SavingsAccount newSavings = new(AccountStatuses.Active, AccountTypes.Saving, 0, currencyType);

        return newSavings;
      }

      return default;
    }





  }
}

