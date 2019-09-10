using UnityEngine;
using UnityEditor;
using System.Linq;
using S = System.String;

namespace Active.Log{
public class DetectChanges : AssetPostprocessor{

     static void OnPostprocessAllAssets (S[] i, S[] d, S[] m, S[] mf){
         if(i.Contains(Config.Path)){
             Debug.Log("Force recompile");
             Recompile.Apply();
         }
     }

}}
