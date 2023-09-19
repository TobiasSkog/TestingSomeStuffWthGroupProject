using Newtonsoft.Json;

namespace GroupProject.BankDatabase.EventLogs.Events
{
  public class ConnectionLog : EventLog
  {
    public ConnectionLog()
    {
      EventCategory = EventCategory.Connection;
    }
    public ConnectionLog(string username, string message) : base(username)
    {
      EventCategory = EventCategory.Connection;
      Message = message;
    }

    [JsonConstructor]
    public ConnectionLog(DateTime timestamp, EventCategory category, string message, string username, Exception ex = null)
    {
      Timestamp = timestamp;
      EventCategory = category;
      Message = message;
      Username = username;
      Ex = ex;
    }
  }
}