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
                if (player.controllers.hasMouse)
                {
                    if (player.controllers.Keyboard.PollForFirstKey().success)
                        return true;
                }
                else
                {
                    if (player.GetAnyButtonDown())
                        return true;
                }
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
            if (player.name != MENU_PLAYER && player.isPlaying)
            {
                if (player.controllers.hasMouse)
                {
                    List<ControllerMap> maps = new List<ControllerMap>(player.controllers.maps.GetMaps(ControllerType.Keyboard, player.controllers.Keyboard.id));
                    ControllerMap map = player.controllers.maps.GetMap(ControllerType.Keyboard, player.controllers.Keyboard.id, 0);
                    foreach (ActionElementMap actionMap in map.ElementMapsWithAction(buttonName))
                    {
                        if (player.controllers.Keyboard.GetKeyDown(actionMap.keyCode))
                            return true;
                    }
                }
                else
                {
                    if (player.GetButtonDown(buttonName))
                        return true;
                }
            }
        }
        return false;
    }
}
