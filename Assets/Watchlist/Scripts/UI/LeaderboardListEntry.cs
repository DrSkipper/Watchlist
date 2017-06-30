using UnityEngine;
using UnityEngine.UI;

public class LeaderboardListEntry : MonoBehaviour
{
    public Text RankText;
    public Text PlayerNameText;
    public Text ScoreText;
    public GameObject UserIcon;
    public GameObject FriendIcon;
    private const string UNRANKED = "UNRANKED";

    public void ConfigureForEntry(LeaderboardManager.LeaderboardEntry entry)
    {
        this.RankText.text = entry.Unranked ? UNRANKED : ("" + entry.Rank);
        this.PlayerNameText.text = entry.PlayerName;
        this.ScoreText.text = "" + entry.Score;
        this.UserIcon.SetActive(entry.IsUser);
        this.FriendIcon.SetActive(entry.IsFriend);
    }
}
