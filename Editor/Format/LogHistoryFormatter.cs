using System.Text;
using UnityEngine;

namespace Active.Log{
public class LogHistoryFormatter : Formatter{

    public void Append(LogMessage x){
        msg = x;
        //
        // First, moving to next line as appropriate.
        // Leave an empty line if new frame or GO, but not 3 if both
        // Or, if it's only a new object/component, put a line break
        if(isNewFrame || isNewGO) Append("\n\n");
        else if(isNewObject)      Append("\n");
        //
        if(isNewFrame)  Append($"{x.frame} -----------------------------\n\n");
        if(isNewGO)
            if(x.owner != null) Append($"■ {x.ownerName}\n");
        if(isNewObject) Append($"[{x.sourceType}]\n");
                        Append($"{x.message} " );
        Trim();
        prev = x;
    }

}}
