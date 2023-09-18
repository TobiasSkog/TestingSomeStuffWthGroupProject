using System.Runtime.Serialization;

namespace GroupProject.App.BankManagement.User
{
  public enum UserTypes
  {
    [EnumMember(Value = "NoUser")]
    NoUser = 0,
    [EnumMember(Value = "Customer")]
    Customer = 1,
    [EnumMember(Value = "Admin")]
    Admin = 2
  }
}
