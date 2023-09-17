using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Admin;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.App.ConsoleHandling;
using Newtonsoft.Json;
using Spectre.Console;
using ValidationUtility;

namespace GroupProject.BankDatabase
{
  public class Database
  {
    private readonly string _DATA;
    private readonly string _FOLDER;
    private string _rootFolder = AppDomain.CurrentDomain.BaseDirectory;
    private readonly string _PATH;

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
      while (true)
      {
        if (UserNameExists(username))
        {
          return GetUser(username);
        }

        Console.WriteLine("\nCould not find user matching given username.\nTry again!");
        username = StringValidationHelper.GetString("Enter username: ");
      }
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
        UserBase user = _users.Find(user => user.Username == username);
        return user;
      }
      catch (Exception ex)
      {
        ExceptionHelper.ExceptionDetails(ex);
        return default;
      }
    }

    private bool UserNameExists(string username)
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
        var settings = new JsonSerializerSettings
        {
          Converters = { new UserBaseConverter() },
          ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
          NullValueHandling = NullValueHandling.Include
        };

        using (StreamReader sr = new StreamReader(_PATH))
        {
          var jsonUsers = sr.ReadToEnd();
          _users = JsonConvert.DeserializeObject<List<UserBase>>(jsonUsers, settings);
        }
      }
      catch (Exception ex)
      {
        ExceptionHelper.ExceptionDetails(ex);
        _users = new List<UserBase>();

      }
    }
    private void SaveData()
    {
      try
      {
        var settings = new JsonSerializerSettings
        {
          Converters = { new UserBaseConverter() },
          ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
          NullValueHandling = NullValueHandling.Include
        };

        var jsonUsers = JsonConvert.SerializeObject(_users, settings);

        File.WriteAllText(_PATH, jsonUsers);
      }
      catch (Exception ex)
      {
        ExceptionHelper.ExceptionDetails(ex);
      }
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
        new UserCustomer("Dabba",  "Svensson", "userDabba"  , "password", "395315678456", new DateTime(2015, 1, 1  ), UserType.Customer),
        new UserCustomer("Ebba",   "Svensson", "userEbba"   , "password", "345745678456", new DateTime(2016, 3, 30 ), UserType.Customer),
        new UserCustomer("Fabba",  "Svensson", "userFabba"  , "password", "002461255321", new DateTime(2022, 4, 12 ), UserType.Customer),
        new UserCustomer("Gabba",  "Svensson", "userGabba"  , "password", "886641486516", new DateTime(2015, 9, 22 ), UserType.Customer),
        new UserCustomer("Habba",  "Svensson", "userHabba"  , "password", "888448484316", new DateTime(2018, 7, 26 ), UserType.Customer)
      };

      return createdUserList;
    }

  }
}