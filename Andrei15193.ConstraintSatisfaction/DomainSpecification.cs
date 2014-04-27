using System;
using System.Collections.Generic;
namespace Andrei15193.ConstraintSatisfaction
{
	public static class DomainSpecification
	{
		public static DomainSpecification<TValue> Create<TValue>(IEnumerable<TValue> values)
		{
			return new DelegateDomainSpecification<TValue>(() => values);
		}
		public static DomainSpecification<TValue> Create<TValue>(params TValue[] values)
		{
			return Create((IEnumerable<TValue>)values);
		}
		public static DomainSpecification<TValue> Create<TValue>(Func<IEnumerable<TValue>> valuesProvider)
		{
			return new DelegateDomainSpecification<TValue>(valuesProvider);
		}

		private class DelegateDomainSpecification<TValue>
					: DomainSpecification<TValue>
		{
			public DelegateDomainSpecification(Func<IEnumerable<TValue>> valuesProvider)
			{
				if (valuesProvider == null)
					throw new ArgumentNullException("valuesProvider");

				_valuesProvider = valuesProvider;
			}

			public override IEnumerable<TValue> Values
			{
				get
				{
					return (_valuesProvider() ?? new TValue[0]);
				}
			}

			private readonly Func<IEnumerable<TValue>> _valuesProvider;
		}
	}

	public abstract class DomainSpecification<TValue>
	{
		public abstract IEnumerable<TValue> Values
		{
			get;
		}
	}
}