using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Active.Log{
public class Frame{

    // Stored separately since derived frames may be empty
    public int index { get; private set; }
    List<LogMessage> messages = new List<LogMessage>();

    public Frame(LogMessage msg){
        messages.Add(msg);
        index = msg.frame;
    }

    Frame(Frame source, GameObject sel){
        index = source.index;
        foreach(var k in source.messages) if(k.owner == sel) messages.Add(k);
    }

    public bool empty => messages.Count == 0;
    public int  count => messages.Count;

    public static Frame operator * (Frame self, GameObject sel)
    => sel ? new Frame(self, sel) : self;

    public static bool operator + (Frame self, LogMessage msg){
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
        if(messages.Count==0) return "Idle\n";
        if(frameNo>0)x.Append($"#{frameNo} ".PadRight(
                                            Config.LogLineLength, '-') + '\n');
        foreach(var k in messages) x.Append(k.Format() + '\n');
        return x.ToString();
    }

    override public string ToString() => Format(index);

}}
