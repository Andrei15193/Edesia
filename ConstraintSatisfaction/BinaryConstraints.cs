using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Andrei15193.ConstraintSatisfaction.Tuples;
namespace Andrei15193.ConstraintSatisfaction
{
	public static class BinaryConstraints
	{
		public static BinaryConstraints<TName, TValue> Create<TName, TValue>(IEnumerable<BinaryConstraint<TName, TValue>> binaryConstraints = null)
		{
			return new BinaryConstraints<TName, TValue>(binaryConstraints);
		}
		public static BinaryConstraints<TName, TValue> Create<TName, TValue>(params BinaryConstraint<TName, TValue>[] binaryConstraints)
		{
			return new BinaryConstraints<TName, TValue>(binaryConstraints);
		}

		public static BinaryConstraints<TName, TValue> Create<TName, TValue>(IComparer<TName> vertexComparer, IEnumerable<BinaryConstraint<TName, TValue>> binaryConstraints = null)
		{
			return new BinaryConstraints<TName, TValue>(vertexComparer, binaryConstraints);
		}
		public static BinaryConstraints<TName, TValue> Create<TName, TValue>(IComparer<TName> vertexComparer, params BinaryConstraint<TName, TValue>[] binaryConstraints)
		{
			return new BinaryConstraints<TName, TValue>(vertexComparer, binaryConstraints);
		}

		public static BinaryConstraints<TName, TValue> Create<TName, TValue>(IEqualityComparer<TName> vertexComparer, IEnumerable<BinaryConstraint<TName, TValue>> binaryConstraints = null)
		{
			return new BinaryConstraints<TName, TValue>(vertexComparer, binaryConstraints);
		}
		public static BinaryConstraints<TName, TValue> Create<TName, TValue>(IEqualityComparer<TName> vertexComparer, params BinaryConstraint<TName, TValue>[] binaryConstraints)
		{
			return new BinaryConstraints<TName, TValue>(vertexComparer, binaryConstraints);
		}
	}

