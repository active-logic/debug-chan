using System.Linq;
using UnityEngine; using UnityEditor;
using static UnityEditor.EditorGUILayout;
using Ed = UnityEditor.EditorApplication;
using GL = UnityEngine.GUILayout;
using EGL = UnityEditor.EditorGUILayout;
using PrologLogger = Activ.Prolog.Logger;  // for 'enable injection'
using PrologHistoryGUI = Activ.Prolog.HistoryGUI;  // TODO ???

namespace Activ.Loggr.UI{
public partial class LogWindow : EditorWindow{

    const int FontSize = 13;
    const float ScrubberButtonsHeight = 24f;
    public static LogWindow instance;
    //
    static Font normalButtonFont;
    static Font _font;

    string currentLog;
    int frame = -1;  // Last frame while playing (store this?)
    // approx msg count (helps tracking memory overheads)
    public static int cumulatedMessageCount;

    LogWindow(){
        Ed.pauseStateChanged +=
            (PauseState s) => { if(s == PauseState.Paused) Repaint(); };
        Activ.Loggr.Logger<string, object>.messageReceived += OnGenericMessage;
    }

    void OnFocus(){
        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnGUI(){
        // TODO needs to move to prolog view; should appear if we're
        // enabling instrumentation but have no config
        if(Activ.Prolog.PrologConfigManager.current == null){
            if(GL.Button("Create Prolog config")){
                Activ.Prolog.PrologConfigManager.Create();
            }
            return;
        }
        //
        model.current = Selection.activeGameObject;
        instance = this;
        DrawScrubber();
        DrawLoggerTextView();
        if(Config.enableInjection) DrawPrologView();
        DrawFooter();
    }

    void DrawFooter(){
        BeginHorizontal();
        Config.useSelection = ToggleLeft("Use Selection", Config.useSelection,
                                GL.MaxWidth(100f));
        Config.allFrames    = ToggleLeft("History",  Config.allFrames,
                                GL.MaxWidth(60));
        DebugChan.logToConsole = Config.logToConsole
            = ToggleLeft("Log to console",  Config.logToConsole,
                                GL.MaxWidth(100));
        //
        if(!isPlaying) DrawEnableInjection();

        EndHorizontal();
        BeginHorizontal();
        //
        GL.Label("Trails - offset: ", GL.MaxWidth(88f));
        Config.trailOffset = FloatField(Config.trailOffset,
                                        GL.MaxWidth(30f));
        GL.Label("size: ", GL.MaxWidth(30f));
        Config.handleSize = FloatField(Config.handleSize, GL.MaxWidth(30f));
        //
        GL.FlexibleSpace();
        if(isPlaying){
            EGL.LabelField($"#{cumulatedMessageCount:0,000,000}", GL.MaxWidth(92f));
        }else{
            EditorGUIUtility.labelWidth = 60;
            Config.maxMessages = EGL.IntField("Max msgs", Config.maxMessages);
            EditorGUIUtility.labelWidth = 0;
        }
        EndHorizontal();
        if(!Config.useSelection) model.current = null;
    }

    void DrawEnableInjection(){
        Config.enableInjection = ToggleLeft(
            $"Instrument ({PrologLogger.injectionTimeMs}ms)",
            Config.enableInjection, GL.ExpandWidth(false));
    }

    void DrawTextView(string text, ref Vector2 scroll){
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
        if(!isPlaying && ScrubberButton($"Clear")) Clear();
        EndHorizontal();
    }

    bool ScrubberButton(string arg)
    => GL.Button(arg, GL.ExpandWidth(false), GL.MinHeight(ScrubberButtonsHeight));

    void ToggleAdvanced(){}

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

    public static void DisplayWindow(){
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
