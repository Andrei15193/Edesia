using System;
using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Resources;
using Andrei15193.Edesia.Resources.Strings;
namespace Andrei15193.Edesia.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
	public class LocalizedEMailAddressAttribute
		: DataTypeAttribute
	{
		public LocalizedEMailAddressAttribute(string errorMessageResourceName)
			: base(DataType.EmailAddress)
		{
			_emailAddressAttribute = new EmailAddressAttribute
			{
				ErrorMessage = null,
				ErrorMessageResourceName = errorMessageResourceName,
				ErrorMessageResourceType = typeof(ErrorStrings)
			};
		}
		public LocalizedEMailAddressAttribute()
			: this(ErrorKey.EMailTextBox_MissingValue)
		{
		}

		new public string ErrorMessage
		{
			get
			{
				return _emailAddressAttribute.ErrorMessage;
			}
			set
			{
				_emailAddressAttribute.ErrorMessage = value;
			}
		}
		new public string ErrorMessageResourceName
		{
			get
			{
				return _emailAddressAttribute.ErrorMessageResourceName;
			}
			set
			{
				_emailAddressAttribute.ErrorMessageResourceName = value;
			}
		}
		new public Type ErrorMessageResourceType
		{
			get
			{
				return _emailAddressAttribute.ErrorMessageResourceType;
			}
			set
			{
				_emailAddressAttribute.ErrorMessageResourceType = value;
			}
		}
		public override bool RequiresValidationContext
		{
			get
			{
				return _emailAddressAttribute.RequiresValidationContext;
			}
		}
		public override string FormatErrorMessage(string name)
		{
			return _emailAddressAttribute.FormatErrorMessage(name);
		}
		new public ValidationResult GetValidationResult(object value, ValidationContext validationContext)
		{
			return _emailAddressAttribute.GetValidationResult(value, validationContext);
		}
		public override bool IsValid(object value)
		{
			return _emailAddressAttribute.IsValid(value);
		}
		new public void Validate(object value, string name)
		{
			_emailAddressAttribute.Validate(value, name);
		}
		new public void Validate(object value, ValidationContext validationContext)
		{
			_emailAddressAttribute.Validate(value, validationContext);
		}

		new protected string ErrorMessageString
		{
			get
			{
				return _emailAddressAttribute.ErrorMessage;
			}
		}

		private readonly EmailAddressAttribute _emailAddressAttribute;
	}
}