using Dynamic.Decorator.UnitTesting.Playground.AmbientTransaction.Entity;
using System.Data.Entity;

namespace Dynamic.Decorator.UnitTesting.Playground.AmbientTransaction
{
	public class Context : DbContext
	{
		public DbSet<Square> SquareSet { get; set; }

		public DbSet<Rectangle> RectangleSet { get; set; }

		public Context(string contextName)
			: base("Data Source=ALEX\\SQLEXPRESS2012;Initial Catalog=AmbientTransaction_Test;Integrated Security=True")
		{
			Database.Initialize(true);

			Database.Log = (msg) =>
			{
				if (Database.CurrentTransaction != null)
				{
					System.Data.SqlClient.SqlTransaction transaction = (System.Data.SqlClient.SqlTransaction)Database.CurrentTransaction.UnderlyingTransaction;

					if (System.Transactions.Transaction.Current != null)
					{
						System.Transactions.Transaction ambientTransation = System.Transactions.Transaction.Current;
					}
				}

				if (msg == "\r\n")
					System.Console.WriteLine(msg);
				else
				{
					System.Console.WriteLine(string.Format("{0} - {1}", contextName, msg));

					if (msg.Contains("Closed connection"))
						System.Console.WriteLine("=======================================================================================" + System.Environment.NewLine + System.Environment.NewLine + System.Environment.NewLine);
				}
			};
		}
	}
}