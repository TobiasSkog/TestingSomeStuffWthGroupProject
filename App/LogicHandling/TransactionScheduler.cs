using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.Account.BankAccounts.BankTransactions;
using GroupProject.App.BankManagement.User;
using GroupProject.BankDatabase;
using GroupProject.BankDatabase.EventLogs;

namespace GroupProject.App.LogicHandling
{
  //TODO: TRANSACTIONSCHEDULER CHANGE TIMESPAN.FROMSECONDS TO MINUTES!!!!!!!!

  public class TransactionScheduler
  {
    public event EventHandler TransactionsProcessed;

    private Timer _timer;
    private List<AccountTransaction> _pendingTransactions = new List<AccountTransaction>();
    private Logger _logger;
    private Database _DB;
    internal List<AccountBase> accounts { get; set; }
    public TransactionScheduler(int timerDelayMinutes, Logger logger, Database database)
    {
      _logger = logger;
      _DB = database;
      _timer = new Timer(ExecuteScheduledTransaction, null, TimeSpan.Zero, TimeSpan.FromSeconds(timerDelayMinutes));
    }

    private void ExecuteScheduledTransaction(object state)
    {
      if (_pendingTransactions.Count > 0)
      {
        List<AccountBase> accountsToUpdate = new();

        foreach (var transaction in _pendingTransactions)
        {
          List<AccountBase> updatedAccounts = ProcessTransaction(transaction);
          accountsToUpdate.AddRange(updatedAccounts);
        }

        OnTransactionProcessed(accountsToUpdate);
      }

      _pendingTransactions.Clear();
    }

    protected virtual void OnTransactionProcessed(List<AccountBase> accountsToUpdate)
    {
      _DB.UpdateAccountDatabase(accountsToUpdate);
      Console.WriteLine("ping");
      TransactionsProcessed?.Invoke(this, EventArgs.Empty);
    }
    private List<AccountBase> ProcessTransaction(AccountTransaction transaction)
    {
      EventLog transactionLog = transaction.ProcessTransaction(transaction, UserType.Admin);
      Console.WriteLine(transactionLog.ToString());
      _logger.Log(transactionLog);
      transaction.User.UserLog.AddUserLog(transaction.User.Username, transactionLog);
      List<AccountBase> accountsToUpdate = new();
      if (transactionLog.Message.Contains("Success"))
      {
        Console.WriteLine("Transaction was successful. Saving Database with new account information");
        accountsToUpdate.Add(transaction.SourceAccount);

        if (transaction.TargetAccount != null)
        {
          accountsToUpdate.Add(transaction.TargetAccount);
        }
      }

      return accountsToUpdate;
    }

    public void QueueTransaction(AccountTransaction transaction)
    {
      _pendingTransactions.Add(transaction);
    }
    public void Stop()
    {
      _timer?.Change(Timeout.Infinite, Timeout.Infinite);
    }
  }
}
