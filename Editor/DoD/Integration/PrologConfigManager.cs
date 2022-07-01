using UnityEngine; using UnityEditor;
using ADB = UnityEditor.AssetDatabase;

namespace Activ.Prolog{
public static class PrologConfigManager{

    const string PathKey = "Prolog.Config.Path";
    const string GUIDKey = "Prolog.Config.GUID";

    public static PrologConfig current{
        get{
            var guid = EditorPrefs.GetString(GUIDKey, null);
            string path = null;
            if(guid != null){
                path = ADB.GUIDToAssetPath(guid);
            }
            if(path == null){
                path = EditorPrefs.GetString(PathKey, null);
            }
            if(path == null) return null;
            return ADB.LoadAssetAtPath<PrologConfig>(path);
        }
        set{
            ADB.TryGetGUIDAndLocalFileIdentifier(value,
                out string guid,
                out long localId);
            if(guid == null) return;
            EditorPrefs.SetString(GUIDKey, guid);
            EditorPrefs.SetString(PathKey, ADB.GUIDToAssetPath(guid));
        }
    }

    public static PrologConfig Create(){
        var obj = ScriptableObject.CreateInstance<PrologConfig>();
        var path = "Assets/Prolog.asset";
        ADB.CreateAsset(obj, path);
        EditorPrefs.SetString(PathKey, path);
        EditorPrefs.SetString(GUIDKey, ADB.AssetPathToGUID(path));
        return obj;
    }

}}
