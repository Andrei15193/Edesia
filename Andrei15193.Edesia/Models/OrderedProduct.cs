using System;
namespace Andrei15193.Edesia.Models
{
	public struct OrderedProduct
		: IEquatable<OrderedProduct>
	{
		public OrderedProduct(Product product, int quanitity)
		{
			if (product == null)
				throw new ArgumentNullException("product");

			if (quanitity <= 0)
				throw new ArgumentException("Must be stricly positive!", "quantity");

			_product = product;
			_quantity = quanitity;
		}

		public static bool operator ==(OrderedProduct left, OrderedProduct right)
		{
			return left.Equals(right);
		}
		public static bool operator ==(OrderedProduct left, object right)
		{
			return left.Equals(right);
		}
		public static bool operator ==(OrderedProduct left, IEquatable<OrderedProduct> right)
		{
			return left.Equals(right);
		}
		public static bool operator ==(object left, OrderedProduct right)
		{
			return right.Equals(left);
		}
		public static bool operator ==(IEquatable<OrderedProduct> left, OrderedProduct right)
		{
			return right.Equals(left);
		}

		public static bool operator !=(OrderedProduct left, OrderedProduct right)
		{
			return !left.Equals(right);
		}
		public static bool operator !=(OrderedProduct left, object right)
		{
			return !left.Equals(right);
		}
		public static bool operator !=(OrderedProduct left, IEquatable<OrderedProduct> right)
		{
			return !left.Equals(right);
		}
		public static bool operator !=(object left, OrderedProduct right)
		{
			return !right.Equals(left);
		}
		public static bool operator !=(IEquatable<OrderedProduct> left, OrderedProduct right)
		{
			return !right.Equals(left);
		}

		public Product Product
		{
			get
			{
				return _product;
			}
		}
		public int Quantity
		{
			get
			{
				return _quantity;
			}
		}

		#region IEquatable<OrderedProduct> Members
		public bool Equals(OrderedProduct other)
		{
			return (_product.Equals(other._product) && _quantity.Equals(_quantity));
		}
		#endregion
		public override bool Equals(object obj)
		{
			return (obj is OrderedProduct && Equals((OrderedProduct)obj));
		}
		public override int GetHashCode()
		{
			return (_product.GetHashCode() ^ _quantity.GetHashCode());
		}

		private readonly Product _product;
		private readonly int _quantity;
	}
}