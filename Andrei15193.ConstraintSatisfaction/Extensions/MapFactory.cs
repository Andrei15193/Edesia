using System;
using System.Collections.Generic;
using System.Linq;
namespace Andrei15193.ConstraintSatisfaction.Extensions
{
	public static class MapFactory
	{
		public static IDictionary<TKey, TValue> Create<TKey, TValue>(MapType mapType = MapType.SortedDictionary)
		{
			return FactoryDelegate<TKey, TValue>(mapType)();
		}
		public static IDictionary<TKey, TValue> Create<TKey, TValue>(int capacity)
		{
			return FactoryDelegate<TKey, TValue>(capacity)();
		}

		public static IDictionary<TKey, TValue> Create<TKey, TValue>(IComparer<TKey> comparer, MapType mapType = MapType.SortedDictionary)
		{
			return FactoryDelegate<TKey, TValue>(comparer, mapType)();
		}
		public static IDictionary<TKey, TValue> Create<TKey, TValue>(int capacity, IComparer<TKey> comparer)
		{
			return FactoryDelegate<TKey, TValue>(capacity, comparer)();
		}

		public static IDictionary<TKey, TValue> Create<TKey, TValue>(IEqualityComparer<TKey> comparer)
		{
			return FactoryDelegate<TKey, TValue>(comparer)();
		}
		public static IDictionary<TKey, TValue> Create<TKey, TValue>(int capacity, IEqualityComparer<TKey> comparer)
		{
			return FactoryDelegate<TKey, TValue>(capacity, comparer)();
		}

