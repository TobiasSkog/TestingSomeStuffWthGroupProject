using GroupProject.App.BankManagement.Account.BankAccounts;
using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Admin;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.App.ConsoleHandling;
using GroupProject.App.EventLogs;
using GroupProject.BankDatabase;

namespace GroupProject.App.LogicHandling
{
  internal class LogicHandler
  {
    private UserChoice _choice { get; set; }
    private UserChoice _previousChoice { get; set; }
    private UserStatuses _status { get; set; }
    private UserTypes _userType { get; set; }
    private UserBase? _user { get; set; }
    private Logger _log { get; set; }
    private Database _DB { get; set; }
    public LogicHandler(Database DB, Logger log)
    {
      _DB = DB;
      _log = log;
      _choice = UserChoice.NoChoiceReceived;
      _previousChoice = UserChoice.NoChoiceReceived;
      _status = UserStatuses.UserDoesNotExist;
      _userType = UserTypes.NoUser;
    }
    public UserChoice GetUserChoice()
    {
      _choice = UserChoice.WelcomeScreen;

      while (true)
      {

        switch (_choice)
        {
          case UserChoice.WelcomeScreen:
            _choice = ConsoleIO.WelcomeMenu();
            break;

          case UserChoice.Login:
            _previousChoice = UserChoice.WelcomeScreen;
            string username = ConsoleIO.Username("Enter username");
            string password = ConsoleIO.Password("Enter password");

            if (_DB.UserNameExists(username))
            {
              _user = _DB.AttemptUserLogin(username, password);

              if (_user != null)
              {
                _userType = _user.UserType;
                _status = _user.Login(username, password);
                if (_user.RemainingAttempts >= 0)
                {
                  _choice = GetUserStatus(_status);
                }
              }

              break;
            }

            _status = UserStatuses.FailedLogin;
            _choice = GetUserStatus(_status);
            break;

          case UserChoice.CustomerMenu:
            _choice = ConsoleIO.CustomerMenu();
            break;

          case UserChoice.AdminMenu:
            _choice = ConsoleIO.AdminMenu();
            break;

          case UserChoice.CreateBankAccount:
            _previousChoice = UserChoice.CustomerMenu;

            _choice = ConsoleIO.CustomerCreateAccount();
            break;

          case UserChoice.CreateCheckingsAccount:
            _previousChoice = UserChoice.CustomerMenu;

            if (_user != null)
            {
              CheckingsAccount newChecking = AccountManager.CreateCheckingsAccount(_user.UserType);
              _DB.AddNewAccountToUser(_user, newChecking);
              _choice = ConsoleIO.CustomerMenu();
              break;
            }
            _choice = ConsoleIO.CustomerMenu();
            break;

          case UserChoice.CreateSavingsAccount:
            _previousChoice = UserChoice.CustomerMenu;

            if (_user != null)
            {
              SavingsAccount newSaving = AccountManager.CreateSavingsAccount(_user.UserType);
              _user.AddAccount(newSaving);
              _choice = ConsoleIO.CustomerMenu();
              break;
            }
            _choice = UserChoice.Back;
            break;


          case UserChoice.CreateUserAccount:
            _previousChoice = UserChoice.WelcomeScreen;

            if (_userType != UserTypes.Admin)
            {
              _choice = ConsoleIO.CustomerCreateUserAccount();
              break;
            }
            _choice = ConsoleIO.AdminCreateUserAccount();
            break;

          case UserChoice.CreateCustomerAccount:
            _previousChoice = UserChoice.CustomerMenu;

            UserCustomer userCustomer = AccountManager.CreateUserCustomerAccount(UserTypes.Customer, _DB);
            _DB.AddNewUserToDatabase(userCustomer);
            _choice = UserChoice.Back;
            break;

          case UserChoice.CreateAdminAccount:
            _previousChoice = UserChoice.AdminMenu;
            if (_user != null && _userType == UserTypes.Admin)
            {
              UserAdmin userAdmin = AccountManager.CreateUserAdminAccount(UserTypes.Admin, _DB);
              _DB.AddNewUserToDatabase(userAdmin);
              _choice = UserChoice.Back;
            }
            break;

          case UserChoice.ListAllAccounts:
            _previousChoice = UserChoice.CustomerMenu;

            if (_userType == UserTypes.Customer)
            {
              bool haveAccounts = _user.CheckIfUserHaveAnyAccounts();

              if (haveAccounts)
              {
                _choice = ConsoleIO.CustomerAccountList(_user.AccountIds, _user);
                break;
              }

              bool createNewAccount = ConsoleIO.AskUser("You don't have any accounts yet.", "Would you like to add one? ");

              if (createNewAccount)
              {
                _choice = ConsoleIO.CustomerCreateAccount();
                break;
              }
            }

            _choice = _previousChoice;
            break;


          // I'M HERE!!!!
          case UserChoice.UpdateCurrencyExchange:
            _previousChoice = UserChoice.AdminMenu;

            if (_userType == UserTypes.Admin)
            {
              _choice = ConsoleIO.AdminCurrencyExchangeMenu();
              break;
            }

            _choice = UserChoice.Back;
            break;

          case UserChoice.MakeDeposit:
            _previousChoice = UserChoice.CustomerMenu;
            Console.WriteLine("Not implemented yet :)");

            _choice = _user.MakeDeposit();
            break;

          case UserChoice.MakeWithdrawal:
            _previousChoice = UserChoice.CustomerMenu;
            Console.WriteLine("Not implemented yet :)");

            _choice = _user.MakeWithdrawal();
            break;

          case UserChoice.LoanMoney:
            _previousChoice = UserChoice.CustomerMenu;
            Console.WriteLine("Not implemented yet :)");

            _choice = _user.LoanMoney();
            break;

          case UserChoice.ShowLog:
            _previousChoice = UserChoice.CustomerMenu;
            Console.WriteLine("Not implemented yet :)");

            _choice = _user.ShowLog();
            break;

          case UserChoice.Back:
            _choice = _previousChoice;
            break;

          case UserChoice.Logout:
            _previousChoice = UserChoice.WelcomeScreen;
            Console.WriteLine("Not implemented yet :)");

            _choice = ConsoleIO.WelcomeMenu();
            break;

          case UserChoice.Exit:
            _previousChoice = UserChoice.WelcomeScreen;
            return UserChoice.Exit;

          default:
            Console.WriteLine($"Chcoice: {_choice} - Previous CHoice: {_previousChoice}");
            Console.WriteLine("UNKOWN ERROR! DEFAULT IN SWITCH!");
            Thread.Sleep(5000);
            _choice = UserChoice.Unknown;
            break;
        }
      }
    }
    private UserChoice GetUserStatus(UserStatuses status)
    {
      switch (status)
      {
        case UserStatuses.Success:
          _log.LogSuccessfulEvent("USERLOGIN", $"{_user.Username} logged in successfuly.");
          _user.AddToLog($"User logged in at {DateTime.Now}.");
          if (_userType == UserTypes.Admin)
          {
            return ConsoleIO.AdminMenu();
          }
          else
          {
            return ConsoleIO.CustomerMenu();
          }

        case UserStatuses.FailedLogin:
          if (_user != null)
          {
            _log.LogFailedEvent("USERLOGIN", $"{_user.Username} attempted login, {_user.RemainingAttempts} attempts remaining.");
            _user.AddToLog($"User failed to login in at {DateTime.Now}, {_user.RemainingAttempts} attempts remaining.");
            return ConsoleIO.WrongLogin($"\nFailed to login! {_user.RemainingAttempts} attempts remaining.");
          }

          _log.LogFailedEvent("USERLOGIN", $"UNKOWN-USER attempted login");
          return _choice = ConsoleIO.WrongLogin($"Failed to login, username not found.");

        case UserStatuses.Locked:
          _log.LogWarning($"User failed to login in at {DateTime.Now} and are now locked for 15 minutes");
          ConsoleIO.WriteLockedMenu();
          return UserChoice.WelcomeScreen;

        default:
          return UserChoice.WelcomeScreen;
      }
    }
  }
}