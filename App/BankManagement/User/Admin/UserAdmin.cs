using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.Account.BankAccounts.BankTransactions;
using GroupProject.App.ConsoleHandling;
using GroupProject.BankDatabase.EventLogs;
using GroupProject.BankDatabase.EventLogs.Events;
using Newtonsoft.Json;
namespace GroupProject.App.BankManagement.User.Admin
{
  public class UserAdmin : UserBase
  {

    public UserAdmin()
    {

    }
    public UserAdmin(string firstName, string lastName, string username, string password, string socialSecurityNumber, DateTime dateOfBirth, UserType userType = UserType.Admin) : base(firstName, lastName, username, password, socialSecurityNumber, dateOfBirth, userType)
    {

    }
    [JsonConstructor]
    public UserAdmin(string firstName, string lastName, string username, string salt, string hashedPassword, sbyte remainingAttempts, string userId, string socialSecurityNumber, DateTime dateOfBirth, UserType userType, UserStatuses userStatus, UserLogs userLog, List<string> logIds, List<String> accountIds = null)
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
      if (userLog == null)
      {
        UserLog = new UserLogs();
      }
      else
      {
        UserLog = userLog;
      }
      if (logIds == null)
      {
        LogIds = new List<string>();
      }
      else
      {
        LogIds = logIds;
      }
    }
    public UserChoice UpdateCurrencyExchange()
    {
      return ConsoleIO.AdminCurrencyExchangeMenu(this);
    }

    public override (UserChoice Choice, AccountTransaction Transaction, TransactionLog Log) MakeDeposit(List<AccountBase> sourceAccounts)
    {
      throw new NotImplementedException();
    }

    public override (UserChoice Choice, AccountTransaction Transaction, TransactionLog Log) MakeWithdrawal(List<AccountBase> sourceAccounts)
    {
      throw new NotImplementedException();
    }

    public override (UserChoice Choice, AccountTransaction Transaction, TransactionLog Log) LoanMoney(List<AccountBase> sourceAccounts)
    {
      throw new NotImplementedException();
    }

    public override (UserChoice Choice, AccountTransaction Transaction, TransactionLog Log) MakeTransfer(List<AccountBase> sourceAccounts, AccountBase targetAccount, UserBase targetUser)
    {
      throw new NotImplementedException();
    }
  }
}