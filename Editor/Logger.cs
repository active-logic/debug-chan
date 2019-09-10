using UnityEngine;

namespace Active.Log{
public static class Logger{

    public static Log _log;

    public static Log log => _log = _log ?? new Log();

    public static void Log(object src, string message){
        var msg = log.Append(new LogMessage(Time.frameCount, src, message));
        ActiveLogWindow.OnMessage(msg);
    }

    public static void LogStatic(string type, string message){
        var msg = log.Append(new LogMessage(Time.frameCount, type, message));
        ActiveLogWindow.OnMessage(msg);
    }

}}
