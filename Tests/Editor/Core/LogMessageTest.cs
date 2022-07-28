using NUnit.Framework;
using UnityEngine;
using Activ.Loggr;

namespace Activ.Loggr.Tests{
public class LogMessageTest{

    LogMessage x;

    [SetUp] public void Setup(){
        x = new LogMessage("Hello", "C:/Foo/Bar.cs", "Update", 123);
    }

    [Test] public void Constructor(){
        var msg = new LogMessage("Hello", "C:/Foo/Bar.cs", "Update", 123);
        Assert.That( msg.message, Is.EqualTo("Hello") );
        Assert.That( msg.info.path, Is.EqualTo("C:/Foo/Bar.cs") );
        Assert.That( msg.info.member, Is.EqualTo("Update") );
        Assert.That( msg.info.line, Is.EqualTo(123) );
        Assert.That( msg.ToString(), Is.EqualTo("123| Bar.Update: Hello"));
    }

    [Test] public void Contains_Empty([Values(true, false)] bool caseSensitive){
        Assert.That( x.Contains(null, caseSensitive), Is.False  );
        Assert.That( x.Contains(" " , caseSensitive), Is.False  );
        Assert.That( x.Contains(""  , caseSensitive), Is.False  );
    }

    [Test] public void Contains([Values(true, false)] bool caseSensitive){
        if(caseSensitive){
            Assert.That( x.Contains("Hell", caseSensitive), Is.True  );
            Assert.That( x.Contains("hell", caseSensitive), Is.False );
        }else{
            Assert.That( x.Contains("Hell", caseSensitive), Is.True );
            Assert.That( x.Contains("hell", caseSensitive), Is.True );
        }
    }

}}
