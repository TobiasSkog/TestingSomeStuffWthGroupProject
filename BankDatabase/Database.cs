using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.Account.BankAccounts;
using GroupProject.App.BankManagement.Account.BankAccounts.BankTransactions;
using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Admin;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.App.ConsoleHandling;
using GroupProject.App.LogicHandling;
using GroupProject.BankDatabase.EventLogs;
using GroupProject.BankDatabase.JsonConverters;
using Newtonsoft.Json;
using System.Transactions;
using ValidationUtility;

namespace GroupProject.BankDatabase
{
  public class Database
  {
    private readonly string _USERFILE = "Users.json";
    private readonly string _ACCOUNTFILE = "Accounts.json";
    private readonly string _BASEFOLDER = "CustomFiles\\Database";
    private static string _PATHUSERS { get; set; }
    private static string _PATHACCS { get; set; }
    private Logger _Logger { get; set; }
    private List<UserBase> _users { get; set; }
    private List<AccountBase> _accounts { get; set; }
    private TransactionScheduler _transactionScheduler { get; set; }

    public Database(Logger logger, int transactionUpdateIntervall = 15)
    {

      _Logger = logger;
      _transactionScheduler = new TransactionScheduler(transactionUpdateIntervall, _Logger, this); // CHANGE FROMSECNODS TO FROMMINUTES
      _PATHUSERS = Path.Combine(_BASEFOLDER, _USERFILE);
      _PATHACCS = Path.Combine(_BASEFOLDER, _ACCOUNTFILE);



      bool foundUserDatabase = File.Exists(_PATHUSERS);
      bool foundAccountDatabase = File.Exists(_PATHACCS);

      CreateBaseFolderIfNotExisting();

      if (!foundUserDatabase)
      {
        _users = InitializeDatabaseWithDefaultData();
        SaveData(true, false);
      }
      if (!foundAccountDatabase)
      {
        _accounts = InitializeDatabaseWithDefaultAccounts();
        SaveData(false, true);
      }
      else
      {
        LoadDataFromFile();
      }

      ConsoleIO.StartUp();
    }

    private void CreateBaseFolderIfNotExisting()
    {
      if (!Directory.Exists(_BASEFOLDER))
      {
        try
        {
          Directory.CreateDirectory(_BASEFOLDER);
        }
        catch (Exception ex)
        {
          ExceptionHelper.ExceptionDetails(ex);
        }
      }
    }

    internal void AddUser(UserBase user)
    {
      _users.Add(user);
      SaveData();
    }
    internal void AddNewAccountToUser(UserBase user, AccountBase account)
    {
      user.AddAccount(account);
      _users.Remove(user);
      _users.Add(user);
      _accounts.Add(account);
      SaveData();
    }

    internal void ScheduleTransaction(AccountTransaction transaction)
    {
      _transactionScheduler.QueueTransaction(transaction);
    }
    internal void AddNewUserToDatabase(UserBase user)
    {
      _users.Add(user);
      SaveData();
    }
    internal UserBase? GetUser(string username)
    {
      try
      {
        if (UserNameExists(username))
        {
          UserBase? user = _users.Find(user => user.Username == username);
          return user;
        }
        return null;
      }

      catch (Exception ex)
      {
        ExceptionHelper.ExceptionDetails(ex);
        return default;
      }
    }

    internal bool UserNameExists(string username)
    {
      try
      {
        if (username != null)
        {
          bool exists = _users.Any(user => user.Username == username);
          return exists;
        }
        return true;
      }
      catch (Exception ex)
      {
        ExceptionHelper.ExceptionDetails(ex);
        return false;
      }

    }

    private void LoadDataFromFile()
    {
      try
      {
        using (StreamReader asr = new StreamReader(_PATHUSERS))
        {
          var jsonUsers = asr.ReadToEnd();
          List<UserBase>? users = JsonConvert.DeserializeObject<List<UserBase>>(jsonUsers, new JsonSerializerSettings
          {
            TypeNameHandling = TypeNameHandling.None,
            Converters = { new CustomUserConverter() }
          });
          asr.Close();
          _users = users;
        }

        using (StreamReader bsr = new StreamReader(_PATHACCS))
        {
          var jsonAccounts = bsr.ReadToEnd();
          List<AccountBase>? accounts = JsonConvert.DeserializeObject<List<AccountBase>>(jsonAccounts, new JsonSerializerSettings
          {
            TypeNameHandling = TypeNameHandling.None,
            Converters = { new CustomAccountConverter() }
          });
          bsr.Close();
          _accounts = accounts;
        }
      }
      catch (Exception ex)
      {
        ExceptionHelper.ExceptionDetails(ex);
        _users = new List<UserBase>();
        _accounts = new List<AccountBase>();

      }
    }

