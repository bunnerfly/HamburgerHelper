// ReSharper disable ConvertToExtensionBlock

using Celeste.Mod.CollabUtils2;
using Celeste.Mod.HamburgerHelper.ModInterop.Imports;

namespace Celeste.Mod.HamburgerHelper.Utils;

public static class OverlayUtils
{
    private delegate float ModificationDelegate(float x, float y);
    private static float DelegateSet(this float x, float y) => y;
    private static float DelegateAdd(this float x, float y) => x + y;
    private static float DelegateMultiply(this float x, float y) => x * y;
    
    public static float ModifyValue(this float value, HamburgerHelperMetadata.OverlayData.ModifierModes modifierMode, 
        string valueFunction, float defaultValue)
    {
        if (!FrostHelperImports.TryCreateSessionExpression(valueFunction, out object valueFunctionExpression)
            || string.IsNullOrWhiteSpace(valueFunction)) return defaultValue;
        
        Session session = (Engine.Scene as Level)?.Session;
        float functionValue = FrostHelperImports.GetFloatSessionExpressionValue(valueFunctionExpression, session);
        ModificationDelegate modification = modifierMode switch
        {
            HamburgerHelperMetadata.OverlayData.ModifierModes.Set => DelegateSet,
            HamburgerHelperMetadata.OverlayData.ModifierModes.Add => DelegateAdd,
            HamburgerHelperMetadata.OverlayData.ModifierModes.Multiply => DelegateMultiply,
            _ => DelegateSet
        };
        
        return modification(value, functionValue);
    }
    
    public static Vector2 ModifyValue(this Vector2 finalModifiedValue,
        HamburgerHelperMetadata.OverlayData.ModifierModes modifierModeX,
        HamburgerHelperMetadata.OverlayData.ModifierModes modifierModeY,
        string valueFunctionExpressionX, string valueFunctionExpressionY, Vector2 defaultValue)
    {
        float x = finalModifiedValue.X.ModifyValue(modifierModeX, valueFunctionExpressionX, defaultValue.X);
        float y = finalModifiedValue.Y.ModifyValue(modifierModeY, valueFunctionExpressionY, defaultValue.Y);
        
        return new Vector2(x, y);
    }
    
    public static void ProcessSessionExpressions(this HamburgerHelperMetadata.OverlayData overlayData)
    {
        if (FrostHelperImports.IsImported)
        {
            overlayData.FinalRotation = overlayData.FinalRotation.ModifyValue(overlayData.RotationMode, overlayData.RotationFunction, overlayData.Rotation);
            
            bool hasScaleFunction = !string.IsNullOrWhiteSpace(overlayData.ScaleFunction);
            overlayData.FinalScale = overlayData.FinalScale.ModifyValue
            (
                hasScaleFunction ? overlayData.ScaleMode : overlayData.ScaleXMode,
                hasScaleFunction ? overlayData.ScaleMode : overlayData.ScaleYMode,
                hasScaleFunction ? overlayData.ScaleFunction : overlayData.ScaleXFunction,
                hasScaleFunction ? overlayData.ScaleFunction : overlayData.ScaleYFunction,
                overlayData.OrigScale
            );

            bool hasPositionFunction = !string.IsNullOrWhiteSpace(overlayData.OffsetFunction);
            overlayData.FinalOffset = overlayData.FinalOffset.ModifyValue
            (
                hasPositionFunction ? overlayData.OffsetMode : overlayData.OffsetXMode,
                hasPositionFunction ? overlayData.OffsetMode : overlayData.OffsetYMode,
                hasPositionFunction ? overlayData.OffsetFunction : overlayData.OffsetXFunction,
                hasPositionFunction ? overlayData.OffsetFunction : overlayData.OffsetYFunction,
                overlayData.OrigOffset
            );
        }
        else
        {
            overlayData.FinalRotation = overlayData.Rotation;
            overlayData.FinalScale = overlayData.OrigScale;
            overlayData.FinalOffset = overlayData.OrigOffset;
        }
    }
    
    public static bool GoldenCollected(AreaKey key, AreaModeStats stats)
        => AreaData.Get(key).Mode[(int) key.Mode].MapData.Goldenberries.Any(berry => stats.Strawberries.Contains(new EntityID(berry.Level.Name, berry.ID)));
    public static bool SilverCollected(AreaKey key, AreaModeStats stats)
        => GoldenCollected(key, stats) && CollabMapDataProcessor.MapsWithSilverBerries.Contains(key.SID);
    public static bool RainbowCollected(AreaKey key, AreaModeStats stats)
        => GoldenCollected(key, stats) && CollabMapDataProcessor.MapsWithRainbowBerries.Contains(key.SID);
    public static bool CheckFlagCondition(HamburgerHelperMetadata.OverlayData overlayData, bool invert = false)
        => SaveData.Instance.HasFlag(overlayData.ConditionFlag) != invert;
}