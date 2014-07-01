using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Andrei15193.Edesia.Exceptions;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.DataAccess.Sql
{
	public class UserRepository
		: IUserRepository, IDisposable, ITranslator<IDataRecord, ApplicationUser>
	{
		public UserRepository(ITranslator<IDataRecord, Product> productTranslator)
		{
			if (productTranslator == null)
				throw new ArgumentNullException("productTranslator");

			_productTranslator = productTranslator;

			_sqlConnection = new SqlConnection(Environment.GetEnvironmentVariable(string.Format(MvcApplication.AzureConnectionStringFormat, "EdesiaDatabaseConnectionString")));
			_sqlConnection.Open();

			_addUserCommand = _CreateAddUserCommand();
			_getUserCommand = _CreateGetUser();
			_getUsersCommand = _CreateGetUsersCommand();
			_getEmployeesCommand.Connection = _sqlConnection;
			_getUserByPasswordCommand = _CreateGetUserByPasswordCommand();
			_getUserByTokenCommand = _CreateGetUserByTokenCommand();
			_setUserPasswordCommand = _CreateSetUserPasswordCommand();
			_setUserTokenCommand = _CreateSetUserTokenCommand();
			_confirmUserCommand = _CreateConfirmUserCommand();

			_addToCartCommand = _CreateAddToCartCommand();
			_updateCartCommand = _CreateUpdateCartCommand();
			_removeFromCartCommand = _CreateRemoveFromCartCommand();
			_clearShoppingCartCommand = _CreateClearShoppingCartCommand();

			_enrollEmployeeCommand = _CreateEnrollEmployeeCommand();
			_removeEmployeeCommand = _CreateRemoveEmployeeCommand();
			_enrollAdministratorCommand = _CreateEnrollAdministratorCommand();
			_removeAdministratorCommand = _CreateRemoveAdministratorCommand();
			_getShoppingCartCommand = _CreateGetShoppingCartCommand();
		}

		public ITranslator<IDataRecord, Product> ProductTranslator
		{
			get
			{
				return _productTranslator;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("ProductTranslator");

				_productTranslator = value;
			}
		}
		#region IUserRepository Members
		public IEnumerable<ApplicationUser> Users
		{
			get
			{
				_CheckIfDisposed();
				lock (_sqlConnection)
				{
					ICollection<ApplicationUser> users = new LinkedList<ApplicationUser>();

					if (_sqlConnection.State == ConnectionState.Closed)
						_sqlConnection.Open();
					using (IDataReader userDataReader = _getUsersCommand.ExecuteReader())
						while (userDataReader.Read())
							users.Add(Translate(userDataReader));

					return users;
				}
			}
		}
		public IEnumerable<Employee> GetEmployees()
		{
			lock (_sqlConnection)
			{
				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();

				ICollection<Employee> employees = new LinkedList<Employee>();
				using (IDataReader applicationUserDataReader = _getEmployeesCommand.ExecuteReader())
					while (applicationUserDataReader.Read())
						employees.Add(Translate(applicationUserDataReader).TryGetRole<Employee>());

				return employees;
			}
		}

		public bool ConfirmUser(string eMail, string registrationToken)
		{
			_CheckIfDisposed();
			if (string.IsNullOrWhiteSpace(eMail))
				throw new ArgumentNullException("eMail");
			if (string.IsNullOrWhiteSpace(eMail))
				throw new ArgumentException("Cannot be empty or whitespace!", "eMail");

			if (string.IsNullOrWhiteSpace(registrationToken))
				throw new ArgumentNullException("registrationToken");
			if (string.IsNullOrWhiteSpace(registrationToken))
				throw new ArgumentException("Cannot be empty or whitespace!", "registrationToken");

			lock (_sqlConnection)
			{
				_confirmUserCommand.Parameters[Database.ApplicationUsers.EMail].Value = eMail;
				_confirmUserCommand.Parameters[Database.ApplicationUsers.RegistrationToken].Value = registrationToken;

				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				_confirmUserCommand.ExecuteNonQuery();

				return (bool)_confirmUserCommand.Parameters["confirmed"].Value;
			}
		}
		public void Add(ApplicationUser user, string password, string registrationToken)
		{
			_CheckIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");
			if (password == null)
				throw new ArgumentNullException("password");
			if (registrationToken == null)
				throw new ArgumentNullException("registrationToken");

			lock (_sqlConnection)
				try
				{
					_addUserCommand.Parameters[Database.ApplicationUsers.EMail].Value = user.EMailAddress;
					_addUserCommand.Parameters[Database.ApplicationUsers.PasswordHash].Value = _ComputeHash(password);
					_addUserCommand.Parameters[Database.ApplicationUsers.RegistrationToken].Value = registrationToken;
					_addUserCommand.Parameters[Database.ApplicationUsers.FirstName].Value = user.FirstName;
					_addUserCommand.Parameters[Database.ApplicationUsers.LastName].Value = user.LastName;
					_addUserCommand.Parameters[Database.ApplicationUsers.DateRegistered].Value = DateTime.Now;

					if (_sqlConnection.State == ConnectionState.Closed)
						_sqlConnection.Open();
					_addUserCommand.ExecuteNonQuery();
				}
				catch (SqlException sqlException)
				{
					throw new AggregateException(new UniqueEMailAddressException(user.EMailAddress, sqlException));
				}
		}
		public void Update(ApplicationUser user)
		{
			_CheckIfDisposed();
			if (user == null)
				throw new ArgumentNullException("user");

			lock (_sqlConnection)
			{
				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				using (SqlTransaction sqlTransaction = _sqlConnection.BeginTransaction())
				{
					Employee employee = user.TryGetRole<Employee>();

					if (employee != null)
					{
						_enrollEmployeeCommand.Transaction = sqlTransaction;
						_enrollEmployeeCommand.Parameters[Database.ApplicationUsers.EMail].Value = employee.EMailAddress;
						_enrollEmployeeCommand.Parameters[Database.ApplicationUsers.TransportCapacity].Value = employee.TransportCapacity;
						_enrollEmployeeCommand.ExecuteNonQuery();
					}
					else
					{
						_removeEmployeeCommand.Transaction = sqlTransaction;
						_removeEmployeeCommand.Parameters[Database.ApplicationUsers.EMail].Value = user.EMailAddress;
						_removeEmployeeCommand.ExecuteNonQuery();
					}

					if (user.IsInRole<Administrator>())
					{
						_enrollAdministratorCommand.Transaction = sqlTransaction;
						_enrollAdministratorCommand.Parameters[Database.ApplicationUsers.EMail].Value = user.EMailAddress;
						_enrollAdministratorCommand.ExecuteNonQuery();
					}
					else
					{
						_removeAdministratorCommand.Transaction = sqlTransaction;
						_removeAdministratorCommand.Parameters[Database.ApplicationUsers.EMail].Value = user.EMailAddress;
						_removeAdministratorCommand.ExecuteNonQuery();
					}

					sqlTransaction.Commit();
				}
			}
		}
		public ApplicationUser Find(string eMail)
		{
			_CheckIfDisposed();
			if (eMail == null)
				throw new ArgumentNullException("eMail");
			if (string.IsNullOrWhiteSpace(eMail))
				throw new ArgumentException("Cannot be empty or white space!", "eMail");

			lock (_sqlConnection)
			{
				_getUserCommand.Parameters[Database.ApplicationUsers.EMail].Value = eMail;

				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				using (IDataReader applicationUserDataReader = _getUserCommand.ExecuteReader())
					if (applicationUserDataReader.Read())
						return Translate(applicationUserDataReader);
					else
						return null;
			}
		}
		public ApplicationUser Find(string eMail, string authenticationToken, AuthenticationTokenType authenticationTokenType = AuthenticationTokenType.Password)
		{
			_CheckIfDisposed();
			if (eMail == null)
				throw new ArgumentNullException("eMail");
			if (string.IsNullOrWhiteSpace(eMail))
				throw new ArgumentException("Cannot be empty or whitesapce!", "eMail");

			if (authenticationToken == null)
				throw new ArgumentNullException("authenticationToken");

			IDataReader userDataReader = null;
			lock (_sqlConnection)
				try
				{
					if (_sqlConnection.State == ConnectionState.Closed)
						_sqlConnection.Open();
					switch (authenticationTokenType)
					{
						case AuthenticationTokenType.Key:
							_getUserByTokenCommand.Parameters[Database.ApplicationUsers.EMail].Value = eMail;
							_getUserByTokenCommand.Parameters[Database.ApplicationUsers.AuthenticationToken].Value = _ComputeHash(authenticationToken);
							userDataReader = _getUserByTokenCommand.ExecuteReader();
							break;
						case AuthenticationTokenType.Password:
						default:
							_getUserByPasswordCommand.Parameters[Database.ApplicationUsers.EMail].Value = eMail;
							_getUserByPasswordCommand.Parameters[Database.ApplicationUsers.PasswordHash].Value = _ComputeHash(authenticationToken);
							userDataReader = _getUserByPasswordCommand.ExecuteReader();
							break;
					}

					if (userDataReader.Read())
						return Translate(userDataReader);
					else
						return null;
				}
				finally
				{
					if (userDataReader != null)
						userDataReader.Dispose();
				}
		}
		public void SetAuthenticationToken(ApplicationUser applicationUser, string authenticationToken, AuthenticationTokenType authenticationTokenType = AuthenticationTokenType.Password)
		{
			_CheckIfDisposed();
			if (applicationUser == null)
				throw new ArgumentNullException("applicationUser");
			if (authenticationTokenType == AuthenticationTokenType.Password
				&& authenticationToken == null)
				throw new ArgumentNullException("authenticationToken");

			lock (_sqlConnection)
			{
				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				switch (authenticationTokenType)
				{
					case AuthenticationTokenType.Key:
						_setUserTokenCommand.Parameters[Database.ApplicationUsers.EMail].Value = applicationUser.EMailAddress;
						_setUserTokenCommand.Parameters[Database.ApplicationUsers.AuthenticationToken].Value = (authenticationToken == null ? (object)DBNull.Value : (object)_ComputeHash(authenticationToken));
						_setUserTokenCommand.ExecuteNonQuery();
						break;
					case AuthenticationTokenType.Password:
					default:
						_setUserPasswordCommand.Parameters[Database.ApplicationUsers.EMail].Value = applicationUser.EMailAddress;
						_setUserPasswordCommand.Parameters[Database.ApplicationUsers.PasswordHash].Value = _ComputeHash(authenticationToken);
						_setUserPasswordCommand.ExecuteNonQuery();
						break;
				}
			}
		}

		public void AddToShoppingCart(ApplicationUser owner, ShoppingCartEntry shoppingCartEntry)
		{
			_CheckIfDisposed();
			if (owner == null)
				throw new ArgumentNullException("owner");

			lock (_sqlConnection)
			{
				_addToCartCommand.Parameters[Database.ShoppingCarts.Owner].Value = owner.EMailAddress;
				_addToCartCommand.Parameters[Database.ShoppingCarts.Product].Value = shoppingCartEntry.Product.Name;
				_addToCartCommand.Parameters[Database.ShoppingCarts.Quantity].Value = shoppingCartEntry.Quantity;

				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				_addToCartCommand.ExecuteNonQuery();
			}
		}
		public void UpdateShoppingCart(ApplicationUser owner, ShoppingCartEntry shoppingCartEntry)
		{
			_CheckIfDisposed();
			if (owner == null)
				throw new ArgumentNullException("owner");

			lock (_sqlConnection)
			{
				_updateCartCommand.Parameters[Database.ShoppingCarts.Owner].Value = owner.EMailAddress;
				_updateCartCommand.Parameters[Database.ShoppingCarts.Product].Value = shoppingCartEntry.Product.Name;
				_updateCartCommand.Parameters[Database.ShoppingCarts.Quantity].Value = shoppingCartEntry.Quantity;

				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				_updateCartCommand.ExecuteNonQuery();
			}
		}
		public void RemoveFromShoppingCart(ApplicationUser owner, Product product)
		{
			_CheckIfDisposed();
			if (owner == null)
				throw new ArgumentNullException("owner");
			if (product == null)
				throw new ArgumentNullException("product");

			lock (_sqlConnection)
			{
				_removeFromCartCommand.Parameters[Database.ShoppingCarts.Owner].Value = owner.EMailAddress;
				_removeFromCartCommand.Parameters[Database.ShoppingCarts.Product].Value = product.Name;

				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				_removeFromCartCommand.ExecuteNonQuery();
			}
		}
		public void Update(ShoppingCart shoppingCart)
		{
			_CheckIfDisposed();
			if (shoppingCart == null)
				throw new ArgumentNullException("shoppingCart");

			lock (_sqlConnection)
			{
				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				using (SqlTransaction sqlTransaction = _sqlConnection.BeginTransaction(IsolationLevel.Serializable))
				{
					_clearShoppingCartCommand.Transaction = sqlTransaction;
					_clearShoppingCartCommand.Parameters[Database.ShoppingCarts.Owner].Value = shoppingCart.Owner.EMailAddress;
					_clearShoppingCartCommand.ExecuteNonQuery();

					_addToCartCommand.Parameters[Database.ShoppingCarts.Owner].Value = shoppingCart.Owner.EMailAddress;
					foreach (ShoppingCartEntry shoppingCartEntry in shoppingCart)
					{
						_addToCartCommand.Parameters[Database.ShoppingCarts.Product].Value = shoppingCartEntry.Product.Name;
						_addToCartCommand.Parameters[Database.ShoppingCarts.Quantity].Value = shoppingCartEntry.Quantity;
						_addToCartCommand.ExecuteNonQuery();
					}

					sqlTransaction.Commit();
				}
			}
		}
		public void ClearShoppingCart(ApplicationUser owner)
		{
			_CheckIfDisposed();
			if (owner == null)
				throw new ArgumentNullException("owner");

			lock (_sqlConnection)
			{
				_clearShoppingCartCommand.Parameters[Database.ShoppingCarts.Owner].Value = owner.EMailAddress;

				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				_clearShoppingCartCommand.ExecuteNonQuery();
			}
		}
		public ShoppingCart GetShoppingCart(ApplicationUser owner)
		{
			_CheckIfDisposed();
			if (owner == null)
				throw new ArgumentNullException("owner");

			lock (_sqlConnection)
			{
				ICollection<ShoppingCartEntry> shoppingCartEntries = new LinkedList<ShoppingCartEntry>();

				_getShoppingCartCommand.Parameters[Database.ShoppingCarts.Owner].Value = owner.EMailAddress;
				if (_sqlConnection.State == ConnectionState.Closed)
					_sqlConnection.Open();
				using (IDataReader shoppingCartEntryDataReader = _getShoppingCartCommand.ExecuteReader())
					while (shoppingCartEntryDataReader.Read())
						shoppingCartEntries.Add(new ShoppingCartEntry(ProductTranslator.Translate(shoppingCartEntryDataReader),
																	  Convert.ToInt32(shoppingCartEntryDataReader[Database.ShoppingCarts.Quantity])));

				return new ShoppingCart(owner, shoppingCartEntries);
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
		#region ITranslator<IDataRecord,ApplicationUser> Members
		public ApplicationUser Translate(IDataRecord applicationUserDataRecord)
		{
			_CheckIfDisposed();
			if (applicationUserDataRecord == null)
				throw new ArgumentNullException("applicationUserDataRecord");

			ApplicationUser user = new ApplicationUser((string)applicationUserDataRecord[Database.ApplicationUsers.EMail],
													   (string)applicationUserDataRecord[Database.ApplicationUsers.FirstName],
													   (string)applicationUserDataRecord[Database.ApplicationUsers.LastName],
													   (DateTime)applicationUserDataRecord[Database.ApplicationUsers.DateRegistered]);

			if (applicationUserDataRecord.GetBoolean(applicationUserDataRecord.GetOrdinal(Database.ApplicationUsers.IsAdministrator)))
				user = new Administrator(user);

			if (applicationUserDataRecord[Database.ApplicationUsers.TransportCapacity] != DBNull.Value)
				user = new Employee(user, Convert.ToDouble(applicationUserDataRecord[Database.ApplicationUsers.TransportCapacity]));

			return user;
		}
		#endregion

		private string _ComputeHash(string authenticationToken)
		{
			return string.Join(string.Empty, _hashAlgorithm.ComputeHash(Encoding.Default.GetBytes(authenticationToken))
														   .Select(hashByte => hashByte.ToString()));
		}
		private void _CheckIfDisposed()
		{
			if (_isDisposed)
				throw new InvalidOperationException("Instance has been disposed!");
		}
		private SqlCommand _CreateGetUsersCommand()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = "select * from Users order by firstName, lastName",
				CommandType = CommandType.Text
			};
		}

		private SqlCommand _CreateConfirmUserCommand()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = "ConfirmUser",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.ApplicationUsers.EMail, SqlDbType.NVarChar, size: 256),
					new SqlParameter(Database.ApplicationUsers.RegistrationToken, SqlDbType.VarChar, size: 50),
					new SqlParameter("confirmed", SqlDbType.Bit)
					{
						Direction = ParameterDirection.Output
					}
				}
			};
		}
		private SqlCommand _CreateAddUserCommand()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = "AddApplicationUser",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.ApplicationUsers.EMail, SqlDbType.NVarChar, size: 256),
					new SqlParameter(Database.ApplicationUsers.PasswordHash, SqlDbType.VarChar, size: 512),
					new SqlParameter(Database.ApplicationUsers.RegistrationToken, SqlDbType.VarChar, size: 50),
					new SqlParameter(Database.ApplicationUsers.FirstName, SqlDbType.NVarChar, size: 100),
					new SqlParameter(Database.ApplicationUsers.LastName, SqlDbType.NVarChar, size: 50),
					new SqlParameter(Database.ApplicationUsers.DateRegistered, SqlDbType.DateTime)
				}
			};
		}
		private SqlCommand _CreateGetUser()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = string.Format("select * from Users where {0} = @{0}",
											Database.ApplicationUsers.EMail),
				CommandType = CommandType.Text,
				Parameters =
				{
					new SqlParameter(Database.ApplicationUsers.EMail, SqlDbType.NVarChar, size: 256)
				}
			};
		}
		private SqlCommand _CreateGetUserByPasswordCommand()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = string.Format("select * from dbo.GetUser(@{0}, @{1})",
											Database.ApplicationUsers.EMail,
											Database.ApplicationUsers.PasswordHash),
				CommandType = CommandType.Text,
				Parameters =
				{
					new SqlParameter(Database.ApplicationUsers.EMail, SqlDbType.NVarChar, size: 256),
					new SqlParameter(Database.ApplicationUsers.PasswordHash, SqlDbType.VarChar, size: 512)
				}
			};
		}
		private SqlCommand _CreateGetUserByTokenCommand()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = string.Format("select * from dbo.GetUserByToken(@{0}, @{1})",
											Database.ApplicationUsers.EMail,
											Database.ApplicationUsers.AuthenticationToken),
				CommandType = CommandType.Text,
				Parameters =
				{
					new SqlParameter(Database.ApplicationUsers.EMail, SqlDbType.NVarChar, size: 256),
					new SqlParameter(Database.ApplicationUsers.AuthenticationToken, SqlDbType.VarChar, size: 512)
				}
			};
		}
		private SqlCommand _CreateSetUserPasswordCommand()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = "SetPasswordHash",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.ApplicationUsers.EMail, SqlDbType.NVarChar, size: 256),
					new SqlParameter(Database.ApplicationUsers.PasswordHash, SqlDbType.VarChar, size: 512)
				}
			};
		}
		private SqlCommand _CreateSetUserTokenCommand()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = "SetAuthenticationToken",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.ApplicationUsers.EMail, SqlDbType.NVarChar, size: 256),
					new SqlParameter(Database.ApplicationUsers.AuthenticationToken, SqlDbType.VarChar, size: 512)
				}
			};
		}

		private SqlCommand _CreateEnrollEmployeeCommand()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = "EnrollEmployee",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.ApplicationUsers.EMail, SqlDbType.NVarChar, size: 256),
					new SqlParameter(Database.ApplicationUsers.TransportCapacity, SqlDbType.Float, size: 8)
				}
			};
		}
		private SqlCommand _CreateRemoveEmployeeCommand()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = "RemoveEmployee",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.ApplicationUsers.EMail, SqlDbType.NVarChar, size: 256)
				}
			};
		}
		private SqlCommand _CreateEnrollAdministratorCommand()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = "EnrollAdministrator",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.ApplicationUsers.EMail, SqlDbType.NVarChar, size: 256)
				}
			};
		}
		private SqlCommand _CreateRemoveAdministratorCommand()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = "RemoveAdministrator",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.ApplicationUsers.EMail, SqlDbType.NVarChar, size: 256)
				}
			};
		}

		private SqlCommand _CreateClearShoppingCartCommand()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = "ClearShoppingCart",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.ShoppingCarts.Owner, SqlDbType.NVarChar, size: 256)
				}
			};
		}
		private SqlCommand _CreateAddToCartCommand()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = "AddToCart",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.ShoppingCarts.Owner, SqlDbType.NVarChar, size: 256),
					new SqlParameter(Database.ShoppingCarts.Product, SqlDbType.NVarChar, size: 100),
					new SqlParameter(Database.ShoppingCarts.Quantity, SqlDbType.Int)
				}
			};
		}
		private SqlCommand _CreateUpdateCartCommand()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = "UpdateCart",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.ShoppingCarts.Owner, SqlDbType.NVarChar, size: 256),
					new SqlParameter(Database.ShoppingCarts.Product, SqlDbType.NVarChar, size: 100),
					new SqlParameter(Database.ShoppingCarts.Quantity, SqlDbType.Int)
				}
			};
		}
		private SqlCommand _CreateRemoveFromCartCommand()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = "RemoveFromCart",
				CommandType = CommandType.StoredProcedure,
				Parameters =
				{
					new SqlParameter(Database.ShoppingCarts.Owner, SqlDbType.NVarChar, size: 256),
					new SqlParameter(Database.ShoppingCarts.Product, SqlDbType.NVarChar, size: 100)
				}
			};
		}
		private SqlCommand _CreateGetShoppingCartCommand()
		{
			return new SqlCommand
			{
				Connection = _sqlConnection,
				CommandText = string.Format("select * from GetShoppingCart(@{0}) order by name",
											Database.ShoppingCarts.Owner),
				CommandType = CommandType.Text,
				Parameters =
				{
					new SqlParameter(Database.ShoppingCarts.Owner, SqlDbType.NVarChar, size: 256)
				}
			};
		}

		private bool _isDisposed = false;
		private ITranslator<IDataRecord, Product> _productTranslator;
		private readonly HashAlgorithm _hashAlgorithm = new SHA256Managed();

		private readonly SqlConnection _sqlConnection;

		private readonly SqlCommand _confirmUserCommand;
		private readonly SqlCommand _addUserCommand;
		private readonly SqlCommand _getUserCommand;
		private readonly SqlCommand _getUsersCommand;
		private readonly SqlCommand _getEmployeesCommand =
			new SqlCommand
			{
				CommandText = "select * from Employees order by firstName, lastName",
				CommandType = CommandType.Text
			};
		private readonly SqlCommand _getUserByPasswordCommand;
		private readonly SqlCommand _getUserByTokenCommand;
		private readonly SqlCommand _setUserPasswordCommand;
		private readonly SqlCommand _setUserTokenCommand;

		private readonly SqlCommand _enrollAdministratorCommand;
		private readonly SqlCommand _enrollEmployeeCommand;
		private readonly SqlCommand _removeAdministratorCommand;
		private readonly SqlCommand _removeEmployeeCommand;

		private readonly SqlCommand _addToCartCommand;
		private readonly SqlCommand _updateCartCommand;
		private readonly SqlCommand _removeFromCartCommand;
		private readonly SqlCommand _clearShoppingCartCommand;
		private readonly SqlCommand _getShoppingCartCommand;
	}
}