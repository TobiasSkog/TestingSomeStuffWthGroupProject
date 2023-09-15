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
                                break;
                            case UserStatus.Locked:
                                break;
                            default:
                                break;
                        }
                        break;
                    case UserChoice.CreateAccount:
                        //choice = CreateAccount();
                        break;
                    case UserChoice.CreateSavingsAccount:
                        //choice = CreateSavingsAccount();
                        break;
                    case UserChoice.CreateCheckingsAccount:
                        //choice = CreateCheckingsAccount();
                        break;
                    case UserChoice.CreateUserAccount:
                        //choice = CreateUserAccount();
                        break;
                    case UserChoice.CreateAdminAccount:
                        //choice = CreateAdminAccount();
                        break;
                    case UserChoice.CreateCustomerAccount:
                        //choice = CreateCustomerAccount();
                        break;
                    case UserChoice.ListAllAccounts:
                        //choice = ListAllAccounts();
                        break;
                    case UserChoice.UpdateCurrencyExchange:
                        //choice = UpdateCurrencyExchange();
                        break;
                    case UserChoice.MakeDeposit:
                        //choice = MakeDeposit();
                        break;
                    case UserChoice.MakeWithdrawal:
                        //choice = MakeWithdrawal();
                        break;
                    case UserChoice.LoanMoney:
                        //choice = LoanMoney();
                        break;
                    case UserChoice.ShowLog:
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