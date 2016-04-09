using UnityEngine;
using UnityEngine.UI;

public class UISlot : VoBehavior
{
    public GameObject PlayerObject;
    public GameObject SlotObject;
    public GameObject SlotContentsObject;
    
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
        this.UpdateSlots(player.GetSlots());
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
            updateSlotParadigm(slots);
        }
    }

    /**
     * Private
     */
    private delegate void UpdateSlotParadigm(WeaponData.Slot[] slots);
    private UpdateSlotParadigm updateSlotParadigm;

    private void updateSlotIndividualParadigm(WeaponData.Slot[] slots)
    {
        configureSprite(slots[this.SlotId]);
    }

    private void updateSlotLevelParadigm(WeaponData.Slot[] slots)
    {
        bool[] weaponTypesFound = { false, false, false, false };
        int numTypesFound = 0;

        for (int i = 0; i < slots.Length; ++i)
        {
            if (slots[i] == WeaponData.Slot.Empty)
                continue;
            int weaponIndex = (int)slots[i] - 1;
            if (!weaponTypesFound[weaponIndex])
            {
                ++numTypesFound;
                weaponTypesFound[weaponIndex] = true;

                if (numTypesFound > this.SlotId)
                {
                    configureSprite(slots[i]);
                    break;
                }
            }
        }
    }

    private void configureSprite(WeaponData.Slot slot)
    {
        switch (slot)
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
