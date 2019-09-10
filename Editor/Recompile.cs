using UnityEngine;
using UnityEditor;

// Ref: https://answers.unity.com/questions/416711/force-unity-to-recompile-scripts.html
namespace Active.Log{
public class Recompile{

    public static void Apply(){
        string[] rebuildSymbols = { "RebuildToggle1", "RebuildToggle2" };
        string definesString =
            PlayerSettings.GetScriptingDefineSymbolsForGroup(
                             EditorUserBuildSettings.selectedBuildTargetGroup);
        var definesStringTemp = definesString;
        if (definesStringTemp.Contains(rebuildSymbols[0])){
            definesStringTemp = definesStringTemp.Replace(
                                         rebuildSymbols[0], rebuildSymbols[1]);
        }else if (definesStringTemp.Contains(rebuildSymbols[1])){
            definesStringTemp = definesStringTemp.Replace(
                                         rebuildSymbols[1], rebuildSymbols[0]);
        }else{
            definesStringTemp += ";" + rebuildSymbols[0];
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            definesStringTemp);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            definesString);
    }

}}
