using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.Account.BankAccounts;
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


    public static decimal AmountOfMoney(string prompt, decimal maxAllowed = decimal.MaxValue)
    {

      decimal input = AnsiConsole.Prompt(
        new TextPrompt<decimal>($"[{userPromptColor}]{prompt}: [/]")
        .PromptStyle(userChoiceColor)
        .ValidationErrorMessage("")
        .Validate(userInput =>
        {
          if (userInput <= 0)
          {
            return ValidationResult.Error("The amount cannot be a negative value");
          }
          else if (userInput > maxAllowed)
          {
            return ValidationResult.Error("The amount cannot be greater than your current account balance");
          }
          else
          {
            return ValidationResult.Success();
          }
        }));

      return input;
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
    }
    public static UserChoice AdminMenu(UserBase user)
    {
      AnsiConsole.Clear();
      WriteDividerAdmin($"{user.FirstName + " " + user.LastName} Admin Menu | Eudgrade Online Banking");
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


    public static UserChoice AdminAddAccountMenu(UserBase user)
    {
      AnsiConsole.Clear();
      WriteDividerAdmin($"{user.FirstName + " " + user.LastName} Admin Account Creation Menu | Eudgrade Online Banking");

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
    public static UserChoice AdminCurrencyExchangeMenu(UserBase user)
    {
      AnsiConsole.Clear();
      WriteDividerAdmin($"{user.FirstName + " " + user.LastName} Admin Currency Exchange Update | Eudgrade Online Banking");

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
    public static UserChoice AdminCreateUserAccount(UserBase user)
    {
      AnsiConsole.Clear();
      WriteDividerAdmin($"{user.FirstName + " " + user.LastName} Admin - User Account Creation Menu | Eudgrade Online Banking");

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
    public static UserChoice CustomerMenu(UserBase user)
    {
      AnsiConsole.Clear();
      WriteDivider($"{user.FirstName + " " + user.LastName} User Menu | Eudgrade Online Banking");

      var choice = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
              .Title($"[{userPromptColor}]What would you like to do?[/]")
              .PageSize(9)
              .HighlightStyle("gold3_1")
              .AddChoices(new[]
              {
                        $"[{userChoiceColor}]Make deposit[/]",
                        $"[{userChoiceColor}]Make withdrawal[/]",
                        $"[{userChoiceColor}]Make transfer[/]",
                        $"[{userChoiceColor}]List all accounts[/]",
                        $"[{userChoiceColor}]Show log[/]",
                        $"[{userChoiceColor}]Create bank account[/]",
                        $"[{userChoiceColor}]Loan money[/]",
                        $"[{userChoiceColor}]Logout[/]",
                        $"[{exitColor}]Exit[/]",
      }));

      return ChoiceEscapeMarkup(choice);
    }
    public static UserChoice WhatKindOfAccountMenu(UserBase user)
    {
      AnsiConsole.Clear();
      WriteDividerAdmin($"{user.FirstName + " " + user.LastName} Admin - User Account Creation Menu | Eudgrade Online Banking");

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


    public static string TransferTargetAccount(UserBase user)
    {
      AnsiConsole.Clear();
      WriteDivider($"{user.FirstName + " " + user.LastName} transfer | Eudgrade Online Banking");

      var choice = AnsiConsole.Prompt(
        new TextPrompt<string>($"[{userPromptColor}]What account number would you like to transfer to?[/]")
        .PromptStyle(userHighlightColor)
        .ValidationErrorMessage("[red]That's not a valid accountnumber[/]")
        .Validate(accountNumber =>
        {
          if (accountNumber.Length < 10)
          {
            return ValidationResult.Error("[red]Account number is too short[/]");
          }
          if (accountNumber.Length > 10)
          {
            return ValidationResult.Error("[red]Account number is too long[/]");
          }
          return ValidationResult.Success();
        }));

      return EscapeMarkup(choice);
    }

    public static CurrencyTypes GetCurrencyTypeFromList(UserBase user)
    {
      AnsiConsole.Clear();
      WriteDivider($"{user.FirstName + " " + user.LastName} User Account Creation | Eudgrade Online Banking");

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
    public static UserChoice CustomerCreateAccount(UserBase user)
    {
      AnsiConsole.Clear();
      WriteDivider($"{user.FirstName + " " + user.LastName} User Bank Account Creation | Eudgrade Online Banking");

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
    //public static UserChoice CreateSavingsAccount(UserBase user)
    //{

    //  Console.WriteLine("CreateSavingsAccount: 475");
    //  return default;
    //}
    //public static UserChoice CreateCheckingsAccount(UserBase user)
    //{
    //  Console.WriteLine("CreateCheckingsAccount: 480");
    //  return default;
    //}
    public static UserChoice CustomerAccountList(UserBase user, List<AccountBase> userAccounts)
    {
      AnsiConsole.Clear();
      WriteDivider($"{user.FirstName + " " + user.LastName} recent activity log | Eudgrade Online Banking");

      Rule accDivider = new Rule().RuleStyle(userChoiceColor);

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
      if (userAccounts.Count > 1)
      {
        table.RemoveRow(userAccounts.Count * 2 - 1);
      }
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

    public static AccountBase GetSpecificAccount(string promptTitle, UserBase user, List<AccountBase> userAccounts)
    {
      AnsiConsole.Clear();
      WriteDivider($"{user.FirstName + " " + user.LastName} Deposit | Eudgrade Online Banking");


      var prompt = new SelectionPrompt<string>()
        .Title($"[{userPromptColor}]{promptTitle}[/]")
        .PageSize(10)
        .HighlightStyle("gold3_1");
      foreach (var account in userAccounts)
      {
        prompt.AddChoice($"[{userChoiceColor}]{account.AccountNumber} - {account.GetBalance()}{account.CurrencyType} - {account.AccountType}[/]");
      }

      prompt.AddChoice($"[{userChoiceColor}]Back[/]");

      string choice = AnsiConsole.Prompt(prompt);

      if (choice.Contains("Back"))
      {
        return null;
      }

      AccountBase selectedAccount = userAccounts.FirstOrDefault(account => choice.Contains(account.AccountNumber));

      return selectedAccount;
    }
    public static UserChoice CustomerLog(UserBase user, List<EventLog> userLogs)
    {
      AnsiConsole.Clear();

      Rule logDivider = new Rule().RuleStyle(userChoiceColor);
      WriteDivider($"{user.FirstName + " " + user.LastName} user logs | Eudgrade Online Banking");


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

      foreach (var log in userLogs)
      {
        table.AddRow($"[{userPromptColor}]{log.Timestamp}[/]", $"[{userPromptColor}]{log.EventCategory}[/]", $"[{userPromptColor}]{log.Message}[/]").LeftAligned();
        table.AddRow(logDivider, logDivider, logDivider);
      }

      if (userLogs.Count > 0)
      {
        table.RemoveRow(userLogs.Count * 2 - 1);
      }

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





