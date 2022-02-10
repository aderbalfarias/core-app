namespace CoreApp.Domain.CSharp8
{
    public class NullCoalescingAssignment
    {
        public Task Calling()
        {
            List<int> numbers = null;
            int? i = null;

            numbers ??= new List<int>();
            numbers.Add(i ??= 17);
            numbers.Add(i ??= 20);

            Console.WriteLine(string.Join(" ", numbers));  // output: 17 17
            Console.WriteLine(i);  // output: 17

            return Task.CompletedTask;
        }
    }
}
