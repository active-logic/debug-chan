using UnityEngine;
using Activ.Loggr; using Activ.LogChan;

public static class DebugChan{

    public static bool logToConsole;
    public static int? maxMessages = null;

    public static Logger<string, object> logger;

    public static void Print(string arg, object source){
        logger?.Log(arg, RemapSource(source), maxMessages);
        if(logToConsole)
            Debug.Log(arg, source as UnityEngine.Object);
    }

    static object RemapSource(object arg){
        switch(arg){
            case Component c: return c.gameObject;
            default:          return arg;
        }
    }

}
