using System;
using System.Collections;
using System.Collections.Generic;
namespace Andrei15193.Edesia.Collections
{
	public sealed class ReadOnlyCollection<TItem>
				: IReadOnlyCollection<TItem>
	{
		public ReadOnlyCollection(ICollection<TItem> collection)
		{
			if (collection == null)
				throw new ArgumentNullException("collection");

			_collection = collection;
		}

		#region IReadOnlyCollection<string> Members
		public int Count
		{
			get
			{
				return _collection.Count;
			}
		}
		#endregion
		#region IEnumerable<TItem> Members
		public IEnumerator<TItem> GetEnumerator()
		{
			return _collection.GetEnumerator();
		}
		#endregion
		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion

		private readonly ICollection<TItem> _collection;
	}
}