using UnityEngine;  // for UnityEngine.Time
using System.Collections.Generic;
using System.Text;
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

    // -------------------------------------------------------------

    public int count => ranges.Count;

    // -------------------------------------------------------------

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

    public string Format(float since, float time){
        int start = RangeId(since) - 1;
        if(start < 0) start = 0;
        var @out = new StringBuilder();
        var t = time;
        for(var i = start; i < count; i++){
            @out.Append(ranges[i].Format(t) + '\n');
        }
        return @out.ToString();
    }

    // -------------------------------------------------------------

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

    // Return the index of the first range containing time 't'
    int RangeId(float time){
        for(int i = ranges.Count - 1; i >= 0; i--){
            if(ranges[i].Contains(time)) return i;
        }
        return -1;
    }

    Range<T> lastRange
    => ranges.Count > 0 ? ranges[ranges.Count-1] : null;

}}
