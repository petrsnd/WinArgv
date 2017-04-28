using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace TestWinArgv
{
    public static class ProcessRunner
    {
        private static string _testBinary;

        private static string TestBinary
        {
            get
            {
                if (string.IsNullOrEmpty(_testBinary))
                {
                    var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
                    var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
                    var dirPath = Path.GetDirectoryName(codeBasePath);
                    if (dirPath == null)
                    {
                        throw new Exception("Unable to locate directory path for assembly");
                    }
                    var binPath = Path.Combine(dirPath, "CommandLineTester.exe");
                    if (!File.Exists(binPath))
                    {
                        throw new Exception($"Test binary could not be found at {binPath}");
                    }
                    _testBinary = binPath;
                }
                return _testBinary;
            }
        }

        public static string[] GetArgs(string argumentString)
        {

            using (var proc = new Process())
            {
                proc.StartInfo.FileName = TestBinary;
                proc.StartInfo.Arguments = argumentString;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.CreateNoWindow = true;
                if (!proc.Start())
                {
                    throw new Exception("ProcessRunner test binary failed to start");
                }
                var stringList = new List<string>();
                while (!proc.StandardOutput.EndOfStream)
                {
                    var line = proc.StandardOutput.ReadLine();
                    if (line != null)
                    {
                        stringList.Add(line.TrimEnd('\r', '\n'));
                    }
                }
                proc.WaitForExit();
                return stringList.ToArray();
            }
        }
    }
}
