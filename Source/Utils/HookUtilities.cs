using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MethodBody = Mono.Cecil.Cil.MethodBody;

namespace Celeste.Mod.HamburgerHelper.Utils;

public static class HookUtilities
{
    public class HookException : Exception
    {
        public HookException(string message, Exception inner = null) : base($"Hook application failed: {message}", inner) { }
        public HookException(ILContext il, string message, Exception inner = null) : base($"IL hook application on method {il.Method.FullName} failed: {message}", inner) { }
    }
}
