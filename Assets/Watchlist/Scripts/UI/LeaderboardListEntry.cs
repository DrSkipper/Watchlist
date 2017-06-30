using UnityEngine;
using UnityEngine.UI;

public class LeaderboardListEntry : MonoBehaviour
{
    public Text RankText;
    public Text PlayerNameText;
    public Text ScoreText;
    public GameObject UserIcon;
    public GameObject FriendIcon;

    public void ConfigureForEntry(LeaderboardManager.LeaderboardEntry entry)
    {
        this.RankText.text = "" + entry.Rank;
        this.PlayerNameText.text = entry.PlayerName;
        this.ScoreText.text = "" + entry.Score;
        this.UserIcon.SetActive(entry.IsUser);
        this.FriendIcon.SetActive(entry.IsFriend);
    }
}
