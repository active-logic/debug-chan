**ALPHA VERSION - A RELEASE WILL BE PROVIDED SOON.**

# Prolog

Prolog is an automated logger for C# and Unity 3D:
- Log every function call (with custom exclusions)
- Display logs interactively
- Log to file

Assemblies are instrumented via [Mono.Cecil](https://github.com/jbevain/cecil); sources are not modified.

## Requirements

- Unity 2018 or later
- C# 7.2 or later
- Mono.Cecil (included)

## Install

- Download the latest release and drag the package into your project.
- Delete all `*.csproj` files at the root of your project (relative to the `Assets` folder, this is the parent directory); recommended because earlier versions of Unity (or IDEs) may build assemblies in unexpected locations.

To uninstall, delete the `Prolog` directory from your project.

## Configuration

*Prolog* processes assemblies located under *PROJECT/Library/ScriptAssemblies*. If you wish to exclude dlls, classes or methods, create a *prolog.config* file in your *Assets* directory. Similar to *.gitignore*, list one exclusion per line.

In addition of exclusion paths, the following switches are supported:

- `--disable`: disable logging (also available in GUI)
- `--file`: log to *Assets/log.txt*.

Here is a sample configuration file:

```
--file     # Log output to Assets/log.txt
Utils.dll  # Exclude the Utils module
Foo        # Exclude any class named Foo (ignores namespaces)
Bar.Print  # Don't log Bar.Print(...) overloads (ignores namespaces)
```

*prolog.config* does not explicitly support comments or pattern matching, however you may comment out an exclusion or switch using '#'

**NOTE** - log files may grow large; exclude *Assets/log.txt* from version control.

## Console window

Open the console via *Window > Activ > Prolog*; options:

- *Enable logging* - uncheck to disable logging (scripts will be recompiled)
- *Use selection* - only view logs for the active game object.
- *History* - when checked, show the history; otherwise, only report the current frame (available while pausing).

## Limitations

- Console output is limited to 1000 lines; necessary because Unity's text widget is slow (customize via *Config.cs*)
- Constructors and accessors are not logged.
- On occasion, injecting log statements within a class, assembly or method causes `InvalidProgramException: Invalid IL code in NAMESPACE.CLASS:Method`; as a workaround, add the offending class or method to *prolog.config* and reimport.

## Future work

Aside from minor formatting and stability improvements, the following are considered:

- Exclude non public calls when processing a target module. Helps with logging stable APIs.
- Interactively navigate beyond the latest 1000 lines
- Log stack-trees
- Log parameters and return values
- Persist console window settings
- Debugger/stepper

Support development and keep software free!

<a href='https://ko-fi.com/A0114I97' target='_blank'><img height='36' style='border:0px;height:36px;' src='https://az743702.vo.msecnd.net/cdn/kofi1.png?v=2' border='0' alt='Send a tip' /></a>
