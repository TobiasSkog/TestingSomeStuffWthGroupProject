using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.Account.BankAccounts;
using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Admin;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.App.EventLogs;
using GroupProject.App.LogicHandling;
using GroupProject.BankDatabase;

namespace GroupProject.App
{
  public static class App
  {
    // TRANSACTIONSCHEDULER CHANGE TIMESPAN.FROMSECONDS TO MINUTES!!!!!!!!
    public static void Run()
    {



      /*    Aldor login:
       * Username: aldorAdmin
       * Password: password
       * Username: userAldor
       * Password: password
       */

      /*    Reidar login:
       * Username: adminReidar
       * Password: password
       * Username: userReidar
       * Password: password
      */

      Database DB = new();
      Logger log = new();
      LogicHandler LH = new(DB, log);



      UserChoice userChoice;
      do
      {
        userChoice = LH.GetUserChoice();

      } while (userChoice != UserChoice.Exit);
    }
  }
}
