using System;
using System.Collections.Generic;
namespace Andrei15193.Edesia.Collections
{
	public static class Comparer
	{
		public static IComparer<T> Create<T>(Func<T, T, int> compare)
		{
			return new DelegateComparer<T>(compare);
		}

		private sealed class DelegateComparer<T>
			: IComparer<T>
		{
			public DelegateComparer(Func<T, T, int> compare)
			{
				if (compare == null)
					throw new ArgumentNullException("compare");

				_compare = compare;
			}

			#region IComparer<T> Members
			public int Compare(T x, T y)
			{
				return _compare(x, y);
			}
			#endregion

			private readonly Func<T, T, int> _compare;
		}
	}
}