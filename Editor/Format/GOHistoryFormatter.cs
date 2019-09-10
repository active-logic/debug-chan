using UnityEngine;
using System.Text;

namespace Active.Log{
public class GOHistoryFormatter : Formatter{

    GameObject target;

    public void Update(GameObject target){
        Clear($"■ {target?.name}");
        foreach(var x in Logger.log.messages) Append(x, target);
    }

    public void Append(LogMessage x, GameObject target){
        msg = x;
        if(x.owner != target) return;
        if(isNewFrame) Append("\n\n");
        if(isNewFrame) Append( (x.frame + " ").PadRight(40, '―') + '\n' );
        if(isNewObject || isNewFrame)
                       Append($"[{x.sourceType}] ");
                       Append($" {x.message}" );
        Trim();
        prev = x;
    }

    bool IsNewTarget(GameObject target){
        if(target == this.target) return false;
        this.target = target;
        return true;
    }

}}
