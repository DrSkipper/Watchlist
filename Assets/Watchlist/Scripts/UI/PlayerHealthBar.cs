using UnityEngine;

public class PlayerHealthBar : UIBar
{
    public int PlayerIndex = 0;

    void Start()
    {
        if (DynamicData.GetSessionPlayer(this.PlayerIndex).HasJoined)
        {
            this.UpdateLength(ProgressData.GetHealthForPlayer(this.PlayerIndex), ProgressData.MAX_HEALTH);
            GlobalEvents.Notifier.Listen(PlayerPointsReceivedEvent.NAME, this, pointsReceived);
            GlobalEvents.Notifier.Listen(PlayerSpawnedEvent.NAME, this, playerSpawned);
        }
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
            GlobalEvents.Notifier.RemoveListenersForOwnerAndEventName(this, PlayerPointsReceivedEvent.NAME);
            spawnEvent.PlayerObject.GetComponent<Damagable>().OnHealthChangeCallbacks.Add(healthUpdated);
        }
    }

    private void healthUpdated(Damagable player, int damage)
    {
        this.UpdateLength(player.Health, player.MaxHealth);
    }

    private void pointsReceived(LocalEventNotifier.Event e)
    {
        SessionPlayer player = DynamicData.GetSessionPlayer(this.PlayerIndex);
        if (player.HasJoined && player.PlayerIndex == ((PlayerPointsReceivedEvent)e).PlayerIndex)
        {
            this.UpdateLength(ProgressData.GetHealthForPlayer(this.PlayerIndex), ProgressData.MAX_HEALTH);
        }
    }
}
