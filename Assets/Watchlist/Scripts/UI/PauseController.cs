using UnityEngine;

public class PauseController : VoBehavior
{
    public static bool IsPaused()
    {
        return _paused || Time.deltaTime == 0.0f;
    }

    public GameObject PauseMenu;
    public bool PauseOnStart = false;
    public bool AllowPausing = true;

    void Start()
    {
        if (this.PauseOnStart)
            pause();
        else
            unpause();
    }

    void Update()
    {
        if (this.AllowPausing && MenuInput.Pause())
        {
            if (_paused)
                unpause();
            else
                pause();
        }
    }

    /**
     * Private
     */
    private static bool _paused;

    private void pause()
    {
        _paused = true;
        Time.timeScale = 0.0f;
        if (this.PauseMenu != null)
            this.PauseMenu.SetActive(true);
    }

    private void unpause()
    {
        _paused = false;
        Time.timeScale = 1.0f;
        if (this.PauseMenu != null)
            this.PauseMenu.SetActive(false);
    }
}
