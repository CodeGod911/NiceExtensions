namespace NiceExtensions.Enumerable.Test
{
	public class ExtensionsTests
	{
		[Fact]
		public void ReverseTest()
		{
			int[] l = [1, 2, 3];
			var rl = l.Reverse();
			Assert.Equal(rl, [3, 2, 1]);
		}


		[Fact]
		public async Task ForEachTests()
		{
			IEnumerable<string> l = [Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()];
			l.ForEach(x => File.Create(x));
			foreach (var x in l)
			{
				Assert.True(File.Exists(x));
			}


			l = [Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()];
			await l.ForEachAsync(async x => { await Task.Delay(1000); File.Create(x); });
			foreach (var x in l)
			{
				Assert.True(File.Exists(x));
			}

			IAsyncEnumerable<string> al = AsyncGen();
			await al.ForEachAsync(x => File.Create(x));
			foreach (var x in l)
			{
				Assert.True(File.Exists(x));
			}

			al = AsyncGen();
			await al.ForEachAsync(async x => { await Task.Delay(1000); File.Create(x); });
			foreach (var x in l)
			{
				Assert.True(File.Exists(x));
			}
		}

		[Fact]
		public void ToDictionaryTests()
		{
			var dic = new Dictionary<string, string>() { { "key1", "val1" }, { "key2", "val2" }, { "key3", "val3" } };
			var rdic = dic.ToList().ToDictionary();
			Assert.Equal(dic, rdic);
		}


		[Fact]
		public void ByMinTest()
		{
			List<(string, int)> l = [("key1", 1), ("key2", 2), ("key3", 3)];
			Assert.Equal(l.ByMin(i => i.Item2), l[0]);


			List<(string, int)> l2 = [("key3", 3), ("key2", 2), ("key1", 1)];
			Assert.Equal(l2.ByMin(i => i.Item2), l2[2]);
		}

		[Fact]
		public void ByMaxTest()
		{
			List<(string, int)> l = [("key1", 1), ("key2", 2), ("key3", 3)];
			Assert.Equal(l.ByMax(i => i.Item2), l[2]);


			List<(string, int)> l2 = [("key3", 3), ("key2", 2), ("key1", 1)];
			Assert.Equal(l2.ByMax(i => i.Item2), l2[0]);
		}



		private async IAsyncEnumerable<string> AsyncGen()
		{
			await Task.Delay(1000); yield return Guid.NewGuid().ToString();
			await Task.Delay(1000); yield return Guid.NewGuid().ToString();
			await Task.Delay(1000); yield return Guid.NewGuid().ToString();
		}
	}
}
