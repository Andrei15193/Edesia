using System.Web.Mvc;
using Andrei15193.Edesia.Controllers;
using Andrei15193.Edesia.Models;
namespace Andrei15193.Edesia.Views
{
	public abstract class ApplicationWebViewPage
		: WebViewPage
	{
		public new ApplicationUser User
		{
			get
			{
				if (_applicationUser == null)
					_applicationUser = ApplicationController.GetApplicationUser(Context);

				return _applicationUser;
			}
		}

		private ApplicationUser _applicationUser = null;
	}

	public abstract class ApplicationWebViewPage<TModel>
		: WebViewPage<TModel>
	{
		public new ApplicationUser User
		{
			get
			{
				if (_applicationUser == null)
					_applicationUser = ApplicationController.GetApplicationUser(Context);

				return _applicationUser;
			}
		}

		private ApplicationUser _applicationUser = null;
	}
}