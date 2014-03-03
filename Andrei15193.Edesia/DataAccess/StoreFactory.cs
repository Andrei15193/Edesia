using System;
using Andrei15193.Edesia.DataAccess.Xml;
namespace Andrei15193.Edesia.DataAccess
{
	public static class StoreFactory
	{
		public static IApplicationUserStore ApplicationUserStore
		{
			get
			{
				return new XmlApplicationUserStore(_xmlDocumentProvider);
			}
		}

		private static readonly XmlDocumentProvider _xmlDocumentProvider = (XmlDocumentProvider)Activator.CreateInstance(typeof(IApplicationUserStore).Assembly.GetType(MvcApplication.EdesiaSettings.StorageSettings.XmlDocumentProviderType, true));
	}
}