using System;
using System.Runtime.Serialization;

namespace ArgvToCommandLine
{
    [Serializable]
    public class CommandLineCreatorException : Exception
    {
        public CommandLineCreatorException()
            : base("Unknown CommandLineCreatorException")
        {
        }

        public CommandLineCreatorException(string message)
            : base(message)
        {
        }

        public CommandLineCreatorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CommandLineCreatorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
