using System.Diagnostics;
using S  = System.String;
using P  = System.Runtime.CompilerServices.CallerFilePathAttribute;
using M  = System.Runtime.CompilerServices.CallerMemberNameAttribute;
using L  = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Activ.Loggr; using Activ.LogChan;

public static class DebugChan{

    public static bool logToConsole;
    public static int? maxMessages = null;

    public static Logger<LogMessage, object> logger;

    [Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
    public static void Print(
        string arg, object source,
        [P] S path="", [M] S member="", [L] int line=0)
    {
        logger?.Log(
            new LogMessage(arg, path, member, line),
            RemapSource(source), maxMessages);
        if(logToConsole)
            UnityEngine.Debug.Log(arg, source as UnityEngine.Object);
    }

    [Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
    public static void Warn(
        string arg, object source,
        [P] S path="", [M] S member="", [L] int line=0)
    {
        Debug.LogWarning($"{Name(source)}: {arg}",
                         source as UnityEngine.Object);
    }

    static object RemapSource(object arg){
        switch(arg){
            case Component c: return c.gameObject;
            default:          return arg;
        }
    }

    static string Name(object arg){
        switch(arg){
            case GameObject go: return go.name;
            case Component c: return c.gameObject.name;
            default: return arg?.ToString() ?? "null";
        }
    }

}
