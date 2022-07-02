namespace Activ.LogChan{
public partial class MessageLogger{

    public delegate void MessageEventHandler(object sender, Message arg);
    public static event MessageEventHandler messageReceived;

}}
