using UnityEngine;
using System.Collections.Generic;

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
    }
}
