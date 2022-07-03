using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Activ.LogChan{
public class Frame{

    public readonly int index;
    public readonly List<Message> messages = new List<Message>(0);

    public Frame(int index) => this.index = index;

    public Message Log(string message, object source){
        var messageObj = new Message(message, source);
        messages.Add(messageObj);
        return messageObj;
    }

    public string Format(){
        var @out = new StringBuilder();
        foreach(var m in messages) @out.Append(m.content + "\n");
        return @out.ToString();
    }

    public string Format(object filter){
        var @out = new StringBuilder();
        foreach(var m in messages){
            if(filter == null || m.SourceMatches(filter))
                @out.Append(m.content + "\n");
        }
        return @out.ToString();
    }

}}
