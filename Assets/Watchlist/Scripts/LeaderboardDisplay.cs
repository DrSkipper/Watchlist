using UnityEngine;
using System.Collections.Generic;
using Steamworks;

public class LeaderboardDisplay : MonoBehaviour
{
    public const string SOLO_LEADERBOARD = "Solo High Score";
    public const string COOP_LEADERBOARD = "Co-op High Score";
    public const int LEADERBOARD_DETAILS_NUM = 0;

    public bool Finished { get; private set; }
    public LeaderboardEntry[] SoloLeaderboard { get { return _soloTopPlayers.ToArray(); } }
    private DataGatherStep _currentStep = DataGatherStep.NotStarted;
    
    public enum DataGatherStep
    {
        NotStarted = 0,
        FindLeaderboard,
        DownloadPlayerScore,
        UpdatePlayerScore,
        GatherLeaderboardEntries,
        CompileLeaderboardEntries,
        Finished
    }

    public struct LeaderboardEntry
    {
        public string PlayerName;
        public int Rank;
        public int Score;
        public bool IsFriend;
        public bool IsUser;
    }

    void Start()
    {
        if (!SteamData.Initialized)
        {
            Finish();
            return;
        }

        NextStep();
    }

    void NextStep()
    {
        _currentStep = (DataGatherStep)((int)_currentStep + 1);
        switch (_currentStep)
        {
            case DataGatherStep.FindLeaderboard:
                FindLeaderboard();
                break;
            case DataGatherStep.DownloadPlayerScore:
                DownloadPlayerScore();
                break;
            case DataGatherStep.UpdatePlayerScore:
                UpdatePlayerScore();
                break;
            case DataGatherStep.GatherLeaderboardEntries:
                GatherLeaderboardEntries();
                break;
            case DataGatherStep.CompileLeaderboardEntries:
                CompileLeaderboardEntries();
                break;
            default:
            case DataGatherStep.Finished:
                Finish();
                break;
        }
    }

    void FindLeaderboard()
    {
        findLeaderboard();
    }

    void DownloadPlayerScore()
    {
        downloadPlayerScore();
    }

    void UpdatePlayerScore()
    {
        bool needsUpload = true;
        int localHighScore = PersistentData.GetSinglePlayerHighScore();
        if (_playerScore.m_cEntryCount != 0)
        {
            int playerScore = _playerEntry.Score;
            Debug.Log("Player score in leaderboard: " + playerScore);
            if (playerScore >= localHighScore)
            {
                needsUpload = false;
                if (playerScore > localHighScore)
                {
                    Debug.Log("Overwriting high score based on server value. prev: " + localHighScore);
                    updateLocalPlayerScore(playerScore);
                }
            }
        }

        if (needsUpload)
            uploadPlayerScore(localHighScore);
        else
            NextStep();
    }

    void GatherLeaderboardEntries()
    {
        gatherLeaderboardEntries();
    }

    void CompileLeaderboardEntries()
    {
        _soloTopPlayers = new List<LeaderboardEntry>();
        for (int i = 0; i < _soloTopPlayerCount; ++i)
        {
            _soloTopPlayers.Add(getLeaderboardEntry(_solotTopPlayerEntries, i));
        }
        NextStep();
    }

    void Finish()
    {
        this.Finished = true;
    }

    /**
     * Private
     */
    private CallResult<LeaderboardScoresDownloaded_t> _playerScoreResult;
    private CallResult<LeaderboardFindResult_t> _soloLeaderboardResult;
    private CallResult<LeaderboardScoreUploaded_t> _playerScoreUploadResult;
    private CallResult<LeaderboardScoresDownloaded_t> _soloTopPlayersResult;
    private SteamLeaderboard_t _soloLeaderboard;
    private LeaderboardScoresDownloaded_t _playerScore;
    private int _soloTopPlayerCount;
    private SteamLeaderboardEntries_t _solotTopPlayerEntries;
    private List<LeaderboardEntry> _soloTopPlayers;
    private LeaderboardEntry _playerEntry;

    private void findLeaderboard()
    {
        _soloLeaderboardResult = CallResult<LeaderboardFindResult_t>.Create(onSoloLeaderboardFound);
        SteamAPICall_t soloCall = SteamUserStats.FindLeaderboard(SOLO_LEADERBOARD);
        _soloLeaderboardResult.Set(soloCall);
    }

