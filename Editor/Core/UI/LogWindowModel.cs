using UnityEngine;
using Activ.Loggr;
using Ints = Activ.Loggr.IntExt;
using PrologHistory = Activ.Prolog.History;
using PrologFrame = Activ.Prolog.Frame;
using PrologFilter = Activ.Prolog.Filter;
using PrologFormatter = Activ.Prolog.Formatter;
using PrologLogger = Activ.Prolog.Logger;

namespace Activ.Loggr.UI{
public class LogWindowModel{

    public GameObject current;
    public int? currentFrame;
    public PrologHistory filtered{ get; private set; }
    PrologFilter filter;
    public PrologFrame   pgRange;  // selected prolog range
    public Range<string> dcRange;  // selected debug-chan range

    public LogWindowModel(){
        PrologLogger.onFrame += OnPrologFrame;
    }

    public void Clear(){
        filtered = null;
        current = null;
        pgRange = null;
        dcRange = null;
    }

    public void Next(){
        int? index = Ints.Min(
            filtered.FirstStopAfter(currentFrame, current),
            DebugChan.logger.FirstStopAfter(currentFrame, current)
        );
        SetCurrentFrame(index);
    }

    public void Prev(){
        int? index = Ints.Max(
            filtered?.LastStopBefore(currentFrame, current),
            DebugChan.logger.LastStopBefore(currentFrame, current)
        );
        SetCurrentFrame(index);
    }

    public void SetCurrentFrame(int? frameIndex){
        if(frameIndex.HasValue){
            var i = frameIndex.Value;
            pgRange = filtered.At(i);
            dcRange = DebugChan.logger.At(i, current);
        }else{
            pgRange = null;
            dcRange = null;
        }
    }

    public string Output(bool useHistory, string rtype){
        filter = new PrologFilter(selection, rtype);
        return useHistory ? PrologFormatter.Latest(source)
                          : PrologFormatter.State(source);
    }

    public void OnPrologFrame(PrologFrame frame){
        if(frame == null) return;
        //Debug.Log($"Got new frame {frame}");
        filtered += frame;
        Activ.Loggr.UI.LogWindow.instance?.Repaint();
    }

    public GameObject selection
    => Config.useSelection ? current : null;

    PrologHistory source
    => selection ? (filtered /= filter) : PrologLogger.history;

}}
