using System;

namespace Activ.Prolog{
public partial class PrologConfig{

[Serializable] public class TypeExclusion{

    public string name;
    public bool strict = false;

    public TypeExclusion(string name) => this.name = name;

    public bool Matches(string arg) => strict
        ? arg == name
        : arg.Contains(name);

}}}
