using UnityEngine;

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

    public static LeaderboardManager.LeaderboardEntry[] GetEntries(LeaderboardManager.LeaderboardType type)
    {
        return _instance.getEntries(type);
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

    }

    private void onCoopComplete()
    {

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
