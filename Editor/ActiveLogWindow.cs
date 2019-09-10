using UnityEngine;
using UnityEditor;
using System.Linq;
using static UnityEditor.EditorGUILayout;

namespace Active.Log{
public class ActiveLogWindow : EditorWindow{

    const int FontSize = 13;
    public static ActiveLogWindow instance;
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

    ActiveLogWindow() => EditorApplication.pauseStateChanged +=
    (PauseState s) => {
        if(s != PauseState.Paused) return;
        historyFmt.Update();
        goHistoryFmt.Update(current);
        Repaint();
    };

    public static void OnMessage(LogMessage message){
        if(Application.isPlaying && instance){
            instance.current = Selection.activeGameObject;
            instance.DoUpdate(Logger.log, message);
        }
    }

    void DoUpdate(Log log, LogMessage msg){
        stateFmt.Append(msg);
        goStateFmt.Append(msg, current);
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
        if(canUseHistory) allFrames = ToggleLeft("History", allFrames);
        if(!useSelection) current = null;
        scroll = BeginScrollView(scroll);
        GUI.backgroundColor = Color.black;
        var style = GUI.skin.textArea;
        var f = font;
        if(f==null) Debug.LogError("font not available");

        style.font = f;
        style.fontSize = FontSize;
        style.normal.textColor  = Color.white * 0.9f;
        style.focused.textColor = Color.white;
        style.focused.textColor = Color.white;
        GUILayout.TextArea(log, GUILayout.ExpandHeight(true));
        EndScrollView();
    }

    void OnSelectionChange(){
        if(!EditorApplication.isPaused) return;
        goHistoryFmt.Update(Selection.activeGameObject);
        Repaint();
    }

    [MenuItem("Window/Active Log")]
    static void Init(){
        instance =
            (ActiveLogWindow)EditorWindow.GetWindow(typeof(ActiveLogWindow));
        instance.Show();
    }

    static Font font{ get{
        if(_font) return _font;
        var avail = new []{ "Menlo", "Consolas", "Courier", "Courier New",
                            "Lucida Console", "Monaco", "Inconsolata" }
                    .Intersect(Font.GetOSInstalledFontNames()).First();
        return _font = Font.CreateDynamicFontFromOSFont(avail, FontSize);
    }}

    Formatter currentFormatter => (useSelection && (current != null))
        ? useHistory ? (Formatter)goHistoryFmt : (Formatter)goStateFmt
        : useHistory ? (Formatter)historyFmt   : (Formatter)stateFmt;

    bool useHistory => allFrames && canUseHistory;

    bool canUseHistory
    => EditorApplication.isPaused || !EditorApplication.isPlaying;

}}
