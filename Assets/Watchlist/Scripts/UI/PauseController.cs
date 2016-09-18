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
            this.Pause();
        else
            this.Unpause();
    }

    void Update()
    {
        if (this.AllowPausing)
        {
            if (MenuInput.Pause())
            {
                if (_paused)
                    this.Unpause();
                else
                    this.Pause();
            }
            else if (MenuInput.Cancel())
            {
                if (_paused)
                    this.Unpause();
            }
        }
    }

    public void Pause()
    {
        _paused = true;
        Time.timeScale = 0.0f;
        if (this.PauseMenu != null)
            this.PauseMenu.SetActive(true);
    }

    public void Unpause()
    {
        _paused = false;
        Time.timeScale = 1.0f;
        if (this.PauseMenu != null)
            this.PauseMenu.SetActive(false);
    }

    /**
     * Private
     */
    private static PauseController _pauseController;
    private bool _paused;
}
