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
    public const string CANCEL = "Cancel";
    public const string MENU_CATEGORY = "Default";

    public static bool SelectCurrentElement(int playerIndex = -1)
    {
        return checkButton(SELECT, playerIndex);
    }

    public static bool NavLeft(int playerIndex = -1)
    {
        return checkButton(LEFT, playerIndex);
    }

    public static bool NavRight(int playerIndex = -1)
    {
        return checkButton(RIGHT, playerIndex);
    }

    public static bool NavUp(int playerIndex = -1)
    {
        return checkButton(UP, playerIndex);
    }

    public static bool NavDown(int playerIndex = -1)
    {
        return checkButton(DOWN, playerIndex);
    }

    public static bool Pause(int playerIndex = -1)
    {
        return checkButton(PAUSE, playerIndex);
    }

    public static bool Exit(int playerIndex = -1)
    {
        return checkButton(EXIT, playerIndex);
    }

    public static bool Cancel(int playerIndex = -1)
    {
        return checkButton(CANCEL, playerIndex);
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

    public static bool ControllerUsed()
    {
        IList<Player> players = ReInput.players.GetPlayers();
        for (int i = 0; i < players.Count; ++i)
        {
            Player player = players[i];
            if (player.controllers.joystickCount > 0 && player.GetAnyButton())
                return true;
        }
        return false;
    }

    /**
     * Private
     */

    private static bool checkButton(string buttonName, int playerIndex)
    {
        if (playerIndex < 0)
        {
            foreach (Player player in ReInput.players.GetPlayers())
            {
                if (player.GetButtonDown(buttonName))
                    return true;
            }
            return false;
        }

        SessionPlayer sessionPlayer = DynamicData.GetSessionPlayer(playerIndex);
        if (sessionPlayer == null || !sessionPlayer.HasJoined)
            return false;

        return ReInput.players.GetPlayer(sessionPlayer.RewiredId).GetButtonDown(buttonName);
    }
}
