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
    public GameObject ChoiceMenu;

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
        _destination = this.LevelSelectSceneName;
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

        if (this.UseChoices)
        {
            GlobalEvents.Notifier.Listen(MenuElementSelectedEvent.NAME, this, choiceMade);
        }

        this.PageHandler.OnFlippedLastPage = flippedLastPage;
        _timedCallbacks.AddCallback(this, beginPageFlipping, this.IntroDelay);
    }

    void Update()
    {
        if (_introComplete && MenuInput.SelectCurrentElement())
        {
            if (this.UseChoices && _choiceIndex < this.Choices.Count && this.PageHandler.CurrentPage == this.IntroPages.Count - 1)
            {
                if (this.PageHandler.PageDone)
                    this.ChoiceMenu.SetActive(true);
            }
            else
            {
                this.PageHandler.IncrementPage();
                if (this.PageFlipCallback != null && this.PageHandler.CurrentPage < _avatarKeys.Length)
                    this.PageFlipCallback(_avatarKeys[this.PageHandler.CurrentPage] != null ? _avatarKeys[this.PageHandler.CurrentPage] : DEFAULT_AVATAR);
            }
        }
    }

    /**
     * Private
     */
    private TimedCallbacks _timedCallbacks;
    private bool _introComplete;
    private string[] _avatarKeys;
    private int _choiceIndex;
    private string _destination;

    private void beginPageFlipping()
    {
        this.PageHandler.IncrementPage();
        _introComplete = true;
    }

    private void flippedLastPage()
    {
        SceneManager.LoadScene(_destination);
    }

    private void choiceMade(LocalEventNotifier.Event eventObject)
    {
        MenuElementSelectedEvent selectionEvent = eventObject as MenuElementSelectedEvent;

        if (selectionEvent.Action.ToLower() == "choice_a")
        {
            this.PageHandler.AddPage(this.Choices[_choiceIndex].ResultTextA);
            _destination = this.Choices[_choiceIndex].DestinationA;
            ++_choiceIndex;
            this.ChoiceMenu.SetActive(false);
            this.PageHandler.IncrementPage();
        }
        else if (selectionEvent.Action.ToLower() == "choice_b")
        {
            this.PageHandler.AddPage(this.Choices[_choiceIndex].ResultTextB);
            _destination = this.Choices[_choiceIndex].DestinationB;
            ++_choiceIndex;
            this.ChoiceMenu.SetActive(false);
            this.PageHandler.IncrementPage();
        }
    }
}
