using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Andrei15193.ConstraintSatisfaction.Extensions;
using Andrei15193.ConstraintSatisfaction.Tuples;
namespace Andrei15193.ConstraintSatisfaction
{
	public class ForwardCheckingSearch<TName, TValue>
		: IConstraintSatisfactionSearch<TName, TValue>
	{
		public ForwardCheckingSearch()
		{
			_CloneSolution = MapFactory.FromDictionaryFactoryDelegate<TName, TValue>(MapType.SortedList);
			_CreateEmptySolutionMap = MapFactory.FactoryDelegate<TName, TValue>(MapType.SortedList);
		}
		public ForwardCheckingSearch(IComparer<TName> variableNameComparer)
		{
			if (variableNameComparer == null)
				throw new ArgumentNullException("variableNameComparer");

			_CloneSolution = MapFactory.FromDictionaryFactoryDelegate<TName, TValue>(variableNameComparer, MapType.SortedList);
			_CreateEmptySolutionMap = MapFactory.FactoryDelegate<TName, TValue>(variableNameComparer, MapType.SortedList);
		}
		public ForwardCheckingSearch(IEqualityComparer<TName> variableNameComparer)
		{
			if (variableNameComparer == null)
				throw new ArgumentNullException("variableNameComparer");

			_CloneSolution = MapFactory.FromDictionaryFactoryDelegate<TName, TValue>(variableNameComparer);
			_CreateEmptySolutionMap = MapFactory.FactoryDelegate<TName, TValue>(variableNameComparer);
		}

		#region IConstraintSatisfactionSearch<TName,TValue> Members
		public IEnumerable<IReadOnlyDictionary<TName, TValue>> Find(IEnumerable<KeyValuePair<TName, IEnumerable<TValue>>> domains, IBinaryConstraints<TName, TValue> binaryConstraints, Constraint<IReadOnlyDictionary<TName, TValue>> solutionConstraint, IHeuristic<TName, TValue> heuristic = null)
		{
			if (domains == null)
				throw new ArgumentNullException("domains");
			if (binaryConstraints == null && solutionConstraint == null)
				throw new ArgumentException("binaryConstraints or solutionConstraint must be provided!");

			if (!domains.Any())
			{
				IReadOnlyDictionary<TName, TValue> solution = new Dictionary<TName, TValue>();
				if (solutionConstraint == null || solutionConstraint.IsSatisfiedBy(solution))
					yield return solution;
			}
			else
			{
				int variableIndex = -1;
				Variable[] variables = new Variable[domains.Count()];
				IDictionary<TName, TValue> partialSolution = _CreateEmptySolutionMap();
				IReadOnlyDictionary<TName, TValue> partialSolutionReadOnlyView = new ReadOnlyDictionary<TName, TValue>(partialSolution);
				IEnumerable<KeyValuePair<TName, IEnumerable<TValue>>> unexploredDomains;

				if (heuristic != null)
					unexploredDomains = heuristic.Apply(domains);
				else
					unexploredDomains = domains;

				do
				{
					if (variableIndex == variables.Length - 1 && (solutionConstraint == null || solutionConstraint.IsSatisfiedBy(partialSolutionReadOnlyView)))
						yield return new ReadOnlyDictionary<TName, TValue>(_CloneSolution(partialSolution));
					else
					{
						variables[variableIndex + 1] = new Variable(unexploredDomains.First(), unexploredDomains.Skip(1), binaryConstraints);
						variableIndex++;
						partialSolution.Add(variables[variableIndex].Name, default(TValue));
					}

					do
					{
						while (variableIndex >= 0 && !variables[variableIndex].MoveNext())
						{
							variables[variableIndex].Dispose();
							partialSolution.Remove(variables[variableIndex].Name);
							variableIndex--;
						}
						if (variableIndex >= 0)
						{
							partialSolution[variables[variableIndex].Name] = variables[variableIndex].Value;
							if (heuristic != null)
								unexploredDomains = heuristic.Apply(variables[variableIndex].UnexploredDomains);
							else
								unexploredDomains = variables[variableIndex].UnexploredDomains;
						}
					} while (variableIndex >= 0 && solutionConstraint != null && !solutionConstraint.IsSatisfiedBy(partialSolutionReadOnlyView));
				} while (variableIndex >= 0);
			}
		}
		public IEnumerable<IReadOnlyDictionary<TName, TValue>> Find(IEnumerable<KeyValuePair<TName, IEnumerable<TValue>>> domains, IBinaryConstraints<TName, TValue> binaryConstraints, IHeuristic<TName, TValue> heuristic = null)
		{
			if (binaryConstraints == null)
				throw new ArgumentNullException("binaryConstraints");

			return Find(domains, binaryConstraints, null, heuristic);
		}
		public IEnumerable<IReadOnlyDictionary<TName, TValue>> Find(IEnumerable<KeyValuePair<TName, IEnumerable<TValue>>> domains, Constraint<IReadOnlyDictionary<TName, TValue>> solutionConstraint, IHeuristic<TName, TValue> heuristic = null)
		{
			if (solutionConstraint == null)
				throw new ArgumentNullException("solutionConstraint");

			return Find(domains, null, solutionConstraint, heuristic);
		}
		#endregion

		private readonly Func<IDictionary<TName, TValue>> _CreateEmptySolutionMap;
		private readonly Func<IDictionary<TName, TValue>, IDictionary<TName, TValue>> _CloneSolution;

		private sealed class Variable
			: IVariable<TName, TValue>, IEnumerator<TValue>
		{
			public Variable(TName domainName, IEnumerable<TValue> domainValues, IEnumerable<KeyValuePair<TName, IEnumerable<TValue>>> unexploredDomains, IBinaryConstraints<TName, TValue> binaryConstraints)
			{
				if (domainName == null)
					throw new ArgumentNullException("domainName");
				if (domainValues == null)
					throw new ArgumentNullException("domainValues");
				if (unexploredDomains == null)
					throw new ArgumentNullException("unexploredDomains");

				_variableName = domainName;
				_variableDomain = domainValues;
				_unexploredDomains = unexploredDomains;
				_binaryConstraints = binaryConstraints;

				Reset();
			}
			public Variable(KeyValuePair<TName, IEnumerable<TValue>> domain, IEnumerable<KeyValuePair<TName, IEnumerable<TValue>>> unexploredDomains, IBinaryConstraints<TName, TValue> binaryConstraints)
				: this(domain.Key, domain.Value, unexploredDomains, binaryConstraints)
			{
			}

			#region IVariable<TName,TValue> Members
			public TName Name
			{
				get
				{
					return _variableName;
				}
			}
			public TValue Value
			{
				get
				{
					return _firstDomainValue.Current;
				}
			}
			#endregion
			#region IVariable Members
			object IVariable.Name
			{
				get
				{
					return Name;
				}
			}
			object IVariable.Value
			{
				get
				{
					return Value;
				}
			}
			#endregion
			#region IEnumerator<TValue> Members
			TValue IEnumerator<TValue>.Current
			{
				get
				{
					return Value;
				}
			}
			#endregion
			#region IDisposable Members
			public void Dispose()
			{
				_firstDomainValue.Dispose();
			}
			#endregion
			#region IEnumerator Members
			object IEnumerator.Current
			{
				get
				{
					return Value;
				}
			}
			public bool MoveNext()
			{
				return _firstDomainValue.MoveNext();
			}
			public void Reset()
			{
				_firstDomainValue = _variableDomain.GetEnumerator();
			}
			#endregion
			public IEnumerable<KeyValuePair<TName, IEnumerable<TValue>>> UnexploredDomains
			{
				get
				{
					foreach (KeyValuePair<TName, IEnumerable<TValue>> unexploredDomain in _unexploredDomains)
						yield return new KeyValuePair<TName, IEnumerable<TValue>>(unexploredDomain.Key,
																				  unexploredDomain.Value
																								  .Where(unexploredValue => (_binaryConstraints == null || _binaryConstraints.AreSatisfiedBy(Variables.Create(this, Tuples.Variable.Create(unexploredDomain.Key, unexploredValue))))));

				}
			}

			private IEnumerator<TValue> _firstDomainValue;
			private readonly TName _variableName;
			private readonly IEnumerable<TValue> _variableDomain;
			private readonly IEnumerable<KeyValuePair<TName, IEnumerable<TValue>>> _unexploredDomains;
			private readonly IBinaryConstraints<TName, TValue> _binaryConstraints;
			private readonly IHeuristic<TName, TValue> _heuristic;
		}
	}
}