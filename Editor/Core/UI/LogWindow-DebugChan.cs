using UnityEngine;
using Ed = UnityEditor.EditorApplication;

namespace Activ.Loggr.UI{
public partial class LogWindow{  // Debug-Chan

    Vector2 dc_scroll;
    int breakFrame = -1;

    // From DebugChan

    public void OnGenericMessage(LogMessage message,
                                 object sender,
                                 int messageCount){
        cumulatedMessageCount = messageCount;
        if(Config.useSelection && model.current == null){
            if(sender is GameObject){
                model.current = sender as GameObject;
            }
            if(sender is Component){
                model.current = (sender as Component).gameObject;
            }
        }
        if(Time.frameCount >= breakFrame && breakFrame != -1){
            Debug.Log($"delayed break on frame {Time.frameCount}");
            Debug.Break();
            breakFrame = -1;
        }
        if(breakpoint != null && message.Contains(
                                 breakpoint, caseSensitive: false)){
            if(breakpointDelayFrames == 0){
                Debug.Break();
            }else{
                breakFrame = Time.frameCount + breakpointDelayFrames;
            }
            if(sender is GameObject){
                model.current = sender as GameObject;
            }
        }
        if(isPlaying && instance){
            instance.DoUpdate(null);
        }
    }

    void DrawLoggerTextView(float time, int? height){
        string content;
        if(browsing && !useHistory){
            content = model.dcRange.Format();
        }else{
            content = EvalTextContent(time);
        }
        DrawTextView(content, height, ref dc_scroll);
    }

    string EvalTextContent(float time){
        var logger = (Activ.Loggr.Logger<LogMessage, object>) DebugChan.logger;
        if(logger == null){
            return "";  // "Debug-Chan: not running";
        }
        if(!Config.useSelection || model.selection == null){
            return "Debug-Chan: no selection";
        }
        if(useHistory && (!isPlaying || Ed.isPaused)){
            var startTime = time - Config.historySpan;
            var text = logger[model.selection]?.Format(since: startTime, time)
                       ?? "Debug-Chan: no messages";
            return model.selection.name + "\n\n" + text;
        }else{
            var frame = logger.CurrentFrame(model.selection);
            var text = frame?.Format() ?? "Debug-Chan: no messages";
            return model.selection.name + "\n\n" + text;
        }
    }

}}
