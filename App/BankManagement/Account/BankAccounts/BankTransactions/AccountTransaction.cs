using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.BankDatabase.EventLogs;
using GroupProject.BankDatabase.EventLogs.Events;
using Spectre.Console;
using System.Transactions;

namespace GroupProject.App.BankManagement.Account.BankAccounts.BankTransactions
{
  public class AccountTransaction
  {
    public UserBase User { get; private set; }
    public UserBase TargetUser { get; private set; }
    public decimal Amount { get; private set; }
    public CurrencyTypes CurrencyType { get; private set; }
    public AccountBase SourceAccount { get; private set; }
    public AccountBase TargetAccount { get; private set; }
    public AccountStatuses AccountStatus { get; private set; }
    public TransactionStatus UserTransactionStatus { get; protected set; }
    public TransactionType TransactionType { get; protected set; }
    public AccountTransaction(
        UserBase user,
        UserBase targetUser,
        decimal amount,
        CurrencyTypes currencyType,
        AccountBase sourceAccount,
        AccountBase targetAccount,
        AccountStatuses accountStatus,
        TransactionType transactionType,
        TransactionStatus transactionStatus = TransactionStatus.Pending)
    {
      User = user;
      TargetUser = targetUser;
      Amount = amount;
      CurrencyType = currencyType;
      SourceAccount = sourceAccount;
      TargetAccount = targetAccount;
      AccountStatus = accountStatus;
      UserTransactionStatus = transactionStatus;
      TransactionType = transactionType;
    }
    private string GetTransactionMessageFromStatus(TransactionStatus transactionStatus)
    {
      return transactionStatus switch
      {
        TransactionStatus.Success => "Successful transaction",
        TransactionStatus.Denied => "Transaction attempt denied",
        TransactionStatus.Pending => "Transaction pending",
        TransactionStatus.Failed => "Transaction failed",
        TransactionStatus.DestinationAccountNotFound => "Destination Account Not Found. Transaction failed",
        TransactionStatus.BalanceTooLowForLoan => "Balance too low for loan. Transaction failed",
        TransactionStatus.BalanceTooLowForWithdrawal => "Balance too low for withdrawal. Transaction failed",
        TransactionStatus.BalanceTooLowForTransfer => "Balance too low for transfer. Transaction failed",
        TransactionStatus.DepositIsANegativeValue => "Deposit is a negative value. Transaction failed",
        TransactionStatus.WithdrawSuccess => "Successful withdrawal",
        TransactionStatus.DepositSuccess => "Successful deposit",
        TransactionStatus.TransferSuccess => "Successful transfer",
        TransactionStatus.WithdrawalLimit => "Withdrawal limit reached. Transaction failed",
        TransactionStatus.LoanSuccess => "Successful loan",
        TransactionStatus.AmountIsANegativeValue => "Tried to transfer a negative amount. Transaction failed",
        _ => "Unknown Transaction Status"

      };


    }
    public EventLog ProcessTransaction(AccountTransaction transaction, UserType userType)
    {
      if (userType != UserType.Admin)
      {
        return new WarningLog(transaction.User.Username, "A non-admin user tried to process a transaction");
      }
      string message;
      TransactionStatus status;
      switch (transaction.TransactionType)
      {
        case TransactionType.Withdraw:
          status = transaction.SourceAccount.WithdrawMoney(transaction);
          message = GetTransactionMessageFromStatus(status);
          break;

        case TransactionType.Deposit:
          status = transaction.SourceAccount.DepositMoney(transaction);
          message = GetTransactionMessageFromStatus(status);

          break;

        case TransactionType.Transfer:
          status = transaction.SourceAccount.TransferMoney(transaction);
          message = GetTransactionMessageFromStatus(status);
          if (status == TransactionStatus.TransferSuccess)
          {
            status = transaction.TargetAccount.ReceiveTransferMoney(transaction.Amount);
            if (status != TransactionStatus.TransferSuccess)
            {
              message = GetTransactionMessageFromStatus(status);
            }
          }
          break;

        case TransactionType.Loan:
          status = transaction.SourceAccount.DisplayRentFromDeposit(transaction);
          message = GetTransactionMessageFromStatus(status);
          break;

        default:
          throw new ArgumentException("Invalid transaction type.");
      }

      return new TransactionLog(
            transaction.User.Username,
            message,
            transaction.Amount,
            transaction.SourceAccount.AccountNumber,
            transaction.TargetAccount.AccountNumber
          );
    }
  }
}
