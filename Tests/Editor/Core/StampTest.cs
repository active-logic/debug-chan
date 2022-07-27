using NUnit.Framework;
using UnityEngine;
using Activ.Loggr;

namespace Activ.Loggr.Tests{
public class StampTest{

    [Test] public void Op_LessThan(){
        var s = new Stamp(0, 5f);
        Assert.That( 3f < s, Is.True );
        Assert.That( s < 6f, Is.True );
    }

    [Test] public void Op_MoreThan(){
        var s = new Stamp(0, 5f);
        Assert.That( 6f > s, Is.True );
        Assert.That( s > 3f, Is.True );
    }

}}
