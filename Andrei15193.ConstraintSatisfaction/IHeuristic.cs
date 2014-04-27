using System.Collections.Generic;
namespace Andrei15193.ConstraintSatisfaction
{
	public interface IHeuristic<TName, TValue>
	{
		IEnumerable<KeyValuePair<TName, IEnumerable<TValue>>> Apply(IEnumerable<KeyValuePair<TName, IEnumerable<TValue>>> domains);
	}
}