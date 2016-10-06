using System.Linq;
using WinArgv;
using Xunit;

namespace TestWinArgv
{
    public class CommandLine
    {
        [Fact]
        public void SimpleCommand()
        {
            var argv = new[] {"cmd.exe", "/c", @"C:\Windows\System32\notepad.exe"};
            var cmdline = ArgvParser.GetCommandLine(argv);
            Assert.Equal("cmd.exe", cmdline.Executable);
            argv = argv.Skip(1).ToArray();
            var parsed = ProcessRunner.GetArgs(cmdline.Arguments);
            Assert.Equal(argv, parsed);
        }

        [Fact]
        public void FullCommandPath()
        {
            var argv = new[] {@"C:\Windows\System32\cmd.exe", "/c", @"C:\Windows\System32\notepad.exe"};
            var cmdline = ArgvParser.GetCommandLine(argv);
            Assert.Equal(@"C:\Windows\System32\cmd.exe", cmdline.Executable);
            argv = argv.Skip(1).ToArray();
            var parsed = ProcessRunner.GetArgs(cmdline.Arguments);
            Assert.Equal(argv, parsed);
        }
    }
}