    internal static List<AccountBase> LoadUserAccounts(List<string> accountIds)
    {
      try
      {

        using (StreamReader sr = new StreamReader(_PATHACCS))
        {
          var jsonAccounts = sr.ReadToEnd();
          List<AccountBase>? accounts = JsonConvert.DeserializeObject<List<AccountBase>>(jsonAccounts, new JsonSerializerSettings
          {
            TypeNameHandling = TypeNameHandling.Objects,
            Converters = { new CustomAccountConverter() }
          });

          List<AccountBase> foundAccounts = new();
          foreach (var account in accounts)
          {
            if (accountIds.Contains(account.AccountId))
            {
              foundAccounts.Add(account);
            }
          }

          return foundAccounts;

        }
      }
      catch (Exception ex)
      {
        ExceptionHelper.ExceptionDetails(ex);
        Console.ReadKey();
        return null;
      }
    }
    internal void SaveData(bool saveDatabase = true, bool saveAccounts = true)
    {
      try
      {
        if (saveDatabase)
        {

          var jsonUserSettings = new JsonSerializerSettings
          {
            TypeNameHandling = TypeNameHandling.Objects,
            Converters = { new CustomUserConverter() }
          };
          var updatedUsers = JsonConvert.SerializeObject(_users, Formatting.Indented, jsonUserSettings);

          File.WriteAllText(_PATHUSERS, updatedUsers);

        }
        if (saveAccounts)
        {
          var jsonAccountSettings = new JsonSerializerSettings
          {
            TypeNameHandling = TypeNameHandling.Objects,
            Converters = { new CustomAccountConverter() }
          };
          var updatedAccounts = JsonConvert.SerializeObject(_accounts, Formatting.Indented, jsonAccountSettings);

          File.WriteAllText(_PATHACCS, updatedAccounts);
        }
      }
      catch (Exception ex)
      {
        ExceptionHelper.ExceptionDetails(ex);
        Console.ReadKey();
      }
    }

    internal void UpdateAccountDatabase(AccountBase account)
    {
      if (_accounts == null)
      {
        _accounts = new List<AccountBase>();
      }
      int existingAccountIndex = _accounts.FindIndex(a => a.AccountId == account.AccountId);
      if (existingAccountIndex != -1)
      {
        _accounts[existingAccountIndex] = account;
      }
      else
      {
        _accounts.Add(account);
      }

      SaveData(false, true);
    }
    private static List<UserBase> InitializeDatabaseWithDefaultData()
    {
      List<UserBase> createdUserList = new List<UserBase>()
      {
        new UserAdmin(   "Tobias", "Skog"    , "adminTobias", "password", "912632161363", new DateTime(1991, 10, 28), UserType.Admin   ),
        new UserAdmin(   "Aldor",  "Admin"   , "adminAldor" , "password", "126261236243", new DateTime(1980, 10, 21), UserType.Admin   ),
        new UserAdmin(   "Reidar", "Admin"   , "adminReidar", "password", "643621611212", new DateTime(1980, 10, 21), UserType.Admin   ),
        new UserCustomer("Aldor",  "User"    , "userAldor"  , "password", "112662362362", new DateTime(1970, 6, 1  ), UserType.Customer),
        new UserCustomer("Reidar", "User"    , "userReidar" , "password", "643634644123", new DateTime(1996, 8, 1  ), UserType.Customer),
        new UserCustomer("Dabba",  "Svensson", "userDabba"  , "password", "395315678456", new DateTime(1997, 1, 1  ), UserType.Customer),
        new UserCustomer("Ebba",   "Svensson", "userEbba"   , "password", "345745678456", new DateTime(1998, 3, 30 ), UserType.Customer),
        new UserCustomer("Fabba",  "Svensson", "userFabba"  , "password", "002461255321", new DateTime(1999, 4, 12 ), UserType.Customer),
        new UserCustomer("Gabba",  "Svensson", "userGabba"  , "password", "886641486516", new DateTime(2000, 9, 22 ), UserType.Customer),
        new UserCustomer("Habba",  "Svensson", "userHabba"  , "password", "888448484316", new DateTime(2001, 7, 26 ), UserType.Customer)
      };

      return createdUserList;
    }

    private static List<AccountBase> InitializeDatabaseWithDefaultAccounts()
    {
      List<AccountBase> createdAccountList = new List<AccountBase>()
      {
        new CheckingsAccount(AccountStatuses.Active, AccountTypes.Checking, 1000000000M, CurrencyTypes.SEK)
      };

      return createdAccountList;
    }

  }
}
