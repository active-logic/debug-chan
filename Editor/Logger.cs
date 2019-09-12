using UnityEngine;
using System.Collections.Generic;

namespace Active.Log{
public static class Logger{

    public static History history    = new History(Config.LogPath);
    public static List<Frame> frames = new List<Frame>();

    public static void Log(object src, string message)
    => Process(new Message(Time.frameCount, src, message));

    public static void LogStatic(string type, string message)
    => Process(new Message(Time.frameCount, type, message));

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
