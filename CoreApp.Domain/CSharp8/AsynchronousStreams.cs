namespace CoreApp.Domain.CSharp8
{
    public class AsynchronousStreams
    {
        public async Task Calling()
        {
            await foreach (var number in GenerateSequence())
                Console.WriteLine(number);
        }

        public static async IAsyncEnumerable<int> GenerateSequence()
        {
            for (int i = 0; i < 20; i++)
            {
                await Task.Delay(100);
                yield return i;
            }
        }
    }
}
