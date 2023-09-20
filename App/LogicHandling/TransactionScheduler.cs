using GroupProject.App.BankManagement.Account.BankAccounts.BankTransactions;
using GroupProject.App.BankManagement.User;
using GroupProject.BankDatabase;
using GroupProject.BankDatabase.EventLogs;

namespace GroupProject.App.LogicHandling
{
  //TODO: TRANSACTIONSCHEDULER CHANGE TIMESPAN.FROMSECONDS TO MINUTES!!!!!!!!

  public class TransactionScheduler
  {
    private Timer _timer;
    private List<AccountTransaction> _pendingTransactions = new List<AccountTransaction>();
    private Logger _logger;
    private Database _database;
    public TransactionScheduler(int timerDelayMinutes, Logger logger, Database database)
    {
      _logger = logger;
      _database = database;
      _timer = new Timer(ExecuteScheduledTransaction, null, TimeSpan.Zero, TimeSpan.FromSeconds(timerDelayMinutes));
    }

    private void ExecuteScheduledTransaction(object state)
    {

      Console.WriteLine("Executing scheduled transaction at: " + DateTime.Now);
      if (_pendingTransactions.Count > 0)
      {
        foreach (var transaction in _pendingTransactions)
        {
          ProcessTransaction(transaction);
        }
      }

      _pendingTransactions.Clear();
    }

    private void ProcessTransaction(AccountTransaction transaction)
    {
      EventLog transactionLog = transaction.ProcessTransaction(transaction, UserType.Admin);
      Console.WriteLine(transactionLog.ToString());
      _logger.Log(transactionLog);
      if (transactionLog.Message.Contains("Successfuly"))
      {
        Console.WriteLine("Transaction was successfull. Saving Database with new account information");
        _database.UpdateAccountDatabase(transaction.SourceAccount);
      }


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
