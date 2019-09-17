using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using static UnityEngine.Application;
using Activ.Prolog.IL;

namespace Activ.Prolog{
[InitializeOnLoad]
public static class Logger{

    public static int injectionTimeMs;
    public static History history    = new History(ConfigKeys.LogPath);
    public static List<Frame> frames = new List<Frame>();

    static Logger(){
        var w = Stopwatch.StartNew();
        Aspect.Process();
        injectionTimeMs = (int)w.Elapsed.TotalMilliseconds;
    }

    public static void Clear(){
        frames = new List<Frame>();
        history.Clear();
    }

    public static void Log(object src, string message)
    { if(isPlaying) Process(new Message(Time.frameCount, src, message)); }

    public static void LogStatic(string type, string message)
    { if(isPlaying) Process(new Message(Time.frameCount, type, message)); }

    static void Process(Message msg){
        if(frame + msg) return;
        history += frame;
        LogWindowModel.instance.Log(frame);
        LogWindow.instance?.Repaint();
        frame = new Frame(msg);
    }

    static Frame frame{
        get => frames.Count == 0 ? null : frames[frames.Count-1];
        set => frames.Add(value);
    }

}}
