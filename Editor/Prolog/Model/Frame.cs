using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Config = Activ.Loggr.Config;

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
        if(source == null){
            Debug.LogError("source frame is null");
        }
        if(filter == null){
            Debug.LogError("no filter");
        }
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

    public string Format(int frameNo=-1){
        var x = new StringBuilder();
        if(messages.Count == 0) return "\n(no output)\n";
        FormatFrameNo(frameNo, x);
        //Message prev = null;
        foreach(var m in messages){
            x.Append(MessageFormatter.Format(m) + '\n');
            // prev = m;
            //x.Append(MessageFormatter.Format(m, prev) + '\n'); prev = m;
        }
        return x.ToString();
    }

    void FormatFrameNo(int frameNo, StringBuilder x){
        if(frameNo < 0) return;
        x.Append($"\n#{frameNo} "
         .PadRight(Config.LogLineLength, '-') + '\n');
    }

    override public string ToString() => Format(index);

}}
