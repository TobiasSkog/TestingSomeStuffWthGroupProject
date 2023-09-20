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

    public override (UserChoice Choice, AccountTransaction Transaction, TransactionLog Log) MakeDeposit()
    {
      //AccountBase? sourceAccount;
      TransactionLog log;

      var sourceAccount = ConsoleIO.GetSpecificAccount(this);
      if (sourceAccount == null)
      {
        log = new TransactionLog(Username, "FailedToMakeDeposit", 0, "Account Not Found", "Account Not Found");
        return (Choice: UserChoice.CustomerMenu, Transaction: null, Log: log);
      }
      decimal amount = ConsoleIO.AmountOfMoney("Amount to deposit");
      AccountTransaction transaction = sourceAccount.DepositMoney(this, amount);

      log = new TransactionLog(Username, "Made a deposit", amount, sourceAccount.AccountNumber, sourceAccount.AccountNumber);

      AddToLog(log);

      return (Choice: UserChoice.CustomerMenu, Transaction: transaction, Log: log);
    }
  }
}