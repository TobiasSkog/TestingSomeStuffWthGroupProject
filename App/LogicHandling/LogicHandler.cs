using GroupProject.App.BankManagement.Account.BankAccounts;
using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Admin;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.App.ConsoleHandling;
using GroupProject.App.EventLogs;
using GroupProject.BankDatabase;
using Spectre.Console;
using ValidationUtility;

namespace GroupProject.App.LogicHandling
{
  internal class LogicHandler
  {
    private UserChoice _choice { get; set; }
    private UserChoice _previousChoice { get; set; }
    private UserStatus _status { get; set; }
    private UserType _userType { get; set; }
    private UserBase? _user { get; set; }
    private Logger _log { get; set; }
    private Database _DB { get; set; }
    public LogicHandler(Database DB, Logger log)
    {
      _DB = DB;
      _log = log;
      _choice = UserChoice.NoChoiceReceived;
      _previousChoice = UserChoice.NoChoiceReceived;
      _status = UserStatus.UserDoesNotExist;
      _userType = UserType.NoUser;
    }
    public UserChoice GetUserChoice()
    {
      _choice = ConsoleIO.WelcomeMenu();

      while (true)
      {
        switch (_choice)
        {
          case UserChoice.Login:

            string username = StringValidationHelper.GetString("Enter username: ");
            string password = PasswordValidationHelper.PasswordValidation("Enter password: ", 2, 113, false, false, false);

            _user = _DB.AttemptUserLogin(username, password);
            // ATTEMPT TO LOGIN
            // ACCOUNT NEED TO EXIST TO CONTINUE
            if (_user != null)
            {
              _userType = _user.UserAccountType;
              _status = _user.Login(username, password);
              _previousChoice = _choice;
              _choice = GetUserStatus(_status);
              break;
            }
            _status = UserStatus.FailedLogin;
            _previousChoice = _choice;
            _choice = GetUserStatus(_status);
            break;


          case UserChoice.CreateAccount:
            if (_userType != UserType.Admin)
            {
              _previousChoice = _choice;
              _choice = ConsoleIO.CustomerCreateUserAccount();
              break;
            }
            _previousChoice = _choice;
            _choice = ConsoleIO.AdminCreateUserAccount();
            break;

          case UserChoice.CreateCheckingsAccount:
            if (_user != null)
            {
              CheckingsAccount checkingsAccount = new(_user);
              _DB.AddNewAccountToUser(_user, checkingsAccount);
            }
            _previousChoice = _choice;
            _choice = UserChoice.Back;
            break;

          case UserChoice.CreateSavingsAccount:
            if (_user != null)
            {
              SavingsAccount savingsAccount = new(_user);
              _DB.AddNewAccountToUser(_user, savingsAccount);
              _previousChoice = _choice;
              _choice = UserChoice.Back;
            }
            _previousChoice = _choice;
            _choice = UserChoice.Back;
            break;


          case UserChoice.CreateUserAccount:
            if (_userType != UserType.Admin)
            {
              _previousChoice = _choice;
              _choice = ConsoleIO.CustomerCreateUserAccount();
              break;
            }
            _previousChoice = _choice;
            _choice = ConsoleIO.AdminCreateUserAccount();
            break;

          case UserChoice.CreateCustomerAccount:
            if (_user != null)
            {
              UserCustomer userCustomer = AccountManager.CreateUserCustomerAccount(UserType.Customer);
              _DB.AddNewUserToDatabase(userCustomer);
              _previousChoice = _choice;
              _choice = UserChoice.Back;
              break;
            }
            _previousChoice = _choice;
            _choice = ConsoleIO.WelcomeMenu();
            break;

          case UserChoice.CreateAdminAccount:
            if (_user != null && _userType == UserType.Admin)
            {
              UserAdmin userAdmin = AccountManager.CreateUserAdminAccount(UserType.Admin);
              _DB.AddNewUserToDatabase(userAdmin);
              _previousChoice = _choice;
              _choice = UserChoice.Back;
            }
            break;

          case UserChoice.ListAllAccounts:
            if (_userType == UserType.Customer)
            {
              bool createAccount = _user.CheckIfUserHaveAnyAccounts();
              if (!createAccount)
              {
                _previousChoice = _choice;
                _choice = ConsoleIO.CustomerAccountList(_user.Accounts, _user);

              }
              _previousChoice = _choice;
              _choice = ConsoleIO.CustomerCreateAccount();
              break;
            }
            _previousChoice = _choice;
            _choice = UserChoice.Back;
            break;

          // I'M HERE!!!!
          case UserChoice.UpdateCurrencyExchange:
            if (_userType == UserType.Admin)
            {
              _previousChoice = _choice;
              _choice = ConsoleIO.AdminCurrencyExchangeMenu();
            }
            _previousChoice = _choice;
            _choice = UserChoice.Back;
            break;

          case UserChoice.MakeDeposit:
            Console.WriteLine("Not implemented yet :)");
            _previousChoice = _choice;
            _choice = UserChoice.Exit;
            //choice = MakeDeposit();
            break;

          case UserChoice.MakeWithdrawal:
            Console.WriteLine("Not implemented yet :)");
            _previousChoice = _choice;
            _choice = UserChoice.Exit;
            //choice = MakeWithdrawal();
            break;

          case UserChoice.LoanMoney:
            Console.WriteLine("Not implemented yet :)");
            _previousChoice = _choice;
            _choice = UserChoice.Exit;
            //choice = LoanMoney();
            break;

          case UserChoice.ShowLog:
            Console.WriteLine("Not implemented yet :)");
            _previousChoice = _choice;
            _choice = UserChoice.Exit;
            //choice = ShowLog();
            break;

          case UserChoice.Back:
            _choice = _previousChoice;
            break;

          case UserChoice.Logout:
            _previousChoice = UserChoice.NoChoiceReceived;
            _choice = ConsoleIO.WelcomeMenu();
            break;

          case UserChoice.Exit:
            _previousChoice = UserChoice.NoChoiceReceived;
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
    private UserChoice GetUserStatus(UserStatus status)
    {
      switch (status)
      {
        case UserStatus.Success:
          _log.LogSuccessfulEvent("USERLOGIN", $"{_user.Username} logged in successfuly.");
          _user.AddToLog($"User logged in at {DateTime.Now}.");
          if (_userType == UserType.Admin)
          {
            return ConsoleIO.AdminMenu();
          }
          else
          {
            return ConsoleIO.CustomerMenu();
          }

        case UserStatus.FailedLogin:
          if (_user != null)
          {
            Console.WriteLine($"\nFailed to login! {_user.RemainingAttempts} attempts remaining.");
            _log.LogFailedEvent("USERLOGIN", $"{_user.Username} attempted login, {_user.RemainingAttempts} attempts remaining.");
            _user.AddToLog($"User failed to login in at {DateTime.Now}, {_user.RemainingAttempts} attempts remaining.");
          }
          else
          {
            Console.WriteLine($"\nFailed to login, username not found.");
            _log.LogFailedEvent("USERLOGIN", $"UNKOWN-USER attempted login");
          }

          return UserChoice.Login;

        case UserStatus.Locked:
          _log.LogWarning($"User failed to login in at {DateTime.Now} and are now locked for 15 minutes");
          ConsoleIO.WriteLockedMenu();
          return ConsoleIO.WelcomeMenu();

        default:
          return ConsoleIO.WelcomeMenu();
      }
    }
  }
}