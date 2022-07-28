namespace Activ.Loggr{
public readonly struct LogInfo{

    public readonly string path, member;
    public readonly int line;

    public LogInfo(string path, string member, int line){
        this.path = path;
        this.member = member;
        this.line = line;
    }

}}
