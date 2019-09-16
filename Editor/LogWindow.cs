using UnityEngine;
using UnityEditor;
using System.Linq;
using static UnityEditor.EditorGUILayout;
using static Active.Log.LogWindowModel;
using Ed = UnityEditor.EditorApplication;

namespace Active.Log{
public class LogWindow : EditorWindow{

    const int FontSize = 13;
    public static LogWindow instance;
    //
    string currentLog;
    static Font     _font;
    int frame = -1;
    Vector2 scroll;
    LogWindowModel model = LogWindowModel.instance;

    LogWindow(){
        Ed.pauseStateChanged +=
                   (PauseState s) => { if(s == PauseState.Paused) Repaint(); };
    }

    public static void OnMessage(Message message){
        if(Application.isPlaying && instance){
            instance.model.current = Selection.activeGameObject;
            instance.DoUpdate(message);
        }
    }

    void DoUpdate(Message msg)
    { if(frame != Time.frameCount){ frame = Time.frameCount; Repaint(); }}

    void OnGUI(){
        if(!Config.enable)
        { Config.enable = ToggleLeft("Enable Logging", Config.enable); return; }
        //
        model.current = Selection.activeGameObject;
        instance = this;
        BeginHorizontal();
        model.useSelection = ToggleLeft("Use Selection", model.useSelection,
                                        GUILayout.MaxWidth(90f));
        model.allFrames    = ToggleLeft("History",  model.allFrames,
                                        GUILayout.MaxWidth(60));
        // TODO - make return type filtering available with the global history
        if(model.applicableSelection){
            GUILayout.Label("→", GUILayout.MaxWidth(25f));
            model.rtypeIndex = Popup(model.rtypeIndex, rtypeOptions);
        }
        EndHorizontal();
        if(!model.useSelection) model.current = null;
        BeginHorizontal();
        GUILayout.Label($"#{Time.frameCount}");
        model.step = ToggleLeft("Step", model.step, GUILayout.MaxWidth(90f));
        EndHorizontal();
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
        string log = model.Output(useHistory, rtypeOptions[model.rtypeIndex]);
        if(currentLog != log && model.step) Ed.isPaused = true;
        currentLog = log;
        GUILayout.TextArea(log, GUILayout.ExpandHeight(true));
        EndScrollView();
        GUI.backgroundColor = Color.white;
        BeginHorizontal();
        Config.enable = ToggleLeft("Enable Logging", Config.enable);
        GUILayout.Label($"{Logger.injectionTimeMs}ms", GUILayout.MaxWidth(90f));
        EndHorizontal();
    }

    void OnSelectionChange()
    { if(Ed.isPaused || !Application.isPlaying) Repaint(); }

    [MenuItem("Window/Active Log")]
    static void Init(){
        instance = (LogWindow)EditorWindow.GetWindow<LogWindow>(title: "Prolog");
        instance.Show();
    }

    static Font font{ get{
        if(_font) return _font;
        var avail = new []{ "Menlo", "Consolas", "Courier", "Courier New",
                            "Lucida Console", "Monaco", "Inconsolata"      }
            .Intersect(Font.GetOSInstalledFontNames()).First();
        return _font = Font.CreateDynamicFontFromOSFont(avail, FontSize);
    }}

    bool useHistory => model.allFrames && canUseHistory;

    bool canUseHistory => Ed.isPaused || !Ed.isPlaying;

}}
