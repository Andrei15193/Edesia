using System.Collections.Generic;
using System.Linq;
namespace Andrei15193.Edesia.Controllers.Validation
{
	public class ValidationResult
	{
		public ValidationResult(params string[] errors)
		{
			if (errors == null)
				_errors = new string[0];
			else
				_errors = errors.ToList();
		}

		public bool HasErrors()
		{
			return (_errors.Count > 0);
		}
		public IReadOnlyList<string> Errors
		{
			get
			{
				return _errors;
			}
		}

		private readonly IReadOnlyList<string> _errors;
	}
}