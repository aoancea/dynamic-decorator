using Dynamic.Decorator.UnitTesting.Playground.AmbientTransaction.Entity;
using System.Data.Entity;

namespace Dynamic.Decorator.UnitTesting.Playground.AmbientTransaction
{
	public class Context : DbContext
	{
		public DbSet<Square> SquareSet { get; set; }

		public DbSet<Rectangle> RectangleSet { get; set; }

		public Context()
			: base("")
		{
			Database.Log = (msg) => { System.Diagnostics.Debug.WriteLine(msg); };
		}
	}
}