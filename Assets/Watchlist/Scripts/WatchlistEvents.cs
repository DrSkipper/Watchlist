using UnityEngine;
using System.Collections.Generic;

public class CollisionEvent : LocalEventNotifier.Event
{
    public static string NAME = "COLLISION";
    List<RaycastHit2D> Hits;

    public CollisionEvent(List<RaycastHit2D> hits)
    {
        this.Name = NAME;
        this.Hits = hits;
    }
}
