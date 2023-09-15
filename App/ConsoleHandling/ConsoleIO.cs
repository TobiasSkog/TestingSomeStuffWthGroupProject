using GroupProject.App.BankManagement.Account.BankAccounts;
using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Admin;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.BankDatabase;
using Spectre.Console;
using ValidationUtility;

namespace GroupProject.App.ConsoleHandling
{
    public static class ConsoleIO
    {
        public static void StartUp()
        {
            AnsiConsole.Clear();

            Console.BackgroundColor = ConsoleColor.Black;
            AnsiConsole.Cursor.Hide();
            AnsiConsole.Write(BankLoggo.Loggo.LeftJustified());
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Text("\n    Connecting to the Edugrade Online Banking Service...", new Style(Color.Purple, Color.Black, Decoration.Bold)));

            AnsiConsole.Cursor.SetPosition(1, 23);
            AnsiConsole.Write(new Markup("[rgb(128,54,176)]║                                                          ║[/]"));
            for (int i = 0; i < 59; i++)
            {
                for (int j = 0; j < i; j++)
                {

                    Markup pb = new($"[rgb(0,{((i % 486) + (j % 1900)) % 280},{((i % 1300) + (j % 9900)) % 6500})]█[/]");
                    AnsiConsole.Write(pb);
                }
                AnsiConsole.Cursor.SetPosition(2, 23);
                Thread.Sleep(50);
            }
            AnsiConsole.Cursor.SetPosition(1, 26);
            AnsiConsole.Cursor.Show();
            Thread.Sleep(50);
        }
        public static UserChoice WriteWelcomeMenu()
        {
            AnsiConsole.Clear();

            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold purple]What would you like to do?[/]")
                .PageSize(3)
                .HighlightStyle(highlightStyle: Color.Purple3)
                .AddChoices(new[] {
                    "[lightsteelblue]Login[/]",
                    "[lightsteelblue]Create User Account[/]",
                    "[deeppink4_2]Exit[/]",
            }));

            string userChoice = StringValidationHelper.GetCleanSpectreConsoleString(choice);
            UserChoice enumChoice = EnumValidationHelper.GetSpecificEnumValue<UserChoice>(userChoice.Trim());
            return enumChoice;
        }
        public static UserChoice WriteCreateUserAccount()
        {
            AnsiConsole.Clear();

            var choice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[mediumpurple2]What would you like to do?[/]")
                            .PageSize(9)
                            .HighlightStyle("gold3_1")
                            .AddChoices(new[]
                            {
                    "[lightsteelblue]Create Customer Account[/]",
                    "[lightsteelblue]Create Admin Account[/]",

                    "[deeppink4_2]Back[/]"
                            }));

            string userChoice = StringValidationHelper.GetCleanSpectreConsoleString(choice);
            UserChoice enumChoice = EnumValidationHelper.GetSpecificEnumValue<UserChoice>(userChoice.Trim());
            return enumChoice;
        }
        public static UserChoice WriteCustomerMenu()
        {
            AnsiConsole.Clear();

            var choice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[mediumpurple2]What would you like to do?[/]")
                            .PageSize(9)
                            .HighlightStyle("gold3_1")
                            .AddChoices(new[]
                            {
                    "[lightsteelblue]Make Deposit[/]",
                    "[lightsteelblue]Make Withdrawal[/]",
                    "[lightsteelblue]List All Accounts[/]",
                    "[lightsteelblue]Show Log[/]",
                    "[lightsteelblue]Create Account[/]",
                    "[lightsteelblue]Loan Money[/]",

                    "[deeppink4_2]Logout[/]"
                            }));



            string userChoice = StringValidationHelper.GetCleanSpectreConsoleString(choice);
            UserChoice enumChoice = EnumValidationHelper.GetSpecificEnumValue<UserChoice>(userChoice.Trim());
            return enumChoice;

        }

        public static UserChoice WriteCustomerCreateAccount()
        {
            AnsiConsole.Clear();

            var choice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[mediumpurple2]What would you like to do?[/]")
                            .PageSize(3)
                            .HighlightStyle("gold3_1")
                            .AddChoices(new[]
                            {
                                "[lightsteelblue]Create Checkings Account[/]",
                                "[lightsteelblue]Create Savings Account[/]",
                                "[lightsteelblue]Back[/]",
                            }));

            string userChoice = StringValidationHelper.GetCleanSpectreConsoleString(choice);
            UserChoice enumChoice = EnumValidationHelper.GetSpecificEnumValue<UserChoice>(userChoice.Trim());
            return enumChoice;
        }
        internal static UserChoice WriteCustomerCreateSavingsAccount()
        {
            AnsiConsole.Clear();


            return UserChoice.Back;
        }
        public static UserChoice WriteCustomerCreateCheckingsAccount()
        {
            AnsiConsole.Clear();


            return UserChoice.Back;
        }
        public static UserChoice WriteAdminMenu()
        {
            AnsiConsole.Clear();

            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[mediumpurple2]What would you like to do?[/]")
                .PageSize(3)
                .HighlightStyle("gold3_1")
                .AddChoices(new[]
                {
                    "[lightsteelblue]Create User Account[/]",
                    "[lightsteelblue]Update Currency Exchange[/]",
                    "[deeppink4_2]Logout[/]"
                }));

            string userChoice = StringValidationHelper.GetCleanSpectreConsoleString(choice);
            UserChoice enumChoice = EnumValidationHelper.GetSpecificEnumValue<UserChoice>(userChoice.Trim());
            return enumChoice;
        }
        public static UserChoice WriteAdminAddAccountMenu()
        {
            AnsiConsole.Clear();

            string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[mediumpurple2]What would you like to do?[/]")
                .PageSize(3)
                .HighlightStyle("gold3_1")
                .AddChoices(new[]
                {
                    "[lightsteelblue]Customer Account[/]",
                    "[lightsteelblue]Administrator Account[/]",
                    "[lightsteelblue]Back[/]"
                }));
            string userChoice = StringValidationHelper.GetCleanSpectreConsoleString(choice);
            UserChoice enumChoice = EnumValidationHelper.GetSpecificEnumValue<UserChoice>(userChoice.Trim());
            return enumChoice;
        }
        public static void WriteLockedMenu()
        {
            AnsiConsole.Clear();

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

                Console.WriteLine(remainingMinutes > 0 ? $"Remaining time {Math.Floor(remainingMinutes)}minutes {remainingSeconds % 60:#}seconds" : $"Remaining time {remainingSeconds}seconds");

                Thread.Sleep(1000);
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
        }


    }
}





