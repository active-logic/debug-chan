using UnityEditor;
using UnityEngine;
using Activ.Loggr;
using Activ.LogChan;

[InitializeOnLoad]
public static class DebugChan{

    public static Logger<string, object> logger;

    public static void Print(string arg, object source){
        logger?.Log(arg, RemapSource(source));
    }

    static object RemapSource(object arg){
        switch(arg){
            case Component c: return c.gameObject;
            default:          return arg;
        }
    }

}
