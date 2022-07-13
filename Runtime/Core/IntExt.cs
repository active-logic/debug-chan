namespace Activ.Loggr{
public static class IntExt{

    public static int? Min(int? a, int? b){
        if(a == null) return b;
        if(b == null) return a;
        return a < b ? a : b;
    }

    public static int? Max(int? a, int? b){
        if(a == null) return b;
        if(b == null) return a;
        return a > b ? a : b;
    }

}}
