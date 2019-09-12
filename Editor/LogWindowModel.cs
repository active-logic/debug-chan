using UnityEngine;
using static Active.Log.Formatter;

namespace Active.Log{
public class LogWindowModel{

    public static LogWindowModel instance = new LogWindowModel();
    //
    Filter filter;
    History selHistory = null;

    public void Log(Frame frame) => selHistory += frame;

    public string Output(bool useHistory, GameObject sel, string rtype){
        filter = new Filter(sel, rtype);
        return sel ? useHistory ? Latest(selHistory /= filter)
                                : State(selHistory /= filter)
                   : useHistory ? Latest(Logger.history)
                                : State(Logger.history);
    }

}}
