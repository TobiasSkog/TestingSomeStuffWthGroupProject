using GroupProject.App.BankManagement.Account;
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

    public virtual List<string> UserLog { get; protected set; }

    /// <summary>
    /// Used for the Json Deserialization to be able to recreate a user
    /// </summary>

    public UserBase()
    {
      UserLog = new List<string>();
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
        UserLog = new List<string>();
      }
    }

    //[JsonConstructor]
    //public UserBase(string firstName, string lastName, string username, string salt, string hashedPassword, sbyte remainingAttempts, string userId, string socialSecurityNumber, DateTime dateOfBirth, UserTypes userAccountType, UserStatuses userStatus, List<String> accountIds = null)
    //{
    //  FirstName = firstName;
    //  LastName = lastName;
    //  Username = username;
    //  Salt = salt;
    //  HashedPassword = hashedPassword;
    //  RemainingAttempts = remainingAttempts;
    //  UserId = userId;
    //  SocialSecurityNumber = socialSecurityNumber;
    //  DateOfBirth = dateOfBirth;
    //  UserType = userAccountType;
    //  UserStatus = userStatus;
    //  if (accountIds == null)
    //  {
    //    AccountIds = new List<string>();
    //  }
    //  else
    //  {
    //    AccountIds = accountIds;
    //  }
    //  UserLog = new List<string>();
    //}

    public virtual void AddToLog(string log)
    {
      if (UserLog == null)
      {
        UserLog = new List<string>();
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
      RemainingAttempts--;

      if (RemainingAttempts < 0 || UserStatus == UserStatuses.Locked)
      {
        return UserStatuses.Locked;
      }
      if (username == Username)
      {

        bool validPassword = BCrypt.Net.BCrypt.Verify(password + Salt, HashedPassword);
        if (validPassword)
        {
          RemainingAttempts = 3;
          return UserStatuses.Success;
        }
      }
      return UserStatuses.FailedLogin;
    }
    public virtual bool CheckIfUserHaveAnyAccounts()
    {
      if (AccountIds.Count == 0 || AccountIds == null)
      {
        bool CreateNewAccount = BoolValidationHelper.PromptForYesOrNo("You have no accounts yet!" +
          "\nWould you like to create one (Y/N): ");
        return CreateNewAccount;
      }
      return true;
    }
  }
}