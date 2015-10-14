using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : Actor2D
{
    public float AccelerationDuration = 0.5f;
    public float MaxSpeed = 1.0f;
    public bool DirectionalAcceleration = true; //TODO - Implement "false" approach for this
    public float ShotStartDistance = 1.0f;
    public WeaponData.Slot[] Slots = { WeaponData.Slot.Empty, WeaponData.Slot.Empty, WeaponData.Slot.Empty, WeaponData.Slot.Empty };
    public int WeaponTypeId = 1; // Exposed for debugging
    public bool UseDebugWeapon = false; // If enabled, ignores Equip Slots and uses whatever properties have been set on the Weapon's inspector

    void Start()
    {
        _acceleration = this.AccelerationDuration > 0 ? this.MaxSpeed / this.AccelerationDuration : this.MaxSpeed * 1000;
        _weapon = this.GetComponent<Weapon>();

        this.WeaponTypeId = WeaponData.WeaponTypeIdFromSlots(this.Slots);
        if (!UseDebugWeapon && StaticData.WeaponData.WeaponTypes.ContainsKey(this.WeaponTypeId))
            _weapon.WeaponType = StaticData.WeaponData.WeaponTypes[this.WeaponTypeId];
    }

    public override void Update()
    {
        Vector2 movementAxis = GameplayInput.GetMovementAxis();

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
            Vector2 aimAxis = GameplayInput.GetAimingAxis();
            if (aimAxis.x != 0 || aimAxis.y != 0)
                _weapon.Fire(aimAxis, this.ShotStartDistance);
        }
    }

    /**
     * Private
     */
    private float _acceleration;
    private Weapon _weapon;
}
