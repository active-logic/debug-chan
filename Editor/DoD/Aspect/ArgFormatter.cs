using Mono.Cecil;

namespace Activ.Prolog.IL{
public static class ArgFormatter{

    public static string Signature(MethodDefinition m)
    => $"{Access(m)}{m.Name} → {m.ReturnType.Name}";

    static char Access(MethodDefinition m)
    => m.IsPublic ? '+' : '-';

}}
