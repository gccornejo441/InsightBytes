using BenchmarkDotNet.Running;


namespace InsightBytes.BlackBox;

public class Md5VsSha256
{

    internal class Program
    {

        static void Main(string[] args)
        {
            var fileName = @"C:\Users\gabriel.cornejo\source\repos\AvaloniaApps\GetN\src\InsightBytes.Console\LargeTextFile.txt";
            
            GetText(fileName);

            Task.Run(() =>
            {
                Console.WriteLine("Starting Task from run");
                var num = 1 + 1;
                Console.WriteLine($"Some number {num}");
            });
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