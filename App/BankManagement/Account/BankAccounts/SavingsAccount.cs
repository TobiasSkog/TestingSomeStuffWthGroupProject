using GroupProject.App.BankManagement.Account.BankAccounts.BankTransactions;
using GroupProject.App.BankManagement.Interfaces;
using GroupProject.App.BankManagement.User;
using GroupProject.App.ConsoleHandling;
using ValidationUtility;

namespace GroupProject.App.BankManagement.Account.BankAccounts
{
  public class SavingsAccount : AccountBase
  {

    public SavingsAccount(AccountStatuses accStatus, AccountTypes accType, decimal balance = 0, CurrencyTypes accountCurrencyType = CurrencyTypes.SEK) : base(accStatus, accType, balance, accountCurrencyType)
    {

    }
    public SavingsAccount(string accountId, decimal balance = 0, AccountStatuses accStatus = AccountStatuses.Active, CurrencyTypes accountCurrencyType = CurrencyTypes.SEK) : base(accStatus, AccountTypes.Saving, balance, accountCurrencyType)
    {
      AccountId = accountId;
    }
    public void CreateSavingsAccount(UserBase user)
    {
      user.AccountIds.Add(AccountId);
    }
  }
}

