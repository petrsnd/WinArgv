using ArgvToCommandLine;
using Xunit;

namespace TestArgvToCommandLine
{
    public class ArgumentString
    {
        [Fact]
        public void EmptyString()
        {
            var argv = new[] { "" };
            var arguments = CommandLineCreator.GetArgumentString(argv);
            var parsed = ProcessRunner.GetArgs(arguments);
            Assert.Equal(argv, parsed);
        }

        [Fact]
        public void EmptyStrings()
        {
            var argv = new[] { "", "", "" };
            var arguments = CommandLineCreator.GetArgumentString(argv);
            var parsed = ProcessRunner.GetArgs(arguments);
            Assert.Equal(argv, parsed);
        }

        [Fact]
        public void Carrot()
        {
            var argv = new[] { "^", "^^", "asdf^asdf" };
            var arguments = CommandLineCreator.GetArgumentString(argv);
            var parsed = ProcessRunner.GetArgs(arguments);
            Assert.Equal(argv, parsed);
        }

        [Fact]
        public void Spaces()
        {
            var argv = new[] {"asdf fff", "sdlemdk!2930 asd  ds", "   ", "asdf ", " asd ", "  as d'"};
            var arguments = CommandLineCreator.GetArgumentString(argv);
            var parsed = ProcessRunner.GetArgs(arguments);
            Assert.Equal(argv, parsed);
        }

        [Fact]
        public void Slashes()
        {
            var argv = new[]
            {
                @"\a\sd\ff\ds\d\f\sd\\\sd\f\sd\f\sd\f\sd", @"\", @"\\", @"\\\", @"\\\\", @"asd\s\d\\d\\\",
                @"\ \ \\ \\ \ \\   \\ \ \ \\", @" \ \\ ", @"\@#02 384\1293 8!#$%^&^&(( \9\%^$"
            };
            var arguments = CommandLineCreator.GetArgumentString(argv);
            var parsed = ProcessRunner.GetArgs(arguments);
            Assert.Equal(argv, parsed);
        }

        [Fact]
        public void Quotes()
        {
            var argv = new[]
            {
                @"""", @""" """, @"\"" \""asdf """, @"a""a""a""a""", @"""\"" \\"" \\\"" \\\\""""",
                @"asdf\a23\4\@#$""""$""""""""""""""a\"" \\\""@#"
            };
            var arguments = CommandLineCreator.GetArgumentString(argv);
            var parsed = ProcessRunner.GetArgs(arguments);
            Assert.Equal(argv, parsed);
        }
        
        [Fact]
        public void WhackyComplicated()
        {
            
        }

        [Fact]
        public void MsdnExamples()
        {

        }
    }
}
