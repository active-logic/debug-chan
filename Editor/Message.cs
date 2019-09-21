using UnityEngine;

namespace Activ.Prolog{
public class Message{

    public int            frame;
    public GameObjectInfo owner;
    public object         source;
    public string         _sourceType,
                          message;

    public Message(int frame, object source, string msg){
        this.frame   = frame;
        this.source  = source;
        this.message = msg;
        this.owner   = (source as Component)?.gameObject;
    }

    public Message(int frame, string sourceType, string msg)
    { this.frame = frame; _sourceType = sourceType; message = msg; }

    public bool isStatic => source == null;

    public string sourceType => source?.GetType().Name ?? _sourceType;

    public static bool operator % (Message x, Message y)
    =>    x.message     == y.message
       && x.source      == y.source
       && x.owner?.name == y.owner?.name
       && x._sourceType == y._sourceType;

    override public string ToString()
    => $"{frame} {owner} {sourceType} {message}";

}}
