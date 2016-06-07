using UnityEngine;
using Rewired;
using System.Collections.Generic;

public static class MenuInput
{
    public const string UP = "MenuUp";
    public const string DOWN = "MenuDown";
    public const string LEFT = "MenuLeft";
    public const string RIGHT = "MenuRight";
    public const string SELECT = "Confirm";
    public const string PAUSE = "Pause";
    public const string EXIT = "Exit";
    public const string MENU_CATEGORY = "Default";

    public static bool SelectCurrentElement()
    {
        return checkButton(SELECT);
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

    private static bool checkButton(string buttonName)
    {
        foreach (Player player in ReInput.players.GetPlayers())
        {
            if (player.GetButtonDown(buttonName))
                return true;
        }
        return false;
    }
}
