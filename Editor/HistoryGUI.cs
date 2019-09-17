using UnityEngine;
using UnityEditor;
using static UnityEngine.Debug;
using static UnityEngine.Vector3;

namespace Activ.Prolog{
public static class HistoryGUI{

    static float size = 0.03f, pickSize = 0.06f, offset = 0f;

    public static Frame Draw(History x, Frame currentSel){
        size     = Config.handleSize;
        pickSize = Config.handleSize * 2;
        offset   = Config.trailOffset;
        if(x == null || x.empty) return null;
        Frame i = null, sel = null;
        foreach(var j in x.frames){
            if(j.empty) continue;
            var J = Pos(j);
            if(i != null) Handles.DrawLine(Pos(i), J);
            if(Select(j)) sel = j;
            i = j;
        }
        Handles.color = Color.red;
        Emphasis(sel ?? currentSel);
        return sel;
    }

    static bool Select(Frame x)
    => (Handles.Button(Pos(x), Rot(x), size, pickSize,
                       Handles.RectangleHandleCap));

    static void Emphasis(Frame x){
        if(x == null) return;
        Handles.Button(Pos(x), Rot(x), size, pickSize,
                       Handles.CubeHandleCap);
    }

    static Vector3 Pos(Frame x) => x.messages[0].owner.position + up * offset;

    static Quaternion Rot(Frame x) => x.messages[0].owner.rotation;

}}
