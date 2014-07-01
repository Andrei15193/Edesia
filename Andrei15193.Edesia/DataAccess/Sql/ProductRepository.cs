using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Andrei15193.Edesia.Exceptions;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess.Sql
{
	public class ProductRepository
		: IProductRepository, IDisposable, ITranslator<IDataRecord, Product>
	{
		public ProductRepository()
		{
			_sqlConnection = new SqlConnection(Environment.GetEnvironmentVariable(string.Format(MvcApplication.AzureConnectionStringFormat, "EdesiaDatabaseConnectionString")));
			_sqlConnection.Open();

			_addProductCommand = _CreateAddProductCommand();
			_removeProductCommand = _CreateRemoveProductCommand();
			_getProductsCommand = _CreateGetProductsCommand();
			_getProductCommand = _CreateGetProductCommand();
		}

		#region IProductRepository Members
		public Product GetProduct(string productName)
		{
			_CheckIfDisposed();
			if (productName == null)
				throw new ArgumentNullException("productName");
			if (string.IsNullOrWhiteSpace(productName))
				throw new ArgumentException("Cannot be empty or white space!", "productName");

			lock (_sqlConnection)
			{
				_getProductCommand.Parameters[Database.Products.Name].Value = productName;
				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				using (IDataReader productDataReader = _getProductCommand.ExecuteReader())
					if (productDataReader.Read())
						return Translate(productDataReader);
					else
						return null;
			}
		}
		public IEnumerable<Product> GetProducts()
		{
			_CheckIfDisposed();
			lock (_sqlConnection)
			{
				ICollection<Product> products = new LinkedList<Product>();

				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				using (IDataReader productDataReader = _getProductsCommand.ExecuteReader())
					while (productDataReader.Read())
						products.Add(Translate(productDataReader));

				return products;
			}
		}
		public void Add(Product product)
		{
			_CheckIfDisposed();
			if (product == null)
				throw new ArgumentNullException("product");

			lock (_sqlConnection)
				try
				{
					_addProductCommand.Parameters[Database.Products.Name].Value = product.Name;
					_addProductCommand.Parameters[Database.Products.Price].Value = product.Price;
					_addProductCommand.Parameters[Database.Products.Capacity].Value = product.Capacity;
					_addProductCommand.Parameters[Database.Products.ImageLocation].Value = product.ImageLocation.AbsoluteUri;

					if (_sqlConnection.State == ConnectionState.Closed)
						_sqlConnection.Open();
					_addProductCommand.ExecuteNonQuery();
				}
				catch (SqlException sqlException)
				{
					throw new AggregateException(new UniqueProductException(product.Name, sqlException));
				}
		}
		public void Remove(string productName)
		{
			_CheckIfDisposed();
			if (productName == null)
				throw new ArgumentNullException("productName");
			if (string.IsNullOrWhiteSpace(productName))
				throw new ArgumentException("Cannot be empty or white space!", "productName");

			lock (_sqlConnection)
			{
				_removeProductCommand.Parameters[Database.Products.Name].Value = productName;

				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				_removeProductCommand.ExecuteNonQuery();
			}
		}
		#endregion
		#region IDisposable Members
		public void Dispose()
		{
			_CheckIfDisposed();

			_isDisposed = true;
			_sqlConnection.Dispose();
			GC.SuppressFinalize(this);
		}
		#endregion
		#region ITranslator<IDataRecord,Product> Members
		public Product Translate(IDataRecord productDataRecord)
		{
			_CheckIfDisposed();
			if (productDataRecord == null)
				throw new ArgumentNullException("productDataRecord");

			return new Product((string)productDataRecord[Database.Products.Name],
							   Convert.ToDouble(productDataRecord[Database.Products.Price]),
							   Convert.ToDouble(productDataRecord[Database.Products.Capacity]),
							   new Uri((string)productDataRecord[Database.Products.ImageLocation], UriKind.Absolute));
		}
		#endregion


		#region IProductProvider Members
		public Product GetProduct(string name, DateTime version)
		{
			throw new NotImplementedException();
		}
		#endregion

		private void _CheckIfDisposed()
		{
			if (_isDisposed)
				throw new InvalidOperationException("Instance has been disposed!");
		}
		private SqlCommand _CreateGetProductCommand()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = string.Format("select * from GetProduct(@{0})",
											Database.Products.Name),
				CommandType = CommandType.Text,
				Parameters =
				{
					new SqlParameter(Database.Products.Name, SqlDbType.NVarChar, size: 100)
				}
			};
		}
		private SqlCommand _CreateGetProductsCommand()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = "select * from ActualProducts order by name",
				CommandType = CommandType.Text
			};
		}
		private SqlCommand _CreateAddProductCommand()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = "AddProduct",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.Products.Name, SqlDbType.NVarChar, size: 100),
					new SqlParameter(Database.Products.Price, SqlDbType.Float, size: 8),
					new SqlParameter(Database.Products.Capacity, SqlDbType.Float, size: 8),
					new SqlParameter(Database.Products.ImageLocation, SqlDbType.NVarChar, size: 1024)
				}
			};
		}
		private SqlCommand _CreateRemoveProductCommand()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = "RemoveProduct",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.Products.Name, SqlDbType.NVarChar, size: 100)
				}
			};
		}

		private bool _isDisposed = false;
		private readonly SqlConnection _sqlConnection;

		private readonly SqlCommand _getProductCommand;
		private readonly SqlCommand _addProductCommand;
		private readonly SqlCommand _removeProductCommand;
		private readonly SqlCommand _getProductsCommand;
	}
}