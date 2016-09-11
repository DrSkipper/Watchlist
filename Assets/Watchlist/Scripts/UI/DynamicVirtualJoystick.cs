using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// Dynamically places joystick at center of touch within valid region, nub follows finger drag until release
public class DynamicVirtualJoystick : VoBehavior
{
    public RectTransform ActiveArea; // Valid area to center joystick on touches
    public Graphic ActiveAreaGraphic; // Fuckin' unity
    public RectTransform JoystickRect; // Should have explicit positioning and width/height
    public RectTransform NubRect; // Should have explict positioning
    public float DeadZone;
    public Vector2 JoystickAxis; // Exposed for debugging

    public delegate void JoystickUpdated(DynamicVirtualJoystick joystick);

    void Start()
    {
        if (Input.touchSupported)
            Input.simulateMouseWithTouches = false;
        _maxX = this.JoystickRect.sizeDelta.x / 2.0f;
        _maxY = this.JoystickRect.sizeDelta.y / 2.0f;
    }

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

            if (_updateCallbacks != null)
            {
                for (int i = 0; i < _updateCallbacks.Count; ++i)
                {
                    _updateCallbacks[i](this);
                }
            }
        }
    }

    public void AddUpdateCallback(JoystickUpdated callback)
    {
        if (_updateCallbacks == null)
            _updateCallbacks = new List<JoystickUpdated>();
        _updateCallbacks.Add(callback);
    }

    public void RemoveUpdateCallback(JoystickUpdated callback)
    {
        if (_updateCallbacks != null)
            _updateCallbacks.Remove(callback);
    }

    /**
     * Private
     */
    private bool _activeTouch;
    private int _fingerId;
    private Vector2 _mostRecentTouchPos;
    private List<JoystickUpdated> _updateCallbacks;
    private float _maxX;
    private float _maxY;

    private void aquireTouch(Touch[] touches)
    {
        for (int i = 0; i < touches.Length; ++i)
        {
            Vector2 touchPos = touches[i].position;
            Vector2 uiPos;

            if (touches[i].phase == TouchPhase.Began &&
            this.ActiveAreaGraphic.Raycast(touchPos, Camera.main))
            {
                bool hitPlane = RectTransformUtility.ScreenPointToLocalPointInRectangle(this.ActiveArea, touchPos, Camera.main, out uiPos);
                //uiPos = this.ActiveArea.worldToLocalMatrix.MultiplyPoint(uiPos);
                if (hitPlane && this.ActiveArea.rect.Contains(uiPos))
                {
                    _activeTouch = true;
                    _fingerId = touches[i].fingerId;
                    _mostRecentTouchPos = uiPos;
                    break;
                }
            }
        }
#if UNITY_EDITOR
        if (!Input.touchSupported && Input.GetMouseButtonDown(0))
        {
            Vector2 uiPos;
            Vector2 touchPos = Input.mousePosition;
            if (this.ActiveAreaGraphic.Raycast(touchPos, Camera.main))
            {
                bool hitPlane = RectTransformUtility.ScreenPointToLocalPointInRectangle(this.ActiveArea, touchPos, Camera.main, out uiPos);
                //uiPos = this.ActiveArea.worldToLocalMatrix.MultiplyPoint(uiPos);
                if (hitPlane && this.ActiveArea.rect.Contains(uiPos))
                {
                    _activeTouch = true;
                    _fingerId = -1;
                    _mostRecentTouchPos = uiPos;
                }
            }
        }
#endif
    }

    private void updateTouch(Touch[] touches)
    {
        bool found = false;
        for (int i = 0; i < touches.Length; ++i)
        {
            if (touches[i].fingerId == _fingerId)
            {
                found = true;
                Vector2 touchPos = touches[i].position;
                Vector2 uiPos;

                bool hitPlane = RectTransformUtility.ScreenPointToLocalPointInRectangle(this.ActiveArea, touchPos, Camera.main, out uiPos);
                if (hitPlane)
                {
                    //uiPos = this.ActiveArea.worldToLocalMatrix.MultiplyPoint(uiPos);
                    _mostRecentTouchPos = uiPos;
                    Vector2 pos = uiPos - (Vector2)this.JoystickRect.localPosition;
                    float dist = pos.magnitude;
                    if (dist > _maxX)
                        pos = pos.normalized * _maxX;

                    this.NubRect.localPosition = new Vector3(pos.x, pos.y, this.NubRect.localPosition.z);

                    this.JoystickAxis = new Vector2(Mathf.Sign(pos.x) * Mathf.Max(1.0f, Mathf.Abs(pos.x) / _maxX), Mathf.Sign(pos.y) * Mathf.Max(1.0f, Mathf.Abs(pos.y) / _maxY));

                    if (this.JoystickAxis.magnitude < this.DeadZone)
                        this.JoystickAxis = Vector2.zero;
                }
                break;
            }
        }

#if UNITY_EDITOR
        if (!Input.touchSupported && Input.GetMouseButton(0))
        {
            found = true;
            Vector2 uiPos;
            Vector2 touchPos = Input.mousePosition;

            bool hitPlane = RectTransformUtility.ScreenPointToLocalPointInRectangle(this.ActiveArea, touchPos, Camera.main, out uiPos);
            if (hitPlane)
            {
                //uiPos = this.ActiveArea.worldToLocalMatrix.MultiplyPoint(uiPos);
                _mostRecentTouchPos = uiPos;
                Vector2 pos = uiPos - (Vector2)this.JoystickRect.localPosition;
                float dist = pos.magnitude;
                if (dist > _maxX)
                    pos = pos.normalized * _maxX;

                this.NubRect.localPosition = new Vector3(pos.x, pos.y, this.NubRect.localPosition.z);

                this.JoystickAxis = new Vector2(Mathf.Sign(pos.x) * Mathf.Max(1.0f, Mathf.Abs(pos.x) / _maxX), Mathf.Sign(pos.y) * Mathf.Max(1.0f, Mathf.Abs(pos.y) / _maxY));

                if (this.JoystickAxis.magnitude < this.DeadZone)
                    this.JoystickAxis = Vector2.zero;
            }
        }
#endif

        if (!found)
        {
            _activeTouch = false;
        }
    }

    private void centerJoystick()
    {
        this.JoystickRect.localPosition = new Vector3(_mostRecentTouchPos.x, _mostRecentTouchPos.y, this.JoystickRect.localPosition.z);
        this.NubRect.localPosition = new Vector3(0, 0, this.NubRect.localPosition.z);
        this.localNotifier.SendEvent(new JoystickCenteredEvent(this));
    }

    private void hideJoystick()
    {
        this.NubRect.localPosition = new Vector3(0, 0, this.NubRect.localPosition.z);
        this.JoystickAxis = Vector2.zero;
        this.localNotifier.SendEvent(new JoystickReleasedEvent(this));
    }
}
