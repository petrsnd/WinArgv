using System;
using System.Runtime.Serialization;

namespace WinArgv
{
    [Serializable]
    public class ArgvParserException : Exception
    {
        public ArgvParserException()
            : base("Unknown ArgvParserException")
        {
        }

        public ArgvParserException(string message)
            : base(message)
        {
        }

        public ArgvParserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ArgvParserException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
