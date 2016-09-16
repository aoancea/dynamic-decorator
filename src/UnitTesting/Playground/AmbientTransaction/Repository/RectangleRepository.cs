using Dynamic.Decorator.UnitTesting.Playground.AmbientTransaction.Entity;
using System;

namespace Dynamic.Decorator.UnitTesting.Playground.AmbientTransaction.Repository
{
	public interface IRectangleRepository
	{
		Rectangle GetByID(Guid id);

		void Upsert(Rectangle Rectangle);

		void Delete(Guid id);
	}

	public class RectangleRepository : IRectangleRepository
	{
		private readonly Context context;

		public RectangleRepository(Context context)
		{
			this.context = context;
		}

		public Rectangle GetByID(Guid id)
		{
			return context.RectangleSet.Find(id);
		}

		public void Upsert(Rectangle Rectangle)
		{
			Rectangle dbRectangle = context.RectangleSet.Find(Rectangle.ID);

			if (dbRectangle == null)
			{
				context.RectangleSet.Add(dbRectangle);
			}
			else
			{
				dbRectangle.Name = Rectangle.Name;
			}

			context.SaveChanges();
		}

		public void Delete(Guid id)
		{
			Rectangle dbRectangle = context.RectangleSet.Find(id);

			if (dbRectangle != null)
			{
				context.RectangleSet.Remove(dbRectangle);

				context.SaveChanges();
			}
		}
	}
}