namespace NiceExtensions.Enumerable.Models
{

	internal class EnumerableCache<T>
	{
		public EnumerableCache(bool complete, List<T> values)
		{
			Complete = complete;
			Values = values;
		}

		public bool Complete { get; set; }
		public List<T> Values { get; set; }
	}
}
