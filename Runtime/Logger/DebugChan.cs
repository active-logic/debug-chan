using UnityEngine;
using UnityEditor;
using Activ.LogChan;


[InitializeOnLoad]
public static class DebugChan{

    public static MessageLogger logger;

    public static void Print(string arg, object source){
        logger?.Log(arg, source, Time.frameCount);
    }

}
