using UnityEngine;
using UnityEditor;
using System.Linq;
using static UnityEditor.EditorGUILayout;
using static Activ.Prolog.LogWindowModel;
//using static Activ.Prolog.PCfg;
using Ed = UnityEditor.EditorApplication;
using GL = UnityEngine.GUILayout;
using EGL = UnityEditor.EditorGUILayout;
//
using PrologLogger = Activ.Prolog.Logger;
using PrologConfigManager = Activ.Prolog.PrologConfigManager;
using PrologFrame = Activ.Prolog.Frame;
using PrologMessage = Activ.Prolog.Message;
using PrologWindowModel = Activ.Prolog.LogWindowModel;
using PrologHistoryGUI = Activ.Prolog.HistoryGUI;
using PCfg = Activ.Prolog.PrologConfig;

namespace Activ.Loggr.UI{
public class LogWindow : EditorWindow{

    const int FontSize = 13;
    const float ScrubberButtonsHeight = 24f;
    public static LogWindow instance;
    //
    static Font normalButtonFont;
    static Font     _font;
    PrologWindowModel model = PrologWindowModel.instance;
    PrologFrame   selectedFrame;   // Last selected frame object
    Vector2 scroll;
    string  currentLog;
    int     frame = -1;      // Last frame while playing (store this?)
    // approx msg count (helps tracking memory overheads)
    public static int cumulatedMessageCount;

    LogWindow(){
        Ed.pauseStateChanged +=
            (PauseState s) => { if(s == PauseState.Paused) Repaint(); };
        Activ.Loggr.Logger<string, object>.messageReceived += OnGenericMessage;
    }

    // From DebugChan
    public void OnGenericMessage(string message, object sender, int messageCount){
        cumulatedMessageCount = messageCount;
        if(isPlaying && instance){
            instance.DoUpdate(null);
        }
    }

    // From Prolog
    public static void OnMessage(object sender, PrologMessage message){
        if(isPlaying && instance){
            instance.model.current = Selection.activeGameObject;
            instance.DoUpdate(message);
        }
    }

    void DoUpdate(PrologMessage msg)
    { if(frame != Time.frameCount){ frame = Time.frameCount; Repaint(); }}

