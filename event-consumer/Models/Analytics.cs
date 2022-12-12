using System;
namespace event_consumer.Models
{
	public class Analytics
	{
		public long Id { get; set; }
		public long UserId { get; set; }
		public string Service { get; set; }
		public DateTime TimeStamp { get; set; }
	}
}

