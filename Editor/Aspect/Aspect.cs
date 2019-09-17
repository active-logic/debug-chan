using System.Linq;  // For char in string
using Dir  = System.IO.DirectoryInfo;
using File = System.IO.FileInfo;
using UnityEditor;
using UnityEngine;
using Mono.Cecil;
using Mono.Cecil.Cil;
using static Activ.Prolog.IL.Injector;

namespace Activ.Prolog.IL{
public static class Aspect{

    static bool   verbose = false;
    const  string root    = "Library/ScriptAssemblies/",
                  self    = "Activ.Prolog.dll";

    [UnityEditor.Callbacks.DidReloadScripts]
    public static void Process(){
        if(!Config.enable){ print("Logging disabled; do not inject"); return; }
        foreach (File f in new Dir(root).GetFiles("*.dll"))
            if(!Config.Exclude(f.Name)) ProcessFile(root + f.Name);
    }

    static void ProcessFile(string path){
        if(path.EndsWith(self))
            { print($"Skip self: {self}");                 return; }
        if(path.ToLower().Contains("test"))
            { print($"Skip likely unit tests: {path}");    return; }
        if(path.ToLower().Contains("unity."))
            { print($"Skip likely Engine module: {path}"); return; }
        if(path.EndsWith("Assembly-CSharp-Editor.dll"))
            { print($"Skip editor scripts: {path}");       return; }
        var module  = ModuleDefinition.ReadModule
                              (path, new ReaderParameters { InMemory = true });
        int tc  = module.Types.Count;
        int ptc = 0;
        foreach (var type in module.Types){
            if(Config.Exclude(type.Name)){
                print($"Skip excluded type: {type.Name}");
                continue;
            }
            if (type.IsPublic){
                foreach(MethodDefinition method in type.Methods){
                    // Note: interface and abstract methods do not have a body
                    if( method.IsConstructor
                        || IsAccessor(method)
                        || method.Body==null
                        || Config.Exclude($"{type.Name}.{method.Name}")
                        || IsInjected(method)
                    ) continue;
                    //rint("Type: " + method.ReturnType.Name);
                    Inject(type, method);
                } ptc++;
            }
        } module.Write(path);
        print($"...injected {ptc}/{tc} public types");
    }

    /*
     * There are two formats for injected methods:
     * [ typeName, [+/-]methodName, LogStatic ]
     * [ this    , [+/-]methodName, Log       ]
     */
    static bool IsInjected(MethodDefinition m){
        var i = m.Body.Instructions[1];
        return i.OpCode == OpCodes.Ldstr
            && i.Operand is string s
            && s.Substring(1).StartsWith(m.Name);

    }

    static bool IsAccessor(MethodDefinition m)
    => m.Name.StartsWith("get_") || m.Name.StartsWith("set_");

    static void print(string arg){
        if(verbose) UnityEngine.Debug.Log(arg);
    }

}}
