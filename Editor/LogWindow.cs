using UnityEngine;
using UnityEditor;
using System.Linq;
using static UnityEditor.EditorGUILayout;
using static Activ.Prolog.LogWindowModel;
using static Activ.Prolog.Config;
using Ed = UnityEditor.EditorApplication;
using GL = UnityEngine.GUILayout;

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
                                         GL.MaxWidth(90f));
        Config.allFrames    = ToggleLeft("History",  Config.allFrames,
                                         GL.MaxWidth(60));
        // TODO - make return type filtering available with the global history
        if(model.applicableSelection){
            GL.Label("→", GL.MaxWidth(25f));
            Config.rtypeIndex = Popup(Config.rtypeIndex, rtypeOptions);
        }
        EndHorizontal();
        if(!Config.useSelection) model.current = null;
        BeginHorizontal();
        int frameNo = browsing ? selectedFrame.index : Time.frameCount;
        if(GL.Button("˂", GL.ExpandWidth(false))) SelectPrev();
        GL.Button($"#{frameNo}", GL.MaxWidth(48f));
        if(GL.Button("˃", GL.ExpandWidth(false))) SelectNext();
        GL.FlexibleSpace();
        Config.step = ToggleLeft("Step", Config.step, GL.MaxWidth(48f));
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
        GL.TextArea(browsing ? selectedFrame.Format() : log,
                           GL.ExpandHeight(true));
        EndScrollView();
        GUI.backgroundColor = Color.white;
        BeginHorizontal();
        Config.enable = ToggleLeft(
            $"Enable Logging ({Logger.injectionTimeMs}ms)",
            Config.enable, GL.ExpandWidth(true));
        if(!Application.isPlaying){
            if(GL.Button($"Clear", GL.MaxWidth(90f))) Clear();
        }
        EndHorizontal();
        BeginHorizontal();
        GL.Label("Trail offset: ", GL.MaxWidth(60f));
        Config.trailOffset = FloatField(Config.trailOffset,
                                        GL.MaxWidth(30f));
        GL.Label("Size: ", GL.MaxWidth(30f));
        Config.handleSize = FloatField(Config.handleSize, GL.MaxWidth(30f));
        EndHorizontal();
    }

    // Ref https://tinyurl.com/yyo8c35g which also demonstrates starting a 2D
    // GUI at handles location
    void OnSceneGUI(SceneView sceneView){
        var sel = HistoryGUI.Draw(model.filtered, selectedFrame);
        if(Ed.isPaused || !Application.isPlaying){
            selectedFrame = sel ?? selectedFrame;
            Repaint();
        }else{
            selectedFrame = null;
        }
    }

    void OnSelectionChange()
    { if(Ed.isPaused || !Application.isPlaying) Repaint(); }

    void Clear(){
        Logger.Clear();
        model.Clear();
        selectedFrame = null;
        SceneView.RepaintAll();
        Repaint();
    }

    void SelectPrev(){
        selectedFrame = model.filtered.Prev(selectedFrame
                                            ?? model.filtered.last);
        SceneView.RepaintAll();
    }

    void SelectNext(){
        if(selectedFrame == null) return;
        selectedFrame = model.filtered.Next(selectedFrame);
        SceneView.RepaintAll();
    }

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
