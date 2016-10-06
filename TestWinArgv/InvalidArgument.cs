using System.Collections.Generic;
using System.Linq;
using WinArgv;
using Xunit;

namespace TestWinArgv
{
    public class InvalidArgument
    {
        [Fact]
        public void NullCommandLineTest()
        {
            Assert.Throws<ArgvParserException>(() => { ArgvParser.GetCommandLine(null); });
        }

        [Fact]
        public void NullArgumentStringTest()
        {
            Assert.Null(ArgvParser.GetArgumentString(null));
        }

        [Fact]
        public void EmptyCommandLineTest()
        {
            Assert.Throws<ArgvParserException>(() => { ArgvParser.GetCommandLine(new string[] {}); });
            Assert.Throws<ArgvParserException>(() => { ArgvParser.GetCommandLine(new List<string>()); });
            Assert.Throws<ArgvParserException>(
                () => { ArgvParser.GetCommandLine(new List<string>().AsEnumerable()); });
        }

        [Fact]
        void EmptyArgumentStringTest()
        {
            Assert.Null(ArgvParser.GetArgumentString(new string[] { }));
            Assert.Null(ArgvParser.GetArgumentString(new List<string>()));
            Assert.Null(ArgvParser.GetArgumentString(new List<string>().AsEnumerable()));
        }
    }
}
