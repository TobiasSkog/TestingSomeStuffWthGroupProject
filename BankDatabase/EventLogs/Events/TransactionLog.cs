using Newtonsoft.Json;

namespace GroupProject.BankDatabase.EventLogs.Events
{
  public class TransactionLog : EventLog
  {
    public decimal Amount { get; set; }
    public string SourceAccount { get; set; }
    public string TargetAccount { get; set; }
    public TransactionLog()
    {
      EventCategory = EventCategory.Transaction;
    }
    public TransactionLog(string username, string message, decimal amount, string sourceAccount, string targetAccount = "") : base(username)
    {
      EventCategory = EventCategory.Transaction;
      Message = message;
      Amount = amount;
      SourceAccount = sourceAccount;
      if (targetAccount == "")
      {
        TargetAccount = sourceAccount;
      }

      else
      {
        TargetAccount = targetAccount;
      }
    }

    [JsonConstructor]
    public TransactionLog(DateTime timestamp, EventCategory category, string message, string username, decimal amount, string sourceAccount, Exception ex = null, string targetAccount = "")
    {
      Timestamp = timestamp;
      Username = username;
      EventCategory = category;
      Message = message;
      Amount = amount;
      SourceAccount = sourceAccount;
      if (targetAccount == "")
      {
        TargetAccount = sourceAccount;
      }

      else
      {
        TargetAccount = targetAccount;
      }
      Ex = ex;
    }
  }
}
//public class TransactionLog : EventLog
//{
//  public decimal Amount { get; set; }
//  public string SourceAccount { get; set; }
//  public string TargetAccount { get; set; }
//}