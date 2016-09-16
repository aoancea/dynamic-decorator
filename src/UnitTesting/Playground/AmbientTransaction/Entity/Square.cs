using System;
using System.ComponentModel.DataAnnotations;

namespace Dynamic.Decorator.UnitTesting.Playground.AmbientTransaction.Entity
{
	public class Square
	{
		[Key]
		public Guid ID { get; set; }

		public string Name { get; set; }
	}
}