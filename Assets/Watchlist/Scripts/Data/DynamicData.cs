using UnityEngine;
using System.Collections.Generic;
using Rewired;

public static class DynamicData
{
    public const int WEAPON_SLOTS = 4;
    public const int MAX_PLAYERS = 4;

    public static WeaponData.Slot[][] WeaponSlotsByPlayer
    {
        get
        {
            WeaponData.Slot[][] playerSlots = new WeaponData.Slot[_weaponSlotsByPlayer.Count][];
            foreach (int player in _weaponSlotsByPlayer.Keys)
            {
                playerSlots[player] = _weaponSlotsByPlayer[player];
            }
            return playerSlots;
        }
    }

    public static void UpdatePlayer(int playerIndex, WeaponData.Slot[] slots)
    {
        _weaponSlotsByPlayer[playerIndex] = slots;
    }

    public static void ResetData()
    {
        _weaponSlotsByPlayer = new Dictionary<int, WeaponData.Slot[]>();
    }

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
    private static Dictionary<int, WeaponData.Slot[]> _weaponSlotsByPlayer = new Dictionary<int, WeaponData.Slot[]>();
    private static SessionPlayer[] _sessionPlayers = new SessionPlayer[MAX_PLAYERS];
}
