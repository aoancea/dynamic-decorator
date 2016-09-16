using Dynamic.Decorator.UnitTesting.Playground.AmbientTransaction.Entity;
using Dynamic.Decorator.UnitTesting.Playground.AmbientTransaction.Repository;
using Dynamic.Decorator.UnitTesting.Playground.AmbientTransaction.Service;
using NUnit.Framework;
using System;
using System.Transactions;

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
		public void InsertSquare_SquareIsInsertedInsideTheAmbientTransaction()
		{
			ITransactionService ambientTransactionService = new AmbientTransactionService(new TransactionService(new SquareRepository(new Context("squareContext")), new RectangleRepository(new Context("rectangleContext"))));

			Square squareToInsert = new Square() { ID = Guid.NewGuid(), Name = "Square1" };

			ambientTransactionService.InsertSquare(squareToInsert);

			Square dbSquare = readContext.SquareSet.Find(squareToInsert.ID);

			Assert.AreNotEqual(squareToInsert, dbSquare);
			Assert.AreEqual(squareToInsert.ID, dbSquare.ID);
			Assert.AreEqual(squareToInsert.Name, dbSquare.Name);
		}

		[Test]
		public void InsertRectangle_RectangleIsInsertedInsideTheAmbientTransaction()
		{
			ITransactionService noAmbientTransactionService = new AmbientTransactionService(new TransactionService(new SquareRepository(new Context("squareContext")), new RectangleRepository(new Context("rectangleContext"))));

			Rectangle rectangleToInsert = new Rectangle() { ID = Guid.NewGuid(), Name = "Rectangle1" };

			noAmbientTransactionService.InsertRectangle(rectangleToInsert);

			Rectangle dbRectangle = readContext.RectangleSet.Find(rectangleToInsert.ID);

			Assert.AreNotEqual(rectangleToInsert, dbRectangle);
			Assert.AreEqual(rectangleToInsert.ID, dbRectangle.ID);
			Assert.AreEqual(rectangleToInsert.Name, dbRectangle.Name);
		}

		[Test]
		public void InsertSquareAndRectangle_WhenNoExceptionIsThrown_ThenAmbientTransactionIsCommitedAndSquareAndRectableAreInsertedInsideTheAmbientTransaction()
		{
			ITransactionService ambientTransactionService = new AmbientTransactionService(new TransactionService(new SquareRepository(new Context("squareContext")), new RectangleRepository(new Context("rectangleContext"))));

			Square squareToInsert = new Square() { ID = Guid.NewGuid(), Name = "Square1" };
			Rectangle rectangleToInsert = new Rectangle() { ID = Guid.NewGuid(), Name = "Rectangle1" };

			ambientTransactionService.InsertSquareAndRectangle(squareToInsert, rectangleToInsert);


			Square dbSquare = readContext.SquareSet.Find(squareToInsert.ID);
			Rectangle dbRectangle = readContext.RectangleSet.Find(rectangleToInsert.ID);

			Assert.AreNotEqual(squareToInsert, dbSquare);
			Assert.AreEqual(squareToInsert.ID, dbSquare.ID);
			Assert.AreEqual(squareToInsert.Name, dbSquare.Name);

			Assert.AreNotEqual(rectangleToInsert, dbRectangle);
			Assert.AreEqual(rectangleToInsert.ID, dbRectangle.ID);
			Assert.AreEqual(rectangleToInsert.Name, dbRectangle.Name);
		}


		[Test]
		public void InsertSquareAndRectangle_WhenExceptionIsThrownBeforeInsertingRectangle_ThenAmbientTransactionIsRolledBackAndNeitherSquareNorRectangleIsInserted()
		{
			ITransactionService ambientTransactionService = new AmbientTransactionService(new TransactionService(new SquareRepository(new Context("squareContext")), new ThrowExceptionRectangleRepository(new RectangleRepository(new Context("rectangleContext")))));

			Square squareToInsert = new Square() { ID = Guid.NewGuid(), Name = "Square1" };
			Rectangle rectangleToInsert = new Rectangle() { ID = Guid.NewGuid(), Name = "Rectangle1" };

			try
			{
				ambientTransactionService.InsertSquareAndRectangle(squareToInsert, rectangleToInsert);
			}
			catch (NotImplementedException exception)
			{
				Assert.IsNotNull(exception);
			}

			Square dbSquare = readContext.SquareSet.Find(squareToInsert.ID);
			Rectangle dbRectangle = readContext.RectangleSet.Find(rectangleToInsert.ID);

			Assert.IsNull(dbSquare);
			Assert.IsNull(dbRectangle);
		}


		private class AmbientTransactionService : ITransactionService
		{
			private readonly ITransactionService transactionService;

			public AmbientTransactionService(ITransactionService transactionService)
			{
				this.transactionService = transactionService;
			}

			public void InsertSquare(Square square)
			{
				using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
				{
					transactionService.InsertSquare(square);

					scope.Complete();
				}
			}

			public void InsertRectangle(Rectangle rectangle)
			{
				using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
				{
					transactionService.InsertRectangle(rectangle);

					scope.Complete();
				}
			}

			public void InsertSquareAndRectangle(Square square, Rectangle rectangle)
			{
				using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
				{
					transactionService.InsertSquareAndRectangle(square, rectangle);

					scope.Complete();
				}
			}
		}

		private class ThrowExceptionRectangleRepository : IRectangleRepository
		{
			private readonly IRectangleRepository rectangleRepository;

			public ThrowExceptionRectangleRepository(IRectangleRepository rectangleRepository)
			{
				this.rectangleRepository = rectangleRepository;
			}

			public Rectangle GetByID(Guid id)
			{
				throw new NotImplementedException();
			}

			public void Upsert(Rectangle rectangle)
			{
				throw new NotImplementedException();
			}

			public void Delete(Guid id)
			{
				throw new NotImplementedException();
			}
		}
	}
}