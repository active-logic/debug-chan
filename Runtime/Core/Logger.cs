using System.Collections.Generic;
using UnityEngine;

namespace Activ.Loggr{
public class Logger<T, S>{

    public static event MessageEventHandler messageReceived;

    Dictionary<S, Log<T>> logs = new Dictionary<S, Log<T>>();
    public int messageCount{ get; private set; }

    public void Log(T message, S source, int? maxMessages){
        Log<T> log;
        if(!logs.TryGetValue(source, out log)){
            log = logs[source] = new Log<T>();
        }
        log.LogMessage(message, out int overhead);
        messageReceived?.Invoke(message, source, messageCount);
        UpdateMessageCount(overhead, maxMessages);
    }

    public Frame<T> CurrentFrame(S source){
        if(logs.TryGetValue(source, out Log<T> log)){
            return log.current;
        }else{
            return null;
        }
    }

    public Log<T> this[S source]{ get{
        if(logs.TryGetValue(source, out Log<T> value))
            return value;
        else
            return null;
    }}

    void UpdateMessageCount(int messagesAdded, int? maxMessages){
        messageCount += messagesAdded;
        if(maxMessages.HasValue && messageCount > maxMessages.Value){
            // NOTE - transitional; for now flush messages if count
            // is exceeded
            Debug.LogWarning("DEBUG-CHAN: MAX MESSAGES EXCEEDED; CLEARING LOGS");
            logs = new Dictionary<S, Log<T>>();
            messageCount = 0;
        }
    }

    // -------------------------------------------------------------

    public delegate void MessageEventHandler(
        T message,
        S source,
        int messageCount
    );

}}
