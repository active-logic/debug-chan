# Prolog

Prolog is an automated logger for C# and Unity:
- Log every function call (with custom exclusions)
- Display logs interactively

Assemblies are instrumented via [Mono.Cecil](https://github.com/jbevain/cecil); sources are not modified.

## Requirements

- Unity 2018 or later
- C# 7.2 or later
- Mono.Cecil (included)

## Install

NOTE: ALPHA VERSION - A RELEASE WILL BE PROVIDED SOON.

- Download the latest release and drag the package into your project.
- Delete all `*.csproj` files at the root of your project (relative to the `Assets` folder, this is the parent directory); recommended because earlier versions of Unity (or IDEs) may build assemblies in unexpected locations.

To uninstall, delete the `Prolog` directory from your project.

## Configuration

*Prolog* processes assemblies located under *PROJECT/Library/ScriptAssemblies*. If you wish to exclude dlls, classes or methods, create a *.prolog* file in your *Assets* directory. Similar to *.gitignore*, list one exclusion per line:

```
Utils.dll  # Exclude the Utils module
Foo        # Exclude any class named Foo (ignores namespaces)
Bar.Print  # Don't log Bar.Print(...) overloads (ignores namespaces)
```

*.prolog* does not support comments or pattern matching.

## Console window

Open the console via *Window > Activ > Prolog*; options:

- *Use selection* - only view logs for the active game object.
- *History* - when checked, show the history; otherwise, only report the current frame (available while pausing).

## Limitations

- Console output is limited to 200,000 characters; necessary because Unity's text widget is slow (customize via *Format.cs*)
- Constructors and accessors are not logged.
- On occasion, injecting log statements within a class, assembly or method causes `InvalidProgramException: Invalid IL code in NAMESPACE.CLASS:Method`; as a workaround, add the offending class or method to *.prolog* and reimport.

## Future work

Aside from minor formatting and stability improvements, the following are considered:

- Exclude non public calls when processing a target module. Helps with logging stable APIs.
- Interactively navigate beyond the latest 200k characters
- Logging to files or external consoles
- Logging stack-trees
- Logging parameters and return values
- Collate identical frame ranges (readability, concision)
- Persist console window settings
- Add a debugger/stepper

Support development and keep software free!

<a href='https://ko-fi.com/A0114I97' target='_blank'><img height='36' style='border:0px;height:36px;' src='https://az743702.vo.msecnd.net/cdn/kofi1.png?v=2' border='0' alt='Send a tip' /></a>
