using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Customer;

namespace GroupProject.App.BankManagement.Account.BankAccounts.BankTransactions
{
  public class Transaction
  {
    public UserCustomer User { get; private set; }
    public UserCustomer TargetUser { get; private set; }
    public decimal Balance { get; private set; }
    public CurrencyTypes CurrencyType { get; private set; }
    public AccountBase SourceAccount { get; private set; }
    public AccountBase TargetAccount { get; private set; }
    public AccountStatuses AccountStatus { get; private set; }
    public TransactionStatus UserTransactionStatus { get; private set; }
    public List<string> TransactionLog { get; private set; }
    public Transaction(
        UserCustomer user,
        decimal balance,
        CurrencyTypes currencyType,
        AccountBase sourceAccount,
        AccountBase targetAccount,
        AccountStatuses accountStatus,
        TransactionStatus transactionStatus = TransactionStatus.Pending)
    {
      User = user ?? throw new ArgumentNullException(nameof(user));
      Balance = balance;
      CurrencyType = currencyType;
      SourceAccount = sourceAccount;
      TargetAccount = targetAccount;
      AccountStatus = accountStatus;
      UserTransactionStatus = transactionStatus;
      TransactionLog = new List<string>();
    }
    public void AddTransactionLog(string transactionLog)
    {
      TransactionLog.Add(transactionLog);
    }

    public void ProcessTransaction(UserTypes userType, TransactionType transactionType)
    {
      if (userType != UserTypes.Admin)
      {
        throw new InvalidOperationException("Only admins can process transactions.");
      }
      string transactionLog = "";

      switch (transactionType)
      {
        case TransactionType.Withdraw:
          SourceAccount.WithdrawMoney(this);

          transactionLog += $"Withdrawal - Status: {UserTransactionStatus}. Withdrawal amount:{Balance}{CurrencyType}. Source account number: {SourceAccount.AccountNumber}.";
          break;

        case TransactionType.Deposit:
          SourceAccount.DepositMoney(this);

          transactionLog += $"Deposit - Status: {UserTransactionStatus}. Deposit amount:{Balance}{CurrencyType}. Destination account number: {SourceAccount.AccountNumber}.";
          break;

        case TransactionType.TransferOwnAccount:
          TargetAccount = SourceAccount;
          SourceAccount.TransferMoney(this);
          transactionLog += $"Transfer - Status: {UserTransactionStatus}. Transfer amount:{Balance}{CurrencyType}. Source account number: {SourceAccount.AccountNumber}. Destination account number: {TargetAccount.AccountNumber}.";
          break;

        case TransactionType.TransferTargetAccount:
          SourceAccount.TransferMoney(this);

          transactionLog += $"Transfer - Status: {UserTransactionStatus}. Transfer amount:{Balance}{CurrencyType}. Source account number: {SourceAccount.AccountNumber}. Destination account number: {TargetAccount.AccountNumber}.";
          break;

        //TODO: IMPLEMENT TRASNACTIONTYPE.RENT IN TRANSACTIONS.CS THIS IS NOT DONE!
        case TransactionType.Rent:
          SourceAccount.DisplayRentFromDeposit(this);

          transactionLog += $"Rent - Status: {UserTransactionStatus}. Rent amount:{Balance}{CurrencyType}. Destination number: {TargetAccount.AccountNumber}.";
          break;

        default:
          throw new ArgumentException("Invalid transaction type.");
      }
      AddTransactionLog(transactionLog);
    }
  }
}
