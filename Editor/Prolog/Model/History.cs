using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Config = Activ.Loggr.Config;

namespace Activ.Prolog{
public class History{

    public List<Frame> frames { get; private set; }
    string path;
    Filter filter = new Filter(null, "any");

    // -------------------------------------------------------------

    public History(string path = null){
        this.path   = path;
        this.frames = new List<Frame>();
        if(path != null){ File.Delete(path); File.Delete(path + ".meta"); }
    }

    public History(Filter filter){
        this.filter = filter;
        this.frames = new List<Frame>();
        foreach(var frame in Logger.frames){ var self = this + frame; }
    }

    // Public properties -------------------------------------------

    public int count => frames.Count;

    public bool empty => !this == 0;

    public Frame last{
        get => empty ? null : frames[!this-1];
        set => frames.Add(value);
    }

    public Frame this[int i] => frames[i];

    public Frame this[int i, bool @default]
    => (i >= frames.Count && @default) ? null : frames[i];

    // Public functions --------------------------------------------

    public Frame At(int φ) => this[RangeId(φ)];

    public void Clear() => frames = new List<Frame>();

    public int End(int i)  // End of range at i
    => i == !this - 1 ? Time.frameCount : frames[i + 1].index - 1;

    // TODO support for selection here is... quirky?
    public int? FirstStopAfter(int? frameIndex, object src){
        if(!frameIndex.HasValue) return null;
        var i = frameIndex.Value;
        foreach(var range in frames){
            if(range.index > i) return range.index;
        }
        return null;
    }

    // TODO support for selection here is... quirky?
    public int? LastStopBefore(int? frameIndex, object src){
        if(!frameIndex.HasValue) return null;
        var i = frameIndex.Value;
        for(int k = frames.Count; k >= 0; k --){
            if(frames[k].index < i) return frames[k].index;
        }
        return null;
    }

    public Frame Next(Frame x)
    => frames[ System.Math.Min(frames.IndexOf(x) + 1, frames.Count-1) ];

    public Frame Prev(Frame x)
    => frames[ System.Math.Max(frames.IndexOf(x) - 1, 0) ];

    // given the specified frame, return the
    // index of the first range containing frame φ
    public int RangeId(float t){
        for(int i = 0; i < frames.Count; i++){
            if(ContainsTimeValue(rangeId: i, t)) return i;
        }
        return -1;
    }

    // given the specified frame, return the
    // index of the first range containing frame φ
    public int RangeId(int φ){
        for(int i = 0; i < frames.Count; i++){
            if(ContainsFrameIndex(rangeId: i, φ)) return i;
        }
        return -1;
    }

    // Operators ---------------------------------------------------

    public static History operator + (History self, Frame frame){
        if(frame == null) return self;
        if(self == null) return self;
        frame *= self.filter;
        if(frame % self.last) return self;
        if(Config.logToFile && self.path != null){
            using(var writer = File.AppendText(self.path))
            { writer.Write(self.last); }
        } self.last = frame;
        return self;
    }

    public static int operator ! (History self)
    => self == null ? 0 : self.frames.Count;

    public static History operator / (History self, Filter filter)
    => (self != null && self.filter == filter) ? self : new History(filter);

    // PRIVATE -----------------------------------------------------

    bool ContainsTimeValue(int rangeId, float time){
        var f0 = frames[rangeId];
        var f1 = this[rangeId + 1, @default: true];
        if(time < f0.time) return false;
        if(f1 == null) return true;
        return time < f1.time;
    }

    bool ContainsFrameIndex(int rangeId, int φ){
        var f0 = frames[rangeId];
        var f1 = rangeId >= count - 1 ? null : frames[rangeId + 1];
        if(φ < f0.index) return false;
        if(f1 == null) return true;
        return φ < f1.index;
    }

    static void print(string str) => UnityEngine.Debug.Log(str);

}}
