using UnityEditor;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Active.Log{
[InitializeOnLoad]
public static class LoggingManager{

    static LoggingManager(){
        //Debug.Log("Logging manager started; instrument...");
        //var watch = Stopwatch.StartNew();
        LoggingAspect.Process();
        //Debug.Log($"...{(int)watch.Elapsed.TotalMilliseconds}ms");
    }

}}
