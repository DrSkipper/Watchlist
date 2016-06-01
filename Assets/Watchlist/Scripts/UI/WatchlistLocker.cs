using UnityEngine;
using UnityEngine.UI;

public class WatchlistLocker : MonoBehaviour
{
    public MenuElement MenuElement;
    public Image ImageToLock;
    public Sprite LockSprite;
    public Text TextToLock;
    public string LockText;
    public int BossId = 0;
    public bool ForceLock = false;

    void Awake()
    {
        if (this.ForceLock)
        {
            this.ImageToLock.sprite = this.LockSprite;
            this.TextToLock.text = this.LockText;
        }
    }
}