	public class BinaryConstraints<TName, TValue>
		: IBinaryConstraints<TName, TValue>
	{
		public BinaryConstraints(IEnumerable<BinaryConstraint<TName, TValue>> binaryConstraints = null)
		{
			_constraints = _CreateEmptyConstraintsMap();
			_AddRange(binaryConstraints);
		}
		public BinaryConstraints(params BinaryConstraint<TName, TValue>[] binaryConstraints)
			: this((IEnumerable<BinaryConstraint<TName, TValue>>)binaryConstraints)
		{
		}

		public BinaryConstraints(IComparer<TName> vertexComparer, IEnumerable<BinaryConstraint<TName, TValue>> binaryConstraints = null)
		{
			_constraints = _CreateEmptyConstraintsMap(vertexComparer);
		}
		public BinaryConstraints(IComparer<TName> vertexComparer, params BinaryConstraint<TName, TValue>[] binaryConstraints)
			: this(vertexComparer, (IEnumerable<BinaryConstraint<TName, TValue>>)binaryConstraints)
		{
			_constraints = _CreateEmptyConstraintsMap(vertexComparer);
		}

		public BinaryConstraints(IEqualityComparer<TName> vertexComparer, IEnumerable<BinaryConstraint<TName, TValue>> binaryConstraints = null)
		{
			_constraints = _CreateEmptyConstraintsMap(vertexComparer);
		}
		public BinaryConstraints(IEqualityComparer<TName> vertexComparer, params BinaryConstraint<TName, TValue>[] binaryConstraints)
			: this(vertexComparer, (IEnumerable<BinaryConstraint<TName, TValue>>)binaryConstraints)
		{
			_constraints = _CreateEmptyConstraintsMap(vertexComparer);
		}

		#region IBinaryConstraints<TName,TValue> Members
		public bool AreSatisfiedBy(IVariables<IVariable<TName, TValue>, IVariable<TName, TValue>> values)
		{
			if (values == null)
				throw new ArgumentNullException("values");

			IDictionary<TName, BinaryConstraint<TName, TValue>> associatedConstraints;
			if (!_constraints.TryGetValue(values.First.Name, out associatedConstraints))
				return true;

			BinaryConstraint<TName, TValue> constraint;
			if (!associatedConstraints.TryGetValue(values.Second.Name, out constraint))
				return true;

			return constraint.IsSatisfiedBy(Pair.Create(values.First.Value, values.Second.Value));
		}
		public IEnumerable<TName> AdjacentVertices(TName vertex)
		{
			if (vertex == null)
				throw new ArgumentNullException("vertex");

			IDictionary<TName, BinaryConstraint<TName, TValue>> vertexConstraints;
			if (_constraints.TryGetValue(vertex, out vertexConstraints))
				return _constraints[vertex].Keys;
			else
				return new TName[0];
		}
		#endregion
		#region IReadOnlyCollection<BinaryConstraint<TName,TValue>> Members
		public int Count
		{
			get
			{
				return _constraints.Sum(constraint => constraint.Value.Count);
			}
		}
		#endregion
		#region IEnumerable<BinaryConstraint<TName,TValue>> Members
		public IEnumerator<BinaryConstraint<TName, TValue>> GetEnumerator()
		{
			return _constraints.Values.SelectMany(binaryConstraints => binaryConstraints.Values).GetEnumerator();
		}
		#endregion
		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
		public static BinaryConstraints<TName, TValue> Empty
		{
			get
			{
				lock (_emptyLock)
					if (_empty == null)
						_empty = new BinaryConstraints<TName, TValue>();

				return _empty;
			}
		}

		private void _AddRange(IEnumerable<BinaryConstraint<TName, TValue>> binaryConstraints)
		{
			if (binaryConstraints != null)
				foreach (BinaryConstraint<TName, TValue> binaryConstraint in binaryConstraints)
				{
					_Add(binaryConstraint);
					_Add(new SwapOperandsBinaryConstraint(binaryConstraint));
				}
		}
		private void _Add(BinaryConstraint<TName, TValue> binaryConstraint)
		{
			if (binaryConstraint != null)
			{
				IDictionary<TName, BinaryConstraint<TName, TValue>> associatedBinaryConstraints;

				if (!_constraints.TryGetValue(binaryConstraint.Vertices.First, out associatedBinaryConstraints))
				{
					associatedBinaryConstraints = new Dictionary<TName, BinaryConstraint<TName, TValue>>();
					_constraints.Add(binaryConstraint.Vertices.First, associatedBinaryConstraints);
				}

				BinaryConstraint<TName, TValue> existingBinaryConstraint;
				if (associatedBinaryConstraints.TryGetValue(binaryConstraint.Vertices.Second, out existingBinaryConstraint))
					associatedBinaryConstraints[binaryConstraint.Vertices.Second] = associatedBinaryConstraints[binaryConstraint.Vertices.Second].And(binaryConstraint);
				else
					associatedBinaryConstraints.Add(binaryConstraint.Vertices.Second, binaryConstraint);
			}
		}

		private IDictionary<TName, IDictionary<TName, BinaryConstraint<TName, TValue>>> _CreateEmptyConstraintsMap()
		{
			if (typeof(IComparable<TName>).IsAssignableFrom(typeof(TName))
				|| typeof(IComparable).IsAssignableFrom(typeof(TName)))
				return new SortedDictionary<TName, IDictionary<TName, BinaryConstraint<TName, TValue>>>();
			else
				return new Dictionary<TName, IDictionary<TName, BinaryConstraint<TName, TValue>>>();
		}
		private IDictionary<TName, IDictionary<TName, BinaryConstraint<TName, TValue>>> _CreateEmptyConstraintsMap(IComparer<TName> vertexComparer)
		{
			if (vertexComparer == null)
				throw new ArgumentNullException("vertexComparer");

			return new SortedDictionary<TName, IDictionary<TName, BinaryConstraint<TName, TValue>>>(vertexComparer);
		}
		private IDictionary<TName, IDictionary<TName, BinaryConstraint<TName, TValue>>> _CreateEmptyConstraintsMap(IEqualityComparer<TName> vertexComparer)
		{
			if (vertexComparer == null)
				throw new ArgumentNullException("vertexComparer");

			return new Dictionary<TName, IDictionary<TName, BinaryConstraint<TName, TValue>>>(vertexComparer);
		}

		private readonly IDictionary<TName, IDictionary<TName, BinaryConstraint<TName, TValue>>> _constraints;
		private static BinaryConstraints<TName, TValue> _empty = null;
		private static readonly object _emptyLock = new object();

		private sealed class SwapOperandsBinaryConstraint
			: BinaryConstraint<TName, TValue>
		{
			public SwapOperandsBinaryConstraint(BinaryConstraint<TName, TValue> binaryConstraint)
				: base(binaryConstraint.Vertices.Second, binaryConstraint.Vertices.First, binaryConstraint)
			{
				if (binaryConstraint == null)
					throw new ArgumentNullException("binaryConstraint");

				_binaryConstraint = binaryConstraint;
			}

			public BinaryConstraint<TName, TValue> BinaryConstraint
			{
				get
				{
					return _binaryConstraint;
				}
			}
			public override bool IsSatisfiedBy(IPair<TValue, TValue> values)
			{
				if (values == null)
					throw new ArgumentNullException("values");

				return _binaryConstraint.IsSatisfiedBy(Pair.Create(values.Second, values.First));
			}

			private readonly BinaryConstraint<TName, TValue> _binaryConstraint;
		}
	}
}