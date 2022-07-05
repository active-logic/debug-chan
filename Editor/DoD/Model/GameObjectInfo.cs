using UnityEngine;

namespace Activ.Prolog{
public class GameObjectInfo{

    public readonly string     name;
    public readonly Vector3    position;
    public readonly Quaternion rotation;
    public readonly GameObject @ref;

    public GameObjectInfo(GameObject @ref){
        this.@ref = @ref;
        name      = @ref.name;
        position  = @ref.transform.position;
        rotation  = @ref.transform.rotation;
    }

    public static implicit operator GameObjectInfo(GameObject x)
    => x == null ? null : new GameObjectInfo(x);

    public static implicit operator GameObject(GameObjectInfo x)
    => x == null ? null : x.@ref;

    override public string ToString() => name;

}}
