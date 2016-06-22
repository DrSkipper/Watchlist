using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MenuElement))]
public class UpgradePurchaseHandler : MonoBehaviour
{
    public int PlayerIndex = 0;
    public WeaponData.Slot SlotType = WeaponData.Slot.Bomb;
    public Text CostText;
    public Text LevelText;
    public string CostString = "[COST:<cost>]";
    public string LevelString = "Lv.<level>";
    public string MaxLevelString = "MAX";
    public PlayerShopHandler ShopHandler;
    public Image SelectionImage;
    public Color NotEnoughColor;
    

    void Awake()
    {
        _menuElement = this.GetComponent<MenuElement>();
        GlobalEvents.Notifier.Listen(MenuElementSelectedEvent.NAME, this, menuSelection);
    }

    void Start()
    {
        _normalColor = this.SelectionImage.color;
        updateDisplay();
    }

    void OnDestroy()
    {
        if (GlobalEvents.Notifier != null)
            GlobalEvents.Notifier.RemoveAllListenersForOwner(this);
    }

    /**
     * Private
     */
    private MenuElement _menuElement;
    private int _cost;
    private Color _normalColor;

    private void menuSelection(LocalEventNotifier.Event e)
    {
        MenuElementSelectedEvent menuSelectionEvent = e as MenuElementSelectedEvent;
        if (menuSelectionEvent.Element == _menuElement)
        {
            if (ProgressData.GetPointsForPlayer(this.PlayerIndex) >= _cost)
            {
                ProgressData.SmartSlot[] smartSlots = ProgressData.GetSmartSlots(this.PlayerIndex);
                bool ok = true;
                for (int i = 0; i < smartSlots.Length; ++i)
                {
                    if (smartSlots[i].SlotType == this.SlotType)
                    {
                        ok = smartSlots[i].Level < WeaponData.GetMaxSlotsByType()[this.SlotType] || smartSlots[i].Ammo < WeaponData.GetSlotDurationsByType()[this.SlotType];
                        break;
                    }
                }
                if (ok)
                {
                    this.SelectionImage.color = _normalColor;
                    ProgressData.ApplyPointsDeltaForPlayer(this.PlayerIndex, -_cost);
                    ProgressData.PickupSlot(this.PlayerIndex, this.SlotType);
                    GlobalEvents.Notifier.SendEvent(new PlayerPointsReceivedEvent(this.PlayerIndex, -_cost));
                    updateDisplay();
                }
                else
                {
                    this.SelectionImage.color = this.NotEnoughColor;
                }
            }
            else
            {
                this.SelectionImage.color = this.NotEnoughColor;
            }
        }
    }

    private void updateDisplay()
    {
        ProgressData.SmartSlot slot = this.ShopHandler.GetSmartSlots()[(int)this.SlotType];
        _cost = WeaponData.GetUpgradeTotalCost(this.SlotType, slot.Level);
        this.CostText.text = this.CostString.Replace("<cost>", "" + _cost);
        if (slot.Level < WeaponData.GetMaxSlotsByType()[this.SlotType])
        {
            this.LevelText.text = this.LevelString.Replace("<level>", "" + slot.Level);
        }
        else
        {
            this.LevelText.text = this.MaxLevelString;
        }
    }
}
