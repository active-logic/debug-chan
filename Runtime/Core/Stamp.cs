using UnityEngine;

namespace Activ.Loggr{
public readonly struct Stamp{

    readonly int frame;
    readonly float time;

    public Stamp(int frame, float time){
        this.frame = Time.frameCount;
        this.time = Time.time;
    }

    public override bool Equals(object other){
        if (other == null) return false;
        if (ReferenceEquals(other, this)) return true;
        if (other.GetType() != this.GetType()) return false;
        return ((Stamp)other).frame == this.frame;
    }

    public override int GetHashCode() => frame;

    public static Stamp operator + (Stamp x, int y)
    => new Stamp(x.frame + y, x.time);

    public static Stamp operator - (Stamp x, int y)
    => new Stamp(x.frame - y, x.time);

    public static bool operator > (Stamp x, Stamp y)
    => x.frame > y.frame;

    public static bool operator < (Stamp x, Stamp y)
    => x.frame < y.frame;

    public static bool operator == (Stamp x, Stamp y)
    => x.frame == y.frame;

    public static bool operator !=(Stamp x, Stamp y)
    => x.frame != y.frame;

}}
