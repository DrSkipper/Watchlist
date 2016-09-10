using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicVirtualJoystick : VoBehavior
{
    public RectTransform ActiveArea; // Valid area to center joystick on touches
    public RectTransform JoystickRect; // Should have explicit positioning and width/height
    public RectTransform NubRect; // Should have explict positioning
    public float DeadZone;
    public Vector2 JoystickAxis; // Exposed for debugging

    void Update()
    {
        Touch[] touches = Input.touches;

        if (!_activeTouch)
        {
            aquireTouch(touches);

            if (_activeTouch)
                centerJoystick();
        }
        else
        {
            updateTouch(touches);

            if (!_activeTouch)
                hideJoystick();
        }
    }

    /**
     * Private
     */
    private bool _activeTouch;
    private int _fingerId;
    private Vector2 _mostRecentTouchPos;

    private void aquireTouch(Touch[] touches)
    {
        for (int i = 0; i < touches.Length; ++i)
        {
            Vector2 touchPos = touches[i].position;
            Vector2 uiPos;

            bool hitPlane = RectTransformUtility.ScreenPointToLocalPointInRectangle(this.ActiveArea, touchPos, Camera.main, out uiPos);
            if (hitPlane && this.ActiveArea.rect.Contains(uiPos))
            {
                _activeTouch = true;
                _fingerId = touches[i].fingerId;
                _mostRecentTouchPos = uiPos;
                break;
            }
        }
    }

    private void updateTouch(Touch[] touches)
    {
        bool found = false;
        for (int i = 0; i < touches.Length; ++i)
        {
            if (touches[i].fingerId == _fingerId)
            {
                Vector2 touchPos = touches[i].position;
                Vector2 uiPos;

                bool hitPlane = RectTransformUtility.ScreenPointToLocalPointInRectangle(this.ActiveArea, touchPos, Camera.main, out uiPos);
                if (hitPlane)
                {
                    found = true;
                    Vector2 pos = _mostRecentTouchPos;
                    float dist = pos.magnitude;
                    if (dist > this.JoystickRect.sizeDelta.x)
                    {
                        pos = _mostRecentTouchPos.normalized * this.JoystickRect.sizeDelta.x;
                    }
                    this.NubRect.localPosition = new Vector3(pos.x, pos.y, this.NubRect.localPosition.z);
                    _mostRecentTouchPos = uiPos;

                    this.JoystickAxis = new Vector2(Mathf.Sign(pos.x) * Mathf.Max(1.0f, Mathf.Abs(pos.x) / this.JoystickRect.sizeDelta.x), Mathf.Sign(pos.y) * Mathf.Max(1.0f, Mathf.Abs(pos.y) / this.JoystickRect.sizeDelta.y));
                    if (this.JoystickAxis.magnitude < this.DeadZone)
                        this.JoystickAxis = Vector2.zero;
                    //TODO: Send JoystickUpdated event
                    break;
                }
            }
        }

        if (!found)
        {
            _activeTouch = false;
        }
    }

    private void centerJoystick()
    {
        this.JoystickRect.localPosition = new Vector3(_mostRecentTouchPos.x, _mostRecentTouchPos.y, this.JoystickRect.localPosition.z);
        this.NubRect.localPosition = new Vector3(0, 0, this.NubRect.localPosition.z);
        //TODO: Send JoystickCentered event
    }

    private void hideJoystick()
    {
        //TODO: Send JoystickHide event
    }
}
