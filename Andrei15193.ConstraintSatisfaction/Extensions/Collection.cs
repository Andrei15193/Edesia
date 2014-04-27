using System;
using System.Collections.Generic;
namespace Andrei15193.ConstraintSatisfaction.Extensions
{
	public static class Collection
	{
		public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
		{
			if (collection == null)
				throw new ArgumentNullException("collection");
			if (items == null)
				throw new ArgumentNullException("items");

			foreach (T item in items)
				collection.Add(item);
		}
	}
}