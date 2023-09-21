using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.Account.BankAccounts;
using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Admin;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.App.ConsoleHandling;
using GroupProject.BankDatabase;
using GroupProject.BankDatabase.EventLogs;
using GroupProject.BankDatabase.EventLogs.Events;

namespace GroupProject.App.LogicHandling
{
  internal class LogicHandler
  {
    private UserChoice _choice { get; set; }
    private UserChoice _previousChoice { get; set; }
    private UserStatuses _status { get; set; }
    private UserType _userType { get; set; }
    private List<AccountBase> _userAccounts { get; set; }
    private UserBase? _user { get; set; }
    private Logger _log { get; set; }
    private Database _DB { get; set; }
    private TransactionScheduler _TS;


    public LogicHandler(Database DB, Logger log, int transactionUpdateIntervall = 15)
    {
      _DB = DB;
      _log = log;
      _TS = new TransactionScheduler(transactionUpdateIntervall, _log, _DB);
      _TS.TransactionsProcessed += OnTransactionsProcessed;
      _choice = UserChoice.NoChoiceReceived;
      _previousChoice = UserChoice.NoChoiceReceived;
      _status = UserStatuses.UserDoesNotExist;
      _userType = UserType.NoUser;
      _userAccounts = new List<AccountBase>();
    }
    private void OnTransactionsProcessed(object sender, EventArgs e)
    {
      _userAccounts = _DB.LoadUserAccounts(_user.AccountIds);
    }
    public UserChoice GetUserChoice()
    {
      _choice = UserChoice.WelcomeScreen;
      ConnectionLog connectionLog;
      AccountCreationLog creationLog;
      UserCustomer userCustomer;
      UserAdmin userAdmin;
      bool haveAccounts;
      bool createNewAccount;
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
              _user = _DB.GetUser(username);

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
            _choice = ConsoleIO.CustomerMenu(_user);
            if (_userAccounts.Count == 0)
            {
              _userAccounts = _DB.LoadUserAccounts(_user.AccountIds);
            }
            break;

          case UserChoice.AdminMenu:
            _choice = ConsoleIO.AdminMenu(_user);
            break;

          case UserChoice.CreateBankAccount:
            _previousChoice = UserChoice.CustomerMenu;

            _choice = ConsoleIO.CustomerCreateAccount(_user);
            break;

          case UserChoice.CreateCheckingsAccount:

            if (_user != null)
            {
              CheckingsAccount newChecking = AccountManager.CreateCheckingsAccount(_user);
              _DB.AddNewAccountToUser(_user, newChecking);
              _DB.SaveData();
              _userAccounts.Add(newChecking);
              _choice = _previousChoice;
              break;
            }
            _choice = ConsoleIO.CustomerMenu(_user);
            break;

          case UserChoice.CreateSavingsAccount:
            _previousChoice = UserChoice.CustomerMenu;

            if (_user != null)
            {
              SavingsAccount newSaving = AccountManager.CreateSavingsAccount(_user);
              _DB.AddNewAccountToUser(_user, newSaving);
              _DB.SaveData();
              _userAccounts.Add(newSaving);
              _choice = _previousChoice;
              break;
            }
            _choice = UserChoice.Back;
            break;


          case UserChoice.CreateUserAccount:
            _previousChoice = UserChoice.WelcomeScreen;

            if (_userType != UserType.Admin)
            {
              _choice = UserChoice.CreateCustomerAccount;
              break;
            }
            _choice = ConsoleIO.WhatKindOfAccountMenu(_user);

            break;

          case UserChoice.CreateCustomerAccount:
            _previousChoice = UserChoice.CustomerMenu;
            if (_userType != UserType.Admin)
            {
              userCustomer = AccountManager.CreateUserCustomerAccount(_DB);
              _user = userCustomer;
              creationLog = new AccountCreationLog(_user.Username, "Created a customer account");
              connectionLog = new ConnectionLog(_user.Username, "logged in to the bank application.");
              _log.Log(creationLog);
              _user.AddToLog(creationLog);
              _log.Log(connectionLog);
              _user.AddToLog(connectionLog);
              _DB.AddNewUserToDatabase(userCustomer);
              _choice = UserChoice.Back;
              break;
            }

            userCustomer = AccountManager.CreateUserCustomerAccount(_DB);
            creationLog = new AccountCreationLog(userCustomer.Username, "Created a customer account");
            _log.Log(creationLog);
            userCustomer.AddToLog(creationLog);
            break;


          case UserChoice.CreateAdminAccount:
            _previousChoice = UserChoice.AdminMenu;
            if (_userType == UserType.Admin)
            {
              userAdmin = AccountManager.CreateUserAdminAccount(_DB);
              creationLog = new AccountCreationLog(userAdmin.Username, "Created a customer account");
              _log.Log(creationLog);
              userAdmin.AddToLog(creationLog);
              _DB.AddNewUserToDatabase(userAdmin);
              _choice = UserChoice.Back;
            }
            break;

          case UserChoice.ListAllAccounts:
            _previousChoice = UserChoice.CustomerMenu;

            haveAccounts = _user.CheckIfUserHaveAnyAccounts();

            if (haveAccounts)
            {
              _choice = ConsoleIO.CustomerAccountList(_user, _userAccounts);

              break;
            }
            createNewAccount = ConsoleIO.AskUser("You don't have any accounts yet.", "Would you like to add one? ");
            if (createNewAccount)
            {
              _choice = ConsoleIO.CustomerCreateAccount(_user);
              break;
            }
            _choice = _previousChoice;
            break;


          case UserChoice.UpdateCurrencyExchange:// NOT IMPLEMENTED
            _previousChoice = UserChoice.AdminMenu;

            if (_userType == UserType.Admin)
            {
              _choice = ConsoleIO.AdminCurrencyExchangeMenu(_user);
              break;
            }

            _choice = UserChoice.Back;
            break;

          case UserChoice.MakeDeposit:// NOT FULLY IMPLEMENTED
            _previousChoice = UserChoice.CustomerMenu;

            haveAccounts = _user.CheckIfUserHaveAnyAccounts();

            if (haveAccounts)
            {
              //List<AccountBase> userAccounts = _DB.LoadUserAccounts(_user.AccountIds);
              var depositResponse = _user.MakeDeposit(_userAccounts);
              _choice = depositResponse.Choice;
              _TS.QueueTransaction(depositResponse.Transaction);
              _log.Log(depositResponse.Log);
              break;
            }
            createNewAccount = ConsoleIO.AskUser("You don't have any accounts yet.", "Would you like to add one? ");
            if (createNewAccount)
            {
              _previousChoice = UserChoice.MakeDeposit;
              _choice = ConsoleIO.CustomerCreateAccount(_user);
              break;
            }
            _choice = _previousChoice;
            break;

          case UserChoice.MakeWithdrawal:
            _previousChoice = UserChoice.CustomerMenu;

            haveAccounts = _user.CheckIfUserHaveAnyAccounts();

            if (haveAccounts)
            {
              // List<AccountBase> userAccounts = _DB.LoadUserAccounts(_user.AccountIds);
              var withdrawalResponse = _user.MakeWithdrawal(_userAccounts);
              _choice = withdrawalResponse.Choice;
              _TS.QueueTransaction(withdrawalResponse.Transaction);
              _log.Log(withdrawalResponse.Log);

              break;
            }
            createNewAccount = ConsoleIO.AskUser("You don't have any accounts yet.", "Would you like to add one? ");
            if (createNewAccount)
            {
              _previousChoice = UserChoice.MakeWithdrawal;
              _choice = ConsoleIO.CustomerCreateAccount(_user);
              break;
            }
            _choice = _previousChoice;
            break;


          case UserChoice.MakeTransfer:
            _previousChoice = UserChoice.CustomerMenu;

            haveAccounts = _user.CheckIfUserHaveAnyAccounts();

            if (haveAccounts)
            {
              string targetAccountNumber = ConsoleIO.TransferTargetAccount(_user);
              AccountBase targetAccount = _DB.FindAccountByAccountNumber(targetAccountNumber);
              UserBase targetUser = _DB.FindUserByAccount(targetAccount);
              if (targetUser != null)
              {
                var transferResponse = _user.MakeTransfer(_userAccounts, targetAccount, targetUser);
                _choice = transferResponse.Choice;
                _TS.QueueTransaction(transferResponse.Transaction);
                _log.Log(transferResponse.Log);
                break;
              }
              ConsoleIO.InformUser("Account number does not exist!");
              _choice = UserChoice.CustomerMenu;
              break;
            }
            createNewAccount = ConsoleIO.AskUser("You don't have any accounts yet.", "Would you like to add one? ");
            if (createNewAccount)
            {
              _previousChoice = UserChoice.MakeTransfer;
              _choice = ConsoleIO.CustomerCreateAccount(_user);
              break;
            }
            _choice = _previousChoice;
            break;

          case UserChoice.LoanMoney:// NOT IMPLEMENTED
            _previousChoice = UserChoice.CustomerMenu;

            haveAccounts = _user.CheckIfUserHaveAnyAccounts();

            if (haveAccounts)
            {
              var loanResponse = _user.LoanMoney(_userAccounts);
              _choice = loanResponse.Choice;
              _TS.QueueTransaction(loanResponse.Transaction);
              _log.Log(loanResponse.Log);
              break;
            }
            createNewAccount = ConsoleIO.AskUser("You don't have any accounts yet.", "Would you like to add one? ");
            if (createNewAccount)
            {
              _previousChoice = UserChoice.LoanMoney;
              _choice = ConsoleIO.CustomerCreateAccount(_user);
              break;
            }
            _choice = _previousChoice;
            break;


          case UserChoice.ShowLog:
            _previousChoice = UserChoice.CustomerMenu;
            List<EventLog> userLogs = _user.UserLog.GetUserLogs(_user.Username);
            _choice = ConsoleIO.CustomerLog(_user, userLogs);
            break;

          case UserChoice.Back:
            _choice = _previousChoice;
            break;

          case UserChoice.Logout:
            _previousChoice = UserChoice.WelcomeScreen;

            connectionLog = new(_user.Username, "logged out from the bank application.");
            _log.Log(connectionLog);
            _user.AddToLog(connectionLog);

            _choice = ConsoleIO.WelcomeMenu();
            break;

          case UserChoice.Exit:
            _previousChoice = UserChoice.WelcomeScreen;

            connectionLog = new("User", "Closed the bank application.");
            _log.Log(connectionLog);
            _log.Dispose();

            return UserChoice.Exit;

          default:
            Console.WriteLine($"Choice: {_choice} - Previous Choice: {_previousChoice}");
            Console.WriteLine("UNKOWN ERROR! DEFAULT IN SWITCH!");
            Thread.Sleep(5000);
            _choice = UserChoice.Unknown;
            break;
        }
      }
    }
    private UserChoice GetUserStatus(UserStatuses status)
    {
      ConnectionLog connectionLog;

      switch (status)
      {
        case UserStatuses.Success:
          connectionLog = new ConnectionLog(_user.Username, "logged in to the bank application.");
          _log.Log(connectionLog);
          _user.AddToLog(connectionLog);
          _userAccounts = _DB.LoadUserAccounts(_user.AccountIds);

          if (_userType == UserType.Admin)
          {
            return ConsoleIO.AdminMenu(_user);
          }
          else
          {
            return ConsoleIO.CustomerMenu(_user);
          }

        case UserStatuses.FailedLogin:
          if (_user != null)
          {
            connectionLog = new ConnectionLog(_user.Username, $"failed login attempt. {_user.RemainingAttempts} attempts remaining.");
            _log.Log(connectionLog);
            _user.AddToLog(connectionLog);
            return ConsoleIO.WrongLogin($"\nFailed to login! {_user.RemainingAttempts} attempts remaining.");
          }
          connectionLog = new ConnectionLog("Unknown User", "failed login attempt.");
          _log.Log(connectionLog);
          return _choice = ConsoleIO.WrongLogin($"Failed to login, username not found.");

        case UserStatuses.Locked:
          connectionLog = new ConnectionLog(_user.Username, "3x failed login attempts, account is locked.");
          _log.Log(connectionLog);
          _user.AddToLog(connectionLog);
          ConsoleIO.WriteLockedMenu();
          return UserChoice.WelcomeScreen;

        default:
          return UserChoice.WelcomeScreen;
      }
    }
  }
}