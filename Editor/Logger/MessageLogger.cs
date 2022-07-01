using System.Collections.Generic;

namespace Activ.LogChan{
public class MessageLogger : IMessageLogger{

    public List<Frame> frames = new List<Frame>();

    public void Log(string arg, object source, int frame){
        this[frame].Log(arg, source);
        // TODO window should be listening to us!
        Activ.Prolog.LogWindow.OnMessage(null);
    }

    public Frame lastFrame => frames.Count > 0
        ? frames[frames.Count - 1]
        : null;

    public Frame this[int index]{ get{
        Frame newFrame;
        for(int i = frames.Count-1; i >= 0; i--){
            if(frames[i].index == index){
                return frames[i];
            }
            if(frames[i].index < index){
                //Debug.Log($"Create new frame({index})");
                newFrame = new Frame(index);
                frames.Insert(i + 1, newFrame);
                return newFrame;
            }
        }
        //Debug.Log($"Create new frame({index})");
        newFrame = new Frame(index);
        frames.Insert(0, newFrame);
        return newFrame;
    }}

}}
