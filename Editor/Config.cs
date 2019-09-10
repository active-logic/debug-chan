using System.Collections.Generic;
using System.Linq;
using static System.IO.File;

namespace Active.Log{
public static class Config{

    public const string Name = "prolog.config";
    public const string Path = "Assets/prolog.config";

    static List<string> elements = Exists(Path)
        ? (from s in ReadAllText(Path).Split('\n') select s.Trim()).ToList()
        : null;

    public static bool logToFile
    => elements?.Contains("--file") ?? false;

    public static bool Exclude(string arg)
    => elements?.Contains(arg) ?? false;

}}
