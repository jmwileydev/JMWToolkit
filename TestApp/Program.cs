internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Running test app");

        TestMutexHelper();
    }
}


internal class TestMutexHelper()
{
    private static void TestMutexHelper()
    {
        // This code will test our MutexHelper. It's job is to simply make sure that release is called
        // when the current scope has exited.

    }
}
