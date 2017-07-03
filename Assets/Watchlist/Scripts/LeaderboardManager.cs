using UnityEngine;
using System.Collections.Generic;
using Steamworks;

public class LeaderboardManager : MonoBehaviour
{
    public const string SOLO_LEADERBOARD = "Solo High Score";
    public const string COOP_LEADERBOARD = "Co-op High Score";
    public const int LEADERBOARD_DETAILS_NUM = 0;

    public delegate void DataGatheredDelegate();

    public bool Finished { get; private set; }
    public LeaderboardEntry[] Leaderboard { get { return _topPlayers != null ? _topPlayers.ToArray() : new LeaderboardEntry[0]; } }
    public LeaderboardEntry[] FriendsLeaderboard { get { return _friends != null ? _friends.ToArray() : new LeaderboardEntry[0]; } }
    public LeaderboardEntry PlayerEntry { get { return _playerEntry; } }
    public LeaderboardType Type { get; private set; }
    public DataGatherStep CurrentStep { get; private set; }
    private DataGatheredDelegate DataGatheredCallback;

    public enum LeaderboardType
    {
        Solo,
        Coop
    }

    public enum DataGatherStep
    {
        NotStarted = 0,
        FindLeaderboard,
        DownloadPlayerScore,
        UpdatePlayerScore,
        GatherLeaderboardEntries,
        CompileLeaderboardEntries,
        GatherNonFriendNames,
        Finished
    }

    public class LeaderboardEntry
    {
        public string PlayerName;
        public CSteamID Id;
        public int Rank;
        public int Score;
        public bool IsFriend;
        public bool IsUser;
        public bool Unranked;
        public bool Stranger { get { return !this.IsUser && !this.IsFriend; } }
    }

    public void BeginGatheringData(LeaderboardType leaderboardType, DataGatheredDelegate callback)
    {
        this.Finished = false;
        _topPlayersReceived = false;
        _friendsReceived = false;
        this.CurrentStep = DataGatherStep.NotStarted;
        this.Type = leaderboardType;
        this.DataGatheredCallback = callback;

        if (!SteamData.Initialized)
            Finish();
        else
            NextStep();
    }

    /**
     * Steps
     */
    void NextStep()
    {
        this.CurrentStep = (DataGatherStep)((int)this.CurrentStep + 1);
        switch (this.CurrentStep)
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
            case DataGatherStep.GatherNonFriendNames:
                GatherNonFriendNames();
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
        int localHighScore = this.Type == LeaderboardType.Solo ? PersistentData.GetSinglePlayerHighScore() : PersistentData.GetCoopHighScore();
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
        {
            if (localHighScore > 0)
                uploadPlayerScore(localHighScore);
            else
            {
                _playerEntry = getUnrankedPlayerEntry();
                NextStep();
            }
        }
        else
        {
            NextStep();
        }
    }

    void GatherLeaderboardEntries()
    {
        gatherLeaderboardEntries();
        gatherFriendsEntries();
    }

    void CompileLeaderboardEntries()
    {
        _topPlayers = new List<LeaderboardEntry>();
        _nonFriends = new List<LeaderboardEntry>();
        _friends = new List<LeaderboardEntry>();

        for (int i = 0; i < _topPlayerCount; ++i)
        {
            LeaderboardEntry entry = getLeaderboardEntry(_topPlayerEntries, i);
            _topPlayers.Add(entry);

            if (entry.Stranger)
                _nonFriends.Add(entry);
        }

        for (int i = 0; i < Mathf.Min(_friendsCount, 10); ++i)
        {
            _friends.Add(getLeaderboardEntry(_friendsEntries, i));
        }

        NextStep();
    }

