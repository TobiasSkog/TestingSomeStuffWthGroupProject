using GroupProject.BankDatabase.JsonConverters;
using Newtonsoft.Json;
using ValidationUtility;

namespace GroupProject.BankDatabase.EventLogs
{

  public enum EventCategorys
  {
    Connection,
    AccountCreation,
    Transaction,
    Warning
  }
  [JsonConverter(typeof(CustomLogConverter))]
  [JsonObject(MemberSerialization.OptIn)]
  public abstract class EventLog
  {
    [JsonProperty]
    public DateTime Timestamp { get; protected set; }
    [JsonProperty]
    public EventCategorys EventCategory { get; protected set; }
    [JsonProperty]
    public string Message { get; protected set; }
    [JsonProperty]
    public string Username { get; protected set; }
    [JsonProperty]
    public string LogID { get; protected set; }
    public Exception? Ex { get; protected set; }

    public EventLog()
    {
      LogID = StringValidationHelper.CreateRandomString(15);
    }
    public EventLog(string username)
    {
      Timestamp = DateTime.Now;
      Username = username;
      LogID = StringValidationHelper.CreateRandomString(15);

    }


    [JsonConstructor]
    public EventLog(DateTime timestamp, EventCategorys category, string message, string username, string logId = "", Exception ex = null)
    {
      Timestamp = timestamp;
      EventCategory = category;
      Message = message;
      Username = username;
      Ex = ex;
      LogID = logId;
    }
    public override string ToString()
    {
      return $"{Timestamp:yyyy-MM-dd HH:mm:ss} [{EventCategory}] - User: {Username} - {Message}";
    }
  }
}