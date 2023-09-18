using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Admin;
using GroupProject.App.BankManagement.User.Customer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class CustomUserConverter : JsonConverter<UserBase>
{
  public override UserBase? ReadJson(JsonReader reader, Type objectType, UserBase? existingValue, bool hasExistingValue, JsonSerializer serializer)
  {
    JObject jsonObject = JObject.Load(reader);

    // string typeName = jsonObject.GetValue("UserType").ToObject<string>();
    int typeAsInt = jsonObject.GetValue("UserType").ToObject<int>();

    Type targetType = typeAsInt switch
    {
      0 => typeof(UserCustomer),
      1 => typeof(UserCustomer),
      2 => typeof(UserAdmin),
      _ => throw new NotSupportedException("Unkown UserType")
    };

    UserBase result = (UserBase)Activator.CreateInstance(targetType);
    serializer.Populate(jsonObject.CreateReader(), result);
    return result;
  }

  public override void WriteJson(JsonWriter writer, UserBase? value, JsonSerializer serializer)
  {
    throw new NotImplementedException();
  }
}