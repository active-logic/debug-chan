using System.Collections.Generic;

namespace Activ.Loggr{
public class Logger<T, S>{

    public static event MessageEventHandler messageReceived;

    Dictionary<S, Log<T>> logs = new Dictionary<S, Log<T>>();

    public void Log(T message, S source){
        Log<T> log;
        if(!logs.TryGetValue(source, out log)){
            log = logs[source] = new Log<T>();
        }
        log.LogMessage(message);
        messageReceived?.Invoke(message, source);
    }

    // -------------------------------------------------------------

    public delegate void MessageEventHandler(T message, S source);

}}
