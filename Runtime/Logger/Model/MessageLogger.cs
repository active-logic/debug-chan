using System.Collections.Generic;

namespace Activ.LogChan{
public partial class MessageLogger{

    public Frame current { get; private set; }
    public List<Range> ranges = new List<Range>();

    public void Log(string arg, object source, int frameIndex){
        if(frameIndex > currentFrameIndex){
            FinalizeFrame(current);
            current = new Frame(frameIndex);
        }
        var msg = current.Log(arg, source);
        messageReceived?.Invoke(this, msg);
    }

    void FinalizeFrame(Frame arg){
        if(arg == null) return;
        var last = lastRange;
        if(last == null || !last.Include(current)){
            ranges.Add(new Range(arg));
        }
    }

    Range lastRange
    => ranges.Count > 0 ? ranges[ranges.Count-1] : null;

    int currentFrameIndex => current?.index ?? -1;

}}
