using GroupProject.App.BankManagement.Account;
using GroupProject.App.BankManagement.Account.BankAccounts.BankTransactions;
using GroupProject.BankDatabase.EventLogs.Events;
using Newtonsoft.Json;

namespace GroupProject.App.BankManagement.User.Bank
{
  public class Bank : UserBase
  {
    public Bank()
    {

    }
    [JsonConstructor]
    public Bank(string firstName, string lastName, string username, string salt, string hashedPassword, sbyte remainingAttempts, string userId, string socialSecurityNumber, DateTime dateOfBirth, UserType userType, UserStatuses userStatus, List<string> accountIds, List<string> logIds)
    {
      FirstName = firstName;
      LastName = lastName;
      Username = username;
      Salt = salt;
      HashedPassword = hashedPassword;
      RemainingAttempts = remainingAttempts;
      UserId = userId;
      SocialSecurityNumber = socialSecurityNumber;
      DateOfBirth = dateOfBirth;
      UserType = userType;
      UserStatus = userStatus;
      AccountIds = accountIds;
      LogIds = logIds;
    }

    public override (UserChoice Choice, AccountTransaction Transaction, TransactionLog Log) LoanMoney(List<AccountBase> sourceAccounts)
    {
      throw new NotImplementedException();
    }

    public override (UserChoice Choice, AccountTransaction Transaction, TransactionLog Log) MakeDeposit(List<AccountBase> sourceAccounts)
    {
      throw new NotImplementedException();
    }

    public override (UserChoice Choice, AccountTransaction Transaction, TransactionLog Log) MakeTransfer(List<AccountBase> sourceAccounts, AccountBase targetAccounts, UserBase targetUser)
    {
      throw new NotImplementedException();
    }

    public override (UserChoice Choice, AccountTransaction Transaction, TransactionLog Log) MakeWithdrawal(List<AccountBase> sourceAccounts)
    {
      throw new NotImplementedException();
    }
  }
}
