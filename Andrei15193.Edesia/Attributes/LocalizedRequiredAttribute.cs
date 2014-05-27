using System;
using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Resources;
namespace Andrei15193.Edesia.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
	public class LocalizedRequiredAttribute
		: RequiredAttribute
	{
		public LocalizedRequiredAttribute(string errorMessageResourceName, Type errorMessageResourceType)
		{
			AllowEmptyStrings = false;
			ErrorMessage = null;
			ErrorMessageResourceName = errorMessageResourceName;
			ErrorMessageResourceType = errorMessageResourceType;
		}
	}
}