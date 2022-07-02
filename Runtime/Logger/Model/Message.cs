using UnityEngine;

namespace Activ.LogChan{
public class Message{

    public readonly string content;
    public readonly object source;

    public Message(string content, object source){
        this.content = content;
        this.source = source;
    }

    public bool SourceMatches(object arg){
        if(arg is GameObject && source is Component){
            var beh = (Component)source;
            var obj = (GameObject) arg;
            return beh.gameObject == obj;
        }
        return arg == source;
    }

}}
