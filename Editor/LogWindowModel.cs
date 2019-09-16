using UnityEngine;
using static UnityEditor.EditorPrefs;

namespace Active.Log{
public class LogWindowModel{

    const string UseSelection = "Prolog.UseSel",
                 AllFrames    = "Prolog.AllFrames",
                 Step         = "Prolog.Step",
                 RTypeIndex   = "Prolog.RType";
    public static readonly string[] rtypeOptions = {"any", "void", "status"};
    public static LogWindowModel instance = new LogWindowModel();
    public GameObject current;
    Filter filter;
    History selHistory = null;

    public bool allFrames
    { set => SetBool(AllFrames, value); get => GetBool(AllFrames); }

    public bool useSelection
    { set => SetBool(UseSelection, value); get => GetBool(UseSelection); }

    public int rtypeIndex
    { set => SetInt(RTypeIndex, value); get => GetInt(RTypeIndex); }

    public bool step
    { set => SetBool(Step, value); get => GetBool(Step); }

    public GameObject applicableSelection => useSelection ? current : null;

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
