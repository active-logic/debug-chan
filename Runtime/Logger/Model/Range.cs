using System.Collections.Generic;

namespace Activ.LogChan{
public class Range{

    public readonly int start;
    public int end {get; private set; }
    public readonly List<Message> messages;

    public Range(Frame arg){
        start    = arg.index;
        end      = arg.index + 1;
        messages = arg.messages;
    }

    public bool Include(Frame arg){
        var c = messages.Count;
        if(arg.messages.Count != c) return false;
        for(int i = 0; i < c; i++){
            if(arg.messages[i] != messages[i]) return false;
        }
        end = arg.index + 1;
        return true;
    }

}}
