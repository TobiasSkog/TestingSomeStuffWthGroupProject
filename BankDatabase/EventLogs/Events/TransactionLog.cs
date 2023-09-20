using Newtonsoft.Json;
using System.Transactions;

namespace GroupProject.BankDatabase.EventLogs.Events
{
  public class TransactionLog : EventLog
  {
    public decimal Amount { get; set; }
    public string SourceAccount { get; set; }
    public string TargetAccount { get; set; }

    public TransactionLog()
    {
      EventCategory = EventCategorys.Transaction;
    }
    public TransactionLog(string username, string message, decimal amount, string sourceAccount, string targetAccount = "") : base(username)
    {
      EventCategory = EventCategorys.Transaction;
      Message = $"{message} for {amount}. From {sourceAccount} to {targetAccount}";
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
    public TransactionLog(DateTime timestamp, EventCategorys category, string message, string username, decimal amount, string sourceAccount, Exception ex = null, string targetAccount = "")
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