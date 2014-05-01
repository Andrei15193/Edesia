using System;
using System.ComponentModel.DataAnnotations;
using Andrei15193.Edesia.Resources;
namespace Andrei15193.Edesia.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
	public class LocalizedRegularExpressionAttribute
		: RegularExpressionAttribute
	{
		public LocalizedRegularExpressionAttribute(string pattern, string errorMessageResourceName)
			: base(pattern)
		{
			ErrorMessage = null;
			ErrorMessageResourceName = errorMessageResourceName;
			ErrorMessageResourceType = typeof(ErrorStrings);
		}
	}
}