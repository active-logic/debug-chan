using UnityEngine;
using System.Text;

namespace Active.Log{
public abstract class Formatter{

    protected LogMessage prev, msg;
    protected StringBuilder builder = new StringBuilder();

    public string output => builder.ToString();
    //
    protected bool isNewFrame   => prev == null || prev.frame  != msg.frame;
    protected bool isNewGO      => prev == null || prev.owner  != msg.owner;
    protected bool isNewObject  => prev == null || prev.source != msg.source;

    protected void Clear(string header=null){
        builder = new StringBuilder(header == null ? null : header + "\n");
        prev = null;
    }

    protected void Append(string msg) => builder.Append(msg);

    protected void Trim(){
        if(builder.Length > Format.maxCharCount)
            builder.Remove(0, Format.maxCharCount/2);
    }

}}
