using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.Account.BankAccounts;
using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Admin;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.App.ConsoleHandling;
using GroupProject.App.EventLogs;
using GroupProject.BankDatabase;
using ValidationUtility;

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
      _choice = ConsoleIO.WelcomeMenu();

      while (true)
      {

        switch (_choice)
        {
          case UserChoice.Login:

            string username = StringValidationHelper.GetString("Enter username: ");
            string password = PasswordValidationHelper.PasswordValidation("Enter password: ", 2, 113, false, false, false);

            //if (_DB.UserNameExists(username))
            //{
            _user = _DB.AttemptUserLogin(username, password);

            //}
            //else
            //{
            //  _choice = UserChoice.Login;
            //  break;
            //}
            // ATTEMPT TO LOGIN
            // ACCOUNT NEED TO EXIST TO CONTINUE
            if (_user != null)
            {
              _userType = _user.UserType;
              _status = _user.Login(username, password);
              _previousChoice = _choice;
              _choice = GetUserStatus(_status);
              break;
            }
            _status = UserStatuses.FailedLogin;
            _previousChoice = _choice;
            _choice = GetUserStatus(_status);
            break;

          case UserChoice.CreateCheckingsAccount:
            if (_user != null)
            {
              CheckingsAccount checkingsAccount = new(AccountStatuses.Active, AccountTypes.Checking);
              _DB.AddNewAccountToUser(_user, checkingsAccount);
            }
            _previousChoice = _choice;
            _choice = UserChoice.Back;
            break;

          case UserChoice.CreateSavingsAccount:
            if (_user != null)
            {
              SavingsAccount savingsAccount = new(AccountStatuses.Active, AccountTypes.Saving);
              _DB.AddNewAccountToUser(_user, savingsAccount);
              _previousChoice = _choice;
              _choice = UserChoice.Back;
            }
            _previousChoice = _choice;
            _choice = UserChoice.Back;
            break;


          case UserChoice.CreateUserAccount:
            if (_userType != UserTypes.Admin)
            {
              _previousChoice = _choice;
              _choice = ConsoleIO.CustomerCreateUserAccount();
              break;
            }
            _previousChoice = _choice;
            _choice = ConsoleIO.AdminCreateUserAccount();
            break;

          case UserChoice.CreateCustomerAccount:
            UserCustomer userCustomer = AccountManager.CreateUserCustomerAccount(UserTypes.Customer);
            _DB.AddNewUserToDatabase(userCustomer);
            _previousChoice = _choice;
            _choice = UserChoice.Back;
            break;

          case UserChoice.CreateAdminAccount:
            if (_user != null && _userType == UserTypes.Admin)
            {
              UserAdmin userAdmin = AccountManager.CreateUserAdminAccount(UserTypes.Admin);
              _DB.AddNewUserToDatabase(userAdmin);
              _previousChoice = _choice;
              _choice = UserChoice.Back;
            }
            break;

          case UserChoice.ListAllAccounts:
            if (_userType == UserTypes.Customer)
            {
              bool createAccount = _user.CheckIfUserHaveAnyAccounts();
              if (!createAccount)
              {
                _previousChoice = _choice;
                _choice = ConsoleIO.CustomerAccountList(_user.AccountIds, _user);

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
            if (_userType == UserTypes.Admin)
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

        case UserStatuses.Locked:
          _log.LogWarning($"User failed to login in at {DateTime.Now} and are now locked for 15 minutes");
          ConsoleIO.WriteLockedMenu();
          return ConsoleIO.WelcomeMenu();

        default:
          return ConsoleIO.WelcomeMenu();
      }
    }
  }
}