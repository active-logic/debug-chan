using System.Collections.Generic;
using System.Linq;
using static System.IO.File;

public static class Config{

    const string path = "Assets/.prolog";

    static List<string> exclusions = Exists(path)
        ? (from s in ReadAllText(path).Split('\n') select s.Trim()).ToList()
        : null;

    public static bool Exclude(string arg)
    => exclusions?.Contains(arg) ?? false;

}
