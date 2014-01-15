namespace Andrei15193.Edesia.Models
{
	public class Task
	{
		public string Title
		{
			get;
			private set;
		}
		public TaskState State
		{
			get;
			set;
		}
		public string Description
		{
			get;
			set;
		}
		public User Assignee
		{
			get;
			private set;
		}
	}
}