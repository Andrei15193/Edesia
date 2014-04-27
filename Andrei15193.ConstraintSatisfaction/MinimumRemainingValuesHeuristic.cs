using System.Collections.Generic;
using System.Linq;
namespace Andrei15193.ConstraintSatisfaction
{
	public class MinimumRemainingValuesHeuristic<TName, TValue>
		: IHeuristic<TName, TValue>
	{
		private MinimumRemainingValuesHeuristic()
		{
		}

		#region IHeuristic<TName,TValue> Members
		public IEnumerable<KeyValuePair<TName, IEnumerable<TValue>>> Apply(IEnumerable<KeyValuePair<TName, IEnumerable<TValue>>> domains)
		{
			return domains.OrderBy(domain => domain.Value.Count()).ToList();
		}
		#endregion
		public static IHeuristic<TName, TValue> Instance
		{
			get
			{
				return _instance;
			}
		}

		private static readonly MinimumRemainingValuesHeuristic<TName, TValue> _instance = new MinimumRemainingValuesHeuristic<TName, TValue>();
	}
}