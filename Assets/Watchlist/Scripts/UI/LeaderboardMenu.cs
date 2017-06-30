using UnityEngine;
using UnityEngine.UI;

public class LeaderboardMenu : MonoBehaviour
{
    public string SoloText;
    public string CoopText;
    public string NoSteamText;
    public Text TitleText;
    public GameObject LeaderboardPanel;
    public GameObject LoadingPanel;
    public LeaderboardListEntry PlayerEntry;
    public LeaderboardListEntry[] List;
    public LeaderboardManager.LeaderboardType CurrentType;

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
    }

    void Update()
    {
        if (MenuInput.NavRight() || MenuInput.NavLeft())
        {
            _prevType = this.CurrentType;
            this.CurrentType = this.CurrentType == LeaderboardManager.LeaderboardType.Solo ? LeaderboardManager.LeaderboardType.Coop : LeaderboardManager.LeaderboardType.Solo;
            this.DisplayLeaderboard();
        }
    }

    public void DisplayLeaderboard()
    {
        if (_loading)
            removeCallback();

        this.TitleText.text = this.CurrentType == LeaderboardManager.LeaderboardType.Solo ? this.SoloText : this.CoopText;

        if (LeaderboardAccessor.LeaderboardFinished(this.CurrentType))
            configure();
        else
            addCallback();
    }

    /**
     * Private
     */
    private bool _loading;
    private LeaderboardManager.LeaderboardType _prevType;

    private void configure()
    {
        _loading = false;
        this.LeaderboardPanel.SetActive(true);
        this.LoadingPanel.SetActive(false);
        LeaderboardManager.LeaderboardEntry[] entries = LeaderboardAccessor.GetEntries(this.CurrentType);

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

        this.PlayerEntry.ConfigureForEntry(LeaderboardAccessor.GetPlayerEntry(this.CurrentType));
    }

    private void addCallback()
    {
        _loading = true;
        this.LeaderboardPanel.SetActive(false);
        this.LoadingPanel.SetActive(true);
        switch (this.CurrentType)
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
