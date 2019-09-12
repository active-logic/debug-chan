namespace Active.Log{
public static class MessageFormatter{

    public static string Format(Message x, Message prev){
        var o = x.ownerName  != prev?.ownerName;
        var t = x.sourceType != prev?.sourceType;
        return $"{Y(x.ownerName, o)} {Z(x.sourceType, o || t)} {x.message}";
    }

    static string Y(string x, bool inc)
    => inc ? '\n' + x : new string(' ', x.Length);

    static string Z(string x, bool inc)
    => inc ? x : new string(' ', x.Length);

}}
