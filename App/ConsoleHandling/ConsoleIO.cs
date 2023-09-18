using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.User;
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
      //read.Sleep(5000);

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
        Thread.Sleep(5);
      }
      Thread.Sleep(50);
      AnsiConsole.Cursor.SetPosition(1, 1);
      AnsiConsole.Cursor.Show();
      AnsiConsole.Clear();
    }
    public static UserChoice WelcomeMenu(string prompt = "")
    {
      AnsiConsole.Clear();

      if (prompt != "")
      {
        Console.WriteLine(prompt);
      }

      var choice = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
              .Title("[bold purple]What would you like to do?[/]")
              .PageSize(3)
              .HighlightStyle(highlightStyle: Color.Purple3)
              .AddChoices(new[] {
                        "[lightsteelblue]Login[/]",
                        "[lightsteelblue]Create user account[/]",
                        "[deeppink4_2]Exit[/]",
      }));

      string userChoice = StringValidationHelper.GetCleanSpectreConsoleString(choice);
      UserChoice enumChoice = EnumValidationHelper.GetSpecificEnumValue<UserChoice>(userChoice.Trim());
      AnsiConsole.Clear();
      return enumChoice;
    }
    public static UserChoice AdminMenu()
    {
      AnsiConsole.Clear();
      var choice = AnsiConsole.Prompt(
      new SelectionPrompt<string>()
          .Title("[mediumpurple2]What would you like to do?[/]")
          .PageSize(3)
          .HighlightStyle("gold3_1")
          .AddChoices(new[]
          {
                    "[lightsteelblue]Create User account[/]",
                    "[lightsteelblue]Update Currency exchange[/]",
                    "[deeppink4_2]Logout[/]"
          }));

      string userChoice = StringValidationHelper.GetCleanSpectreConsoleString(choice);
      UserChoice enumChoice = EnumValidationHelper.GetSpecificEnumValue<UserChoice>(userChoice.Trim());
      AnsiConsole.Clear();
      return enumChoice;
    }
    public static UserChoice AdminAddAccountMenu()
    {
      string choice = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
          .Title("[mediumpurple2]What kind of account would you like to create?[/]")
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
      AnsiConsole.Clear();
      return enumChoice;
    }
    public static UserChoice AdminCurrencyExchangeMenu()
    {
      string choice = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
              .Title("[mediumpurple2]What would you like to do?[/]")
              .PageSize(3)
              .HighlightStyle("gold3_1")
              .AddChoices(new[]
              {
                        "[lightsteelblue]Update to the latest currency exchange rates[/]",

                        "[lightsteelblue]Back[/]"
      }));

      string userChoice = StringValidationHelper.GetCleanSpectreConsoleString(choice);
      UserChoice enumChoice = EnumValidationHelper.GetSpecificEnumValue<UserChoice>(userChoice.Trim());
      AnsiConsole.Clear();
      return enumChoice;
    }
    public static UserChoice AdminCreateUserAccount()
    {
      AnsiConsole.Clear();

      var choice = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
              .Title("[mediumpurple2]What kind of account would you like to create?[/]")
              .PageSize(9)
              .HighlightStyle("gold3_1")
              .AddChoices(new[]
              {
                        "[lightsteelblue]Create customer account[/]",
                        "[lightsteelblue]Create admin account[/]",

                        "[deeppink4_2]Back[/]"
      }));

      string userChoice = StringValidationHelper.GetCleanSpectreConsoleString(choice);
      UserChoice enumChoice = EnumValidationHelper.GetSpecificEnumValue<UserChoice>(userChoice.Trim());
      AnsiConsole.Clear();
      return enumChoice;
    }
    public static UserChoice CustomerMenu()
    {
      AnsiConsole.Clear();

      var choice = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
              .Title("[mediumpurple2]What would you like to do?[/]")
              .PageSize(9)
              .HighlightStyle("gold3_1")
              .AddChoices(new[]
              {
                        "[lightsteelblue]Make deposit[/]",
                        "[lightsteelblue]Make withdrawal[/]",
                        "[lightsteelblue]List all accounts[/]",
                        "[lightsteelblue]Show log[/]",
                        "[lightsteelblue]Create account[/]",
                        "[lightsteelblue]Loan money[/]",

                        "[deeppink4_2]Logout[/]"
      }));



      string userChoice = StringValidationHelper.GetCleanSpectreConsoleString(choice);
      UserChoice enumChoice = EnumValidationHelper.GetSpecificEnumValue<UserChoice>(userChoice.Trim());
      AnsiConsole.Clear();
      return enumChoice;
    }
    public static UserChoice CustomerCreateUserAccount() => UserChoice.CreateCustomerAccount;
    public static UserChoice CustomerCreateAccount()
    {
      AnsiConsole.Clear();

      var choice = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
              .Title("[mediumpurple2]What kind of account would you like to create?[/]")
              .PageSize(3)
              .HighlightStyle("gold3_1")
              .AddChoices(new[]
              {
                        "[lightsteelblue]Create checkings account[/]",
                        "[lightsteelblue]Create savings account[/]",
                        "[lightsteelblue]Back[/]",
      }));

      string userChoice = StringValidationHelper.GetCleanSpectreConsoleString(choice);
      UserChoice enumChoice = EnumValidationHelper.GetSpecificEnumValue<UserChoice>(userChoice.Trim());
      AnsiConsole.Clear();
      return enumChoice;
    }
    public static UserChoice CustomerAccountList(List<string> accountIds, UserBase user)
    {
      Grid grid = new();

      grid.AddColumns(3);
      grid.AddRow(new Text[]{
                new Text($"{user.FirstName}", new Style(Color.MediumPurple2, Color.Black)).LeftJustified(),
                new Text($"{user.SocialSecurityNumber}", new Style(Color.MediumPurple2, Color.Black)).Centered(),
                new Text($"Time: {DateTime.UtcNow:D}", new Style(Color.MediumPurple2, Color.Black)).RightJustified(),
            });
      List<AccountBase> accounts = Database.GetAccountsInDatabase(accountIds);
      foreach (AccountBase account in accounts)
      {
        grid.AddRow(new Text[]{
                    new Text($"{account.GetBalance()}{account.CurrencyType}", new Style(Color.LightSteelBlue, Color.Black)).LeftJustified(),
                    new Text($"{account.AccountNumber}", new Style(Color.LightSteelBlue, Color.Black)).Centered(),
                    new Text($"{account.AccountType}", new Style(Color.LightSteelBlue, Color.Black)).RightJustified(),
                });
      }

      string choice = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
          .PageSize(3)
          .HighlightStyle("gold3_1")
          .AddChoices(new[]
          {
                    "[lightsteelblue]Back[/]"
          }));

      AnsiConsole.Clear();

      return UserChoice.Back;
    }
    public static void WriteLockedMenu()
    {
      AnsiConsole.Clear();
      AnsiConsole.Cursor.Hide();
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
      AnsiConsole.Clear();
      AnsiConsole.Cursor.Show();
    }




  }
}





