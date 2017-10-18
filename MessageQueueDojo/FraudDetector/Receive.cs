using Common;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;


namespace FraudDetector
{
	class Receive
	{
		private static IModel channel;
		public static void Main()
		{
			Console.Title = "Fraud Detector";

			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			using (channel = connection.CreateModel())
			{
				QueueFactory.MakePublishSubscribeFraudQueue(channel);

				Console.WriteLine(" [*] Waiting for transactions.");

				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += MessageReceiver;
				channel.BasicConsume(queue: "fraud",
									 autoAck: true,
									 consumer: consumer);

				Console.WriteLine(" Press [enter] to exit.");
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
				CheckTransaction(transaction);
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.Message);
				Console.ResetColor();
			}
		}

		private static void CheckTransaction(Transaction transaction)
		{
			throw new NotImplementedException("Exercise 3");
		}

		private static void SendToFraudHelpDesk(Transaction transaction)
		{
			var messageBody = transaction.ToByteArray();

			throw new NotImplementedException("Exercise 3");

			//exchange: "dead-letter-exchange",
			//routingKey: "fraud.errors"
		}

		private static void SendToBankAccountManager(Transaction transaction)
		{
			var messageBody = transaction.ToByteArray();

			channel.BasicPublish(exchange: "rollback-exchange",
								 routingKey: "rollback",
								 basicProperties: null,
								 body: messageBody);
		}
	}
}
