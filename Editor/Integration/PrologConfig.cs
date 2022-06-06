//using System;
using System.Collections.Generic;
using File = System.IO.FileInfo;
using UnityEngine;
using ADB = UnityEditor.AssetDatabase;
using System.Linq;

namespace Activ.Prolog{
[CreateAssetMenu(
    fileName = "prolog", menuName = "Prolog/Prolog Config",
    order    = 10
)] public partial class PrologConfig : ScriptableObject{

    const string Path = "Assets/prolog.asset";
    public static PrologConfig ι;
    public List<Assembly> assemblies;
    public List<TypeExclusion> typeExclusions;

    public static IEnumerable<File> dlls
    => from Assembly x in instance.assemblies select x.file;

    public static bool Exclude(string fqn)
    => instance.typeExclusions.Exists( e => e.Matches(fqn));

    static PrologConfig instance => ι != null
        ? ι :  (ι = PrologConfigManager.current);

    void OnValidate(){
        AddMissing();
        RemoveExtra();
    }

    void AddMissing(){
        List<string> adding = new List<string>(0);
        foreach(var f in Assemblies.std){
            if(!assemblies.Exists( x => x.name == f.Name))
                adding.Add(f.Name);
        }
        if(adding.Count > 0) foreach(var x in adding){
            assemblies.Add(new Assembly(x));
        }
    }

    void RemoveExtra(){
        var all = Assemblies.std;
        assemblies.RemoveAll( x =>
            !all.Exists(y => y.Name == x.name)
        );
    }

}}