    private void downloadPlayerScore()
    {
        _playerScoreResult = CallResult<LeaderboardScoresDownloaded_t>.Create(onPlayerScoreRetrieved);
        SteamAPICall_t scoreCall = SteamUserStats.DownloadLeaderboardEntriesForUsers(_soloLeaderboard, new CSteamID[] { SteamUser.GetSteamID() }, 1);
        _playerScoreResult.Set(scoreCall);
    }

    private LeaderboardEntry getLeaderboardEntry(SteamLeaderboardEntries_t entries, int index)
    {
        LeaderboardEntry_t entry;
        int[] playerDetails = new int[LEADERBOARD_DETAILS_NUM];
        SteamUserStats.GetDownloadedLeaderboardEntry(entries, index, out entry, playerDetails, LEADERBOARD_DETAILS_NUM);

        LeaderboardEntry retVal = new LeaderboardEntry();
        retVal.IsUser = entry.m_steamIDUser == SteamData.UserSteamId;
        retVal.IsFriend = !retVal.IsUser && SteamFriends.HasFriend(entry.m_steamIDUser, EFriendFlags.k_EFriendFlagAll);
        retVal.Rank = entry.m_nGlobalRank;
        retVal.Score = entry.m_nScore;

        if (retVal.IsUser)
            retVal.PlayerName = SteamData.UserDisplayName;
        else if (retVal.IsFriend)
            retVal.PlayerName = SteamFriends.GetFriendPersonaName(entry.m_steamIDUser);
        else
            retVal.PlayerName = entry.m_steamIDUser.ToString();

        return retVal;
    }

    private void uploadPlayerScore(int score)
    {
        int[] details = new int[LEADERBOARD_DETAILS_NUM];
        _playerScoreUploadResult = CallResult<LeaderboardScoreUploaded_t>.Create(onSoloScoreUploaded);
        SteamAPICall_t call = SteamUserStats.UploadLeaderboardScore(_soloLeaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, score, details, LEADERBOARD_DETAILS_NUM);
        _playerScoreUploadResult.Set(call);
    }

    private void updateLocalPlayerScore(int score)
    {
        PersistentData.OverwriteHighScoreSolo(score);
        PersistentData.SaveToDisk();
    }

    private void gatherLeaderboardEntries()
    {
        _soloTopPlayersResult = CallResult<LeaderboardScoresDownloaded_t>.Create(onLeaderboardEntriesDownloaded);
        SteamAPICall_t call = SteamUserStats.DownloadLeaderboardEntries(_soloLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 0, 9);
        _soloTopPlayersResult.Set(call);
    }

    private void onPlayerScoreRetrieved(LeaderboardScoresDownloaded_t result, bool failure)
    {
        if (failure)
        {
            Debug.LogWarning("Error finding player's highscore");
            Finish();
            return;
        }
        else
        {
            _playerScore = result;
            _playerEntry = getLeaderboardEntry(_playerScore.m_hSteamLeaderboardEntries, 0);
            NextStep();
        }
    }
    
    private void onSoloScoreUploaded(LeaderboardScoreUploaded_t result, bool failure)
    {
        if (failure)
        {
            Debug.LogWarning("Error uploading player's solo score");
            Finish();
            return;
        }

        downloadPlayerScore();
    }

    private void onSoloLeaderboardFound(LeaderboardFindResult_t result, bool failure)
    {
        if (failure)
        {
            Debug.LogWarning("Error finding solo leaderboard");
            Finish();
            return;
        }
        else if (result.m_bLeaderboardFound != 1)
        {
            Debug.LogWarning("Could not find solo leaderboard");
            Finish();
            return;
        }

        _soloLeaderboard = result.m_hSteamLeaderboard;
        NextStep();
    }

    private void onLeaderboardEntriesDownloaded(LeaderboardScoresDownloaded_t result, bool failure)
    {
        if (failure)
        {
            Debug.LogWarning("Error downloading top players");
            Finish();
            return;
        }

        _soloTopPlayerCount = result.m_cEntryCount;
        _solotTopPlayerEntries = result.m_hSteamLeaderboardEntries;
        NextStep();
    }
}
