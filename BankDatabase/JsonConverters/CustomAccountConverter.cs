using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.Account.BankAccounts;
using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Admin;
using GroupProject.App.BankManagement.User.Customer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace GroupProject.BankDatabase.JsonConverters
{
  public class CustomAccountConverter : JsonConverter<AccountBase>
  {
    public override AccountBase? ReadJson(JsonReader reader, Type objectType, AccountBase? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
      //// Check if objectType is CheckingsAccount or SavingsAccount
      //if (objectType == typeof(CheckingsAccount))
      //{
      //  return (AccountBase)serializer.Deserialize<CheckingsAccount>(reader);
      //}
      //else if (objectType == typeof(SavingsAccount))
      //{
      //  return (AccountBase)serializer.Deserialize<SavingsAccount>(reader);
      //}

      //// If objectType is AccountBase or another base class, deserialize it as such
      //return (AccountBase)serializer.Deserialize<AccountBase>(reader);

      JObject jsonObject = JObject.Load(reader);
      int typeAsInt = jsonObject.GetValue("AccountType").ToObject<int>();

      Type targetType = typeAsInt switch
      {
        0 => typeof(CheckingsAccount),
        1 => typeof(SavingsAccount),
        _ => throw new NotSupportedException("Unkown AccountType")
      };

      AccountBase result = (AccountBase)Activator.CreateInstance(targetType);
      serializer.Populate(jsonObject.CreateReader(), result);
      return result;
    }

    public override void WriteJson(JsonWriter writer, AccountBase? value, JsonSerializer serializer)
    {
      // Define the structure of the JSON object as you need it
      var json = new JObject(
      new JProperty("AccountId", value.AccountId),
      new JProperty("AccountStatus", value.AccountStatus),
      new JProperty("AccountType", value.AccountType),
      new JProperty("AccountNumber", value.AccountNumber),
      new JProperty("Balance", value.Balance),
      new JProperty("CurrencyType", value.CurrencyType)

      );

      json.WriteTo(writer);
    }
  }
}
