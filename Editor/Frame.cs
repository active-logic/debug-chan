using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Activ.Prolog{
public class Frame{

    // Stored separately since derived frames may be empty
    public readonly int index;
    public readonly List<Message> messages = new List<Message>();

    public Frame(Message msg){
        messages.Add(msg);
        index = msg.frame;
    }

    Frame(Frame source, Filter filter){
        index = source.index;
        foreach(var k in source.messages)
            if(filter.Accept(k)) messages.Add(k);
    }

    public bool empty => messages.Count == 0;
    public int  count => messages.Count;

    public static Frame operator * (Frame self, Filter filter)
    => filter.isNeutral ? self : new Frame(self, filter);

    public static bool operator + (Frame self, Message msg){
        if(msg.frame != self?.index) return false;
        self.messages.Add(msg);
        return true;
    }

    public static bool operator % (Frame x, Frame y){
        if(x == null ^ y == null) return false;
        if(x == null && y == null) return true;
        if(x.count != y.count) return false;
        for(int i = 0; i < x.count; i++){
            if(x.messages[i] % y.messages[i]) continue;
            return false;
        } return true;
    }

    public string Format(int frameNo = -1){
        var x = new StringBuilder();
        if(messages.Count==0) return "\n(no output)\n";
        if(frameNo>0)x.Append($"\n#{frameNo} ".PadRight(
                                            Config.LogLineLength, '-') + '\n');
        Message prev = null; foreach(var m in messages){
            x.Append(MessageFormatter.Format(m, prev) + '\n'); prev = m;
        }
        return x.ToString();
    }

    override public string ToString() => Format(index);

}}
