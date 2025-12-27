namespace Celeste.Mod.HamburgerHelper;

public static class HamburgerHelperGFX
{
    private static Dictionary<string, Effect> Effects = new Dictionary<string, Effect>();
    
    internal static void LoadContent()
    {
        // in fullscreen, the game renders directly to the backbuffer, so i need it to preservecontents
        // this makes my rendertarget changes not reset all rendering in OuiEffects
        
        // logging my crimes to make sure if it needs to be found, it can be
        // Logger.Log(LogLevel.Verbose, "HamburgerHelper", "Modified backbuffer usage to PreserveContents");
        // Logger.Log(LogLevel.Verbose, "HamburgerHelper", "If you're another mod developer, and this is a problem, contact me so I can avoid problems in the future");
        // PresentationParameters pp = Engine.Graphics.GraphicsDevice.PresentationParameters;
        // pp.RenderTargetUsage = RenderTargetUsage.PreserveContents;
    }
    
    internal static void UnloadContent()
    {
        ClearEffects();
    }

    public static void ClearEffects()
    {
        foreach (Effect eff in Effects.Values.ToList())
        {
            eff.Dispose();
        }
        Effects.Clear();
    }
    
    /// <summary>
    /// Loads an Effect from from a file path
    /// </summary>
    /// <param name="id">Used for internal shaders (in HamburgerHelper)</param>
    /// <param name="fullpath">Used for external shaders (outside of HamburgerHelper)</param>
    /// <returns></returns>
    public static Effect LoadEffect(string id, string fullpath = null)
    {
        string path = $"Effects/HamburgerHelper/{id}.cso";
        if (fullpath != null) path = $"{fullpath}.cso";

        if (!Everest.Content.TryGet(path, out ModAsset effect))
            Logger.Log(LogLevel.Error, "HamburgerHelperGFX", $"Failed loading effect from {path}");

        if (Effects.TryGetValue(path, out Effect cachedEff)) return cachedEff;
        
        Effects[path] = new Effect(Engine.Graphics.GraphicsDevice, effect.Data);
        Logger.Log(LogLevel.Verbose, "HamburgerHelperGFX", $"Loaded effect from {path}");
        
        return Effects.Values.Last();
    }
}
