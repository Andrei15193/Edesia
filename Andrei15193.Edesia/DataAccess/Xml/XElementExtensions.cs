using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
namespace Andrei15193.Edesia.DataAccess.Xml
{
	public static class XElementExtensions
	{
		public static XElement Find<TKey>(this XElement xElement, XName name, TKey key, Func<XElement, TKey> keySelector, IComparer<TKey> keyComparer)
		{
			if (xElement == null)
				throw new ArgumentNullException("xElement");
			if (name == null)
				throw new ArgumentNullException("name");

			return Find(xElement.Elements(name), key, keySelector, keyComparer);
		}
		public static XElement Find<TKey>(this XElement xElement, TKey key, Func<XElement, TKey> keySelector, IComparer<TKey> keyComparer)
		{
			if (xElement == null)
				throw new ArgumentNullException("xElement");

			return Find(xElement.Elements(), key, keySelector, keyComparer);
		}

		public static XElement Find<TKey>(this IEnumerable<XElement> xElements, TKey key, Func<XElement, TKey> keySelector, IComparer<TKey> keyComparer)
		{
			if (xElements == null)
				throw new ArgumentNullException("xElements");

			return Find((IReadOnlyList<XElement>)xElements.ToList(), key, keySelector, keyComparer);
		}
		public static XElement Find<TKey>(this IList<XElement> xElements, TKey key, Func<XElement, TKey> keySelector, IComparer<TKey> keyComparer)
		{
			if (xElements == null)
				throw new ArgumentNullException("xElements");

			return Find((IReadOnlyList<XElement>)new ReadOnlyCollection<XElement>(xElements), key, keySelector, keyComparer);
		}

		public static XElement Find<TKey>(this IReadOnlyList<XElement> xElements, TKey key, Func<XElement, TKey> keySelector, IComparer<TKey> keyComparer)
		{
			if (xElements == null)
				throw new ArgumentNullException("xElements");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (keyComparer == null)
				throw new ArgumentNullException("comparer");

			if (xElements.Count <= 2)
				return xElements.FirstOrDefault(xElement => (keyComparer.Compare(key, keySelector(xElement)) == 0));

			int start = 0, end = xElements.Count, middle = ((end - start + 1) / 2);
			int comparisonResult = keyComparer.Compare(key, keySelector(xElements[middle]));

			while (comparisonResult != 0 && start < end)
			{
				if (comparisonResult < 0)
					end -= middle;
				else
					start += middle;

				middle = ((end - start + 1) / 2);
				comparisonResult = keyComparer.Compare(key, keySelector(xElements[middle]));
			}

			if (comparisonResult != 0)
				return null;

			return xElements[middle];
		}

		public static bool Insert<TKey>(this XElement xElement, XName name, XElement element, Func<XElement, TKey> keySelector, IComparer<TKey> keyComparer)
		{
			if (xElement == null)
				throw new ArgumentNullException("xElement");
			if (name == null)
				throw new ArgumentNullException("name");

			return _Insert(xElement, xElement.Elements(name), element, keySelector, keyComparer);
		}
		public static bool Insert<TKey>(this XElement xElement, XElement element, Func<XElement, TKey> keySelector, IComparer<TKey> keyComparer)
		{
			if (xElement == null)
				throw new ArgumentNullException("xElement");

			return _Insert(xElement, xElement.Elements(), element, keySelector, keyComparer);
		}

		private static bool _Insert<TKey>(XElement root, IEnumerable<XElement> xElements, XElement element, Func<XElement, TKey> keySelector, IComparer<TKey> keyComparer)
		{
			if (element == null)
				throw new ArgumentNullException("element");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (keyComparer == null)
				throw new ArgumentNullException("comparer");

			if (!xElements.Any())
			{
				root.Add(keyComparer);
				return true;
			}

			IReadOnlyList<XElement> xElementList = xElements.ToList();
			int start = 0, end = xElementList.Count, middle = ((end - start + 1) / 2);
			int comparisonResult = keyComparer.Compare(keySelector(element), keySelector(xElementList[middle]));

			while (comparisonResult != 0 && start < end)
			{
				if (comparisonResult < 0)
					end -= middle;
				else
					start += middle;

				middle = ((end - start + 1) / 2);
				comparisonResult = keyComparer.Compare(keySelector(element), keySelector(xElementList[middle]));
			}

			if (comparisonResult == 0)
				return false;

			if (comparisonResult < 0)
				xElementList[start].AddBeforeSelf(element);
			else
				xElementList[start].AddAfterSelf(element);
			return true;
		}
	}
}