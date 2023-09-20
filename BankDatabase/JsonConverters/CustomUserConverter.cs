using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Admin;
using GroupProject.App.BankManagement.User.Customer;
using GroupProject.BankDatabase.EventLogs.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class CustomUserConverter : JsonConverter<UserBase>
{
  public override UserBase? ReadJson(JsonReader reader, Type objectType, UserBase? existingValue, bool hasExistingValue, JsonSerializer serializer)
  {
    JObject jsonObject = JObject.Load(reader);
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
    var json = new JObject(
      new JProperty("FirstName", value.FirstName),
      new JProperty("LastName", value.LastName),
      new JProperty("Username", value.Username),
      new JProperty("Salt", value.Salt),
      new JProperty("HashedPassword", value.HashedPassword),
      new JProperty("RemainingAttempts", value.RemainingAttempts),
      new JProperty("UserId", value.UserId),
      new JProperty("SocialSecurityNumber", value.SocialSecurityNumber),
      new JProperty("DateOfBirth", value.DateOfBirth),
      new JProperty("UserType", value.UserType),
      new JProperty("UserStatus", value.UserStatus),
      new JProperty("AccountIds", value.AccountIds),
      new JProperty("LogIds", value.LogIds)
      );
    json.WriteTo(writer);
  }
}