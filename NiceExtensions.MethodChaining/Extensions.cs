namespace NiceExtensions.MethodChaining
{
	public static class Extensions
	{
		public static bool IfTrue(this bool value, Action action)
		{
			if (value)
				action();
			return value;
		}
		public static bool IfNot(this bool value, Action action)
		{
			if (!value)
				action();
			return value;
		}

		//public static IEnumerable<T3> CrossSelect<T1,T2,T3,T4,T5>(this List<T1> list1, List<T2> list2, Func<T1,T5> pred1, Func<T2,T5> pred2, Func<T1,T2,T3> action)
		//{
		//    foreach (var item in list1)
		//    {
		//        list2.()
		//    }
		//}
	}
}