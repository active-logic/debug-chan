# ProLog - Documentation

Prolog is an automated logger for C# and Unity. This program performs the following tasks:
- Log every function call (with custom exclusions)
- Display logs interactively

Prolog instruments your assemblies (aka 'dlls') via [Mono.Cecil](http://todo). If you are using AoP or injection, conflicts may occur.

Prolog does not modify sources.

## Requirements

- Unity 2018 or later
- C# 7.2 or later
- Mono.Cecil (included)

## Install

- Download the latest release and drag the package into your project.
- Delete all `*.csproj` files at the root of your project; this is recommended because earlier versions of Unity (and possibly some IDEs) build assemblies in unexpected locations.

To uninstall, delete the `Prolog` directory from your project.

## Configuration

By default, Prolog instruments assemblies under PROJECT/Library/ScriptAssemblies. If you wish to exclude dlls, classes or methods, create a `.prolog` file in your `Assets` directory. Similar to `.gitignore` the file includes one exclusion per line:

```
Utils.dll  # Exclude the Utils module
Foo        # Exclude any class named Foo (ignores namespaces)
Bar.Print  # Don't log method Bar.Print(...) overloads (ignores namespaces)
```

`.prolog` does not support comments or pattern matching.

## Console window

To view logs, open the console via *Window > Activ > Prolog*. The console provides the following options:

- Use selection - only view logs for the selected game object
- History - when checked, show the history; otherwise, only report the current frame.

## Known issues and limitations

- By default, console output is limited to 200,000 characters. Earlier logs are culled; this limitation is necessary because Unity's text widget does not gracefully support large text (customizable via `Format.cs`)
- Constructors and accessors are not logged.
- On occasion, injecting log statements within a class, assembly or method does not work. In such cases, an error looking like `InvalidProgramException: Invalid IL code in NAMESPACE.CLASS:Method` is reported; as a workaround, add the offending class or method to `.prolog` and reimport.

## Future work

Aside from minor formatting and stability improvements, the following are considered:

- An option to exclude non public calls when processing a target module. This is useful to track the uses of a stable API, without logging the details.
- An option to interactively navigate beyond the latest 200k characters
- Logging to files or external consoles
- Logging stack-trees
- Logging parameters and return values

This project is a work in progress. To support development and keep this software free, please consider sending a tip.
