using System;
namespace SudokuSolver
{
	public struct SudokuPosition
	{
		public SudokuPosition(int row, int column, bool isSet = false)
		{
			if (row < 0 || 8 < row)
				throw new ArgumentException("Must be between 0 and 8 (inclusive)", "row");
			if (column < 0 || 8 < column)
				throw new ArgumentException("Must be between 0 and 8 (inclusive)", "column");

			_row = row;
			_column = column;
			_isSet = isSet;
		}

		public int Row
		{
			get
			{
				return _row;
			}
		}
		public int Column
		{
			get
			{
				return _column;
			}
		}
		public override bool Equals(object obj)
		{
			if (!(obj is SudokuPosition))
				return false;

			SudokuPosition other = (SudokuPosition)obj;
			return (_row == other._row && _column == other._column);
		}
		public override int GetHashCode()
		{
			return (_row.GetHashCode() ^ _column.GetHashCode());
		}

		internal bool IsSet
		{
			get
			{
				return _isSet;
			}
		}

		private readonly int _row;
		private readonly int _column;
		private readonly bool _isSet;
	}
}