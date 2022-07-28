namespace Activ.Loggr{
public readonly struct LogMessage{

    public readonly string message;
    public readonly LogInfo info;
    public readonly string quickFormat;

    public LogMessage(string message,
                      string path, string member, int line){
        this.message = message;
        this.info = new LogInfo(path, member, line);
        var filename = QuickParseFileName(path);
        quickFormat =
            line.ToString().PadLeft(3, '0') + "| "
            + filename + "." + member + ": " + message;
    }

    public bool Contains(string arg, bool caseSensitive){
        if(arg == null || arg.Trim().Length == 0) return false;
        var self = this.ToString();
        if(!caseSensitive){
            self = self.ToLower();
            arg = arg.ToLower();
        }
        return self.Contains(arg);
    }

    override public string ToString() => quickFormat;

    static string QuickParseFileName(string arg){
        arg = arg.Replace('\\', '/');
        var i = arg.LastIndexOf('/');
        return arg.Substring(i + 1, arg.Length - (i + 4));
    }

}}
