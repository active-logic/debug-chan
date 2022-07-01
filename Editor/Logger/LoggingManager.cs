using UnityEngine;
using UnityEditor;

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
                DebugChan.logger = new MessageLogger();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                Debug.Log("Clear DebugChan logger");
                DebugChan.logger = null;
                break;
        }
    }

}}
