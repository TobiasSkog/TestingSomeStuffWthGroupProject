using GroupProject.App.BankManagement.User.Admin;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.BankDatabase.EventLogs;
using GroupProject.BankDatabase.EventLogs.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace GroupProject.BankDatabase.JsonConverters
{
  public class CustomLogConverter : JsonConverter<EventLog>
  {
    public override EventLog? ReadJson(JsonReader reader, Type objectType, EventLog existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
      JObject jsonObject = JObject.Load(reader);
      int category = jsonObject.GetValue("EventCategory").ToObject<int>();

      Type targetType = category switch
      {
        0 => typeof(ConnectionLog),
        1 => typeof(AccountCreationLog),
        2 => typeof(TransactionLog),
        3 => typeof(WarningLog),
        _ => throw new NotSupportedException("Unkown EventCategory")
      };
      EventLog result = (EventLog)Activator.CreateInstance(targetType);
      serializer.Populate(jsonObject.CreateReader(), result);
      return result;
    }

    public override void WriteJson(JsonWriter writer, EventLog value, JsonSerializer serializer)
    {
      // Define the structure of the JSON object as you need it
      var json = new JObject(
          new JProperty("Username", value.Username),
          new JProperty("LogID", value.LogID),
          new JProperty("EventCategory", value.EventCategory),
          new JProperty("Message", value.Message),
          new JProperty("Timestamp", value.Timestamp)
          );

      json.WriteTo(writer);
    }

  }
}