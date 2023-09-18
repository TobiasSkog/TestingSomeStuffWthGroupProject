using GroupProject.App.BankManagement.Account;

namespace GroupProject.App.BankManagement.Interfaces
{
  public interface ITransaction
  {
    void DisplayRentFromDeposit(decimal depositAmount, int timeSpan);
    void DepositMoney(decimal depositAmount);
    void WithdrawMoney(decimal withdrawAmount);
    void TransferMoney(decimal transferAmount, AccountBase targetAccount);
  }
}
