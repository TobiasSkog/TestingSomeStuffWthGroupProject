using Newtonsoft.Json;

namespace GroupProject.BankDatabase.EventLogs.Events
{
  public class AccountCreationLog : EventLog
  {
    public AccountCreationLog()
    {
      EventCategory = EventCategory.AccountCreation;
    }
    public AccountCreationLog(string username, string message) : base(username)
    {
      EventCategory = EventCategory.AccountCreation;
      Message = message;
    }

    [JsonConstructor]
    public AccountCreationLog(DateTime timestamp, EventCategory category, string message, string username, Exception ex = null)
    {
      Timestamp = timestamp;
      EventCategory = category;
      Message = message;
      Username = username;
      Ex = ex;
    }
  }
}