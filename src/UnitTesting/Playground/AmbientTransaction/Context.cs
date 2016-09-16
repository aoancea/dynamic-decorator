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
				string efTransactionIdentifier = string.Empty;
				if (Database.CurrentTransaction != null)
				{
					System.Data.SqlClient.SqlTransaction transaction = (System.Data.SqlClient.SqlTransaction)Database.CurrentTransaction.UnderlyingTransaction;

					efTransactionIdentifier = transaction.GetHashCode().ToString();
				}

				string localAmbientTransactionIdentifier = string.Empty;
				string distributedAmbientTransactionIdentifier = string.Empty;
				if (System.Transactions.Transaction.Current != null)
				{
					System.Transactions.Transaction ambientTransation = System.Transactions.Transaction.Current;

					localAmbientTransactionIdentifier = ambientTransation.TransactionInformation.LocalIdentifier;
					distributedAmbientTransactionIdentifier = ambientTransation.TransactionInformation.DistributedIdentifier.ToString();
				}

				if (msg == "\r\n")
					System.Console.WriteLine(msg);
				else
				{
					System.Console.WriteLine(string.Format("{0} {1} {2}", contextName, TransactionIdentifier(efTransactionIdentifier, localAmbientTransactionIdentifier, distributedAmbientTransactionIdentifier), msg));

					if (msg.Contains("Closed connection"))
						System.Console.WriteLine("=======================================================================================" + System.Environment.NewLine + System.Environment.NewLine + System.Environment.NewLine);
				}
			};
		}

		private string TransactionIdentifier(string efTransactionIdentifier, string localAmbientTransactionIdentifier, string distributedAmbientTransactionIdentifier)
		{
			if (string.IsNullOrEmpty(efTransactionIdentifier) && string.IsNullOrEmpty(localAmbientTransactionIdentifier) && string.IsNullOrEmpty(distributedAmbientTransactionIdentifier))
				return ":";

			if (!string.IsNullOrEmpty(efTransactionIdentifier))
				return string.Format("- [EF]{0} :", efTransactionIdentifier);

			if (!string.IsNullOrEmpty(localAmbientTransactionIdentifier) && !string.IsNullOrEmpty(distributedAmbientTransactionIdentifier))
				return string.Format("- [L]{0} <=> [D]{1} :", localAmbientTransactionIdentifier, distributedAmbientTransactionIdentifier);
			else if (!string.IsNullOrEmpty(localAmbientTransactionIdentifier))
				return string.Format("- [L]{0} :", localAmbientTransactionIdentifier);
			else
				return string.Format("- [D]{0} :", distributedAmbientTransactionIdentifier);
		}
	}
}