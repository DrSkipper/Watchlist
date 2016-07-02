using UnityEngine;

public class CollisionEvent : LocalEventNotifier.Event
{
    public const string NAME = "COLLISION";
    public GameObject[] Hits;
    public Vector2 VelocityAtHit; // Velocity of actor at time collision was detected, before being multiplied by Time.deltaTime
    public Vector2 VelocityApplied; // How much of the velocity, AFTER Time.deltaTime multiplier, was applied before detecting the collision

    public CollisionEvent(GameObject[] hits, Vector2 velocity, Vector2 velocityApplied)
    {
        this.Name = NAME;
        this.Hits = hits;
        this.VelocityAtHit = velocity;
        this.VelocityApplied = velocityApplied;
    }
}

public class LaserCastEvent : LocalEventNotifier.Event
{
    public const string NAME = "LASER_CAST";
    public CollisionManager.RaycastResult RaycastResult;
    public IntegerVector Origin;
    public AllegianceInfo AllegianceInfo;

    public LaserCastEvent(CollisionManager.RaycastResult raycastResult, IntegerVector origin, AllegianceInfo allegianceInfo)
    {
        this.Name = NAME;
        this.RaycastResult = raycastResult;
        this.Origin = origin;
        this.AllegianceInfo = allegianceInfo;
    }
}

public class HitEvent : LocalEventNotifier.Event
{
    public const string NAME = "DAMAGE";
    public GameObject Hit;

    public HitEvent(GameObject hit)
    {
        this.Name = NAME;
        this.Hit = hit;
    }
}

public class InvincibilityToggleEvent : LocalEventNotifier.Event
{
    public const string NAME = "INVINCIBILITY";
    public bool ToggledOn;

    public InvincibilityToggleEvent(bool toggledOn)
    {
        this.Name = NAME;
        this.ToggledOn = toggledOn;
    }
}

public class PlayerSpawnedEvent : LocalEventNotifier.Event
{
    public const string NAME = "PLAYER_SPAWN";
    public GameObject PlayerObject;
    public int PlayerIndex;

    public PlayerSpawnedEvent(GameObject playerObject, int playerIndex)
    {
        this.Name = NAME;
        this.PlayerObject = playerObject;
        this.PlayerIndex = playerIndex;
    }
}

public class PlayerDiedEvent : LocalEventNotifier.Event
{
    public const string NAME = "PLAYER_DIED";
    public GameObject PlayerObject;
    public int PlayerIndex;

    public PlayerDiedEvent(GameObject playerObject, int playerIndex)
    {
        this.Name = NAME;
        this.PlayerObject = playerObject;
        this.PlayerIndex = playerIndex;
    }
}

public class LevelCompleteEvent : LocalEventNotifier.Event
{
    public const string NAME = "LEVEL_END";

    public LevelCompleteEvent()
    {
        this.Name = NAME;
    }
}

public class MenuElementSelectedEvent : LocalEventNotifier.Event
{
    public const string NAME = "MENU_SELECT";
    public MenuElement Element;
    public string Action;

    public MenuElementSelectedEvent(MenuElement element, string action)
    {
        this.Name = NAME;
        this.Element = element;
        this.Action = action;
    }
}

public class PlayerPointsReceivedEvent : LocalEventNotifier.Event
{
    public const string NAME = "PLAYER_POINTS";
    public int PlayerIndex;
    public int PointsDelta;

    public PlayerPointsReceivedEvent(int playerIndex, int pointsDelta)
    {
        this.Name = NAME;
        this.PlayerIndex = playerIndex;
        this.PointsDelta = pointsDelta;
    }
}

public class BeginGameplayEvent : LocalEventNotifier.Event
{
    public const string NAME = "BEGIN_GAMEPLAY";

    public BeginGameplayEvent()
    {
        this.Name = NAME;
    }
}

public class HideShopEvent : LocalEventNotifier.Event
{
    public const string NAME = "HIDE_SHOP";

    public HideShopEvent()
    {
        this.Name = NAME;
    }
}

public class BeginMusicFadeEvent : LocalEventNotifier.Event
{
    public const string NAME = "MUSIC_FADE";

    public BeginMusicFadeEvent()
    {
        this.Name = NAME;
    }
}

public class PlayMusicEvent : LocalEventNotifier.Event
{
    public const string NAME = "PLAY_MUSIC";

    public PlayMusicEvent()
    {
        this.Name = NAME;
    }
}
