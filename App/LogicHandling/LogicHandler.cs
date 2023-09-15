using GroupProject.App.BankManagement.Account.BankAccounts;
using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Admin;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.App.ConsoleHandling;
using GroupProject.BankDatabase;
using ValidationUtility;

namespace GroupProject.App.LogicHandling
{
    internal class LogicHandler
    {
        public static UserChoice GetUserChoice(Database DB)
        {
            UserChoice choice = ConsoleIO.WriteWelcomeMenu();
            UserChoice previousChoice = UserChoice.Back;
            UserStatus status;
            UserBase? user = null;
            UserType type;
            while (true)
            {
                switch (choice)
                {

                    case UserChoice.Login:
                        user = DB.FindUserInDatabase();
                        if (user == null)
                        {
                            Console.WriteLine("User does not exists!");
                            break;
                        }

                        status = user.Login(user.UserName);

                        switch (status)
                        {
                            case UserStatus.Success:
                                type = user.GetUserType();
                                if (type == UserType.Admin)
                                {
                                    choice = ConsoleIO.WriteAdminMenu();

                                }
                                else
                                {
                                    choice = ConsoleIO.WriteCustomerMenu();
                                }

                                break;

                            case UserStatus.FailedLogin:
                                Console.WriteLine($"Failed Login! {user.RemainingAttempts} attempts remaining.");
                                break;

                            case UserStatus.Locked:
                                ConsoleIO.WriteLockedMenu();
                                break;

                            default:
                                break;
                        }
                        break;

                    case UserChoice.CreateAccount:
                        choice = ConsoleIO.WriteCustomerCreateAccount();
                        switch (choice)
                        {
                            case UserChoice.CreateCheckingsAccount:
                                if (user != null)
                                {
                                    CheckingsAccount checkingsAccount = new(user);
                                    user.AddAccount(checkingsAccount);
                                    DB.SaveAccount(checkingsAccount);
                                    choice = UserChoice.Back;
                                }
                                break;

                            case UserChoice.CreateSavingsAccount:
                                if (user != null)
                                {
                                    SavingsAccount savingsAccount = new(user);
                                    user.AddAccount(savingsAccount);
                                    DB.SaveAccount(savingsAccount);
                                    choice = UserChoice.Back;
                                }
                                break;
                        }
                        break;

                    case UserChoice.CreateUserAccount:
                        if (user?.GetUserType() != UserType.Admin)
                        {
                            choice = UserChoice.CreateCustomerAccount;
                            break;
                        }
                        choice = ConsoleIO.WriteCreateUserAccount();
                        break;

                    case UserChoice.CreateCustomerAccount:
                        if (user != null)
                        {
                            UserCustomer userCustomer = AccountManager.CreateUserCustomerAccount(UserType.Customer);
                            DB.SaveUser(userCustomer);
                            choice = UserChoice.Back;
                        }
                        break;

                    case UserChoice.CreateAdminAccount:
                        if (user != null)
                        {
                            UserAdmin userAdmin = AccountManager.CreateUserAdminAccount(UserType.Admin);
                            DB.SaveUser(userAdmin);
                            choice = UserChoice.Back;
                        }
                        break;

                    case UserChoice.ListAllAccounts:
                        if (user.GetUserType() == UserType.Customer)
                        {

                            bool createAccount = user.ListAccounts();
                            if (createAccount)
                            {
                                choice = ConsoleIO.WriteCustomerCreateAccount();
                                break;
                            }
                        }
                        choice = UserChoice.Back;
                        break;

                    // I'M HERE!!!!
                    case UserChoice.UpdateCurrencyExchange:
                        if (user.GetUserType() == UserType.Admin)
                        {
                            UserAdmin userAdmin = (UserAdmin)user;
                            userAdmin.UpdateCurrencyExchange();
                        }
                        choice = UserChoice.Back;
                        break;

                    case UserChoice.MakeDeposit:
                        Console.WriteLine("Not implemented yet :)");
                        choice = UserChoice.Exit;
                        //choice = MakeDeposit();
                        break;

                    case UserChoice.MakeWithdrawal:
                        Console.WriteLine("Not implemented yet :)");
                        choice = UserChoice.Exit;
                        //choice = MakeWithdrawal();
                        break;

                    case UserChoice.LoanMoney:
                        Console.WriteLine("Not implemented yet :)");
                        choice = UserChoice.Exit;
                        //choice = LoanMoney();
                        break;

                    case UserChoice.ShowLog:
                        Console.WriteLine("Not implemented yet :)");
                        choice = UserChoice.Exit;
                        //choice = ShowLog();
                        break;

                    case UserChoice.Back:
                        choice = previousChoice;
                        break;

                    case UserChoice.Logout:
                        choice = ConsoleIO.WriteWelcomeMenu();
                        break;

                    case UserChoice.Exit:
                        return UserChoice.Exit;

                    default:
                        break;
                }
            }
        }
    }
}