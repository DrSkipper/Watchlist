using UnityEngine;
using Steamworks;

public static class SteamData
{
    public const uint AppId = 652760;
    public static bool Initialized { get { return SteamManager.Initialized; } }
    public static string UserDisplayName { get { return Initialized ? SteamFriends.GetPersonaName() : ""; } }
    public static CSteamID UserSteamId { get { return SteamUser.GetSteamID(); } }

    public static void UnlockBossAchievement(string bossName)
    {
        if (Initialized)
        {
            SteamUserStats.SetAchievement(bossName.ToUpper() + BOSS_ACHIEVEMENT_SUFFIX);
            SteamUserStats.StoreStats();
        }
    }

    public static void UnlockMasterDefeatedAchievement()
    {
        if (Initialized)
        {
            SteamUserStats.SetAchievement(MASTER_DEFEATED_ACHIEVEMENT);
            SteamUserStats.StoreStats();
        }
    }

    public static void UnlockMasterChoiceAchievement(bool accepted)
    {
        if (Initialized)
        {
            SteamUserStats.SetAchievement(accepted ? ACCEPT_MASTER_ACHIEVEMENT : REFUSE_MASTER_ACHIEVEMENT);
            SteamUserStats.StoreStats();
        }
    }

    public static void UnlockCompleteWatchlistAchievement()
    {
        if (Initialized)
        {
            SteamUserStats.SetAchievement(COMPLETE_WATCHLIST_ACHIEVEMENT);
            SteamUserStats.StoreStats();
        }
    }

    /**
     * Private
     */
    private const string BOSS_ACHIEVEMENT_SUFFIX = "_DEFEATED";
    private const string MASTER_DEFEATED_ACHIEVEMENT = "MASTER_DEFEATED";
    private const string ACCEPT_MASTER_ACHIEVEMENT = "ACCEPT_MASTER";
    private const string REFUSE_MASTER_ACHIEVEMENT = "REFUSE_MASTER";
    private const string COMPLETE_WATCHLIST_ACHIEVEMENT = "WATCHLIST_COMPLETE";
}
