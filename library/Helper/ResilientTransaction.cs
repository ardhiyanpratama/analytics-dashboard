using System;
using Microsoft.EntityFrameworkCore;

namespace library.Helper
{
	public class ResilientTransaction
	{
		private readonly DbContext _context;

		private ResilientTransaction(DbContext context) => _context = context ?? throw new ArgumentNullException(nameof(context));

		public static ResilientTransaction New(DbContext context) => new ResilientTransaction(context);

		public async Task ExecuteAsync(Func<Task> action)
		{
			//Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
			//See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
			var strategy = _context.Database.CreateExecutionStrategy();
			await strategy.ExecuteAsync(async () =>
			{
				using var transaction = _context.Database.BeginTransaction();
				try
				{
					await action();
					await transaction.CommitAsync();
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
					throw;
				}
				finally
				{
					await transaction.DisposeAsync();
				}
			});
		}
	}
}

