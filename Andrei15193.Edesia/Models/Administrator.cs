namespace Andrei15193.Edesia.Models
{
	public class Administrator
		: ApplicationUserRole
	{
		public Administrator(ApplicationUser administrator)
			: base(administrator)
		{
		}
	}
}