using System;
using System.Text;
using event_consumer.BackgroundTask;
using event_consumer.Models;
using event_consumer.Settings;
using library.Adapter;
using library.Helper;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;

namespace event_consumer.DbTransactions
{
	public class EventConsumerTransactions : ITransaction<ApplicationContext>
	{
		private readonly ILoggerAdapter<EventConsumerService> _logger;
		private readonly MqttFactory _mqttFactory;
		private readonly Mqtt _options;

		public EventConsumerTransactions(
			ILoggerAdapter<EventConsumerService> logger,
			MqttFactory mqttFactory,
			Mqtt options)
		{
			_logger = logger;
			_mqttFactory = mqttFactory;
			_options = options;
		}

		public async Task Run(ApplicationContext context)
		{
			IMqttClient client = _mqttFactory.CreateMqttClient();

			if (client.IsConnected)
			{
				await SubscribeMessageAsync(client);
			}

			var options = new MqttClientOptionsBuilder()
				.WithClientId(Guid.NewGuid().ToString())
				.WithTcpServer(_options.Url, _options.Port)
				.WithCleanSession()
				.Build();

			await client.ConnectAsync(options);


			await SubscribeMessageAsync(client);

		}

		private static async Task SubscribeMessageAsync(IMqttClient client)
		{
			var topic = new MqttTopicFilterBuilder()
				.WithTopic("/analytics")
				.Build();

			await client.SubscribeAsync(topic);

			client.ApplicationMessageReceivedAsync += Client_ApplicationMessageReceivedAsync;
		}

		private static Task Client_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
		{
			Console.WriteLine($"Received message : {Encoding.UTF8.GetString(arg.ApplicationMessage.Payload)}");
			return null;
		}
	}
}

