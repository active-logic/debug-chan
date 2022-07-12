using UnityEngine;
using GL = UnityEngine.GUILayout;
using UnityEditor;
using Ed  = UnityEditor.EditorApplication;
using EGL = UnityEditor.EditorGUILayout;
using static UnityEditor.EditorGUILayout;
//
using PCfg = Activ.Prolog.PrologConfig;
using PrologConfigManager = Activ.Prolog.PrologConfigManager;
using PrologFrame = Activ.Prolog.Frame;
using PrologLogger = Activ.Prolog.Logger;
using PrologMessage = Activ.Prolog.Message;
using PrologWindowModel = Activ.Prolog.LogWindowModel;
using static Activ.Prolog.LogWindowModel;

namespace Activ.Loggr.UI{
public partial class LogWindow{  // Prolog

    Vector2 p_scroll;

    PrologWindowModel model = PrologWindowModel.instance;
    PrologFrame selectedFrame;   // Last selected frame object

    void DrawPrologView(){
        DrawPrologHeader();
        DrawPrologTextView();
    }

    void DrawPrologHeader(){
        BeginHorizontal();
        DrawConfigSelector();
        DrawReturnTypeSelector();
        //
        // TODO generic option, likely should partake x-logger header
        Config.step = ToggleLeft("Step", Config.step, GL.MaxWidth(48f));
        //
        EndHorizontal();
    }

    void DrawPrologTextView(){
        string log = model.Output(useHistory, rtypeOptions[Config.rtypeIndex]);
        if(currentLog != log && Config.step) Ed.isPaused = true;
        currentLog = log;
        DrawTextView(log, ref p_scroll);
    }

    void DrawConfigSelector(){
        var selected = EGL.ObjectField(
            PrologConfigManager.current,
            typeof(PCfg),
            allowSceneObjects: false);
        PrologConfigManager.current = (PCfg)selected;
    }

    void DrawReturnTypeSelector(){
        if(model.selection){
            GL.Label("→", GL.MaxWidth(25f));
            Config.rtypeIndex = Popup(Config.rtypeIndex, rtypeOptions);
        }
    }

    // =============================================================

    void DoUpdate(PrologMessage msg)
    { if(frame != Time.frameCount){ frame = Time.frameCount; Repaint(); }}

    // From Prolog
    public static void OnMessage(object sender, PrologMessage message){
        if(isPlaying && instance){
            instance.model.current = Selection.activeGameObject;
            instance.DoUpdate(message);
        }
    }

}}