    void GatherNonFriendNames()
    {
        _personaStateChangeCallback = Callback<PersonaStateChange_t>.Create(onPersonaStateChange);

        for (int i = 0; i < _nonFriends.Count;)
        {
            if (SteamFriends.RequestUserInformation(_nonFriends[i].Id, true))
                ++i;
            else
                _nonFriends.RemoveAt(i);
        }

        if (_nonFriends.Count == 0)
            NextStep();
    }

    void Finish()
    {
        this.Finished = true;
        if (this.DataGatheredCallback != null)
            this.DataGatheredCallback();
    }

    /**
     * Private
     */
    private CallResult<LeaderboardScoresDownloaded_t> _playerScoreResult;
    private CallResult<LeaderboardFindResult_t> _leaderboardResult;
    private CallResult<LeaderboardScoreUploaded_t> _playerScoreUploadResult;
    private CallResult<LeaderboardScoresDownloaded_t> _topPlayersResult;
    private CallResult<LeaderboardScoresDownloaded_t> _friendsResult;
    private Callback<PersonaStateChange_t> _personaStateChangeCallback;
    private SteamLeaderboard_t _leaderboard;
    private LeaderboardScoresDownloaded_t _playerScore;
    private int _topPlayerCount;
    private int _friendsCount;
    private bool _topPlayersReceived;
    private bool _friendsReceived;
    private SteamLeaderboardEntries_t _topPlayerEntries;
    private SteamLeaderboardEntries_t _friendsEntries;
    private List<LeaderboardEntry> _topPlayers;
    private List<LeaderboardEntry> _friends;
    private List<LeaderboardEntry> _nonFriends;
    private LeaderboardEntry _playerEntry;

    private const string SOLO = "solo";
    private const string COOP = "coop";
    private string _typeString
    {
        get
        {
            return this.Type == LeaderboardType.Solo ? SOLO : COOP;
        }
    }

    private string _leaderboardName
    {
        get
        {
            return this.Type == LeaderboardType.Solo ? SOLO_LEADERBOARD : COOP_LEADERBOARD;
        }
    }

    private void findLeaderboard()
    {
        _leaderboardResult = CallResult<LeaderboardFindResult_t>.Create(onLeaderboardFound);
        SteamAPICall_t call = SteamUserStats.FindLeaderboard(_leaderboardName);
        _leaderboardResult.Set(call);
    }

    private void downloadPlayerScore()
    {
        _playerScoreResult = CallResult<LeaderboardScoresDownloaded_t>.Create(onPlayerScoreRetrieved);
        SteamAPICall_t scoreCall = SteamUserStats.DownloadLeaderboardEntriesForUsers(_leaderboard, new CSteamID[] { SteamUser.GetSteamID() }, 1);
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
        retVal.Unranked = false;
        retVal.Id = entry.m_steamIDUser;

        if (retVal.IsUser)
            retVal.PlayerName = SteamData.UserDisplayName;
        else if (retVal.IsFriend)
            retVal.PlayerName = SteamFriends.GetFriendPersonaName(entry.m_steamIDUser);
        else
        {
            //SteamFriends.RequestUserInformation(entry.m_steamIDUser, true);
            retVal.PlayerName = entry.m_steamIDUser.ToString();
        }

        return retVal;
    }

    private LeaderboardEntry getUnrankedPlayerEntry()
    {
        LeaderboardEntry retVal = new LeaderboardEntry();
        retVal.IsUser = true;
        retVal.IsFriend = false;
        retVal.Rank = -1;
        retVal.Unranked = true;
        retVal.Score = 0;
        retVal.PlayerName = SteamData.UserDisplayName;
        retVal.Id = SteamData.UserSteamId;
        return retVal;
    }

    private void uploadPlayerScore(int score)
    {
        int[] details = new int[LEADERBOARD_DETAILS_NUM];
        _playerScoreUploadResult = CallResult<LeaderboardScoreUploaded_t>.Create(onScoreUploaded);
        SteamAPICall_t call = SteamUserStats.UploadLeaderboardScore(_leaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, score, details, LEADERBOARD_DETAILS_NUM);
        _playerScoreUploadResult.Set(call);
    }

