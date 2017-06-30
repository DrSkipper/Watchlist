using UnityEngine;
using System.Collections.Generic;

public static class PersistentData
{
    public const string DATA_PATH = "persistent.dat";

    public static int[] GenerateBossesForPlaythrough()
    {
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

            if (possibilities.Count >= chosen.Length - i)
                possibilities.Remove(chosen[i]);
        }
        return chosen;
    }

    public static bool CheckForLockedBoss(int bossId)
    {
        LoadFromDisk();
        return _bossLockStatuses[bossId];
    }

    public static void UnlockBoss(int bossId)
    {
        LoadFromDisk();
        _bossLockStatuses[bossId] = false;
        if (!_bossUnlocksNeedingNotification.Contains(bossId))
            _bossUnlocksNeedingNotification.Add(bossId);
        SaveToDisk();
    }

    public static int[] BossUnlocksNeedingNotification()
    {
        LoadFromDisk();
        return _bossUnlocksNeedingNotification.ToArray();
    }

    public static void BossUnlockNotified(int bossId)
    {
        LoadFromDisk();
        _bossUnlocksNeedingNotification.Remove(bossId);
        SaveToDisk();

        if (allBossesUnlocked)
            SteamData.UnlockCompleteWatchlistAchievement();
    }

    public static void SaveToDisk()
    {
        if (_bossLockStatuses == null)
            getInitialBossLockData();
        PersistentDiskData diskData = new PersistentDiskData();
        diskData.DataSaved = true;
        diskData.BossLockStatuses = _bossLockStatuses;
        diskData.BossUnlocksNeedingNotification = _bossUnlocksNeedingNotification;
        diskData.LevelsBeatenByNumPlayers = _levelsBeatenByNumPlayers;
        diskData.BossesBeaten = _bossesBeaten;
        diskData.TimesBeat4CornerBosses = _timesBeat4CornerBosses;
        diskData.TimesAcceptedMaster = _timesAcceptedMaster;
        diskData.TimesRefusedMaster = _timesRefusedMaster;
        diskData.TimesDefeatedMaster = _timesDefeatedMaster;
        diskData.TimesClearedMap = _timesClearedMap;
        diskData.HighScoreSinglePlayer = _highScoreSinglePlayer;
        diskData.HighScoreCoop = _highScoreCoop;
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

            _levelsBeatenByNumPlayers = diskData.LevelsBeatenByNumPlayers;
            _bossesBeaten = diskData.BossesBeaten;
            _timesBeat4CornerBosses = diskData.TimesBeat4CornerBosses;
            _timesAcceptedMaster = diskData.TimesAcceptedMaster;
            _timesRefusedMaster = diskData.TimesRefusedMaster;
            _timesDefeatedMaster = diskData.TimesDefeatedMaster;
            _timesClearedMap = diskData.TimesClearedMap;
            _highScoreSinglePlayer = diskData.HighScoreSinglePlayer;
            _highScoreCoop = diskData.HighScoreCoop;

            if (_levelsBeatenByNumPlayers == null)
                _levelsBeatenByNumPlayers = new int[DynamicData.MAX_PLAYERS];
        }
    }

    public static void EraseLocalData()
    {
        getInitialBossLockData();
        _levelsBeatenByNumPlayers = new int[DynamicData.MAX_PLAYERS];
        _bossesBeaten = 0;
        _timesBeat4CornerBosses = 0;
        _timesAcceptedMaster = 0;
        _timesRefusedMaster = 0;
        _timesDefeatedMaster = 0;
        _timesClearedMap = 0;
        _highScoreSinglePlayer = 0;
        _highScoreCoop = 0;
        SaveToDisk();
    }

    public static int GetSinglePlayerHighScore()
    {
        return _highScoreSinglePlayer;
    }

    public static int GetCoopHighScore()
    {
        return _highScoreCoop;
    }

    public static void RecordHighScore()
    {
        int score = 0;
        int numPlayers = 0;
        for (int i = 0; i < DynamicData.MAX_PLAYERS; ++i)
        {
            SessionPlayer p = DynamicData.GetSessionPlayer(i);
            if (p.HasJoined)
            {
                ++numPlayers;
                score += ProgressData.GetPointsForPlayer(i);
            }
        }

        numPlayers = Mathf.Max(ProgressData.MostPlayersUsed, numPlayers);

        if (numPlayers > 1)
        {
            if (score > _highScoreCoop)
                OverwriteHighScoreCoop(score);
        }
        else
        {
            if (score > _highScoreSinglePlayer)
                OverwriteHighScoreSolo(score);
        }
    }

    //NOTE: Use carefully! i.e. only when syncing leaderboard
    public static void OverwriteHighScoreSolo(int score)
    {
        Debug.Log("Overwriting High Score Solo!! " + score);
        _highScoreSinglePlayer = score;
    }

    public static void OverwriteHighScoreCoop(int score)
    {
        Debug.Log("Overwriting High Score Coop!! " + score);
        _highScoreCoop = score;
    }

    public static void RegisterLevelBeaten(int numPlayers)
    {
        LoadFromDisk();
        _levelsBeatenByNumPlayers[numPlayers - 1]++;
        checkLoversUnlock();
        checkCommunalUnlock();
    }

    public static void RegisterBossBeaten(int bossId)
    {
        LoadFromDisk();
        ++_bossesBeaten;
        Debug.Log("Attempting boss unlock achievement for boss id: " + bossId);
        if (StaticData.BossData.BossTypes.ContainsKey(bossId))
            SteamData.UnlockBossAchievement(StaticData.BossData.BossTypes[bossId].Name);
    }

    public static void Register4CornerBossesBeaten()
    {
        LoadFromDisk();
        ++_timesBeat4CornerBosses;
        checkElderUnlock();
    }

    public static void RegisterAcceptedMaster()
    {
        LoadFromDisk();
        ++_timesAcceptedMaster;
        SteamData.UnlockMasterChoiceAchievement(true);
    }

    public static void RegisterRefusedMaster()
    {
        LoadFromDisk();
        ++_timesRefusedMaster;
        SteamData.UnlockMasterChoiceAchievement(false);
    }

    public static void RegisterDefeatedMaster()
    {
        LoadFromDisk();
        ++_timesDefeatedMaster;
        SteamData.UnlockMasterDefeatedAchievement();
    }

    public static void RegisterClearedMap()
    {
        LoadFromDisk();
        ++_timesClearedMap;
        checkEarthUnlock();
    }

    [System.Serializable]
    public struct PersistentDiskData
    {
        public bool DataSaved;
        public Dictionary<int, bool> BossLockStatuses;
        public List<int> BossUnlocksNeedingNotification;
        public int[] LevelsBeatenByNumPlayers;
        public int BossesBeaten;
        public int TimesBeat4CornerBosses;
        public int TimesAcceptedMaster;
        public int TimesRefusedMaster;
        public int TimesDefeatedMaster;
        public int TimesClearedMap;
        public int HighScoreSinglePlayer;
        public int HighScoreCoop;
    }

    /**
     * Private
     */
    private static Dictionary<int, bool> _bossLockStatuses;
    private static List<int> _bossUnlocksNeedingNotification;
    private static int[] _levelsBeatenByNumPlayers;
    private static int _bossesBeaten;
    private static int _timesBeat4CornerBosses;
    private static int _timesAcceptedMaster;
    private static int _timesRefusedMaster;
    private static int _timesDefeatedMaster;
    private static int _timesClearedMap;
    private static int _highScoreSinglePlayer;
    private static int _highScoreCoop;
    private static bool _hasLoaded = false;

    private const int BOSS_LOVERS = 1000001;
    private const int BOSS_COMMUNAL = 1000003;
    private const int BOSS_EARTH = 1000006;
    private const int BOSS_ELDER = 1000008;

    private static bool allBossesUnlocked
    {
        get
        {
            for (int i = 0; i < _bossLockStatuses.Count; ++i)
            {
                if (_bossLockStatuses[i])
                    return false;
            }
            return true;
        }
    }

    private static void getInitialBossLockData()
    {
        _bossLockStatuses = new Dictionary<int, bool>();
        _bossUnlocksNeedingNotification = new List<int>();
        foreach (int key in StaticData.BossLockData.BossLocks.Keys)
        {
            _bossLockStatuses[key] = StaticData.BossLockData.BossLocks[key];
        }
    }

    private static void guaranteeAllBossesPresent()
    {
        if (_bossLockStatuses == null)
            _bossLockStatuses = new Dictionary<int, bool>();
        if (_bossUnlocksNeedingNotification == null)
            _bossUnlocksNeedingNotification = new List<int>();
        foreach (int key in StaticData.BossLockData.BossLocks.Keys)
        {
            if (!_bossLockStatuses.ContainsKey(key))
                _bossLockStatuses[key] = StaticData.BossLockData.BossLocks[key];
        }
    }

    private static void checkLoversUnlock()
    {
        if (_bossLockStatuses[BOSS_LOVERS])
        {
            if (_levelsBeatenByNumPlayers[1] + _levelsBeatenByNumPlayers[2] + _levelsBeatenByNumPlayers[3] >= 3)
            {
                UnlockBoss(BOSS_LOVERS);
            }
        }
    }

    private static void checkCommunalUnlock()
    {
        if (_bossLockStatuses[BOSS_COMMUNAL])
        {
            if (_levelsBeatenByNumPlayers[2] + _levelsBeatenByNumPlayers[3] > 2)
            {
                UnlockBoss(BOSS_COMMUNAL);
            }
        }
    }

    private static void checkEarthUnlock()
    {
        if (_bossLockStatuses[BOSS_EARTH])
        {
            if (_timesClearedMap >= 1)
            {
                UnlockBoss(BOSS_EARTH);
            }
        }
    }

    private static void checkElderUnlock()
    {
        if (_bossLockStatuses[BOSS_ELDER])
        {
            if (_timesBeat4CornerBosses >= 1)
            {
                UnlockBoss(BOSS_ELDER);
            }
        }
    }
}
