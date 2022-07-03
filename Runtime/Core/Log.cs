using UnityEngine;  // for UnityEngine.Time
using System.Collections.Generic;
using Ex = System.Exception;

namespace Activ.Loggr{
/* Stores message associated with an implied source */
public class Log<T>{

    public Frame<T> current { get; private set; }
    List<Range<T>> ranges = new List<Range<T>>();

    public void LogMessage(T message){
        var time = new Stamp(Time.frameCount, Time.time);
        if(current == null){
            current = new Frame<T>(time);
        }else if(time > current.time){
            FinalizeCurrentFrame();
            current = new Frame<T>(time);
        }else if(time != current.time){
            throw new Ex("Cannot log to prior frame");
        }
        current.Add(message);
    }

    public string Format() => ranges.Format();

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
