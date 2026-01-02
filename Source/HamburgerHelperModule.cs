using Celeste.Mod.HamburgerHelper.Entities;
using Celeste.Mod.HamburgerHelper.Misc;

namespace Celeste.Mod.HamburgerHelper;

public class HamburgerHelperModule : EverestModule 
{
    public static HamburgerHelperModule Instance { get; private set; }

    public override Type SettingsType => typeof(HamburgerHelperModuleSettings);
    public static HamburgerHelperModuleSettings Settings => (HamburgerHelperModuleSettings) Instance._Settings;

    public override Type SessionType => typeof(HamburgerHelperModuleSession);
    public static HamburgerHelperModuleSession Session => (HamburgerHelperModuleSession) Instance._Session;

    public override Type SaveDataType => typeof(HamburgerHelperModuleSaveData);
    public static HamburgerHelperModuleSaveData SaveData => (HamburgerHelperModuleSaveData) Instance._SaveData;
    
    public HamburgerHelperModule() 
    {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel(nameof(HamburgerHelperModule), LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel(nameof(HamburgerHelperModule), LogLevel.Info);
#endif
    }

    public override void Load() 
    {
        MoveBlockWaitController.Load();
        
        ChapterPanelCustomization.Load();
        OverworldCustomization.Load();
        
        WindowUtils.Load();
        
        HamburgerHelperMetadata.Load();
        HamburgerHelperGFX.Load();
    }

    public override void Unload() 
    {
        MoveBlockWaitController.Unload();
        
        ChapterPanelCustomization.Unload();
        OverworldCustomization.Unload();
        
        WindowUtils.Unload();
        
        HamburgerHelperMetadata.Unload();
        HamburgerHelperGFX.Unload();
        
        HamburgerHelperGFX.UnloadContent();
    }
    
    public override void LoadContent(bool firstLoad)
    {
        ChapterPanelCustomization.LoadContent();
        
        HamburgerHelperGFX.LoadContent();
    }
}