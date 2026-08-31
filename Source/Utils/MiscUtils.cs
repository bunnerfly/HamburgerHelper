using Celeste.Mod.HamburgerHelper.ModInterop.Imports;
using MonoMod.Utils;

namespace Celeste.Mod.HamburgerHelper.Utils;

public static class MiscUtils
{
    internal static void Initialize<T>(this DynamicData data, string var, T value)
    {
        if (data.Get<T>(var) is null)
        {
            data.Set(var, value);
        }
    }

    private delegate float ModificationDelegate(float x, float y);
    private static float DelegateSet(this float x, float y) => y;
    private static float DelegateAdd(this float x, float y) => x + y;
    private static float DelegateMultiply(this float x, float y) => x * y;

    internal static float ModifyValue(this float value, HamburgerHelperMetadata.OverlayData.ModifierModes modifierMode, string valueFunction, float defaultValue)
    {
        if (string.IsNullOrWhiteSpace(valueFunction) || !FrostHelperImports.TryCreateSessionExpression(valueFunction, out object valueFunctionExpression)) return defaultValue;

        float functionValue = FrostHelperImports.GetFloatSessionExpressionValue(valueFunctionExpression, (Engine.Scene as Level)?.Session);
        ModificationDelegate modification = modifierMode switch
        {
            HamburgerHelperMetadata.OverlayData.ModifierModes.Set => DelegateSet,
            HamburgerHelperMetadata.OverlayData.ModifierModes.Add => DelegateAdd,
            HamburgerHelperMetadata.OverlayData.ModifierModes.Multiply => DelegateMultiply,
            _ => DelegateSet
        };

        return modification(value, functionValue);
    }

    internal static Vector2 ModifyValue(this Vector2 finalModifiedValue,
        HamburgerHelperMetadata.OverlayData.ModifierModes modifierModeX,
        HamburgerHelperMetadata.OverlayData.ModifierModes modifierModeY,
        string valueFunctionExpressionX,
        string valueFunctionExpressionY,
        Vector2 defaultValue)
    {
        return new Vector2(finalModifiedValue.X.ModifyValue(modifierModeX, valueFunctionExpressionX, defaultValue.X),
            finalModifiedValue.Y.ModifyValue(modifierModeY, valueFunctionExpressionY, defaultValue.Y));
    }
}