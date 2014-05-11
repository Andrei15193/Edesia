using System;
using System.Globalization;
using System.Text.RegularExpressions;
namespace Andrei15193.Edesia.Models
{
	public struct Colour
		: IEquatable<Colour>
	{
		public Colour(byte red, byte green, byte blue)
		{
			_red = red;
			_green = green;
			_blue = blue;
		}
		public static Colour Parse(string hexazecimalFormat)
		{
			Match match = Regex.Match(hexazecimalFormat, "#(?<red>[0-9a-fA-F]{2})(?<green>[0-9a-fA-F]{2})(?<blue>[0-9a-fA-F]{2})");
			if (!match.Success)
				throw new FormatException();

			return new Colour(byte.Parse(match.Groups["red"].Value, NumberStyles.HexNumber),
							  byte.Parse(match.Groups["green"].Value, NumberStyles.HexNumber),
							  byte.Parse(match.Groups["blue"].Value, NumberStyles.HexNumber));
		}

		public static bool operator ==(Colour left, Colour right)
		{
			return left.Equals(right);
		}
		public static bool operator ==(Colour left, IEquatable<Colour> right)
		{
			return left.Equals(right);
		}
		public static bool operator ==(Colour left, object right)
		{
			return left.Equals(right);
		}
		public static bool operator ==(IEquatable<Colour> left, Colour right)
		{
			return right.Equals(left);
		}
		public static bool operator ==(object left, Colour right)
		{
			return right.Equals(left);
		}

		public static bool operator !=(Colour left, Colour right)
		{
			return !(left.Equals(right));
		}
		public static bool operator !=(Colour left, object right)
		{
			return !(left.Equals(right));
		}
		public static bool operator !=(Colour left, IEquatable<Colour> right)
		{
			return !(left.Equals(right));
		}
		public static bool operator !=(IEquatable<Colour> left, Colour right)
		{
			return !(right.Equals(left));
		}
		public static bool operator !=(object left, Colour right)
		{
			return !(right.Equals(left));
		}

		#region IEquatable<Colour> Members
		public bool Equals(Colour other)
		{
			return (_red == other._red
					&& _green == other._green
					&& _blue == other._blue);
		}
		#endregion
		public override bool Equals(object obj)
		{
			return (obj is Colour && Equals((Colour)obj));
		}
		public override int GetHashCode()
		{
			return (_red.GetHashCode() ^ _green.GetHashCode() ^ _blue.GetHashCode());
		}
		public override string ToString()
		{
			return string.Format("#{0:X2}{1:X2}{2:X2}", _red, _green, _blue);
		}

		private readonly byte _red;
		private readonly byte _green;
		private readonly byte _blue;
	}
}