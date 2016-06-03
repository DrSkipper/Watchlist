using UnityEngine;
using UnityEngine.UI;

public class WatchlistUnlockDialog : MonoBehaviour
{
    public UIDialog Dialog;
    public GameObject RootDialogObject;
    public Image BossImage;
    public Text BossName;
    public Texture2D BossImages;
    public string BossAvatarPrefix;
    public string DismissalString;

    void Awake()
    {
        GlobalEvents.Notifier.Listen(MenuElementSelectedEvent.NAME, this, menuItemSelected);
    }

    public void ShowForBoss(BossType boss)
    {
        this.RootDialogObject.SetActive(true);
        UIDialogManager.Instance.ActiveDialog = this.Dialog;
        this.BossName.text = "THE " + boss.Name.ToUpper();
        this.BossImage.sprite = this.BossImages.GetSprites()[this.BossAvatarPrefix + boss.Name.ToLower()];
    }

    public void Hide()
    {
        if (UIDialogManager.Instance.ActiveDialog == this.Dialog)
        {
            UIDialogManager.Instance.MoveActiveToBack();
        }

        this.RootDialogObject.SetActive(false);
    }

    private void menuItemSelected(LocalEventNotifier.Event e)
    {
        if ((e as MenuElementSelectedEvent).Action == this.DismissalString)
            Hide();
    }
}
