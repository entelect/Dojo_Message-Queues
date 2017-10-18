using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace Common
{
	public static class Bank
	{
		private static List<BankAccount> BankAccounts = new List<BankAccount>()
		{
			new BankAccount("Tim", 1000),
			new BankAccount("Susan", 1000),
			new BankAccount("Jeff", 1000),
			new BankAccount("Diana", 1000),
			new BankAccount("Hector", 1000),
			new BankAccount("Brooke", 1000),
			new BankAccount("Stanley", 1000),
			new BankAccount("Opal", 1000),
			new BankAccount("Darrel", 1000),
			new BankAccount("Lucille", 1000),
			new BankAccount("Joe", 1000),
			new BankAccount("Stacy", 1000),
			new BankAccount("Clay", 1000),
			new BankAccount("Ginger", 1000)
		};

		private static List<Guid> TransactionHistory = new List<Guid>();

		public static void ProcessTransaction(Transaction transaction)
		{
			var bankAccount = BankAccounts.Where(x => x.AccountHolder == transaction.AccountHolder).SingleOrDefault();
			if (bankAccount == null)
			{
				throw new BankAccountNotFoundException();
			}

			//AssertTransactionIsUnique(transaction.TransactionId);
			bankAccount.ProcessTransaction(transaction.Amount);
			TransactionHistory.Add(transaction.TransactionId);
		}

		public static void RollbackTransaction(Transaction transaction)
		{
			var bankAccount = BankAccounts.Where(x => x.AccountHolder == transaction.AccountHolder).SingleOrDefault();
			if (bankAccount == null)
			{
				throw new BankAccountNotFoundException();
			}

			if (transaction.Amount > 0)
			{
				bankAccount.ProcessTransaction(-1 * transaction.Amount);
			}
			else
			{
				Console.WriteLine(bankAccount.ToString());
			}
		}

		private static void AssertTransactionIsUnique(Guid transactionId)
		{
			if (TransactionHistory.Any(x => x == transactionId))
			{
				throw new DuplicateTransactionException();
			}
		}
	}
}
