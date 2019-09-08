using UnityEngine;
using System.Text;

namespace Active.Log{
public class GOStateFormatter : Formatter{

    GameObject target;

    public void Append(LogMessage x, GameObject target){
        msg = x;
        if(isNewFrame || IsNewTarget(target)) Clear("■ "+x.ownerName);
        if(x.owner != target) return;
        if(isNewObject) Append($"    [{x.sourceType}]\n");
                        Append($"        {x.message}\n");
        prev = x;
    }

    bool IsNewTarget(GameObject target){
        if(target == this.target) return false;
        this.target = target;
        return true;
    }

}}
