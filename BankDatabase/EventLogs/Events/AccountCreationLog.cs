using Newtonsoft.Json;

namespace GroupProject.BankDatabase.EventLogs.Events
{
  public class AccountCreationLog : EventLog
  {
    public AccountCreationLog()
    {
      EventCategory = EventCategorys.AccountCreation;
    }
    public AccountCreationLog(string username, string message) : base(username)
    {
      EventCategory = EventCategorys.AccountCreation;
      Message = message;
    }

    [JsonConstructor]
    public AccountCreationLog(DateTime timestamp, EventCategorys category, string message, string username, Exception ex = null)
    {
      Timestamp = timestamp;
      EventCategory = category;
      Message = message;
      Username = username;
      Ex = ex;
    }
  }
}