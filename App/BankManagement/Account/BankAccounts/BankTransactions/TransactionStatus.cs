using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.App.BankManagement.Account.BankAccounts.BankTransactions
{
    public enum TransactionStatus
    {
        Success,
        Denied,
        Pending,
        Failed,
        DestinationAccountNotFound,
        BalanceTooLowForLoan,
        BalanceTooLowForWithdrawal,
        BalanceTooLowForTransfer,
        DepositIsANegativeValue,
        WithdrawSuccess,
        DepositSuccess,
        TransferSuccess,
        WithdrawalLimit,
        LoanSuccess
    }
}
