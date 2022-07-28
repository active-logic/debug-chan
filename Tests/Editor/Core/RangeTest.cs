using NUnit.Framework;
using UnityEngine;
using Activ.Loggr;

namespace Activ.Loggr.Tests{
public class RangeTest{

    [Test] public void TestInclude(){
        var s0 = new Stamp(0, 5f);
        var s1 = new Stamp(1, 6f);
        var range = new Range<string>(s0, null);
        var f = new Frame<string>(s1);
        Assert.That( range.Include(f), Is.True );
    }

    [Test] public void TestInclude_NeedsContiguous(){
        var s0 = new Stamp(0, 5f);
        var s1 = new Stamp(2, 6f);
        var range = new Range<string>(s0, null);
        var f = new Frame<string>(s1);
        Assert.That( range.Include(f), Is.False );
    }

    [Test] public void TestContains(){
        var range = CreateEmpty(5f, 6f);
        Assert.That( range.Contains(5.5f), Is.True );
        Assert.That( range.Contains(12f), Is.False );
    }

    [Test] public void TestContains_IsLowerBoundInclusive(){
        var range = CreateEmpty(5f, 6f);
        Assert.That( range.Contains(5f), Is.True );
    }

    [Test] public void TestContains_IsUpperBoundExclusive(){
        var range = CreateEmpty(5f, 6f);
        Assert.That( range.Contains(6f), Is.False );
    }

    Range<string> CreateEmpty(float t0, float t1){
        var s0 = new Stamp(0, t0);
        var s1 = new Stamp(1, t1);
        var range = new Range<string>(s0, null);
        range.Include(new Frame<string>(s1));
        return range;
    }

}}
