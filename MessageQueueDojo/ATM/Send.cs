using Common;
using RabbitMQ.Client;
using System;

namespace ATM
{
	class Send
	{
		private static string accountHolderName = "Stacy";

		static void Main(string[] args)
		{
			Console.Title = "ATM";

			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				QueueFactory.MakeBankExchange(channel);

				Console.WriteLine("Press [enter] to exit.");
				SendMessages(channel);
			}
		}

		private static void SendMessages(IModel channel)
		{
			string input = Console.ReadLine();
			while (!string.IsNullOrEmpty(input))
			{
				var amount = Convert.ToDecimal(input);
				var transaction = new Transaction(accountHolderName, amount);
				var messageBody = transaction.ToByteArray();

				channel.BasicPublish(exchange: "bank",
									 routingKey: "",
									 basicProperties: null,
									 body: messageBody);
				Console.WriteLine("Sent {0} {1}", transaction.AccountHolder, transaction.Amount);
				input = Console.ReadLine();
			}
		}
	}
}
