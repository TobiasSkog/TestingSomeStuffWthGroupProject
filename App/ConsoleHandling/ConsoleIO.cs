using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.User;
using GroupProject.App.LogicHandling;
using GroupProject.BankDatabase;
using GroupProject.BankDatabase.EventLogs;
using Spectre.Console;
using ValidationUtility;
using Text = Spectre.Console.Text;

namespace GroupProject.App.ConsoleHandling
{
  public static class ConsoleIO
  {
    private static Color exitColor = Color.Red3;
    private static Color errorColor = Color.Red;
    private static Color information = Color.DarkOrange;

    private static Color adminPromptColor = Color.DarkOrange3_1;
    private static Color adminChoiceColor = Color.DarkViolet;
    private static Color adminHighlightColor = Color.Gold3_1;
    private static Color adminDivider = Color.Gold3_1;
    private static Color adminDividerTextColor = Color.DarkOrange3_1;

    private static Color userPromptColor = Color.Gold3_1;
    private static Color userChoiceColor = Color.DarkViolet;
    private static Color userHighlightColor = Color.Gold3_1;
    private static Color userDivider = Color.DarkViolet;
    private static Color userDividerTextColor = Color.Gold3_1;

    public static void StartUp()
    {
      AnsiConsole.Clear();
      Console.BackgroundColor = ConsoleColor.Black;
      AnsiConsole.Cursor.Hide();

      WriteDivider("Eudgrade Online Banking");

      AnsiConsole.MarkupLine($"[{userChoiceColor}]{BankLoggo.Loggo}[/]");
      AnsiConsole.WriteLine("\n\n");
      AnsiConsole.MarkupLine($"[{userPromptColor}]\nConnecting to the Edugrade Online Banking Service...\n[/]");

      AnsiConsole.Cursor.SetPosition(1, 24);

      AnsiConsole.Write(new Markup("║                                                          ║", userChoiceColor));
      for (int i = 0; i < 59; i++)
      {
        for (int j = 0; j < i; j++)
        {

          Markup pb = new($"[rgb(0,{((i % 486) + (j % 1900)) % 280},{((i % 1300) + (j % 9900)) % 6500})]█[/]");
          AnsiConsole.Write(pb);
        }
        AnsiConsole.Cursor.SetPosition(2, 24);
        Thread.Sleep(1);
      }
      Thread.Sleep(1);
      AnsiConsole.Cursor.SetPosition(1, 1);
      AnsiConsole.Cursor.Show();
      AnsiConsole.Clear();
    }

    private static void WriteDivider(string text)
    {
      AnsiConsole.Write(new Rule($"[{userDividerTextColor}]{text}[/]").RuleStyle(userDivider).LeftJustified());
    }
    private static void WriteDividerAdmin(string text)
    {
      AnsiConsole.Write(new Rule($"[{adminDividerTextColor}]{text}[/]").RuleStyle(adminDivider).LeftJustified());
    }
    private static string EscapeMarkup(string input) => StringValidationHelper.GetCleanSpectreConsoleString(input);
    private static UserChoice ChoiceEscapeMarkup(string input)
    {
      string choice = EscapeMarkup(input);
      choice = StringValidationHelper.GetCleanSpectreConsoleStringForEnums(choice);
      UserChoice enumChoice = EnumValidationHelper.GetSpecificEnumValue<UserChoice>(choice);
      return enumChoice;
    }
    private static CurrencyTypes CurrencyEscapeMarkup(string input)
    {
      string choice = EscapeMarkup(input);
      choice = StringValidationHelper.GetCleanSpectreConsoleStringForEnumsOnly(choice);
      CurrencyTypes enumChoice = EnumValidationHelper.GetSpecificEnumValue<CurrencyTypes>(choice);
      return enumChoice;
    }
    public static string Username(string prompt)
    {
      string input = AnsiConsole.Prompt(
        new TextPrompt<string>($"[{userPromptColor}]{prompt}[/]")
        .PromptStyle(userChoiceColor)
        .ValidationErrorMessage(string.Empty)
        .Validate(username =>
        {
          var controlledUsername = UsernameValidationHelper.SpectreConsoleUsernameValidation(username, 5, 20, true, true);

          if (controlledUsername.ErrorMessages.Count <= 0)
          {
            input = controlledUsername.Username;
            return true;
          }
          else
          {
            AnsiConsole.Clear();
            foreach (string error in controlledUsername.ErrorMessages)
            {
              AnsiConsole.MarkupLine($"[{errorColor}]{error}[/]");
            }

            return false;
          }

        }));

      string output = StringValidationHelper.GetCleanSpectreConsoleString(input);
      return output;
    }

