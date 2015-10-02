using UnityEngine;
using System.Collections.Generic;

public class CollisionEvent : LocalEventNotifier.Event
{
    public static string NAME = "COLLISION";
    public GameObject[] Hits;
    public Vector2 VelocityAtHit;

    public CollisionEvent(GameObject[] hits, Vector2 velocity)
    {
        this.Name = NAME;
        this.Hits = hits;
        this.VelocityAtHit = velocity;
    }
}
