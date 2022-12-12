using System;
using Microsoft.EntityFrameworkCore;

namespace event_consumer.Models
{
	public class ApplicationContext : DbContext
	{
		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
		{
		}

		public virtual DbSet<Analytics> Analytics { get; set; } = null!;

	}
}

