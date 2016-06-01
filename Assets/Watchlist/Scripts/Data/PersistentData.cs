using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class PersistentData
{
    public const string DATA_PATH = "persistent.dat";
    
    public static int[] GenerateBossesForPlaythrough()
    {
        if (_bossLockStatuses == null)
            LoadFromDisk();
        List<int> possibilities = new List<int>();

        foreach (int bossId in _bossLockStatuses.Keys)
        {
            if (!_bossLockStatuses[bossId])
                possibilities.Add(bossId);
        }

        int[] chosen = new int[4];
        for (int i = 0; i < chosen.Length; ++i)
        {
            chosen[i] = possibilities[Random.Range(0, possibilities.Count)];

            if (possibilities.Count > chosen.Length)
                possibilities.Remove(chosen[i]);
        }
        return chosen;
    }

    public static bool CheckForLockedBoss(int bossId)
    {
        if (_bossLockStatuses == null)
            LoadFromDisk();
        return _bossLockStatuses[bossId];
    }

    public static void SaveToDisk()
    {
        if (_bossLockStatuses == null)
            getInitialBossLockData();
        PersistentDiskData diskData = new PersistentDiskData();
        diskData.DataSaved = true;
        diskData.BossLockStatuses = _bossLockStatuses;
        DiskDataHandler.Save(DATA_PATH, diskData);
    }

    public static void LoadFromDisk()
    {
        if (!_hasLoaded)
        {
            _hasLoaded = true;
            PersistentDiskData diskData = DiskDataHandler.Load<PersistentDiskData>(DATA_PATH);
            if (diskData.DataSaved)
            {
                _bossLockStatuses = diskData.BossLockStatuses;
                guaranteeAllBossesPresent();
            }
            else
            {
                getInitialBossLockData();
            }
        }
    }

    public static void EraseLocalData()
    {
        getInitialBossLockData();
        SaveToDisk();
    }

    [System.Serializable]
    public struct PersistentDiskData
    {
        public bool DataSaved;
        public Dictionary<int, bool> BossLockStatuses;
    }

    /**
     * Private
     */
    private static Dictionary<int, bool> _bossLockStatuses;
    private static bool _hasLoaded = false;

    private static void getInitialBossLockData()
    {
        _bossLockStatuses = new Dictionary<int, bool>();
        foreach (int key in StaticData.BossLockData.BossLocks.Keys)
        {
            _bossLockStatuses[key] = StaticData.BossLockData.BossLocks[key];
        }
    }

    private static void guaranteeAllBossesPresent()
    {
        foreach (int key in StaticData.BossLockData.BossLocks.Keys)
        {
            if (!_bossLockStatuses.ContainsKey(key))
                _bossLockStatuses[key] = StaticData.BossLockData.BossLocks[key];
        }
    }
}
