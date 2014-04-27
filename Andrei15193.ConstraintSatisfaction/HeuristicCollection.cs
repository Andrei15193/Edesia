using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Andrei15193.ConstraintSatisfaction
{
	public static class HeuristicCollection
	{
		public static IHeuristic<TName, TValue> Create<TName, TValue>(IEnumerable<IHeuristic<TName, TValue>> heuristics)
		{
			return new HeuristicCollection<TName, TValue>(heuristics);
		}
		public static IHeuristic<TName, TValue> Create<TName, TValue>(params IHeuristic<TName, TValue>[] heuristics)
		{
			return new HeuristicCollection<TName, TValue>(heuristics);
		}
	}

	public sealed class HeuristicCollection<TName, TValue>
		: IHeuristic<TName, TValue>, IReadOnlyList<IHeuristic<TName, TValue>>
	{
		public HeuristicCollection(IEnumerable<IHeuristic<TName, TValue>> heuristics)
		{
			if (heuristics == null)
				throw new ArgumentNullException("heuristics");

			_heuristics = heuristics.Where(heuristic => heuristic != null).ToList();
		}
		public HeuristicCollection(params IHeuristic<TName, TValue>[] heuristics)
			: this((IEnumerable<IHeuristic<TName, TValue>>)heuristics)
		{
		}

		#region IHeuristic<TName,TValue> Members
		public IEnumerable<KeyValuePair<TName, IEnumerable<TValue>>> Apply(IEnumerable<KeyValuePair<TName, IEnumerable<TValue>>> domains)
		{
			foreach (IHeuristic<TName, TValue> heuristic in _heuristics)
				domains = heuristic.Apply(domains);

			return domains;
		}
		#endregion
		#region IReadOnlyList<IHeuristic<TName,TValue>> Members
		public IHeuristic<TName, TValue> this[int index]
		{
			get
			{
				return _heuristics[index];
			}
		}
		#endregion
		#region IReadOnlyCollection<IHeuristic<TName,TValue>> Members
		public int Count
		{
			get
			{
				return _heuristics.Count;
			}
		}
		#endregion
		#region IEnumerable<IHeuristic<TName,TValue>> Members
		public IEnumerator<IHeuristic<TName, TValue>> GetEnumerator()
		{
			return _heuristics.GetEnumerator();
		}
		#endregion
		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion

		private readonly IReadOnlyList<IHeuristic<TName, TValue>> _heuristics;
	}
}