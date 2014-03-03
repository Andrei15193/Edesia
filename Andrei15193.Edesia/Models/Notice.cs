using System.Collections.Generic;
namespace Andrei15193.Edesia.Models
{
	public struct Notice
	{
		public Notice(string pageTitle, string title = null, params string[] paragraphs)
		{
			_pageTitle = (pageTitle ?? string.Empty);
			_title = title;
			_paragraphs = paragraphs;
		}

		public string PageTitle
		{
			get
			{
				return _pageTitle;
			}
		}
		public string Title
		{
			get
			{
				return _title;
			}
		}
		public IReadOnlyList<string> Paragraphs
		{
			get
			{
				return _paragraphs;
			}
		}

		private readonly string _title;
		private readonly string _pageTitle;
		private readonly string[] _paragraphs;
	}
}