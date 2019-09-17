using UnityEngine;

namespace Activ.Prolog{
public class LogWindowModel{

    public static readonly string[] rtypeOptions = {"any", "void", "status"};
    public static LogWindowModel instance = new LogWindowModel();
    public GameObject current;
    Filter filter;
    public History filtered{ get; private set; }

    public GameObject applicableSelection
    => Config.useSelection ? current : null;

    public void Clear(){
        filtered = null;
        current = null;
    }

    public void Log(Frame frame) => filtered += frame;

    public string Output(bool useHistory, string rtype){
        filter = new Filter(applicableSelection, rtype);
        return applicableSelection
            ? useHistory ? Formatter.Latest(filtered /= filter)
                         : Formatter.State(filtered /= filter)
            : useHistory ? Formatter.Latest(Logger.history)
                         : Formatter.State(Logger.history);
    }

}}
