using System.Collections.Generic;
using System.Linq; using System.IO; using System.Text;
using S = System.String;
using UnityEditor; using static UnityEditor.EditorPrefs;
using static Activ.Loggr.ConfigKeys;

namespace Activ.Loggr{
public class Config : AssetPostprocessor{

    public const int LogLineLength = 32;
    public const int MaxLines = 1000;

    // -------------------------------------------------------------

    public static bool allFrames
    { set => SetBool(AllFrames, value); get => GetBool(AllFrames, true); }

    public static bool enableInjection{
        set{
            if(value == enableInjection) return;
            SetBool(EnableLogging, value);
            // TODO dep? enableInjection should be prolog config
            // not here
            Activ.Prolog.Recompile.Apply();
        }get{
            try{
                return GetBool(EnableLogging);
            }catch(System.Exception){
                //UnityEngine.Debug.Log(ex)
                return false;
            }
        }
    }

    public static float handleSize
    { set => SetFloat(HandleSize, value); get => GetFloat(HandleSize); }

    public static bool logToConsole
    { set => SetBool(LogToConsole, value); get => GetBool(LogToConsole, false); }

    public static bool logToFile => elements?.Contains(FileFlag) ?? false;

    public static int maxMessages
    { set => SetInt(MaxMessages, value); get => GetInt(MaxMessages, 25000); }

    public static int rtypeIndex
    { set => SetInt(RTypeIndex, value); get => GetInt(RTypeIndex); }

    public static bool step
    { set => SetBool(Step, value); get => GetBool(Step); }

    public static float trailOffset
    { set => SetFloat(TrailOffset, value); get => GetFloat(TrailOffset); }

    public static bool useSelection
    { set => SetBool(UseSelection, value); get => GetBool(UseSelection, true); }

    // -------------------------------------------------------------

    public static bool Exclude(string arg) => elements?.Contains(arg) ?? false;

    // Static properties -------------------------------------------

    static List<string> elements = File.Exists(ConfigKeys.Path) ?
        (from s in File.ReadAllText(ConfigKeys.Path).Split('\n')
         select s.Trim()).ToList() : null;

    static void OnPostprocessAllAssets (S[] i, S[] d, S[] m, S[] mf){
        if(!i.Contains(ConfigKeys.Path)) return;
        print("Config changed - recompile"); Activ.Prolog.Recompile.Apply();
    }

    static void print(string x) => UnityEngine.Debug.Log(x);

}}
