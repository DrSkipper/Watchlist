using UnityEngine;
using System.Collections.Generic;
using Rewired;

public static class DynamicData
{
    public const int MAX_PLAYERS = 4;

    public static SessionPlayer GetSessionPlayer(int playerIndex)
    {
        SessionPlayer player = _sessionPlayers[playerIndex];
        if (player == null)
        {
            player = new SessionPlayer(playerIndex);
            _sessionPlayers[playerIndex] = player;
        }
        return player;
    }

    public static SessionPlayer GetSessionPlayerByRewiredId(int rewiredId)
    {
        foreach (SessionPlayer p in _sessionPlayers)
        {
            if (p != null && p.HasJoined && p.RewiredId == rewiredId)
                return p;
        }
        return null;
    }
    
    /**
     * Private
     */
    private static SessionPlayer[] _sessionPlayers = new SessionPlayer[MAX_PLAYERS];
}
