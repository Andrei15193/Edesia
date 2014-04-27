namespace Andrei15193.ConstraintSatisfaction.Tuples
{
	public interface IPair<out TFirst>
	{
		TFirst First
		{
			get;
		}
	}

	public interface IPair<out TFirst, out TSecond>
		: IPair<TFirst>
	{
		TSecond Second
		{
			get;
		}
	}

	public interface IPair<out TFirst, out TSecond, out TThird>
		: IPair<TFirst, TSecond>
	{
		TThird Third
		{
			get;
		}
	}

	public interface IPair<out TFirst, out TSecond, out TThird, out TFourth>
		: IPair<TFirst, TSecond, TThird>
	{
		TFourth Fourth
		{
			get;
		}
	}

	public interface IPair<out TFirst, out TSecond, out TThird, out TFourth, out TFifth>
		: IPair<TFirst, TSecond, TThird, TFourth>
	{
		TFifth Fifth
		{
			get;
		}
	}

	public interface IPair<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth>
		: IPair<TFirst, TSecond, TThird, TFourth, TFifth>
	{
		TSixth Sixth
		{
			get;
		}
	}

	public interface IPair<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth, out TSeventh>
		: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>
	{
		TSeventh Seventh
		{
			get;
		}
	}

	public interface IPair<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth, out TSeventh, out TEighth>
		: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>
	{
		TEighth Eighth
		{
			get;
		}
	}

	public interface IPair<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth, out TSeventh, out TEighth, out TNineth>
		: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>
	{
		TNineth Nineth
		{
			get;
		}
	}

	public interface IPair<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth, out TSeventh, out TEighth, out TNineth, out TTenth>
		: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth>
	{
		TTenth Tenth
		{
			get;
		}
	}

	public interface IPair<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth, out TSeventh, out TEighth, out TNineth, out TTenth, out TEleventh>
		: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth>
	{
		TEleventh Eleventh
		{
			get;
		}
	}

	public interface IPair<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth, out TSeventh, out TEighth, out TNineth, out TTenth, out TEleventh, out TTwelfth>
		: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh>
	{
		TTwelfth Twelfth
		{
			get;
		}
	}

	public interface IPair<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth, out TSeventh, out TEighth, out TNineth, out TTenth, out TEleventh, out TTwelfth, out TThirteenth>
		: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth>
	{
		TThirteenth Thirteenth
		{
			get;
		}
	}

	public interface IPair<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth, out TSeventh, out TEighth, out TNineth, out TTenth, out TEleventh, out TTwelfth, out TThirteenth, out TFourteenth>
		: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth>
	{
		TFourteenth Fourteenth
		{
			get;
		}
	}

	public interface IPair<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth, out TSeventh, out TEighth, out TNineth, out TTenth, out TEleventh, out TTwelfth, out TThirteenth, out TFourteenth, out TFifteenth>
		: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>
	{
		TFifteenth Fifteenth
		{
			get;
		}
	}

	public interface IPair<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth, out TSeventh, out TEighth, out TNineth, out TTenth, out TEleventh, out TTwelfth, out TThirteenth, out TFourteenth, out TFifteenth, out TSixteenth>
		: IPair<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNineth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>
	{
		TSixteenth Sixteenth
		{
			get;
		}
	}
}