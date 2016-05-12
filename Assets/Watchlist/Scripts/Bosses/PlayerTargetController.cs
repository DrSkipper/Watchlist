using UnityEngine;
using System.Collections.Generic;

public class PlayerTargetController : MonoBehaviour
{
    public static List<Transform> Targets;

    void Awake()
    {
        Targets = new List<Transform>();
        GlobalEvents.Notifier.Listen(PlayerSpawnedEvent.NAME, this, playerSpawned);
        GlobalEvents.Notifier.Listen(PlayerDiedEvent.NAME, this, playerDied);
    }

    void OnDestroy()
    {
        Targets = null;
    }

    private void playerSpawned(LocalEventNotifier.Event e)
    {
        Targets.Add((e as PlayerSpawnedEvent).PlayerObject.transform);
    }

    private void playerDied(LocalEventNotifier.Event e)
    {
        Targets.Remove((e as PlayerDiedEvent).PlayerObject.transform);
    }
}
