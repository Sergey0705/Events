using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public class BankAccount
    {
        int _sum;

        public BankAccount(int sum)
        {
            _sum = sum;
        }

        public void Transaction(TypeOfTransaction typeOfTransaction, int sum)
        {
   
            switch (typeOfTransaction)
            {
                case TypeOfTransaction.Withdraw when _sum < sum:
                    Notify?.Invoke(this, new AccountEventArgs($"Такой суммы нет на счете: {sum}", sum));
                    break;
                case TypeOfTransaction.Withdraw:
                    TransactionActedEventArgs args = CreateTransactionActedEventArgs(typeOfTransaction, sum);
                    _sum -= OnTransactionActed(args);
                    Notify?.Invoke(this, new AccountEventArgs($"Сумма на счете: {_sum}", _sum));
                    break;
                case TypeOfTransaction.Put:
                    args = CreateTransactionActedEventArgs(typeOfTransaction, sum);
                    _sum += OnTransactionActed(args);
                    Notify?.Invoke(this, new AccountEventArgs($"Сумма на счете: {_sum}", _sum));
                    break;
            }
  
        }

        protected TransactionActedEventArgs CreateTransactionActedEventArgs(TypeOfTransaction typeOfTransaction, int sum)
        {
            TransactionActedEventArgs args = new TransactionActedEventArgs();

            args.Sum = sum;
            args.TimeOfTransaction = DateTime.Now;
            args.TypeOfTransaction = typeOfTransaction;

            return args;
        }

        protected int OnTransactionActed(TransactionActedEventArgs e)
        {
            EventHandler<TransactionActedEventArgs> handler = TransactionActed;

            int result = 0;
            if (handler != null)
            {
                result = handler(this, e);
            }

            return result;
        }

        public delegate int EventHandler<TransactionActedEventArgs>(object sender, TransactionActedEventArgs e);
        public event EventHandler<TransactionActedEventArgs> TransactionActed;
        public delegate void AccountHandler(object sender, AccountEventArgs e);
        public event AccountHandler Notify;
    }

    public class TransactionActedEventArgs : EventArgs
    {
        public int Sum { get; set; }
        public DateTime TimeOfTransaction { get; set; }
        public TypeOfTransaction TypeOfTransaction { get; set; }
    }

    public class AccountEventArgs
    {
        public string Message { get; }
        public int Sum { get; }

        public AccountEventArgs(string mes, int sum)
        {
            Message = mes;
            Sum = sum;
        }
    }

    public enum TypeOfTransaction
    {
        Put,
        Withdraw
    }
}
