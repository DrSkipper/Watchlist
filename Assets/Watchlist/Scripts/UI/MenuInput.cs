using UnityEngine;

public static class MenuInput
{
    public const float MENU_AXIS_DEADZONE = 0.4f;
    private const float AXIS_CONSUMPTION_TIME = 0.2f;

    public static bool HighlightNextElement()
    {
        return Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || ((Input.GetAxis("Vertical") < -MENU_AXIS_DEADZONE) && ConsumeAxisEvent());
    }

    public static bool HighlightPreviousElement()
    {
        return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || ((Input.GetAxis("Vertical") > MENU_AXIS_DEADZONE) && ConsumeAxisEvent());
    }

    public static bool SelectCurrentElement()
    {
        return Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Select");
    }

    public static bool NavLeft()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || (Input.GetAxis("Horizontal") < -MENU_AXIS_DEADZONE && ConsumeAxisEvent());
    }

    public static bool NavRight()
    {
        return Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || (Input.GetAxis("Horizontal") > MENU_AXIS_DEADZONE && ConsumeAxisEvent());
    }

    public static bool NavUp()
    {
        return HighlightPreviousElement();
    }

    public static bool NavDown()
    {
        return HighlightNextElement();
    }

    public static bool Pause()
    {
        return Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Pause");
    }

    public static bool Exit()
    {
        return Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("Exit");
    }

    public static bool ConsumeAxisEvent()
    {
        bool available = axesAvailable();
        if (available)
            _consumedAxisTime = Time.unscaledTime;
        return available;
    }

    /**
     * Private
     */
    private static float _consumedAxisTime;

    private static bool axesAvailable()
    {
        return _consumedAxisTime == 0.0f || Time.unscaledTime - _consumedAxisTime > AXIS_CONSUMPTION_TIME;
    }
}
