namespace NiceExtensions.Enumerable.Tests
{
    public class EagerLoadingEnumerableTests
    {
        [Fact]
        private async Task TestEagerLoadingEnumerableCompletenes()
        {
            var ints = GetInts(true).ToEagerLoadingEnumerable(-1);
            await Task.Delay(100 * 100 + 1000);


            var start = DateTime.Now;
            var intlist = ints.ToList();
            Assert.True(start - DateTime.Now < TimeSpan.FromMilliseconds(100));
            Assert.Equal(intlist, GetInts(false));
        }


        [Fact]
        private async Task TestEagerLoadingAsyncEnumerableFromEnumerableCompletenes()
        {
            var ints = GetInts(true).ToEagerLoadingAsyncEnumerable(-1);
            await Task.Delay(100 * 100 + 1000);

            var start = DateTime.Now;
            var intlist = await ints.ToListAsync();
            Assert.True(start - DateTime.Now < TimeSpan.FromMilliseconds(100));
            Assert.Equal(intlist, GetInts(false));
        }


        [Fact]
        private async Task TestEagerLoadingAsyncEnumerableCompletenes()
        {
            var ints = GetIntsAsync(true).ToEagerLoadingAsyncEnumerable(-1);
            await Task.Delay(100 * 100 + 1000);


            var start = DateTime.Now;
            var intlist = await ints.ToListAsync();
            Assert.True(start - DateTime.Now < TimeSpan.FromMilliseconds(100));
            Assert.Equal(intlist, GetIntsAsync(false));
        }

        public IEnumerable<int> GetInts(bool wait)
        {
            for (int i = 0; i < 100; i++)
            {
                yield return i;
                if (wait)
                    Task.Delay(100).Wait();
            }
        }


        public async IAsyncEnumerable<int> GetIntsAsync(bool wait)
        {
            for (int i = 0; i < 100; i++)
            {
                yield return i;
                if (wait)
                    await Task.Delay(100);
            }
        }
    }
}
