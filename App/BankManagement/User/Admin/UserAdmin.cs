using GroupProject.App.ConsoleHandling;
using Newtonsoft.Json;

namespace GroupProject.App.BankManagement.User.Admin
{
  public class UserAdmin : UserBase
  {

    public UserAdmin()
    {

    }
    public UserAdmin(string firstName, string lastName, string username, string password, string socialSecurityNumber, DateTime dateOfBirth, UserTypes userType = UserTypes.Admin) : base(firstName, lastName, username, password, socialSecurityNumber, dateOfBirth, userType)
    {

    }
    [JsonConstructor]
    public UserAdmin(string firstName, string lastName, string username, string salt, string hashedPassword, sbyte remainingAttempts, string userId, string socialSecurityNumber, DateTime dateOfBirth, UserTypes userType, UserStatuses userStatus, List<String> accountIds = null)
    {
      FirstName = firstName;
      LastName = lastName;
      Username = username;
      Salt = salt;
      HashedPassword = hashedPassword;
      RemainingAttempts = remainingAttempts;
      UserId = userId;
      SocialSecurityNumber = socialSecurityNumber;
      DateOfBirth = dateOfBirth;
      UserType = userType;
      UserStatus = userStatus;
      if (accountIds == null)
      {
        AccountIds = new List<string>();
      }
      else
      {
        AccountIds = accountIds;
      }
      UserLog = new List<string>();
    }
    public UserChoice UpdateCurrencyExchange()
    {
      return ConsoleIO.AdminCurrencyExchangeMenu();
    }
  }
}