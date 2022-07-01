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

    static bool   verbose = true;
    const  string root    =  "Library/ScriptAssemblies/",
                  self    = "Activ.Prolog.dll";

    [UnityEditor.Callbacks.DidReloadScripts]
    public static void Process(){
        if(!Config.enableInjection){ print("Logging disabled; do not inject"); return; }
        foreach (File f in PrologConfig.enabledAssemblies){
            ProcessFile(root + f.Name);
        }
    }

    static void ProcessFile(string path){
        ModuleDefinition module;
        try{
            module  = ModuleDefinition.ReadModule
                          (path,
                           new ReaderParameters { InMemory = true });
        }catch(System.BadImageFormatException ex){
            Debug.LogWarning ($"Bad image: {path}\n{ex}");
            return;
        }
        int tc  = module.Types.Count;
        int ptc = 0;
        foreach (var type in module.Types){
            //Debug.Log($"process type -- {type}");
            if(PrologConfig.Exclude(type.Name)){
                print($"Skip excluded type: {type.Name}");
                continue;
            }
            if (type.IsPublic){
                print($"Process type: {type.Name}");
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
        }
        // TODO clarify error cause
        try{
            module.Write(path);
            print( $"@{path}... injected {ptc}/{tc} public types");
        }catch(AssemblyResolutionException ex){
            Debug.LogWarning($"Cannot inject module {module} ({ex})");
        }
    }

    /*
     * There are two formats for injected methods:
     * [ typeName, [+/-]methodName, LogStatic ]
     * [ this    , [+/-]methodName, Log       ]
     */
    static bool IsInjected(MethodDefinition m){
        if(m.Body.Instructions.Count < 2) return false;
        var i = m.Body.Instructions[1];
        return i.OpCode == OpCodes.Ldstr
            && i.Operand is string s
            && s.Length > 1
            && s.Substring(1).StartsWith(m.Name);

    }

    static bool IsAccessor(MethodDefinition m)
    => m.Name.StartsWith("get_") || m.Name.StartsWith("set_");

    static void print(string arg){
        if(verbose) UnityEngine.Debug.Log(arg);
    }

}}
