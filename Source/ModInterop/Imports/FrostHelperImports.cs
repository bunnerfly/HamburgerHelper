using ModInteropImportGenerator;
using System.Diagnostics.CodeAnalysis;

namespace Celeste.Mod.HamburgerHelper.ModInterop.Imports;

[GenerateImports("FrostHelper")]
public static partial class FrostHelperImports
{
    public static partial bool TryCreateSessionExpression(string str, [NotNullWhen(true)] out object expression);
    public static partial float GetFloatSessionExpressionValue(object expression, Session session);
}