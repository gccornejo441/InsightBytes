using BenchmarkDotNet.Running;


namespace InsightBytes.BlackBox;

public class Md5VsSha256
{

    internal class Program
    {

        static void Main(string[] args)
        {
            CancellationTokenSource cts = new();

            // Pass the token to the cancelable operation.
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoSomeWork),cts.Token);
            Thread.Sleep(2500);

            // Request cancellation.
            cts.Cancel();
            Console.WriteLine("Cancellation set in token source...");
            Thread.Sleep(2500);
            // Cancellation should have happened, so call Dispose.
            cts.Dispose();

            //var fileName = @"C:\Users\gabriel.cornejo\source\repos\AvaloniaApps\GetN\src\InsightBytes.Console\LargeTextFile.txt";

            //GetText(fileName);

            //Task.Run(() =>
            //{
            //    Console.WriteLine("Starting Task from run");
            //    var num = 1 + 1;
            //    Console.WriteLine($"Some number {num}");
            //});
        }

        static void DoSomeWork(object? obj)
        {
            if (obj is null)
                return;

            CancellationToken token = (CancellationToken)obj;

            for (int i = 0; i < 100000; i++)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("In iteration {0}, cancellation has been requested...",
                                      i + 1);
                    // Perform cleanup if necessary.
                    //...
                    // Terminate the operation.
                    break;
                }
                // Simulate some work.
                Thread.SpinWait(500000);
            }
        }

        public static Task<string> GetText(string path)
        {
            Console.WriteLine("Starting Task GetText");

            return Task.Run(() =>
            {
                if (File.Exists(path))
                    return File.ReadAllText(path);

                return null;
            });
        }
    }
}