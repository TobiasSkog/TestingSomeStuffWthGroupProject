using GroupProject.App.BankManagement.User;
using System.Text.Json.Serialization;

namespace GroupProject.App.BankManagement.Account.BankAccounts
{
  public class SavingsAccount : AccountBase
  {
    public SavingsAccount()
    {

    }
    public SavingsAccount(AccountStatuses accStatus, AccountTypes accType, decimal balance = 0, CurrencyTypes accountCurrencyType = CurrencyTypes.SEK) : base(accStatus, accType, balance, accountCurrencyType)
    {

    }
    public SavingsAccount(string accountId, decimal balance = 0, AccountStatuses accStatus = AccountStatuses.Active, CurrencyTypes accountCurrencyType = CurrencyTypes.SEK) : base(accStatus, AccountTypes.Saving, balance, accountCurrencyType)
    {
      AccountId = accountId;
    }

    [JsonConstructor]
    public SavingsAccount(AccountStatuses accountStatus, AccountTypes accountType, string accountNumber, string accountId, CurrencyTypes currencyType = CurrencyTypes.SEK, decimal balance = 0m)
    {
      AccountStatus = accountStatus;
      AccountType = accountType;
      AccountNumber = accountNumber;
      AccountId = accountId;
      CurrencyType = currencyType;
      Balance = balance;
    }
    public void CreateSavingsAccount(UserBase user)
    {
      user.AccountIds.Add(AccountId);
    }
  }
}

