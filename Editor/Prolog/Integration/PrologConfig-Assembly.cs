using System;
using File = System.IO.FileInfo;

namespace Activ.Prolog{
public partial class PrologConfig{

[Serializable] public class Assembly{

    public string name;
    public bool inject;

    public Assembly(string name) => this.name = name;

    public File file => new File(Assemblies.root + "/" + name);

}}}
