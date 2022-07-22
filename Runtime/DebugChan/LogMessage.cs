namespace Activ.Loggr{
public readonly struct LogMessage{

    readonly string message;
    readonly LogInfo info;

    public LogMessage(string message,
                      string path, string member, int line){
        this.message = message;
        this.info = new LogInfo(path, member, line);
    }

}}
