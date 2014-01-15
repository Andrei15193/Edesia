namespace Andrei15193.Edesia.Models
{
	public class Threshold
	{
		public Threshold(string name, uint limit, User owner)
		{
			Name = name;
			Limit = limit;
			Owner = owner;
		}

		public uint Limit
		{
			get;
			private set;
		}
		public string Name
		{
			get;
			private set;
		}
		public User Owner
		{
			get;
			private set;
		}
	}
}