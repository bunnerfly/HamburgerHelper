using MonoMod.Cil;

namespace Celeste.Mod.HamburgerHelper.Entities;

[Tracked]
[CustomEntity("HamburgerHelper/WallboostLeniencyController")]
public class WallboostLeniencyController : Entity
{
    private readonly int WallboostFrames;
    private float WallboostTime => WallboostFrames * (1 / 60f);
    
    public WallboostLeniencyController(EntityData data, Vector2 offset) 
        : base(data.Position + offset)
    {
        WallboostFrames = data.Int("wallboostFrames", 12);
    }

    [OnLoad]
    internal static void Load()
    {
        IL.Celeste.Player.ClimbJump += PlayerOnClimbJump;
    }

    [OnUnload]
    internal static void Unload()
    {
        IL.Celeste.Player.ClimbJump -= PlayerOnClimbJump;
    }

    private static void PlayerOnClimbJump(ILContext il)
    {
        ILCursor cursor = new ILCursor(il);
        
        /*
         * IL_0057: ldarg.0
         * IL_0058: ldc.r4 0.2
         * IL_005d: stfld float32 Celeste.Player::wallBoostTimer
         */
        if (!cursor.TryGotoNext(MoveType.After,
            i => i.MatchLdarg0(),
            i => i.MatchLdcR4(0.2f),
            i => i.MatchStfld<Player>("wallBoostTimer")))
            throw new HookException(il, "ClimbJump hook failed at finding wallBoostTimer");
        
        cursor.Index--;
        
        cursor.EmitLdarg0();
        cursor.EmitDelegate(ModWallboostTimer);
        
        return;
        
        static float ModWallboostTimer(float orig, Player self)
        {
            WallboostLeniencyController controller = self?.Scene?.Tracker?.GetEntity<WallboostLeniencyController>();
            return controller?.WallboostTime ?? orig;
        }
    }
}
