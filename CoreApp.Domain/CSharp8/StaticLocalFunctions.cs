namespace CoreApp.Domain.CSharp8
{
    public class StaticLocalFunctions
    {
        int M()
        {
            int y;
            LocalFunction();

            return y;

            void LocalFunction() => y = 0;
        }

        int M2()
        {
            int y = 5;
            int x = 7;
            return Add(x, y);

            static int Add(int left, int right) => left + right;
        }
    }
}
