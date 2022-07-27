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

/*
    [Test] public void TestInclude_True_WithElements(){
        var f0 = new Frame<string>(0);
        var f1 = new Frame<string>(1);
        f0.Add("A"); f0.Add("B");
        f1.Add("A"); f1.Add("B");
        var range = new Range<string>(f0, null);
        Assert.That( range.Include(f1), Is.True );
    }

    [Test] public void TestInclude_NotContiguous(){
        var f0 = new Frame<string>(0);
        var f1 = new Frame<string>(2);
        var range = new Range<string>(f0, null);
        Assert.That( range.Include(f1), Is.False );
    }

    [Test] public void TestInclude_ElemCountMismatch(){
        var f0 = new Frame<string>(0);
        var f1 = new Frame<string>(1);
        f0.Add("A");
        f1.Add("A"); f1.Add("B");
        var range = new Range<string>(f0, null);
        Assert.That( range.Include(f1), Is.False );
    }
*/
}}
