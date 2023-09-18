using GroupProject.App.BankManagement;
using GroupProject.App.BankManagement.Account;
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
    public static UserCustomer CreateUserCustomerAccount(UserTypes userType)
    {
      if (userType == UserTypes.Customer)
      {

        bool uniqueUsername = false;
        string firstName = StringValidationHelper.GetString("Enter your first name: ");
        string lastName = StringValidationHelper.GetString("Enter your last name: ");
        string username = StringValidationHelper.GetString("Enter a username: ");
        //while (true)
        //{
        //    Console.WriteLine("Username taken! Choose another username.");

        //}

        string password = StringValidationHelper.GetString("Enter a password: ");
        string socialSecurityNumber = StringValidationHelper.GetString("Enter your social security number: ");
        DateTime dateOfBirth = DateTimeValidationHelper.GetExactDateTimeAgeRestriction("Enter your date of birth ", "yyyy/MM/dd", 18);
        return new UserCustomer(firstName, lastName, username, password, socialSecurityNumber, dateOfBirth, userType);
      }
      return default;
    }

    public static UserAdmin CreateUserAdminAccount(UserTypes userType)
    {
      if (userType == UserTypes.Admin)
      {
        bool uniqueUsername = false;
        string firstName = StringValidationHelper.GetString("Enter your first name: ");
        string lastName = StringValidationHelper.GetString("Enter your last name: ");
        string username = "";
        //while (Database.ExistingUsername(username = StringValidationHelper.GetString("Enter a username: ")))
        //{
        //    Console.WriteLine("Username taken! Choose another username.");

        //}

        string password = StringValidationHelper.GetString("Enter a password: ");
        string socialSecurityNumber = StringValidationHelper.GetString("Enter your social security number: ");
        DateTime dateOfBirth = DateTimeValidationHelper.GetExactDateTimeAgeRestriction("Enter your date of birth ", "yyyy/MM/dd", 18);
        return new UserAdmin(firstName, lastName, username, password, socialSecurityNumber, dateOfBirth, userType);
      }
      return default;
    }


  }
}

