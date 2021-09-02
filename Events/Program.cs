using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Events
{
    class Program
    {
        static void Main(string[] args)
        {
            BankAccount bankAccount = new BankAccount(100);
            bankAccount.TransactionActed += BankAccount_TransactionActed;
            bankAccount.Notify += BankAccount_Notify;

            bankAccount.Transaction(TypeOfTransaction.Put, 200);
            Thread.Sleep(2000);
            bankAccount.Transaction(TypeOfTransaction.Withdraw, 100);
            bankAccount.Transaction(TypeOfTransaction.Withdraw, 500);


            Console.ReadKey();
        }

        private static void BankAccount_Notify(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine();
        }

        private static int BankAccount_TransactionActed(object sender, TransactionActedEventArgs e)
        {
            switch (e.TypeOfTransaction)
            {
                case TypeOfTransaction.Put:
                    Console.WriteLine($"На счет положено: {e.Sum}\nВремя транзакции: {e.TimeOfTransaction}");
                    break;
                case TypeOfTransaction.Withdraw:
                    Console.WriteLine($"Со счета снято: {e.Sum}\nВремя транзакции: {e.TimeOfTransaction}");
                    break;
            }

            return e.Sum;
        }
    }
}
