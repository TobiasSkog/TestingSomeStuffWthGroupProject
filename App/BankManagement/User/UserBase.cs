using GroupProject.App.BankManagement.Account;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ValidationUtility;

namespace GroupProject.App.BankManagement.User
{
  [Serializable]
  [JsonConverter(typeof(UserBaseConverter))]
  public abstract class UserBase : Bank
  {
    [JsonProperty("FirstName")]
    public virtual string FirstName { get; private set; }
    [JsonProperty("LastName")]
    public virtual string LastName { get; private set; }
    [JsonProperty("Username")]
    public virtual string Username { get; private set; }
    [JsonProperty("Salt")]
    public virtual string Salt { get; private set; }
    [JsonProperty("HashedPassword")]
    public virtual string HashedPassword { get; private set; }
    [JsonProperty("RemainingAttempts")]
    public virtual sbyte RemainingAttempts { get; private set; }
    [JsonProperty("UserId")]
    public virtual string UserId { get; private set; }

    [StringLength(12, MinimumLength = 3, ErrorMessage = "Social security numbers must be 3 or 12 characters.")]
    [JsonProperty("SocialSecuirtyNumber")]
    public virtual string SocialSecurityNumber { get; private set; }
    [JsonProperty("DateOfBirth")]
    public virtual DateTime DateOfBirth { get; private set; }
    [JsonProperty("UserAccountType")]
    public virtual UserType UserAccountType { get; private set; }
    [JsonProperty("UserAccountStatus")]
    public virtual UserStatus UserAccountStatus { get; private set; }
    [JsonProperty("UserAccounts")]
    private List<AccountBase> _accounts = new List<AccountBase>();
    public virtual List<AccountBase> Accounts
    {
      get => _accounts;
      private set => _accounts = value;
    }
    [JsonProperty("UserLog")]
    public virtual List<string> UserLog { get; set; }

    /// <summary>
    /// Used for the Json Deserialization to be able to recreate a user
    /// </summary>
    public UserBase()
    {
      Accounts = new List<AccountBase>();
      UserLog = new List<string>();
      string data = $"{Username}.txt";
      string folder = "CustomFiles\\Database";
      string path = Path.Combine(folder, data);
      PrintPropertiesToFile(path);
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
        UserAccountType = userType;
        UserAccountStatus = UserStatus.Exists;
        RemainingAttempts = 3;
        UserId = StringValidationHelper.CreateRandomString();
        Accounts = new List<AccountBase>();
        UserLog = new List<string>();
        string data = $"{Username}.txt";
        string folder = "CustomFiles\\Database";
        string path = Path.Combine(folder, data);
        PrintPropertiesToFile(path);
      }
    }
    public void PrintPropertiesToFile(string filePath)
    {
      // Open the file for writing (you can specify FileMode, etc. as needed).
      using (StreamWriter writer = new StreamWriter(filePath))
      {
        // Get the type of the current instance (UserBase).
        Type type = this.GetType();

        // Get all the public instance properties of the type.
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // Loop through the properties and write their names and values to the file.
        foreach (PropertyInfo property in properties)
        {
          object value = property.GetValue(this);
          writer.WriteLine($"{property.Name}: {value}");
        }
      }
    }

    //public string ConvertUserToJSON(UserType userType)
    //{
    //  string userJSON = "";

    //  if (userType != UserType.Admin)
    //  {
    //    return userJSON;
    //  }
    //  userJSON = JsonConvert.SerializeObject(this);
    //  return userJSON;
    //}
    //public static UserBase FromJSON(string json)
    //{
    //  return JsonConvert.DeserializeObject<UserBase>(json);
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
      if (Accounts == null)
      {
        Accounts = new List<AccountBase>();
      }
      Accounts.Add(account);
    }
    public virtual UserStatus Login(string username, string password)
    {
      RemainingAttempts--;

      if (RemainingAttempts <= 0 || UserAccountStatus == UserStatus.Locked)
      {
        return UserStatus.Locked;
      }
      if (username == Username)
      {

        bool validPassword = BCrypt.Net.BCrypt.Verify(password + Salt, HashedPassword);

        Console.WriteLine($"\n" +
            $"\t\t\tUser Input" +
            $"\n\tusername: {username}" +
            $"\n\tpassword: {password}" +
            $"\n\t\t\tUser Information" +
            $"\n\tUsername: {Username}" +
            $"\n\t\t\tPassword Details" +
            $"\n\t\tSalt: {Salt}" +
            $"\n\t\tHashedPassword: {HashedPassword}" +
            $"\n\t\tvalidPassword: {validPassword}");

        string data = $"{Username}LOGIN.txt";
        string folder = "CustomFiles\\Database";
        string path = Path.Combine(folder, data);
        PrintPropertiesToFile(path);


        if (validPassword)
        {
          return UserStatus.Success;
        }
      }
      return UserStatus.FailedLogin;
    }
    public virtual bool CheckIfUserHaveAnyAccounts()
    {
      if (Accounts.Count == 0 || UserAccountType != UserType.Admin)
      {
        bool CreateNewAccount = BoolValidationHelper.PromptForYesOrNo("You have no accounts yet, would you like to create one (Y/N): ");
        return CreateNewAccount;
      }
      return false;
    }
  }
}