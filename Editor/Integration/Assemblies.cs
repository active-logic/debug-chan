using System.Collections.Generic;
using File = System.IO.FileInfo;
using Dir  = System.IO.DirectoryInfo;

namespace Activ.Prolog{
public static class Assemblies{

    public static string root = "Library/ScriptAssemblies/";

    public static List<File> all{ get{
        var @out = new List<File>();
        foreach (File f in new Dir(root).GetFiles("*.dll"))
            @out.Add(f);
        return @out;
    }}

    public static List<File> std => all.FindAll(Include);

    public static bool Include(File f){
        var x = f.Name.ToLower();
        return !(
            x.Contains("unity")
            || x.Contains("active.logic")
            || x == "assembly-csharp.dll"
            || x == "activ.prolog.dll"
            || x == "FOOOOO"
        );
    }

}}
