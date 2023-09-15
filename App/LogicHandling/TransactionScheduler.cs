﻿using GroupProject.App.BankManagement.Account.BankAccounts.BankTransactions;
using GroupProject.App.BankManagement.User;

namespace GroupProject.App.LogicHandling
{
    //TODO: TRANSACTIONSCHEDULER CHANGE TIMESPAN.FROMSECONDS TO MINUTES!!!!!!!!

    public class TransactionScheduler
    {
        private Timer _timer;
        private List<Transaction> _pendingTransactions = new List<Transaction>();
        public TransactionScheduler(int timerDelayMinutes)
        {

            _timer = new Timer(ExecuteScheduledTransaction, null, TimeSpan.Zero, TimeSpan.FromSeconds(timerDelayMinutes));
        }

        private void ExecuteScheduledTransaction(object state)
        {
            Console.WriteLine("Executing scheduled transaction at: " + DateTime.Now);
            if (_pendingTransactions.Count > 0)
            {
                foreach (var transaction in _pendingTransactions)
                {
                    ProcessTransaction(transaction);
                }
            }

            _pendingTransactions.Clear();
        }

        private void ProcessTransaction(Transaction transaction)
        {
            //transaction.User
            transaction.User.Accounts(UserType.Admin);


        }

        public void QueueTransaction(Transaction transaction)
        {
            _pendingTransactions.Add(transaction);
        }
        public void Stop()
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
}