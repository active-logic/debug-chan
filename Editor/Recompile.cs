using static UnityEditor.PlayerSettings;
using static UnityEditor.EditorUserBuildSettings;

// Ref:
// answers.unity.com/questions/416711/force-unity-to-recompile-scripts.html
namespace Activ.Prolog{
public static class Recompile{

    const string s0 = "RebuildToggle1", s1 = "RebuildToggle2";

    // TODO - works but looks overdone. If we're setting defines to
    // a temp value, then reverting right away, why do we need to alternate?
    public static void Apply(){
        var g    = selectedBuildTargetGroup;
        var defs = GetScriptingDefineSymbolsForGroup(g);
        var tmp  = defs;
        if      (tmp.Contains(s0)) tmp  = tmp.Replace(s0, s1);
        else if (tmp.Contains(s1)) tmp  = tmp.Replace(s1, s0);
        else                       tmp += ";" + s0;
        SetScriptingDefineSymbolsForGroup(g, tmp);
        SetScriptingDefineSymbolsForGroup(g, defs);
    }

}}
