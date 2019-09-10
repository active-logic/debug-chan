//using System;
using System.IO;
//using UnityEngine;
//using UnityEditor;

namespace Active.Log{
public static class LogToFile{

    const string path = "Assets/log.txt";
    static StreamWriter writer;

    static LogToFile(){
        if(!Config.logToFile) return;
        File.Delete(path);
        writer = File.AppendText(path);
    }

    public static void Log(LogMessage x){
        if(Config.logToFile) writer.WriteLine(x); 
    }

}}
