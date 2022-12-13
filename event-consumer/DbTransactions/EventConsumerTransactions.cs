using System;
using System.Text;
using event_consumer.BackgroundTask;
using event_consumer.Models;
using event_consumer.Settings;
using library.Adapter;
using library.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;

namespace event_consumer.DbTransactions
{
	public class EventConsumerTransactions : ITransaction<ApplicationContext>
	{
		private readonly ILoggerAdapter<EventConsumerService> _logger;
		private readonly Mqtt _options;
		private Analytics analytics;

		public EventConsumerTransactions(
			ILoggerAdapter<EventConsumerService> logger,
			Mqtt options)
		{
			_logger = logger;
			_options = options;
		}

		public async Task Run(ApplicationContext context)
		{

        }

		private async Task SubscribeMessageAsync(IMqttClient client)
		{
		}
	}
}

