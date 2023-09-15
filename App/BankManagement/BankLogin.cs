//using GroupProject.App.BankManagement.User;
//using GroupProject.App.LogicHandling;
//using Spectre.Console;
//using ValidationUtility;

//namespace GroupProject.App.BankManagement
//{
//    public class BankLogin : IAccountManager
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
//                        WriteLockedMenu();
//                        break;
//                    case UserStatus.Success:
//                        Console.WriteLine($"\nWelcome {user.FirstName}.");
//                        WriteUserMenu(user);
//                        return;
//                }
//            }
//        }
//        public static void WriteLockedMenu()
//        {
//            int LOCKEDFOR = 10;
//            DateTime now = DateTime.Now;
//            DateTime lockedDuration = now.AddMinutes(LOCKEDFOR);
//            TimeSpan timeRemaining;
//            Console.WriteLine($"You are locked for {LOCKEDFOR} minutes");

//            for (int i = 0; i < LOCKEDFOR * 60; i++)
//            {
//                now = DateTime.Now;
//                timeRemaining = lockedDuration - now;
//                double remainingMinutes = timeRemaining.TotalMinutes;
//                double remainingSeconds = timeRemaining.TotalSeconds;

//                Console.WriteLine(remainingMinutes > 0 ? $"Remaining time {Math.Floor(remainingMinutes)}minutes {remainingSeconds % 60:#}seconds" : $"Remaining time {remainingSeconds}seconds");

//                Thread.Sleep(1000);
//                Console.SetCursorPosition(0, Console.CursorTop - 1);
//                ConsoleExtras.ClearCurrentConsoleLine();
//            }
//        }
//        public static void WriteUserMenu(UserBase user)
//        {

//            var grid = new Grid();

//            grid.AddColumns(3);

//            grid.AddRow(new Text[]{
//                        new Text($"{user.FirstName}", new Style(Color.Red3_1, Color.Black)).LeftJustified(),
//                        new Text($"{DateTime.Now}", new Style(Color.SpringGreen1, Color.Black)).Centered()
//            });

//            grid.AddRow(
//                new Text("Personkonto").LeftJustified(),
//                new Text("4500SEK").Centered(),
//                new Text("Privatkonto").RightJustified()
//            );
//            grid.AddRow(
//                new Text("Personkonto").LeftJustified(),
//                new Text("150USD").Centered(),
//                new Text("Valutakonto").RightJustified()
//            );
//            grid.AddRow(
//                new Text("Sparkonto").LeftJustified(),
//                new Text("125000SEK").Centered(),
//                new Text("Privatkonto").RightJustified()
//            );

//            AnsiConsole.Write(grid);
//            Console.ReadKey();

//        }
//    }
//}
