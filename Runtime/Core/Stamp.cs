//using UnityEngine;

namespace Activ.Loggr{
public class Stamp{

    public readonly int frame;
    public readonly float time;

    public Stamp(int frame, float time){
        this.frame = frame;
        this.time = time;
    }

    public override bool Equals(object other){
        if (other == null) return false;
        if (ReferenceEquals(other, this)) return true;
        if (other.GetType() != this.GetType()) return false;
        return ((Stamp)other).frame == this.frame;
    }

    public override int GetHashCode() => frame;

    public static implicit operator int(Stamp arg)
    => arg.frame;

    public static implicit operator float(Stamp arg)
    => arg.time;

    public static implicit operator Stamp(float arg)
    => new Stamp(-1, arg);

    public static implicit operator Stamp(int arg)
    => new Stamp(arg, -1f);

    public static Stamp operator + (Stamp x, int y)
    => new Stamp(x.frame + y, x.time);

    public static Stamp operator - (Stamp x, int y)
    => new Stamp(x.frame - y, x.time);

    public static bool operator > (Stamp x, Stamp y)
    => x.frame > y.frame;

    public static bool operator > (Stamp x, float y)
    => x.time > y;

    public static bool operator < (Stamp x, float y)
    => x.time < y;

    public static bool operator > (float x, Stamp y)
    => x > y.time;

    public static bool operator < (float x, Stamp y)
    => x < y.time;

    public static bool operator < (Stamp x, Stamp y)
    => x.frame < y.frame;

    public static bool operator == (Stamp x, Stamp y)
        => (x?.frame ?? -1) == (y?.frame ?? -1);

    public static bool operator !=(Stamp x, Stamp y)
    => x.frame != y.frame;

}}
