using GroupProject.App.BankManagement.Account;

namespace GroupProject.App.BankManagement.Interfaces
{
    public interface ITransaction
    {
        void DisplayRentFromDeposit(decimal depositAmount, int timeFrame);
        void DepositMoney(decimal depositAmount, AccountBase userAccount);
        void WithdrawMoney(decimal withdrawAmount, AccountBase userAccount);
        void TransferMoney(decimal transferAmount, AccountBase userAccount, AccountBase targetAccount);
    }
}
