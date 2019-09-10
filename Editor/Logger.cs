using UnityEngine;

namespace Active.Log{
public static class Logger{

    public static Log _log;

    public static Log log => _log = _log ?? new Log();

    public static void Log(object src, string message)
    => Dispatch(new LogMessage(Time.frameCount, src, message));

    public static void LogStatic(string type, string message)
    => Dispatch(new LogMessage(Time.frameCount, type, message));

    static void Dispatch(LogMessage msg){
        log.Append(msg);
        LogToFile.Log(msg);
        ActiveLogWindow.OnMessage(msg);
    }

}}
