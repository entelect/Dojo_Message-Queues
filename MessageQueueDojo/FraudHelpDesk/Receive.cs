using System;
using System.Text;
using Common;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FraudHelpDesk
{
	class Receive
	{
		public static void Main()
		{
			Console.Title = "Fraud Help Desk";

			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				QueueFactory.MakeFraudHelpDeskQueue(channel);

				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += MessageReceiver;
				channel.BasicConsume(queue: "fraudhelpdesk",
										autoAck: true,
										consumer: consumer);

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
				Console.WriteLine("Received Fraudulent Transaction: {0} {1}", transaction.AccountHolder, transaction.Amount);

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
