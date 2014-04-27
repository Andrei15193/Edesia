namespace Andrei15193.ConstraintSatisfaction.Tuples
{
	public interface IVariables<out TFirst>
		where TFirst : IVariable
	{
		TFirst First
		{
			get;
		}
	}

	public interface IVariables<out TFirst, out TSecond>
		: IVariables<TFirst>
		where TFirst : IVariable
		where TSecond : IVariable
	{
		TSecond Second
		{
			get;
		}
	}

	public interface IVariables<out TFirst, out TSecond, out TThird>
		: IVariables<TFirst, TSecond>
		where TFirst : IVariable
		where TSecond : IVariable
		where TThird : IVariable
	{
		TThird Third
		{
			get;
		}
	}

	public interface IVariables<out TFirst, out TSecond, out TThird, out TFourth>
		: IVariables<TFirst, TSecond, TThird>
		where TFirst : IVariable
		where TSecond : IVariable
		where TThird : IVariable
		where TFourth : IVariable
	{
		TFourth Fourth
		{
			get;
		}
	}

	public interface IVariables<out TFirst, out TSecond, out TThird, out TFourth, out TFifth>
		: IVariables<TFirst, TSecond, TThird, TFourth>
		where TFirst : IVariable
		where TSecond : IVariable
		where TThird : IVariable
		where TFourth : IVariable
		where TFifth : IVariable
	{
		TFifth Fifth
		{
			get;
		}
	}

	public interface IVariables<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth>
		: IVariables<TFirst, TSecond, TThird, TFourth, TFifth>
		where TFirst : IVariable
		where TSecond : IVariable
		where TThird : IVariable
		where TFourth : IVariable
		where TFifth : IVariable
		where TSixth : IVariable
	{
		TSixth Sixth
		{
			get;
		}
	}

	public interface IVariables<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth, out TSeventh>
		: IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>
		where TFirst : IVariable
		where TSecond : IVariable
		where TThird : IVariable
		where TFourth : IVariable
		where TFifth : IVariable
		where TSixth : IVariable
		where TSeventh : IVariable
	{
		TSeventh Seventh
		{
			get;
		}
	}

	public interface IVariables<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth, out TSeventh, out TEighth>
		: IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>
		where TFirst : IVariable
		where TSecond : IVariable
		where TThird : IVariable
		where TFourth : IVariable
		where TFifth : IVariable
		where TSixth : IVariable
		where TSeventh : IVariable
		where TEighth : IVariable
	{
		TEighth Eighth
		{
			get;
		}
	}

	public interface IVariables<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth, out TSeventh, out TEighth, out TNineth>
		: IVariables<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>
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
		TNineth Nineth
		{
			get;
		}
	}

	public interface IVariables<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth, out TSeventh, out TEighth, out TNineth, out TTenth>
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
		where TTenth : IVariable
	{
		TTenth Tenth
		{
			get;
		}
	}

	public interface IVariables<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth, out TSeventh, out TEighth, out TNineth, out TTenth, out TEleventh>
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
		where TEleventh : IVariable
	{
		TEleventh Eleventh
		{
			get;
		}
	}

	public interface IVariables<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth, out TSeventh, out TEighth, out TNineth, out TTenth, out TEleventh, out TTwelfth>
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
		where TTwelfth : IVariable
	{
		TTwelfth Twelfth
		{
			get;
		}
	}

	public interface IVariables<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth, out TSeventh, out TEighth, out TNineth, out TTenth, out TEleventh, out TTwelfth, out TThirteenth>
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
		where TThirteenth : IVariable
	{
		TThirteenth Thirteenth
		{
			get;
		}
	}

	public interface IVariables<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth, out TSeventh, out TEighth, out TNineth, out TTenth, out TEleventh, out TTwelfth, out TThirteenth, out TFourteenth>
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
		where TFourteenth : IVariable
	{
		TFourteenth Fourteenth
		{
			get;
		}
	}

	public interface IVariables<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth, out TSeventh, out TEighth, out TNineth, out TTenth, out TEleventh, out TTwelfth, out TThirteenth, out TFourteenth, out TFifteenth>
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
		where TFifteenth : IVariable
	{
		TFifteenth Fifteenth
		{
			get;
		}
	}

	public interface IVariables<out TFirst, out TSecond, out TThird, out TFourth, out TFifth, out TSixth, out TSeventh, out TEighth, out TNineth, out TTenth, out TEleventh, out TTwelfth, out TThirteenth, out TFourteenth, out TFifteenth, out TSixteenth>
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
		where TSixteenth : IVariable
	{
		TSixteenth Sixteenth
		{
			get;
		}
	}
}