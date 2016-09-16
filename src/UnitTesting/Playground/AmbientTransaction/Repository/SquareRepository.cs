using Dynamic.Decorator.UnitTesting.Playground.AmbientTransaction.Entity;
using System;

namespace Dynamic.Decorator.UnitTesting.Playground.AmbientTransaction.Repository
{
	public interface ISquareRepository
	{
		Square GetByID(Guid id);

		void Upsert(Square square);

		void Delete(Guid id);
	}

	public class SquareRepository : ISquareRepository
	{
		private readonly Context context;

		public SquareRepository(Context context)
		{
			this.context = context;
		}

		public Square GetByID(Guid id)
		{
			return context.SquareSet.Find(id);
		}

		public void Upsert(Square square)
		{
			Square dbSquare = context.SquareSet.Find(square.ID);

			if (dbSquare == null)
			{
				context.SquareSet.Add(square);
			}
			else
			{
				dbSquare.Name = square.Name;
			}

			context.SaveChanges();
		}

		public void Delete(Guid id)
		{
			Square dbSquare = context.SquareSet.Find(id);

			if (dbSquare != null)
			{
				context.SquareSet.Remove(dbSquare);

				context.SaveChanges();
			}
		}
	}
}