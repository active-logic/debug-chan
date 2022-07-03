using System.Collections.Generic;
using System.Text;

namespace Activ.Loggr{
public static class ListExt{

    public static string Format<T>(this List<T> self){
        var @out = new StringBuilder();
        foreach(var e in self){
            switch(e){
                case Formatting f:
                    @out.Append(f.Format());
                    break;
                default:
                    @out.Append(e.ToString());
                    break;
            }
            @out.Append('\n');
        }
        return @out.ToString();
    }

}}
