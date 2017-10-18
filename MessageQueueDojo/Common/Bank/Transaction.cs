using System;
using System.Text;
using Newtonsoft.Json;

namespace Common
{
	public class Transaction
	{
		public Guid TransactionId { get; set; }
		public string AccountHolder { get; set; }
		public decimal Amount { get; set; }
		public int NumberOfFailures { get; set; }

		public bool IsFraud { get; set; }

		public Transaction(string accountHolder, decimal amount)
		{
			this.TransactionId = Guid.NewGuid();
			this.AccountHolder = accountHolder;
			this.Amount = amount;
		}

		public byte[] ToByteArray()
		{
			string json = JsonConvert.SerializeObject(this);
			byte[] messageBody = Encoding.UTF8.GetBytes(json);
			return messageBody;
		}
	}
}
