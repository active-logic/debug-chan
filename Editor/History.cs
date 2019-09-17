using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace Activ.Prolog{
public class History{

    public List<Frame> frames { get; private set; }
    string path;
    Filter filter = new Filter(null, "any");

    public History(string path = null){
        this.path   = path;
        this.frames = new List<Frame>();
        if(path != null) File.Delete(path);
    }

    public History(Filter filter){
        this.filter = filter;
        this.frames = new List<Frame>();
        foreach(var frame in Logger.frames){ var self = this + frame; }
    }

    public bool empty => !this == 0;

    public Frame last{
        get => empty ? null : frames[!this-1];
        set => frames.Add(value);
    }

    public Frame this[int i] => frames[i];

    public void Clear() => frames = new List<Frame>();

    public Frame Next(Frame x)
    => frames[ System.Math.Min(frames.IndexOf(x) + 1, frames.Count-1) ];

    public Frame Prev(Frame x)
    => frames[ System.Math.Max(frames.IndexOf(x) - 1, 0) ];

    public int End(int i)  // End of range at i
    => i == !this - 1 ? Time.frameCount : frames[i + 1].index - 1;

    public static History operator + (History self, Frame frame){
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

    static void print(string str) => UnityEngine.Debug.Log(str);

}}
