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
    
    public static float ModifyValue(this float value, HamburgerHelperMetadata.GlobalOverlayData.ModifierModes modifierMode, 
        string valueFunction, float defaultValue)
    {
        if (!FrostHelperImports.TryCreateSessionExpression(valueFunction, out object valueFunctionExpression)
            || string.IsNullOrWhiteSpace(valueFunction)) return defaultValue;
        
        Session session = (Engine.Scene as Level)?.Session;
        float functionValue = FrostHelperImports.GetFloatSessionExpressionValue(valueFunctionExpression, session);
        ModificationDelegate modification = modifierMode switch
        {
            HamburgerHelperMetadata.GlobalOverlayData.ModifierModes.Set => DelegateSet,
            HamburgerHelperMetadata.GlobalOverlayData.ModifierModes.Add => DelegateAdd,
            HamburgerHelperMetadata.GlobalOverlayData.ModifierModes.Multiply => DelegateMultiply,
            _ => DelegateSet
        };
        
        return modification(value, functionValue);
    }
    
    public static Vector2 ModifyValue(this Vector2 finalModifiedValue,
        HamburgerHelperMetadata.GlobalOverlayData.ModifierModes modifierModeX,
        HamburgerHelperMetadata.GlobalOverlayData.ModifierModes modifierModeY,
        string valueFunctionExpressionX, string valueFunctionExpressionY, Vector2 defaultValue)
    {
        float x = finalModifiedValue.X.ModifyValue(modifierModeX, valueFunctionExpressionX, defaultValue.X);
        float y = finalModifiedValue.Y.ModifyValue(modifierModeY, valueFunctionExpressionY, defaultValue.Y);
        
        return new Vector2(x, y);
    }
    
    // I couldn't figure out an automatic way to do this so we're doing it manually IG
    public static void InitializeOverlays(this List<HamburgerHelperMetadata.OverlayData> overlays, HamburgerHelperMetadata metadata)
    {
        HamburgerHelperMetadata.GlobalOverlayData globalOverlayData = metadata.ChapterPanelCustomization.OverlaysGlobal;
        foreach (HamburgerHelperMetadata.OverlayData overlayData in overlays)
        {
            if (overlayData.Initialized) return;

            overlayData.Condition ??= globalOverlayData.Condition;
            overlayData.ConditionFlag ??= globalOverlayData.ConditionFlag;
            overlayData.Anchor ??= globalOverlayData.Anchor;
            overlayData.Layer ??= globalOverlayData.Layer;
            overlayData.Color ??= globalOverlayData.Color;
            overlayData.Texture ??= globalOverlayData.Texture;
            overlayData.Rotation ??= globalOverlayData.Rotation;
            overlayData.RotationFunction ??= globalOverlayData.RotationFunction;
            overlayData.RotationMode ??= globalOverlayData.RotationMode;
            overlayData.Scale ??= globalOverlayData.Scale;
            overlayData.ScaleFunction ??= globalOverlayData.ScaleFunction;
            overlayData.ScaleXFunction ??= globalOverlayData.ScaleXFunction;
            overlayData.ScaleYFunction ??= globalOverlayData.ScaleYFunction;
            overlayData.ScaleMode ??= globalOverlayData.ScaleMode;
            overlayData.ScaleXMode ??= globalOverlayData.ScaleXMode;
            overlayData.ScaleYMode ??= globalOverlayData.ScaleYMode;
            overlayData.Offset ??= globalOverlayData.Offset;
            overlayData.OffsetFunction ??= globalOverlayData.OffsetFunction;
            overlayData.OffsetXFunction ??= globalOverlayData.OffsetXFunction;
            overlayData.OffsetYFunction ??= globalOverlayData.OffsetYFunction;
            overlayData.OffsetMode ??= globalOverlayData.OffsetMode;
            overlayData.OffsetXMode ??= globalOverlayData.OffsetXMode;
            overlayData.OffsetYMode ??= globalOverlayData.OffsetYMode;
            overlayData.Animated ??= globalOverlayData.Animated;
            overlayData.AnimationSpeed ??= globalOverlayData.AnimationSpeed;
            overlayData.AnimationOffset ??= globalOverlayData.AnimationOffset;
            overlayData.UseGameplayAtlas ??= globalOverlayData.UseGameplayAtlas;
            overlayData.BlendMode ??= globalOverlayData.BlendMode;
            overlayData.SampleMode ??= globalOverlayData.SampleMode;
            overlayData.DrawCentered ??= globalOverlayData.DrawCentered;
            overlayData.RenderEffect ??= globalOverlayData.RenderEffect;

            overlayData.Initialized = true;
        }
    }

    public static void ProcessSessionExpressions(this HamburgerHelperMetadata.OverlayData overlayData)
    {
        if (FrostHelperImports.IsImported)
        {
            overlayData.FinalRotation = overlayData.FinalRotation.ModifyValue
            (
                overlayData.RotationMode.GetValueOrDefault(),
                overlayData.RotationFunction,
                overlayData.Rotation.GetValueOrDefault()
            );
            
            bool hasScaleFunction = !string.IsNullOrWhiteSpace(overlayData.ScaleFunction);
            overlayData.FinalScale = overlayData.FinalScale.ModifyValue
            (
                hasScaleFunction ? overlayData.ScaleMode.GetValueOrDefault() : overlayData.ScaleXMode.GetValueOrDefault(),
                hasScaleFunction ? overlayData.ScaleMode.GetValueOrDefault() : overlayData.ScaleYMode.GetValueOrDefault(),
                hasScaleFunction ? overlayData.ScaleFunction : overlayData.ScaleXFunction,
                hasScaleFunction ? overlayData.ScaleFunction : overlayData.ScaleYFunction,
                overlayData.OrigScale
            );

            bool hasPositionFunction = !string.IsNullOrWhiteSpace(overlayData.OffsetFunction);
            overlayData.FinalOffset = overlayData.FinalOffset.ModifyValue
            (
                hasPositionFunction ? overlayData.OffsetMode.GetValueOrDefault() : overlayData.OffsetXMode.GetValueOrDefault(),
                hasPositionFunction ? overlayData.OffsetMode.GetValueOrDefault() : overlayData.OffsetYMode.GetValueOrDefault(),
                hasPositionFunction ? overlayData.OffsetFunction : overlayData.OffsetXFunction,
                hasPositionFunction ? overlayData.OffsetFunction : overlayData.OffsetYFunction,
                overlayData.OrigOffset
            );
        }
        else
        {
            overlayData.FinalRotation = overlayData.Rotation.GetValueOrDefault();
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