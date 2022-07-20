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

    public bool Contains(float time){
        if(time <= start) return false;
        if(end == null) return true;
        return time < end;
    }

    // NOTE: 'Add' or 'ExtendWith' may be better names
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

    public string Format(float time)
    => FormatSpan(time) + "\n" + messages.Format();

    string FormatSpan()
    => $"[ {start.frame} → {end.frame} ]";

    string FormatSpan(float time)
    => $"[ {start.frame} → {end.frame} ] {(time - start.time):0.00}s ago ";

}}
