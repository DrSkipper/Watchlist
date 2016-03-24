using UnityEngine;
using System.Collections.Generic;
using Rewired;

public static class DynamicData
{
    public const int WEAPON_SLOTS = 4;
    public const int MAX_PLAYERS = 4;

    public static IntegerVector[] CompletedTiles { get { return _completedTiles.ToArray(); } }
    public static IntegerVector MostRecentTile { get { return _mostRecentTile.HasValue ? _mostRecentTile.Value : IntegerVector.Zero; } }

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

    public static void CompleteTile(IntegerVector tile)
    {
        if (!_completedTiles.Contains(tile))
            _completedTiles.Add(tile);
        _mostRecentTile = tile;
    }

    public static void SelectTile(IntegerVector tile)
    {
        _mostRecentTile = tile;
    }

    public static void UpdatePlayer(int playerIndex, WeaponData.Slot[] slots)
    {
        _weaponSlotsByPlayer[playerIndex] = slots;
    }

    public static void ResetData()
    {
        _completedTiles = new List<IntegerVector>();
        _mostRecentTile = null;
        _weaponSlotsByPlayer = new Dictionary<int, WeaponData.Slot[]>();
    }

    public static int GetCurrentDifficulty()
    {
        int radius = Mathf.Max(Mathf.Abs(DynamicData.MostRecentTile.X), Mathf.Abs(DynamicData.MostRecentTile.Y));
        if (radius <= 1)
            return 0; // Easy
        if (radius >= 3)
            return 2; // Hard
        return 1; // Medium
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
    private static List<IntegerVector> _completedTiles = new List<IntegerVector>();
    private static IntegerVector? _mostRecentTile = null;
    private static Dictionary<int, WeaponData.Slot[]> _weaponSlotsByPlayer = new Dictionary<int, WeaponData.Slot[]>();
    private static SessionPlayer[] _sessionPlayers = new SessionPlayer[MAX_PLAYERS];
}
