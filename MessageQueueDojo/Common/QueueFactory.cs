using System;
using RabbitMQ.Client;

namespace Common
{
	public static class QueueFactory
	{
		public static void MakeProducerConsumerQueue(IModel channel)
		{
			channel.QueueDeclare(queue: "bank",
							     durable: false,
							     exclusive: false,
							     autoDelete: false,
							     arguments: null);
		}

		public static void MakeInsufficientFundsErrorQueue(IModel channel)
		{
			MakeDeadLetterExchange(channel);

			channel.QueueDeclare(queue: "insufficientfunds",
								 durable: false,
								 exclusive: false,
								 autoDelete: false,
								 arguments: null);

			//Exercise 2: Need to bind to the queue"
		}

		public static void MakeBankExchange(IModel channel)
		{
			channel.ExchangeDeclare(exchange: "bank", type: "fanout");
		}

		public static void MakePublishSubscribeAccountsQueue(IModel channel)
		{

			MakeBankExchange(channel);
			MakeInsufficientFundsErrorQueue(channel);
			channel.QueueDeclare(queue: "bank",
								 durable: false,
								 exclusive: false,
								 autoDelete: false,
								 arguments: null);
			channel.QueueBind(queue: "bank",
							  exchange: "bank",
							  routingKey: "accounts");
		}

		public static void MakePublishSubscribeFraudQueue(IModel channel)
		{

			MakeBankExchange(channel);
			MakeFraudHelpDeskQueue(channel);
			channel.QueueDeclare(queue: "fraud",
								 durable: false,
								 exclusive: false,
								 autoDelete: false,
								 arguments: null);
			channel.QueueBind(queue: "fraud",
							  exchange: "bank",
							  routingKey: "fraud");
		}

		public static void MakeFraudHelpDeskQueue(IModel channel)
		{

			MakeDeadLetterExchange(channel);

			channel.QueueDeclare(queue: "fraudhelpdesk",
								 durable: false,
								 exclusive: false,
								 autoDelete: false,
								 arguments: null);
			channel.QueueBind(queue: "fraudhelpdesk",
							  exchange: "dead-letter-exchange",
							  routingKey: "fraud.errors");
		}

		public static void MakeReportingQueue(IModel channel)
		{

			MakeDeadLetterExchange(channel);

			channel.QueueDeclare(queue: "reporting",
								 durable: false,
								 exclusive: false,
								 autoDelete: false,
								 arguments: null);
			throw new NotImplementedException("Exercise 4: need to bind to the queue. Think about routing keys");
		}

		private static void MakeDeadLetterExchange(IModel channel)
		{
			/*Exercise 4
				open rabbitmq cmd and execute the following commands:
					> rabbitmqctl stop_app
					> rabbitmqctl reset
					> rabbitmqctl start_app
			*/
			channel.ExchangeDeclare(exchange: "dead-letter-exchange", type: "fanout");
		}

		public static void MakeRollbackQueue(IModel channel)
		{
			channel.ExchangeDeclare(exchange: "rollback-exchange", type: "direct");
			channel.QueueDeclare(queue: "rollback",
								 durable: false,
								 exclusive: false,
								 autoDelete: false,
								 arguments: null);
			channel.QueueBind(queue: "rollback",
							  exchange: "rollback-exchange",
							  routingKey: "rollback");
		}
	}
}
