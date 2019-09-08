using UnityEngine;
using UnityEditor;
using static UnityEditor.EditorGUILayout;

namespace Active.Log{
public class ActiveLog : EditorWindow{

    public static ActiveLog instance;
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
        //
        // filter       = TextField("Filter: ", filter);
        useSelection = ToggleLeft("Use Selection", useSelection);
        if(EditorApplication.isPaused)
            allFrames = ToggleLeft("History", allFrames);
        if(!useSelection) current = null;
        scroll = BeginScrollView(scroll);
        GUI.backgroundColor = Color.black;
        GUI.skin.textArea.normal.textColor = Color.white * 0.9f;
        GUI.skin.textArea.focused.textColor = Color.white;
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

    protected void print(string str) => Debug.Log(str);

    protected void Label(string str) => GUILayout.Label(str);

    Formatter currentFormatter => (useSelection && (current != null))
        ? useHistory ? (Formatter)goHistoryFmt : (Formatter)goStateFmt
        : useHistory ? (Formatter)historyFmt   : (Formatter)stateFmt;

    bool useHistory => allFrames && EditorApplication.isPaused;

}}
