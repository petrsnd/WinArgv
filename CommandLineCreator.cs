using System.Collections.Generic;
using System.Linq;

namespace ArgvToCommandLine
{
    public static class CommandLineCreator
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
                throw new CommandLineCreatorException("Argv cannot be empty for requesting command line, missing Executable");
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
                throw new CommandLineCreatorException("You cannot process a command line from a null argument vector");
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
                throw new CommandLineCreatorException("Element in argument vector cannot be null");
            }
            if (argument == "")
            {
                return "\"\"";
            }
            var processed = ProcessSlashes(argument.GetEnumerator(), "");
            return ProcessWhitespace(processed.GetEnumerator(), false, "");
        }

        private static string ProcessSlashes(IEnumerator<char> argumentIterator, string prevSlashes)
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
                    return ProcessSlashes(argumentIterator, prevSlashes + argumentIterator.Current);
                case '"':
                    // Double all the backslashes because it is followed by a "
                    return prevSlashes.Replace("\\", "\\\\") + "\\" + argumentIterator.Current +
                           ProcessSlashes(argumentIterator, "");
                default:
                    // Any other character means we don't need to escape preceding
                    // backslash with another backslash because they will all be literal.
                    return prevSlashes + argumentIterator.Current + ProcessSlashes(argumentIterator, "");
            }
        }

        private static string ProcessWhitespace(IEnumerator<char> argumentIterator, bool inWhitespace, string prevSlashes)
        {
            // This routine is also based on the information found here:
            // https://msdn.microsoft.com/en-us/library/windows/desktop/17w5ykft(v=vs.85).aspx
            //
            // However, double quotes within double quotes and escaping does NOT
            // work the way any reasonable person would expect.  Quote matching
            // seems to be greedy and the only purpose for quotes is to surround
            // spaces.  Quotes are allowed on the inside of an argument.  Weird.
            if (!argumentIterator.MoveNext())
            {
                return inWhitespace ? "\"" : "";
            }
            if (argumentIterator.Current == ' ' || argumentIterator.Current == '\t')
            {
                // If we are already in whitespace just continuing writing string.
                if (inWhitespace)
                    return argumentIterator.Current + ProcessWhitespace(argumentIterator, true, prevSlashes);
                // If we are just entering whitespace add a double quote before it.
                return "\"" + argumentIterator.Current + ProcessWhitespace(argumentIterator, true, prevSlashes);
            }
            // If we have been in whitespace add a double quote to end it.
            if (inWhitespace)
                return "\"" + argumentIterator.Current + ProcessWhitespace(argumentIterator, false, prevSlashes);
            // Otherwise, just keep writing the string.
            return argumentIterator.Current + ProcessWhitespace(argumentIterator, false, prevSlashes);
        }
    }
}
