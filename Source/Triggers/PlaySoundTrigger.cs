namespace Celeste.Mod.HamburgerHelper.Triggers;

[CustomEntity("HamburgerHelper/PlaySoundTrigger")]
public class PlaySoundTrigger : Trigger
{
    private readonly string EventId;
    private readonly string ParameterName;
    private readonly float ParameterValue;
    
    private readonly bool TriggerOnce;
    private readonly bool OnlyOnce;

    private readonly EntityID Id;
    
    public PlaySoundTrigger(EntityData data, Vector2 offset, EntityID id) 
        : base(data, offset)
    {
        EventId = data.Attr("eventId", "event:/none");
        ParameterName = data.Attr("parameterName", "");
        ParameterValue = data.Float("parameterValue", 0);
        
        TriggerOnce = data.Bool("triggerOnlyOnce", false);
        OnlyOnce = data.Bool("onlyOnce", false);
        
        Id = id;
    }

    public override void OnEnter(Player player)
    {
        base.OnEnter(player);

        if (Scene is not Level level) return;
        
        Audio.Play(EventId, ParameterName, ParameterValue);
        
        if (TriggerOnce)
        {
            RemoveSelf();
        }

        // ReSharper disable once InvertIf
        if (OnlyOnce)
        {
            level.Session.SetFlag("DoNotLoad" + Id);
            level.Session.DoNotLoad.Add(Id);
            
            RemoveSelf();
        }
    }
}
