using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreUpdater : MonoBehaviour
{
    public int PlayerIndex = 0;
    public Text ScoreText;

    void Awake()
    {
        _prefix = this.ScoreText.text;
        GlobalEvents.Notifier.Listen(PlayerPointsReceivedEvent.NAME, this, playerScoreUpdated);
    }

    void Start()
    {
        this.ScoreText.text = _prefix + ProgressData.GetPointsForPlayer(this.PlayerIndex);
    }

    /**
     * Private
     */
    private string _prefix;

    private void playerScoreUpdated(LocalEventNotifier.Event e)
    {
        PlayerPointsReceivedEvent pointsEvent = e as PlayerPointsReceivedEvent;

        if (pointsEvent.PlayerIndex == this.PlayerIndex)
        {
            ProgressData.ApplyPointsDeltaForPlayer(this.PlayerIndex, pointsEvent.PointsDelta);
            this.ScoreText.text = _prefix + ProgressData.GetPointsForPlayer(this.PlayerIndex);
        }
    }
}
