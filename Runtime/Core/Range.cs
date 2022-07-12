using System.Collections.Generic;

namespace Activ.Loggr{
/* Spans contiguous frames holding equivalent content */
public class Range<T> : Formatting{

    public readonly Stamp start;
    public Stamp end {get; private set;}
    public readonly T[] messages;

    public Range(Stamp time, T[] messages){
        start         = time;
        end           = time;
        this.messages = messages;
    }

    public bool Include(Frame<T> arg){
        if(arg.time != end + 1) return false;
        var count = messages.Length;
        if(arg.messages.Count != count) return false;
        for(int i = 0; i < count; i++){
            if(!arg.messages[i].Equals(messages[i])) return false;
        }
        end = arg.time;
        return true;
    }

    public string Format() => FormatSpan() + "\n" + messages.Format();

    string FormatSpan() => start.frame + " => " + end.frame;

}}
