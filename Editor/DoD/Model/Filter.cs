using UnityEngine;

namespace Activ.Prolog{
public readonly struct Filter{

    const string NoSel = "(no sel)";
    public readonly GameObject selection;
    public readonly string rtype;

    public Filter(GameObject go, string t){
        if(t == "void") t = "Void";
        selection = go;
        rtype = t;
    }

    public bool isNeutral => (selection == null) && (rtype == "any");

    public static bool operator == (in Filter x, in Filter y) => x.Equals(y);

    public static bool operator != (in Filter x, in Filter y) => !x.Equals(y);

    public override bool Equals(object x) => x is Filter && Equals((Filter)x);

    public bool Accept(Message msg){
        if(selection != null ) if(msg.owner != selection)       return false;
        if(rtype     != "any") if(!msg.message.EndsWith(rtype)) return false;
        return true;
    }

    public bool Equals(Filter x) => selection==x.selection && rtype==x.rtype;

    public override string ToString()
    => $"Filter[{selection?.name ?? NoSel}, {rtype}]";

    public override int GetHashCode() => 0;  // Dummy

}}
