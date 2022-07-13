using System.Text;
using UnityEngine;
using Config = Activ.Loggr.Config;

namespace Activ.Prolog{
public static class Formatter{

    public static string State(History history)
    => history.empty ? "History is empty"
                     : history.last.Format();

    public static string Latest(History history){
        int count = 0, N = !history;
        int i = N;
        while((i > 0) && (count < Config.MaxLines))
            count += history[--i].count;
        var x = new StringBuilder();
        for(int z = i; z < N; z++){
            x.Append($"\n#{FrameRange(history, z)} ".PadRight(
                                            Config.LogLineLength, '-') + '\n');
            x.Append(history[z].Format(-1));
        }
        return x.ToString();
    }

    public static string Latest(History history, float startTime){
        var firstId = history.RangeId(startTime) - 1;
        // NOTE: still an error to have a negative id... probably
        if(firstId < 0) firstId = 0;
        //ebug.Log($"Format from {firstId}/{history.count} (φ0: {sinceFrame})");
        var x = new StringBuilder();
        var t = Time.time;
        for(int i = firstId; i < history.count; i++){
            var lapse = t - history[i].time;
            x.Append($"\n[ {FrameRange(history, i)} ] {lapse:0.00}s ago "
                           .PadRight(Config.LogLineLength, '-') + "\n\n");
            x.Append(history[i].Format(-1));
        }
        return x.ToString();
    }

    static string FrameRange(History history, int i){
        int begin = history[i].index, end = history.End(i);
        if(begin == end) return begin.ToString();
        // Upon exiting play mode, frame count is set to zero while components
        // are disabled, as a result, the end index of a range may be -1
        return end >= 0 ? $"{begin} → {end}" : $"{begin} → ({end})";
    }

}}
