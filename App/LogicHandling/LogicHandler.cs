using GroupProject.App.BankManagement.User;
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
            UserChoice previousChoice;
            UserStatus status;
            UserType type;
            while (true)
            {
                previousChoice = choice;
                switch (choice)
                {

                    case UserChoice.Login:

                        UserBase user = DB.FindUserInDatabase();
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
                        Console.WriteLine("Not implemented yet :)");
                        choice = UserChoice.Exit;
                        //choice = CreateAccount();
                        break;
                    case UserChoice.CreateSavingsAccount:
                        Console.WriteLine("Not implemented yet :)");
                        choice = UserChoice.Exit;
                        //choice = CreateSavingsAccount();
                        break;
                    case UserChoice.CreateCheckingsAccount:
                        Console.WriteLine("Not implemented yet :)");
                        choice = UserChoice.Exit;
                        //choice = CreateCheckingsAccount();
                        break;
                    case UserChoice.CreateUserAccount:
                        Console.WriteLine("Not implemented yet :)");
                        choice = UserChoice.Exit;
                        //choice = CreateUserAccount();
                        break;
                    case UserChoice.CreateAdminAccount:
                        Console.WriteLine("Not implemented yet :)");
                        choice = UserChoice.Exit;
                        //choice = CreateAdminAccount();
                        break;
                    case UserChoice.CreateCustomerAccount:
                        Console.WriteLine("Not implemented yet :)");
                        choice = UserChoice.Exit;
                        //choice = CreateCustomerAccount();
                        break;
                    case UserChoice.ListAllAccounts:
                        Console.WriteLine("Not implemented yet :)");
                        choice = UserChoice.Exit;
                        //choice = ListAllAccounts();
                        break;
                    case UserChoice.UpdateCurrencyExchange:
                        Console.WriteLine("Not implemented yet :)");
                        choice = UserChoice.Exit;
                        //choice = UpdateCurrencyExchange();
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