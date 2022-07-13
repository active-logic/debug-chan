using UnityEngine;
using Ed = UnityEditor.EditorApplication;

namespace Activ.Loggr.UI{
public partial class LogWindow{  // Debug-Chan

    Vector2 dc_scroll;

    // From DebugChan
    public void OnGenericMessage(string message, object sender, int messageCount){
        cumulatedMessageCount = messageCount;
        if(isPlaying && instance){
            instance.DoUpdate(null);
        }
    }

    void DrawLoggerTextView(){
        string content;
        if(browsing && !useHistory){
            content = model.dcRange.Format();
        }else{
            content = EvalTextContent();
        }
        DrawTextView(content, ref dc_scroll);
    }

    string EvalTextContent(){
        var logger = (Activ.Loggr.Logger<string, object>) DebugChan.logger;
        if(logger == null){
            return "Debug-Chan: not running";
        }
        if(!Config.useSelection || model.selection == null){
            return "Debug-Chan: no selection";
        }
        if(useHistory && Ed.isPaused){
            var text = logger[model.selection]?.Format() ?? "Debug-Chan: No messages";
            // NOTE: history does not include latest frame.
            var frame = logger.CurrentFrame(model.selection);
            if(frame != null) text += frame.Format();
            return model.selection.name + "\n\n" + text;
        }else{
            var frame = logger.CurrentFrame(model.selection);
            var text = frame?.Format() ?? "Debug-Chan: no messages";
            return model.selection.name + "\n\n" + text;
        }
    }

}}