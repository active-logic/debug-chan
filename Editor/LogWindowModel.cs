using UnityEngine;
using static Active.Log.Formatter;

namespace Active.Log{
public class LogWindowModel{

    public static LogWindowModel instance = new LogWindowModel();

    History selHistory = null;

    public void Log(Frame frame) => selHistory += frame;

    // TODO - when not playing returning cached output might be okay...
    // if it works
    public string Output(bool useHistory, GameObject sel){
        //if(!Application.isPlaying) return "Not playing";
        return sel ? useHistory ? Latest(selHistory /= sel)
                                : State(selHistory /= sel)
                   : useHistory ? Latest(Logger.history)
                                : State(Logger.history);
    }

}}
