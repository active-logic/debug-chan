using UnityEngine;

namespace Active.Log{
public class Message{

    public int        frame;
    public string     _sourceType, ownerName, message;
    public GameObject owner;
    public object     source;

    public Message(int frame, object source, string msg){
        this.frame   = frame;
        this.source  = source;
        this.message = msg;
        if(source is Component c)
        { owner = c.gameObject; ownerName = owner.name; }
    }

    public Message(int frame, string sourceType, string msg)
    { this.frame = frame; _sourceType = sourceType; message = msg; }

    public bool isStatic => source == null;

    public string sourceType => source?.GetType().Name ?? _sourceType;

    public static bool operator % (Message x, Message y)
    =>  x.message     == y.message &&
        x.source      == y.source &&
        x.owner       == y.owner &&
        x.ownerName   == y.ownerName &&
        x._sourceType == y._sourceType;

    override public string ToString()
    => $"{frame} {ownerName} {sourceType} {message}";

}}
