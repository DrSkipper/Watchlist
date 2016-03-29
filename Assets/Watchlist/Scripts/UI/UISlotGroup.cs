using UnityEngine;

public class UISlotGroup : MonoBehaviour
{
    public int PlayerIndex = 0;
    public UISlot[] Slots;

    void Start()
    {
        if (DynamicData.GetSessionPlayer(this.PlayerIndex).HasJoined)
            GlobalEvents.Notifier.Listen(PlayerSpawnedEvent.NAME, this, playerSpawned);
        else
            this.gameObject.SetActive(false);
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
            foreach (UISlot slot in this.Slots)
            {
                slot.SetPlayer(spawnEvent.PlayerObject);
            }
        }
    }
}

