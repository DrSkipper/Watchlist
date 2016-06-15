using UnityEngine;
using System.Collections.Generic;

public static class ProgressData
{
    public const int WEAPON_SLOTS = 4;
    public const int POINTS_FOR_HIT = 1;
    public const int POINTS_FOR_KILL = 3;
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

    public struct SmartSlot
    {
        public WeaponData.Slot SlotType;
        public int Ammo;
        public int Level;

        public SmartSlot(WeaponData.Slot slotType, int ammo, int level)
        {
            this.SlotType = slotType;
            this.Ammo = ammo;
            this.Level = level;
        }
    }

    public static SmartSlot GetSmartSlot(SlotWrapper[] slots, int slotId)
    {
        bool[] weaponTypesFound = { false, false, false, false };
        int[] ammoRemaining = { 0, 0, 0, 0 };
        int[] weaponLevel = { 0, 0, 0, 0 };
        int numTypesFound = 0;
        WeaponData.Slot chosenSlotType = WeaponData.Slot.Empty;
        int chosenWeaponIndex = -1;

        for (int i = 0; i < slots.Length; ++i)
        {
            if (slots[i].SlotType == WeaponData.Slot.Empty)
                continue;
            int weaponIndex = (int)slots[i].SlotType - 1;
            ++weaponLevel[weaponIndex];
            if (chosenSlotType == WeaponData.Slot.Empty && !weaponTypesFound[weaponIndex])
            {
                ++numTypesFound;
                weaponTypesFound[weaponIndex] = true;
                ammoRemaining[weaponIndex] = slots[i].AmmoRemaining;

                if (numTypesFound > slotId)
                {
                    chosenSlotType = slots[i].SlotType;
                    chosenWeaponIndex = weaponIndex;
                }
            }
        }

        int ammo = chosenSlotType != WeaponData.Slot.Empty ? ammoRemaining[chosenWeaponIndex] : 0;
        int level = chosenSlotType != WeaponData.Slot.Empty ? weaponLevel[chosenWeaponIndex] : 0;
        return new SmartSlot(chosenSlotType, ammo, level);
    }

    public static SmartSlot[] GetSmartSlots(int playerIndex)
    {
        SmartSlot[] smartSlots = new SmartSlot[5];
        SlotWrapper[] wrappers = WeaponSlotsByPlayer[playerIndex];
        for (int i = 0; i < 4; ++i)
        {
            SmartSlot smartSlot = GetSmartSlot(wrappers, i);
            if (smartSlot.SlotType != WeaponData.Slot.Empty)
                smartSlots[(int)smartSlot.SlotType] = smartSlot;
            else
                break;
        }
        return smartSlots;
    }

    public static void PickupSlot(int playerIndex, WeaponData.Slot slotType)
    {
        int count = 0;

        List<ProgressData.SlotWrapper> slots = new List<ProgressData.SlotWrapper>(WeaponSlotsByPlayer[playerIndex]);
        foreach (ProgressData.SlotWrapper slot in slots)
        {
            if (slot.SlotType == slotType)
                ++count;
        }

        if (count < WeaponData.GetMaxSlotsByType()[slotType])
        {
            slots.Add(new ProgressData.SlotWrapper(slotType));
        }
        else
        {
            int shots = WeaponData.GetSlotDurationsByType()[slotType];
            int shotsToAdd = shots;

            for (int i = slots.Count - 1; i >= 0; --i)
            {
                ProgressData.SlotWrapper slot = slots[i];
                if (slot.SlotType == slotType)
                {
                    slot.AmmoRemaining += shotsToAdd;
                    if (slot.AmmoRemaining > shots)
                    {
                        shotsToAdd = slot.AmmoRemaining - shots;
                        slot.AmmoRemaining = shots;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        UpdatePlayerSlots(playerIndex, slots.ToArray());
    }
    
    public static int[] GetCurrentBosses()
    {
        if (_currentBosses == null)
            _currentBosses = PersistentData.GenerateBossesForPlaythrough();
        return _currentBosses;
    }

    public static int GetPointsForPlayer(int playerIndex)
    {
        if (_playerPoints == null)
            _playerPoints = new int[DynamicData.MAX_PLAYERS];
        return _playerPoints[playerIndex];
    }

    public static void ApplyPointsDeltaForPlayer(int playerIndex, int delta)
    {
        if (_playerPoints == null)
            _playerPoints = new int[DynamicData.MAX_PLAYERS];
        _playerPoints[playerIndex] += delta;
    }

    public static void WipeData()
    {
        _completedTiles = new List<IntegerVector>();
        _weaponSlotsByPlayer = new Dictionary<int, SlotWrapper[]>();
        _mostRecentTile = null;
        _currentBosses = null;
        _playerPoints = null;
    }

    public static void SaveToDisk()
    {
        ProgressDiskData diskData = new ProgressDiskData();
        diskData.DataSaved = true;
        diskData.CompletedTiles = _completedTiles;
        diskData.HaveUsedMostRecentTile = _mostRecentTile != null;
        diskData.MostRecentTile = _mostRecentTile.HasValue ? _mostRecentTile.Value : new IntegerVector();
        diskData.WeaponSlotsByPlayer = _weaponSlotsByPlayer;
        diskData.CurrentBosses = _currentBosses;
        diskData.PlayerPoints = _playerPoints;
        DiskDataHandler.Save(DATA_PATH, diskData);
    }

    public static void LoadFromDisk(bool force = false)
    {
        if (!_hasLoaded || force)
        {
            _hasLoaded = true;
            ProgressDiskData diskData = DiskDataHandler.Load<ProgressDiskData>(DATA_PATH);
            if (diskData.DataSaved)
            {
                _completedTiles = diskData.CompletedTiles;
                if (diskData.HaveUsedMostRecentTile)
                    _mostRecentTile = diskData.MostRecentTile;
                _weaponSlotsByPlayer = diskData.WeaponSlotsByPlayer;

                _currentBosses = diskData.CurrentBosses;
                if (_currentBosses != null && _currentBosses.Length < 4)
                    _currentBosses = null;

                _playerPoints = diskData.PlayerPoints;
                if (_playerPoints != null && _playerPoints.Length < DynamicData.MAX_PLAYERS)
                    _playerPoints = null;
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
        public int[] CurrentBosses;
        public int[] PlayerPoints;
    }

    /**
     * Private
     */
    private static List<IntegerVector> _completedTiles = new List<IntegerVector>();
    private static IntegerVector? _mostRecentTile = null;
    private static Dictionary<int, SlotWrapper[]> _weaponSlotsByPlayer = new Dictionary<int, SlotWrapper[]>();
    private static int[] _currentBosses;
    private static int[] _playerPoints;
    private static bool _hasLoaded = false;
}
