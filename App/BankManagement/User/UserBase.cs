using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.Account.BankAccounts.BankTransactions;
using GroupProject.App.ConsoleHandling;
using GroupProject.BankDatabase.EventLogs;
using GroupProject.BankDatabase.EventLogs.Events;
using GroupProject.BankDatabase.JsonConverters;
using Newtonsoft.Json;
using System.Transactions;
using ValidationUtility;

namespace GroupProject.App.BankManagement.User
{
  [JsonConverter(typeof(CustomUserConverter))]
  [JsonObject(MemberSerialization.OptIn)]
  public abstract class UserBase
  {
    [JsonProperty]
    public virtual string FirstName { get; protected set; }
    [JsonProperty]
    public virtual string LastName { get; protected set; }
    [JsonProperty]
    public virtual string Username { get; protected set; }
    [JsonProperty]
    public virtual string Salt { get; protected set; }
    [JsonProperty]
    public virtual string HashedPassword { get; protected set; }
    [JsonProperty]
    public virtual sbyte RemainingAttempts { get; protected set; }
    [JsonProperty]
    public virtual string UserId { get; protected set; }
    [JsonProperty]
    public virtual string SocialSecurityNumber { get; protected set; }
    [JsonProperty]
    public virtual DateTime DateOfBirth { get; protected set; }
    [JsonProperty]
    public virtual UserType UserType { get; protected set; }
    [JsonProperty]
    public virtual UserStatuses UserStatus { get; protected set; }
    [JsonProperty]
    public List<string> AccountIds { get; protected set; }
    [JsonProperty]
    public List<string> LogIds { get; protected set; }
    [JsonProperty]
    public UserLogs UserLog { get; protected set; }

    /// <summary>
    /// Used for the Json Deserialization to be able to recreate a user
    /// </summary>

    public UserBase()
    {
      UserLog = new UserLogs();
      LogIds = new List<string>();
    }
    public UserBase(string firstName, string lastName, string username, string password, string socialSecurityNumber, DateTime dateOfBirth, UserType userType)
    {
      if (BoolValidationHelper.ValidateAgeRestriction(dateOfBirth, 1))
      {
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Salt = BCrypt.Net.BCrypt.GenerateSalt();
        HashedPassword = BCrypt.Net.BCrypt.HashPassword(password + Salt);
        SocialSecurityNumber = socialSecurityNumber;
        DateOfBirth = dateOfBirth;
        UserType = userType;
        UserStatus = UserStatuses.Exists;
        RemainingAttempts = 3;
        UserId = StringValidationHelper.CreateRandomString(15);
        AccountIds = new List<string>();
        UserLog = new UserLogs();
        LogIds = new List<string>();

      }
    }
    [JsonConstructor]
    public UserBase(string firstName, string lastName, string username, string salt, string hashedPassword, sbyte remainingAttempts, string userId, string socialSecurityNumber, DateTime dateOfBirth, UserType userType, UserStatuses userStatus, UserLogs userLog, List<string> logIds, List<String> accountIds = null)
    {
      if (BoolValidationHelper.ValidateAgeRestriction(dateOfBirth, 1))
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

        if (logIds == null || logIds.Count == 0)
        {
          LogIds = new List<string>();
        }
        else
        {
          LogIds = logIds;
        }
      }
    }

    public virtual void AddToLog(EventLog log)
    {
      if (UserLog == null)
      {
        UserLog = new UserLogs();
      }
      UserLog.AddUserLog(Username, log);
    }

    public virtual List<EventLog> GetUserLogs()
    {
      return UserLog.GetUserLogs(Username);
    }
    public virtual void AddAccount(AccountBase account)
    {
      if (AccountIds == null)
      {
        AccountIds = new List<string>();
      }

      AccountIds.Add(account.AccountId);
    }
    public virtual UserStatuses Login(string username, string password)
    {
      if (RemainingAttempts <= 0 || UserStatus == UserStatuses.Locked)
      {
        return UserStatuses.Locked;
      }

      RemainingAttempts--;

      if (username == Username)
      {
        bool validPassword = BCrypt.Net.BCrypt.Verify(password + Salt, HashedPassword);

        if (validPassword)
        {
          RemainingAttempts = 3;
          return UserStatuses.Success;
        }

        if (RemainingAttempts <= 0)
        {
          return UserStatuses.Locked;
        }
      }

      return UserStatuses.FailedLogin;
    }

    public abstract (UserChoice Choice, AccountTransaction Transaction, TransactionLog Log) MakeDeposit();


    public virtual (UserChoice Choice, TransactionLog Log) MakeWithdrawal()
    {
      Console.WriteLine("This is a loan");
      AccountBase sourceAccount = default;
      AccountBase targetAccount = default;
      TransactionLog log = new TransactionLog(Username, "Made a deposit", 4000, sourceAccount.AccountNumber, sourceAccount.AccountNumber);

      AddToLog(log);

      return (Choice: UserChoice.CustomerMenu, Log: log);
    }

    public virtual (UserChoice Choice, TransactionLog Log) LoanMoney()
    {
      Console.WriteLine("This is a loan");
      AccountBase sourceAccount = default;
      AccountBase targetAccount = default;
      TransactionLog log = new TransactionLog(Username, "Made a deposit", 4000, sourceAccount.AccountNumber, sourceAccount.AccountNumber);

      AddToLog(log);

      return (Choice: UserChoice.CustomerMenu, Log: log);
    }
    public virtual bool CheckIfUserHaveAnyAccounts()
    {
      if (AccountIds.Count == 0 || AccountIds == null)
      {
        return false;
      }
      return true;
    }
  }
}