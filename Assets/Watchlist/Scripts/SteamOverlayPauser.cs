using UnityEngine;
using Steamworks;

public class SteamOverlayPauser : MonoBehaviour
{
    void Start()
    {
        if (SteamData.Initialized)
        {
            _overlayCallback = Callback<GameOverlayActivated_t>.Create(onSteamOverlay);
        }
    }

    private void OnDestroy()
    {
        _overlayCallback.Dispose();
    }

    private Callback<GameOverlayActivated_t> _overlayCallback;

    private void onSteamOverlay(GameOverlayActivated_t callback)
    {
        if (callback.m_bActive != 0 && !PauseController.IsPaused())
            PauseController.Pause(true);
    }
}
