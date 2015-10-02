using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : Actor2D
{
    public float AccelerationDuration = 0.5f;
    public float MaxSpeed = 1.0f;
    public bool DirectionalAcceleration = true; //TODO - Implement "false" approach for this
    public float ShotCooldown = 0.2f;
    public float ShotSpeed = 1.5f;
    public float ShotStartDistance = 1.0f;

    public GameObject BulletPrefab;

    void Start()
    {
        _acceleration = this.AccelerationDuration > 0 ? this.MaxSpeed / this.AccelerationDuration : this.MaxSpeed * 1000;
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
        //TODO - fcole - Weapon behaviors should be handled in own class(es)
        if (_shotCooldown <= 0.0f)
        {
            _shotCooldown = this.ShotCooldown;
            Vector2 aimAxis = GameplayInput.GetAimingAxis();

            if (aimAxis.x != 0 || aimAxis.y != 0)
            {
                // Create instance of bullet prefab and set velocity on it's BulletController component
                Vector3 position = this.transform.position + (new Vector3(aimAxis.x, aimAxis.y, 0) * this.ShotStartDistance);
                GameObject bullet = Instantiate(BulletPrefab, position, Quaternion.identity) as GameObject;
                bullet.GetComponent<BulletController>().Velocity = new Vector2(aimAxis.x * this.ShotSpeed, aimAxis.y * this.ShotSpeed);
            }
        }
        else
        {
            _shotCooldown -= Time.deltaTime;
        }
    }

    /**
     * Private
     */
    private float _acceleration;
    private float _shotCooldown;
    private bool _usingMovingSprite;
}
