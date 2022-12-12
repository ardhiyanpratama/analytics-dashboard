using System;
using event_consumer.DbTransactions;
using event_consumer.Models;
using event_consumer.Settings;
using library.Adapter;
using library.Helper;
using Microsoft.Extensions.Options;
using MQTTnet;

namespace event_consumer.BackgroundTask
{
	public class EventConsumerService : BackgroundService
	{
		private readonly ILoggerAdapter<EventConsumerService> _logger;
		private readonly IServiceScopeFactory _serviceScopeFactory;
		private readonly Mqtt _options;

		public EventConsumerService(
			ILogger<EventConsumerService> logger,
			IServiceScopeFactory serviceScopeFactory,
			IOptions<Mqtt> options)
		{
			_logger = new LoggerAdapter<EventConsumerService>(logger);
			_serviceScopeFactory = serviceScopeFactory;
			_options = options.Value;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation($"Event Consumer Starting at : {DateTime.Now}");
			stoppingToken.Register(() => _logger.LogInformation($"Event consumer is stopping at : {DateTime.Now}"));
			while (!stoppingToken.IsCancellationRequested)
			{
				_logger.LogInformation($"Event Consumer Is Running Now at : {DateTime.Now}");
				await DoConsumeAsync();
				await Task.Delay((int)TimeSpan.FromSeconds(5).TotalMilliseconds, stoppingToken);
			}
			_logger.LogInformation($"Event consumer is stopping at : {DateTime.Now}");
		}

		private async Task DoConsumeAsync()
		{
			try
			{
				using var scope = _serviceScopeFactory.CreateScope();
				var context = scope.ServiceProvider.GetService<ApplicationContext>();
				var mqttFact = scope.ServiceProvider.GetService<MqttFactory>();

				await context!.DoTransactionAsync(new EventConsumerTransactions(_logger, mqttFact, _options));

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
		}
	}
}

