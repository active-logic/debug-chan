using System.Collections.Generic;
using Ex = System.Exception;

namespace Activ.Loggr{
/* Stores message associated with an implied source */
public class Log<T>{

    Frame<T> current;
    List<Range<T>> ranges = new List<Range<T>>();

    public void LogMessage(T message){
        var time = new Stamp();
        if(time == current.time){
            current.Add(message);
        }else if(time > current.time){
            current = new Frame<T>(time);
        }else{
            throw new Ex("Cannot log to prior frame");
        }
    }

    void FinalizeCurrentFrame(){
        if(current == null) return;
        var last = lastRange;
        if(last == null || !last.Include(current)){
            ranges.Add(new Range<T>(current));
        }
    }

    Range<T> lastRange
    => ranges.Count > 0 ? ranges[ranges.Count-1] : null;

}}
