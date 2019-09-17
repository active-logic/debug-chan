using UnityEngine;
using UnityEditor;
using System.Linq;
using static UnityEditor.EditorGUILayout;
using static Activ.Prolog.LogWindowModel;
using static Activ.Prolog.Config;
using Ed = UnityEditor.EditorApplication;

namespace Activ.Prolog{
public class LogWindow : EditorWindow{

    const int FontSize = 13;
    public static LogWindow instance;
    //
    static Font     _font;
    LogWindowModel model = LogWindowModel.instance;
    Frame          selectedFrame;   // Last selected frame object
    Vector2        scroll;
    string         currentLog;
    int            frame = -1;      // Last frame while playing (store this?)

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

    void OnFocus(){
        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnGUI(){
        if(!Config.enable)
        { Config.enable = ToggleLeft("Enable Logging", Config.enable); return; }
        //
        model.current = Selection.activeGameObject;
        instance = this;
        BeginHorizontal();
        Config.useSelection = ToggleLeft("Use Selection", Config.useSelection,
                                        GUILayout.MaxWidth(90f));
        Config.allFrames    = ToggleLeft("History",  Config.allFrames,
                                        GUILayout.MaxWidth(60));
        // TODO - make return type filtering available with the global history
        if(model.applicableSelection){
            GUILayout.Label("→", GUILayout.MaxWidth(25f));
            Config.rtypeIndex = Popup(Config.rtypeIndex, rtypeOptions);
        }
        EndHorizontal();
        if(!Config.useSelection) model.current = null;
        BeginHorizontal();
        int frameNo = browsing ? selectedFrame.index : Time.frameCount;
        GUILayout.Label($"#{frameNo}");
        Config.step = ToggleLeft("Step", Config.step, GUILayout.MaxWidth(90f));
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
        string log = model.Output(useHistory, rtypeOptions[Config.rtypeIndex]);
        if(currentLog != log && Config.step) Ed.isPaused = true;
        currentLog = log;
        GUILayout.TextArea(browsing ? selectedFrame.Format() : log,
                           GUILayout.ExpandHeight(true));
        EndScrollView();
        GUI.backgroundColor = Color.white;
        BeginHorizontal();
        Config.enable = ToggleLeft("Enable Logging", Config.enable);
        GUILayout.Label($"{Logger.injectionTimeMs}ms", GUILayout.MaxWidth(90f));
        EndHorizontal();
        BeginHorizontal();
        GUILayout.Label("Trail offset: ", GUILayout.MaxWidth(60f));
        Config.trailOffset = FloatField(Config.trailOffset,
                                        GUILayout.MaxWidth(30f));
        GUILayout.Label("Size: ", GUILayout.MaxWidth(30f));
        Config.handleSize = FloatField(Config.handleSize, GUILayout.MaxWidth(30f));
        EndHorizontal();
    }

    // Ref https://tinyurl.com/yyo8c35g which also demonstrates starting a 2D
    // GUI at handles location
    void OnSceneGUI(SceneView sceneView){
        var sel = HistoryGUI.Draw(model.filtered);
        if(Ed.isPaused || !Application.isPlaying){
            selectedFrame = sel ?? selectedFrame;
            Repaint();
        }else{
            selectedFrame = null;
        }
    }

    void OnSelectionChange()
    { if(Ed.isPaused || !Application.isPlaying) Repaint(); }

    [MenuItem("Window/Prolog")]
    static void Init(){
        instance = (LogWindow)EditorWindow
                   .GetWindow<LogWindow>(title: "Prolog");
        instance.Show();
    }

    static Font font{ get{
        if(_font) return _font;
        var avail = new []{ "Menlo", "Consolas", "Courier", "Courier New",
                            "Lucida Console", "Monaco", "Inconsolata"      }
            .Intersect(Font.GetOSInstalledFontNames()).First();
        return _font = Font.CreateDynamicFontFromOSFont(avail, FontSize);
    }}

    bool browsing
    => (Ed.isPaused || !Application.isPlaying) && selectedFrame != null;

    bool useHistory => Config.allFrames && canUseHistory;

    bool canUseHistory => Ed.isPaused || !Ed.isPlaying;

}}
