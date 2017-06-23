using UnityEngine;
using UnityEngine.UI;

public class HighScoreDisplay : MonoBehaviour
{
    public GameObject ParentScoreObject;
    public Text SinglePlayerScoreText;
    public Text CoopScoreText;

    void Start()
    {
        int singlePlayerScore = PersistentData.GetSinglePlayerHighScore();
        int coopScore = PersistentData.GetCoopHighScore();

        if (singlePlayerScore <= 0 && coopScore <= 0)
        {
            this.ParentScoreObject.SetActive(false);
        }
        else
        {
            if (singlePlayerScore > 0)
                this.SinglePlayerScoreText.text += singlePlayerScore;
            else
                this.SinglePlayerScoreText.enabled = false;

            if (coopScore > 0)
                this.CoopScoreText.text += coopScore;
            else
                this.CoopScoreText.enabled = false;
        }
    }
}
