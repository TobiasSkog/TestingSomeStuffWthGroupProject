//using GroupProject.App.BankManagement;
//using GroupProject.App.BankManagement.User;
//using GroupProject.App.ConsoleHandling;
//using ValidationUtility;

//namespace GroupProject.App.LogicHandling
//{
//    public interface IAccountManager
//    {
//        public static void FindUserName(Bank[] bankDatabase)
//        {
//            string userName = StringValidationHelper.GetString("Enter username: ");

//            foreach (UserBase user in bankDatabase)
//            {
//                string realUserName = user.FirstName;
//                if (userName == realUserName)
//                {
//                    AttemptLogin(user, realUserName);
//                }
//            }
//        }
//        public static void AttemptLogin(UserBase user, string userName)
//        {
//            UserStatus attemptingLogin = user.ExistingAccount(userName);
//            if (attemptingLogin == UserStatus.Exists)
//            {
//                ValidateLogin(user, userName);
//            }
//        }
//        public static void ValidateLogin(UserBase user, string userName)
//        {
//            UserStatus userStatus;
//            while (user.RemainingAttempts > 0)
//            {
//                string password = PasswordValidationHelper.PasswordValidation("Enter password: ", 4, 113, false, false, false);
//                userStatus = user.Login(userName, password);
//                switch (userStatus)
//                {
//                    case UserStatus.FailedLogin:
//                        Console.WriteLine($"\nWrong password! {user.RemainingAttempts} attempts left.");
//                        break;
//                    case UserStatus.Locked:
//                        Console.WriteLine($"\nUser account is locked! Contact an administrator.");
//                        ConsoleIO.WriteLockedMenu();
//                        break;
//                    case UserStatus.Success:
//                        Console.WriteLine($"\nWelcome {user.FirstName}.");
//                        //ConsoleIO.WriteCustomerMenu((UserCustomer)user);
//                        return;
//                }
//            }
//        }

//    }
//}
