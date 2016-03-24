using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class PersistentData
{
    public const string DATA_ROOT = "Data/";
    public const string BOSSLOCKS_DATA_PATH = DATA_ROOT + "bosslocks.xml";

    public static BossLockData BossLockData { get {
            if (_bosslockData == null) _bosslockData = BossLockData.Load(Path.Combine(Application.streamingAssetsPath, BOSSLOCKS_DATA_PATH));
            return _bosslockData;
    } }

    public static int[] GetCurrentBosses()
    {
        if (_currentBosses == null)
            _currentBosses = BossLockData.GenerateBossesForPlaythrough();
        return _currentBosses;
    }

    public static void WipeData()
    {
        _bosslockData = null;
        _currentBosses = null;
    }

    /**
     * Private
     */
    private static BossLockData _bosslockData;
    private static int[] _currentBosses;
}
