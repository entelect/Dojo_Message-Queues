using Common;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace DebtCollector
{

	class Receive
	{
		public static void Main()
		{
			Console.Title = "Debt Collector";

			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				QueueFactory.MakeInsufficientFundsErrorQueue(channel);

				throw new NotImplementedException("Exercise 2: Need to consume from the queue");

				Console.WriteLine("Press [enter] to exit.");
				Console.ReadLine();
			}
		}


		private static void MessageReceiver(Object model, BasicDeliverEventArgs eventArgs)
		{
			try
			{
				var messageBody = eventArgs.Body;
				var json = Encoding.UTF8.GetString(messageBody);
				var transaction = JsonConvert.DeserializeObject<Transaction>(json);
				Console.WriteLine("Received Insufficient Funds Error: {0} {1}", transaction.AccountHolder, transaction.Amount);

			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.Message);
				Console.ResetColor();
			}
		}


	}
}
