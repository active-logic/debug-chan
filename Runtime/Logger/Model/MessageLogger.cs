using System.Collections.Generic;

namespace Activ.LogChan{
public partial class MessageLogger{

    public List<Frame> frames = new List<Frame>();

    public void Log(string arg, object source, int frameIndex){
        if(frameIndex > currentFrameIndex){
            frames.Add(new Frame(frameIndex));
        }
        var msg = lastFrame.Log(arg, source);
        messageReceived?.Invoke(this, msg);
    }

    int currentFrameIndex => lastFrame?.index ?? -1;

    public Frame lastFrame => frames.Count > 0
        ? frames[frames.Count - 1] : null;

}}
