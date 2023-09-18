using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Admin;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.App.ConsoleHandling;
using GroupProject.BankDatabase.JsonConverters;
using Newtonsoft.Json;
using ValidationUtility;

namespace GroupProject.BankDatabase
{
  public class Database
  {
    private readonly string _DATA;
    private readonly string _FOLDER;
    private string _rootFolder = AppDomain.CurrentDomain.BaseDirectory;
    private readonly string _PATH;
    private static readonly string _PATHACC = "CustomFiles\\Database\\Accounts.json";

    private List<UserBase> _users { get; set; }

    public Database(string data = "Users.json", string folder = "CustomFiles\\Database")
    {

      _DATA = data;
      _FOLDER = folder;
      _PATH = Path.Combine(_FOLDER, _DATA);
      if (!Directory.Exists(_FOLDER))
      {
        try
        {
          Directory.CreateDirectory(_FOLDER);
        }
        catch (Exception ex)
        {
          ExceptionHelper.ExceptionDetails(ex);
        }
      }
      if (!File.Exists(_PATH))
      {
        try
        {
          _users = InitializeDatabaseWithDefaultData();
          // File.Create(_PATH).Close();
          SaveData();
        }
        catch (Exception ex)
        {
          ExceptionHelper.ExceptionDetails(ex);
        }
      }
      if (!File.Exists(_PATHACC))
      {
        try
        {
          File.Create(_PATHACC).Close();
        }
        catch (Exception ex)
        {
          ExceptionHelper.ExceptionDetails(ex);
          Console.ReadKey();
        }
      }
      else
      {
        LoadDataFromFile();
      }

      ConsoleIO.StartUp();
    }
    public void AddUser(UserBase user)
    {
      _users.Add(user);
      SaveData();
    }
    public UserBase? AttemptUserLogin(string username, string password)
    {
      UserBase user;
      UserStatuses userStatus;
      do
      {
        user = GetUser(username);
        userStatus = user.Login(username, password);

        if (userStatus == UserStatuses.Locked)
        {
          ConsoleIO.WelcomeMenu();
        }

        if (userStatus != UserStatuses.Success)
        {
          if (user.Username == username)
          {
            Console.WriteLine($"\nIncorrect password! {user.RemainingAttempts} attempts remaining.");
            password = PasswordValidationHelper.PasswordValidation("\nEnter password: ");
            user = AttemptUserLogin(username, password);
          }
          else
          {
            Console.WriteLine($"\nIncorrect username!");
          }
        }
      } while (userStatus != UserStatuses.Success || userStatus == UserStatuses.Locked);


      return user;
    }

    public void AddNewAccountToUser(UserBase user, AccountBase account)
    {
      user.AddAccount(account);
      SaveData();
    }

    public void AddNewUserToDatabase(UserBase user)
    {
      _users.Add(user);
      SaveData();
    }

    private UserBase GetUser(string username)
    {
      try
      {
        var user = _users.Find(user => user.Username == username);
        return user;
      }
      catch (Exception ex)
      {
        ExceptionHelper.ExceptionDetails(ex);
        return default;
      }
    }

    public bool UserNameExists(string username)
    {
      try
      {
        bool exists = _users.Any(user => user.Username == username);
        return exists;
      }
      catch (Exception ex)
      {
        ExceptionHelper.ExceptionDetails(ex);
        return true;
      }

    }
    private void LoadDataFromFile()
    {
      try
      {
        using (StreamReader sr = new StreamReader(_PATH))
        {
          var jsonUsers = sr.ReadToEnd();
          List<UserBase> users = JsonConvert.DeserializeObject<List<UserBase>>(jsonUsers, new JsonSerializerSettings
          {
            TypeNameHandling = TypeNameHandling.None,
            Converters = { new CustomUserConverter() }
          });

          //List<UserBase> correctedList = new();
          //foreach (UserBase user in users)
          //{
          //  if (user.UserType == UserTypes.Customer)
          //  {
          //    correctedList.Add(new UserCustomer(
          //      user.FirstName,
          //      user.LastName,
          //      user.Username,
          //      user.Salt,
          //      user.HashedPassword,
          //      user.RemainingAttempts,
          //      user.
          //      ));
          //  }
          //  else if (user.UserType == UserTypes.Admin)
          //  {

          //  }
          //}
          _users = users;
        }
      }
      catch (Exception ex)
      {
        ExceptionHelper.ExceptionDetails(ex);
        _users = new List<UserBase>();

      }
    }

    public static List<AccountBase> GetAccountsInDatabase(List<string> accountIds)
    {
      try
      {

        using (StreamReader sr = new StreamReader(_PATHACC))
        {
          var jsonAccounts = sr.ReadToEnd();
          List<AccountBase> accounts = JsonConvert.DeserializeObject<List<AccountBase>>(jsonAccounts, new JsonSerializerSettings
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
    public void SaveData()
    {
      try
      {
        using (StreamWriter file = File.CreateText(_PATH))
        {
          JsonSerializer serializer = new JsonSerializer();
          serializer.Serialize(file, _users);
        }
      }
      catch (Exception ex)
      {
        ExceptionHelper.ExceptionDetails(ex);
        Console.ReadKey();
      }
    }
    private static List<UserBase> InitializeDatabaseWithDefaultData()
    {
      List<UserBase> createdUserList = new List<UserBase>()
      {
        new UserAdmin(   "Tobias", "Skog"    , "adminTobias", "password", "912632161363", new DateTime(1991, 10, 28), UserTypes.Admin   ),
        new UserAdmin(   "Aldor",  "Admin"   , "adminAldor" , "password", "126261236243", new DateTime(1980, 10, 21), UserTypes.Admin   ),
        new UserAdmin(   "Reidar", "Admin"   , "adminReidar", "password", "643621611212", new DateTime(1980, 10, 21), UserTypes.Admin   ),
        new UserCustomer("Aldor",  "User"    , "userAldor"  , "password", "112662362362", new DateTime(1970, 6, 1  ), UserTypes.Customer),
        new UserCustomer("Reidar", "User"    , "userReidar" , "password", "643634644123", new DateTime(1996, 8, 1  ), UserTypes.Customer),
        new UserCustomer("Dabba",  "Svensson", "userDabba"  , "password", "395315678456", new DateTime(2015, 1, 1  ), UserTypes.Customer),
        new UserCustomer("Ebba",   "Svensson", "userEbba"   , "password", "345745678456", new DateTime(2016, 3, 30 ), UserTypes.Customer),
        new UserCustomer("Fabba",  "Svensson", "userFabba"  , "password", "002461255321", new DateTime(2022, 4, 12 ), UserTypes.Customer),
        new UserCustomer("Gabba",  "Svensson", "userGabba"  , "password", "886641486516", new DateTime(2015, 9, 22 ), UserTypes.Customer),
        new UserCustomer("Habba",  "Svensson", "userHabba"  , "password", "888448484316", new DateTime(2018, 7, 26 ), UserTypes.Customer)
      };

      return createdUserList;
    }

  }
}