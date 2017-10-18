using Common;
using RabbitMQ.Client;
using System;

namespace Demo
{
	class Program
	{
		private static string accountHolderName = "Stacy";

		static void Main(string[] args)
		{
			var factory = new ConnectionFactory() { HostName = "local" };
			factory.UserName = "dojo";
			factory.Password = "password";
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queue: "bank",
									 durable: false,
									 exclusive: false,
									 autoDelete: false,
									 arguments: null);

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

				channel.BasicPublish(exchange: "",
									 routingKey: "bank",
									 basicProperties: null,
									 body: messageBody);
				Console.WriteLine("Sent {0} {1}", transaction.AccountHolder, transaction.Amount);
				input = Console.ReadLine();
			}
		}
	}
}
