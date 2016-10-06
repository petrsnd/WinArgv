using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WinArgv
{
    public static class ArgvParser
    {
        public static string GetArgumentString(IEnumerable<string> argv)
        {
            return argv == null ? null : HandleRemainingArgv(argv.GetEnumerator());
        }

        public static string GetArgumentString(string[] argv)
        {
            return argv == null ? null : GetArgumentString(argv.AsEnumerable());
        }

        public static string GetArgumentString(IList<string> argv)
        {
            return argv == null ? null : GetArgumentString(argv.AsEnumerable());
        }

        public static CommandLine GetCommandLine(IEnumerable<string> argv)
        {
            CheckForNullArgumentToGetCommandLine(argv);
            var argvIterator = argv.GetEnumerator();
            if (!argvIterator.MoveNext())
            {
                throw new ArgvParserException("Argv cannot be empty for requesting command line, missing Executable");
            }
            return new CommandLine
            {
                Executable = ProcessExecutableString(argvIterator.Current),
                Arguments = HandleRemainingArgv(argvIterator)
            };
        }

        public static CommandLine GetCommandLine(string[] argv)
        {
            CheckForNullArgumentToGetCommandLine(argv);
            return GetCommandLine(argv.AsEnumerable());
        }

        public static CommandLine GetCommandLine(IList<string> argv)
        {
            CheckForNullArgumentToGetCommandLine(argv);
            return GetCommandLine(argv.AsEnumerable());
        }

        public static ProcessStartInfo GetProcessStartInfo(IEnumerable<string> argv)
        {
            CheckForNullArgumentToGetCommandLine(argv);
            var argvIterator = argv.GetEnumerator();
            if (!argvIterator.MoveNext())
            {
                throw new ArgvParserException("Argv cannot be empty for requesting command line, missing Executable");
            }
            return new ProcessStartInfo
            {
                FileName= ProcessExecutableString(argvIterator.Current),
                Arguments = HandleRemainingArgv(argvIterator)
            };
        }

        public static ProcessStartInfo GetProcessStartInfo(string[] argv)
        {
            CheckForNullArgumentToGetCommandLine(argv);
            return GetProcessStartInfo(argv.AsEnumerable());
        }

        public static ProcessStartInfo GetProcessStartInfo(IList<string> argv)
        {
            CheckForNullArgumentToGetCommandLine(argv);
            return GetProcessStartInfo(argv.AsEnumerable());
        }

        private static string ProcessExecutableString(string executable)
        {
            // For now, I'm not going to doctor this... shouldn't have to.
            return executable;
        }

        // ReSharper disable once UnusedParameter.Local
        private static void CheckForNullArgumentToGetCommandLine(object argv)
        {
            if (argv == null)
            {
                throw new ArgvParserException("You cannot process a command line from a null argument vector");
            }
        }

        private static string HandleRemainingArgv(IEnumerator<string> argvIterator)
        {
            string argumentString = null;
            while (argvIterator.MoveNext())
            {
                if (argumentString == null)
                {
                    argumentString = ProcessArgumentString(argvIterator.Current);
                }
                else
                {
                    argumentString += (" " + ProcessArgumentString(argvIterator.Current));
                }
            }
            return argumentString;
        }

        private static string ProcessArgumentString(string argument)
        {
            if (argument == null)
            {
                throw new ArgvParserException("Element in argument vector cannot be null");
            }
            if (argument == "")
            {
                return "\"\"";
            }
            var processed = ProcessSlashesAndDoubleQuotes(argument.GetEnumerator(), "");
            return ProcessWhitespaceWithQuotesAndHandleSlashes(processed.GetEnumerator(), false, "");
        }

        private static string ProcessSlashesAndDoubleQuotes(IEnumerator<char> argumentIterator, string prevSlashes)
        {
            // This routine is based on the information found here:
            // https://msdn.microsoft.com/en-us/library/windows/desktop/17w5ykft(v=vs.85).aspx
            if (!argumentIterator.MoveNext())
            {
                // The string ends with a \ which means we need to double them
                // all because we are going to wrap the string in double quotes.
                return prevSlashes;
            }
            switch (argumentIterator.Current)
            {
                case '\\':
                    // Accumulate the backslash as keep going
                    return ProcessSlashesAndDoubleQuotes(argumentIterator, prevSlashes + argumentIterator.Current);
                case '"':
                    // Double all the backslashes because it is followed by a "
                    return prevSlashes.Replace("\\", "\\\\") + "\\" + argumentIterator.Current +
                           ProcessSlashesAndDoubleQuotes(argumentIterator, "");
                default:
                    // Any other character means we don't need to escape preceding
                    // backslash with another backslash because they will all be literal.
                    return prevSlashes + argumentIterator.Current + ProcessSlashesAndDoubleQuotes(argumentIterator, "");
            }
        }

        private static string ProcessWhitespaceWithQuotesAndHandleSlashes(IEnumerator<char> argumentIterator,
            bool inWhitespace, string prevSlashes)
        {
            // This routine is also based on the information found here:
            // https://msdn.microsoft.com/en-us/library/windows/desktop/17w5ykft(v=vs.85).aspx
            //
            // However, double quotes within double quotes and escaping does NOT
            // work the way any reasonable person would expect.  Quote matching
            // seems to be greedy and the only purpose for quotes is to surround
            // spaces.  Quotes are allowed on the inside of an argument.  Weird.

            // If we are on the last token, then end with a quote if in whitespace
            // or an empty string as we return to unroll the recursion.
            if (!argumentIterator.MoveNext())
            {
                return inWhitespace ? "\"" : "";
            }

            // The next token is horizontal whitespace
            if (argumentIterator.Current == ' ' || argumentIterator.Current == '\t')
            {
                // If we are already in whitespace, then just continuing writing string.
                if (inWhitespace)
                    return argumentIterator.Current +
                           ProcessWhitespaceWithQuotesAndHandleSlashes(argumentIterator, true, "");
                // If we are just entering whitespace, then add a double quote before it
                // but first add any accumulated slashes to duplicate them
                return prevSlashes + "\"" + argumentIterator.Current +
                       ProcessWhitespaceWithQuotesAndHandleSlashes(argumentIterator, true, "");
            }

            // If we have been in whitespace, then add a double quote to end it. If the
            // next token is a slash don't forget to accumulate it.
            if (inWhitespace)
                return "\"" + argumentIterator.Current +
                       ProcessWhitespaceWithQuotesAndHandleSlashes(argumentIterator, false,
                           argumentIterator.Current == '\\' ? prevSlashes + "\\" : "");

            // If we just encountered a slash then accumulate it as you continue. 
            // Otherwise, just continue writing the string.
            return argumentIterator.Current +
                   ProcessWhitespaceWithQuotesAndHandleSlashes(argumentIterator, false,
                       argumentIterator.Current == '\\' ? prevSlashes + "\\" : "");
        }
    }
}
