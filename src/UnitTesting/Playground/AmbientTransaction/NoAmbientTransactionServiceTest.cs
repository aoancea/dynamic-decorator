using NUnit.Framework;

namespace Dynamic.Decorator.UnitTesting.Playground.AmbientTransaction
{
	[TestFixture]
	[Ignore("Don't run in CI")]
	public class NoAmbientTransactionServiceTest
	{
		private Context readContext;

		[SetUp]
		public void SetUp()
		{
			readContext = new Context();
		}

		[TearDown]
		public void TearDown()
		{
			readContext.Database.ExecuteSqlCommand("DELETE FROM [dbo].[Squares]");
			readContext.Database.ExecuteSqlCommand("DELETE FROM [dbo].[Rectangles]");

			readContext.Dispose();
		}

		[Test]
		public void InsertSquare_SquareInserted()
		{
		}


		[Test]
		public void InsertRectangle_RectangleInserted()
		{
		}

		[Test]
		public void InsertSquareAndRectangle_SquareAndRectableAreInsertedWhenNoErrorOccurs()
		{
		}
	}
}