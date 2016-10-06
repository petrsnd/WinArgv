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
            Assert.Throws<CommandLineCreatorException>(() => { CommandLineCreator.GetCommandLine(null); });
        }

        [Fact]
        public void NullArgumentStringTest()
        {
            Assert.Null(CommandLineCreator.GetArgumentString(null));
        }

        [Fact]
        public void EmptyCommandLineTest()
        {
            Assert.Throws<CommandLineCreatorException>(() => { CommandLineCreator.GetCommandLine(new string[] {}); });
            Assert.Throws<CommandLineCreatorException>(() => { CommandLineCreator.GetCommandLine(new List<string>()); });
            Assert.Throws<CommandLineCreatorException>(
                () => { CommandLineCreator.GetCommandLine(new List<string>().AsEnumerable()); });
        }

        [Fact]
        void EmptyArgumentStringTest()
        {
            Assert.Null(CommandLineCreator.GetArgumentString(new string[] { }));
            Assert.Null(CommandLineCreator.GetArgumentString(new List<string>()));
            Assert.Null(CommandLineCreator.GetArgumentString(new List<string>().AsEnumerable()));
        }
    }
}
