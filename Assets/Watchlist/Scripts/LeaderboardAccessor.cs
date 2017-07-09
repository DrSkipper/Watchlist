using UnityEngine;
using Steamworks;

public class LeaderboardAccessor : MonoBehaviour
{
    public LeaderboardManager SoloManager;
    public LeaderboardManager CoopManager;
    public LeaderboardManager.DataGatheredDelegate SoloCallback;
    public LeaderboardManager.DataGatheredDelegate CoopCallback;

    public enum LeaderboardFilter
    {
        Friends,
        Global
    }

    void Start()
    {
        _instance = this;
    }

    public static void BeginGatheringData()
    {
        _instance.beginGatheringData();
    }

    public static LeaderboardManager.LeaderboardEntry[] GetEntries(LeaderboardManager.LeaderboardType type, LeaderboardFilter filter)
    {
        switch (filter)
        {
            default:
            case LeaderboardFilter.Global:
                return _instance.getEntries(type);
            case LeaderboardFilter.Friends:
                return _instance.getFriendEntries(type);
        }
    }

    public static bool LeaderboardFinished(LeaderboardManager.LeaderboardType type)
    {
        return _instance.leadeboardFinished(type);
    }

    public static LeaderboardAccessor Instance
    {
        get
        {
            return _instance;
        }
    }

    public static LeaderboardManager.LeaderboardEntry GetPlayerEntry(LeaderboardManager.LeaderboardType type)
    {
        return _instance.getPlayerEntry(type);
    }

    /**
     * Private
     */
    private static LeaderboardAccessor _instance;

    private void beginGatheringData()
    {
        this.SoloManager.BeginGatheringData(LeaderboardManager.LeaderboardType.Solo, onSoloComplete);
        this.CoopManager.BeginGatheringData(LeaderboardManager.LeaderboardType.Coop, onCoopComplete);
    }

    private void onSoloComplete()
    {
        if (this.SoloCallback != null)
            this.SoloCallback();
    }

    private void onCoopComplete()
    {
        if (this.CoopCallback != null)
            this.CoopCallback();
    }

    private LeaderboardManager.LeaderboardEntry[] getEntries(LeaderboardManager.LeaderboardType type)
    {
        switch (type)
        {
            default:
            case LeaderboardManager.LeaderboardType.Solo:
                return this.SoloManager.Leaderboard;
            case LeaderboardManager.LeaderboardType.Coop:
                return this.CoopManager.Leaderboard;
        }
    }

    private LeaderboardManager.LeaderboardEntry[] getFriendEntries(LeaderboardManager.LeaderboardType type)
    {
        switch (type)
        {
            default:
            case LeaderboardManager.LeaderboardType.Solo:
                return this.SoloManager.FriendsLeaderboard;
            case LeaderboardManager.LeaderboardType.Coop:
                return this.CoopManager.FriendsLeaderboard;
        }
    }

    private bool leadeboardFinished(LeaderboardManager.LeaderboardType type)
    {
        switch (type)
        {
            default:
            case LeaderboardManager.LeaderboardType.Solo:
                return this.SoloManager.Finished;
            case LeaderboardManager.LeaderboardType.Coop:
                return this.CoopManager.Finished;
        }
    }

    private LeaderboardManager.LeaderboardEntry getPlayerEntry(LeaderboardManager.LeaderboardType type)
    {
        switch (type)
        {
            default:
            case LeaderboardManager.LeaderboardType.Solo:
                return this.SoloManager.PlayerEntry;
            case LeaderboardManager.LeaderboardType.Coop:
                return this.CoopManager.PlayerEntry;
        }
    }
}
