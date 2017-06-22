using UnityEngine;

public class PauseController : VoBehavior
{
    public static bool IsPaused()
    {
        return (_pauseController != null && _pauseController._paused) || Time.deltaTime == 0.0f;
    }

    public static void EnablePausing(bool enable)
    {
        _pauseController.AllowPausing = enable;
    }

    public GameObject PauseMenu;
    public bool PauseOnStart = false;
    public bool AllowPausing = true;

    void Awake()
    {
        _pauseController = this;
    }

    void Start()
    {
        if (this.PauseOnStart)
            pause();
        else
            unpause();
    }

    void Update()
    {
        if (this.AllowPausing)
        {
            if (MenuInput.Pause())
            {
                if (_paused)
                    unpause();
                else
                    pause();
            }
            else if (MenuInput.Cancel())
            {
                if (_paused)
                    unpause();
            }
        }
    }

    /**
     * Private
     */
    private static PauseController _pauseController;
    private bool _paused;

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
