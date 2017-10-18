using Common;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace BankAccountManager
{
	class Receive
	{
		private static IModel channel;
		static void Main(string[] args)
		{
			Console.Title = "Bank Account Manager";

			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			using (channel = connection.CreateModel())
			{
				QueueFactory.MakePublishSubscribeAccountsQueue(channel);
				//QueueFactory.MakeRollbackQueue(channel);

				Console.WriteLine(" [*] Waiting for transactions.");

				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += MessageReceiver;
				channel.BasicConsume(queue: "bank",
									 autoAck: true,
									 consumer: consumer);

				//channel.BasicConsume(queue: "rollback",
				//					 autoAck: true,
				//					 consumer: consumer);

				Console.WriteLine(" Press [enter] to exit.");
				Console.ReadLine();
			}
		}

		private static void MessageReceiver(Object model, BasicDeliverEventArgs eventArgs)
		{
			Transaction transaction = null;
			try
			{
				var messageBody = eventArgs.Body;
				var json = Encoding.UTF8.GetString(messageBody);
				transaction = JsonConvert.DeserializeObject<Transaction>(json);

				Console.WriteLine("Received: {0} {1}", transaction.AccountHolder, transaction.Amount);
				Bank.ProcessTransaction(transaction);

			}
			catch (InsufficientFundsException ex)
			{
				PrintException(ex);
				//Retry(eventArgs.DeliveryTag);
				//LimitedRetry(transaction);
				//SendToDebtCollector(transaction);
			}
			catch (Exception ex)
			{
				PrintException(ex);
			}
		}


		private static void Retry(ulong deliveryTag)
		{
			channel.BasicNack(deliveryTag, false, true);
		}

		private static void LimitedRetry(Transaction transaction)
		{
			throw new NotImplementedException();
			var messageBody = transaction.ToByteArray();

			channel.BasicPublish(exchange: "bank",
								 routingKey: "bank",
								 basicProperties: null,
								 body: messageBody);
		}

		private static void SendToDebtCollector(Transaction transaction)
		{
			var messageBody = transaction.ToByteArray();

			channel.BasicPublish(exchange: "dead-letter-exchange",
								 routingKey: "insufficient.funds.errors",
								 basicProperties: null,
								 body: messageBody);
		}

		private static void PrintException(Exception ex)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(ex.Message);
			Console.ResetColor();
		}
	}
}
