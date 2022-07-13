using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Activ.Prolog.IL{
public class Injector{

    static System.Type LOGGER => typeof(Activ.Prolog.Logger);
    static System.Type OBJECT => typeof(object);
    static System.Type STRING => typeof(string);

    public static void Inject(TypeDefinition type, MethodDefinition method){
        if(method.IsStatic) InjectClassMethod    (type, method);
        else                InjectInstanceMethod (type, method);
    }

    static void InjectInstanceMethod(TypeDefinition type,
                                            MethodDefinition method){
        var il    = method.Body.GetILProcessor ();
        var call  = il.Create (OpCodes.Call, method.Module.ImportReference (
                    LOGGER.GetMethod ("Log", new[]{ OBJECT, STRING })));
        il.InsertBefore(First(method), call);
        il.InsertBefore(First(method), MethodLdstr(il, method));
        il.InsertBefore(First(method), il.Create(OpCodes.Ldarg_0));  // 'this'
    }

    static void InjectClassMethod(TypeDefinition type,
                                  MethodDefinition method){
        var il   = method.Body.GetILProcessor ();
        var call = il.Create (OpCodes.Call, method.Module.ImportReference (
                   LOGGER.GetMethod ("LogStatic", new[]{ STRING, STRING })));
        il.InsertBefore(First(method), call);
        il.InsertBefore(First(method), MethodLdstr(il, method));
        il.InsertBefore(First(method), il.Create (OpCodes.Ldstr, type.Name));
    }

    static Instruction MethodLdstr(ILProcessor il, MethodDefinition method)
    => il.Create (OpCodes.Ldstr, ArgFormatter.Signature(method));

    static Instruction First(MethodDefinition m)
    => m.Body.Instructions[0];

}}
