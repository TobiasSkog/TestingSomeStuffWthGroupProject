using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.Account.BankAccounts;
using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Admin;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.App.ConsoleHandling;
using GroupProject.App.LogicHandling;
using GroupProject.BankDatabase;
using GroupProject.BankDatabase.EventLogs;
using Spectre.Console;

namespace GroupProject.App
{
  public static class App
  {
    // TRANSACTIONSCHEDULER CHANGE TIMESPAN.FROMSECONDS TO MINUTES!!!!!!!!
    private const int transactionUpdateIntervall = 15;
    public static void Run()
    {
      AnsiConsole.Background = Color.Black;
      Logger logger = new();
      Database DB = new(logger);
      LogicHandler LH = new(DB, logger, transactionUpdateIntervall);

      UserChoice userChoice;
      do
      {
        userChoice = LH.GetUserChoice();

      } while (userChoice != UserChoice.Exit);
    }
  }
}
