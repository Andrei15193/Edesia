using System.Collections.Generic;
using Andrei15193.ConstraintSatisfaction.Tuples;
namespace Andrei15193.ConstraintSatisfaction
{
	public interface IBinaryConstraints<TName, TValue>
		: IReadOnlyCollection<BinaryConstraint<TName, TValue>>
	{
		bool AreSatisfiedBy(IVariables<IVariable<TName, TValue>, IVariable<TName, TValue>> values);
		IEnumerable<TName> AdjacentVertices(TName vertex);
	}
}