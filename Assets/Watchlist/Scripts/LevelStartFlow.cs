using UnityEngine;
using UnityEngine.UI;

public class LevelStartFlow : VoBehavior
{
    public GameObject LevelIntroPanel;
    public GameObject LevelIntroImage;
    public float IntroDelay = 1.0f;
    public float LevelIntroScreenLength = 3.0f;

    void Start()
    {
        this.GetComponent<TimedCallbacks>().AddCallback(this, this.LevelStartIn, this.IntroDelay);
    }

    public void LevelStartIn()
    {
        this.LevelIntroPanel.GetComponent<Animator>().SetTrigger("LevelStartIn");
        this.GetComponent<TimedCallbacks>().AddCallback(this, this.LevelStartOut, this.LevelIntroScreenLength);
    }

    public void LevelStartOut()
    {
        this.LevelIntroPanel.GetComponent<Animator>().SetTrigger("LevelStartOut");
    }
}
