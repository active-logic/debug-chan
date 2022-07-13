using UnityEngine;
using Activ.Loggr;
using PrologHistory = Activ.Prolog.History;
using PrologFrame = Activ.Prolog.Frame;
using PrologFilter = Activ.Prolog.Filter;
using PrologFormatter = Activ.Prolog.Formatter;
using PrologLogger = Activ.Prolog.Logger;

namespace Activ.Loggr.UI{
public class LogWindowModel{

    public GameObject current;
    public PrologHistory filtered{ get; private set; }
    PrologFilter filter;
    public PrologFrame selectedFrame;   // Last selected frame object

    public LogWindowModel(){
        PrologLogger.onFrame += OnPrologFrame;
    }

    public void Clear(){
        filtered = null;
        current = null;
        selectedFrame = null;
    }

    public PrologFrame Next(PrologFrame current){
        return filtered.Next(current);
    }

    public PrologFrame Prev(PrologFrame current){
        return filtered.Prev(current ?? filtered.last);
    }

    public string Output(bool useHistory, string rtype){
        filter = new PrologFilter(selection, rtype);
        return useHistory ? PrologFormatter.Latest(source)
                          : PrologFormatter.State(source);
    }

    public void OnPrologFrame(PrologFrame frame){
        filtered += frame;
        Activ.Loggr.UI.LogWindow.instance?.Repaint();
    }

    public GameObject selection
    => Config.useSelection ? current : null;

    PrologHistory source
    => selection ? (filtered /= filter) : PrologLogger.history;

}}
