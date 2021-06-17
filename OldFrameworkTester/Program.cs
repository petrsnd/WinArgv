using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using WinArgv;

namespace OldFrameworkTester
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Any())
            {
                var i = 1;
                foreach (var arg in args)
                {
                    Console.WriteLine($"argv[{i}] = {arg}");
                    i++;
                }
            }
            else
            {
                Console.WriteLine("Manual test application for WinArgv.");
                Console.WriteLine("Enter the literal args to pass and type DONE after your last argument:");

                var argv = new List<string>();
                var assemblyUri = new Uri(Assembly.GetExecutingAssembly().CodeBase);
                var assemblyPath = Uri.UnescapeDataString(assemblyUri.AbsolutePath);
                argv.Add(assemblyPath);
                for (var i = 1; ; i++)
                {
                    Console.Write($"argv[{i}]: ");
                    var arg = Console.ReadLine();
                    if (arg == null)
                        continue;
                    if (arg.Equals("DONE"))
                        break;
                    argv.Add(arg);
                }

                var processStartInfo = ArgvParser.GetProcessStartInfo(argv);

                Console.WriteLine($"Command: {processStartInfo.FileName}");
                Console.WriteLine($"Arguments String: {processStartInfo.Arguments}");
                Console.WriteLine("");
                Console.WriteLine("Press any key...");
                Console.ReadKey();

                processStartInfo.CreateNoWindow = true;
                processStartInfo.LoadUserProfile = false;
                processStartInfo.UseShellExecute = false;
                processStartInfo.RedirectStandardOutput = true;

                var process = Process.Start(processStartInfo);
                if (process == null)
                {
                    Console.WriteLine("Process failed to start :(");
                }
                else
                {
                    while (!process.StandardOutput.EndOfStream)
                    {
                        Console.WriteLine(process.StandardOutput.ReadLine());
                    }
                    process.WaitForExit(1000);
                }

                Console.WriteLine("Press any key...");
                Console.ReadKey();
            }
        }
    }
}
