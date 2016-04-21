using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerController : Actor2D
{
    public int PlayerIndex;
    public float AccelerationDuration = 0.5f;
    public float MaxSpeed = 1.0f;
    public bool DirectionalAcceleration = true; //TODO - Implement "false" approach for this
    public float ShotStartDistance = 1.0f;
    public List<ProgressData.SlotWrapper> Slots = new List<ProgressData.SlotWrapper>();
    public int WeaponTypeId = 1; // Exposed for debugging
    public LayerMask PickupLayer;
    public bool UseDebugWeapon = false; // If enabled, ignores Equip Slots and uses whatever properties have been set on the Weapon's inspector
    public ReticlePositioner Reticle;

    public delegate void SlotChangeDelegate(ProgressData.SlotWrapper[] newSlots);

    void Start()
    {
        _acceleration = this.AccelerationDuration > 0 ? this.MaxSpeed / this.AccelerationDuration : this.MaxSpeed * 1000;
        _weapon = this.GetComponent<Weapon>();
        _weapon.ShotFiredCallback = shotFired;

        this.Reticle.PlayerIndex = this.PlayerIndex;
        this.GetComponent<Damagable>().OnDeathCallbacks.Add(died);
        this.localNotifier.Listen(CollisionEvent.NAME, this, this.OnCollide);
        updateSlots();

        if (this.PlayerIndex != 0)
        {
            AllegianceColorizer colorizer = this.GetComponent<AllegianceColorizer>();
            AllegianceInfo info = colorizer.AllegianceInfo;
            info.MemberId = this.PlayerIndex;
            colorizer.UpdateVisual(info);
            _weapon.AllegianceInfo = info;
        }
    }

    public override void Update()
    {
        if (PauseController.IsPaused())
            return;

        Vector2 movementAxis = GameplayInput.GetMovementAxis(this.PlayerIndex);

        float targetX = movementAxis.x * this.MaxSpeed;
        float targetY = movementAxis.y * this.MaxSpeed;

        float changeX = targetX - this.Velocity.x;
        float changeY = targetY - this.Velocity.y;

        if (changeX != 0 || changeY != 0)
        {
            float changeTotal = Mathf.Sqrt(Mathf.Pow(changeX, 2) + Mathf.Pow(changeY, 2));

            if (changeX != 0)
            {
                float ax = Mathf.Abs(_acceleration * changeX / changeTotal);
                this.Velocity.x = Mathf.Lerp(this.Velocity.x, targetX, ax * Time.deltaTime / Math.Abs(changeX));
            }

            if (changeY != 0)
            {
                float ay = Mathf.Abs(_acceleration * changeY / changeTotal);
                this.Velocity.y = Mathf.Lerp(this.Velocity.y, targetY, ay * Time.deltaTime / Math.Abs(changeY));
            }
        }

        base.Update();

        // Shooting
        if (_weapon != null)
        {
            Vector2 aimAxis = GameplayInput.GetAimingAxis(this.PlayerIndex, this.transform.position);
            if (aimAxis.x != 0 || aimAxis.y != 0)
            {
                if (GameplayInput.GetFireButton(this.PlayerIndex))
                    _weapon.Fire(aimAxis, this.ShotStartDistance);
            }
        }
    }

    public void OnCollide(LocalEventNotifier.Event localEvent)
    {
        foreach (GameObject hit in ((CollisionEvent)localEvent).Hits)
        {
            if (((1 << hit.layer) & this.PickupLayer) != 0)
            {
                WeaponPickup pickup = hit.GetComponent<WeaponPickup>();
                if (pickup != null)
                    pickupWeaponSlot(pickup.SlotType);

                Destroy(hit);
            }
        }
    }

    public void AddSlotChangeCallback(SlotChangeDelegate callback)
    {
        _slotChangeDelegates.Add(callback);
    }

    public WeaponData.Slot[] GetRawSlots()
    {
        WeaponData.Slot[] slots = new WeaponData.Slot[this.Slots.Count];
        for (int i = 0; i < this.Slots.Count; ++i)
            slots[i] = this.Slots[i].SlotType;
        return slots;
    }

    /**
     * Private
     */
    private int _selectedSlot;
    private float _acceleration;
    private Weapon _weapon;
    private List<SlotChangeDelegate> _slotChangeDelegates = new List<SlotChangeDelegate>();

    private void pickupWeaponSlot(WeaponData.Slot slotType)
    {
        int count = 0;

        foreach (ProgressData.SlotWrapper slot in this.Slots)
        {
            if (slot.SlotType == slotType)
                ++count;
        }

        if (count < WeaponData.GetMaxSlotsByType()[slotType])
        {
            this.Slots.Add(new ProgressData.SlotWrapper(slotType));
        }
        else
        {
            int shots = WeaponData.GetSlotDurationsByType()[slotType];
            int shotsToAdd = shots;

            for (int i = this.Slots.Count - 1; i >= 0; --i)
            {
                ProgressData.SlotWrapper slot = this.Slots[i];
                if (slot.SlotType == slotType)
                {
                    slot.AmmoRemaining += shotsToAdd;
                    if (slot.AmmoRemaining > shots)
                    {
                        shotsToAdd = slot.AmmoRemaining - shots;
                        slot.AmmoRemaining = shots;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        /*bool found = false;
        for (int i = 0; i < this.Slots.Length; ++i)
        {
            if (this.Slots[i] == WeaponData.Slot.Empty)
            {
                found = true;
                this.Slots[i] = slotType;
                _selectedSlot = i + 1 >= this.Slots.Length ? 0 : i + 1;
                break;
            }
        }
        if (!found)
        {
            this.Slots[_selectedSlot] = slotType;
            _selectedSlot = _selectedSlot + 1 >= this.Slots.Length ? 0 : _selectedSlot + 1;
        }*/

        updateSlots();
    }

    private void updateSlots()
    {
        // Old style
        //this.WeaponTypeId = WeaponData.WeaponTypeIdFromSlots(this.Slots);
        //if (!UseDebugWeapon && StaticData.WeaponData.WeaponTypes.ContainsKey(this.WeaponTypeId))
        //    _weapon.WeaponType = StaticData.WeaponData.WeaponTypes[this.WeaponTypeId];

        // New Style
        WeaponData.Slot[] slotArray = this.GetRawSlots();

        if (!this.UseDebugWeapon)
            _weapon.WeaponType = WeaponData.NewWeaponTypeFromSlots(slotArray);

        // Notification
        foreach (SlotChangeDelegate callback in _slotChangeDelegates)
            callback(this.Slots.ToArray());
    }

    private void died(Damagable d)
    {
        GlobalEvents.Notifier.SendEvent(new PlayerDiedEvent(this.gameObject, this.PlayerIndex));
    }

    private void shotFired(bool ignoreExplosions)
    {
        // Update slots
        bool[] typesFound = { false, false, false, false };
        for (int i = 0; i < this.Slots.Count;)
        {
            ProgressData.SlotWrapper slot = this.Slots[i];
            if ((slot.SlotType != WeaponData.Slot.Bomb || !ignoreExplosions) && !typesFound[(int)slot.SlotType - 1])
            {
                slot.AmmoRemaining -= 1;
                if (slot.AmmoRemaining <= 0)
                {
                    slot.SlotType = WeaponData.Slot.Empty;
                    this.Slots.RemoveAt(i);
                    continue;
                }
            }
            ++i;
        }
        
        this.updateSlots();
    }
}
