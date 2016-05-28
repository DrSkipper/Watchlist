using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCondition : VoBehavior
{
    public float Delay = 3.0f;
    public string Destination = "GameOver";

    void Start()
    {
        GlobalEvents.Notifier.Listen(PlayerSpawnedEvent.NAME, this, playerSpawned);
        GlobalEvents.Notifier.Listen(PlayerDiedEvent.NAME, this, playerDied);
    }

    public void GameOver()
    {
        this.GetComponent<TimedCallbacks>().AddCallback(this, this.GoToGameOver, this.Delay);
    }

    public void GoToGameOver()
    {
        SceneManager.LoadScene(this.Destination);
    }

    /**
     * Private
     */
    private int _playerCount;

    private void playerDied(LocalEventNotifier.Event e)
    {
        --_playerCount;

        if (_playerCount <= 0)
            this.GameOver();
    }

    private void playerSpawned(LocalEventNotifier.Event e)
    {
        ++_playerCount;
    }
}
