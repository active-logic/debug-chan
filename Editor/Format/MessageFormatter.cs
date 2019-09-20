namespace Activ.Prolog{
public static class MessageFormatter{

    public static string Format(Message x, Message prev){
        if(x == null) throw new System.Exception("don't format null msg");
        var o = x.owner?.name != prev?.owner?.name;
        var t = x.sourceType != prev?.sourceType;
        if(x.owner == null)
            return $"{Z(x.sourceType, o || t)} {x.message}";
        else
            return $"{Y(x.owner.name, o)} {Z(x.sourceType, o || t)} {x.message}";
    }

    static string Y(string x, bool inc)
    => inc ? '\n' + x : new string(' ', x.Length);

    static string Z(string x, bool inc)
    => inc ? x : new string(' ', x.Length);

}}
