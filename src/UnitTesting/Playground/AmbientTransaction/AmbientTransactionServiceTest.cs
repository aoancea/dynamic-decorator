using NUnit.Framework;

namespace Dynamic.Decorator.UnitTesting.Playground.AmbientTransaction
{
	[TestFixture]
	[Ignore("Don't run in CI")]
	public class AmbientTransactionServiceTest
	{
		private Context readContext;

		[SetUp]
		public void SetUp()
		{
			readContext = new Context("readContext");
		}

		[TearDown]
		public void TearDown()
		{
			readContext.Database.ExecuteSqlCommand("DELETE FROM [dbo].[Squares]");
			readContext.Database.ExecuteSqlCommand("DELETE FROM [dbo].[Rectangles]");

			readContext.Dispose();
		}

		[Test]
		public void InsertSquare()
		{

		}

		[Test]
		public void InsertRectangle()
		{

		}

		[Test]
		public void InsertSquareAndRectangle()
		{

		}
	}
}