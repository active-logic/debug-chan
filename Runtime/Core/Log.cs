using UnityEngine;  // for UnityEngine.Time
using System.Collections.Generic;
using Ex = System.Exception;

namespace Activ.Loggr{
/* Stores message associated with an implied source */
public class Log<T>{

    public Frame<T> current { get; private set; }
    List<Range<T>> ranges = new List<Range<T>>();

    public Range<T> At(int φ){
        foreach(var range in ranges){
            if(range.start.frame >= φ) return range;
        }
        return null;
    }

    public int? FirstStopAfter(int? frameIndex){
        if(!frameIndex.HasValue) return null;
        var i = frameIndex.Value;
        foreach(var range in ranges){
            if(range.start.frame > i) return range.start.frame;
        }
        return null;
    }

    public int? LastStopBefore(int? frameIndex){
        if(!frameIndex.HasValue) return null;
        var i = frameIndex.Value;
        for(int k = ranges.Count; k >= 0; k --){
            if(ranges[k].start.frame < i) return ranges[k].start.frame;
        }
        return null;
    }

    public void LogMessage(T message, out int overhead){
        var time = new Stamp(Time.frameCount, Time.time);
        if(current == null){
            current = new Frame<T>(time);
            overhead = 0;
        }else if(time > current.time){
            FinalizeCurrentFrame(out overhead);
            current.Clear(time);
        }else if(time != current.time){
            overhead = 0;
            throw new Ex("Cannot log to prior frame");
        }else{
            overhead = 0;
        }
        current.Add(message);
    }

    public string Format() => ranges.Format();

    void FinalizeCurrentFrame(out int overhead){
        if(current == null){
            overhead = 0;
            return;
        }
        var last = lastRange;
        if(last == null || !last.Include(current)){
            // NOTE array conversion both because size will not
            // change, and we cannot ref copy current.messages
            var newRange = new Range<T>(
                current.time,
                current.messages.ToArray()
            );
            ranges.Add(newRange);
            overhead = newRange.messages.Length;
        }else{
            overhead = 0;
        }
    }

    Range<T> lastRange
    => ranges.Count > 0 ? ranges[ranges.Count-1] : null;

}}
