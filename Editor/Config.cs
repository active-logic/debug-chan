using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using System.IO;
using System.Text;
using S = System.String;

namespace Active.Log{
public class Config : AssetPostprocessor{

    const string DISABLE_FLAG = "--disable";
    const string FILE_FLAG    = "--file";
    //
    public const int    LogLineLength = 26;
    public const int    MaxLines = 1000;
    public const string LogPath  = "Assets/log.txt",
                        Name     = "prolog.config",
                        Path     = "Assets/prolog.config";

    static List<string> elements = File.Exists(Path) ?
        (from s in File.ReadAllText(Path).Split('\n') select s.Trim()).ToList()
        : null;

    public static bool logToFile => elements?.Contains(FILE_FLAG) ?? false;

    public static bool enable{
        get{ return !elements?.Contains(DISABLE_FLAG) ?? true;   }
        set{ if(enable != value) SetFlag(DISABLE_FLAG, !value); }
    }

    public static bool Exclude(string arg) => elements?.Contains(arg) ?? false;

    static void SetFlag(string x, bool apply){
        if(apply) elements.Insert(0, x);
        else      elements.Remove(x);
        Write();
    }

    static void Write(){
        StringBuilder builder = new StringBuilder();
        foreach(var k in elements) builder.Append(k + '\n');
        File.Delete(Path);
        using(var w = File.AppendText(Path)){ w.Write(builder.ToString()); }
        Recompile.Apply();
    }

    static void OnPostprocessAllAssets (S[] i, S[] d, S[] m, S[] mf){
        if(i.Contains(Config.Path)){
            print("Config changed - recompile");
            Recompile.Apply();
        }
    }

    static void print(string x) => UnityEngine.Debug.Log(x);

}}
