using System;
namespace Andrei15193.ConstraintSatisfaction.Tuples
{
	public static class Pair
	{
		public static IPair<TFirst> Create<TFirst>(TFirst first)
		{
			return new VariablesPair<TFirst>(first);
		}
		public static IPair<TFirst, TSecond> Create<TFirst, TSecond>(TFirst first, TSecond second)
		{
			return new VariablesPair<TFirst, TSecond>(first, second);
		}
		public static IPair<TFirst, TSecond, TThird> Create<TFirst, TSecond, TThird>(TFirst first, TSecond second, TThird third)
		{
			return new VariablesPair<TFirst, TSecond, TThird>(first, second, third);
		}
		public static IPair<TFirst, TSecond, TThird, TFourth> Create<TFirst, TSecond, TThird, TFourth>(TFirst first, TSecond second, TThird third, TFourth forth)
		{
			return new VariablesPair<TFirst, TSecond, TThird, TFourth>(first, second, third, forth);
		}
		public static IPair<TFirst, TSecond, TThird, TFourth, TFifth> Create<TFirst, TSecond, TThird, TFourth, TFifth>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth)
		{
			return new VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth>(first, second, third, forth, fifth);
		}
		public static IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth)
		{
			return new VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(first, second, third, forth, fifth, sixth);
		}
		public static IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh)
		{
			return new VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(first, second, third, forth, fifth, sixth, seventh);
		}
		public static IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth)
		{
			return new VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(first, second, third, forth, fifth, sixth, seventh, eighth);
		}
		public static IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth)
		{
			return new VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth>(first, second, third, forth, fifth, sixth, seventh, eighth, nineth);
		}
		public static IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth)
		{
			return new VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth>(first, second, third, forth, fifth, sixth, seventh, eighth, nineth, tenth);
		}
		public static IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh)
		{
			return new VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh>(first, second, third, forth, fifth, sixth, seventh, eighth, nineth, tenth, eleventh);
		}
		public static IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh, TTwelfth twelfth)
		{
			return new VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth>(first, second, third, forth, fifth, sixth, seventh, eighth, nineth, tenth, eleventh, twelfth);
		}
		public static IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh, TTwelfth twelfth, TThirteenth thirteenth)
		{
			return new VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth>(first, second, third, forth, fifth, sixth, seventh, eighth, nineth, tenth, eleventh, twelfth, thirteenth);
		}
		public static IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh, TTwelfth twelfth, TThirteenth thirteenth, TFourteenth fourteenth)
		{
			return new VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>(first, second, third, forth, fifth, sixth, seventh, eighth, nineth, tenth, eleventh, twelfth, thirteenth, fourteenth);
		}
		public static IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh, TTwelfth twelfth, TThirteenth thirteenth, TFourteenth fourteenth, TFifteenth fifteenth)
		{
			return new VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>(first, second, third, forth, fifth, sixth, seventh, eighth, nineth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth);
		}
		public static IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth> Create<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth>(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh, TTwelfth twelfth, TThirteenth thirteenth, TFourteenth fourteenth, TFifteenth fifteenth, TSixteenth sixteenth)
		{
			return new VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth>(first, second, third, forth, fifth, sixth, seventh, eighth, nineth, tenth, eleventh, twelfth, thirteenth, fourteenth, fifteenth, sixteenth);
		}

		private class VariablesPair<TFirst>
			: IPair<TFirst>
		{
			public VariablesPair(TFirst first)
			{
				_first = first;
			}

			#region IPair<TFirst> Members
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
				IPair<TFirst> variables = obj as IPair<TFirst>;

				return (variables != null
						&& object.Equals(_first, variables.First));
			}
			public override int GetHashCode()
			{
				return (_first == null ? 0 : _first.GetHashCode());
			}
			public override string ToString()
			{
				return ("First = " + (_first == null ? string.Empty : _first.ToString()));
			}

			private readonly TFirst _first;
		}
		private class VariablesPair<TFirst, TSecond>
			: IPair<TFirst, TSecond>
		{
			public VariablesPair(TFirst first, TSecond second)
			{
				_first = first;
				_second = second;
			}

			#region IPair<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond> Members
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
				IPair<TFirst, TSecond> variables = obj as IPair<TFirst, TSecond>;

				return (variables != null
						&& object.Equals(_first, variables.First)
						&& object.Equals(_second, variables.Second));
			}
			public override int GetHashCode()
			{
				return ((_first == null ? 0 : _first.GetHashCode())
						^ (_second == null ? 0 : _second.GetHashCode()));
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + (_first == null ? string.Empty : _first.ToString()),
								   "Second = " + (_second == null ? string.Empty : _second.ToString()));
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
		}
		private class VariablesPair<TFirst, TSecond, TThird>
			: IPair<TFirst, TSecond, TThird>
		{
			public VariablesPair(TFirst first, TSecond second, TThird third)
			{
				_first = first;
				_second = second;
				_third = third;
			}

			#region IPair<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird> Members
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
				IPair<TFirst, TSecond, TThird> variables = obj as IPair<TFirst, TSecond, TThird>;

				return (variables != null
						&& object.Equals(_first, variables.First)
						&& object.Equals(_second, variables.Second)
						&& object.Equals(_third, variables.Third));
			}
			public override int GetHashCode()
			{
				return ((_first == null ? 0 : _first.GetHashCode())
						^ (_second == null ? 0 : _second.GetHashCode())
						^ (_third == null ? 0 : _third.GetHashCode()));
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + (_first == null ? string.Empty : _first.ToString()),
								   "Second = " + (_second == null ? string.Empty : _second.ToString()),
								   "Third = " + (_third == null ? string.Empty : _third.ToString()));
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
			private readonly TThird _third;
		}
		private class VariablesPair<TFirst, TSecond, TThird, TFourth>
			: IPair<TFirst, TSecond, TThird, TFourth>
		{
			public VariablesPair(TFirst first, TSecond second, TThird third, TFourth forth)
			{
				_first = first;
				_second = second;
				_third = third;
				_fourth = forth;
			}

			#region IPair<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth> Members
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
				IPair<TFirst, TSecond, TThird, TFourth> variables = obj as IPair<TFirst, TSecond, TThird, TFourth>;

				return (variables != null
						&& object.Equals(_first, variables.First)
						&& object.Equals(_second, variables.Second)
						&& object.Equals(_third, variables.Third)
						&& object.Equals(_fourth, variables.Fourth));
			}
			public override int GetHashCode()
			{
				return ((_first == null ? 0 : _first.GetHashCode())
						^ (_second == null ? 0 : _second.GetHashCode())
						^ (_third == null ? 0 : _third.GetHashCode())
						^ (_fourth == null ? 0 : _fourth.GetHashCode()));
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + (_first == null ? string.Empty : _first.ToString()),
								   "Second = " + (_second == null ? string.Empty : _second.ToString()),
								   "Third = " + (_third == null ? string.Empty : _third.ToString()),
								   "Fourth = " + (_fourth == null ? string.Empty : _fourth.ToString()));
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
			private readonly TThird _third;
			private readonly TFourth _fourth;
		}
		private class VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth>
			: IPair<TFirst, TSecond, TThird, TFourth, TFifth>
		{
			public VariablesPair(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth)
			{
				_first = first;
				_second = second;
				_third = third;
				_fourth = forth;
				_fifth = fifth;
			}

			#region IPair<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth> Members
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
				IPair<TFirst, TSecond, TThird, TFourth, TFifth> variables = obj as IPair<TFirst, TSecond, TThird, TFourth, TFifth>;

				return (variables != null
						&& object.Equals(_first, variables.First)
						&& object.Equals(_second, variables.Second)
						&& object.Equals(_third, variables.Third)
						&& object.Equals(_fourth, variables.Fourth)
						&& object.Equals(_fifth, variables.Fifth));
			}
			public override int GetHashCode()
			{
				return ((_first == null ? 0 : _first.GetHashCode())
						^ (_second == null ? 0 : _second.GetHashCode())
						^ (_third == null ? 0 : _third.GetHashCode())
						^ (_fourth == null ? 0 : _fourth.GetHashCode())
						^ (_fifth == null ? 0 : _fifth.GetHashCode()));
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + (_first == null ? string.Empty : _first.ToString()),
								   "Second = " + (_second == null ? string.Empty : _second.ToString()),
								   "Third = " + (_third == null ? string.Empty : _third.ToString()),
								   "Fourth = " + (_fourth == null ? string.Empty : _fourth.ToString()),
								   "Fifth = " + (_fifth == null ? string.Empty : _fifth.ToString()));
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
			private readonly TThird _third;
			private readonly TFourth _fourth;
			private readonly TFifth _fifth;
		}
		private class VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>
			: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>
		{
			public VariablesPair(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth)
			{
				_first = first;
				_second = second;
				_third = third;
				_fourth = forth;
				_fifth = fifth;
				_sixth = sixth;
			}

			#region IPair<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
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
				IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth> variables = obj as IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>;

				return (variables != null
						&& object.Equals(_first, variables.First)
						&& object.Equals(_second, variables.Second)
						&& object.Equals(_third, variables.Third)
						&& object.Equals(_fourth, variables.Fourth)
						&& object.Equals(_fifth, variables.Fifth)
						&& object.Equals(_sixth, variables.Sixth));
			}
			public override int GetHashCode()
			{
				return ((_first == null ? 0 : _first.GetHashCode())
						^ (_second == null ? 0 : _second.GetHashCode())
						^ (_third == null ? 0 : _third.GetHashCode())
						^ (_fourth == null ? 0 : _fourth.GetHashCode())
						^ (_fifth == null ? 0 : _fifth.GetHashCode())
						^ (_sixth == null ? 0 : _sixth.GetHashCode()));
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + (_first == null ? string.Empty : _first.ToString()),
								   "Second = " + (_second == null ? string.Empty : _second.ToString()),
								   "Third = " + (_third == null ? string.Empty : _third.ToString()),
								   "Fourth = " + (_fourth == null ? string.Empty : _fourth.ToString()),
								   "Fifth = " + (_fifth == null ? string.Empty : _fifth.ToString()),
								   "Sixth = " + (_sixth == null ? string.Empty : _sixth.ToString()));
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
			private readonly TThird _third;
			private readonly TFourth _fourth;
			private readonly TFifth _fifth;
			private readonly TSixth _sixth;
		}
		private class VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>
			: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>
		{
			public VariablesPair(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh)
			{
				_first = first;
				_second = second;
				_third = third;
				_fourth = forth;
				_fifth = fifth;
				_sixth = sixth;
				_seventh = seventh;
			}

			#region IPair<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh> Members
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
				IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh> variables = obj as IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>;

				return (variables != null
						&& object.Equals(_first, variables.First)
						&& object.Equals(_second, variables.Second)
						&& object.Equals(_third, variables.Third)
						&& object.Equals(_fourth, variables.Fourth)
						&& object.Equals(_fifth, variables.Fifth)
						&& object.Equals(_sixth, variables.Sixth)
						&& object.Equals(_seventh, variables.Seventh));
			}
			public override int GetHashCode()
			{
				return ((_first == null ? 0 : _first.GetHashCode())
						^ (_second == null ? 0 : _second.GetHashCode())
						^ (_third == null ? 0 : _third.GetHashCode())
						^ (_fourth == null ? 0 : _fourth.GetHashCode())
						^ (_fifth == null ? 0 : _fifth.GetHashCode())
						^ (_sixth == null ? 0 : _sixth.GetHashCode())
						^ (_seventh == null ? 0 : _seventh.GetHashCode()));
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + (_first == null ? string.Empty : _first.ToString()),
								   "Second = " + (_second == null ? string.Empty : _second.ToString()),
								   "Third = " + (_third == null ? string.Empty : _third.ToString()),
								   "Fourth = " + (_fourth == null ? string.Empty : _fourth.ToString()),
								   "Fifth = " + (_fifth == null ? string.Empty : _fifth.ToString()),
								   "Sixth = " + (_sixth == null ? string.Empty : _sixth.ToString()),
								   "Seventh = " + (_seventh == null ? string.Empty : _seventh.ToString()));
			}

			private readonly TFirst _first;
			private readonly TSecond _second;
			private readonly TThird _third;
			private readonly TFourth _fourth;
			private readonly TFifth _fifth;
			private readonly TSixth _sixth;
			private readonly TSeventh _seventh;
		}
		private class VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>
			: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>
		{
			public VariablesPair(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth)
			{
				_first = first;
				_second = second;
				_third = third;
				_fourth = forth;
				_fifth = fifth;
				_sixth = sixth;
				_seventh = seventh;
				_eighth = eighth;
			}

			#region IPair<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh> Members
			public TSeventh Seventh
			{
				get
				{
					return _seventh;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth> Members
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
				IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> variables = obj as IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>;

				return (variables != null
						&& object.Equals(_first, variables.First)
						&& object.Equals(_second, variables.Second)
						&& object.Equals(_third, variables.Third)
						&& object.Equals(_fourth, variables.Fourth)
						&& object.Equals(_fifth, variables.Fifth)
						&& object.Equals(_sixth, variables.Sixth)
						&& object.Equals(_seventh, variables.Seventh)
						&& object.Equals(_eighth, variables.Eighth));
			}
			public override int GetHashCode()
			{
				return ((_first == null ? 0 : _first.GetHashCode())
						^ (_second == null ? 0 : _second.GetHashCode())
						^ (_third == null ? 0 : _third.GetHashCode())
						^ (_fourth == null ? 0 : _fourth.GetHashCode())
						^ (_fifth == null ? 0 : _fifth.GetHashCode())
						^ (_sixth == null ? 0 : _sixth.GetHashCode())
						^ (_seventh == null ? 0 : _seventh.GetHashCode())
						^ (_eighth == null ? 0 : _eighth.GetHashCode()));
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + (_first == null ? string.Empty : _first.ToString()),
								   "Second = " + (_second == null ? string.Empty : _second.ToString()),
								   "Third = " + (_third == null ? string.Empty : _third.ToString()),
								   "Fourth = " + (_fourth == null ? string.Empty : _fourth.ToString()),
								   "Fifth = " + (_fifth == null ? string.Empty : _fifth.ToString()),
								   "Sixth = " + (_sixth == null ? string.Empty : _sixth.ToString()),
								   "Seventh = " + (_seventh == null ? string.Empty : _seventh.ToString()),
								   "Eighth = " + (_eighth == null ? string.Empty : _eighth.ToString()));
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
		private class VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth>
			: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth>
		{
			public VariablesPair(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth)
			{
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

			#region IPair<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh> Members
			public TSeventh Seventh
			{
				get
				{
					return _seventh;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth> Members
			public TEighth Eighth
			{
				get
				{
					return _eighth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth> Members
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
				IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth> variables = obj as IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth>;

				return (variables != null
						&& object.Equals(_first, variables.First)
						&& object.Equals(_second, variables.Second)
						&& object.Equals(_third, variables.Third)
						&& object.Equals(_fourth, variables.Fourth)
						&& object.Equals(_fifth, variables.Fifth)
						&& object.Equals(_sixth, variables.Sixth)
						&& object.Equals(_seventh, variables.Seventh)
						&& object.Equals(_eighth, variables.Eighth)
						&& object.Equals(_nineth, variables.Nineth));
			}
			public override int GetHashCode()
			{
				return ((_first == null ? 0 : _first.GetHashCode())
						^ (_second == null ? 0 : _second.GetHashCode())
						^ (_third == null ? 0 : _third.GetHashCode())
						^ (_fourth == null ? 0 : _fourth.GetHashCode())
						^ (_fifth == null ? 0 : _fifth.GetHashCode())
						^ (_sixth == null ? 0 : _sixth.GetHashCode())
						^ (_seventh == null ? 0 : _seventh.GetHashCode())
						^ (_eighth == null ? 0 : _eighth.GetHashCode())
						^ (_nineth == null ? 0 : _nineth.GetHashCode()));
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + (_first == null ? string.Empty : _first.ToString()),
								   "Second = " + (_second == null ? string.Empty : _second.ToString()),
								   "Third = " + (_third == null ? string.Empty : _third.ToString()),
								   "Fourth = " + (_fourth == null ? string.Empty : _fourth.ToString()),
								   "Fifth = " + (_fifth == null ? string.Empty : _fifth.ToString()),
								   "Sixth = " + (_sixth == null ? string.Empty : _sixth.ToString()),
								   "Seventh = " + (_seventh == null ? string.Empty : _seventh.ToString()),
								   "Eighth = " + (_eighth == null ? string.Empty : _eighth.ToString()),
								   "Nineth = " + (_nineth == null ? string.Empty : _nineth.ToString()));
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
		private class VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth>
			: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth>
		{
			public VariablesPair(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth)
			{
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

			#region IPair<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh> Members
			public TSeventh Seventh
			{
				get
				{
					return _seventh;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth> Members
			public TEighth Eighth
			{
				get
				{
					return _eighth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth> Members
			public TNineth Nineth
			{
				get
				{
					return _nineth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth> Members
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
				IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth> variables = obj as IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth>;

				return (variables != null
						&& object.Equals(_first, variables.First)
						&& object.Equals(_second, variables.Second)
						&& object.Equals(_third, variables.Third)
						&& object.Equals(_fourth, variables.Fourth)
						&& object.Equals(_fifth, variables.Fifth)
						&& object.Equals(_sixth, variables.Sixth)
						&& object.Equals(_seventh, variables.Seventh)
						&& object.Equals(_eighth, variables.Eighth)
						&& object.Equals(_nineth, variables.Nineth)
						&& object.Equals(_tenth, variables.Tenth));
			}
			public override int GetHashCode()
			{
				return ((_first == null ? 0 : _first.GetHashCode())
						^ (_second == null ? 0 : _second.GetHashCode())
						^ (_third == null ? 0 : _third.GetHashCode())
						^ (_fourth == null ? 0 : _fourth.GetHashCode())
						^ (_fifth == null ? 0 : _fifth.GetHashCode())
						^ (_sixth == null ? 0 : _sixth.GetHashCode())
						^ (_seventh == null ? 0 : _seventh.GetHashCode())
						^ (_eighth == null ? 0 : _eighth.GetHashCode())
						^ (_nineth == null ? 0 : _nineth.GetHashCode())
						^ (_tenth == null ? 0 : _tenth.GetHashCode()));
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + (_first == null ? string.Empty : _first.ToString()),
								   "Second = " + (_second == null ? string.Empty : _second.ToString()),
								   "Third = " + (_third == null ? string.Empty : _third.ToString()),
								   "Fourth = " + (_fourth == null ? string.Empty : _fourth.ToString()),
								   "Fifth = " + (_fifth == null ? string.Empty : _fifth.ToString()),
								   "Sixth = " + (_sixth == null ? string.Empty : _sixth.ToString()),
								   "Seventh = " + (_seventh == null ? string.Empty : _seventh.ToString()),
								   "Eighth = " + (_eighth == null ? string.Empty : _eighth.ToString()),
								   "Nineth = " + (_nineth == null ? string.Empty : _nineth.ToString()),
								   "Tenth = " + (_tenth == null ? string.Empty : _tenth.ToString()));
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
		private class VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh>
			: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh>
		{
			public VariablesPair(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh)
			{
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

			#region IPair<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh> Members
			public TSeventh Seventh
			{
				get
				{
					return _seventh;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth> Members
			public TEighth Eighth
			{
				get
				{
					return _eighth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth> Members
			public TNineth Nineth
			{
				get
				{
					return _nineth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth> Members
			public TTenth Tenth
			{
				get
				{
					return _tenth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh> Members
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
				IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh> variables = obj as IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh>;

				return (variables != null
						&& object.Equals(_first, variables.First)
						&& object.Equals(_second, variables.Second)
						&& object.Equals(_third, variables.Third)
						&& object.Equals(_fourth, variables.Fourth)
						&& object.Equals(_fifth, variables.Fifth)
						&& object.Equals(_sixth, variables.Sixth)
						&& object.Equals(_seventh, variables.Seventh)
						&& object.Equals(_eighth, variables.Eighth)
						&& object.Equals(_nineth, variables.Nineth)
						&& object.Equals(_tenth, variables.Tenth)
						&& object.Equals(_eleventh, variables.Eleventh));
			}
			public override int GetHashCode()
			{
				return ((_first == null ? 0 : _first.GetHashCode())
						^ (_second == null ? 0 : _second.GetHashCode())
						^ (_third == null ? 0 : _third.GetHashCode())
						^ (_fourth == null ? 0 : _fourth.GetHashCode())
						^ (_fifth == null ? 0 : _fifth.GetHashCode())
						^ (_sixth == null ? 0 : _sixth.GetHashCode())
						^ (_seventh == null ? 0 : _seventh.GetHashCode())
						^ (_eighth == null ? 0 : _eighth.GetHashCode())
						^ (_nineth == null ? 0 : _nineth.GetHashCode())
						^ (_tenth == null ? 0 : _tenth.GetHashCode())
						^ (_eleventh == null ? 0 : _eleventh.GetHashCode()));
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + (_first == null ? string.Empty : _first.ToString()),
								   "Second = " + (_second == null ? string.Empty : _second.ToString()),
								   "Third = " + (_third == null ? string.Empty : _third.ToString()),
								   "Fourth = " + (_fourth == null ? string.Empty : _fourth.ToString()),
								   "Fifth = " + (_fifth == null ? string.Empty : _fifth.ToString()),
								   "Sixth = " + (_sixth == null ? string.Empty : _sixth.ToString()),
								   "Seventh = " + (_seventh == null ? string.Empty : _seventh.ToString()),
								   "Eighth = " + (_eighth == null ? string.Empty : _eighth.ToString()),
								   "Nineth = " + (_nineth == null ? string.Empty : _nineth.ToString()),
								   "Tenth = " + (_tenth == null ? string.Empty : _tenth.ToString()),
								   "Eleventh = " + (_eleventh == null ? string.Empty : _eleventh.ToString()));
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
		private class VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth>
			: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth>
		{
			public VariablesPair(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh, TTwelfth twelfth)
			{
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

			#region IPair<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh> Members
			public TSeventh Seventh
			{
				get
				{
					return _seventh;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth> Members
			public TEighth Eighth
			{
				get
				{
					return _eighth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth> Members
			public TNineth Nineth
			{
				get
				{
					return _nineth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth> Members
			public TTenth Tenth
			{
				get
				{
					return _tenth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh> Members
			public TEleventh Eleventh
			{
				get
				{
					return _eleventh;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth> Members
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
				IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth> variables = obj as IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth>;

				return (variables != null
						&& object.Equals(_first, variables.First)
						&& object.Equals(_second, variables.Second)
						&& object.Equals(_third, variables.Third)
						&& object.Equals(_fourth, variables.Fourth)
						&& object.Equals(_fifth, variables.Fifth)
						&& object.Equals(_sixth, variables.Sixth)
						&& object.Equals(_seventh, variables.Seventh)
						&& object.Equals(_eighth, variables.Eighth)
						&& object.Equals(_nineth, variables.Nineth)
						&& object.Equals(_tenth, variables.Tenth)
						&& object.Equals(_eleventh, variables.Eleventh)
						&& object.Equals(_twelfth, variables.Twelfth));
			}
			public override int GetHashCode()
			{
				return ((_first == null ? 0 : _first.GetHashCode())
						^ (_second == null ? 0 : _second.GetHashCode())
						^ (_third == null ? 0 : _third.GetHashCode())
						^ (_fourth == null ? 0 : _fourth.GetHashCode())
						^ (_fifth == null ? 0 : _fifth.GetHashCode())
						^ (_sixth == null ? 0 : _sixth.GetHashCode())
						^ (_seventh == null ? 0 : _seventh.GetHashCode())
						^ (_eighth == null ? 0 : _eighth.GetHashCode())
						^ (_nineth == null ? 0 : _nineth.GetHashCode())
						^ (_tenth == null ? 0 : _tenth.GetHashCode())
						^ (_eleventh == null ? 0 : _eleventh.GetHashCode())
						^ (_twelfth == null ? 0 : _twelfth.GetHashCode()));
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + (_first == null ? string.Empty : _first.ToString()),
								   "Second = " + (_second == null ? string.Empty : _second.ToString()),
								   "Third = " + (_third == null ? string.Empty : _third.ToString()),
								   "Fourth = " + (_fourth == null ? string.Empty : _fourth.ToString()),
								   "Fifth = " + (_fifth == null ? string.Empty : _fifth.ToString()),
								   "Sixth = " + (_sixth == null ? string.Empty : _sixth.ToString()),
								   "Seventh = " + (_seventh == null ? string.Empty : _seventh.ToString()),
								   "Eighth = " + (_eighth == null ? string.Empty : _eighth.ToString()),
								   "Nineth = " + (_nineth == null ? string.Empty : _nineth.ToString()),
								   "Tenth = " + (_tenth == null ? string.Empty : _tenth.ToString()),
								   "Eleventh = " + (_eleventh == null ? string.Empty : _eleventh.ToString()),
								   "Twelfth = " + (_twelfth == null ? string.Empty : _twelfth.ToString()));
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
		private class VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth>
			: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth>
		{
			public VariablesPair(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh, TTwelfth twelfth, TThirteenth thirteenth)
			{
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

			#region IPair<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh> Members
			public TSeventh Seventh
			{
				get
				{
					return _seventh;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth> Members
			public TEighth Eighth
			{
				get
				{
					return _eighth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth> Members
			public TNineth Nineth
			{
				get
				{
					return _nineth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth> Members
			public TTenth Tenth
			{
				get
				{
					return _tenth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh> Members
			public TEleventh Eleventh
			{
				get
				{
					return _eleventh;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth> Members
			public TTwelfth Twelfth
			{
				get
				{
					return _twelfth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth,TThirteenth> Members
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
				IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth> variables = obj as IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth>;

				return (variables != null
						&& object.Equals(_first, variables.First)
						&& object.Equals(_second, variables.Second)
						&& object.Equals(_third, variables.Third)
						&& object.Equals(_fourth, variables.Fourth)
						&& object.Equals(_fifth, variables.Fifth)
						&& object.Equals(_sixth, variables.Sixth)
						&& object.Equals(_seventh, variables.Seventh)
						&& object.Equals(_eighth, variables.Eighth)
						&& object.Equals(_nineth, variables.Nineth)
						&& object.Equals(_tenth, variables.Tenth)
						&& object.Equals(_eleventh, variables.Eleventh)
						&& object.Equals(_twelfth, variables.Twelfth)
						&& object.Equals(_thirteenth, variables.Thirteenth));
			}
			public override int GetHashCode()
			{
				return ((_first == null ? 0 : _first.GetHashCode())
						^ (_second == null ? 0 : _second.GetHashCode())
						^ (_third == null ? 0 : _third.GetHashCode())
						^ (_fourth == null ? 0 : _fourth.GetHashCode())
						^ (_fifth == null ? 0 : _fifth.GetHashCode())
						^ (_sixth == null ? 0 : _sixth.GetHashCode())
						^ (_seventh == null ? 0 : _seventh.GetHashCode())
						^ (_eighth == null ? 0 : _eighth.GetHashCode())
						^ (_nineth == null ? 0 : _nineth.GetHashCode())
						^ (_tenth == null ? 0 : _tenth.GetHashCode())
						^ (_eleventh == null ? 0 : _eleventh.GetHashCode())
						^ (_twelfth == null ? 0 : _twelfth.GetHashCode())
						^ (_thirteenth == null ? 0 : _thirteenth.GetHashCode()));
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + (_first == null ? string.Empty : _first.ToString()),
								   "Second = " + (_second == null ? string.Empty : _second.ToString()),
								   "Third = " + (_third == null ? string.Empty : _third.ToString()),
								   "Fourth = " + (_fourth == null ? string.Empty : _fourth.ToString()),
								   "Fifth = " + (_fifth == null ? string.Empty : _fifth.ToString()),
								   "Sixth = " + (_sixth == null ? string.Empty : _sixth.ToString()),
								   "Seventh = " + (_seventh == null ? string.Empty : _seventh.ToString()),
								   "Eighth = " + (_eighth == null ? string.Empty : _eighth.ToString()),
								   "Nineth = " + (_nineth == null ? string.Empty : _nineth.ToString()),
								   "Tenth = " + (_tenth == null ? string.Empty : _tenth.ToString()),
								   "Eleventh = " + (_eleventh == null ? string.Empty : _eleventh.ToString()),
								   "Twelfth = " + (_twelfth == null ? string.Empty : _twelfth.ToString()),
								   "Thirteenth = " + (_thirteenth == null ? string.Empty : _thirteenth.ToString()));
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
		private class VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>
			: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>
		{
			public VariablesPair(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh, TTwelfth twelfth, TThirteenth thirteenth, TFourteenth fourteenth)
			{
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

			#region IPair<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh> Members
			public TSeventh Seventh
			{
				get
				{
					return _seventh;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth> Members
			public TEighth Eighth
			{
				get
				{
					return _eighth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth> Members
			public TNineth Nineth
			{
				get
				{
					return _nineth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth> Members
			public TTenth Tenth
			{
				get
				{
					return _tenth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh> Members
			public TEleventh Eleventh
			{
				get
				{
					return _eleventh;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth> Members
			public TTwelfth Twelfth
			{
				get
				{
					return _twelfth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth,TThirteenth> Members
			public TThirteenth Thirteenth
			{
				get
				{
					return _thirteenth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth,TThirteenth,TFourteenth> Members
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
				IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth> variables = obj as IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>;

				return (variables != null
						&& object.Equals(_first, variables.First)
						&& object.Equals(_second, variables.Second)
						&& object.Equals(_third, variables.Third)
						&& object.Equals(_fourth, variables.Fourth)
						&& object.Equals(_fifth, variables.Fifth)
						&& object.Equals(_sixth, variables.Sixth)
						&& object.Equals(_seventh, variables.Seventh)
						&& object.Equals(_eighth, variables.Eighth)
						&& object.Equals(_nineth, variables.Nineth)
						&& object.Equals(_tenth, variables.Tenth)
						&& object.Equals(_eleventh, variables.Eleventh)
						&& object.Equals(_twelfth, variables.Twelfth)
						&& object.Equals(_thirteenth, variables.Thirteenth)
						&& object.Equals(_fourteenth, variables.Fourteenth));
			}
			public override int GetHashCode()
			{
				return ((_first == null ? 0 : _first.GetHashCode())
						^ (_second == null ? 0 : _second.GetHashCode())
						^ (_third == null ? 0 : _third.GetHashCode())
						^ (_fourth == null ? 0 : _fourth.GetHashCode())
						^ (_fifth == null ? 0 : _fifth.GetHashCode())
						^ (_sixth == null ? 0 : _sixth.GetHashCode())
						^ (_seventh == null ? 0 : _seventh.GetHashCode())
						^ (_eighth == null ? 0 : _eighth.GetHashCode())
						^ (_nineth == null ? 0 : _nineth.GetHashCode())
						^ (_tenth == null ? 0 : _tenth.GetHashCode())
						^ (_eleventh == null ? 0 : _eleventh.GetHashCode())
						^ (_twelfth == null ? 0 : _twelfth.GetHashCode())
						^ (_thirteenth == null ? 0 : _thirteenth.GetHashCode())
						^ (_fourteenth == null ? 0 : _fourteenth.GetHashCode()));
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + (_first == null ? string.Empty : _first.ToString()),
								   "Second = " + (_second == null ? string.Empty : _second.ToString()),
								   "Third = " + (_third == null ? string.Empty : _third.ToString()),
								   "Fourth = " + (_fourth == null ? string.Empty : _fourth.ToString()),
								   "Fifth = " + (_fifth == null ? string.Empty : _fifth.ToString()),
								   "Sixth = " + (_sixth == null ? string.Empty : _sixth.ToString()),
								   "Seventh = " + (_seventh == null ? string.Empty : _seventh.ToString()),
								   "Eighth = " + (_eighth == null ? string.Empty : _eighth.ToString()),
								   "Nineth = " + (_nineth == null ? string.Empty : _nineth.ToString()),
								   "Tenth = " + (_tenth == null ? string.Empty : _tenth.ToString()),
								   "Eleventh = " + (_eleventh == null ? string.Empty : _eleventh.ToString()),
								   "Twelfth = " + (_twelfth == null ? string.Empty : _twelfth.ToString()),
								   "Thirteenth = " + (_thirteenth == null ? string.Empty : _thirteenth.ToString()),
								   "Fourteenth = " + (_fourteenth == null ? string.Empty : _fourteenth.ToString()));
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
		private class VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>
			: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>
		{
			public VariablesPair(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh, TTwelfth twelfth, TThirteenth thirteenth, TFourteenth fourteenth, TFifteenth fifteenth)
			{
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

			#region IPair<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh> Members
			public TSeventh Seventh
			{
				get
				{
					return _seventh;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth> Members
			public TEighth Eighth
			{
				get
				{
					return _eighth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth> Members
			public TNineth Nineth
			{
				get
				{
					return _nineth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth> Members
			public TTenth Tenth
			{
				get
				{
					return _tenth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh> Members
			public TEleventh Eleventh
			{
				get
				{
					return _eleventh;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth> Members
			public TTwelfth Twelfth
			{
				get
				{
					return _twelfth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth,TThirteenth> Members
			public TThirteenth Thirteenth
			{
				get
				{
					return _thirteenth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth,TThirteenth,TFourteenth> Members
			public TFourteenth Fourteenth
			{
				get
				{
					return _fourteenth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth,TThirteenth,TFourteenth,TFifteenth> Members
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
				IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth> variables = obj as IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>;

				return (variables != null
						&& object.Equals(_first, variables.First)
						&& object.Equals(_second, variables.Second)
						&& object.Equals(_third, variables.Third)
						&& object.Equals(_fourth, variables.Fourth)
						&& object.Equals(_fifth, variables.Fifth)
						&& object.Equals(_sixth, variables.Sixth)
						&& object.Equals(_seventh, variables.Seventh)
						&& object.Equals(_eighth, variables.Eighth)
						&& object.Equals(_nineth, variables.Nineth)
						&& object.Equals(_tenth, variables.Tenth)
						&& object.Equals(_eleventh, variables.Eleventh)
						&& object.Equals(_twelfth, variables.Twelfth)
						&& object.Equals(_thirteenth, variables.Thirteenth)
						&& object.Equals(_fourteenth, variables.Fourteenth)
						&& object.Equals(_fifteenth, variables.Fifteenth));
			}
			public override int GetHashCode()
			{
				return ((_first == null ? 0 : _first.GetHashCode())
						^ (_second == null ? 0 : _second.GetHashCode())
						^ (_third == null ? 0 : _third.GetHashCode())
						^ (_fourth == null ? 0 : _fourth.GetHashCode())
						^ (_fifth == null ? 0 : _fifth.GetHashCode())
						^ (_sixth == null ? 0 : _sixth.GetHashCode())
						^ (_seventh == null ? 0 : _seventh.GetHashCode())
						^ (_eighth == null ? 0 : _eighth.GetHashCode())
						^ (_nineth == null ? 0 : _nineth.GetHashCode())
						^ (_tenth == null ? 0 : _tenth.GetHashCode())
						^ (_eleventh == null ? 0 : _eleventh.GetHashCode())
						^ (_twelfth == null ? 0 : _twelfth.GetHashCode())
						^ (_thirteenth == null ? 0 : _thirteenth.GetHashCode())
						^ (_fourteenth == null ? 0 : _fourteenth.GetHashCode())
						^ (_fifteenth == null ? 0 : _fifteenth.GetHashCode()));
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + (_first == null ? string.Empty : _first.ToString()),
								   "Second = " + (_second == null ? string.Empty : _second.ToString()),
								   "Third = " + (_third == null ? string.Empty : _third.ToString()),
								   "Fourth = " + (_fourth == null ? string.Empty : _fourth.ToString()),
								   "Fifth = " + (_fifth == null ? string.Empty : _fifth.ToString()),
								   "Sixth = " + (_sixth == null ? string.Empty : _sixth.ToString()),
								   "Seventh = " + (_seventh == null ? string.Empty : _seventh.ToString()),
								   "Eighth = " + (_eighth == null ? string.Empty : _eighth.ToString()),
								   "Nineth = " + (_nineth == null ? string.Empty : _nineth.ToString()),
								   "Tenth = " + (_tenth == null ? string.Empty : _tenth.ToString()),
								   "Eleventh = " + (_eleventh == null ? string.Empty : _eleventh.ToString()),
								   "Twelfth = " + (_twelfth == null ? string.Empty : _twelfth.ToString()),
								   "Thirteenth = " + (_thirteenth == null ? string.Empty : _thirteenth.ToString()),
								   "Fourteenth = " + (_fourteenth == null ? string.Empty : _fourteenth.ToString()),
								   "Fifteenth = " + (_fifteenth == null ? string.Empty : _fifteenth.ToString()));
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
		private class VariablesPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth>
			: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth>
		{
			public VariablesPair(TFirst first, TSecond second, TThird third, TFourth forth, TFifth fifth, TSixth sixth, TSeventh seventh, TEighth eighth, TNineth nineth, TTenth tenth, TEleventh eleventh, TTwelfth twelfth, TThirteenth thirteenth, TFourteenth fourteenth, TFifteenth fifteenth, TSixteenth sixteenth)
			{
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

			#region IPair<TFirst> Members
			public TFirst First
			{
				get
				{
					return _first;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond> Members
			public TSecond Second
			{
				get
				{
					return _second;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird> Members
			public TThird Third
			{
				get
				{
					return _third;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth> Members
			public TFourth Fourth
			{
				get
				{
					return _fourth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth> Members
			public TFifth Fifth
			{
				get
				{
					return _fifth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth> Members
			public TSixth Sixth
			{
				get
				{
					return _sixth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh> Members
			public TSeventh Seventh
			{
				get
				{
					return _seventh;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth> Members
			public TEighth Eighth
			{
				get
				{
					return _eighth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth> Members
			public TNineth Nineth
			{
				get
				{
					return _nineth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth> Members
			public TTenth Tenth
			{
				get
				{
					return _tenth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh> Members
			public TEleventh Eleventh
			{
				get
				{
					return _eleventh;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth> Members
			public TTwelfth Twelfth
			{
				get
				{
					return _twelfth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth,TThirteenth> Members
			public TThirteenth Thirteenth
			{
				get
				{
					return _thirteenth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth,TThirteenth,TFourteenth> Members
			public TFourteenth Fourteenth
			{
				get
				{
					return _fourteenth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth,TThirteenth,TFourteenth,TFifteenth> Members
			public TFifteenth Fifteenth
			{
				get
				{
					return _fifteenth;
				}
			}
			#endregion
			#region IPair<TFirst,TSecond,TThird,TFourth,TFifth,TSixth,TSeventh,TEighth,TNineth,TTenth,TEleventh,TTwelfth,TThirteenth,TFourteenth,TFifteenth,TSixteenth> Members
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
				IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth> variables = obj as IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth>;

				return (variables != null
						&& object.Equals(_first, variables.First)
						&& object.Equals(_second, variables.Second)
						&& object.Equals(_third, variables.Third)
						&& object.Equals(_fourth, variables.Fourth)
						&& object.Equals(_fifth, variables.Fifth)
						&& object.Equals(_sixth, variables.Sixth)
						&& object.Equals(_seventh, variables.Seventh)
						&& object.Equals(_eighth, variables.Eighth)
						&& object.Equals(_nineth, variables.Nineth)
						&& object.Equals(_tenth, variables.Tenth)
						&& object.Equals(_eleventh, variables.Eleventh)
						&& object.Equals(_twelfth, variables.Twelfth)
						&& object.Equals(_thirteenth, variables.Thirteenth)
						&& object.Equals(_fourteenth, variables.Fourteenth)
						&& object.Equals(_fifteenth, variables.Fifteenth)
						&& object.Equals(_sixteenth, variables.Sixteenth));
			}
			public override int GetHashCode()
			{
				return ((_first == null ? 0 : _first.GetHashCode())
						^ (_second == null ? 0 : _second.GetHashCode())
						^ (_third == null ? 0 : _third.GetHashCode())
						^ (_fourth == null ? 0 : _fourth.GetHashCode())
						^ (_fifth == null ? 0 : _fifth.GetHashCode())
						^ (_sixth == null ? 0 : _sixth.GetHashCode())
						^ (_seventh == null ? 0 : _seventh.GetHashCode())
						^ (_eighth == null ? 0 : _eighth.GetHashCode())
						^ (_nineth == null ? 0 : _nineth.GetHashCode())
						^ (_tenth == null ? 0 : _tenth.GetHashCode())
						^ (_eleventh == null ? 0 : _eleventh.GetHashCode())
						^ (_twelfth == null ? 0 : _twelfth.GetHashCode())
						^ (_thirteenth == null ? 0 : _thirteenth.GetHashCode())
						^ (_fourteenth == null ? 0 : _fourteenth.GetHashCode())
						^ (_fifteenth == null ? 0 : _fifteenth.GetHashCode())
						^ (_sixteenth == null ? 0 : _sixteenth.GetHashCode()));
			}
			public override string ToString()
			{
				return string.Join(Environment.NewLine,
								   "First = " + (_first == null ? string.Empty : _first.ToString()),
								   "Second = " + (_second == null ? string.Empty : _second.ToString()),
								   "Third = " + (_third == null ? string.Empty : _third.ToString()),
								   "Fourth = " + (_fourth == null ? string.Empty : _fourth.ToString()),
								   "Fifth = " + (_fifth == null ? string.Empty : _fifth.ToString()),
								   "Sixth = " + (_sixth == null ? string.Empty : _sixth.ToString()),
								   "Seventh = " + (_seventh == null ? string.Empty : _seventh.ToString()),
								   "Eighth = " + (_eighth == null ? string.Empty : _eighth.ToString()),
								   "Nineth = " + (_nineth == null ? string.Empty : _nineth.ToString()),
								   "Tenth = " + (_tenth == null ? string.Empty : _tenth.ToString()),
								   "Eleventh = " + (_eleventh == null ? string.Empty : _eleventh.ToString()),
								   "Twelfth = " + (_twelfth == null ? string.Empty : _twelfth.ToString()),
								   "Thirteenth = " + (_thirteenth == null ? string.Empty : _thirteenth.ToString()),
								   "Fourteenth = " + (_fourteenth == null ? string.Empty : _fourteenth.ToString()),
								   "Fifteenth = " + (_fifteenth == null ? string.Empty : _fifteenth.ToString()),
								   "Sixteenth = " + (_sixteenth == null ? string.Empty : _sixteenth.ToString()));
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