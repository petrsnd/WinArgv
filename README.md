[![Build status](https://ci.appveyor.com/api/projects/status/t625yp2mviq0iudr?svg=true)](https://ci.appveyor.com/project/petrsnd/winargv)

NuGet.org is the best place to get WinArgv: [Nuget.Org/WinArgv](https://www.nuget.org/packages/WinArgv/)

# WinArgv
Windows command line generation from a standard args array (POSIX argv style)

## Background
The .NET Framework's [`Process.Start()`](https://msdn.microsoft.com/en-us/library/e8zac0ca(v=vs.110).aspx) method receives all 
arguments in a single string to launch an executable. This is the case no matter which overload you call:

[`Process.Start(String, String)`](https://msdn.microsoft.com/en-us/library/h6ak8zt5(v=vs.110).aspx)

or, even if you use a [ProcessStartInfo](https://msdn.microsoft.com/en-us/library/system.diagnostics.processstartinfo(v=vs.110).aspx):

[`Process.Start(ProcessStartInfo)`](https://msdn.microsoft.com/en-us/library/0w4h05yb(v=vs.110).aspx)

This places the burden on the caller for escaping and quoting to ensure that args array is properly formed for the Main() method of 
the executable launched by `Process.Start()`.

This seems easy until there are lots of quotes, spaces, and non-standard characters in your arguments.  The purpose 
of this project is to allow the caller to build the array of arguments just as they expect them to be received by the called 
executable.  This library will build the properly escaped and quoted argument string to pass to Process.Start().  It will also generate
a `ProcessStartInfo` with the `FileName` and `Arguments` fields filled out, and you can customize the rest from there.

Even though this is a simple library, this isn't a problem that should have to be solved every developer who would like to create
a process.
