using System;
using System.Collections.Generic;
using System.Linq;
namespace Andrei15193.ConstraintSatisfaction
{
	public static class Constraint
	{
		public static Constraint<TValue> Create<TValue>(Predicate<TValue> constraint)
		{
			return new DelegateConstraint<TValue>(constraint);
		}

		private class DelegateConstraint<TValue>
			: Constraint<TValue>
		{
			public DelegateConstraint(Predicate<TValue> constraint)
			{
				if (constraint == null)
					throw new ArgumentNullException("constraint");

				_constraint = constraint;
			}

			public override bool IsSatisfiedBy(TValue value)
			{
				return _constraint(value);
			}

			private readonly Predicate<TValue> _constraint;
		}
	}

	public abstract class Constraint<TValue>
	{
		public abstract bool IsSatisfiedBy(TValue value);
		public virtual Constraint<TValue> Not()
		{
			NegatedConstraint negatedSelf = this as NegatedConstraint;
			if (negatedSelf != null)
				return negatedSelf.InitialConstraint;

			return new NegatedConstraint(this);
		}
		public Constraint<TValue> And(Constraint<TValue> other)
		{
			if (other == null)
				throw new ArgumentNullException("other");

			return new AndConstraint(this, other);
		}
		public Constraint<TValue> Or(Constraint<TValue> other)
		{
			if (other == null)
				throw new ArgumentNullException("other");

			return new OrConstraint(this, other);
		}

		private sealed class NegatedConstraint
			: Constraint<TValue>
		{
			public NegatedConstraint(Constraint<TValue> constraint)
			{
				if (constraint == null)
					throw new ArgumentNullException("negatedConstraint");

				_initialConstraint = constraint;
			}

			public override bool IsSatisfiedBy(TValue value)
			{
				if (value == null)
					throw new ArgumentNullException("value");

				return !_initialConstraint.IsSatisfiedBy(value);
			}
			public Constraint<TValue> InitialConstraint
			{
				get
				{
					return _initialConstraint;
				}
			}

			private readonly Constraint<TValue> _initialConstraint;
		}
		private sealed class AndConstraint
			: Constraint<TValue>
		{
			public AndConstraint(Constraint<TValue> first, Constraint<TValue> second)
			{
				if (first == null)
					throw new ArgumentNullException("first");
				if (second == null)
					throw new ArgumentNullException("second");

				List<Constraint< TValue>> constraints = new List<Constraint< TValue>>();

				constraints.AddRange(_GetConstraints(first));
				constraints.AddRange(_GetConstraints(second));

				_aggregatedConstraints = constraints;
			}

			public override bool IsSatisfiedBy(TValue value)
			{
				if (value == null)
					throw new ArgumentNullException("value");

				return _aggregatedConstraints.All(constraint => constraint.IsSatisfiedBy(value));
			}

			private static IEnumerable<Constraint<TValue>> _GetConstraints(Constraint<TValue> constraint)
			{
				AndConstraint andConstraint = constraint as AndConstraint;

				if (andConstraint != null)
					foreach (Constraint<TValue> aggregatedConstraint in andConstraint._aggregatedConstraints)
						yield return aggregatedConstraint;

				else
					yield return constraint;
			}

			private readonly IEnumerable<Constraint<TValue>> _aggregatedConstraints;
		}
		private sealed class OrConstraint
			: Constraint<TValue>
		{
			public OrConstraint(Constraint<TValue> first, Constraint<TValue> second)
			{
				if (first == null)
					throw new ArgumentNullException("first");
				if (second == null)
					throw new ArgumentNullException("second");

				List<Constraint<TValue>> constraints = new List<Constraint<TValue>>();

				constraints.AddRange(_GetConstraints(first));
				constraints.AddRange(_GetConstraints(second));

				_aggregatedConstraints = constraints;
			}

			public override bool IsSatisfiedBy(TValue value)
			{
				if (value == null)
					throw new ArgumentNullException("value");

				return _aggregatedConstraints.Any(constraint => constraint.IsSatisfiedBy(value));
			}

			private static IEnumerable<Constraint< TValue>> _GetConstraints(Constraint< TValue> constraint)
			{
				OrConstraint andConstraint = constraint as OrConstraint;

				if (andConstraint != null)
					foreach (Constraint<TValue> aggregatedConstraint in andConstraint._aggregatedConstraints)
						yield return aggregatedConstraint;

				else
					yield return constraint;
			}

			private readonly IEnumerable<Constraint< TValue>> _aggregatedConstraints;
		}
	}
}