		public static IDictionary<TKey, TValue> Clone<TKey, TValue>(IDictionary<TKey, TValue> dictionary, MapType mapType = MapType.SortedDictionary)
		{
			return FromDictionaryFactoryDelegate<TKey, TValue>(mapType)(dictionary);
		}
		public static IDictionary<TKey, TValue> Clone<TKey, TValue>(IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer, MapType mapType = MapType.SortedDictionary)
		{
			return FromDictionaryFactoryDelegate<TKey, TValue>(comparer, mapType)(dictionary);
		}
		public static IDictionary<TKey, TValue> Clone<TKey, TValue>(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
		{
			return FromDictionaryFactoryDelegate<TKey, TValue>(comparer)(dictionary);
		}

		public static IDictionary<TKey, TValue> CreateFrom<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> collection, MapType mapType = MapType.SortedDictionary)
		{
			return FromCollectionFactoryDelegate<TKey, TValue>(mapType)(collection);
		}
		public static IDictionary<TKey, TValue> CreateFrom<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> collection, IComparer<TKey> comparer, MapType mapType = MapType.SortedDictionary)
		{
			return FromCollectionFactoryDelegate<TKey, TValue>(comparer, mapType)(collection);
		}
		public static IDictionary<TKey, TValue> CreateFrom<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer)
		{
			return FromCollectionFactoryDelegate<TKey, TValue>(comparer)(collection);
		}

		public static Func<IDictionary<TKey, TValue>> FactoryDelegate<TKey, TValue>(MapType mapType = MapType.SortedDictionary)
		{
			if (typeof(IComparable<TKey>).IsAssignableFrom(typeof(TKey)) || typeof(IComparable).IsAssignableFrom(typeof(TKey)))
				switch (mapType)
				{
					case MapType.SortedList:
						return (() => new SortedList<TKey, TValue>());
					default:
						return (() => new SortedDictionary<TKey, TValue>());
				}
			else
				return (() => new Dictionary<TKey, TValue>());
		}
		public static Func<IDictionary<TKey, TValue>> FactoryDelegate<TKey, TValue>(int capacity)
		{
			if (typeof(IComparable<TKey>).IsAssignableFrom(typeof(TKey)) || typeof(IComparable).IsAssignableFrom(typeof(TKey)))
				return (() => new SortedList<TKey, TValue>(capacity));
			else
				return (() => new Dictionary<TKey, TValue>(capacity));
		}

		public static Func<IDictionary<TKey, TValue>> FactoryDelegate<TKey, TValue>(IComparer<TKey> comparer, MapType mapType = MapType.SortedDictionary)
		{
			switch (mapType)
			{
				case MapType.SortedList:
					return (() => new SortedList<TKey, TValue>(comparer));
				default:
					return (() => new SortedDictionary<TKey, TValue>(comparer));
			}
		}
		public static Func<IDictionary<TKey, TValue>> FactoryDelegate<TKey, TValue>(int capacity, IComparer<TKey> comparer)
		{
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return (() => new SortedList<TKey, TValue>(capacity, comparer));
		}

		public static Func<IDictionary<TKey, TValue>> FactoryDelegate<TKey, TValue>(IEqualityComparer<TKey> comparer)
		{
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return (() => new Dictionary<TKey, TValue>(comparer));
		}
		public static Func<IDictionary<TKey, TValue>> FactoryDelegate<TKey, TValue>(int capacity, IEqualityComparer<TKey> comparer)
		{
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return (() => new Dictionary<TKey, TValue>(capacity, comparer));
		}

		public static Func<IDictionary<TKey, TValue>, IDictionary<TKey, TValue>> FromDictionaryFactoryDelegate<TKey, TValue>(MapType mapType = MapType.SortedDictionary)
		{
			if (typeof(IComparable<TKey>).IsAssignableFrom(typeof(TKey)) || typeof(IComparable).IsAssignableFrom(typeof(TKey)))
				switch (mapType)
				{
					case MapType.SortedList:
						return ((dictionary) => new SortedList<TKey, TValue>(dictionary));
					default:
						return ((dictionary) => new SortedDictionary<TKey, TValue>(dictionary));
				}
			else
				return ((dictionary) => new Dictionary<TKey, TValue>(dictionary));
		}
		public static Func<IDictionary<TKey, TValue>, IDictionary<TKey, TValue>> FromDictionaryFactoryDelegate<TKey, TValue>(IComparer<TKey> comparer, MapType mapType = MapType.SortedDictionary)
		{
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			switch (mapType)
			{
				case MapType.SortedList:
					return ((dictionary) => new SortedList<TKey, TValue>(dictionary, comparer));
				default:
					return ((dictionary) => new SortedDictionary<TKey, TValue>(dictionary, comparer));
			}
		}
		public static Func<IDictionary<TKey, TValue>, IDictionary<TKey, TValue>> FromDictionaryFactoryDelegate<TKey, TValue>(IEqualityComparer<TKey> comparer)
		{
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return ((dictionary) => new Dictionary<TKey, TValue>(dictionary, comparer));
		}

		public static Func<IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>> FromCollectionFactoryDelegate<TKey, TValue>(MapType mapType = MapType.SortedDictionary)
		{
			if (typeof(IComparable<TKey>).IsAssignableFrom(typeof(TKey)) || typeof(IComparable).IsAssignableFrom(typeof(TKey)))
				switch (mapType)
				{
					case MapType.SortedList:
						return ((collection) =>
								{
									if (collection == null)
										throw new ArgumentNullException("collection");

									IDictionary<TKey, TValue> dictionary = new SortedList<TKey, TValue>(collection.Count());

									foreach (KeyValuePair<TKey, TValue> item in collection)
										dictionary.Add(item);

									return dictionary;
								});
					default:
						return ((collection) =>
								{
									if (collection == null)
										throw new ArgumentNullException("collection");

									IDictionary<TKey, TValue> dictionary = new SortedDictionary<TKey, TValue>();

									foreach (KeyValuePair<TKey, TValue> item in collection)
										dictionary.Add(item);

									return dictionary;
								});
				}
			else
				return ((collection) =>
						{
							if (collection == null)
								throw new ArgumentNullException("collection");

							IDictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(collection.Count());

							foreach (KeyValuePair<TKey, TValue> item in collection)
								dictionary.Add(item);

							return dictionary;
						});
		}
		public static Func<IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>> FromCollectionFactoryDelegate<TKey, TValue>(IComparer<TKey> comparer, MapType mapType = MapType.SortedDictionary)
		{
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			switch (mapType)
			{
				case MapType.SortedList:
					return ((collection) =>
							{
								if (comparer == null)
									throw new ArgumentNullException("comparer");

								IDictionary<TKey, TValue> dictionary = new SortedList<TKey, TValue>(collection.Count(), comparer);

								foreach (KeyValuePair<TKey, TValue> item in collection)
									dictionary.Add(item);

								return dictionary;
							});
				default:
					return ((collection) =>
							{
								if (comparer == null)
									throw new ArgumentNullException("comparer");

								IDictionary<TKey, TValue> dictionary = new SortedDictionary<TKey, TValue>(comparer);

								foreach (KeyValuePair<TKey, TValue> item in collection)
									dictionary.Add(item);

								return dictionary;
							});
			}
		}
		public static Func<IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>> FromCollectionFactoryDelegate<TKey, TValue>(IEqualityComparer<TKey> comparer)
		{
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return ((collection) =>
					{
						if (collection == null)
							throw new ArgumentNullException("collection");

						IDictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(collection.Count());

						foreach (KeyValuePair<TKey, TValue> item in collection)
							dictionary.Add(item);

						return dictionary;
					});
		}
	}
}