using UnityEngine;

namespace Active.Log{
public class LogMessage{

    public int        frame;
    public string     _sourceType, ownerName, message;
    public GameObject owner;
    public object     source;

    public LogMessage(int frame, object source, string msg){
        this.frame = frame;
        if(source is Component c){
            owner     = c.gameObject;
            ownerName = owner.name;
        }
        this.source = source;
        this.message = msg;
    }

    public LogMessage(int frame, string sourceType, string msg){
        this.frame      = frame;
        this._sourceType = sourceType;
        this.message    = msg;
    }

    public bool   isStatic   => source == null;
    public string call       => $"{sourceType}{message}";
    public string sourceType => source?.GetType().Name ?? _sourceType;

    override public string ToString() => $"{frame} {ownerName} {call}";

}}
