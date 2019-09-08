using System.Collections.Generic;
using UnityEngine;

namespace Active.Log{
public class Log{

    public List<LogMessage> messages = new List<LogMessage>();

    public LogMessage Append(LogMessage msg){
        messages.Add(msg);
        return msg;
    }

    public LogMessage lastMessage => messages[messages.Count-1];

    public int size => messages.Count;

}}
