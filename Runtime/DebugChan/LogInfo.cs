namespace Activ.Loggr{
public readonly struct LogInfo{

    readonly string path, member;
    readonly int line;

    public LogInfo(string path, string member, int line){
        this.path = path;
        this.member = member;
        this.line = line;
    }

}}
