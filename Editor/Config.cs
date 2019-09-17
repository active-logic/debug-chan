using System.Collections.Generic;
using System.Linq; using System.IO; using System.Text;
using S = System.String;
using UnityEditor; using static UnityEditor.EditorPrefs;
using static Activ.Prolog.ConfigKeys;

namespace Activ.Prolog{
public class Config : AssetPostprocessor{

    public const int LogLineLength = 32;
    public const int MaxLines = 1000;

    static List<string> elements = File.Exists(ConfigKeys.Path) ?
        (from s in File.ReadAllText(ConfigKeys.Path).Split('\n')
         select s.Trim()).ToList() : null;

    public static bool logToFile => elements?.Contains(FileFlag) ?? false;

    public static bool enable{
        set{
            if(value == enable) return;
            SetBool(EnableLogging, value);
            Recompile.Apply();
        } get => GetBool(EnableLogging);
    }

    public static bool allFrames
    { set => SetBool(AllFrames, value); get => GetBool(AllFrames); }

    public static bool useSelection
    { set => SetBool(UseSelection, value); get => GetBool(UseSelection); }

    public static float trailOffset
    { set => SetFloat(TrailOffset, value); get => GetFloat(TrailOffset); }

    public static float handleSize
    { set => SetFloat(HandleSize, value); get => GetFloat(HandleSize); }

    public static int rtypeIndex
    { set => SetInt(RTypeIndex, value); get => GetInt(RTypeIndex); }

    public static bool step
    { set => SetBool(Step, value); get => GetBool(Step); }

    public static bool Exclude(string arg) => elements?.Contains(arg) ?? false;

    static void OnPostprocessAllAssets (S[] i, S[] d, S[] m, S[] mf){
        if(!i.Contains(ConfigKeys.Path)) return;
        print("Config changed - recompile"); Recompile.Apply();
    }

    static void print(string x) => UnityEngine.Debug.Log(x);

}}
