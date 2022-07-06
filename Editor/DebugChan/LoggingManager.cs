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
                DebugChan.logger = new Logger<string, object>();
                DebugChan.logToConsole = Activ.Prolog.Config.logToConsole;
                break;
            case PlayModeStateChange.ExitingPlayMode:
                DebugChan.logger = null;
                break;
        }
    }

}}
