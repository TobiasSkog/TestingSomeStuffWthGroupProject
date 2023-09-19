using GroupProject.App.BankManagement.Account;
using GroupProject.App.ConsoleHandling;
using GroupProject.BankDatabase.EventLogs;
using GroupProject.BankDatabase.EventLogs.Events;
using Newtonsoft.Json;
using ValidationUtility;

namespace GroupProject.App.BankManagement.User
{
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
    public virtual UserTypes UserType { get; protected set; }
    [JsonProperty]
    public virtual UserStatuses UserStatus { get; protected set; }
    [JsonProperty]
    public List<string> AccountIds { get; protected set; }
    public List<EventLog> UserLog { get; protected set; }

    /// <summary>
    /// Used for the Json Deserialization to be able to recreate a user
    /// </summary>

    public UserBase()
    {
      UserLog = new List<EventLog>();
    }
    public UserBase(string firstName, string lastName, string username, string password, string socialSecurityNumber, DateTime dateOfBirth, UserTypes userType)
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
        UserId = StringValidationHelper.CreateRandomString();
        AccountIds = new List<string>();
        UserLog = new List<EventLog>();
      }
    }

    public virtual void AddToLog(EventLog log)
    {
      if (UserLog == null)
      {
        UserLog = new List<EventLog>();
      }

      UserLog.Add(log);
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
    public virtual UserChoice ShowLog()
    {
      Console.WriteLine("This is the log");
      return UserChoice.CustomerMenu;
    }
    public virtual UserChoice LoanMoney()
    {
      Console.WriteLine("This is a loan");
      return UserChoice.CustomerMenu;
    }
    public virtual UserChoice MakeWithdrawal()
    {
      Console.WriteLine("This is a withdrawal");
      return UserChoice.CustomerMenu;
    }
    public virtual UserChoice MakeDeposit()
    {
      Console.WriteLine("This is a deposit");
      return UserChoice.CustomerMenu;
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