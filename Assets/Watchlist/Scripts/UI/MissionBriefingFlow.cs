using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[RequireComponent(typeof(TimedCallbacks))]
public class MissionBriefingFlow : VoBehavior
{
    public bool LimitOnePagePerBoss = false;
    public PageHandler PageHandler;
    public float IntroDelay = 2.0f;
    public List<string> IntroPages;
    public List<Choice> Choices;
    public List<string> OutroPages;
    public string LevelSelectSceneName;
    public PageFlipDelegate PageFlipCallback;
    public bool UseBossText = true;
    public bool UseChoices = false;

    public delegate void PageFlipDelegate(string avatarName);
    public const string DEFAULT_AVATAR = "master";

    [System.Serializable]
    public struct Choice
    {
        public string ChoiceTextA;
        public string ChoiceTextB;
        public string ResultTextA;
        public string ResultTextB;
        public string DestinationA;
        public string DestinationB;
    }

    void Awake()
    {
        _timedCallbacks = this.GetComponent<TimedCallbacks>();
    }

    void Start()
    {
        List<string> allPages = new List<string>();

        allPages.AddRange(this.IntroPages);
        int[] bosses = PersistentData.GetCurrentBosses();

        if (this.UseBossText)
        {
            foreach (int bossId in bosses)
            {
                BossType bossType = StaticData.BossData.BossTypes[bossId];
                allPages.Add(bossType.PageText1);
                if (!this.LimitOnePagePerBoss)
                    allPages.Add(bossType.PageText2);
            }
        }

        allPages.AddRange(this.OutroPages);

        foreach (string page in allPages)
        {
            this.PageHandler.AddPage(page);
        }

        _avatarKeys = new string[allPages.Count];

        if (this.UseBossText)
        {
            for (int i = this.IntroPages.Count; i < allPages.Count - this.OutroPages.Count; ++i)
            {
                int bossId = bosses[i - this.IntroPages.Count];
                BossType bossType = StaticData.BossData.BossTypes[bossId];
                _avatarKeys[i] = bossType.SceneKey;
                if (!this.LimitOnePagePerBoss)
                    ++i;
            }
        }

        this.PageHandler.OnFlippedLastPage = flippedLastPage;
        _timedCallbacks.AddCallback(this, beginPageFlipping, this.IntroDelay);
    }

    void Update()
    {
        if (_introComplete && MenuInput.SelectCurrentElement())
        {
            this.PageHandler.IncrementPage();
            if (this.PageFlipCallback != null && this.PageHandler.CurrentPage < _avatarKeys.Length)
                this.PageFlipCallback(_avatarKeys[this.PageHandler.CurrentPage] != null ? _avatarKeys[this.PageHandler.CurrentPage] : DEFAULT_AVATAR);
        }
    }

    /**
     * Private
     */
    private TimedCallbacks _timedCallbacks;
    private bool _introComplete;
    private string[] _avatarKeys;

    private void beginPageFlipping()
    {
        this.PageHandler.IncrementPage();
        _introComplete = true;
    }

    private void flippedLastPage()
    {
        SceneManager.LoadScene(this.LevelSelectSceneName);
    }
}
