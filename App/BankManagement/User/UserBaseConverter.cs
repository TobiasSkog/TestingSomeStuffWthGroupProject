using GroupProject.App.BankManagement.User.Admin;
using GroupProject.App.BankManagement.User.Customer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace GroupProject.App.BankManagement.User
{
  public class UserBaseConverter : JsonConverter<UserBase>
  {
    public override UserBase ReadJson(JsonReader reader, Type objectType, UserBase existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
      // Load the JSON into a JObject to determine the actual type of the object
      var jsonObject = JObject.Load(reader);

      // Check if the "UserAccountType" property exists in the JSON
      if (jsonObject.TryGetValue("UserAccountType", out var userTypeToken) && userTypeToken.Type == JTokenType.Integer)
      {
        var userType = userTypeToken.ToObject<UserType>();

        // Create an instance of the concrete type based on the "UserAccountType"
        UserBase user;
        switch (userType)
        {
          case UserType.Customer:
            user = new UserCustomer();
            break;
          case UserType.Admin:
            user = new UserAdmin();
            break;
          default:
            throw new InvalidOperationException("Unknown UserType");
        }

        // Populate the user object from the JSON
        serializer.Populate(jsonObject.CreateReader(), user);

        return user;
      }

      throw new JsonSerializationException("UserAccountType is missing or not valid.");
    }

    public override void WriteJson(JsonWriter writer, UserBase value, JsonSerializer serializer)
    {
      var jsonObject = new JObject();

      // Use reflection to get all properties except Accounts and UserLog
      var properties = value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
          .Where(prop => prop.Name != "Accounts" && prop.Name != "UserLog");

      foreach (var prop in properties)
      {
        // Add the property to the JSON object
        jsonObject.Add(prop.Name, JToken.FromObject(prop.GetValue(value)));
      }

      // Write the JSON object to the writer
      jsonObject.WriteTo(writer);
    }
    //public override void WriteJson(JsonWriter writer, UserBase value, JsonSerializer serializer)
    //{
    //  var jsonObject = new JObject();

    //  // Use reflection to get all properties except Accounts and UserLog
    //  var properties = value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
    //      .Where(prop => prop.Name != "Accounts" && prop.Name != "UserLog");

    //  foreach (var prop in properties)
    //  {
    //    // Add the property to the JSON object
    //    jsonObject.Add(prop.Name, JToken.FromObject(prop.GetValue(value)));
    //  }

    //  // Write the JSON object to the writer
    //  jsonObject.WriteTo(writer);
    //}
  }
}