using UnityEngine;
using UnityEditor;
using System.Linq;
using static UnityEditor.EditorGUILayout;
using Ed = UnityEditor.EditorApplication;

namespace Active.Log{
public class LogWindow : EditorWindow{

    const int FontSize = 13;
    //
    public static LogWindow instance;
    //
    public bool       useSelection = true, allFrames = false;
    public string     filter;
    public GameObject current;
    //
    static Font _font;
    int frame = -1;
    Vector2 scroll;
    LogWindowModel model = LogWindowModel.instance;

    LogWindow() => Ed.pauseStateChanged += (PauseState s) =>
    { if(s == PauseState.Paused) Repaint(); };

    public static void OnMessage(LogMessage message){
        if(Application.isPlaying && instance){
            instance.current = Selection.activeGameObject;
            instance.DoUpdate(message);
        }
    }

    void DoUpdate(LogMessage msg)
    { if(frame != Time.frameCount){ frame = Time.frameCount; Repaint(); }}

    void OnGUI(){
        Config.enable = ToggleLeft("Enable Logging", Config.enable);
        if(!Config.enable) return;
        //
        current = Selection.activeGameObject;
        string log = model.Output(useHistory, applicableSelection);
        instance = this;
        // filter    = TextField("Filter: ", filter);
        useSelection  = ToggleLeft("Use Selection", useSelection);
        if(canUseHistory) allFrames = ToggleLeft("History", allFrames);
        if(!useSelection) current = null;
        scroll = BeginScrollView(scroll);
        GUI.backgroundColor = Color.black;
        var style = GUI.skin.textArea;
        var f = font;
        if(f == null) Debug.LogError("font not available");
        style.font = f;
        style.fontSize = FontSize;
        style.normal.textColor  = Color.white * 0.9f;
        style.focused.textColor = Color.white;
        style.focused.textColor = Color.white;
        GUILayout.TextArea(log, GUILayout.ExpandHeight(true));
        EndScrollView();
    }

    void OnSelectionChange(){ if(Ed.isPaused) Repaint(); }

    [MenuItem("Window/Active Log")]
    static void Init(){
        instance = (LogWindow)EditorWindow
                   .GetWindow(typeof(LogWindow));
        instance.Show();
    }

    static Font font{ get{
        if(_font) return _font;
        var avail = new []{ "Menlo", "Consolas", "Courier", "Courier New",
                            "Lucida Console", "Monaco", "Inconsolata"      }
            .Intersect(Font.GetOSInstalledFontNames()).First();
        return _font = Font.CreateDynamicFontFromOSFont(avail, FontSize);
    }}

    GameObject applicableSelection => useSelection ? current : null;

    bool useHistory => allFrames && canUseHistory;

    bool canUseHistory => Ed.isPaused || !Ed.isPlaying;

}}
