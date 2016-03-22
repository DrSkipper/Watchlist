using UnityEngine;
using Rewired;

public static class MenuInput
{
    private const string VERTICAL = "MoveVertical";
    private const string HORIZONTAL = "MoveHorizontal";
    private const string SELECT = "Confirm";
    private const string PAUSE = "Pause";
    private const string EXIT = "Exit";
    private const float DEADZONE = 0.4f;
    private const float AXIS_CONSUMPTION_TIME = 0.2f;

    public static bool AnyInput()
    {
        foreach (Player player in ReInput.players.GetPlayers())
        {
            if (player.GetAnyButtonDown() || Mathf.Abs(player.GetAxis(VERTICAL)) > DEADZONE || Mathf.Abs(player.GetAxis(HORIZONTAL)) > DEADZONE)
                return true;
        }
        return false;
    }

    public static bool HighlightNextElement()
    {
        return checkAxis(VERTICAL, -1);
    }

    public static bool HighlightPreviousElement()
    {
        return checkAxis(VERTICAL, 1);
    }

    public static bool SelectCurrentElement()
    {
        return checkButton(SELECT);
    }

    public static bool NavLeft()
    {
        return checkAxis(HORIZONTAL, -1);
    }

    public static bool NavRight()
    {
        return checkAxis(HORIZONTAL, 1);
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
        return checkButton(PAUSE);
    }

    public static bool Exit()
    {
        return checkButton(EXIT);
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

    private static bool checkAxis(string axisName, int sign)
    {
        if (axesAvailable())
        {
            foreach (Player player in ReInput.players.GetPlayers())
            {
                if (sign * player.GetAxis(axisName) > 0.4f && ConsumeAxisEvent())
                    return true;
            }
        }
        return false;
    }

    private static bool checkButton(string buttonName)
    {
        foreach (Player player in ReInput.players.GetPlayers())
        {
            if (player.GetButton(buttonName))
                return true;
        }
        return false;
    }
}
