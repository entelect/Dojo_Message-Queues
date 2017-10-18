using System;
using Common;

namespace Common
{
	public class BankAccount
	{
		public string AccountHolder { get; set; }
		public decimal Balance { get; set; }

		public BankAccount(string accountHolder, decimal balance)
		{
			this.AccountHolder = accountHolder;
			this.Balance = balance;
		}

		public void ProcessTransaction(decimal amount)
		{
			if (this.IsWithdrawal(amount))
			{
				this.AssertFundsAvailable(amount);
			}
			this.Balance += amount;
			Console.WriteLine(this.ToString());
		}

		public override string ToString()
		{
			return string.Format("{0} new balance: R{1}", this.AccountHolder, this.Balance);
		}

		private bool IsWithdrawal(decimal amount)
		{
			return amount < 0;
		}

		private void AssertFundsAvailable(decimal withdrawalAmount)
		{
			if (this.Balance >= Math.Abs(withdrawalAmount))
			{
				return;
			}

			throw new InsufficientFundsException();
		}
	}
}
