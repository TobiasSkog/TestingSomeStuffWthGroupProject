using GroupProject.App.BankManagement.User;
using Newtonsoft.Json;

namespace GroupProject.App.BankManagement.Account.BankAccounts
{
  public class CheckingsAccount : AccountBase
  {

    public CheckingsAccount()
    {

    }
    public CheckingsAccount(AccountStatuses accStatus, AccountTypes accType, decimal balance = 0, CurrencyTypes accountCurrencyType = CurrencyTypes.SEK) : base(accStatus, accType, balance, accountCurrencyType)
    {

    }
    public CheckingsAccount(string accountId, decimal balance = 0, AccountStatuses accStatus = AccountStatuses.Active, CurrencyTypes accountCurrencyType = CurrencyTypes.SEK) : base(accStatus, AccountTypes.Checking, balance, accountCurrencyType)
    {
      AccountId = accountId;
    }


    [JsonConstructor]
    public CheckingsAccount(AccountStatuses accountStatus, AccountTypes accountType, string accountNumber, string accountId, CurrencyTypes currencyType = CurrencyTypes.SEK, decimal balance = 0m)
    {
      AccountStatus = accountStatus;
      AccountType = accountType;
      AccountNumber = accountNumber;
      AccountId = accountId;
      CurrencyType = currencyType;
      Balance = balance;
    }
    public void CreateCheckingsAccount(UserBase user)
    {
      user.AccountIds.Add(AccountId);
    }
  }
}
