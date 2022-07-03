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

    public Frame<T> CurrentFrame(S source){
        if(logs.TryGetValue(source, out Log<T> log)){
            return log.current;
        }else{
            return null;
        }
    }

    public Log<T> this[S source] => logs[source];

    // -------------------------------------------------------------

    public delegate void MessageEventHandler(T message, S source);

}}
