using System;
using Andrei15193.Edesia.DataAccess.Xml;
using Andrei15193.Edesia.DataAccess.Xml.Azure;
using Andrei15193.Edesia.DataAccess.Xml.Local;
namespace Andrei15193.Edesia.DataAccess
{
	public static class StoreFactory
	{
		public static IApplicationUserStore ApplicationUserStore
		{
			get
			{
				return new XmlApplicationUserStore((IXmlDocumentProvider)Activator.CreateInstance(typeof(IApplicationUserStore).Assembly.GetType(MvcApplication.EdesiaSettings.StorageSettings.XmlDocumentProviderType, true)));
			}
		}
	}
}