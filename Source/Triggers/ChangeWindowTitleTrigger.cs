namespace Celeste.Mod.HamburgerHelper.Triggers;

[CustomEntity("HamburgerHelper/ChangeWindowTitleTrigger")]
public class ChangeWindowTitleTrigger : Trigger
{
    private readonly string WindowTitle;
    
    private readonly bool UseFlag;
    private readonly string FlagName;
    
    public ChangeWindowTitleTrigger(EntityData data, Vector2 offset) 
        : base(data, offset)
    {
        WindowTitle = data.Attr("windowTitle", "Celeste");
        
        UseFlag = data.Bool("useFlag", false);
        FlagName = data.Attr("flagName", "");
    }

    public override void OnEnter(Player player)
    {
        base.OnEnter(player);

        if (Scene is not Level level) return;
        if (UseFlag && !level.Session.GetFlag(FlagName)) return;
        
        Engine.Instance.Window.Title = WindowTitle;
    }
}