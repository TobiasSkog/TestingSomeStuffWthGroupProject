using GroupProject.App.Tests;
using GroupProject.Bank.User;
using Spectre.Console;
using System.ComponentModel;
using System.Reflection;
using ValidationUtility;

namespace GroupProject.Bank
{
    public static class BankLogin
    {
        public static void FindUserName(Bank[] bankDatabase)
        {
            string userName = StringValidationHelper.GetString("Enter username: ");

            foreach (UserBase user in bankDatabase)
            {
                string realUserName = user.FirstName + user.LastName;
                if (userName == realUserName)
                {
                    AttemptLogin(user, realUserName);
                }
            }
        }

        public static void AttemptLogin(UserBase user, string userName)
        {
            UserStatus attemptingLogin = user.ExistingAccount(userName);
            if (attemptingLogin == UserStatus.Exists)
            {
                ValidateLogin(user, userName);
            }
        }

        public static void ValidateLogin(UserBase user, string userName)
        {
            UserStatus userStatus;
            while (user.RemainingAttempts > 0)
            {
                string password = PasswordValidationHelper.PasswordValidation("Enter password: ", 4, 113, false, false, false);
                userStatus = user.Login(userName, password);
                switch (userStatus)
                {
                    case UserStatus.FailedLogin:
                        Console.WriteLine($"\nWrong password! {user.RemainingAttempts} attempts left.");
                        break;
                    case UserStatus.Locked:
                        Console.WriteLine($"\nUser account is locked! Contact an administrator.");
                        WriteLockedMenu();
                        break;
                    case UserStatus.Success:
                        Console.WriteLine($"\nWelcome {user.FirstName}.");
                        WriteUserMenu(user);
                        return;
                }
            }
        }
        public static void WriteLockedMenu()
        {
            int LOCKEDFOR = 10;
            DateTime now = DateTime.Now;
            DateTime lockedDuration = now.AddMinutes(LOCKEDFOR);
            TimeSpan timeRemaining;
            Console.WriteLine($"You are locked for {LOCKEDFOR} minutes");

            for (int i = 0; i < LOCKEDFOR * 60; i++)
            {
                now = DateTime.Now;
                timeRemaining = lockedDuration - now;
                double remainingMinutes = timeRemaining.TotalMinutes;
                double remainingSeconds = timeRemaining.TotalSeconds;

                Console.WriteLine(remainingMinutes > 0 ? $"Remaining time {Math.Floor(remainingMinutes)}minutes {(remainingSeconds % 60):#}seconds" : $"Remaining time {remainingSeconds}seconds");

                Thread.Sleep(1000);
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                ClearCurrentConsoleLine();

            }
        }
        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
        public static void WriteUserMenu(UserBase user)
        {

            var grid = new Grid();

            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();

            grid.AddRow(new Text[]{
                        new Text($"{user.FirstName}", new Style(Color.Red3_1, Color.Black)).LeftJustified(),
                        new Text($"{user.LastName}", new Style(Color.Red3, Color.Black)).Centered(),
                        new Text($"{DateTime.Now}", new Style(Color.SpringGreen1, Color.Black)).RightJustified()
            });

            grid.AddRow(
                new Text("Personkonto").LeftJustified(),
                new Text("4500SEK").Centered(),
                new Text("Privatkonto").RightJustified()
            );
            grid.AddRow(
                new Text("Personkonto").LeftJustified(),
                new Text("150USD").Centered(),
                new Text("Valutakonto").RightJustified()
            );
            grid.AddRow(
                new Text("Sparkonto").LeftJustified(),
                new Text("125000SEK").Centered(),
                new Text("Privatkonto").RightJustified()
            );

            AnsiConsole.Write(grid);
            Console.ReadKey();

        }
    }
}
