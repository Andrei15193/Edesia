using System;
namespace Andrei15193.ConstraintSatisfaction.Tuples
{
	public class Variable<TName, TValue>
		: IVariable<TName, TValue>
	{
		public Variable(TName name, TValue value = default(TValue))
		{
			if (name == null)
				throw new ArgumentNullException("name");

			_name = name;
			_value = value;
		}

		#region IVariable<TName,TValue> Members
		public TName Name
		{
			get
			{
				return _name;
			}
		}
		public TValue Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}
		#endregion
		#region IVariable Members
		object IVariable.Name
		{
			get
			{
				return _name;
			}
		}
		object IVariable.Value
		{
			get
			{
				return _value;
			}
		}
		#endregion

		private TValue _value;
		private readonly TName _name;
	}

	public static class Variable
	{
		public static IVariable<TName, TValue> Create<TName, TValue>(TName name, TValue value = default(TValue))
		{
			return new Variable<TName, TValue>(name, value);
		}
	}
}