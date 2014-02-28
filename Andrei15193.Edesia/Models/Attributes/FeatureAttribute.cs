using System;
namespace Andrei15193.Edesia.Models.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class FeatureAttribute
		: Attribute
	{
		public FeatureAttribute(string name, string category, string description = null)
		{
			_name = name;
			_description = description;
			_category = (category == null ? string.Empty : description.Trim());
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}
		public string Description
		{
			get
			{
				return _description;
			}
		}
		public string Category
		{
			get
			{
				return _category;
			}
		}

		private readonly string _name;
		private readonly string _description;
		private readonly string _category;
	}
}