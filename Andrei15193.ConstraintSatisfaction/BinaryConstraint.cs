using System;
using System.Collections.Generic;
using System.Linq;
using Andrei15193.ConstraintSatisfaction.Tuples;
namespace Andrei15193.ConstraintSatisfaction
{
	public static class BinaryConstraint
	{
		public static BinaryConstraint<TName, TValue> Create<TName, TValue>(TName firstVertex, TName secondVertex, Predicate<IPair<TValue, TValue>> constraint)
		{
			return new BinaryConstraint<TName, TValue>(firstVertex, secondVertex, Constraint.Create(constraint));
		}
		public static BinaryConstraint<TName, TValue> Create<TName, TValue>(TName firstVertex, TName secondVertex, Constraint<IPair<TValue, TValue>> constraint)
		{
			return new BinaryConstraint<TName, TValue>(firstVertex, secondVertex, constraint);
		}
	}

	public class BinaryConstraint<TName, TValue>
		: Constraint<IPair<TValue, TValue>>
	{
		public BinaryConstraint(TName firstVertex, TName secondVertex, Constraint<IPair<TValue, TValue>> constraint)
		{
			if (firstVertex == null)
				throw new ArgumentNullException("firstVertex");
			if (secondVertex == null)
				throw new ArgumentNullException("secondVertex");
			if (constraint == null)
				throw new ArgumentNullException("constraint");
			if (firstVertex.Equals(secondVertex))
				throw new ArgumentException("firstVertex must be different from second vertex");

			_vertices = Pair.Create(firstVertex, secondVertex);
			_constraint = constraint;
		}

		public IPair<TName, TName> Vertices
		{
			get
			{
				return _vertices;
			}
		}
		public override bool IsSatisfiedBy(IPair<TValue, TValue> values)
		{
			if (values == null)
				throw new ArgumentNullException("values");

			return _constraint.IsSatisfiedBy(values);
		}

		public BinaryConstraint<TName, TValue> And(BinaryConstraint<TName, TValue> other)
		{
			if (other == null)
				throw new ArgumentNullException("other");

			return new AndBinaryConstraint(this, other);
		}
		public BinaryConstraint<TName, TValue> Or(BinaryConstraint<TName, TValue> other)
		{
			if (other == null)
				throw new ArgumentNullException("other");

			return new OrBinaryConstraint(this, other);
		}

		private readonly IPair<TName, TName> _vertices;
		private readonly Constraint<IPair<TValue, TValue>> _constraint;

		private sealed class AndBinaryConstraint
			: BinaryConstraint<TName, TValue>
		{
			public AndBinaryConstraint(BinaryConstraint<TName, TValue> first, BinaryConstraint<TName, TValue> second)
				: base(first.Vertices.First, first.Vertices.Second, first.And((Constraint<IPair<TValue, TValue>>)second))
			{
				if (first == null)
					throw new ArgumentNullException("first");
				if (second == null)
					throw new ArgumentNullException("second");
				if (!first.Vertices.First.Equals(second.Vertices.First))
					throw new ArgumentException("Must have same vertices!", "first");
				if (!first.Vertices.Second.Equals(second.Vertices.Second))
					throw new ArgumentException("Must have same vertices!", "second");

				List<BinaryConstraint<TName, TValue>> constraints = new List<BinaryConstraint<TName, TValue>>();

				constraints.AddRange(_GetConstraints(first));
				constraints.AddRange(_GetConstraints(second));

				_aggregatedConstraints = constraints;
			}

			public override bool IsSatisfiedBy(IPair<TValue, TValue> values)
			{
				if (values == null)
					throw new ArgumentNullException("values");

				return _aggregatedConstraints.All(constraint => constraint.IsSatisfiedBy(values));
			}

			private static IEnumerable<BinaryConstraint<TName, TValue>> _GetConstraints(BinaryConstraint<TName, TValue> binaryConstraint)
			{
				AndBinaryConstraint andBinaryConstraint = binaryConstraint as AndBinaryConstraint;

				if (andBinaryConstraint != null)
					foreach (BinaryConstraint<TName, TValue> aggregatedConstraint in andBinaryConstraint._aggregatedConstraints)
						yield return aggregatedConstraint;

				else
					yield return binaryConstraint;
			}

			private readonly IEnumerable<BinaryConstraint<TName, TValue>> _aggregatedConstraints;
		}
		private sealed class OrBinaryConstraint
			: BinaryConstraint<TName, TValue>
		{
			public OrBinaryConstraint(BinaryConstraint<TName, TValue> first, BinaryConstraint<TName, TValue> second)
				: base(first.Vertices.First, first.Vertices.Second, first.And(second))
			{
				if (first == null)
					throw new ArgumentNullException("first");
				if (second == null)
					throw new ArgumentNullException("second");
				if (!first.Vertices.First.Equals(second.Vertices.First))
					throw new ArgumentException("Must have same vertices!", "first");
				if (!first.Vertices.Second.Equals(second.Vertices.Second))
					throw new ArgumentException("Must have same vertices!", "second");

				List<BinaryConstraint<TName, TValue>> constraints = new List<BinaryConstraint<TName, TValue>>();

				constraints.AddRange(_GetConstraints(first));
				constraints.AddRange(_GetConstraints(second));

				_aggregatedConstraints = constraints;
			}

			public override bool IsSatisfiedBy(IPair<TValue, TValue> values)
			{
				if (values == null)
					throw new ArgumentNullException("values");

				return _aggregatedConstraints.Any(constraint => constraint.IsSatisfiedBy(values));
			}

			private static IEnumerable<BinaryConstraint<TName, TValue>> _GetConstraints(BinaryConstraint<TName, TValue> binaryConstraint)
			{
				OrBinaryConstraint orBinaryConstraint = binaryConstraint as OrBinaryConstraint;

				if (orBinaryConstraint != null)
					foreach (BinaryConstraint<TName, TValue> aggregatedConstraint in orBinaryConstraint._aggregatedConstraints)
						yield return aggregatedConstraint;

				else
					yield return binaryConstraint;
			}

			private readonly IEnumerable<BinaryConstraint<TName, TValue>> _aggregatedConstraints;
		}
	}
}