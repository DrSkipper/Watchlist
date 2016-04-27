using UnityEngine;
using UnityEngine.UI;

public class BriefingAvatarHandler : MonoBehaviour
{
    public MissionBriefingFlow BriefingFlow;
    public Texture2D SpriteSheet;
    public string AvatarKeyPrefix = "briefing_avatars_";
    
    void Awake()
    {
        _image = this.GetComponent<Image>();
        this.BriefingFlow.PageFlipCallback = this.PageFlipped;
    }

    public void PageFlipped(string avatarKey)
    {
        _image.sprite = this.SpriteSheet.GetSprites()[this.AvatarKeyPrefix + avatarKey.ToLower()];
    }

    /**
     * Private
     */
    private Image _image;
}
