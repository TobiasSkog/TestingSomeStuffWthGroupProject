using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.App.BankManagement.Account.BankAccounts.BankTransactions
{
    public enum TransactionType
    {
        Withdraw,
        Deposit,
        TransferOwnAccount,
        TransferTargetAccount,
        Rent
    }
}
