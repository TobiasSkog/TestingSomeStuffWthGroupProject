namespace GroupProject.App.BankManagement.Interfaces
{
    public interface ITransaction
    {
        void DepositMoney(decimal balance, string accNumber);
        void WithdrawMoney(decimal balance, string accNumber);
        void TransferMoney(decimal balance, string userAccNumber, string destAccNumber);
        void CheckBalance(string accNumber);
    }
}
