using System;
namespace Andrei15193.Edesia.Models
{
	public struct NavigationBarAction
	{
		public NavigationBarAction(string title, string action, string controller, string icon = null)
		{
			if (title == null)
				throw new ArgumentNullException("title");
			if (string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace("title"))
				throw new ArgumentException("Cannot be empty or whitespace!", "title");
			if (action == null)
				throw new ArgumentNullException("action");
			if (string.IsNullOrEmpty(action) || string.IsNullOrWhiteSpace("action"))
				throw new ArgumentException("Cannot be empty or whitespace!", "action");
			if (controller == null)
				throw new ArgumentNullException("controller");
			if (string.IsNullOrEmpty(controller) || string.IsNullOrWhiteSpace("controller"))
				throw new ArgumentException("Cannot be empty or whitespace!", "controller");

			_title = title;
			_action = action;
			_controller = controller;
			_icon = icon;
		}

		public string Title
		{
			get
			{
				return _title;
			}
		}
		public string Action
		{
			get
			{
				return _action;
			}
		}
		public string Controller
		{
			get
			{
				return _controller;
			}
		}
		public string Icon
		{
			get
			{
				return _icon;
			}
		}

		private readonly string _title;
		private readonly string _action;
		private readonly string _controller;
		private readonly string _icon;
	}
}