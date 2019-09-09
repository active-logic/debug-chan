using UnityEngine;
using UnityEditor;
using System.Linq;
using static UnityEditor.EditorGUILayout;

namespace Active.Log{
public class ActiveLog : EditorWindow{

    const int FontSize = 13;
    public static ActiveLog instance;
    static Font _font;
    //
    public bool          useSelection = true;
    public bool          allFrames    = false;
    public string        filter;
    public GameObject    current;
    public static string fullLog;
    //
    LogHistoryFormatter historyFmt    = new LogHistoryFormatter();
    LogStateFormatter   stateFmt      = new LogStateFormatter();
    GOHistoryFormatter  goHistoryFmt  = new GOHistoryFormatter();
    GOStateFormatter    goStateFmt    = new GOStateFormatter();
    //
    int frame = -1;
    Vector2 scroll;

    public static void OnMessage(LogMessage message){
        if(Application.isPlaying && instance){
            instance.current = Selection.activeGameObject;
            instance.DoUpdate(Logger.log, message);
        }
    }

    void DoUpdate(Log log, LogMessage msg){
        historyFmt.Append(msg);
        stateFmt.Append(msg);
        goStateFmt.Append(msg, current);
        goHistoryFmt.Append(log, msg, current);
        //
        if(frame != Time.frameCount){
            frame = Time.frameCount;
            Repaint();
        }
    }

    void OnGUI(){
        string log = currentFormatter.output;
        current = Selection.activeGameObject;
        instance = this;
        // filter    = TextField("Filter: ", filter);
        useSelection = ToggleLeft("Use Selection", useSelection);
        if(EditorApplication.isPaused)
            allFrames = ToggleLeft("History", allFrames);
        if(!useSelection) current = null;
        scroll = BeginScrollView(scroll);
        GUI.backgroundColor = Color.black;
        var style = GUI.skin.textArea;
        style.font = font;
        style.fontSize = FontSize;
        style.normal.textColor  = Color.white * 0.9f;
        style.focused.textColor = Color.white;
        style.focused.textColor = Color.white;
        GUILayout.TextArea(log, GUILayout.ExpandHeight(true));
        EndScrollView();
    }

    void OnSelectionChange(){
        goHistoryFmt.Rebuild(Logger.log, Selection.activeGameObject);
        Repaint();
    }

    [MenuItem("Window/Active Log")]
    static void Init(){
        instance = (ActiveLog)EditorWindow.GetWindow(typeof(ActiveLog));
        instance.Show();
    }

    static Font font => _font = _font ?? Font.CreateDynamicFontFromOSFont(
        new []{"Menlo", "Consolas", "Courier", "Courier New", "Lucida Console",
               "Monaco", "Inconsolata"}
        .Intersect(Font.GetOSInstalledFontNames()).First(), FontSize);

    Formatter currentFormatter => (useSelection && (current != null))
        ? useHistory ? (Formatter)goHistoryFmt : (Formatter)goStateFmt
        : useHistory ? (Formatter)historyFmt   : (Formatter)stateFmt;

    bool useHistory => allFrames && EditorApplication.isPaused;

}}
