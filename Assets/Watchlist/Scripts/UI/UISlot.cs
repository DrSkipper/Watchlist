using UnityEngine;
using UnityEngine.UI;

public class UISlot : VoBehavior
{
    public GameObject PlayerObject;
    public GameObject SlotObject;
    public GameObject SlotContentsObject;

    public int SlotId;

    public Sprite UnlockedSlotSprite;
    public Sprite LockedSlotSprite;

    public Sprite EmptySlotSprite;
    public Sprite BounceSlotSprite;
    public Sprite SpreadshotSlotSprite;
    public Sprite LaserSlotSprite;
    public Sprite BombSlotSprite;

    void Start()
    {
        PlayerController player = PlayerObject.GetComponent<PlayerController>();
        this.UpdateSlots(player.Slots);
        player.AddSlotChangeCallback(this.UpdateSlots);
    }

    public void UpdateSlots(WeaponData.Slot[] slots)
    {
        if (this.SlotId >= slots.Length)
        {
            SlotObject.GetComponent<Image>().sprite = this.LockedSlotSprite;
            SlotContentsObject.GetComponent<Image>().sprite = this.EmptySlotSprite;
        }
        else
        {
            SlotObject.GetComponent<Image>().sprite = this.UnlockedSlotSprite;

            switch (slots[this.SlotId])
            {
                default:
                case WeaponData.Slot.Empty:
                    SlotContentsObject.GetComponent<Image>().sprite = this.EmptySlotSprite;
                    break;
                case WeaponData.Slot.Bounce:
                    SlotContentsObject.GetComponent<Image>().sprite = this.BounceSlotSprite;
                    break;
                case WeaponData.Slot.Spreadshot:
                    SlotContentsObject.GetComponent<Image>().sprite = this.SpreadshotSlotSprite;
                    break;
                case WeaponData.Slot.Laser:
                    SlotContentsObject.GetComponent<Image>().sprite = this.LaserSlotSprite;
                    break;
                case WeaponData.Slot.Bomb:
                    SlotContentsObject.GetComponent<Image>().sprite = this.BombSlotSprite;
                    break;
            }
        }
    }
}
