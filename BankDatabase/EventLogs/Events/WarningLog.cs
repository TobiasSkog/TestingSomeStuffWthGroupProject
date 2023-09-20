using Newtonsoft.Json;

namespace GroupProject.BankDatabase.EventLogs.Events
{
  public class WarningLog : EventLog
  {
    public WarningLog()
    {
      EventCategory = EventCategorys.Warning;
    }
    public WarningLog(string username, string message) : base(username)
    {
      EventCategory = EventCategorys.Warning;
      Message = message;

    }

    [JsonConstructor]
    public WarningLog(DateTime timestamp, EventCategorys category, string message, string username, Exception ex = null)
    {
      Timestamp = timestamp;
      EventCategory = category;
      Message = message;
      Username = username;
      Ex = ex;
    }
  }
}
