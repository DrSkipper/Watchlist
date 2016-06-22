using UnityEngine;

public class UISlotGroup : MonoBehaviour
{
    public int PlayerIndex = 0;
    public UISlot[] Slots;

    void Start()
    {
        SessionPlayer player = DynamicData.GetSessionPlayer(this.PlayerIndex);
        if (player.HasJoined)
        {
            for (int i = 0; i < this.Slots.Length; ++i)
            {
                this.Slots[i].UpdateWithSessionPlayer(player);
            }
            GlobalEvents.Notifier.Listen(PlayerPointsReceivedEvent.NAME, this, pointsReceived);
            GlobalEvents.Notifier.Listen(PlayerSpawnedEvent.NAME, this, playerSpawned);
        }
        else
        {
            this.gameObject.SetActive(false);
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
        GlobalEvents.Notifier.RemoveListenersForOwnerAndEventName(this, PlayerPointsReceivedEvent.NAME);
        PlayerSpawnedEvent spawnEvent = playerSpawnedEvent as PlayerSpawnedEvent;

        if (spawnEvent.PlayerIndex == this.PlayerIndex)
        {
            foreach (UISlot slot in this.Slots)
            {
                slot.SetPlayer(spawnEvent.PlayerObject);
            }
        }
    }

    private void pointsReceived(LocalEventNotifier.Event e)
    {
        SessionPlayer player = DynamicData.GetSessionPlayer(this.PlayerIndex);
        if (player.HasJoined && player.PlayerIndex == ((PlayerPointsReceivedEvent)e).PlayerIndex)
        {
            for (int i = 0; i < this.Slots.Length; ++i)
            {
                this.Slots[i].UpdateWithSessionPlayer(player);
            }
        }
    }
}

