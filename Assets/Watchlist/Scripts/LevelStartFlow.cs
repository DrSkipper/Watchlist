using UnityEngine;

public class LevelStartFlow : VoBehavior
{
    public GameObject LevelIntroPanel;
    public float IntroDelay = 1.0f;
    public float LevelIntroScreenLength = 3.0f;
    public bool WaitForGameplayBeginEvent = false;

    void Start()
    {
        if (this.WaitForGameplayBeginEvent)
            GlobalEvents.Notifier.Listen(BeginGameplayEvent.NAME, this, begin);
        else
            begin(null);
    }

    public void LevelStartIn()
    {
        if (this.LevelIntroPanel != null)
            this.LevelIntroPanel.GetComponent<Animator>().SetTrigger("LevelStartIn");
        this.GetComponent<TimedCallbacks>().AddCallback(this, this.LevelStartOut, this.LevelIntroScreenLength);
    }

    public void LevelStartOut()
    {
        if (this.LevelIntroPanel != null)
            this.LevelIntroPanel.GetComponent<Animator>().SetTrigger("LevelStartOut");
    }

    private void begin(LocalEventNotifier.Event e)
    {
        this.GetComponent<TimedCallbacks>().AddCallback(this, this.LevelStartIn, this.IntroDelay);
    }
}
