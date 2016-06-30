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
    public bool NoFire = false;
    public bool NoMove = false;
    public float ControllerAimerSpeed = 200.0f;
    public ReticlePositioner Reticle;
    public Texture2D SpriteAtlas;

    public Vector2 AimAxis { get { return _aimAxis; } }

    public delegate void SlotChangeDelegate(ProgressData.SlotWrapper[] newSlots);

    void Start()
    {
        _acceleration = this.AccelerationDuration > 0 ? this.MaxSpeed / this.AccelerationDuration : this.MaxSpeed * 1000;
        _weapon = this.GetComponent<Weapon>();
        _weapon.ShotFiredCallback = shotFired;
        _aimAxis = Vector2.zero;

        this.Reticle.PlayerIndex = this.PlayerIndex;
        _damagable = this.GetComponent<Damagable>();
        _damagable.OnDeathCallbacks.Add(died);
        this.localNotifier.Listen(CollisionEvent.NAME, this, this.OnCollide);
        updateSlots();

        if (this.PlayerIndex != 0)
        {
            AllegianceColorizer colorizer = this.GetComponent<AllegianceColorizer>();
            AllegianceInfo info = colorizer.AllegianceInfo;
            info.MemberId = this.PlayerIndex;
            colorizer.UpdateVisual(info);
            _weapon.AllegianceInfo = info;

            this.spriteRenderer.sprite = this.SpriteAtlas.GetSprites()["player_body_" + this.PlayerIndex];
        }

        if (_initialHealth > 0)
            _damagable.DirectSetHealth(_initialHealth);
    }

    public void SetUsingController()
    {
        _usingController = true;
    }

    public void SetInteractionDelay(float delay)
    {
        _initialNoFire = this.NoFire;
        _initialNoMove = this.NoMove;
        this.NoFire = true;
        this.NoMove = true;
        this.GetComponent<TimedCallbacks>().AddCallback(this, beginInteraction, delay);
    }

    private void beginInteraction()
    {
        this.NoFire = _initialNoFire;
        this.NoMove = _initialNoMove;
    }

    public void SetInitialHealth(int health)
    {
        _initialHealth = health;
    }

    public override void Update()
    {
        if (PauseController.IsPaused())
            return;

        if (!this.NoMove)
        {
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
        }
        else
        {
            this.Velocity = Vector2.zero;
        }

        base.Update();

        // Shooting
        if (!this.NoFire && _weapon != null)
        {
            Vector2 rawAimAxis = GameplayInput.GetAimingAxis(this.PlayerIndex, this.transform.position, !_usingController);

            if (_usingController)
            {
                _aimAxis = Vector2.MoveTowards(_aimAxis, rawAimAxis, this.ControllerAimerSpeed * Time.deltaTime);
            }
            else
            {
                _aimAxis = rawAimAxis;
            }

            if (_aimAxis.x != 0 || _aimAxis.y != 0)
            {
                if (GameplayInput.GetFireButton(this.PlayerIndex))
                    _weapon.Fire(_aimAxis.normalized, this.ShotStartDistance);
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
                {
                    if (pickup.PickupContents.Type == WeaponPickup.PickupType.WeaponSlot)
                    {
                        WeaponData.Slot slotType = (WeaponData.Slot)pickup.PickupContents.Parameter;
                        ProgressData.SmartSlot[] smartSlots = ProgressData.SmartSlotsFromWrappers(this.Slots.ToArray());
                        bool ok = true;
                        for (int i = 0; i < smartSlots.Length; ++i)
                        {
                            if (smartSlots[i].SlotType == slotType)
                            {
                                ok = smartSlots[i].Level < WeaponData.GetMaxSlotsByType()[slotType] || smartSlots[i].Ammo < WeaponData.GetSlotDurationsByType()[slotType];
                                break;
                            }
                        }
                        if (ok)
                        {
                            pickupWeaponSlot(slotType);
                            Destroy(hit);
                        }
                    }
                    else
                    {
                        if (_damagable.Health < ProgressData.MAX_HEALTH)
                        {
                            _damagable.Heal(pickup.PickupContents.Parameter);
                            Destroy(hit);
                        }
                    }
                }
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
    //private int _selectedSlot;
    private bool _initialNoFire;
    private bool _initialNoMove;
    private float _acceleration;
    private int _initialHealth;
    private bool _usingController;
    private Vector2 _aimAxis;
    private Weapon _weapon;
    private Damagable _damagable;
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
                typesFound[(int)slot.SlotType - 1] = true;
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
