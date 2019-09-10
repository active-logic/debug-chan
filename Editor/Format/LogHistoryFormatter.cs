using System.Text;
using UnityEngine;

namespace Active.Log{
public class LogHistoryFormatter : Formatter{

    const int Padding = 12;

    public void Append(LogMessage x){
        msg = x;
        if(isNewFrame) Append( (x.frame + " ").PadRight(40, '―') + '\n' );
        Append(  Pad(isNewGO     ? x.ownerName  : "")
               + Pad(isNewObject ? x.sourceType : "")
               + x.message
               + "\n");
        Trim();
        prev = x;
    }

    static string Pad(string s) => s.PadRight(Padding);

}}
