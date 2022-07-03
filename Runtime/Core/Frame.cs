using System.Collections.Generic;

namespace Activ.Loggr{
public class Frame<T>{

    public readonly Stamp time;
    public readonly List<T> messages = new List<T>();

    public Frame(Stamp time) => this.time = time;

    public void Add(T message) => messages.Add(message);

    public string Format() => messages.Format();

}}
