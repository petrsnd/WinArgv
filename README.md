[![Build status](https://ci.appveyor.com/api/projects/status/belpoe9qhduusy3j?svg=true)](https://ci.appveyor.com/project/petrsnd/cfa533rs232)

# WinArgv
Windows command line generation from a standard args array (POSIX argv style)

## Background
The C# Process.Start() method receives all arguments as a string to launch an executable.  This places the burden on the caller for
escaping and quoting to ensure that the resultant argument array is properly formed.  This seems easy until there are lots of quotes,
spaces, and non-standard characters in your arguments.  The purpose of this project is to allow the caller to build the array of
arguments just as they expect them to be received by the called executable.  This library will build the properly escaped and quoted
argument string to pass to Process.Start().  Even though this is a simple library, this isn't a problem that should have to be solved
every developer who would like to create a process.
