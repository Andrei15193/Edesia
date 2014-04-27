using System;
namespace Andrei15193.ConstraintSatisfaction.Tuples
{
	public static class Variables
	{
		public static IVariables<TFirst> Create<TFirst>(TFirst first)
			where TFirst : IVariable
		{
			return new VariablesTuple<TFirst>(first);
		}
		public static IVariables<TFirst, TSecond> Create<TFirst, TSecond>(TFirst first, TSecond second)
			where TFirst : IVariable
			where TSecond : IVariable
		{
			return new VariablesTuple<TFirst, TSecond>(first, second);
		}
		public static IVariables<TFirst, TSecond, TThird> Create<TFirst, TSecond, TThird>(TFirst first, TSecond second, TThird third)
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
		{
			return new VariablesTuple<TFirst, TSecond, TThird>(first, second, third);
		}
		public static IVariables<TFirst, TSecond, TThird, TFourth> Create<TFirst, TSecond, TThird, TFourth>(TFirst first, TSecond second, TThird third, TFourth forth)
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
		{
			return new VariablesTuple<TFirst, TSecond, TThird, TFourth>(first, second, third, forth);
		}
		public static IVariables<TFirst, TSecond, TThird, TFourth, TFifth> Create<TFirst, TSecond, TThird, TFourth, TFifth>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth)
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
		{
			return new VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth>(first, second, third, forth, fifth);
		}
		public static IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth)
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
		{
			return new VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(first, second, third, forth, fifth, sixth);
		}
		public static IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh)
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
			where TSeventh : IVariable
		{
			return new VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(first, second, third, forth, fifth, sixth, seventh);
		}
		public static IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth)
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
			where TSeventh : IVariable
			where TEighth : IVariable
		{
			return new VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(first, second, third, forth, fifth, sixth, seventh, eighth);
		}
		public static IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth)
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
			where TSeventh : IVariable
			where TEighth : IVariable
			where TNineth : IVariable
		{
			return new VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth>(first, second, third, forth, fifth, sixth, seventh, eighth, nineth);
		}
		public static IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth)
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
			where TSeventh : IVariable
			where TEighth : IVariable
			where TNineth : IVariable
			where TTenth : IVariable
		{
			return new VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth>(first, second, third, forth, fifth, sixth, seventh, eighth, nineth, tenth);
		}
		public static IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh)
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
			where TSeventh : IVariable
			where TEighth : IVariable
			where TNineth : IVariable
			where TTenth : IVariable
			where TEleventh : IVariable
		{
			return new VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh>(first, second, third, forth, fifth, sixth, seventh, eighth, nineth, tenth, eleventh);
		}
		public static IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh, TTwelfth twelfth)
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
			where TSeventh : IVariable
			where TEighth : IVariable
			where TNineth : IVariable
			where TTenth : IVariable
			where TEleventh : IVariable
			where TTwelfth : IVariable
		{
			return new VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth>(first, second, third, forth, fifth, sixth, seventh, eighth, nineth, tenth, eleventh, twelfth);
		}
		public static IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh, TTwelfth twelfth, TThirteenth thirteenth)
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
			where TSeventh : IVariable
			where TEighth : IVariable
			where TNineth : IVariable
			where TTenth : IVariable
			where TEleventh : IVariable
			where TTwelfth : IVariable
			where TThirteenth : IVariable
		{
			return new VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth>(first, second, third, forth, fifth, sixth, seventh, eighth, nineth, tenth, eleventh, twelfth, thirteenth);
		}
		public static IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh, TTwelfth twelfth, TThirteenth thirteenth, TFourteenth fourteenth)
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
			where TSeventh : IVariable
			where TEighth : IVariable
			where TNineth : IVariable
			where TTenth : IVariable
			where TEleventh : IVariable
			where TTwelfth : IVariable
			where TThirteenth : IVariable
			where TFourteenth : IVariable
		{
			return new VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>(first, second, third, forth, fifth, sixth, seventh, eighth, nineth, tenth, eleventh, twelfth, thirteenth, fourteenth);
		}
		public static IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh, TTwelfth twelfth, TThirteenth thirteenth, TFourteenth fourteenth, TFifteenth fifteenth)
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
			where TSeventh : IVariable
			where TEighth : IVariable
			where TNineth : IVariable
			where TTenth : IVariable
			where TEleventh : IVariable
			where TTwelfth : IVariable
			where TThirteenth : IVariable
			where TFourteenth : IVariable
			where TFifteenth : IVariable
		{
			return new VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>(first, second, third, forth, fifth, sixth, seventh, eighth, nineth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth);
		}
		public static IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh, TTwelfth twelfth, TThirteenth thirteenth, TFourteenth fourteenth, TFifteenth fifteenth, TSixteenth sixteenth)
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
			where TSeventh : IVariable
			where TEighth : IVariable
			where TNineth : IVariable
			where TTenth : IVariable
			where TEleventh : IVariable
			where TTwelfth : IVariable
			where TThirteenth : IVariable
			where TFourteenth : IVariable
			where TFifteenth : IVariable
			where TSixteenth : IVariable
		{
			return new VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth>(first, second, third, forth, fifth, sixth, seventh, eighth, nineth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth, sixteenth);
		}

		private class VariablesTuple<TFirst>
			: IVariables<TFirst>
			where TFirst : IVariable
		{
			public VariablesTuple(TFirst first)
			{
				if (first == null)
					throw new ArgumentNullException("first");

				_first = first;
			}

			#region IVariables<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			public override bool Equals(object obj)
			{
				IVariables<TFirst> variables = obj as IVariables<TFirst>;

				return (variables != null
						&& _first.Equals(variables.First));
			}
			public override int GetHashCode()
			{
				return _first.GetHashCode();
			}
			public override string ToString()
			{
				return ("First = " + _first.ToString());
			}

			private readonly TFirst _first;
		}
		private class VariablesTuple<TFirst, TSecond>
			: IVariables<TFirst, TSecond>
			where TFirst : IVariable
			where TSecond : IVariable
		{
			public VariablesTuple(TFirst first, TSecond second)
			{
				if (first == null)
					throw new ArgumentNullException("first");
				if (second == null)
					throw new ArgumentNullException("second");

				_first = first;
				_second = second;
			}

			#region IVariables<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			public override bool Equals(object obj)
			{
				IVariables<TFirst, TSecond> variables = obj as IVariables<TFirst, TSecond>;

				return (variables != null
						&& _first.Equals(variables.First)
						&& _second.Equals(variables.Second));
			}
			public override int GetHashCode()
			{
				return (_first.GetHashCode() ^ _second.GetHashCode());
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + _first.ToString(),
								   "Second = " + _second.ToString());
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
		}
		private class VariablesTuple<TFirst, TSecond, TThird>
			: IVariables<TFirst, TSecond, TThird>
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
		{
			public VariablesTuple(TFirst first, TSecond second, TThird third)
			{
				if (first == null)
					throw new ArgumentNullException("first");
				if (second == null)
					throw new ArgumentNullException("second");
				if (third == null)
					throw new ArgumentNullException("third");

				_first = first;
				_second = second;
				_third = third;
			}

			#region IVariables<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			public override bool Equals(object obj)
			{
				IVariables<TFirst, TSecond, TThird> variables = obj as IVariables<TFirst, TSecond, TThird>;

				return (variables != null
						&& _first.Equals(variables.First)
						&& _second.Equals(variables.Second)
						&& _third.Equals(variables.Third));
			}
			public override int GetHashCode()
			{
				return (_first.GetHashCode() ^ _second.GetHashCode() ^ _third.GetHashCode());
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + _first.ToString(),
								   "Second = " + _second.ToString(),
								   "Third = " + _third.ToString());
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
			private readonly TThird _third;
		}
		private class VariablesTuple<TFirst, TSecond, TThird, TFourth>
			: IVariables<TFirst, TSecond, TThird, TFourth>
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
		{
			public VariablesTuple(TFirst first, TSecond second, TThird third, TFourth forth)
			{
				if (first == null)
					throw new ArgumentNullException("first");
				if (second == null)
					throw new ArgumentNullException("second");
				if (third == null)
					throw new ArgumentNullException("third");
				if (forth == null)
					throw new ArgumentNullException("forth");

				_first = first;
				_second = second;
				_third = third;
				_fourth = forth;
			}

			#region IVariables<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			public override bool Equals(object obj)
			{
				IVariables<TFirst, TSecond, TThird, TFourth> variables = obj as IVariables<TFirst, TSecond, TThird, TFourth>;

				return (variables != null
						&& _first.Equals(variables.First)
						&& _second.Equals(variables.Second)
						&& _third.Equals(variables.Third)
						&& _fourth.Equals(variables.Fourth));
			}
			public override int GetHashCode()
			{
				return (_first.GetHashCode() ^ _second.GetHashCode() ^ _third.GetHashCode() ^ _fourth.GetHashCode());
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + _first.ToString(),
								   "Second = " + _second.ToString(),
								   "Third = " + _third.ToString(),
								   "Fourth = " + _fourth.ToString());
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
			private readonly TThird _third;
			private readonly TFourth _fourth;
		}
		private class VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth>
			: IVariables<TFirst, TSecond, TThird, TFourth, TFifth>
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
		{
			public VariablesTuple(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth)
			{
				if (first == null)
					throw new ArgumentNullException("first");
				if (second == null)
					throw new ArgumentNullException("second");
				if (third == null)
					throw new ArgumentNullException("third");
				if (forth == null)
					throw new ArgumentNullException("forth");
				if (fifth == null)
					throw new ArgumentNullException("fifth");

				_first = first;
				_second = second;
				_third = third;
				_fourth = forth;
				_fifth = fifth;
			}

			#region IVariables<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			public override bool Equals(object obj)
			{
				IVariables<TFirst, TSecond, TThird, TFourth, TFifth> variables = obj as IVariables<TFirst, TSecond, TThird, TFourth, TFifth>;

				return (variables != null
						&& _first.Equals(variables.First)
						&& _second.Equals(variables.Second)
						&& _third.Equals(variables.Third)
						&& _fourth.Equals(variables.Fourth)
						&& _fifth.Equals(variables.Fifth));
			}
			public override int GetHashCode()
			{
				return (_first.GetHashCode() ^ _second.GetHashCode() ^ _third.GetHashCode() ^ _fourth.GetHashCode() ^ _fifth.GetHashCode());
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + _first.ToString(),
								   "Second = " + _second.ToString(),
								   "Third = " + _third.ToString(),
								   "Fourth = " + _fourth.ToString(),
								   "Fifth = " + _fifth.ToString());
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
			private readonly TThird _third;
			private readonly TFourth _fourth;
			private readonly TFifth _fifth;
		}
		private class VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>
			: IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
		{
			public VariablesTuple(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth)
			{
				if (first == null)
					throw new ArgumentNullException("first");
				if (second == null)
					throw new ArgumentNullException("second");
				if (third == null)
					throw new ArgumentNullException("third");
				if (forth == null)
					throw new ArgumentNullException("forth");
				if (fifth == null)
					throw new ArgumentNullException("fifth");
				if (sixth == null)
					throw new ArgumentNullException("sixth");

				_first = first;
				_second = second;
				_third = third;
				_fourth = forth;
				_fifth = fifth;
				_sixth = sixth;
			}

			#region IVariables<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			public override bool Equals(object obj)
			{
				IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth> variables = obj as IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>;

				return (variables != null
						&& _first.Equals(variables.First)
						&& _second.Equals(variables.Second)
						&& _third.Equals(variables.Third)
						&& _fourth.Equals(variables.Fourth)
						&& _fifth.Equals(variables.Fifth)
						&& _sixth.Equals(variables.Sixth));
			}
			public override int GetHashCode()
			{
				return (_first.GetHashCode() ^ _second.GetHashCode() ^ _third.GetHashCode() ^ _fourth.GetHashCode() ^ _fifth.GetHashCode() ^ _sixth.GetHashCode());
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + _first.ToString(),
								   "Second = " + _second.ToString(),
								   "Third = " + _third.ToString(),
								   "Fourth = " + _fourth.ToString(),
								   "Fifth = " + _fifth.ToString(),
								   "Sixth = " + _sixth.ToString());
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
			private readonly TThird _third;
			private readonly TFourth _fourth;
			private readonly TFifth _fifth;
			private readonly TSixth _sixth;
		}
		private class VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>
			: IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
			where TSeventh : IVariable
		{
			public VariablesTuple(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh)
			{
				if (first == null)
					throw new ArgumentNullException("first");
				if (second == null)
					throw new ArgumentNullException("second");
				if (third == null)
					throw new ArgumentNullException("third");
				if (forth == null)
					throw new ArgumentNullException("forth");
				if (fifth == null)
					throw new ArgumentNullException("fifth");
				if (sixth == null)
					throw new ArgumentNullException("sixth");
				if (seventh == null)
					throw new ArgumentNullException("seventh");

				_first = first;
				_second = second;
				_third = third;
				_fourth = forth;
				_fifth = fifth;
				_sixth = sixth;
				_seventh = seventh;
			}

			#region IVariables<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh> Members
			public TSeventh Seventh
			{
				get
				{
					return _seventh;
				}
			}
			#endregion
			public override bool Equals(object obj)
			{
				IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh> variables = obj as IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>;

				return (variables != null
						&& _first.Equals(variables.First)
						&& _second.Equals(variables.Second)
						&& _third.Equals(variables.Third)
						&& _fourth.Equals(variables.Fourth)
						&& _fifth.Equals(variables.Fifth)
						&& _sixth.Equals(variables.Sixth)
						&& _seventh.Equals(variables.Seventh));
			}
			public override int GetHashCode()
			{
				return (_first.GetHashCode() ^ _second.GetHashCode() ^ _third.GetHashCode() ^ _fourth.GetHashCode() ^ _fifth.GetHashCode() ^ _sixth.GetHashCode() ^ _seventh.GetHashCode());
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + _first.ToString(),
								   "Second = " + _second.ToString(),
								   "Third = " + _third.ToString(),
								   "Fourth = " + _fourth.ToString(),
								   "Fifth = " + _fifth.ToString(),
								   "Sixth = " + _sixth.ToString(),
								   "Seventh = " + _seventh.ToString());
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
			private readonly TThird _third;
			private readonly TFourth _fourth;
			private readonly TFifth _fifth;
			private readonly TSixth _sixth;
			private readonly TSeventh _seventh;
		}
		private class VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>
			: IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
			where TSeventh : IVariable
			where TEighth : IVariable
		{
			public VariablesTuple(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth)
			{
				if (first == null)
					throw new ArgumentNullException("first");
				if (second == null)
					throw new ArgumentNullException("second");
				if (third == null)
					throw new ArgumentNullException("third");
				if (forth == null)
					throw new ArgumentNullException("forth");
				if (fifth == null)
					throw new ArgumentNullException("fifth");
				if (sixth == null)
					throw new ArgumentNullException("sixth");
				if (seventh == null)
					throw new ArgumentNullException("seventh");
				if (eighth == null)
					throw new ArgumentNullException("eighth");

				_first = first;
				_second = second;
				_third = third;
				_fourth = forth;
				_fifth = fifth;
				_sixth = sixth;
				_seventh = seventh;
				_eighth = eighth;
			}

			#region IVariables<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh> Members
			public TSeventh Seventh
			{
				get
				{
					return _seventh;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth> Members
			public TEighth Eighth
			{
				get
				{
					return _eighth;
				}
			}
			#endregion
			public override bool Equals(object obj)
			{
				IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> variables = obj as IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>;

				return (variables != null
						&& _first.Equals(variables.First)
						&& _second.Equals(variables.Second)
						&& _third.Equals(variables.Third)
						&& _fourth.Equals(variables.Fourth)
						&& _fifth.Equals(variables.Fifth)
						&& _sixth.Equals(variables.Sixth)
						&& _seventh.Equals(variables.Seventh)
						&& _eighth.Equals(variables.Eighth));
			}
			public override int GetHashCode()
			{
				return (_first.GetHashCode() ^ _second.GetHashCode() ^ _third.GetHashCode() ^ _fourth.GetHashCode() ^ _fifth.GetHashCode() ^ _sixth.GetHashCode() ^ _seventh.GetHashCode() ^ _eighth.GetHashCode());
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + _first.ToString(),
								   "Second = " + _second.ToString(),
								   "Third = " + _third.ToString(),
								   "Fourth = " + _fourth.ToString(),
								   "Fifth = " + _fifth.ToString(),
								   "Sixth = " + _sixth.ToString(),
								   "Seventh = " + _seventh.ToString(),
								   "Eighth = " + _eighth.ToString());
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
			private readonly TThird _third;
			private readonly TFourth _fourth;
			private readonly TFifth _fifth;
			private readonly TSixth _sixth;
			private readonly TSeventh _seventh;
			private readonly TEighth _eighth;
		}
		private class VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth>
			: IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth>
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
			where TSeventh : IVariable
			where TEighth : IVariable
			where TNineth : IVariable
		{
			public VariablesTuple(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth)
			{
				if (first == null)
					throw new ArgumentNullException("first");
				if (second == null)
					throw new ArgumentNullException("second");
				if (third == null)
					throw new ArgumentNullException("third");
				if (forth == null)
					throw new ArgumentNullException("forth");
				if (fifth == null)
					throw new ArgumentNullException("fifth");
				if (sixth == null)
					throw new ArgumentNullException("sixth");
				if (seventh == null)
					throw new ArgumentNullException("seventh");
				if (eighth == null)
					throw new ArgumentNullException("eighth");
				if (nineth == null)
					throw new ArgumentNullException("nineth");

				_first = first;
				_second = second;
				_third = third;
				_fourth = forth;
				_fifth = fifth;
				_sixth = sixth;
				_seventh = seventh;
				_eighth = eighth;
				_nineth = nineth;
			}

			#region IVariables<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh> Members
			public TSeventh Seventh
			{
				get
				{
					return _seventh;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth> Members
			public TEighth Eighth
			{
				get
				{
					return _eighth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth> Members
			public TNineth Nineth
			{
				get
				{
					return _nineth;
				}
			}
			#endregion
			public override bool Equals(object obj)
			{
				IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth> variables = obj as IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth>;

				return (variables != null
						&& _first.Equals(variables.First)
						&& _second.Equals(variables.Second)
						&& _third.Equals(variables.Third)
						&& _fourth.Equals(variables.Fourth)
						&& _fifth.Equals(variables.Fifth)
						&& _sixth.Equals(variables.Sixth)
						&& _seventh.Equals(variables.Seventh)
						&& _eighth.Equals(variables.Eighth)
						&& _nineth.Equals(variables.Nineth));
			}
			public override int GetHashCode()
			{
				return (_first.GetHashCode() ^ _second.GetHashCode() ^ _third.GetHashCode() ^ _fourth.GetHashCode() ^ _fifth.GetHashCode() ^ _sixth.GetHashCode() ^ _seventh.GetHashCode() ^ _eighth.GetHashCode() ^ _nineth.GetHashCode());
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + _first.ToString(),
								   "Second = " + _second.ToString(),
								   "Third = " + _third.ToString(),
								   "Fourth = " + _fourth.ToString(),
								   "Fifth = " + _fifth.ToString(),
								   "Sixth = " + _sixth.ToString(),
								   "Seventh = " + _seventh.ToString(),
								   "Eighth = " + _eighth.ToString(),
								   "Nineth = " + _nineth.ToString());
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
			private readonly TThird _third;
			private readonly TFourth _fourth;
			private readonly TFifth _fifth;
			private readonly TSixth _sixth;
			private readonly TSeventh _seventh;
			private readonly TEighth _eighth;
			private readonly TNineth _nineth;
		}
		private class VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth>
			: IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth>
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
			where TSeventh : IVariable
			where TEighth : IVariable
			where TNineth : IVariable
			where TTenth : IVariable
		{
			public VariablesTuple(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth)
			{
				if (first == null)
					throw new ArgumentNullException("first");
				if (second == null)
					throw new ArgumentNullException("second");
				if (third == null)
					throw new ArgumentNullException("third");
				if (forth == null)
					throw new ArgumentNullException("forth");
				if (fifth == null)
					throw new ArgumentNullException("fifth");
				if (sixth == null)
					throw new ArgumentNullException("sixth");
				if (seventh == null)
					throw new ArgumentNullException("seventh");
				if (eighth == null)
					throw new ArgumentNullException("eighth");
				if (nineth == null)
					throw new ArgumentNullException("nineth");
				if (tenth == null)
					throw new ArgumentNullException("tenth");

				_first = first;
				_second = second;
				_third = third;
				_fourth = forth;
				_fifth = fifth;
				_sixth = sixth;
				_seventh = seventh;
				_eighth = eighth;
				_nineth = nineth;
				_tenth = tenth;
			}

			#region IVariables<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh> Members
			public TSeventh Seventh
			{
				get
				{
					return _seventh;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth> Members
			public TEighth Eighth
			{
				get
				{
					return _eighth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth> Members
			public TNineth Nineth
			{
				get
				{
					return _nineth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth> Members
			public TTenth Tenth
			{
				get
				{
					return _tenth;
				}
			}
			#endregion
			public override bool Equals(object obj)
			{
				IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth> variables = obj as IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth>;

				return (variables != null
						&& _first.Equals(variables.First)
						&& _second.Equals(variables.Second)
						&& _third.Equals(variables.Third)
						&& _fourth.Equals(variables.Fourth)
						&& _fifth.Equals(variables.Fifth)
						&& _sixth.Equals(variables.Sixth)
						&& _seventh.Equals(variables.Seventh)
						&& _eighth.Equals(variables.Eighth)
						&& _nineth.Equals(variables.Nineth)
						&& _tenth.Equals(variables.Tenth));
			}
			public override int GetHashCode()
			{
				return (_first.GetHashCode() ^ _second.GetHashCode() ^ _third.GetHashCode() ^ _fourth.GetHashCode() ^ _fifth.GetHashCode() ^ _sixth.GetHashCode() ^ _seventh.GetHashCode() ^ _eighth.GetHashCode() ^ _nineth.GetHashCode() ^ _tenth.GetHashCode());
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + _first.ToString(),
								   "Second = " + _second.ToString(),
								   "Third = " + _third.ToString(),
								   "Fourth = " + _fourth.ToString(),
								   "Fifth = " + _fifth.ToString(),
								   "Sixth = " + _sixth.ToString(),
								   "Seventh = " + _seventh.ToString(),
								   "Eighth = " + _eighth.ToString(),
								   "Nineth = " + _nineth.ToString(),
								   "Tenth = " + _tenth.ToString());
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
			private readonly TThird _third;
			private readonly TFourth _fourth;
			private readonly TFifth _fifth;
			private readonly TSixth _sixth;
			private readonly TSeventh _seventh;
			private readonly TEighth _eighth;
			private readonly TNineth _nineth;
			private readonly TTenth _tenth;
		}
		private class VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh>
			: IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh>
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
			where TSeventh : IVariable
			where TEighth : IVariable
			where TNineth : IVariable
			where TTenth : IVariable
			where TEleventh : IVariable
		{
			public VariablesTuple(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh)
			{
				if (first == null)
					throw new ArgumentNullException("first");
				if (second == null)
					throw new ArgumentNullException("second");
				if (third == null)
					throw new ArgumentNullException("third");
				if (forth == null)
					throw new ArgumentNullException("forth");
				if (fifth == null)
					throw new ArgumentNullException("fifth");
				if (sixth == null)
					throw new ArgumentNullException("sixth");
				if (seventh == null)
					throw new ArgumentNullException("seventh");
				if (eighth == null)
					throw new ArgumentNullException("eighth");
				if (nineth == null)
					throw new ArgumentNullException("nineth");
				if (tenth == null)
					throw new ArgumentNullException("tenth");
				if (eleventh == null)
					throw new ArgumentNullException("eleventh");

				_first = first;
				_second = second;
				_third = third;
				_fourth = forth;
				_fifth = fifth;
				_sixth = sixth;
				_seventh = seventh;
				_eighth = eighth;
				_nineth = nineth;
				_tenth = tenth;
				_eleventh = eleventh;
			}

			#region IVariables<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh> Members
			public TSeventh Seventh
			{
				get
				{
					return _seventh;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth> Members
			public TEighth Eighth
			{
				get
				{
					return _eighth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth> Members
			public TNineth Nineth
			{
				get
				{
					return _nineth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth> Members
			public TTenth Tenth
			{
				get
				{
					return _tenth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh> Members
			public TEleventh Eleventh
			{
				get
				{
					return _eleventh;
				}
			}
			#endregion
			public override bool Equals(object obj)
			{
				IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh> variables = obj as IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh>;

				return (variables != null
						&& _first.Equals(variables.First)
						&& _second.Equals(variables.Second)
						&& _third.Equals(variables.Third)
						&& _fourth.Equals(variables.Fourth)
						&& _fifth.Equals(variables.Fifth)
						&& _sixth.Equals(variables.Sixth)
						&& _seventh.Equals(variables.Seventh)
						&& _eighth.Equals(variables.Eighth)
						&& _nineth.Equals(variables.Nineth)
						&& _tenth.Equals(variables.Tenth)
						&& _eleventh.Equals(variables.Eleventh));
			}
			public override int GetHashCode()
			{
				return (_first.GetHashCode() ^ _second.GetHashCode() ^ _third.GetHashCode() ^ _fourth.GetHashCode() ^ _fifth.GetHashCode() ^ _sixth.GetHashCode() ^ _seventh.GetHashCode() ^ _eighth.GetHashCode() ^ _nineth.GetHashCode() ^ _tenth.GetHashCode() ^ _eleventh.GetHashCode());
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + _first.ToString(),
								   "Second = " + _second.ToString(),
								   "Third = " + _third.ToString(),
								   "Fourth = " + _fourth.ToString(),
								   "Fifth = " + _fifth.ToString(),
								   "Sixth = " + _sixth.ToString(),
								   "Seventh = " + _seventh.ToString(),
								   "Eighth = " + _eighth.ToString(),
								   "Nineth = " + _nineth.ToString(),
								   "Tenth = " + _tenth.ToString(),
								   "Eleventh = " + _eleventh.ToString());
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
			private readonly TThird _third;
			private readonly TFourth _fourth;
			private readonly TFifth _fifth;
			private readonly TSixth _sixth;
			private readonly TSeventh _seventh;
			private readonly TEighth _eighth;
			private readonly TNineth _nineth;
			private readonly TTenth _tenth;
			private readonly TEleventh _eleventh;
		}
		private class VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth>
			: IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth>
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
			where TSeventh : IVariable
			where TEighth : IVariable
			where TNineth : IVariable
			where TTenth : IVariable
			where TEleventh : IVariable
			where TTwelfth : IVariable
		{
			public VariablesTuple(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh, TTwelfth twelfth)
			{
				if (first == null)
					throw new ArgumentNullException("first");
				if (second == null)
					throw new ArgumentNullException("second");
				if (third == null)
					throw new ArgumentNullException("third");
				if (forth == null)
					throw new ArgumentNullException("forth");
				if (fifth == null)
					throw new ArgumentNullException("fifth");
				if (sixth == null)
					throw new ArgumentNullException("sixth");
				if (seventh == null)
					throw new ArgumentNullException("seventh");
				if (eighth == null)
					throw new ArgumentNullException("eighth");
				if (nineth == null)
					throw new ArgumentNullException("nineth");
				if (tenth == null)
					throw new ArgumentNullException("tenth");
				if (eleventh == null)
					throw new ArgumentNullException("eleventh");
				if (twelfth == null)
					throw new ArgumentNullException("twelfth");

				_first = first;
				_second = second;
				_third = third;
				_fourth = forth;
				_fifth = fifth;
				_sixth = sixth;
				_seventh = seventh;
				_eighth = eighth;
				_nineth = nineth;
				_tenth = tenth;
				_eleventh = eleventh;
				_twelfth = twelfth;
			}

			#region IVariables<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh> Members
			public TSeventh Seventh
			{
				get
				{
					return _seventh;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth> Members
			public TEighth Eighth
			{
				get
				{
					return _eighth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth> Members
			public TNineth Nineth
			{
				get
				{
					return _nineth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth> Members
			public TTenth Tenth
			{
				get
				{
					return _tenth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh> Members
			public TEleventh Eleventh
			{
				get
				{
					return _eleventh;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth> Members
			public TTwelfth Twelfth
			{
				get
				{
					return _twelfth;
				}
			}
			#endregion
			public override bool Equals(object obj)
			{
				IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth> variables = obj as IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth>;

				return (variables != null
						&& _first.Equals(variables.First)
						&& _second.Equals(variables.Second)
						&& _third.Equals(variables.Third)
						&& _fourth.Equals(variables.Fourth)
						&& _fifth.Equals(variables.Fifth)
						&& _sixth.Equals(variables.Sixth)
						&& _seventh.Equals(variables.Seventh)
						&& _eighth.Equals(variables.Eighth)
						&& _nineth.Equals(variables.Nineth)
						&& _tenth.Equals(variables.Tenth)
						&& _eleventh.Equals(variables.Eleventh)
						&& _twelfth.Equals(variables.Twelfth));
			}
			public override int GetHashCode()
			{
				return (_first.GetHashCode() ^ _second.GetHashCode() ^ _third.GetHashCode() ^ _fourth.GetHashCode() ^ _fifth.GetHashCode() ^ _sixth.GetHashCode() ^ _seventh.GetHashCode() ^ _eighth.GetHashCode() ^ _nineth.GetHashCode() ^ _tenth.GetHashCode() ^ _eleventh.GetHashCode() ^ _twelfth.GetHashCode());
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + _first.ToString(),
								   "Second = " + _second.ToString(),
								   "Third = " + _third.ToString(),
								   "Fourth = " + _fourth.ToString(),
								   "Fifth = " + _fifth.ToString(),
								   "Sixth = " + _sixth.ToString(),
								   "Seventh = " + _seventh.ToString(),
								   "Eighth = " + _eighth.ToString(),
								   "Nineth = " + _nineth.ToString(),
								   "Tenth = " + _tenth.ToString(),
								   "Eleventh = " + _eleventh.ToString(),
								   "Twelfth = " + _twelfth.ToString());
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
			private readonly TThird _third;
			private readonly TFourth _fourth;
			private readonly TFifth _fifth;
			private readonly TSixth _sixth;
			private readonly TSeventh _seventh;
			private readonly TEighth _eighth;
			private readonly TNineth _nineth;
			private readonly TTenth _tenth;
			private readonly TEleventh _eleventh;
			private readonly TTwelfth _twelfth;
		}
		private class VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth>
			: IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth>
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
			where TSeventh : IVariable
			where TEighth : IVariable
			where TNineth : IVariable
			where TTenth : IVariable
			where TEleventh : IVariable
			where TTwelfth : IVariable
			where TThirteenth : IVariable
		{
			public VariablesTuple(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh, TTwelfth twelfth, TThirteenth thirteenth)
			{
				if (first == null)
					throw new ArgumentNullException("first");
				if (second == null)
					throw new ArgumentNullException("second");
				if (third == null)
					throw new ArgumentNullException("third");
				if (forth == null)
					throw new ArgumentNullException("forth");
				if (fifth == null)
					throw new ArgumentNullException("fifth");
				if (sixth == null)
					throw new ArgumentNullException("sixth");
				if (seventh == null)
					throw new ArgumentNullException("seventh");
				if (eighth == null)
					throw new ArgumentNullException("eighth");
				if (nineth == null)
					throw new ArgumentNullException("nineth");
				if (tenth == null)
					throw new ArgumentNullException("tenth");
				if (eleventh == null)
					throw new ArgumentNullException("eleventh");
				if (twelfth == null)
					throw new ArgumentNullException("twelfth");
				if (thirteenth == null)
					throw new ArgumentNullException("thirteenth");

				_first = first;
				_second = second;
				_third = third;
				_fourth = forth;
				_fifth = fifth;
				_sixth = sixth;
				_seventh = seventh;
				_eighth = eighth;
				_nineth = nineth;
				_tenth = tenth;
				_eleventh = eleventh;
				_twelfth = twelfth;
				_thirteenth = thirteenth;
			}

			#region IVariables<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh> Members
			public TSeventh Seventh
			{
				get
				{
					return _seventh;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth> Members
			public TEighth Eighth
			{
				get
				{
					return _eighth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth> Members
			public TNineth Nineth
			{
				get
				{
					return _nineth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth> Members
			public TTenth Tenth
			{
				get
				{
					return _tenth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh> Members
			public TEleventh Eleventh
			{
				get
				{
					return _eleventh;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth> Members
			public TTwelfth Twelfth
			{
				get
				{
					return _twelfth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth,TThirteenth> Members
			public TThirteenth Thirteenth
			{
				get
				{
					return _thirteenth;
				}
			}
			#endregion
			public override bool Equals(object obj)
			{
				IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth> variables = obj as IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth>;

				return (variables != null
						&& _first.Equals(variables.First)
						&& _second.Equals(variables.Second)
						&& _third.Equals(variables.Third)
						&& _fourth.Equals(variables.Fourth)
						&& _fifth.Equals(variables.Fifth)
						&& _sixth.Equals(variables.Sixth)
						&& _seventh.Equals(variables.Seventh)
						&& _eighth.Equals(variables.Eighth)
						&& _nineth.Equals(variables.Nineth)
						&& _tenth.Equals(variables.Tenth)
						&& _eleventh.Equals(variables.Eleventh)
						&& _twelfth.Equals(variables.Twelfth)
						&& _thirteenth.Equals(variables.Thirteenth));
			}
			public override int GetHashCode()
			{
				return (_first.GetHashCode() ^ _second.GetHashCode() ^ _third.GetHashCode() ^ _fourth.GetHashCode() ^ _fifth.GetHashCode() ^ _sixth.GetHashCode() ^ _seventh.GetHashCode() ^ _eighth.GetHashCode() ^ _nineth.GetHashCode() ^ _tenth.GetHashCode() ^ _eleventh.GetHashCode() ^ _twelfth.GetHashCode() ^ _thirteenth.GetHashCode());
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + _first.ToString(),
								   "Second = " + _second.ToString(),
								   "Third = " + _third.ToString(),
								   "Fourth = " + _fourth.ToString(),
								   "Fifth = " + _fifth.ToString(),
								   "Sixth = " + _sixth.ToString(),
								   "Seventh = " + _seventh.ToString(),
								   "Eighth = " + _eighth.ToString(),
								   "Nineth = " + _nineth.ToString(),
								   "Tenth = " + _tenth.ToString(),
								   "Eleventh = " + _eleventh.ToString(),
								   "Twelfth = " + _twelfth.ToString(),
								   "Thirteenth = " + _thirteenth.ToString());
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
			private readonly TThird _third;
			private readonly TFourth _fourth;
			private readonly TFifth _fifth;
			private readonly TSixth _sixth;
			private readonly TSeventh _seventh;
			private readonly TEighth _eighth;
			private readonly TNineth _nineth;
			private readonly TTenth _tenth;
			private readonly TEleventh _eleventh;
			private readonly TTwelfth _twelfth;
			private readonly TThirteenth _thirteenth;
		}
		private class VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>
			: IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
			where TSeventh : IVariable
			where TEighth : IVariable
			where TNineth : IVariable
			where TTenth : IVariable
			where TEleventh : IVariable
			where TTwelfth : IVariable
			where TThirteenth : IVariable
			where TFourteenth : IVariable
		{
			public VariablesTuple(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh, TTwelfth twelfth, TThirteenth thirteenth, TFourteenth fourteenth)
			{
				if (first == null)
					throw new ArgumentNullException("first");
				if (second == null)
					throw new ArgumentNullException("second");
				if (third == null)
					throw new ArgumentNullException("third");
				if (forth == null)
					throw new ArgumentNullException("forth");
				if (fifth == null)
					throw new ArgumentNullException("fifth");
				if (sixth == null)
					throw new ArgumentNullException("sixth");
				if (seventh == null)
					throw new ArgumentNullException("seventh");
				if (eighth == null)
					throw new ArgumentNullException("eighth");
				if (nineth == null)
					throw new ArgumentNullException("nineth");
				if (tenth == null)
					throw new ArgumentNullException("tenth");
				if (eleventh == null)
					throw new ArgumentNullException("eleventh");
				if (twelfth == null)
					throw new ArgumentNullException("twelfth");
				if (thirteenth == null)
					throw new ArgumentNullException("thirteenth");
				if (fourteenth == null)
					throw new ArgumentNullException("fourteenth");

				_first = first;
				_second = second;
				_third = third;
				_fourth = forth;
				_fifth = fifth;
				_sixth = sixth;
				_seventh = seventh;
				_eighth = eighth;
				_nineth = nineth;
				_tenth = tenth;
				_eleventh = eleventh;
				_twelfth = twelfth;
				_thirteenth = thirteenth;
				_fourteenth = fourteenth;
			}

			#region IVariables<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh> Members
			public TSeventh Seventh
			{
				get
				{
					return _seventh;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth> Members
			public TEighth Eighth
			{
				get
				{
					return _eighth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth> Members
			public TNineth Nineth
			{
				get
				{
					return _nineth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth> Members
			public TTenth Tenth
			{
				get
				{
					return _tenth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh> Members
			public TEleventh Eleventh
			{
				get
				{
					return _eleventh;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth> Members
			public TTwelfth Twelfth
			{
				get
				{
					return _twelfth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth,TThirteenth> Members
			public TThirteenth Thirteenth
			{
				get
				{
					return _thirteenth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth,TThirteenth,TFourteenth> Members
			public TFourteenth Fourteenth
			{
				get
				{
					return _fourteenth;
				}
			}
			#endregion
			public override bool Equals(object obj)
			{
				IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth> variables = obj as IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>;

				return (variables != null
						&& _first.Equals(variables.First)
						&& _second.Equals(variables.Second)
						&& _third.Equals(variables.Third)
						&& _fourth.Equals(variables.Fourth)
						&& _fifth.Equals(variables.Fifth)
						&& _sixth.Equals(variables.Sixth)
						&& _seventh.Equals(variables.Seventh)
						&& _eighth.Equals(variables.Eighth)
						&& _nineth.Equals(variables.Nineth)
						&& _tenth.Equals(variables.Tenth)
						&& _eleventh.Equals(variables.Eleventh)
						&& _twelfth.Equals(variables.Twelfth)
						&& _thirteenth.Equals(variables.Thirteenth)
						&& _fourteenth.Equals(variables.Fourteenth));
			}
			public override int GetHashCode()
			{
				return (_first.GetHashCode() ^ _second.GetHashCode() ^ _third.GetHashCode() ^ _fourth.GetHashCode() ^ _fifth.GetHashCode() ^ _sixth.GetHashCode() ^ _seventh.GetHashCode() ^ _eighth.GetHashCode() ^ _nineth.GetHashCode() ^ _tenth.GetHashCode() ^ _eleventh.GetHashCode() ^ _twelfth.GetHashCode() ^ _thirteenth.GetHashCode() ^ _fourteenth.GetHashCode());
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + _first.ToString(),
								   "Second = " + _second.ToString(),
								   "Third = " + _third.ToString(),
								   "Fourth = " + _fourth.ToString(),
								   "Fifth = " + _fifth.ToString(),
								   "Sixth = " + _sixth.ToString(),
								   "Seventh = " + _seventh.ToString(),
								   "Eighth = " + _eighth.ToString(),
								   "Nineth = " + _nineth.ToString(),
								   "Tenth = " + _tenth.ToString(),
								   "Eleventh = " + _eleventh.ToString(),
								   "Twelfth = " + _twelfth.ToString(),
								   "Thirteenth = " + _thirteenth.ToString(),
								   "Fourteenth = " + _fourteenth.ToString());
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
			private readonly TThird _third;
			private readonly TFourth _fourth;
			private readonly TFifth _fifth;
			private readonly TSixth _sixth;
			private readonly TSeventh _seventh;
			private readonly TEighth _eighth;
			private readonly TNineth _nineth;
			private readonly TTenth _tenth;
			private readonly TEleventh _eleventh;
			private readonly TTwelfth _twelfth;
			private readonly TThirteenth _thirteenth;
			private readonly TFourteenth _fourteenth;
		}
		private class VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>
			: IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
			where TSeventh : IVariable
			where TEighth : IVariable
			where TNineth : IVariable
			where TTenth : IVariable
			where TEleventh : IVariable
			where TTwelfth : IVariable
			where TThirteenth : IVariable
			where TFourteenth : IVariable
			where TFifteenth : IVariable
		{
			public VariablesTuple(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh, TTwelfth twelfth, TThirteenth thirteenth, TFourteenth fourteenth, TFifteenth fifteenth)
			{
				if (first == null)
					throw new ArgumentNullException("first");
				if (second == null)
					throw new ArgumentNullException("second");
				if (third == null)
					throw new ArgumentNullException("third");
				if (forth == null)
					throw new ArgumentNullException("forth");
				if (fifth == null)
					throw new ArgumentNullException("fifth");
				if (sixth == null)
					throw new ArgumentNullException("sixth");
				if (seventh == null)
					throw new ArgumentNullException("seventh");
				if (eighth == null)
					throw new ArgumentNullException("eighth");
				if (nineth == null)
					throw new ArgumentNullException("nineth");
				if (tenth == null)
					throw new ArgumentNullException("tenth");
				if (eleventh == null)
					throw new ArgumentNullException("eleventh");
				if (twelfth == null)
					throw new ArgumentNullException("twelfth");
				if (thirteenth == null)
					throw new ArgumentNullException("thirteenth");
				if (fourteenth == null)
					throw new ArgumentNullException("fourteenth");
				if (fifteenth == null)
					throw new ArgumentNullException("fifteenth");

				_first = first;
				_second = second;
				_third = third;
				_fourth = forth;
				_fifth = fifth;
				_sixth = sixth;
				_seventh = seventh;
				_eighth = eighth;
				_nineth = nineth;
				_tenth = tenth;
				_eleventh = eleventh;
				_twelfth = twelfth;
				_thirteenth = thirteenth;
				_fourteenth = fourteenth;
				_fifteenth = fifteenth;
			}

			#region IVariables<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh> Members
			public TSeventh Seventh
			{
				get
				{
					return _seventh;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth> Members
			public TEighth Eighth
			{
				get
				{
					return _eighth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth> Members
			public TNineth Nineth
			{
				get
				{
					return _nineth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth> Members
			public TTenth Tenth
			{
				get
				{
					return _tenth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh> Members
			public TEleventh Eleventh
			{
				get
				{
					return _eleventh;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth> Members
			public TTwelfth Twelfth
			{
				get
				{
					return _twelfth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth,TThirteenth> Members
			public TThirteenth Thirteenth
			{
				get
				{
					return _thirteenth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth,TThirteenth,TFourteenth> Members
			public TFourteenth Fourteenth
			{
				get
				{
					return _fourteenth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth,TThirteenth,TFourteenth,TFifteenth> Members
			public TFifteenth Fifteenth
			{
				get
				{
					return _fifteenth;
				}
			}
			#endregion
			public override bool Equals(object obj)
			{
				IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth> variables = obj as IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>;

				return (variables != null
						&& _first.Equals(variables.First)
						&& _second.Equals(variables.Second)
						&& _third.Equals(variables.Third)
						&& _fourth.Equals(variables.Fourth)
						&& _fifth.Equals(variables.Fifth)
						&& _sixth.Equals(variables.Sixth)
						&& _seventh.Equals(variables.Seventh)
						&& _eighth.Equals(variables.Eighth)
						&& _nineth.Equals(variables.Nineth)
						&& _tenth.Equals(variables.Tenth)
						&& _eleventh.Equals(variables.Eleventh)
						&& _twelfth.Equals(variables.Twelfth)
						&& _thirteenth.Equals(variables.Thirteenth)
						&& _fourteenth.Equals(variables.Fourteenth)
						&& _fifteenth.Equals(variables.Fifteenth));
			}
			public override int GetHashCode()
			{
				return (_first.GetHashCode() ^ _second.GetHashCode() ^ _third.GetHashCode() ^ _fourth.GetHashCode() ^ _fifth.GetHashCode() ^ _sixth.GetHashCode() ^ _seventh.GetHashCode() ^ _eighth.GetHashCode() ^ _nineth.GetHashCode() ^ _tenth.GetHashCode() ^ _eleventh.GetHashCode() ^ _twelfth.GetHashCode() ^ _thirteenth.GetHashCode() ^ _fourteenth.GetHashCode() ^ _fifteenth.GetHashCode());
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + _first.ToString(),
								   "Second = " + _second.ToString(),
								   "Third = " + _third.ToString(),
								   "Fourth = " + _fourth.ToString(),
								   "Fifth = " + _fifth.ToString(),
								   "Sixth = " + _sixth.ToString(),
								   "Seventh = " + _seventh.ToString(),
								   "Eighth = " + _eighth.ToString(),
								   "Nineth = " + _nineth.ToString(),
								   "Tenth = " + _tenth.ToString(),
								   "Eleventh = " + _eleventh.ToString(),
								   "Twelfth = " + _twelfth.ToString(),
								   "Thirteenth = " + _thirteenth.ToString(),
								   "Fourteenth = " + _fourteenth.ToString(),
								   "Fifteenth = " + _fifteenth.ToString());
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
			private readonly TThird _third;
			private readonly TFourth _fourth;
			private readonly TFifth _fifth;
			private readonly TSixth _sixth;
			private readonly TSeventh _seventh;
			private readonly TEighth _eighth;
			private readonly TNineth _nineth;
			private readonly TTenth _tenth;
			private readonly TEleventh _eleventh;
			private readonly TTwelfth _twelfth;
			private readonly TThirteenth _thirteenth;
			private readonly TFourteenth _fourteenth;
			private readonly TFifteenth _fifteenth;
		}
		private class VariablesTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth>
			: IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth>
			where TFirst : IVariable
			where TSecond : IVariable
			where TThird : IVariable
			where TFourth : IVariable
			where TFifth : IVariable
			where TSixth : IVariable
			where TSeventh : IVariable
			where TEighth : IVariable
			where TNineth : IVariable
			where TTenth : IVariable
			where TEleventh : IVariable
			where TTwelfth : IVariable
			where TThirteenth : IVariable
			where TFourteenth : IVariable
			where TFifteenth : IVariable
			where TSixteenth : IVariable
		{
			public VariablesTuple(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh, TTwelfth twelfth, TThirteenth thirteenth, TFourteenth fourteenth, TFifteenth fifteenth, TSixteenth sixteenth)
			{
				if (first == null)
					throw new ArgumentNullException("first");
				if (second == null)
					throw new ArgumentNullException("second");
				if (third == null)
					throw new ArgumentNullException("third");
				if (forth == null)
					throw new ArgumentNullException("forth");
				if (fifth == null)
					throw new ArgumentNullException("fifth");
				if (sixth == null)
					throw new ArgumentNullException("sixth");
				if (seventh == null)
					throw new ArgumentNullException("seventh");
				if (eighth == null)
					throw new ArgumentNullException("eighth");
				if (nineth == null)
					throw new ArgumentNullException("nineth");
				if (tenth == null)
					throw new ArgumentNullException("tenth");
				if (eleventh == null)
					throw new ArgumentNullException("eleventh");
				if (twelfth == null)
					throw new ArgumentNullException("twelfth");
				if (thirteenth == null)
					throw new ArgumentNullException("thirteenth");
				if (fourteenth == null)
					throw new ArgumentNullException("fourteenth");
				if (fifteenth == null)
					throw new ArgumentNullException("fifteenth");
				if (sixteenth == null)
					throw new ArgumentNullException("sixteenth");

				_first = first;
				_second = second;
				_third = third;
				_fourth = forth;
				_fifth = fifth;
				_sixth = sixth;
				_seventh = seventh;
				_eighth = eighth;
				_nineth = nineth;
				_tenth = tenth;
				_eleventh = eleventh;
				_twelfth = twelfth;
				_thirteenth = thirteenth;
				_fourteenth = fourteenth;
				_fifteenth = fifteenth;
				_sixteenth = sixteenth;
			}

			#region IVariables<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh> Members
			public TSeventh Seventh
			{
				get
				{
					return _seventh;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth> Members
			public TEighth Eighth
			{
				get
				{
					return _eighth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth> Members
			public TNineth Nineth
			{
				get
				{
					return _nineth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth> Members
			public TTenth Tenth
			{
				get
				{
					return _tenth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh> Members
			public TEleventh Eleventh
			{
				get
				{
					return _eleventh;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth> Members
			public TTwelfth Twelfth
			{
				get
				{
					return _twelfth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth,TThirteenth> Members
			public TThirteenth Thirteenth
			{
				get
				{
					return _thirteenth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth,TThirteenth,TFourteenth> Members
			public TFourteenth Fourteenth
			{
				get
				{
					return _fourteenth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth,TThirteenth,TFourteenth,TFifteenth> Members
			public TFifteenth Fifteenth
			{
				get
				{
					return _fifteenth;
				}
			}
			#endregion
			#region IVariables<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth,TThirteenth,TFourteenth,TFifteenth,TSixteenth> Members
			public TSixteenth Sixteenth
			{
				get
				{
					return _sixteenth;
				}
			}
			#endregion
			public override bool Equals(object obj)
			{
				IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth> variables = obj as IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth>;

				return (variables != null
						&& _first.Equals(variables.First)
						&& _second.Equals(variables.Second)
						&& _third.Equals(variables.Third)
						&& _fourth.Equals(variables.Fourth)
						&& _fifth.Equals(variables.Fifth)
						&& _sixth.Equals(variables.Sixth)
						&& _seventh.Equals(variables.Seventh)
						&& _eighth.Equals(variables.Eighth)
						&& _nineth.Equals(variables.Nineth)
						&& _tenth.Equals(variables.Tenth)
						&& _eleventh.Equals(variables.Eleventh)
						&& _twelfth.Equals(variables.Twelfth)
						&& _thirteenth.Equals(variables.Thirteenth)
						&& _fourteenth.Equals(variables.Fourteenth)
						&& _fifteenth.Equals(variables.Fifteenth)
						&& _sixteenth.Equals(variables.Sixteenth));
			}
			public override int GetHashCode()
			{
				return (_first.GetHashCode() ^ _second.GetHashCode() ^ _third.GetHashCode() ^ _fourth.GetHashCode() ^ _fifth.GetHashCode() ^ _sixth.GetHashCode() ^ _seventh.GetHashCode() ^ _eighth.GetHashCode() ^ _nineth.GetHashCode() ^ _tenth.GetHashCode() ^ _eleventh.GetHashCode() ^ _twelfth.GetHashCode() ^ _thirteenth.GetHashCode() ^ _fourteenth.GetHashCode() ^ _fifteenth.GetHashCode() ^ _sixteenth.GetHashCode());
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + _first.ToString(),
								   "Second = " + _second.ToString(),
								   "Third = " + _third.ToString(),
								   "Fourth = " + _fourth.ToString(),
								   "Fifth = " + _fifth.ToString(),
								   "Sixth = " + _sixth.ToString(),
								   "Seventh = " + _seventh.ToString(),
								   "Eighth = " + _eighth.ToString(),
								   "Nineth = " + _nineth.ToString(),
								   "Tenth = " + _tenth.ToString(),
								   "Eleventh = " + _eleventh.ToString(),
								   "Twelfth = " + _twelfth.ToString(),
								   "Thirteenth = " + _thirteenth.ToString(),
								   "Fourteenth = " + _fourteenth.ToString(),
								   "Fifteenth = " + _fifteenth.ToString(),
								   "Sixteenth = " + _sixteenth.ToString());
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
			private readonly TThird _third;
			private readonly TFourth _fourth;
			private readonly TFifth _fifth;
			private readonly TSixth _sixth;
			private readonly TSeventh _seventh;
			private readonly TEighth _eighth;
			private readonly TNineth _nineth;
			private readonly TTenth _tenth;
			private readonly TEleventh _eleventh;
			private readonly TTwelfth _twelfth;
			private readonly TThirteenth _thirteenth;
			private readonly TFourteenth _fourteenth;
			private readonly TFifteenth _fifteenth;
			private readonly TSixteenth _sixteenth;
		}
	}
}