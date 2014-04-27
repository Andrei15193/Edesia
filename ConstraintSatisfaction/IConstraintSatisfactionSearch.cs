using System.Collections.Generic;
namespace Andrei15193.ConstraintSatisfaction
{
	public interface IConstraintSatisfactionSearch<TName, TValue>
	{
		IEnumerable<IReadOnlyDictionary<TName, TValue>> Find(IEnumerable<KeyValuePair<TName, IEnumerable<TValue>>> domains, IBinaryConstraints<TName, TValue> binaryConstraints, Constraint<IReadOnlyDictionary<TName, TValue>> solutionConstraint, IHeuristic<TName, TValue> heuristic = null);
		IEnumerable<IReadOnlyDictionary<TName, TValue>> Find(IEnumerable<KeyValuePair<TName, IEnumerable<TValue>>> domains, IBinaryConstraints<TName, TValue> binaryConstraints, IHeuristic<TName, TValue> heuristic = null);
		IEnumerable<IReadOnlyDictionary<TName, TValue>> Find(IEnumerable<KeyValuePair<TName, IEnumerable<TValue>>> domains, Constraint<IReadOnlyDictionary<TName, TValue>> solutionConstraint, IHeuristic<TName, TValue> heuristic = null);
	}
}