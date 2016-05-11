using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public RectTransform HealthBar;
    public int PlayerIndex = 0;
    public int TargetHeight = 104;

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
        float percentHealth = (float)player.Health / (float)player.MaxHealth;
        int height = Mathf.RoundToInt((float)this.TargetHeight * percentHealth);
        this.HealthBar.sizeDelta = new Vector2(this.HealthBar.sizeDelta.x, height);
    }
}
