using System;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace TestArgvToCommandLine
{
    // For the record I tried to use this to validate my command line tests, but it returns
    // BAD results!  It seems to have greedy quote matching and a bunch of other issues.

    // WARNING: DO NOT USE THIS PARSING FUNCTION WITH QUOTED OR ESCAPED COMMAND LINES!


    // Following code is courtesy of a pinvoke.net sample:
    // http://www.pinvoke.net/default.aspx/shell32.commandlinetoargvw

    internal static class CmdLineToArgvW
    {
        public static string[] SplitArgs(string unsplitArgumentLine)
        {
            int numberOfArgs;

            var ptrToSplitArgs = CommandLineToArgvW(unsplitArgumentLine, out numberOfArgs);

            // CommandLineToArgvW returns NULL upon failure.
            if (ptrToSplitArgs == IntPtr.Zero)
                throw new ArgumentException("Unable to split argument.", new Win32Exception());

            // Make sure the memory ptrToSplitArgs to is freed, even upon failure.
            try
            {
                var splitArgs = new string[numberOfArgs];

                // ptrToSplitArgs is an array of pointers to null terminated Unicode strings.
                // Copy each of these strings into our split argument array.
                for (var i = 0; i < numberOfArgs; i++)
                    splitArgs[i] = Marshal.PtrToStringUni(
                        Marshal.ReadIntPtr(ptrToSplitArgs, i * IntPtr.Size));

                return splitArgs;
            }
            finally
            {
                // Free memory obtained by CommandLineToArgW.
                LocalFree(ptrToSplitArgs);
            }
        }

        [DllImport("shell32.dll", SetLastError = true)]
        static extern IntPtr CommandLineToArgvW(
            [MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine,
            out int pNumArgs);

        [DllImport("kernel32.dll")]
        static extern IntPtr LocalFree(IntPtr hMem);
    }
}
