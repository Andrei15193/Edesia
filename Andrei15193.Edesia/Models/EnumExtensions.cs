using System;
using System.Resources;
namespace Andrei15193.Edesia.Models
{
	public static class EnumExtensions
	{
		public static string LocalizedToString<TEnum>(this TEnum taskState, ResourceManager resourceManager)
			where TEnum : struct
		{
			if (!taskState.GetType().IsEnum)
				throw new ArgumentException("Not instance of an enum type!", "taskState");

			if (resourceManager == null)
				throw new ArgumentNullException("resourceManager");

			return resourceManager.GetString(taskState.ToString());
		}
	}
}