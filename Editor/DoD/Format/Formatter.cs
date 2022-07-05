using System.Text;
using UnityEngine;

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

    static string FrameRange(History history, int i){
        int begin = history[i].index, end = history.End(i);
        if(begin == end) return begin.ToString();
        // Upon exiting play mode, frame count is set to zero while components
        // are disabled, as a result, the end index of a range may be -1
        return end >= 0 ? $"{begin}→{end}" : $"{begin}→({end})";
    }

}}
