using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.Account.BankAccounts;
using GroupProject.BankDatabase.EventLogs;
using GroupProject.BankDatabase.EventLogs.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Nodes;


namespace GroupProject.BankDatabase.JsonConverters
{
  public class CustomLogConverter : JsonConverter<EventLog>
  {

    public override EventLog? ReadJson(JsonReader reader, Type objectType, EventLog existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
      JObject jsonObject = JObject.Load(reader);

      string? category = jsonObject["EventCategory"]?.Value<string>();

      switch (category)
      {
        case "AccountCreation":
          return jsonObject.ToObject<AccountCreationLog>();
        case "Transaction":
          return jsonObject.ToObject<TransactionLog>();
        case "Connection":
          return jsonObject.ToObject<ConnectionLog>();
        default:
          return jsonObject.ToObject<EventLog>();
      }
    }

    public override void WriteJson(JsonWriter writer, EventLog value, JsonSerializer serializer)
    {
      // Define the structure of the JSON object as you need it
      var json = new JObject(
          new JProperty("EventCategory", value.EventCategory.ToString()),
          new JProperty("Message", value.Message),
          new JProperty("Timestamp", value.Timestamp),
          new JProperty("Username", value.Username)
          );

      json.WriteTo(writer);
    }

  }
}