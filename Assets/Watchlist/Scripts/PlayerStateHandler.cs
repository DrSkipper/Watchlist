using UnityEngine;
using System.Collections.Generic;

public class PlayerStateHandler : MonoBehaviour
{
    public bool UseProgressData = true;

    void Awake()
    {
        if (this.UseProgressData)
        {
            _playerControllers = new PlayerController[DynamicData.MAX_PLAYERS];
            GlobalEvents.Notifier.Listen(PlayerSpawnedEvent.NAME, this, playerSpawned);
            GlobalEvents.Notifier.Listen(LevelCompleteEvent.NAME, this, levelComplete);
        }
    }

    private void playerSpawned(LocalEventNotifier.Event e)
    {
        PlayerSpawnedEvent playerSpawnedEvent = e as PlayerSpawnedEvent;
        PlayerController playerController = playerSpawnedEvent.PlayerObject.GetComponent<PlayerController>();
        playerController.Slots = new List<ProgressData.SlotWrapper>(ProgressData.WeaponSlotsByPlayer[playerSpawnedEvent.PlayerIndex]);
        _playerControllers[playerSpawnedEvent.PlayerIndex] = playerController;
        playerController.SetInitialHealth(ProgressData.GetHealthForPlayer(playerSpawnedEvent.PlayerIndex));
    }

    private void levelComplete(LocalEventNotifier.Event e)
    {
        for (int i = 0; i < _playerControllers.Length; ++i)
        {
            PlayerController playerController = _playerControllers[i];
            ProgressData.SlotWrapper[] slots = playerController != null ? playerController.Slots.ToArray() : new ProgressData.SlotWrapper[0];
            ProgressData.UpdatePlayerSlots(i, slots);
            if (DynamicData.GetSessionPlayer(i).HasJoined)
                ProgressData.SetHealthForPlayer(i, playerController != null ? playerController.GetComponent<Damagable>().Health : 0);
        }
    }

    /**
     * Private
     */
    PlayerController[] _playerControllers;
}
