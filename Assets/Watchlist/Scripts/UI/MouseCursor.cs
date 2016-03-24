using UnityEngine;
using Rewired;

public class MouseCursor : MonoBehaviour
{
    void Start()
    {
        _rectTransform = this.GetComponent<RectTransform>();
        _rectTransform.position = new Vector3(-99999, -99999, _rectTransform.position.z);
    }

    void Update()
    {
        if (!_setup)
        {
            for (int i = 0; i < DynamicData.MAX_PLAYERS; ++i)
            {
                SessionPlayer p = DynamicData.GetSessionPlayer(i);
                if (p.HasJoined)
                {
                    if (ReInput.players.GetPlayer(p.RewiredId).controllers.hasMouse)
                    {
                        _player = p;
                        break;
                    }
                }
            }
            _setup = true;
        }
        
        if (PauseController.IsPaused())
        {
            _rectTransform.position = new Vector3(-99999, -99999, _rectTransform.position.z);
        }

        else if (_player != null)
        {
            Player rewiredP = ReInput.players.GetPlayer(_player.RewiredId);
            Vector2 screenPos = rewiredP.controllers.Mouse.screenPosition;
            _rectTransform.position = new Vector3(screenPos.x, screenPos.y, _rectTransform.position.z);
        }
    }

    /**
     * Private
     */
    private bool _setup;
    private SessionPlayer _player;
    private RectTransform _rectTransform;
}
