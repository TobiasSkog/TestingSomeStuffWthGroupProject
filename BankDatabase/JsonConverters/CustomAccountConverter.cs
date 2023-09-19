using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.Account.BankAccounts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace GroupProject.BankDatabase.JsonConverters
{
  public class CustomAccountConverter : JsonConverter<AccountBase>
  {
    public override AccountBase? ReadJson(JsonReader reader, Type objectType, AccountBase? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
      JObject jsonObject = JObject.Load(reader);

      // string typeName = jsonObject.GetValue("UserType").ToObject<string>();
      int typeAsInt = jsonObject.GetValue("AccountType").ToObject<int>();

      Type targetType = typeAsInt switch
      {
        0 => typeof(CheckingsAccount),
        1 => typeof(SavingsAccount),
        _ => throw new NotSupportedException("Unkown UserType")
      };

      AccountBase result = (AccountBase)Activator.CreateInstance(targetType);
      serializer.Populate(jsonObject.CreateReader(), result);
      return result;
    }

    public override void WriteJson(JsonWriter writer, AccountBase? value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }
  }
}
