using UnityEngine;
using Rewired;

public static class MenuInput
{
    public const string UP = "MenuUp";
    public const string DOWN = "MenuDown";
    public const string LEFT = "MenuLeft";
    public const string RIGHT = "MenuRight";
    public const string SELECT = "Confirm";
    public const string PAUSE = "Pause";
    public const string EXIT = "Exit";
    public const string MENU_PLAYER = "MenuPlayer";

    public static bool SelectCurrentElement(bool onlyCheckPlayingPlayers = false)
    {
        return checkButton(SELECT, onlyCheckPlayingPlayers);
    }

    public static bool HighlightNextElement(bool onlyCheckPlayingPlayers = false)
    {
        return NavDown(onlyCheckPlayingPlayers);
    }

    public static bool HighlightPreviousElement(bool onlyCheckPlayingPlayers = false)
    {
        return NavUp(onlyCheckPlayingPlayers);
    }

    public static bool NavLeft(bool onlyCheckPlayingPlayers = false)
    {
        return checkButton(LEFT, onlyCheckPlayingPlayers);
    }

    public static bool NavRight(bool onlyCheckPlayingPlayers = false)
    {
        return checkButton(RIGHT, onlyCheckPlayingPlayers);
    }

    public static bool NavUp(bool onlyCheckPlayingPlayers = false)
    {
        return checkButton(UP, onlyCheckPlayingPlayers);
    }

    public static bool NavDown(bool onlyCheckPlayingPlayers = false)
    {
        return checkButton(DOWN, onlyCheckPlayingPlayers);
    }

    public static bool Pause(bool onlyCheckPlayingPlayers = false)
    {
        return checkButton(PAUSE, onlyCheckPlayingPlayers);
    }

    public static bool Exit(bool onlyCheckPlayingPlayers = false)
    {
        return checkButton(EXIT, onlyCheckPlayingPlayers);
    }

    public static bool AnyInput(bool onlyCheckPlayingPlayers = false)
    {
        foreach (Player player in ReInput.players.GetPlayers())
        {
            if (!onlyCheckPlayingPlayers || player.isPlaying)
            {
                if (player.GetAnyButtonDown())
                    return true;
            }
        }
        return false;
    }

    /**
     * Private
     */
    private static bool checkButton(string buttonName, bool onlyCheckPlayingPlayers)
    {
        if (!onlyCheckPlayingPlayers)
            return ReInput.players.GetPlayer(MENU_PLAYER).GetButtonDown(buttonName);

        foreach (Player player in ReInput.players.GetPlayers())
        {
            if (player.name != MENU_PLAYER && player.isPlaying && player.GetButtonDown(buttonName))
                return true;
        }
        return false;
    }
}
