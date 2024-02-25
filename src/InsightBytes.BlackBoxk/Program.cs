using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using InsightBytes.Utilities.Endpoints;

namespace InsightBytes.BlackBox
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Program p = new Program();
            await p.StartServerAndRequestData();
        }

        async Task StartServerAndRequestData()
        {
            // Start the Go server process
            var process = ProcessStarter();
            try
            {
                // Request data from the Go server
                var users = await GetAllUsers();

                foreach (var user in users)
                {
                    Console.WriteLine($"ID: {user.ID}, Name: {user.Name}, Age: {user.Age}");
                }
            }
            finally
            {
                // Ensure to kill the process when done
                if (!process.HasExited)
                {
                    process.Kill();
                }
            }
        }

        Process ProcessStarter()
        {
            string goServerExePath = @"C:\Users\gabriel.cornejo\source\repos\VSCodeApps\Golang\gin-quickstart\goServerExecutable.exe";
           
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = goServerExePath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            };

            Process process = new Process()
            {
                StartInfo = startInfo
            };

            process.Start();

            return process;
        }

        async Task<List<User>> GetAllUsers()
        {
            List<User> userList = new List<User>();
            Console.WriteLine("Getting users from Go service...");
            Console.WriteLine("##############################\n");

            var httpClient = new HttpClient();
            var golangService = new GolangService(httpClient);

            try
            {
                var users = await golangService.GetUserDataAsync();
                userList.AddRange(users);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting users: {e.Message}");
            }
            finally
            {
                Console.WriteLine("##############################\n");
            }

            return userList;
        }
    }
}
