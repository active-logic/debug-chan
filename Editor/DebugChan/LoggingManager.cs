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
                DebugChan.logToConsole = Config.logToConsole;
                DebugChan.maxMessages = Config.maxMessages == 0 ? (int?)null : Config.maxMessages;
                Activ.Loggr.UI.LogWindow.cumulatedMessageCount = 0;
                break;
            case PlayModeStateChange.ExitingPlayMode:
                DebugChan.logger = null;
                break;
        }
    }

}}
