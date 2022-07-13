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

namespace Activ.Loggr.UI{
public partial class LogWindow{  // Prolog

    public static readonly string[] rtypeOptions = {"any", "void", "status"};

    Vector2 p_scroll;

    void DrawPrologView(float time){
        DrawPrologHeader();
        DrawPrologTextView(time);
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

    void DrawPrologTextView(float time){
        string log;
        var rtype = rtypeOptions[Config.rtypeIndex];
        if(useHistory){
            var startTime = time - Config.historySpan;
            log = model.GetPrologOutput(rtype, since: startTime, time);
        }else{
            log = model.GetPrologOutput(rtype);
        }

        if(currentLog != log && Config.step) Ed.isPaused = true;
        currentLog = log;
        DrawTextView(browsing ? model.pgRange.Format() : log, ref p_scroll);
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
