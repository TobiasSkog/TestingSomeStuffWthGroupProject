using GroupProject.App.BankManagement.User;
using GroupProject.App.BankManagement.User.Customer;

namespace GroupProject.App.BankManagement.Account.BankAccounts.BankTransactions
{
    public class Transaction
    {
        public UserCustomer User { get; private set; }
        public UserCustomer TargetUser { get; private set; }
        public decimal Balance { get; private set; }
        public CurrencyType CurrencyType { get; private set; }
        public AccountBase SourceAccount { get; private set; }
        public AccountBase TargetAccount { get; private set; }
        public AccountStatus AccountStatus { get; private set; }
        public TransactionStatus UserTransactionStatus { get; private set; }
        public string TransactionLog { get; set; }
        public Transaction(
            UserCustomer user,
            decimal balance,
            CurrencyType currencyType,
            AccountBase sourceAccount,
            AccountBase targetAccount,
            AccountStatus accountStatus,
            TransactionStatus transactionStatus = TransactionStatus.Pending)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            Balance = balance;
            CurrencyType = currencyType;
            SourceAccount = sourceAccount;
            TargetAccount = targetAccount;
            AccountStatus = accountStatus;
            UserTransactionStatus = transactionStatus;
        }
        public void AddTransactionLog(string transactionLog)
        {
            TransactionLog = transactionLog;
        }

        public void ProcessTransaction(UserType userType, TransactionType transactionType)
        {
            if (userType != UserType.Admin)
            {
                throw new InvalidOperationException("Only admins can process transactions.");
            }
            string transactionLog = string.Empty;

            switch (transactionType)
            {
                case TransactionType.Withdraw:
                    SourceAccount.WithdrawMoney(this);

                    transactionLog += $"Withdrawal - Status: {UserTransactionStatus}. Withdrawal amount:{Balance}{CurrencyType}. Source account number: {SourceAccount.GetAccountNumber(UserType.Admin)}.";
                    break;

                case TransactionType.Deposit:
                    SourceAccount.DepositMoney(this);

                    transactionLog += $"Deposit - Status: {UserTransactionStatus}. Deposit amount:{Balance}{CurrencyType}. Destination account number: {SourceAccount.GetAccountNumber(UserType.Admin)}.";
                    break;

                case TransactionType.TransferOwnAccount:
                    TargetAccount = SourceAccount;
                    SourceAccount.TransferMoney(this);
                    transactionLog += $"Transfer - Status: {UserTransactionStatus}. Transfer amount:{Balance}{CurrencyType}. Source account number: {SourceAccount.GetAccountNumber(UserType.Admin)}. Destination account number: {TargetAccount.GetAccountNumber(UserType.Admin)}.";
                    break;

                case TransactionType.TransferTargetAccount:
                    SourceAccount.TransferMoney(this);

                    transactionLog += $"Transfer - Status: {UserTransactionStatus}. Transfer amount:{Balance}{CurrencyType}. Source account number: {SourceAccount.GetAccountNumber(UserType.Admin)}. Destination account number: {TargetAccount.GetAccountNumber(UserType.Admin)}.";
                    break;

                //TODO: IMPLEMENT TRASNACTIONTYPE.RENT IN TRANSACTIONS.CS THIS IS NOT DONE!
                case TransactionType.Rent:
                    SourceAccount.DisplayRentFromDeposit(this);

                    transactionLog += $"Rent - Status: {UserTransactionStatus}. Rent amount:{Balance}{CurrencyType}. Destination number: {TargetAccount.GetAccountNumber(UserType.Admin)}.";
                    break;

                default:
                    throw new ArgumentException("Invalid transaction type.");
            }
            AddTransactionLog(transactionLog);
        }
    }
}
