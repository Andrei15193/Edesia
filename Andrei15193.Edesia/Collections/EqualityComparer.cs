using System;
using System.Collections.Generic;
namespace Andrei15193.Edesia.Collections
{
	public class EqualityComparer
	{
		public static IEqualityComparer<T> Create<T>(Func<T, T, bool> equals, Func<T, int> getHashCode)
		{
			return new DelegateEqualityComparer<T>(equals, getHashCode);
		}

		private sealed class DelegateEqualityComparer<T>
			: IEqualityComparer<T>
		{
			public DelegateEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
			{
				if (equals == null)
					throw new ArgumentNullException("equals");
				if (getHashCode == null)
					throw new ArgumentNullException("getHashCode");

				_equals = equals;
				_getHashCode = getHashCode;
			}

			#region IEqualityComparer<T> Members
			public bool Equals(T x, T y)
			{
				return _equals(x, y);
			}
			public int GetHashCode(T obj)
			{
				if (obj == null)
					throw new ArgumentNullException("obj");

				return _getHashCode(obj);
			}
			#endregion

			private readonly Func<T, T, bool> _equals;
			private readonly Func<T, int> _getHashCode;
		}
	}
}