    void OnFocus(){
        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnGUI(){
        if(PrologConfigManager.current == null){
            if(GL.Button("Create Debug-Chan config")){
                PrologConfigManager.Create();
            }
            return;
        }
        OnGUI_Content();
        // TODO move somewhere else
        // DrawConfigManager();
    }

    void OnGUI_Content(){
        model.current = Selection.activeGameObject;
        instance = this;
        DrawScrubber();
        if(Config.enableInjection) DrawDebuggerTextView();
        DrawLoggerTextView();
        DrawFooter();
        //DrawTrailsConfig();
    }

    void DrawLoggerTextView(){
        var logger = (Activ.Loggr.Logger<string, object>) DebugChan.logger;
        if(logger == null){
            DrawTextView("Not running");
            return;
        }
        if(!Config.useSelection || model.selection == null){
            DrawTextView("No selection");
            return;
        }
        if(useHistory && Ed.isPaused){
            var text = logger[model.selection]?.Format() ?? "No messages";
            DrawTextView(model.selection.name + "\n\n" + text);
        }else{
            var frame = logger.CurrentFrame(model.selection);
            var text = frame?.Format() ?? "No messages";
            DrawTextView(model.selection.name + "\n\n" + text);
        }
    }

    void DrawDebuggerTextView(){
        string log = model.Output(useHistory, rtypeOptions[Config.rtypeIndex]);
        if(currentLog != log && Config.step) Ed.isPaused = true;
        currentLog = log;
        DrawTextView(log);
    }

    void DrawTextView(string text){
        scroll = BeginScrollView(scroll);
        GUI.backgroundColor = Color.black;
        ConfigTextAreaStyle();
        GL.TextArea(browsing ? selectedFrame.Format() : text,
                           GL.ExpandHeight(true));
        EndScrollView();
        GUI.backgroundColor = Color.white;
    }

    void ConfigTextAreaStyle(){
        var f = monofont;
        if(f == null) Debug.LogError("font not available");
        var style = GUI.skin.button;
        style.font = f;
        style.fontSize = FontSize;
        style.normal.textColor  = Color.white * 0.9f;
        style.focused.textColor = Color.white;
        style.focused.textColor = Color.white;
    }

    void DrawFooter(){
        BeginHorizontal();
        Config.useSelection = ToggleLeft("Use Selection", Config.useSelection,
                                GL.MaxWidth(100f));
        Config.allFrames    = ToggleLeft("History",  Config.allFrames,
                                GL.MaxWidth(60));
        DebugChan.logToConsole = Config.logToConsole
            = ToggleLeft("Log to console",  Config.logToConsole,
                                GL.MaxWidth(120));
        //
        EndHorizontal();
        BeginHorizontal();
        //
        GL.Label("Trails - offset: ", GL.MaxWidth(88f));
        Config.trailOffset = FloatField(Config.trailOffset,
                                        GL.MaxWidth(30f));
        GL.Label("size: ", GL.MaxWidth(30f));
        Config.handleSize = FloatField(Config.handleSize, GL.MaxWidth(30f));
        //
        // TODO - make return type filtering available with the global history
        // TODO for Prolog update
        //if(model.selection){
        //    GL.Label("→", GL.MaxWidth(25f));
        //    PCfg.rtypeIndex = Popup(PCfg.rtypeIndex, rtypeOptions);
        //}
        //EndHorizontal();
        //BeginHorizontal();
        GL.FlexibleSpace();
        if(isPlaying){
            EGL.LabelField($"#{cumulatedMessageCount:0,000,000}", GL.MaxWidth(92f));
        }else{
            EditorGUIUtility.labelWidth = 60;
            Config.maxMessages = EGL.IntField("Max msgs", Config.maxMessages);
            EditorGUIUtility.labelWidth = 0;
            //if(ScrubberButton($"Clear")) Clear();
        }
        EndHorizontal();
        if(!Config.useSelection) model.current = null;
    }

    void DrawScrubber(){
        BeginHorizontal();
        int frameNo = browsing ? selectedFrame.index : Time.frameCount;
        var style = GUI.skin.button;
        normalButtonFont = style.font;
        style.font = monofont;
        if(ScrubberButton("<")) SelectPrev();
        if(isPlaying){
            GL.Button($"#{frameNo:0000}", GL.MaxWidth(64f), GL.MinHeight(ScrubberButtonsHeight));
        }else{
            GL.Button($"-----", GL.MaxWidth(64f), GL.MinHeight(ScrubberButtonsHeight));
        }
        if(ScrubberButton(">")) SelectNext();
        style.font = normalButtonFont;
        GL.FlexibleSpace();
        if(isPlaying){
            //EGL.LabelField($"N#{cumulatedMessageCount:0,000,000}", GL.MaxWidth(92f));
        }else{
            if(ScrubberButton($"Clear")) Clear();
        }
        // TODO reenable
        //PCfg.step = ToggleLeft("Step", PCfg.step, GL.MaxWidth(48f));
        EndHorizontal();
    }

    bool ScrubberButton(string arg)
    => GL.Button(arg, GL.ExpandWidth(false), GL.MinHeight(ScrubberButtonsHeight));

    // TODO re-enable but move elsewhere
    //void DrawPauseModeConfig(){
    //    PCfg.enableInjection = ToggleLeft(
    //        $"Instrument ({Logger.injectionTimeMs}ms)",
    //        PCfg.enableInjection, GL.ExpandWidth(true));
    //}

    void ToggleAdvanced(){}

    void DrawConfigManager(){
        var selected = EGL.ObjectField(
            PrologConfigManager.current,
            typeof(PCfg),
            allowSceneObjects: false);
        PrologConfigManager.current = (PCfg)selected;
    }

    // Ref https://tinyurl.com/yyo8c35g which also demonstrates starting a 2D
    // GUI at handles location
    void OnSceneGUI(SceneView sceneView){
        var sel = PrologHistoryGUI.Draw(model.filtered, selectedFrame);
        if(Ed.isPaused || !isPlaying){
            selectedFrame = sel ?? selectedFrame;
            Repaint();
        }else{
            selectedFrame = null;
        }
    }

    void OnSelectionChange()
    { if(Ed.isPaused || !isPlaying) Repaint(); }

    void Clear(){
        PrologLogger.Clear();
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

    [MenuItem("Window/Activ/Debug-Chan")]
    static void Init(){
        instance = (LogWindow)EditorWindow
                   .GetWindow<LogWindow>(title: "Debug-Chan");
        instance.Show();
    }

    static Font monofont{ get{
        if(_font) return _font;
        var avail = new []{
            "Menlo", "Consolas", "Inconsolata", "Bitstream Vera Sans Mono",
            "Oxygen Mono", "Ubuntu Mono", "Cousine", "Courier", "Courier New",
            "Lucida Console", "Monaco"
        }.Intersect(Font.GetOSInstalledFontNames()).First();
        return _font = Font.CreateDynamicFontFromOSFont(avail, FontSize);
    }}

    bool browsing
    => (Ed.isPaused || !isPlaying) && selectedFrame != null;

    static bool isPlaying => Application.isPlaying;

    bool useHistory => Config.allFrames && canUseHistory;

    bool canUseHistory => Ed.isPaused || !Ed.isPlaying;

}}
