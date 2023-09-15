using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Admin;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.App.ConsoleHandling;
using GroupProject.App.LogicHandling;
using GroupProject.BankDatabase;

namespace GroupProject.App
{
    public static class App
    {
        public static void Run()
        {
            List<UserBase> bankUsers = new(){
                new UserAdmin   ("Tobias", "Skog",      "123",          new DateTime(1991, 10, 28)),
                new UserAdmin   ("Aldor",  "Admin",     "123",          new DateTime(1980, 10, 21)),
                new UserAdmin   ("Reidar", "Admin",     "123",          new DateTime(1980, 10, 21)),
                new UserCustomer("Aldor",  "User",      "123",          new DateTime(1970, 6,  1) ),
                new UserCustomer("Reidar", "User",      "123",          new DateTime(1996, 8,  1) ),
                new UserCustomer("Dabba",  "Svensson",  "395315678456", new DateTime(2015, 1,  1) ),
                new UserCustomer("Ebba",   "Svensson",  "345745678456", new DateTime(2016, 3,  30)),
                new UserCustomer("Fabba",  "Svensson",  "002461255321", new DateTime(2022, 4,  12)),
                new UserCustomer("Gabba",  "Svensson",  "886641486516", new DateTime(2015, 9,  22)),
                new UserCustomer("Habba",  "Svensson",  "888448484316", new DateTime(2018, 7,  26))
            };

            /*     Aldor login:
             * Username: AldorAdmin
             * Password: 123Aldor
             * Username: AldorUser
             * Password: Aldor123
             */

            /*    Reidar login:
             * Username: ReidarAdmin
             * Password: 123Reidar
             * Username: ReidarUser
             * Password: 123Reidar
            */

            Database DB = new Database(bankUsers);
            DB.InitiateDataBase();

            UserChoice userChoice;
            do
            {
                userChoice = LogicHandler.GetUserChoice(DB);

            } while (userChoice != UserChoice.Exit);

            //BankLogin.FindUserName(bankDatabase);
            //TestingConsole.TestDate("Enter Date of Birth: ", 18);
            //TestingConsole.PrintConsole();
        }
    }
}
