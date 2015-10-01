using UnityEngine;
using System.Collections.Generic;

public class CollisionEvent : LocalEventNotifier.Event
{
    public static string NAME = "COLLISION";
    GameObject[] Hits;

    public CollisionEvent(GameObject[] hits)
    {
        this.Name = NAME;
        this.Hits = hits;
    }
}
