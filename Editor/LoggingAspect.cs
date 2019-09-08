using UnityEditor;  using UnityEngine;
using Mono.Cecil;   using Mono.Cecil.Cil;
using System.Linq;  // For char in string
using Dir  = System.IO.DirectoryInfo;
using File = System.IO.FileInfo;

namespace Active.Log{
public static class LoggingAspect{

    static bool verbose = false;
    //
    const string root = "Library/ScriptAssemblies/";
    const string self = "Activ.Prolog.dll";
    public static bool injectAccessors;

    [UnityEditor.Callbacks.DidReloadScripts]
    public static void Process(){
        File[] files = new Dir(root).GetFiles("*.dll");
        foreach (File f in files) ProcessFile(root + f.Name);
    }

    public static void ProcessFile(string path){
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
                    if(!method.IsConstructor && !IsAccessor(method)){
                        // Interface and abstract methods do not have a body
                        if(method.Body == null) continue;
                        if(!IsInjected(method)) Inject(type, method);
                    }
                } ptc++;
            }
        } module.Write(path);
        print($"...injected {ptc}/{tc} public types");
    }

    public static void Inject(TypeDefinition type, MethodDefinition method){
        if(method.IsStatic) InjectClassMethod(type, method);
        else                InjectInstanceMethod(type, method);
    }

    public static void InjectInstanceMethod(TypeDefinition type,
                                            MethodDefinition method){
        var il    = method.Body.GetILProcessor ();
        var ldstr = il.Create (OpCodes.Ldstr,
         (method.IsPublic ? '+' : '-') + method.Name);
        var call  = il.Create (OpCodes.Call,
           method.Module.ImportReference (
                 typeof (Active.Log.Logger).GetMethod (
                    "Log", new[]{ typeof(object), typeof(string) })));
        il.InsertBefore(First(method), call);
        il.InsertBefore(First(method), ldstr);
        il.InsertBefore(First(method), il.Create(OpCodes.Ldarg_0));  // 'this'
    }

    public static void InjectClassMethod(TypeDefinition type,
                                         MethodDefinition method){
        var il    = method.Body.GetILProcessor ();
        var call  = il.Create (OpCodes.Call,
           method.Module.ImportReference (
                 typeof (Active.Log.Logger).GetMethod (
                    "LogStatic", new[]{ typeof(string), typeof(string) })));
        il.InsertBefore(First(method), call);
        il.InsertBefore(First(method), il.Create(OpCodes.Ldstr,
                               (method.IsPublic ? '+' : '-') + method.Name));
        il.InsertBefore(First(method), il.Create (OpCodes.Ldstr, type.Name));
    }

    /*
     * There are two formats for injected methods:
     * [ typeName, [+/-]methodName, LogStatic ]
     * [ this    , [+/-]methodName, Log       ]
     */
    public static bool IsInjected(MethodDefinition m){
        var i = m.Body.Instructions[1];
        return i.OpCode == OpCodes.Ldstr
            && i.Operand is string s
            && s.EndsWith(m.Name);
    }

    public static bool IsAccessor(MethodDefinition m)
    => m.Name.StartsWith("get_") || m.Name.StartsWith("set_");

    public static Instruction First(MethodDefinition m)
    => m.Body.Instructions[0];

    public static void print(string arg){
        if(verbose) UnityEngine.Debug.Log(arg);
    }

}}
