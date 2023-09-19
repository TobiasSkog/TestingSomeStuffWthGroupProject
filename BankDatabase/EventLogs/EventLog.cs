using Newtonsoft.Json;

namespace GroupProject.BankDatabase.EventLogs
{

  public enum EventCategory
  {
    Connection,
    AccountCreation,
    Transaction
  }

  [JsonObject(MemberSerialization.OptIn)]
  public abstract class EventLog
  {
    [JsonProperty]
    public DateTime Timestamp { get; protected set; }
    [JsonProperty]
    public EventCategory EventCategory { get; protected set; }
    [JsonProperty]
    public string Message { get; protected set; }
    [JsonProperty]
    public string Username { get; protected set; }

    public Exception? Ex { get; protected set; }

    public EventLog()
    {

    }
    public EventLog(string username)
    {
      Timestamp = DateTime.Now;
      Username = username;
    }


    [JsonConstructor]
    public EventLog(DateTime timestamp, EventCategory category, string message, string username, Exception ex = null)
    {
      Timestamp = timestamp;
      EventCategory = category;
      Message = message;
      Username = username;
      Ex = ex;
    }
    public override string ToString()
    {
      return $"{Timestamp:yyyy-MM-dd HH:mm:ss} [{EventCategory}] - User: {Username} - {Message}";
    }
  }
}