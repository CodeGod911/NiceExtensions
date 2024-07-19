namespace NiceExtensions.Enumerable.Test
{
	public class CachedEnumerableTests
	{
		private Dictionary<int, string> _pairs = new() { { 1, "test" }, { 2, "test" }, { 3, "test" } };

		[Fact]
		public void ToCachedEnumerableTest()
		{
			var e = _pairs.Select(i => i.Value);
			var c = e.ToCachedEnumerable();
			Assert.Equal(c, e);
			_pairs[1] = "test2";
			Assert.NotEqual(c, e);
		}

		[Fact]
		public void ToCachedAsyncEnumerableTest()
		{
			var e = AsyncGen();
			var c = e.ToCachedAsyncEnumerable();
			Assert.Equal(c.ToBlockingEnumerable(), e.ToBlockingEnumerable());
			_pairs[2] = "test2";
			Assert.NotEqual(c.ToBlockingEnumerable(), e.ToBlockingEnumerable());
		}

		private async IAsyncEnumerable<KeyValuePair<int, string>> AsyncGen()
		{
			foreach (var pair in _pairs)
			{
				yield return await Task.FromResult(pair);
			}
		}
	}
}