    public static UserChoice InformUser(string prompt)
    {
      AnsiConsole.Clear();
      WriteDivider("Information");
      var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
          .Title($"[{information}]{prompt}[/]")
          .PageSize(3)
          .HighlightStyle(userHighlightColor)
          .AddChoices(new[]
          {
            $"[{userChoiceColor}]Back[/]",
            $"[{exitColor}]Exit[/]",
          }));

      return ChoiceEscapeMarkup(choice);
    }

    public static UserChoice WrongLogin(string prompt)
    {
      AnsiConsole.Clear();
      WriteDivider("Information");
      var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
          .Title($"[{information}]{prompt}[/]")
          .PageSize(3)
          .HighlightStyle(userHighlightColor)
          .AddChoices(new[]
          {
            $"[{userChoiceColor}]Back[/]",
            $"[{exitColor}]Exit[/]",
          }));
      UserChoice userChoice = ChoiceEscapeMarkup(choice);

      if (userChoice == UserChoice.Back)
      {
        return UserChoice.WelcomeScreen;
      }

      return userChoice;
    }
    public static UserChoice InformAdmin(string prompt)
    {
      AnsiConsole.Clear();
      WriteDividerAdmin("Information");
      var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
          .Title($"[{information}]{prompt}[/]")
          .PageSize(3)
          .HighlightStyle(adminHighlightColor)
          .AddChoices(new[]
          {
            $"[{adminChoiceColor}]Back[/]",
            $"[{exitColor}]Exit[/]",
          }));

      return ChoiceEscapeMarkup(choice);
    }
    public static string CreateUsername(string prompt)
    {
      string input = AnsiConsole.Prompt(
        new TextPrompt<string>($"[{userPromptColor}]{prompt}[/]")
        .PromptStyle(userChoiceColor)
        .ValidationErrorMessage(string.Empty)
        .Validate(username =>
        {
          var controlledUsername = UsernameValidationHelper.SpectreConsoleUsernameValidation(username, 5, 20, true, true);
          if (controlledUsername.ErrorMessages.Count <= 0)
          {
            input = controlledUsername.Username;
            return true;
          }
          else
          {
            AnsiConsole.Clear();
            foreach (string error in controlledUsername.ErrorMessages)
            {
              AnsiConsole.MarkupLine($"[{errorColor}]{error}[/]");
            }

            return false;
          }

        }));

      string output = StringValidationHelper.GetCleanSpectreConsoleString(input);
      return output;
    }

    public static bool AskUser(string statement, string prompt)
    {
      AnsiConsole.MarkupLine($"[{userPromptColor}]{statement}[/]");
      var answer = AnsiConsole.Confirm($"[{userPromptColor}]{prompt}[/]");

      return answer;
    }
    public static string Password(string prompt)
    {
      string input = AnsiConsole.Prompt(
        new TextPrompt<string>($"[{userPromptColor}]{prompt}[/]")
        .PromptStyle(userChoiceColor)
        .ValidationErrorMessage(string.Empty)
        .Secret('o')
        .Validate(password =>
        {
          char[] specialChars = new char[] {
              '@', '#', '$', '%', '^', '&','*', '-',
              '_', '!', '+', '=','[', ']', '{', '}',
              '|', '\\',':', '\'', ',', '.', '?', '/',
              '`', '~', '"', '(', ')', ';','<', '>' };

          //PasswordValidationHelper.PasswordValidation(password, 3, 18, true, true, true, false);
          var controlledPw = PasswordValidationHelper.SpectreConsolePasswordValidation(password, 3, 12, false, false, false, false, specialChars);

          if (controlledPw.ErrorMessages.Count <= 0)
          {
            input = controlledPw.Password;

            return true;
          }
          else
          {
            AnsiConsole.Clear();
            foreach (string error in controlledPw.ErrorMessages)
            {
              AnsiConsole.MarkupLine($"[{errorColor}]{error}[/]");
            }

            return false;
          }
        }));

      string output = StringValidationHelper.GetCleanSpectreConsoleString(input);

      return output;
    }
    public static string GetUserInformation(string prompt)
    {
      string input = AnsiConsole.Prompt(
        new TextPrompt<string>($"[{userPromptColor}]{prompt}: [/]")
        .PromptStyle(userChoiceColor)
        .ValidationErrorMessage(string.Empty)
        .Validate(userInput =>
        {
          var controlledInput = StringValidationHelper.GetCleanSpectreConsoleUserInformation(userInput, true, 3, 30, true, true);

          if (controlledInput.ErrorMessages.Count <= 0)
          {
            input = controlledInput.UserInput;
            return true;
          }
          else
          {
            AnsiConsole.Clear();
            foreach (string error in controlledInput.ErrorMessages)
            {
              AnsiConsole.MarkupLine($"[{errorColor}]{error}[/]");
            }

            return false;
          }
        }));

      string output = StringValidationHelper.GetCleanSpectreConsoleString(input);
      return output;

    }
    public static UserChoice WelcomeMenu(string prompt = "")
    {
      AnsiConsole.Clear();
      WriteDivider("Main Menu | Eudgrade Online Banking");
      if (prompt != "")
      {
        Console.WriteLine(prompt);
      }

      var choice = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
              .Title($"[{userPromptColor}]What would you like to do?[/]")
              .PageSize(3)
              .HighlightStyle(userHighlightColor)
              .AddChoices(new[] {
                        $"[{userChoiceColor}]Login[/]",
                        $"[{userChoiceColor}]Create user account[/]",
                        $"[{exitColor}]Exit[/]",
      }));
      return ChoiceEscapeMarkup(choice);

      //string userChoice = StringValidationHelper.GetCleanSpectreConsoleString(choice);
      //UserChoice enumChoice = EnumValidationHelper.GetSpecificEnumValue<UserChoice>(userChoice.Trim());
      //AnsiConsole.Clear();
      //return enumChoice;
    }
    public static UserChoice AdminMenu()
    {
      AnsiConsole.Clear();
      WriteDividerAdmin("Admin Menu | Eudgrade Online Banking");
      var choice = AnsiConsole.Prompt(
      new SelectionPrompt<string>()
          .Title($"[{adminPromptColor}]What would you like to do?[/]")
          .PageSize(3)
          .HighlightStyle("gold3_1")
          .AddChoices(new[]
          {
                        $"[{adminChoiceColor}]Login[/]",
                        $"[{adminChoiceColor}]Create user account[/]"
                        ,
                        $"[{adminChoiceColor}]Logout[/]",
                        $"[{exitColor}]Exit[/]"
          }));

      return ChoiceEscapeMarkup(choice);
    }


    public static UserChoice AdminAddAccountMenu()
    {
      AnsiConsole.Clear();
      WriteDividerAdmin("Account Creation Menu | Eudgrade Online Banking");

      string choice = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
          .Title($"[{adminPromptColor}]What kind of account would you like to create?[/]")
          .PageSize(3)
          .HighlightStyle("gold3_1")
          .AddChoices(new[]
          {
                    $"[{adminChoiceColor}]Customer Account[/]",
                    $"[{adminChoiceColor}]Administrator Account[/]",
                    $"[{adminChoiceColor}]Back[/]"
          }));

      return ChoiceEscapeMarkup(choice);
    }
    public static UserChoice AdminCurrencyExchangeMenu()
    {
      AnsiConsole.Clear();
      WriteDividerAdmin("Currency Exchange Update | Eudgrade Online Banking");

      string choice = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
             .Title($"[{adminPromptColor}]What would you like to do?[/]")
              .PageSize(3)
              .HighlightStyle("gold3_1")
              .AddChoices(new[]
              {
                        $"[{adminChoiceColor}]Update to the latest currency exchange rates[/]",

                        $"[{adminChoiceColor}]Back[/]"
      }));

      return ChoiceEscapeMarkup(choice);
    }
    public static UserChoice AdminCreateUserAccount()
    {
      AnsiConsole.Clear();
      WriteDividerAdmin("User Account Creation Menu | Eudgrade Online Banking");

      var choice = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
              .Title($"[{adminPromptColor}]What kind of account would you like to create?[/]")
              .PageSize(9)
              .HighlightStyle("gold3_1")
              .AddChoices(new[]
              {
                         $"[{adminChoiceColor}]Create customer account[/]",
                         $"[{adminChoiceColor}]Create admin account[/]",

                         $"[{adminChoiceColor}]Back[/]"
      }));

      return ChoiceEscapeMarkup(choice);
    }
    public static UserChoice CustomerMenu()
    {
      AnsiConsole.Clear();
      WriteDivider("User Menu | Eudgrade Online Banking");

      var choice = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
              .Title($"[{userPromptColor}]What would you like to do?[/]")
              .PageSize(9)
              .HighlightStyle("gold3_1")
              .AddChoices(new[]
              {
                        $"[{userChoiceColor}]Make deposit[/]",
                        $"[{userChoiceColor}]Make withdrawal[/]",
                        $"[{userChoiceColor}]List all accounts[/]",
                        $"[{userChoiceColor}]Show log[/]",
                        $"[{userChoiceColor}]Create bank account[/]",
                        $"[{userChoiceColor}]Loan money[/]",

                        $"[{userChoiceColor}]Logout[/]",
                        $"[{exitColor}]Exit[/]",
      }));

      return ChoiceEscapeMarkup(choice);
    }
    public static UserChoice WhatKindOfAccountMenu()
    {
      //;
      //ConsoleIO.CustomerCreateUserAccount();

      AnsiConsole.Clear();
      WriteDividerAdmin("User Account Creation Menu | Eudgrade Online Banking");

      var choice = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
              .Title($"[{adminPromptColor}]What kind of account would you like to create?[/]")
              .PageSize(9)
              .HighlightStyle("gold3_1")
              .AddChoices(new[]
              {
                         $"[{adminChoiceColor}]Create customer account[/]",
                         $"[{adminChoiceColor}]Create admin account[/]",

                         $"[{adminChoiceColor}]Back[/]"
      }));


      return ChoiceEscapeMarkup(choice);
    }

    public static CurrencyTypes GetCurrencyTypeFromList()
    {
      AnsiConsole.Clear();
      WriteDivider("User Account Creation | Eudgrade Online Banking");

      //var prompt = new SelectionPrompt<CurrencyTypes>()
      //   .Title($"[{userPromptColor}]What currency type would you like to have?[/]")
      //   .PageSize(10)
      //   .HighlightStyle(userHighlightColor);

      //foreach (CurrencyTypes value in Enum.GetValues(typeof(CurrencyTypes)))
      //{
      //  // prompt.AddChoice($"[{userPromptColor}]{value}[/]");
      //  prompt.AddChoice(value);
      //}

      //CurrencyTypes choice = AnsiConsole.Prompt(prompt);

      var prompt = new SelectionPrompt<string>()
    .Title($"[{userPromptColor}]What currency type would you like to have?[/]")
    .PageSize(10)
    .HighlightStyle(userHighlightColor);

      foreach (CurrencyTypes value in Enum.GetValues(typeof(CurrencyTypes)))
      {
        prompt.AddChoice($"[{userChoiceColor}]{value}[/]");

      }

      var choice = AnsiConsole.Prompt(prompt);

      return CurrencyEscapeMarkup(choice);
    }
    public static UserChoice CustomerCreateAccount()
    {
      AnsiConsole.Clear();
      WriteDivider("User Bank Account Creation | Eudgrade Online Banking");

      var choice = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
              .Title($"[{userPromptColor}]What kind of account would you like to create?[/]")
              .PageSize(3)
              .HighlightStyle("gold3_1")
              .AddChoices(new[]
              {
                        $"[{userChoiceColor}]Create checkings account[/]",
                        $"[{userChoiceColor}]Create savings account[/]",
                        $"[{userChoiceColor}]Back[/]",
      }));

      return ChoiceEscapeMarkup(choice);
    }
    public static UserChoice CreateSavingsAccount()
    {
      Console.WriteLine("CreateSavingsAccount: 475");
      return default;
    }
    public static UserChoice CreateCheckingsAccount()
    {
      Console.WriteLine("CreateCheckingsAccount: 480");
      return default;
    }
    public static UserChoice CustomerAccountList(UserBase user)
    {
      AnsiConsole.Clear();
      WriteDivider($"{user.FirstName + " " + user.LastName} recent activity log | Eudgrade Online Banking");

      Rule accDivider = new Rule().RuleStyle(userChoiceColor);


      List<AccountBase> userAccounts = Database.LoadUserAccounts(user.AccountIds);

      var table = new Table()
          .Border(TableBorder.Double)
          .BorderColor(userChoiceColor)
          .LeftAligned()
          .Collapse()
          .AddColumns(
            new TableColumn($"[{userPromptColor}]Balance[/]").Centered(),
            new TableColumn($"[{userPromptColor}]Account Number[/]").Centered(),
            new TableColumn($"[{userPromptColor}]Account Type[/]").Centered()
          );

      foreach (var account in userAccounts)
      {
        string balance = $"[{userPromptColor}]{account.GetBalance()} {account.CurrencyType}[/]";
        table.AddRow(balance, $"[{userPromptColor}]{account.AccountNumber}[/]", $"[{userPromptColor}]{account.AccountType}[/]").LeftAligned();
        table.AddRow(accDivider, accDivider, accDivider);
      }

      table.RemoveRow(userAccounts.Count * 2 - 1);

      AnsiConsole.Write(table);

      var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title($"[{adminPromptColor}][/]")
        .PageSize(3)
        .HighlightStyle("gold3_1")
        .AddChoices(new[]
        {
          $"[{adminChoiceColor}]Back[/]"
        }));

      return ChoiceEscapeMarkup(choice);
    }

    public static UserChoice CustomerLog(UserBase user)
    {
      AnsiConsole.Clear();

      Rule logDivider = new Rule().RuleStyle(userChoiceColor);
      WriteDivider($"{user.FirstName + " " + user.LastName} user logs | Eudgrade Online Banking");

      var filteredLogs = user.UserLog;

      var table = new Table()
          .Border(TableBorder.Double)
          .BorderColor(userChoiceColor)
          .LeftAligned()
          .Collapse()
          .AddColumns(
            new TableColumn($"[{userPromptColor}]Time[/]").Centered(),
            new TableColumn($"[{userPromptColor}]Event[/]").Centered(),
            new TableColumn($"[{userPromptColor}]Event Details[/]").Centered()
          );

      foreach (var log in filteredLogs)
      {
        table.AddRow($"[{userPromptColor}{log.Timestamp}[/]", $"[{userPromptColor}]{log.EventCategory}[/]", $"[{userPromptColor}]{log.Message}[/]").LeftAligned();
        table.AddRow(logDivider, logDivider, logDivider);
      }

      table.RemoveRow(filteredLogs.Count * 2 - 1);

      AnsiConsole.Write(table);

      var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title($"[{adminPromptColor}][/]")
        .PageSize(3)
        .HighlightStyle("gold3_1")
        .AddChoices(new[]
        {
          $"[{adminChoiceColor}]Back[/]"
        }));

      return ChoiceEscapeMarkup(choice);
    }
    public static void WriteLockedMenu()
    {
      AnsiConsole.Clear();
      AnsiConsole.Cursor.Hide();
      int LOCKEDFOR = 10;
      DateTime now = DateTime.Now;
      DateTime lockedDuration = now.AddMinutes(LOCKEDFOR);
      TimeSpan timeRemaining;
      AnsiConsole.WriteLine($"You are locked for {LOCKEDFOR} minutes");

      for (int i = 0; i < LOCKEDFOR * 60; i++)
      {
        now = DateTime.Now;
        timeRemaining = lockedDuration - now;
        double remainingMinutes = timeRemaining.TotalMinutes;
        double remainingSeconds = timeRemaining.TotalSeconds;

        AnsiConsole.WriteLine(remainingMinutes > 0 ? $"Remaining time {Math.Floor(remainingMinutes)}minutes {remainingSeconds % 60:#}seconds" : $"Remaining time {remainingSeconds}seconds");

        Thread.Sleep(1000);
        AnsiConsole.Cursor.SetPosition(0, Console.CursorTop - 1);
      }
      AnsiConsole.Clear();
      AnsiConsole.Cursor.Show();
    }



  }
}





