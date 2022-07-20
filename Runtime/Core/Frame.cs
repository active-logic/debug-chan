using System.Collections.Generic;

namespace Activ.Loggr{
public class Frame<T>{

    public Stamp time { get; private set; }
    public readonly List<T> messages = new List<T>();

    public Frame(Stamp time) => this.time = time;

    public void Add(T message) => messages.Add(message);

    // Reset a frame; allows reusing the same frame object over
    // and over; this is for optimization, as it will also avoid
    // reallocating the message list, and we (hopefully) avoid
    // resizing the list too often.
    // TODO - verify whether Clear() is likely to trim capacity
    public void Clear(Stamp time){
        this.time = time;
        messages.Clear();
    }

    public string Format() => messages.Format();

}}
