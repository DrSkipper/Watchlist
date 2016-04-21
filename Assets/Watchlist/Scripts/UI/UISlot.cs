using UnityEngine;
using UnityEngine.UI;

public class UISlot : VoBehavior
{
    public GameObject PlayerObject;
    public Image SlotObject;
    public Image SlotContentsObject;

    public Text TierText;
    public RectTransform AmmoVisual;
    
    public int SlotId;
    public bool UseWeaponLevelParadigm = false;
    public int AmmoVisualHeight = 21;

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
        bool[] weaponTypesFound = { false, false, false, false };
        int[] ammoRemaining = { 0, 0, 0, 0 };
        int[] weaponLevel = { 0, 0, 0, 0 };
        int numTypesFound = 0;
        WeaponData.Slot chosenSlotType = WeaponData.Slot.Empty;
        int chosenWeaponIndex = -1;

        for (int i = 0; i < slots.Length; ++i)
        {
            if (slots[i].SlotType == WeaponData.Slot.Empty)
                continue;
            int weaponIndex = (int)slots[i].SlotType - 1;
            ++weaponLevel[weaponIndex];
            if (chosenSlotType == WeaponData.Slot.Empty && !weaponTypesFound[weaponIndex])
            {
                ++numTypesFound;
                weaponTypesFound[weaponIndex] = true;
                ammoRemaining[weaponIndex] = slots[i].AmmoRemaining;

                if (numTypesFound > this.SlotId)
                {
                    chosenSlotType = slots[i].SlotType;
                    chosenWeaponIndex = weaponIndex;
                }
            }
        }

        int ammo = chosenSlotType != WeaponData.Slot.Empty ? ammoRemaining[chosenWeaponIndex] : 0;
        int level = chosenSlotType != WeaponData.Slot.Empty ? weaponLevel[chosenWeaponIndex] : 0;
        updateSlotHelper(chosenSlotType, ammo, level);
    }

    private void updateSlotHelper(WeaponData.Slot slotType, int ammoRemaining, int weaponLevel)
    {
        configureSprite(slotType);

        if (slotType != WeaponData.Slot.Empty)
        {
            SlotObject.sprite = this.UnlockedSlotSprite;

            if (this.AmmoVisual != null)
            {
                float percentRemaining = (float)ammoRemaining / (float)WeaponData.GetSlotDurationsByType()[slotType];
                int endHeight = Mathf.RoundToInt(percentRemaining * (float)this.AmmoVisualHeight);
                this.AmmoVisual.sizeDelta = new Vector2(this.AmmoVisual.sizeDelta.x, endHeight);
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
            if (this.AmmoVisual != null)
                this.AmmoVisual.sizeDelta = new Vector2(this.AmmoVisual.sizeDelta.x, 0);
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
