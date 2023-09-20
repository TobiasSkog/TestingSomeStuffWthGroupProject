using GroupProject.App.BankManagement.Account.BankAccounts.BankTransactions;
using GroupProject.App.BankManagement.Interfaces;
using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.BankDatabase.JsonConverters;
using Newtonsoft.Json;
using ValidationUtility;

namespace GroupProject.App.BankManagement.Account
{
  [JsonConverter(typeof(CustomAccountConverter))]
  [JsonObject(MemberSerialization.OptIn)]
  public abstract class AccountBase
  {
    [JsonProperty]
    public virtual AccountStatuses AccountStatus { get; protected set; }
    [JsonProperty]
    public virtual AccountTypes AccountType { get; protected set; }
    [JsonProperty]
    public virtual string AccountNumber { get; protected set; }
    [JsonProperty]
    public string AccountId { get; protected set; }
    [JsonProperty]
    public virtual CurrencyTypes CurrencyType { get; protected set; }
    [JsonProperty]
    internal virtual decimal Balance { get; set; }

    public AccountBase()
    {

    }
    public AccountBase(AccountStatuses accStatus, AccountTypes accType, decimal balance = 0m, CurrencyTypes currencyType = CurrencyTypes.SEK)
    {
      AccountStatus = accStatus;
      AccountType = accType;
      AccountId = Guid.NewGuid().ToString();
      AccountNumber = StringValidationHelper.CreateRandomString(10, "1234567890");

      CurrencyType = currencyType;
      if (balance >= 0)
      {
        Balance = balance;
      }
    }

    [JsonConstructor]
    public AccountBase(AccountStatuses accountStatus, AccountTypes accountType, string accountNumber, string accountId, CurrencyTypes currencyType = CurrencyTypes.SEK, decimal balance = 0m)
    {
      AccountStatus = accountStatus;
      AccountType = accountType;
      AccountNumber = accountNumber;
      AccountId = accountId;

      CurrencyType = currencyType;
      if (balance >= 0)
      {
        Balance = balance;
      }
      else
      {
        Balance = 0m;
      }
    }
    public decimal GetBalance()
    {
      if (AccountStatus != AccountStatuses.Locked)
      {
        return Balance;
      }
      return -1;
    }
    public AccountBase GetAccounts(string accountNumber, UserBase user)
    {
      if (user.AccountIds.Contains(accountNumber) && AccountNumber == accountNumber)
      {
        return this;
      }
      return null;
    }
    public virtual void DisplayRentFromDeposit(decimal depositAmount, int timeFrame)
    {

    }
    public TransactionStatus DisplayRentFromDeposit(AccountTransaction transaction)
    {
      return TransactionStatus.Success;
    }



    ///////////////////////////////////////////////////////////////

    public virtual AccountTransaction DepositMoney(UserBase user, decimal depositAmount)
    {
      return new AccountTransaction(user, user, depositAmount, CurrencyType, this, this, AccountStatus, TransactionType.Deposit);
    }



    public virtual void WithdrawMoney(decimal withdrawAmount)
    {
      if (Balance < 0 || withdrawAmount > Balance)
      {
        throw new ArgumentException("You cannot withdraw more than you have on your account.");
      }

      // Create a transaction to be handled at the correct timepoint

    }


    public virtual void TransferMoney(decimal transferAmount, AccountBase targetAccount)
    {
      if (Balance < 0 || transferAmount > Balance)
      {
        throw new ArgumentException("You cannot transfer more than you have on your account.");
      }
      if (targetAccount.AccountStatus != AccountStatuses.Active)
      {
        throw new ArgumentException("Could not find the target account.");
      }
      // Create a transaction to be handled at the correct timepoint

    }

    ///////////////////////////////////////////////////////////////
    public virtual TransactionStatus DepositMoney(AccountTransaction transaction)
    {
      if (transaction.Amount < 0)
      {
        return TransactionStatus.DepositIsANegativeValue;
      }

      Balance += transaction.Amount;
      return TransactionStatus.DepositSuccess;
    }
    public virtual TransactionStatus WithdrawMoney(AccountTransaction transaction)
    {
      if (Balance < 0 || transaction.Amount > Balance)
      {
        return TransactionStatus.BalanceTooLowForWithdrawal;
      }
      Balance -= transaction.Amount;
      return TransactionStatus.WithdrawSuccess;
    }
    public virtual TransactionStatus TransferMoney(AccountTransaction transaction)
    {
      if (Balance < 0 || transaction.Amount > Balance)
      {
        return TransactionStatus.BalanceTooLowForTransfer;
      }
      if (transaction.SourceAccount.AccountId != null)
      {
        return TransactionStatus.DestinationAccountNotFound;
      }
      Balance -= transaction.Amount;
      transaction.TargetAccount.Balance += transaction.Amount;
      return TransactionStatus.TransferSuccess;
    }
  }
}
