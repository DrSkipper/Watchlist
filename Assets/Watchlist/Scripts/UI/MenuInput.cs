using UnityEngine;
using Rewired;

public static class MenuInput
{
    public static bool SelectCurrentElement()
    {
        return checkButton(SELECT);
    }

    public static bool HighlightNextElement()
    {
        return NavDown();
    }

    public static bool HighlightPreviousElement()
    {
        return NavUp();
    }

    public static bool NavLeft()
    {
        return checkButton(LEFT);
    }

    public static bool NavRight()
    {
        return checkButton(RIGHT);
    }

    public static bool NavUp()
    {
        return checkButton(UP);
    }

    public static bool NavDown()
    {
        return checkButton(DOWN);
    }

    public static bool Pause()
    {
        return checkButton(PAUSE);
    }

    public static bool Exit()
    {
        return checkButton(EXIT);
    }

    public static bool AnyInput()
    {
        foreach (Player player in ReInput.players.GetPlayers())
        {
            if (player.GetAnyButtonDown())
                return true;
        }
        return false;
    }

    /**
     * Private
     */
    private const string UP = "MenuUp";
    private const string DOWN = "MenuDown";
    private const string LEFT = "MenuLeft";
    private const string RIGHT = "MenuRight";
    private const string SELECT = "Confirm";
    private const string PAUSE = "Pause";
    private const string EXIT = "Exit";

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
