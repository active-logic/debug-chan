# Prolog

Prolog is an automated logger for C# and Unity 3D:
- Log every function call (with custom exclusions)
- Display logs interactively, with an option to view logs on a per game object basis.
- Review call history from scene view
- Log to file

![Prolog](https://github.com/active-logic/Media/blob/master/Prolog/Prolog.png)

Assemblies are instrumented via [Mono.Cecil](https://github.com/jbevain/cecil); sources and build targets (iOS/standalone/etc) are not modified.

## Requirements

- Unity 2018 or later
- C# 7.2 or later
- Mono.Cecil

## Install

### Using the Unity Package Manager (UPM)

If you do not need to modify the source, add `"com.activ.prolog": "https://github.com/active-logic/prolog.git"` to your projects' *Packages/manifest.json*. Otherwise...

- Clone the repository outside of your project.
- Open your project and the package manager window (*window > package manager*)
- Using the [+] button (top-left) choose add from disk and locate **package.json** add the root of this repository
- Package and dependencies will import automatically.

### Legacy package import

- Download the latest [release](https://github.com/active-logic/prolog/releases) and drag the package into your project.

### Setup

Delete all `*.csproj` files at the root of your project (relative to the `Assets` folder, this is the parent directory); recommended because earlier versions of Unity (or IDEs) may build assemblies in unexpected locations.

## Configuration

*Prolog* processes assemblies located under *PROJECT/Library/ScriptAssemblies*. If you wish to exclude dlls, classes or methods, create a *prolog.config* file in your *Assets* directory. Similar to *.gitignore*, list one exclusion per line.

In addition of exclusion paths, the following switches are supported:

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
- Log stack-trees
- Log parameters and return values
- Breakpoints

## License

Prolog is licensed under [Fair Source One](LICENSE). TLDR the first user in your org goes free, or buy me coffee (one coffee per additional user).

Do not send beans; use ko_fi. Do leave a note stating the name of your organization.

<a href='https://ko-fi.com/A0114I97' target='_blank'><img height='36' style='border:0px;height:36px;' src='https://az743702.vo.msecnd.net/cdn/kofi1.png?v=2' border='0' alt='Send a tip' /></a>

You may find information about the Fair source license [here](https://fair.io).
