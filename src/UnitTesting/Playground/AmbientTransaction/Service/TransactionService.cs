using Dynamic.Decorator.UnitTesting.Playground.AmbientTransaction.Entity;
using Dynamic.Decorator.UnitTesting.Playground.AmbientTransaction.Repository;
using System;

namespace Dynamic.Decorator.UnitTesting.Playground.AmbientTransaction.Service
{
	public interface ITransactionService
	{
		void InsertSquare(Square square);

		void InsertRectangle(Rectangle rectangle);

		void InsertSquareAndRectangle(Square square, Rectangle rectangle);
	}

	public class TransactionService : ITransactionService
	{
		private readonly ISquareRepository squareRepository;
		private readonly IRectangleRepository rectangleRepository;

		public TransactionService(ISquareRepository squareRepository, IRectangleRepository rectangleRepository)
		{
			this.squareRepository = squareRepository;
			this.rectangleRepository = rectangleRepository;
		}

		public void InsertSquare(Square square)
		{
			squareRepository.Upsert(square);
		}

		public Square GetSquareByID(Guid id)
		{
			return squareRepository.GetByID(id);
		}


		public void InsertRectangle(Rectangle rectangle)
		{
			rectangleRepository.Upsert(rectangle);
		}

		public Rectangle GetRectangleByID(Guid id)
		{
			return rectangleRepository.GetByID(id);
		}


		public void InsertSquareAndRectangle(Square square, Rectangle rectangle)
		{
			squareRepository.Upsert(square);

			rectangleRepository.Upsert(rectangle);
		}
	}
}