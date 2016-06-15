using UnityEngine;
using Rewired;

public class PlayerShopHandler : MonoBehaviour
{
    public bool HasReadied = false;
    public int PlayerIndex = 0;

    void Awake()
    {
        regenSmartSlots(null);
    }

    void Start()
    {
        _sessionPlayer = DynamicData.GetSessionPlayer(this.PlayerIndex);
        if (!_sessionPlayer.HasJoined)
            this.HasReadied = true;
        else
            _rewiredPlayer = ReInput.players.GetPlayer(_sessionPlayer.RewiredId);
        GlobalEvents.Notifier.Listen(PlayerPointsReceivedEvent.NAME, this, regenSmartSlots);
    }

    void Update()
    {
        if (_sessionPlayer.HasJoined)
        {
            if (_rewiredPlayer.GetButtonDown(MenuInput.PAUSE) || _rewiredPlayer.GetButtonDown(MenuInput.EXIT))
            {
                this.HasReadied = !this.HasReadied;
            }
        }
    }

    void LateUpdate()
    {
        if (!_sessionPlayer.HasJoined)
            Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        if (GlobalEvents.Notifier != null)
            GlobalEvents.Notifier.RemoveAllListenersForOwner(this);
    }

    public ProgressData.SmartSlot[] GetSmartSlots()
    {
        return _smartSlots;
    }

    /**
     * Private
     */
    private SessionPlayer _sessionPlayer;
    private Player _rewiredPlayer;
    private ProgressData.SmartSlot[] _smartSlots;

    private void regenSmartSlots(LocalEventNotifier.Event e)
    {
        _smartSlots = ProgressData.GetSmartSlots(this.PlayerIndex);
    }
}
