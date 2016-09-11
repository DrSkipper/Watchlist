using UnityEngine;
using Rewired;

public class ControllerCursor : MonoBehaviour
{
    public int PlayerIndex = 0;
    public float AimingImpact = 50.0f;
    public int ReferenceHeight = 600;

    void Start()
    {
        _rectTransform = this.GetComponent<RectTransform>();
        _rectTransform.position = new Vector3(-99999, -99999, _rectTransform.position.z);
        GlobalEvents.Notifier.Listen(PlayerSpawnedEvent.NAME, this, playerSpawned);
        GlobalEvents.Notifier.Listen(PlayerDiedEvent.NAME, this, playerDied);
        _aimingImpactTotal = this.AimingImpact * Screen.height / this.ReferenceHeight;
    }

    void Update()
    {
        if (_player != null)
        {
            Vector2 aimAxis = _playerController.AimAxis;

            if (PauseController.IsPaused() || aimAxis == Vector2.zero)
            {
                _rectTransform.position = new Vector3(-99999, -99999, _rectTransform.position.z);
            }
            else
            {
                Vector2 screenPos = (Vector2)Camera.main.WorldToScreenPoint(_playerController.transform.position) + aimAxis * _aimingImpactTotal;
                _rectTransform.position = new Vector3(screenPos.x, screenPos.y, _rectTransform.position.z);
            }
        }
    }

    /**
     * Private
     */
    private SessionPlayer _player;
    private RectTransform _rectTransform;
    private PlayerController _playerController;
    private float _aimingImpactTotal;

    private void playerSpawned(LocalEventNotifier.Event e)
    {
        PlayerSpawnedEvent playerSpawnedEvent = e as PlayerSpawnedEvent;
        if (playerSpawnedEvent.PlayerIndex == this.PlayerIndex)
        {
            SessionPlayer p = DynamicData.GetSessionPlayer(this.PlayerIndex);
            if (p.HasJoined && ReInput.players.GetPlayer(p.RewiredId).controllers.joystickCount > 0)
            {
                _player = p;
                _playerController = playerSpawnedEvent.PlayerObject.GetComponent<PlayerController>();
                _playerController.SetUsingController();
            }
        }
    }

    private void playerDied(LocalEventNotifier.Event e)
    {
        if ((e as PlayerDiedEvent).PlayerIndex == this.PlayerIndex)
        {
            _rectTransform.position = new Vector3(-99999, -99999, _rectTransform.position.z);
            _player = null;
            _playerController = null;
        }
    }
}
