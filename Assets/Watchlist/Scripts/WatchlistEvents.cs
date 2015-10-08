using UnityEngine;
using System.Collections.Generic;

public class CollisionEvent : LocalEventNotifier.Event
{
    public static string NAME = "COLLISION";
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
    public static string NAME = "LASER_CAST";
    public CollisionManager.RaycastResult RaycastResult;
    public IntegerVector Origin;

    public LaserCastEvent(CollisionManager.RaycastResult raycastResult, IntegerVector origin)
    {
        this.Name = NAME;
        this.RaycastResult = raycastResult;
        this.Origin = origin;
    }
}
