using Dynamic.Decorator.UnitTesting.Playground.AmbientTransaction.Entity;
using System.Data.Entity;

namespace Dynamic.Decorator.UnitTesting.Playground.AmbientTransaction
{
	public class Context : DbContext
	{
		public DbSet<Square> SquareSet { get; set; }

		public DbSet<Rectangle> RectangleSet { get; set; }

		public Context()
			: base("Data Source=ALEX\\SQLEXPRESS2012;Initial Catalog=AmbientTransaction_Test;Integrated Security=True")
		{
			Database.Log = (msg) => { System.Diagnostics.Debug.WriteLine(msg); };

			Database.Initialize(true);
		}
	}
}