using System;
using System.Web.Mvc;
namespace Andrei15193.Edesia.Attributes
{
	public class PasswordAttribute
		: Attribute, IMetadataAware
	{
		#region IMetadataAware Members
		public void OnMetadataCreated(ModelMetadata metadata)
		{
			metadata.TemplateHint = "Password";
		}
		#endregion
	}
}