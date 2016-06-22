using UnityEngine;
using UnityEngine.UI;

public class UISlot : VoBehavior
{
    public GameObject PlayerObject;
    public Image SlotObject;
    public Image SlotContentsObject;

    public Text TierText;
    public GameObject BaseAmmo;
    public UIBar AmmoBar;
    
    public int SlotId;
    public bool UseWeaponLevelParadigm = false;

    public Sprite UnlockedSlotSprite;
    public Sprite LockedSlotSprite;

    public Sprite EmptySlotSprite;
    public Sprite BounceSlotSprite;
    public Sprite SpreadshotSlotSprite;
    public Sprite LaserSlotSprite;
    public Sprite BombSlotSprite;

    void Start()
    {
        if (this.UseWeaponLevelParadigm)
            updateSlotParadigm = updateSlotLevelParadigm;
        else
            updateSlotParadigm = updateSlotIndividualParadigm;

        if (this.PlayerObject != null)
            this.SetPlayer(this.PlayerObject);
    }

    public void SetPlayer(GameObject playerObject)
    {
        this.PlayerObject = playerObject;
        PlayerController player = PlayerObject.GetComponent<PlayerController>();
        this.UpdateSlots(player.Slots.ToArray());
        player.AddSlotChangeCallback(this.UpdateSlots);
    }

    public void UpdateSlots(ProgressData.SlotWrapper[] slots)
    {
        updateSlotParadigm(slots);
    }

    /**
     * Private
     */
    private delegate void UpdateSlotParadigm(ProgressData.SlotWrapper[] slots);
    private UpdateSlotParadigm updateSlotParadigm;

    private void updateSlotIndividualParadigm(ProgressData.SlotWrapper[] slots)
    {
        if (this.SlotId >= slots.Length)
        {
            SlotObject.sprite = this.LockedSlotSprite;
            SlotContentsObject.sprite = this.EmptySlotSprite;
            return;
        }

        SlotObject.sprite = this.UnlockedSlotSprite;
        configureSprite(slots[this.SlotId].SlotType);
    }

    private void updateSlotLevelParadigm(ProgressData.SlotWrapper[] slots)
    {
        ProgressData.SmartSlot smartSlot = ProgressData.GetSmartSlot(slots, this.SlotId);
        updateSlotHelper(smartSlot.SlotType, smartSlot.Ammo, smartSlot.Level);
    }

    private void updateSlotHelper(WeaponData.Slot slotType, int ammoRemaining, int weaponLevel)
    {
        configureSprite(slotType);

        if (slotType != WeaponData.Slot.Empty)
        {
            SlotObject.sprite = this.UnlockedSlotSprite;

            if (this.BaseAmmo != null && !this.BaseAmmo.activeInHierarchy)
                this.BaseAmmo.SetActive(true);
            if (this.AmmoBar != null)
            {
                this.AmmoBar.UpdateLength(ammoRemaining, WeaponData.GetSlotDurationsByType()[slotType]);
            }

            if (this.TierText != null)
            {
                if (weaponLevel == WeaponData.GetMaxSlotsByType()[slotType])
                    this.TierText.text = "MAX";
                else
                    this.TierText.text = "Lv." + weaponLevel;
            }
        }
        else
        {
            if (this.AmmoBar != null)
                this.AmmoBar.EmptyCompletely();
            if (this.BaseAmmo != null)
                this.BaseAmmo.SetActive(false);
            if (this.TierText != null)
                this.TierText.text = "";
            SlotObject.sprite = this.LockedSlotSprite;
        }
    }

    private void configureSprite(WeaponData.Slot slot)
    {
        switch (slot)
        {
            default:
            case WeaponData.Slot.Empty:
                SlotContentsObject.sprite = this.EmptySlotSprite;
                break;
            case WeaponData.Slot.Bounce:
                SlotContentsObject.sprite = this.BounceSlotSprite;
                break;
            case WeaponData.Slot.Spreadshot:
                SlotContentsObject.sprite = this.SpreadshotSlotSprite;
                break;
            case WeaponData.Slot.Laser:
                SlotContentsObject.sprite = this.LaserSlotSprite;
                break;
            case WeaponData.Slot.Bomb:
                SlotContentsObject.sprite = this.BombSlotSprite;
                break;
        }
    }
}
