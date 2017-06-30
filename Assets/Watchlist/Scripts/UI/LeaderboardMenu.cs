using UnityEngine;
using UnityEngine.UI;

public class LeaderboardMenu : MonoBehaviour
{
    public string SoloFriendsText;
    public string CoopFriendsText;
    public string SoloGlobalText;
    public string CoopGlobalText;
    public string NoSteamText;
    public Text TitleText;
    public GameObject LeaderboardPanel;
    public GameObject LoadingPanel;
    public LeaderboardListEntry PlayerEntry;
    public LeaderboardListEntry[] List;
    public LeaderboardDisplayType CurrentDisplayType;
    public const string EXIT_EVENT = "scene_exit";

    public enum LeaderboardDisplayType
    {
        SoloFriends = 0,
        CoopFriends,
        SoloGlobal,
        CoopGlobal
    }
    private const int LEADERBOARD_DISPLAY_TYPE_COUNT = 4;

    void Start()
    {
        if (LeaderboardAccessor.Instance != null && SteamData.Initialized)
            this.DisplayLeaderboard();
        else
        {
            this.LeaderboardPanel.SetActive(false);
            this.TitleText.gameObject.SetActive(false);
            this.LoadingPanel.SetActive(true);
            this.LoadingPanel.GetComponent<Text>().text = this.NoSteamText;
            this.enabled = false;
        }

        GlobalEvents.Notifier.Listen(EXIT_EVENT, this, onSceneExit);
    }

    void Update()
    {
        bool right = MenuInput.NavRight();
        bool left = MenuInput.NavLeft();
        if (right || left)
        {
            _prevType = _currentType;
            int displayType = (int)this.CurrentDisplayType;
            if (right)
                displayType = displayType >= LEADERBOARD_DISPLAY_TYPE_COUNT - 1 ? 0 : displayType + 1;
            else
                displayType = displayType <= 0 ? LEADERBOARD_DISPLAY_TYPE_COUNT - 1 : displayType - 1;
            this.CurrentDisplayType = (LeaderboardDisplayType)displayType;
            alignTypeAndFilter();
            this.DisplayLeaderboard();
        }
    }

    public void DisplayLeaderboard()
    {
        if (_loading)
            removeCallback();

        setTitleText();

        if (LeaderboardAccessor.LeaderboardFinished(_currentType))
            configure();
        else
            addCallback();
    }

    /**
     * Private
     */
    private LeaderboardManager.LeaderboardType _currentType;
    private LeaderboardAccessor.LeaderboardFilter _currentFilter;
    private bool _loading;
    private LeaderboardManager.LeaderboardType _prevType;

    private void onSceneExit(LocalEventNotifier.Event e)
    {
        if (_loading)
            removeCallback();
    }

    private void alignTypeAndFilter()
    {
        switch (this.CurrentDisplayType)
        {
            default:
            case LeaderboardDisplayType.SoloFriends:
            case LeaderboardDisplayType.CoopFriends:
                _currentFilter = LeaderboardAccessor.LeaderboardFilter.Friends;
                break;
            case LeaderboardDisplayType.SoloGlobal:
            case LeaderboardDisplayType.CoopGlobal:
                _currentFilter = LeaderboardAccessor.LeaderboardFilter.Global;
                break;
        }

        switch (this.CurrentDisplayType)
        {
            default:
            case LeaderboardDisplayType.SoloFriends:
            case LeaderboardDisplayType.SoloGlobal:
                _currentType = LeaderboardManager.LeaderboardType.Solo;
                break;
            case LeaderboardDisplayType.CoopFriends:
            case LeaderboardDisplayType.CoopGlobal:
                _currentType = LeaderboardManager.LeaderboardType.Coop;
                break;
        }
    }

    private void setTitleText()
    {
        switch (this.CurrentDisplayType)
        {
            default:
            case LeaderboardDisplayType.SoloFriends:
                this.TitleText.text = this.SoloFriendsText;
                break;
            case LeaderboardDisplayType.CoopFriends:
                this.TitleText.text = this.CoopFriendsText;
                break;
            case LeaderboardDisplayType.SoloGlobal:
                this.TitleText.text = this.SoloGlobalText;
                break;
            case LeaderboardDisplayType.CoopGlobal:
                this.TitleText.text = this.CoopGlobalText;
                break;
        }
    }

    private void configure()
    {
        _loading = false;
        this.LeaderboardPanel.SetActive(true);
        this.LoadingPanel.SetActive(false);
        LeaderboardManager.LeaderboardEntry[] entries = LeaderboardAccessor.GetEntries(_currentType);

        for (int i = 0; i < this.List.Length; ++i)
        {
            if (i < entries.Length)
            {
                this.List[i].gameObject.SetActive(true);
                this.List[i].ConfigureForEntry(entries[i]);
            }
            else
            {
                this.List[i].gameObject.SetActive(false);
            }
        }

        this.PlayerEntry.ConfigureForEntry(LeaderboardAccessor.GetPlayerEntry(_currentType));
    }

    private void addCallback()
    {
        _loading = true;
        this.LeaderboardPanel.SetActive(false);
        this.LoadingPanel.SetActive(true);
        switch (_currentType)
        {
            default:
            case LeaderboardManager.LeaderboardType.Solo:
                LeaderboardAccessor.Instance.SoloCallback += configure;
                break;
            case LeaderboardManager.LeaderboardType.Coop:
                LeaderboardAccessor.Instance.CoopCallback += configure;
                break;
        }
    }

    private void removeCallback()
    {
        _loading = false;
        switch (_prevType)
        {
            default:
            case LeaderboardManager.LeaderboardType.Solo:
                LeaderboardAccessor.Instance.SoloCallback -= configure;
                break;
            case LeaderboardManager.LeaderboardType.Coop:
                LeaderboardAccessor.Instance.CoopCallback -= configure;
                break;
        }
    }
}
