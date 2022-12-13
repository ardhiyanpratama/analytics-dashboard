using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace analytics_dashboard.Models
{
	public class Analytics
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long? Id { get; set; }
		public long? UserId { get; set; }
		public string? Service { get; set; }
		public DateTime? TimeStamp { get; set; }
	}
}

