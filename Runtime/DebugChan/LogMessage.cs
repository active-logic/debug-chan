namespace Activ.Loggr{
public readonly struct LogMessage{

    #if UNITY_EDITOR_WIN
    const char pathsep = '\\';
    #else
    const char pathsep = '/';
    #endif

    readonly string message;
    readonly LogInfo info;
    readonly string quickFormat;

    public LogMessage(string message,
                      string path, string member, int line){
        this.message = message;
        this.info = new LogInfo(path, member, line);
        var filename = QuickParseFileName(path);
        quickFormat =
            line.ToString().PadLeft(3, '0') + "| "
            + filename + "." + member + ": " + message;
    }

    override public string ToString() => quickFormat;

    static string QuickParseFileName(string arg){
        var i = arg.LastIndexOf(pathsep);
        return arg.Substring(i + 1, arg.Length - (i + 4));
    }

}}
