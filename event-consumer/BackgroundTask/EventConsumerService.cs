using event_consumer.Models;
using event_consumer.Settings;
using library.Adapter;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using System;
using System.Text;

namespace event_consumer.BackgroundTask
{
	public class EventConsumerService : BackgroundService
	{
		private readonly ILoggerAdapter<EventConsumerService> _logger;
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public EventConsumerService(
			ILogger<EventConsumerService> logger,
			IServiceScopeFactory serviceScopeFactory)
		{
			_logger = new LoggerAdapter<EventConsumerService>(logger);
			_serviceScopeFactory = serviceScopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation($"Event Consumer Starting at : {DateTime.Now}");
			stoppingToken.Register(() => _logger.LogInformation($"Event consumer is stopping at : {DateTime.Now}"));

			await DoConsumeAsync(stoppingToken);

			_logger.LogInformation($"Event consumer is stopping at : {DateTime.Now}");
		}

		private async Task DoConsumeAsync(CancellationToken cancellationToken)
		{
			try
			{
				using var scope = _serviceScopeFactory.CreateScope();

				var context = scope.ServiceProvider.GetService<ApplicationContext>();
				var mqttOptions = scope.ServiceProvider.GetRequiredService<IOptions<Mqtt>>();

				var factory = new MqttFactory();
				using var client = factory.CreateMqttClient();

				var options = new MqttClientOptionsBuilder()
					.WithClientId(Guid.NewGuid().ToString())
					.WithTcpServer(mqttOptions.Value.Url, mqttOptions.Value.Port)
					.WithCleanSession()
					.Build();

				client.ApplicationMessageReceivedAsync += async e =>
				{
					Console.WriteLine($"Received message : {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");

					var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
					var jsonResult = JsonConvert.DeserializeObject<Analytics>(payload);
					jsonResult.TimeStamp = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc); ;

					await context.Analytics.AddAsync(jsonResult);
					await context.SaveChangesAsync();
				};

				client.ConnectedAsync += e =>
				{
					Console.WriteLine("Connected to mqtt broker");

					return Task.CompletedTask;
				};

				client.DisconnectedAsync += e =>
				{
					Console.WriteLine("Disconnected from mqtt broker");

					return Task.CompletedTask;
				};

				await client.ConnectAsync(options, cancellationToken);

				var topic = new MqttTopicFilterBuilder()
					.WithTopic("/analytics")
					.Build();

				await client.SubscribeAsync(topic, cancellationToken);

				while (!cancellationToken.IsCancellationRequested)
				{
					Console.WriteLine("Listening to mqtt broker");

					await Task.Delay(10000);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
		}
	}
}

