using UnityEngine;

public class PlayerHealthBar : UIBar
{
    public int PlayerIndex = 0;

    void Start()
    {
        if (DynamicData.GetSessionPlayer(this.PlayerIndex).HasJoined)
            GlobalEvents.Notifier.Listen(PlayerSpawnedEvent.NAME, this, playerSpawned);
    }

    void OnDestroy()
    {
        if (GlobalEvents.Notifier != null)
            GlobalEvents.Notifier.RemoveAllListenersForOwner(this);
    }

    /**
     * Private
     */
    private void playerSpawned(LocalEventNotifier.Event playerSpawnedEvent)
    {
        PlayerSpawnedEvent spawnEvent = playerSpawnedEvent as PlayerSpawnedEvent;
        if (spawnEvent.PlayerIndex == this.PlayerIndex)
        {
            spawnEvent.PlayerObject.GetComponent<Damagable>().OnHealthChangeCallbacks.Add(healthUpdated);
        }
    }

    private void healthUpdated(Damagable player, int damage)
    {
        this.UpdateLength(player.Health, player.MaxHealth);
    }
}
