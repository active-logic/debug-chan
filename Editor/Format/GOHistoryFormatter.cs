using UnityEngine;
using System.Text;

namespace Active.Log{
public class GOHistoryFormatter : Formatter{

    GameObject target;

    public void Append(Log log, LogMessage x, GameObject target){
        msg = x;
        if(x.owner != target) return;
        if(IsNewTarget(target)) { Rebuild(log, target); return; }
        if(isNewFrame) Append("\n\n");
        if(isNewFrame) Append($"{x.frame} ------------------------------\n\n");
        if(isNewObject || isNewFrame)
                       Append($"[{x.sourceType}] ");
                       Append($" {x.message}" );
        Trim();
        prev = x;
    }

    public void Rebuild(Log log, GameObject target){
        Clear($"■ {target?.name}");
        foreach(var m in log.messages) Append(log, m, target);
    }

    bool IsNewTarget(GameObject target){
        if(target == this.target) return false;
        this.target = target;
        return true;
    }

}}
