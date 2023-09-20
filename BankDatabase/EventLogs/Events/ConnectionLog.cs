using Newtonsoft.Json;

namespace GroupProject.BankDatabase.EventLogs.Events
{
  public class ConnectionLog : EventLog
  {
    public ConnectionLog()
    {
      EventCategory = EventCategorys.Connection;
    }
    public ConnectionLog(string username, string message) : base(username)
    {
      EventCategory = EventCategorys.Connection;
      Message = message;
    }

    [JsonConstructor]
    public ConnectionLog(DateTime timestamp, EventCategorys category, string message, string username, Exception ex = null)
    {
      Timestamp = timestamp;
      EventCategory = category;
      Message = message;
      Username = username;
      Ex = ex;
    }
  }
}