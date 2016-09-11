using UnityEngine;

public class PlayerVirtualJoystickHandler : MonoBehaviour
{
    public int PlayerIndex = 0;
    public DynamicVirtualJoystick MoveStick;
    public DynamicVirtualJoystick AimStick;

    void Awake()
    {
        GlobalEvents.Notifier.Listen(PlayerSpawnedEvent.NAME, this, playerSpawned);
    }

    /**
     * Private
     */
    private void playerSpawned(LocalEventNotifier.Event e)
    {
        PlayerSpawnedEvent spawnEvent = e as PlayerSpawnedEvent;
        if (spawnEvent.PlayerIndex == this.PlayerIndex)
        {
            PlayerController player = spawnEvent.PlayerObject.GetComponent<PlayerController>();
            this.MoveStick.AddUpdateCallback(player.UpdateVirtualMoveStick);
            this.AimStick.AddUpdateCallback(player.UpdateVirtualAimStick);
        }
    }
}
