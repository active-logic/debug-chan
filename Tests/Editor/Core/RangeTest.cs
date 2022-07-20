using NUnit.Framework;
using UnityEngine;
using Activ.Loggr;

namespace Activ.Loggr.Tests{
public class RangeTest{

    [Test] public void TestInclude_True(){
        var f0 = new Frame<string>(0);
        var f1 = new Frame<string>(1);
        var range = new Range<string>(f0);
        Assert.That( range.Include(f1), Is.True );
        Debug.Log(range.end.frame);
        Assert.That( range.end.frame, Is.EqualTo(1) );
    }

    [Test] public void TestInclude_True_WithElements(){
        var f0 = new Frame<string>(0);
        var f1 = new Frame<string>(1);
        f0.Add("A"); f0.Add("B");
        f1.Add("A"); f1.Add("B");
        var range = new Range<string>(f0);
        Assert.That( range.Include(f1), Is.True );
    }

    [Test] public void TestInclude_NotContiguous(){
        var f0 = new Frame<string>(0);
        var f1 = new Frame<string>(2);
        var range = new Range<string>(f0);
        Assert.That( range.Include(f1), Is.False );
    }

    [Test] public void TestInclude_ElemCountMismatch(){
        var f0 = new Frame<string>(0);
        var f1 = new Frame<string>(1);
        f0.Add("A");
        f1.Add("A"); f1.Add("B");
        var range = new Range<string>(f0);
        Assert.That( range.Include(f1), Is.False );
    }

}}
