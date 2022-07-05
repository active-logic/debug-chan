using UnityEngine;

namespace Activ.Prolog{
public class LogWindowModel{

    public static readonly string[] rtypeOptions = {"any", "void", "status"};
    public static LogWindowModel instance = new LogWindowModel();
    public GameObject current;
    public History filtered{ get; private set; }
    Filter filter;

    public GameObject selection
    => Config.useSelection ? current : null;

    public void Clear(){ filtered = null; current = null; }

    public string Output(bool useHistory, string rtype){
        filter = new Filter(selection, rtype);
        return useHistory ? Formatter.Latest(source)
                          : Formatter.State(source);
    }

    public void Log(Frame frame) => filtered += frame;

    History source
    => selection ? (filtered /= filter) : Logger.history;

}}
