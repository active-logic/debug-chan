using UnityEngine;

namespace Active.Log{
public class LogWindowModel{

    public static readonly string[] rtypeOptions = {"any", "void", "status"};
    public static LogWindowModel instance = new LogWindowModel();
    public GameObject current;
    Filter filter;
    History selHistory = null;

    public GameObject applicableSelection
    => Config.useSelection ? current : null;

    public void Log(Frame frame) => selHistory += frame;

    public string Output(bool useHistory, string rtype){
        filter = new Filter(applicableSelection, rtype);
        return applicableSelection
            ? useHistory ? Formatter.Latest(selHistory /= filter)
                         : Formatter.State(selHistory /= filter)
            : useHistory ? Formatter.Latest(Logger.history)
                         : Formatter.State(Logger.history);
    }

}}
