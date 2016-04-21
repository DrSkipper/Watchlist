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
    public List<string> OutroPages;
    public string LevelSelectSceneName;

    void Awake()
    {
        _timedCallbacks = this.GetComponent<TimedCallbacks>();
    }

    void Start()
    {
        List<string> allPages = new List<string>();

        allPages.AddRange(this.IntroPages);
        int[] bosses = PersistentData.GetCurrentBosses();

        foreach (int bossId in bosses)
        {
            BossType bossType = StaticData.BossData.BossTypes[bossId];
            allPages.Add(bossType.PageText1);
            if (!this.LimitOnePagePerBoss)
                allPages.Add(bossType.PageText2);
        }

        allPages.AddRange(this.OutroPages);

        foreach (string page in allPages)
        {
            this.PageHandler.AddPage(page);
        }

        this.PageHandler.OnFlippedLastPage = flippedLastPage;
        _timedCallbacks.AddCallback(this, beginPageFlipping, this.IntroDelay);
    }

    void Update()
    {
        if (_introComplete && MenuInput.SelectCurrentElement())
            this.PageHandler.IncrementPage();
    }

    /**
     * Private
     */
    private TimedCallbacks _timedCallbacks;
    private bool _introComplete;

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
