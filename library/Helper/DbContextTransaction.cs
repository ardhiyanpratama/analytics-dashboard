using System;
using Microsoft.EntityFrameworkCore;

namespace library.Helper
{
	public interface ITransaction<T> where T : DbContext
	{
		Task Run(T context);
	}

	public interface ITransaction<Td, Tr>
			where Td : DbContext
			where Tr : class
	{
		Task<Tr> Run(Td context);
	}

	public static class DbContextTransactionExtensions
	{
		public static async Task DoTransactionAsync<T>(this T context, ITransaction<T> transaction) where T : DbContext
		{
			try
			{
				var strategy = context.Database.CreateExecutionStrategy();
				await strategy.Execute(async () =>
				{
					using var dbTransaction = context.Database.BeginTransaction();
					try
					{
						await transaction.Run(context);
						dbTransaction.Commit();
					}
					catch (Exception)
					{
						await dbTransaction.RollbackAsync();
						throw;
					}
					finally
					{
						await dbTransaction.DisposeAsync();
					}
				});
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static async Task<Tr> DoTransactionAsync<Td, Tr>(this Td context, ITransaction<Td, Tr> transaction)
				where Td : DbContext
				where Tr : class
		{
			try
			{
				var strategy = context.Database.CreateExecutionStrategy();
				Tr result = default;
				await strategy.Execute(async () =>
				{
					using var dbTransaction = context.Database.BeginTransaction();
					try
					{
						result = transaction.Run(context).Result;
						dbTransaction.Commit();
					}
					catch (Exception)
					{
						await dbTransaction.RollbackAsync();
						throw;
					}
					finally
					{
						await dbTransaction.DisposeAsync();
					}
				});
				return result;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}

