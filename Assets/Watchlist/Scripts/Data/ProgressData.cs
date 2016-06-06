using UnityEngine;
using System.Collections.Generic;

public static class ProgressData
{
    public const int WEAPON_SLOTS = 4;
    public const string DATA_PATH = "progress.dat";

    public static IntegerVector[] CompletedTiles { get { LoadFromDisk(); return _completedTiles.ToArray(); } }
    public static IntegerVector MostRecentTile { get { LoadFromDisk(); return _mostRecentTile.HasValue ? _mostRecentTile.Value : IntegerVector.Zero; } }
    
    [System.Serializable]
    public class SlotWrapper
    {
        public WeaponData.Slot SlotType;
        public int AmmoRemaining;

        public SlotWrapper(WeaponData.Slot slotType)
        {
            this.SlotType = slotType;
            this.AmmoRemaining = WeaponData.GetSlotDurationsByType()[slotType];
        }

        public SlotWrapper(SlotWrapper other)
        {
            this.SlotType = other.SlotType;
            this.AmmoRemaining = other.AmmoRemaining;
        }
    }

    public static void CompleteTile(IntegerVector tile)
    {
        if (!_completedTiles.Contains(tile))
        {
            _completedTiles.Add(tile);
            PersistentData.RegisterLevelBeaten(DynamicData.NumJoinedPlayers());
            if (IsCornerBoss(tile))
            {
                PersistentData.RegisterBossBeaten();
                if (NumBossesBeaten() == 4)
                    PersistentData.Register4CornerBossesBeaten();
            }

            if (_completedTiles.Count == 44)
                PersistentData.RegisterClearedMap();
        }
        _mostRecentTile = tile;
    }

    public static bool IsCornerBoss(IntegerVector tile)
    {
        return Mathf.Abs(tile.X) == 3 && Mathf.Abs(tile.Y) == 3;
    }

    public static int NumBossesBeaten()
    {
        int num = 0;
        for (int i = 0; i < _completedTiles.Count; ++i)
        {
            if (IsCornerBoss(_completedTiles[i]))
                ++num;
        }
        return num;
    }

    public static void SelectTile(IntegerVector tile)
    {
        _mostRecentTile = tile;
    }

    public static int GetCurrentDifficulty()
    {
        int radius = Mathf.Max(Mathf.Abs(ProgressData.MostRecentTile.X), Mathf.Abs(ProgressData.MostRecentTile.Y));
        if (radius <= 1)
            return 0; // Easy
        if (radius >= 3)
            return 2; // Hard
        return 1; // Medium
    }

    public static SlotWrapper[][] WeaponSlotsByPlayer
    {
        get
        {
            SlotWrapper[][] playerSlots = new SlotWrapper[DynamicData.MAX_PLAYERS][];
            for (int player = 0; player < DynamicData.MAX_PLAYERS; ++player)
            {
                if (_weaponSlotsByPlayer.ContainsKey(player))
                {
                    playerSlots[player] = new SlotWrapper[_weaponSlotsByPlayer[player].Length];
                    for (int i = 0; i < _weaponSlotsByPlayer[player].Length; ++i)
                    {
                        playerSlots[player][i] = new SlotWrapper(_weaponSlotsByPlayer[player][i]);
                    }
                }
                else
                {
                    playerSlots[player] = new SlotWrapper[0];
                }
            }
            return playerSlots;
        }
    }

    public static void UpdatePlayerSlots(int playerIndex, SlotWrapper[] slots)
    {
        if (!_weaponSlotsByPlayer.ContainsKey(playerIndex))
            _weaponSlotsByPlayer.Add(playerIndex, new SlotWrapper[slots.Length]);
        else
            _weaponSlotsByPlayer[playerIndex] = new SlotWrapper[slots.Length];
        for (int i = 0; i < slots.Length; ++i)
        {
            _weaponSlotsByPlayer[playerIndex][i] = new SlotWrapper(slots[i]);
        }
    }
    
    public static int[] GetCurrentBosses()
    {
        if (_currentBosses == null)
            _currentBosses = PersistentData.GenerateBossesForPlaythrough();
        return _currentBosses;
    }

    public static void WipeData()
    {
        _completedTiles = new List<IntegerVector>();
        _weaponSlotsByPlayer = new Dictionary<int, SlotWrapper[]>();
        _mostRecentTile = null;
        _currentBosses = null;
    }

    public static void SaveToDisk()
    {
        ProgressDiskData diskData = new ProgressDiskData();
        diskData.DataSaved = true;
        diskData.CompletedTiles = _completedTiles;
        diskData.HaveUsedMostRecentTile = _mostRecentTile != null;
        diskData.MostRecentTile = _mostRecentTile.HasValue ? _mostRecentTile.Value : new IntegerVector();
        diskData.WeaponSlotsByPlayer = _weaponSlotsByPlayer;
        DiskDataHandler.Save(DATA_PATH, diskData);
    }

    public static void LoadFromDisk()
    {
        if (!_hasLoaded)
        {
            _hasLoaded = true;
            ProgressDiskData diskData = DiskDataHandler.Load<ProgressDiskData>(DATA_PATH);
            if (diskData.DataSaved)
            {
                _completedTiles = diskData.CompletedTiles;
                if (diskData.HaveUsedMostRecentTile)
                    _mostRecentTile = diskData.MostRecentTile;
                _weaponSlotsByPlayer = diskData.WeaponSlotsByPlayer;
            }
        }
    }

    [System.Serializable]
    public struct ProgressDiskData
    {
        public bool DataSaved;
        public List<IntegerVector> CompletedTiles;
        public bool HaveUsedMostRecentTile;
        public IntegerVector MostRecentTile;
        public Dictionary<int, SlotWrapper[]> WeaponSlotsByPlayer;
    }

    /**
     * Private
     */
    private static List<IntegerVector> _completedTiles = new List<IntegerVector>();
    private static IntegerVector? _mostRecentTile = null;
    private static Dictionary<int, SlotWrapper[]> _weaponSlotsByPlayer = new Dictionary<int, SlotWrapper[]>();
    private static int[] _currentBosses;
    private static bool _hasLoaded = false;
}
