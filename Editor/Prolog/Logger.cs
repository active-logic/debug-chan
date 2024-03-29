using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using Activ.Prolog.IL;
using Activ.Loggr;  // for config keys

namespace Activ.Prolog{
[InitializeOnLoad]
public static class Logger{

    public static event FrameEventHandler onFrame;

    public static int injectionTimeMs;
    public static History history    = new History(ConfigKeys.LogPath);
    public static List<Frame> frames = new List<Frame>();

    static bool isPlaying = false;

    private static void OnPlayState(PlayModeStateChange state){
        switch(state){
            case PlayModeStateChange.EnteredEditMode: break;
            case PlayModeStateChange.ExitingEditMode: break;
            case PlayModeStateChange.EnteredPlayMode:
                isPlaying = true;
                break;
            case PlayModeStateChange.ExitingPlayMode:
                isPlaying = false;
                break;
        }
    }

    static Logger(){
        EditorApplication.playModeStateChanged += OnPlayState;
        var w = Stopwatch.StartNew();
        Aspect.Process();
        injectionTimeMs = (int)w.Elapsed.TotalMilliseconds;
    }

    public static void Clear(){
        frames = new List<Frame>();
        history.Clear();
    }

    public static void Log(object src, string message)
    { if(isPlaying) Process(new Message(Time.frameCount, Time.time, src, message)); }

    public static void LogStatic(string type, string message)
    { if(isPlaying) Process(new Message(Time.frameCount, Time.time, type, message)); }

    static void Process(Message msg){
        if(frame + msg) return;
        history += frame;
        onFrame?.Invoke(frame);
        frame = new Frame(msg);
    }

    static Frame frame{
        get => frames.Count == 0 ? null : frames[frames.Count-1];
        set => frames.Add(value);
    }

    // -------------------------------------------------------------

    public delegate void FrameEventHandler(Frame frame);

}}