    private void updateLocalPlayerScore(int score)
    {
        if (this.Type == LeaderboardType.Solo)
            PersistentData.OverwriteHighScoreSolo(score);
        else
            PersistentData.OverwriteHighScoreCoop(score);
        PersistentData.SaveToDisk();
    }

    private void gatherLeaderboardEntries()
    {
        _topPlayersResult = CallResult<LeaderboardScoresDownloaded_t>.Create(onLeaderboardEntriesDownloaded);
        SteamAPICall_t call = SteamUserStats.DownloadLeaderboardEntries(_leaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 0, 9);
        _topPlayersResult.Set(call);
    }

    private void gatherFriendsEntries()
    {
        _friendsResult = CallResult<LeaderboardScoresDownloaded_t>.Create(onFriendsEntriesDownloaded);
        SteamAPICall_t call = SteamUserStats.DownloadLeaderboardEntries(_leaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestFriends, 0, 9);
        _friendsResult.Set(call);
    }

    /**
     * Async Callbacks
     */
    private void onPlayerScoreRetrieved(LeaderboardScoresDownloaded_t result, bool failure)
    {
        if (failure)
        {
            Debug.LogWarning("Error finding player's highscore: " + _typeString);
            Finish();
            return;
        }
        else
        {
            _playerScore = result;
            if (result.m_cEntryCount > 0)
                _playerEntry = getLeaderboardEntry(_playerScore.m_hSteamLeaderboardEntries, 0);
            NextStep();
        }
    }
    
    private void onScoreUploaded(LeaderboardScoreUploaded_t result, bool failure)
    {
        if (failure)
        {
            Debug.LogWarning("Error uploading player's score: " + _typeString);
            Finish();
            return;
        }

        downloadPlayerScore();
    }

    private void onLeaderboardFound(LeaderboardFindResult_t result, bool failure)
    {
        if (failure)
        {
            Debug.LogWarning("Error finding leaderboard: " + _typeString);
            Finish();
            return;
        }
        else if (result.m_bLeaderboardFound != 1)
        {
            Debug.LogWarning("Could not find leaderboard: " + _typeString);
            Finish();
            return;
        }

        _leaderboard = result.m_hSteamLeaderboard;
        NextStep();
    }

    private void onLeaderboardEntriesDownloaded(LeaderboardScoresDownloaded_t result, bool failure)
    {
        if (failure)
        {
            Debug.LogWarning("Error downloading top players: " + _typeString);
            Finish();
            return;
        }

        _topPlayerCount = result.m_cEntryCount;
        _topPlayerEntries = result.m_hSteamLeaderboardEntries;
        _topPlayersReceived = true;
        checkTopPlayersAndFriendsDownloaded();
    }

    private void onFriendsEntriesDownloaded(LeaderboardScoresDownloaded_t result, bool failure)
    {
        if (failure)
        {
            Debug.LogWarning("Error downloading friends entries: " + _typeString);
            Finish();
            return;
        }

        _friendsCount = result.m_cEntryCount;
        _friendsEntries = result.m_hSteamLeaderboardEntries;
        _friendsReceived = true;
        checkTopPlayersAndFriendsDownloaded();
    }

    private void checkTopPlayersAndFriendsDownloaded()
    {
        if (_topPlayersReceived && _friendsReceived)
            NextStep();
    }

    private void onPersonaStateChange(PersonaStateChange_t result)
    {
        for (int i = 0; i < _nonFriends.Count; ++i)
        {
            if (_nonFriends[i].Id.m_SteamID == result.m_ulSteamID)
            {
                _nonFriends[i].PlayerName = SteamFriends.GetFriendPersonaName(_nonFriends[i].Id);
                _nonFriends.RemoveAt(i);
                break;
            }
        }

        if (_nonFriends.Count == 0)
            NextStep();
    }
}
