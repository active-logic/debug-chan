using UnityEngine;
using System.Text;

namespace Active.Log{
public class LogStateFormatter : Formatter{

    public void Append(LogMessage x){
        msg = x;
        string PCO = " ";
        if(isNewFrame)  Clear();
        if(isNewGO)
            if(x.owner != null)Append($"\n■ {x.ownerName}\n"  );
            else             { Append($"\n\n"                 ); PCO = "□"; }
        if(isNewObject)        Append($"{PCO}   [{x.sourceType}]\n");
                               Append($"        {x.message}\n");
        prev = x;
    }

}}
