using UnityEngine;
using UnityEditor;
using Activ.Loggr;

namespace Activ.LogChan{
[InitializeOnLoad]
public static class LoggingManager{

    static LoggingManager(){
        EditorApplication.playModeStateChanged += OnPlayState;
    }

    static void OnPlayState(PlayModeStateChange state){
        switch(state){
            case PlayModeStateChange.EnteredPlayMode:
                Debug.Log("Set DebugChan logger");
                DebugChan.logger = new Logger<string, object>();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                Debug.Log("Clear DebugChan logger");
                DebugChan.logger = null;
                break;
        }
    }

}}
