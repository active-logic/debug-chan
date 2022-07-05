using NUnit.Framework;
using UnityEngine;
using Activ.Loggr;

namespace Activ.Loggr.Tests{
public class FrameTest{

    [Test] public void Construct_withInt(){
        var f0 = new Frame<string>(0);
        Assert.That( f0.time.frame, Is.EqualTo(0) );
        Assert.That( f0.messages.Count, Is.EqualTo(0) );
    }

    [Test] public void Construct_withStamp(){
        var f0 = new Frame<string>( new Stamp(2, 5.3f) );
        Assert.That( f0.time.frame, Is.EqualTo(2) );
        Assert.That( f0.time.time, Is.EqualTo(5.3f) );
        Assert.That( f0.messages.Count, Is.EqualTo(0) );
    }

}}
