using UnityEngine;

public class LeaderboardStarter : MonoBehaviour
{
    void Start()
    {
        if (LeaderboardAccessor.Instance != null && SteamData.Initialized)
            LeaderboardAccessor.BeginGatheringData();
    }
}
